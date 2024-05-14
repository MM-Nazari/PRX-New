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

        public UsersController(PRXDbContext context, IConfiguration configuration)
        {
            _context = context;
            _utils = new Utils.Utils(); // Instantiate Utils
            _configuration = configuration;
        }

        [HttpGet("PhoneExistance/{phoneNumber}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CheckPhoneNumberExists(string phoneNumber)
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
                    return Ok(new { messagde = ResponseMessages.PhoneExistanseFalse });
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
        public IActionResult CreateUser([FromBody] UserDto userDto)
        {
            // Validate the DTO if necessary
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Optional: Check if user already exists
            var existingUser = _context.Users.FirstOrDefault(u => u.PhoneNumber == userDto.PhoneNumber);
            if (existingUser != null)
            {
                //Response.Headers.Add("message", ResponseMessages.UserExists);
                return BadRequest(new { message = ResponseMessages.UsersPhoneExists });
            }

            try 
            {

                // Hash the password before saving it to the database
                var hashedPassword = _utils.HashPassword(userDto.Password);

                // Map DTO to domain model
                var user = new PRX.Models.User.User
                {
                    Password = hashedPassword,
                    PhoneNumber = userDto.PhoneNumber,
                    ReferenceCode = userDto.ReferenceCode
                };

                // Add user to database
                _context.Users.Add(user);
                _context.SaveChanges();

                // Return the created user
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);

            } 
            catch (Exception ex)          
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }


        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Login([FromBody] UserDto userDto)
        {
            var user = _context.Users.FirstOrDefault(u => u.PhoneNumber == userDto.PhoneNumber);
            if (user == null)
            {
                //Response.Headers.Add("message", "User does not exist");
                return Unauthorized(new {message = ResponseMessages.UserNotExists});
            }


            if (!_utils.VerifyPassword(userDto.Password, user.Password))
            {
                return Unauthorized(new {message = ResponseMessages.PasswordIncorrect});
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

    }

}
