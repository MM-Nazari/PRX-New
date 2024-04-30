using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using PRX.Data;
using PRX.Dto.User;
using PRX.Models.User;

namespace PRX.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "UserTypes")]
    public class UserTypesController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public UserTypesController(PRXDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllUserTypes()
        {
            var userTypes = _context.UserTypes.ToList();
            var userTypeDtos = userTypes.Select(userType => new UserTypeDto
            {
                Id = userType.Id,
                UserId = userType.UserId,
                Type = userType.Type
            }).ToList();
            return Ok(userTypeDtos);
        }

        [HttpGet("GetById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserTypeById(int id)
        {
            var userType = _context.UserTypes.FirstOrDefault(u => u.Id == id && !u.IsDeleted);
            if (userType == null)
            {
                return NotFound();
            }
            var userTypeDto = new UserTypeDto
            {
                Id = userType.Id,
                UserId = userType.UserId,
                Type = userType.Type
            };
            return Ok(userTypeDto);
        }

        [HttpGet("GetByUserId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserTypeByUserId(int id)
        {
            var userType = _context.UserTypes.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
            if (userType == null)
            {
                return NotFound();
            }
            var userTypeDto = new UserTypeDto
            {
                Id = userType.Id,
                UserId = userType.UserId,
                Type = userType.Type
            };
            return Ok(userTypeDto);
        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateUserType([FromBody] UserTypeDto userTypeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userType = new UserType
            {
                UserId = userTypeDto.UserId,
                Type = userTypeDto.Type
            };

            _context.UserTypes.Add(userType);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUserTypeById), new { id = userType.Id }, userType);
        }

        [HttpPut("PutById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserType(int id, [FromBody] UserTypeDto userTypeDto)
        {
            var userType = _context.UserTypes.FirstOrDefault(u => u.Id == id && !u.IsDeleted);
            if (userType == null)
            {
                return NotFound();
            }

            userType.UserId = userTypeDto.UserId;
            userType.Type = userTypeDto.Type;

            _context.SaveChanges();

            return Ok();
        }


        [HttpPut("PutByUserId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserTypeBy(int id, [FromBody] UserTypeDto userTypeDto)
        {
            var userType = _context.UserTypes.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
            if (userType == null)
            {
                return NotFound();
            }

            userType.Id = userTypeDto.Id;
            userType.Type = userTypeDto.Type;

            _context.SaveChanges();

            return Ok();
        }


        [HttpDelete("DeleteById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserType(int id)
        {
            var userType = _context.UserTypes.FirstOrDefault(u => u.Id == id);
            if (userType == null)
            {
                return NotFound();
            }

            userType.IsDeleted = true;
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("DeleteByUserId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserTypeBy(int id)
        {
            var userType = _context.UserTypes.FirstOrDefault(u => u.UserId == id);
            if (userType == null)
            {
                return NotFound();
            }

            userType.IsDeleted = true;
            _context.SaveChanges();

            return Ok();
        }


        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearUserTypes()
        {
            _context.UserTypes.RemoveRange(_context.UserTypes);
            _context.SaveChanges();
            return Ok();
        }
    }
}
