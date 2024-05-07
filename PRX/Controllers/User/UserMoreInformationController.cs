﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using PRX.Data;
using PRX.Dto.User;
using PRX.Models.User;

namespace PRX.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "UserMoreInformations")]
    public class UserMoreInformationController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public UserMoreInformationController(PRXDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllUserMoreInformation()
        {
            var userMoreInformation = _context.UserMoreInformations.ToList();
            return Ok(userMoreInformation);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserMoreInformationById(int id)
        {

            try
            {

                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Ensure that the user is updating their own profile
                if (id != tokenUserId)
                {
                    return Forbid(); // Or return 403 Forbidden
                }
                var userMoreInformation = _context.UserMoreInformations.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userMoreInformation == null)
                {
                    return NotFound();
                }
                return Ok(userMoreInformation);

            }

            catch (Exception ex)
            {
                return BadRequest();
            }


        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateUserMoreInformation([FromBody] UserMoreInformationDto userMoreInformationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userMoreInformation = new UserMoreInformation
            {
                UserId = userMoreInformationDto.UserId,
                Info = userMoreInformationDto.Info
            };

            _context.UserMoreInformations.Add(userMoreInformation);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUserMoreInformationById), new { id = userMoreInformation.Id }, userMoreInformation);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserMoreInformation(int id, [FromBody] UserMoreInformationDto userMoreInformationDto)
        {

            try
            {

                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Ensure that the user is updating their own profile
                if (id != tokenUserId)
                {
                    return Forbid(); // Or return 403 Forbidden
                }
                var userMoreInformation = _context.UserMoreInformations.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userMoreInformation == null)
                {
                    return NotFound();
                }

                
                userMoreInformation.Info = userMoreInformationDto.Info;

                _context.SaveChanges();

                return Ok(userMoreInformation);

            }

            catch (Exception ex)
            {
                
                return BadRequest();
            }


        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserMoreInformation(int id)
        {

            try
            {

                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Ensure that the user is updating their own profile
                if (id != tokenUserId)
                {
                    return Forbid(); // Or return 403 Forbidden
                }
                var userMoreInformation = _context.UserMoreInformations.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userMoreInformation == null)
                {
                    return NotFound();
                }

                userMoreInformation.IsDeleted = true;
                _context.SaveChanges();

                return NoContent();

            }

            catch (Exception ex)
            {
               
                return BadRequest();
            }


        }

        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult ClearUserMoreInformations()
        {
            _context.UserMoreInformations.RemoveRange(_context.UserMoreInformations);
            _context.SaveChanges();

            return NoContent();
        }

        // PUT: api/HaghighiUserProfile/complete/{id}
        [HttpPut("complete/{id}")]
        //[Authorize(Roles = "Admin")] // Assuming only admins can mark profiles as complete
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkFinancwChangeAsComplete(int id)
        {
            var userFinancialChanges = _context.UserMoreInformations.FirstOrDefault(u => u.UserId == id);
            if (userFinancialChanges == null)
            {
                return NotFound();
            }

            userFinancialChanges.IsComplete = true;
            _context.SaveChanges();

            return Ok();
        }

        [HttpGet("Admin")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAllUserMoreInformations()
        {
            var userMoreInformations = _context.UserMoreInformations.ToList();
            var userMoreInformationDtos = userMoreInformations.Select(info => new UserMoreInformationDto
            {
                UserId = info.UserId,
                Info = info.Info,
                IsComplete = info.IsComplete,
                IsDeleted = info.IsDeleted
            }).ToList();
            return Ok(userMoreInformationDtos);
        }

        [HttpGet("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserMoreInformationByIdAdmin(int id)
        {
            var userMoreInformation = _context.UserMoreInformations.FirstOrDefault(info => info.UserId == id && !info.IsDeleted);
            if (userMoreInformation == null)
            {
                return NotFound();
            }

            var userMoreInformationDto = new UserMoreInformationDto
            {
                UserId = userMoreInformation.UserId,
                Info = userMoreInformation.Info,
                IsComplete = userMoreInformation.IsComplete,
                IsDeleted = userMoreInformation.IsDeleted
            };

            return Ok(userMoreInformationDto);
        }

        [HttpPut("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserMoreInformationAdmin(int id, [FromBody] UserMoreInformationDto userMoreInformationDto)
        {
            var userMoreInformation = _context.UserMoreInformations.FirstOrDefault(info => info.UserId == id && !info.IsDeleted);
            if (userMoreInformation == null)
            {
                return NotFound();
            }

            userMoreInformation.Info = userMoreInformationDto.Info;

            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserMoreInformationAdmin(int id)
        {
            var userMoreInformation = _context.UserMoreInformations.FirstOrDefault(info => info.UserId == id && !info.IsDeleted);
            if (userMoreInformation == null)
            {
                return NotFound();
            }

            userMoreInformation.IsDeleted = true;
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("Admin/Clear")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearUserMoreInformationsAdmin()
        {
            _context.UserMoreInformations.RemoveRange(_context.UserMoreInformations);
            _context.SaveChanges();

            return Ok();
        }

    }
}
