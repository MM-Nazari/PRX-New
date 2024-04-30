using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using PRX.Data;
using PRX.Dto.User;
using PRX.Models.User;

namespace PRX.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "UserStates")]
    public class UserStatesController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public UserStatesController(PRXDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllUserStates()
        {
            var userStates = _context.UserStates.ToList();
            var userStateDtos = userStates.Select(userState => new UserStateDto
            {
                Id = userState.Id,
                UserId = userState.UserId,
                State = userState.State,
                IsDeleted = userState.IsDeleted
            }).ToList();
            return Ok(userStateDtos);
        }

        [HttpGet("GetByUserId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserStateByUserId(int id)
        {
            var userState = _context.UserStates.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
            if (userState == null)
            {
                return NotFound();
            }
            var userStateDto = new UserStateDto
            {
                Id = userState.Id,
                UserId = userState.UserId,
                State = userState.State
            };
            return Ok(userStateDto);
        }

        [HttpGet("GetById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserStateById(int id)
        {
            var userState = _context.UserStates.FirstOrDefault(u => u.Id == id && !u.IsDeleted);
            if (userState == null)
            {
                return NotFound();
            }
            var userStateDto = new UserStateDto
            {
                Id = userState.Id,
                UserId = userState.UserId,
                State = userState.State
            };
            return Ok(userStateDto);
        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateUserState([FromBody] UserStateDto userStateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userState = new UserState
            {
                UserId = userStateDto.UserId,
                State = userStateDto.State
            };

            _context.UserStates.Add(userState);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUserStateById), new { id = userState.Id }, userState);
        }

        [HttpPut("PutById{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserState(int id, [FromBody] UserStateDto userStateDto)
        {
            var userState = _context.UserStates.FirstOrDefault(u => u.Id == id && !u.IsDeleted);
            if (userState == null)
            {
                return NotFound();
            }

            userState.UserId = userStateDto.UserId;
            userState.State = userStateDto.State;

            _context.SaveChanges();

            return Ok();
        }


        [HttpPut("PutByUserId{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserStateByUserId(int id, [FromBody] UserStateDto userStateDto)
        {
            var userState = _context.UserStates.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
            if (userState == null)
            {
                return NotFound();
            }
            userState.Id = userStateDto.Id;
            userState.State = userStateDto.State;

            _context.SaveChanges();

            return Ok();
        }



        [HttpDelete("DeleteById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserState(int id)
        {
            var userState = _context.UserStates.FirstOrDefault(u => u.Id == id);
            if (userState == null)
            {
                return NotFound();
            }

            userState.IsDeleted = true;
            _context.SaveChanges();

            return Ok();
        }


        [HttpDelete("DeleteByUserId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserStateByUser(int id)
        {
            var userState = _context.UserStates.FirstOrDefault(u => u.UserId == id);
            if (userState == null)
            {
                return NotFound();
            }

            userState.IsDeleted = true;
            _context.SaveChanges();

            return Ok();
        }


        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearUserStates()
        {
            _context.UserStates.RemoveRange(_context.UserStates);
            _context.SaveChanges();
            return Ok();
        }
    }
}
