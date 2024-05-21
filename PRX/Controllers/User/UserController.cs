using Microsoft.AspNetCore.Mvc;
using PRX.Dto.User;
using PRX.Models.User;
using PRX.Data;
using PRX.Utils;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Kavenegar;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace PRX.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Users")]
    public class UsersController : ControllerBase
    {
        private readonly PRXDbContext _context;
        private readonly Utils.Utils _utils; // Add Utils instance
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        //private readonly KavenegarApi kavenegar;
        //private readonly Random _random;

        public UsersController(PRXDbContext context, IConfiguration configuration, IMemoryCache cache /*, Random random*/)
        {
            _context = context;
            _utils = new Utils.Utils(); // Instantiate Utils
            _configuration = configuration;
            _cache = cache;
            // _random = random;
        }

        [HttpGet("PhoneExistance/{phoneNumber}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CheckPhoneNumberExistsAsync(string phoneNumber)
        {

            try 
            {
                var user = _context.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
                if (user != null)
                {
                    //Response.Headers.Append("message", "Phone number exists");
                    return Ok(new { message = ResponseMessages.PhoneExistanseTrue });
                }
                else
                {
                    // Call the SMS sending API to send the OTP
                    var isOtpSent = await SendOtp(phoneNumber);
                    if (!isOtpSent)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to send OTP." });
                    }
                    return Ok(new { message = ResponseMessages.PhoneExistanseFalse });
                }


            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }


        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserDto userDto)
        {
            // Validate the DTO if necessary
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Optional: Check if user already exists
                var existingUser = _context.Users.FirstOrDefault(u => u.PhoneNumber == userDto.PhoneNumber);
                if (existingUser != null)
                {
                    return BadRequest(new { message = ResponseMessages.UsersPhoneExists });
                }


                int referencedUserId;

                // Check if reference code is provided and valid
                if (string.IsNullOrWhiteSpace(userDto.ReferenceCode))
                {
                    referencedUserId = 0;
                }
                else
                {
                    // Check if the reference code is in the format "PRX-{UserId}"
                    //if (!Regex.IsMatch(userDto.ReferenceCode, @"^PRX-\d+$"))
                    //{
                    //    return BadRequest(new { message = ResponseMessages.UserRefernceCodeIsInvalid });
                    //}

                    // Check if the reference code is in the correct format
                    if (!Regex.IsMatch(userDto.ReferenceCode, @"^PRX-\d{1,}-\d{6}$"))
                    {
                        return BadRequest(new { message = ResponseMessages.UserRefernceCodeFormatIsInvalid });
                    }

                    // Extract the user ID from the reference code
                    referencedUserId = ExtractUserIdFromReferenceCode(userDto.ReferenceCode);

                    // Check if the extracted user ID is valid
                    if (referencedUserId <= 0)
                    {
                        return BadRequest(new { message = ResponseMessages.UserRefernceCodeFormatIsInvalid });
                    }
                }

                // Hash the password before saving it to the database
                var hashedPassword = _utils.HashPassword(userDto.Password);

                // Map DTO to domain model
                var user = new PRX.Models.User.User
                {
                    Password = hashedPassword,
                    PhoneNumber = userDto.PhoneNumber
                };

                // Add user to database
                _context.Users.Add(user);
                _context.SaveChanges();


                //// Call the SMS verification API to verify the OTP
                //var isOtpVerified = await VerifyOtp(userDto.PhoneNumber, userDto.otp);
                //if (!isOtpVerified)
                //{
                //    return BadRequest(new { message = "OTP verification failed." });
                //}


                // Generate reference code using the user's ID
                var referenceCode = GenerateReferenceCode(user.Id);

                // Update user with reference code
                user.ReferenceCode = referenceCode;
                _context.SaveChanges();

                // Add user reference to database if the random part of the reference code is correct
                if (IsValidRandomPart(referenceCode))
                {
                    var UserReference = new UserReference
                    {
                        UserId = user.Id,
                        ReferencedUser = referencedUserId
                    };

                    _context.UserReferences.Add(UserReference);
                    _context.SaveChanges();
                }
                else
                {
                    // Handle the case where the random part is not valid
                    return BadRequest(new { message = ResponseMessages.UserRefernceCodeIsInvalid });
                }

                var userReference = new UserReference
                {
                    UserId = user.Id,
                    ReferencedUser = referencedUserId
                };

                // Add user reference to database
                _context.UserReferences.Add(userReference);
                _context.SaveChanges();

                // Return the created user
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        [HttpPost("verify-otp")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VerifyOtpAsync([FromBody] OtpDto otpVerificationDto)
        {
            try
            {
                var isOtpVerified = await VerifyOtp(otpVerificationDto.PhoneNumber, otpVerificationDto.Otp);
                if (!isOtpVerified)
                {
                    return BadRequest(new { message = "OTP verification failed." });
                }


                return Ok(new { message = "OTP verified successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }



        private async Task<bool> SendOtp(string mobile)
        {
            try
            {
                var requestBody = new
                {
                    mobile,
                    user = 0,
                    template = "prx"
                };

                var jsonContent = JsonConvert.SerializeObject(requestBody);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Key", "9ac6bc9a-cca3-4779-8640-6836b6ab1daa");
                    var response = await httpClient.PostAsync("http://172.21.30.78:8000/api/v1/sms/send_code/", httpContent);
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                // Log or handle the error
                return false;
            }
        }

        private async Task<bool> VerifyOtp(string mobile, int code)
        {
            try
            {
                var requestBody = new
                {
                    mobile,
                    user = 0,
                    template = "prx",
                    code
                };

                var jsonContent = JsonConvert.SerializeObject(requestBody);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Key", "9ac6bc9a-cca3-4779-8640-6836b6ab1daa");
                    var response = await httpClient.PostAsync("http://172.21.30.78:8000/api/v1/sms/verify_code/", httpContent);
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                // Log or handle the error
                return false;
            }
        }




        //[HttpPost]
        //[AllowAnonymous]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public IActionResult CreateUser([FromBody] UserDto userDto)
        //{
        //    // Validate the DTO if necessary
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    // Optional: Check if user already exists
        //    var existingUser = _context.Users.FirstOrDefault(u => u.PhoneNumber == userDto.PhoneNumber);
        //    if (existingUser != null)
        //    {
        //        //Response.Headers.Add("message", ResponseMessages.UserExists);
        //        return BadRequest(new { message = ResponseMessages.UsersPhoneExists });
        //    }

        //    try 
        //    {

        //        // Hash the password before saving it to the database
        //        var hashedPassword = _utils.HashPassword(userDto.Password);

        //        //var userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        //        //var referenceCode = GenerateReferenceCode(userId);



        //        // Map DTO to domain model
        //        var user = new PRX.Models.User.User
        //        {
        //            Password = hashedPassword,
        //            PhoneNumber = userDto.PhoneNumber
        //        };

        //        // Add user to database
        //        _context.Users.Add(user);
        //        _context.SaveChanges();

        //        // Generate reference code using the user's ID
        //        var referenceCode = GenerateReferenceCode(user.Id);

        //        // Check if the reference code is valid and extract the user ID
        //        int referencedUserId = ExtractUserIdFromReferenceCode(userDto.ReferenceCode);

        //        // Update user with reference code
        //        user.ReferenceCode = referenceCode;
        //        _context.SaveChanges();

        //        var userReference = new UserReference
        //        {
        //            UserId = user.Id,
        //            ReferencedUser = referencedUserId
        //        };

        //        _context.UserReferences.Add(userReference);
        //        _context.SaveChanges();

        //        // Return the created user
        //        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);

        //    } 
        //    catch (Exception ex)          
        //    {

        //        return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
        //    }


        //}


        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Login([FromBody] UserDto userDto)
        {
            //var sender = "1000689696";
            //var receptor = "09393691800";
            //var message = ".وب سرویس پیام کوتاه کاوه نگار";
            //var api = new Kavenegar.KavenegarApi("306A654573316748434365654D6143304969562F652F706679696755434652574735385149706C685654413D");
            //api.Send(sender, receptor, message);

            //Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi("306A654573316748434365654D6143304969562F652F706679696755434652574735385149706C685654413D");
            //var result = api.Send("1000689696", "09393691800", "خدمات پیام کوتاه کاوه نگار");

            var user = _context.Users.FirstOrDefault(u => u.PhoneNumber == userDto.PhoneNumber);
            if (user == null)
            {
                //Response.Headers.Add("message", "User does not exist");
                return Unauthorized(new {message = ResponseMessages.UserNotExists});
            }


            if (!_utils.VerifyPassword(userDto.Password, user.Password))
            {
                return BadRequest(new {message = ResponseMessages.PasswordIncorrect});
            }

            try 
            {
                // Log user login
                LogUserLogin(user.Id);

                // Generate JWT token
                var token = GenerateJwtToken(user);

                return Ok(new { Authorization = token });
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }


        [HttpPost("logout")]
        [Authorize(Roles = "User")]
        public IActionResult Logout()
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (token == null)
                {
                    return BadRequest(new { message = "Token is required" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                {
                    return BadRequest(new { message = "Invalid token" });
                }

                // Calculate token expiry
                var expiry = jwtToken.ValidTo;

                // Blacklist the token
                _cache.Set(token, "blacklisted", new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = expiry
                });

                return Ok(new { message = "Logged out successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal Server Error", detail = ex.Message });
            }
        }

        private void LogUserLogin(int userId)
        {

            var userLoginLog = new UserLoginLog
            {
                UserId = userId,
                LoginTime = DateTime.Now
            };

            _context.UserLoginLog.Add(userLoginLog);
            _context.SaveChanges();
        }


        // by ID Endpoints

        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserById(int id)
        {
            try
            {

                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }


                // Retrieve the user ID from the token
                //var tokenUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Ensure that the user is accessing their own data
                if (id != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized }); 
                }

                var user = _context.Users.FirstOrDefault(u => u.Id == id && !u.IsDeleted);
                if (user == null)
                {
                    return NotFound(new {message = ResponseMessages.UserNotFound});
                }

                var userDto = new UserDto
                {
                    Id = user.Id,
                    PhoneNumber = user.PhoneNumber,
                    ReferenceCode = user.ReferenceCode
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        private IActionResult Forbiden(object value)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateUser(int id, [FromBody] UserDto userDto)
        {
            try
            {


                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId});
                }


                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Ensure that the user is updating their own data
                if (id != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });  // Or return 403 Forbidden
                }

                // Find the user to update
                var user = _context.Users.FirstOrDefault(u => u.Id == id && !u.IsDeleted);
                if (user == null)
                {
                    return NotFound(new { message = ResponseMessages.UserNotFound });
                }

                // Update the user properties
                user.PhoneNumber = userDto.PhoneNumber;
                user.ReferenceCode = userDto.ReferenceCode;

                // If password is provided in the DTO, hash and update the password
                if (!string.IsNullOrEmpty(userDto.Password))
                {
                    var hashedPassword = _utils.HashPassword(userDto.Password);
                    user.Password = hashedPassword;
                }

                // Save changes to database
                _context.SaveChanges();

                // Return 204 No Content
                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteUser(int id)
        {
            try
            {

                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }


                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Ensure that the user is updating their own data
                if (id != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });  // Or return 403 Forbidden
                }

                // Find the user to update
                var user = _context.Users.FirstOrDefault(u => u.Id == id && !u.IsDeleted);
                if (user == null)
                {
                    return NotFound(new { message = ResponseMessages.UserNotFound });
                }

                user.IsDeleted = true;
                _context.SaveChanges();

                //_context.Users.Remove(user);
                //_context.SaveChanges();

                return Ok(new {message = ResponseMessages.OK});
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }


        // Adimn

        [HttpGet("Admin")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllUsers()
        {
            try
            {

                var users = _context.Users.ToList();
                var userDtos = users.Select(user => new UserDto
                {
                    Id = user.Id,
                    PhoneNumber = user.PhoneNumber,
                    ReferenceCode = user.ReferenceCode,
                    IsDeleted = user.IsDeleted
                }).ToList();

                return Ok(userDtos);

            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        [HttpGet("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserByIdAdmin(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var user = _context.Users.FirstOrDefault(u => u.Id == id && !u.IsDeleted);
                if (user == null)
                {
                    return NotFound(new { message = ResponseMessages.UserNotFound });
                }

                var userDto = new UserDto
                {
                    Id = user.Id,
                    PhoneNumber = user.PhoneNumber,
                    ReferenceCode = user.ReferenceCode,
                    IsDeleted = user.IsDeleted
                };

                return Ok(userDto);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        [HttpPut("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateUserAdmin(int id, [FromBody] UserDto userDto)
        {

            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var user = _context.Users.FirstOrDefault(u => u.Id == id && !u.IsDeleted);
                if (user == null)
                {
                    return NotFound(new { message = ResponseMessages.UserNotFound });
                }

                user.PhoneNumber = userDto.PhoneNumber;
                user.ReferenceCode = userDto.ReferenceCode;

                _context.SaveChanges();

                return Ok(new {message = ResponseMessages.OK});
            }
             catch(Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpDelete("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteUserAdmin(int id)
        {

            try 
            {

                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var user = _context.Users.FirstOrDefault(u => u.Id == id && !u.IsDeleted);
                if (user == null)
                {
                    return NotFound(new { message = ResponseMessages.UserNotFound });
                }

                user.IsDeleted = true;
                _context.SaveChanges();

                return Ok(new {message = ResponseMessages.OK});
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }


        [HttpDelete("Admin/Clear")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ClearUser()
        {
            try
            {
                _context.Users.RemoveRange(_context.Users);
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }



        }

        private string GenerateJwtToken(PRX.Models.User.User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            if (securityKey.KeySize < 256)
            {
                throw new ArgumentException("JWT security key size must be at least 256 bits (32 bytes).");
            }

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Define custom claim types
            const string IdClaimType = "id";
            const string PhoneClaimType = "phone_number";
            const string RoleClaimType = "role";

            // Create claims for user ID and phone number using custom claim types
            var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(IdClaimType, user.Id.ToString()),
                new System.Security.Claims.Claim(PhoneClaimType, user.PhoneNumber),
                new System.Security.Claims.Claim(RoleClaimType, user.Role),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(24), // Token expiration time
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //private string GenerateReferenceCode(int userId)
        //{
        //    // Generate a random number between 1000 and 9999
        //    //int randomNumber = _random.Next(1000, 10000);

        //    // Concatenate "PRX", UserId, and the random number
        //    string referenceCode = $"PRX-{userId}";

        //    return referenceCode;
        //}

        //private int ExtractUserIdFromReferenceCode(string referenceCode)
        //{
        //    // Check if the reference code starts with the expected format
        //    if (referenceCode.StartsWith("PRX-"))
        //    {
        //        // Extract the substring after "PRX-"
        //        string userIdString = referenceCode.Substring(4);

        //        // Try to parse the substring as an integer
        //        if (int.TryParse(userIdString, out int userId))
        //        {
        //            return userId;
        //        }
        //    }

        //    // If the reference code format is invalid, return 0 or throw an exception
        //    throw new ArgumentException("Invalid reference code format.");
        //}

        private string GenerateReferenceCode(int userId)
        {
            // Generate a random string of numbers
            var random = new Random();
            var randomNumbers = random.Next(100000, 999999); // Generates a random 6-digit number

            // Combine the random numbers with the user ID
            var referenceCode = $"PRX-{userId}-{randomNumbers}";

            // Check if the reference code is already used
            while (_context.Users.Any(u => u.ReferenceCode == referenceCode))
            {
                // If the reference code is already used, regenerate it
                randomNumbers = random.Next(100000, 999999);
                referenceCode = $"PRX-{userId}-{randomNumbers}";
            }

            return referenceCode;
        }

        private int ExtractUserIdFromReferenceCode(string referenceCode)
        {
            // Split the reference code by "-"
            var parts = referenceCode.Split('-');

            // The user ID is the second part after "PRX-"
            if (parts.Length >= 2 && int.TryParse(parts[1], out int userId))
            {
                return userId;
            }

            return 0;
        }

        private bool IsValidRandomPart(string referenceCode)
        {
            // Extract the random part from the reference code
            var randomPart = referenceCode.Split('-')[2];

            // Validate the random part (you can implement your own validation logic here)
            // For example, you can check the length or format of the random part
            return randomPart.Length == 6 && int.TryParse(randomPart, out _);
        }


    }

}
