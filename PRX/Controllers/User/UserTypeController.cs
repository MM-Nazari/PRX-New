using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using PRX.Data;
using PRX.Dto.User;
using PRX.Models.User;
using PRX.Utils;
using Microsoft.AspNetCore.JsonPatch;

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
            try
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        [HttpGet("GetById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserTypeById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userType = _context.UserTypes.FirstOrDefault(u => u.Id == id && !u.IsDeleted);
                if (userType == null)
                {
                    return NotFound(new { message = ResponseMessages.UserTypeNotFound });
                }
                var userTypeDto = new UserTypeDto
                {
                    Id = userType.Id,
                    UserId = userType.UserId,
                    Type = userType.Type
                };
                return Ok(userTypeDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpGet("GetByUserId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserTypeByUserId(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userType = _context.UserTypes.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userType == null)
                {
                    return NotFound(new { message = ResponseMessages.UserTypeNotFound });
                }
                var userTypeDto = new UserTypeDto
                {
                    Id = userType.Id,
                    UserId = userType.UserId,
                    Type = userType.Type
                };
                return Ok(userTypeDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateUserType([FromBody] UserTypeDto userTypeDto)
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpPut("PutById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserType(int id, [FromBody] UserTypeDto userTypeDto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userType = _context.UserTypes.FirstOrDefault(u => u.Id == id && !u.IsDeleted);
                if (userType == null)
                {
                    return NotFound(new { message = ResponseMessages.UserTypeNotFound });
                }

                userType.UserId = userTypeDto.UserId;
                userType.Type = userTypeDto.Type;

                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }


        [HttpPut("PutByUserId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserTypeBy(int id, [FromBody] UserTypeDto userTypeDto)
        {
            try
            {

                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userType = _context.UserTypes.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userType == null)
                {
                    return NotFound(new { message = ResponseMessages.UserTypeNotFound });
                }

                userType.Id = userTypeDto.Id;
                userType.Type = userTypeDto.Type;

                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // PATCH: api/UserType/PatchById/{id}
        [HttpPatch("PatchById/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult PatchUserType(int id, [FromBody] JsonPatchDocument<UserTypeDto> patchDoc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userType = _context.UserTypes.FirstOrDefault(u => u.Id == id && !u.IsDeleted);
                if (userType == null)
                {
                    return NotFound(new { message = ResponseMessages.UserTypeNotFound });
                }

                // Create a DTO to hold the current user type
                var userTypeDto = new UserTypeDto
                {
                    Id = userType.Id,
                    UserId = userType.UserId,
                    Type = userType.Type
                };

                // Apply the patch document to the DTO
                patchDoc.ApplyTo(userTypeDto, ModelState);

                // Validate the model after applying the patch
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Update the user type properties based on the modified DTO
                userType.UserId = userTypeDto.UserId; // Update if present
                userType.Type = userTypeDto.Type; // Update if present

                // Save changes to the database
                _context.SaveChanges();

                // Return 204 No Content
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        // PATCH: api/UserType/PatchByUserId/{id}
        [HttpPatch("PatchByUserId/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult PatchUserTypeByUserId(int id, [FromBody] JsonPatchDocument<UserTypeDto> patchDoc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userType = _context.UserTypes.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userType == null)
                {
                    return NotFound(new { message = ResponseMessages.UserTypeNotFound });
                }

                // Create a DTO to hold the current user type
                var userTypeDto = new UserTypeDto
                {
                    Id = userType.Id,
                    UserId = userType.UserId,
                    Type = userType.Type
                };

                // Apply the patch document to the DTO
                patchDoc.ApplyTo(userTypeDto, ModelState);

                // Validate the model after applying the patch
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Update the user type properties based on the modified DTO
                userType.Id = userTypeDto.Id; // Update if present
                userType.Type = userTypeDto.Type; // Update if present

                // Save changes to the database
                _context.SaveChanges();

                // Return 204 No Content
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }



        [HttpDelete("DeleteById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserType(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userType = _context.UserTypes.FirstOrDefault(u => u.Id == id);
                if (userType == null)
                {
                    return NotFound(new { message = ResponseMessages.UserTypeNotFound });
                }

                userType.IsDeleted = true;
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
        public IActionResult DeleteUserTypeBy(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userType = _context.UserTypes.FirstOrDefault(u => u.UserId == id);
                if (userType == null)
                {
                    return NotFound(new { message = ResponseMessages.UserTypeNotFound });
                }

                userType.IsDeleted = true;
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
        public IActionResult ClearUserTypes()
        {
            try
            {
                _context.UserTypes.RemoveRange(_context.UserTypes);
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
