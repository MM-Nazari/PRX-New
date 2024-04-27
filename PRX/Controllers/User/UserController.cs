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

        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAllUsers()
        {
            var users = _context.Users.ToList();
            var userDtos = users.Select(user => new UserDto
            {
                Id = user.Id,
                PhoneNumber = user.PhoneNumber,
                ReferenceCode = user.ReferenceCode
            }).ToList();
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserById(int id)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                {
                    return NotFound();
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
                // Log the exception for debugging purposes
                // Replace Console.WriteLine with your preferred logging mechanism
                Console.WriteLine($"Exception occurred: {ex}");

                // Return 401 Unauthorized status code
                return Unauthorized(new { Message = "Unauthorized. Please check your token or authentication credentials." });
            }
        }


        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateUser([FromBody] UserDto userDto)
        {
            // Validate the DTO if necessary
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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


        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] UserDto userDto)
        {
            var user = _context.Users.FirstOrDefault(u => u.PhoneNumber == userDto.PhoneNumber);
            if (user == null || !VerifyPassword(userDto.Password, user.Password))
            {
                return Unauthorized(new { Message = "Invalid phone number or password" });
            }

            // Generate JWT token
            var token = GenerateJwtToken(user);

            return Ok(new { Authorization = token });
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            // Extract the salt and hashed password from the stored hash
            string[] hashParts = hashedPassword.Split(':');
            byte[] salt = Convert.FromBase64String(hashParts[0]);
            string storedHash = hashParts[1];

            // Compute the hash of the provided password with the same salt
            string computedHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32));

            // Compare the computed hash with the stored hash
            return storedHash == computedHash;
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

            // Create claims for user ID and phone number using custom claim types
            var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(IdClaimType, user.Id.ToString()),
                new System.Security.Claims.Claim(PhoneClaimType, user.PhoneNumber)
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



        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUser(int id, [FromBody] UserDto userDto)
        {
            // Find the user to update
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
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

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("clear")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearUser()
        {
            _context.Users.RemoveRange(_context.Users);
            _context.SaveChanges();

            return Ok();
        }

    }

}
