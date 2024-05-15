using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using PRX.Data;
using PRX.Dto.User;
using PRX.Models.User;
using PRX.Utils;

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
            try
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpGet("GetByUserId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserStateByUserId(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userState = _context.UserStates.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userState == null)
                {
                    return NotFound(new { message = ResponseMessages.UserStateNotFound });
                }
                var userStateDto = new UserStateDto
                {
                    Id = userState.Id,
                    UserId = userState.UserId,
                    State = userState.State
                };
                return Ok(userStateDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpGet("GetById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserStateById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userState = _context.UserStates.FirstOrDefault(u => u.Id == id && !u.IsDeleted);
                if (userState == null)
                {
                    return NotFound(new { message = ResponseMessages.UserStateNotFound });
                }
                var userStateDto = new UserStateDto
                {
                    Id = userState.Id,
                    UserId = userState.UserId,
                    State = userState.State
                };
                return Ok(userStateDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateUserState([FromBody] UserStateDto userStateDto)
        {

            try
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpPut("PutById{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserState(int id, [FromBody] UserStateDto userStateDto)
        {

            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userState = _context.UserStates.FirstOrDefault(u => u.Id == id && !u.IsDeleted);
                if (userState == null)
                {
                    return NotFound(new { message = ResponseMessages.UserStateNotFound });
                }

                userState.UserId = userStateDto.UserId;
                userState.State = userStateDto.State;

                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }


        [HttpPut("PutByUserId{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserStateByUserId(int id, [FromBody] UserStateDto userStateDto)
        {
            try
            {

                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userState = _context.UserStates.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userState == null)
                {
                    return NotFound(new { message = ResponseMessages.UserStateNotFound });
                }
                userState.Id = userStateDto.Id;
                userState.State = userStateDto.State;

                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }



        [HttpDelete("DeleteById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserState(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userState = _context.UserStates.FirstOrDefault(u => u.Id == id);
                if (userState == null)
                {
                    return NotFound(new { message = ResponseMessages.UserStateNotFound });
                }

                userState.IsDeleted = true;
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }


        [HttpDelete("DeleteByUserId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserStateByUser(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userState = _context.UserStates.FirstOrDefault(u => u.UserId == id);
                if (userState == null)
                {
                    return NotFound(new { message = ResponseMessages.UserStateNotFound });
                }

                userState.IsDeleted = true;
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }


        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearUserStates()
        {
            try
            {
                _context.UserStates.RemoveRange(_context.UserStates);
                _context.SaveChanges();
                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }
    }
}
