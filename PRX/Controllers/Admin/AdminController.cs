using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PRX.Data;
using PRX.Dto.Admin;
using PRX.Dto.User;
using PRX.Models.Admin;
using PRX.Models.User;
using PRX.Utils;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace PRX.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Admins")]
    public class AdminController : ControllerBase
    {
        private readonly PRXDbContext _context;
        private readonly Utils.Utils _utils; // Add Utils instance
        private readonly IConfiguration _configuration;

        public AdminController(PRXDbContext context, IConfiguration configuration)
        {
            _context = context;
            _utils = new Utils.Utils();
            _configuration = configuration;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult RegisterAdmin([FromBody] AdminDto adminDto)
        {
            try 
            {
                // Validate the DTO if necessary
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var hashedPassword = _utils.HashPassword(adminDto.Password);


                // Map DTO to domain model
                var admin = new PRX.Models.Admin.Admin
                {
                    Username = adminDto.Username,
                    Password = hashedPassword,
                    Role = "Admin" // Set role as "Admin"
                };

                // Add admin to database
                _context.Admins.Add(admin);
                _context.SaveChanges();

                // Return the created admin
                return CreatedAtAction(nameof(GetAdminById), new { id = admin.Id }, admin);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] AdminDto adminDto)
        {
            try
            {
                var admin = _context.Admins.FirstOrDefault(a => a.Username == adminDto.Username);
                if (admin == null || !_utils.VerifyPassword(adminDto.Password, admin.Password))
                {
                    return Unauthorized(new { message = ResponseMessages.AdminNotFound});
                }

                // Generate JWT token
                var token = GenerateJwtToken(admin);

                return Ok(new { Authorization = token });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // GET all admins
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetAllAdmins()
        {
            try
            {
                var admins = _context.Admins.ToList();
                var adminDtos = admins.Select(admin => new AdminDto
                {
                    Id = admin.Id,
                    Username = admin.Username
                    // Other properties as needed
                }).ToList();
                return Ok(adminDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // GET admin by ID
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAdminById(int id)
        {
            try
            {
                var admin = _context.Admins.FirstOrDefault(a => a.Id == id);
                if (admin == null)
                {
                    return Unauthorized(new { message = ResponseMessages.AdminNotFound });
                }

                var adminDto = new AdminDto
                {
                    Id = admin.Id,
                    Username = admin.Username
                    // Other properties as needed
                };

                return Ok(adminDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // Update admin by ID
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateAdmin(int id, [FromBody] AdminDto adminDto)
        {
            try
            {
                var admin = _context.Admins.FirstOrDefault(a => a.Id == id);
                if (admin == null)
                {
                    return Unauthorized(new { message = ResponseMessages.AdminNotFound });
                }

                // Update admin properties
                admin.Username = adminDto.Username;
                var hashedPassword = _utils.HashPassword(adminDto.Password);
                admin.Password = hashedPassword;
                // Update other properties as needed

                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // Delete admin by ID
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteAdmin(int id)
        {
            try
            {
                var admin = _context.Admins.FirstOrDefault(a => a.Id == id);
                if (admin == null)
                {
                    return Unauthorized(new { message = ResponseMessages.AdminNotFound });
                }

                _context.Admins.Remove(admin);
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        private string GenerateJwtToken(PRX.Models.Admin.Admin admin)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            if (securityKey.KeySize < 256)
            {
                throw new ArgumentException("JWT security key size must be at least 256 bits (32 bytes).");
            }

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Define custom claim types
            const string IdClaimType = "id";
            const string UserNameClaimType = "user_name";
            const string RoleClaimType = "role";

            // Create claims for user ID and phone number using custom claim types
            var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(IdClaimType, admin.Id.ToString()),
                new System.Security.Claims.Claim(UserNameClaimType, admin.Username),
                new System.Security.Claims.Claim(RoleClaimType, admin.Role),
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
