using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.User;
using PRX.Models.User;
using PRX.Utils;

namespace PRX.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "UserFinancialChanges")]
    public class UserFinancialChangesController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public UserFinancialChangesController(PRXDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllUserFinancialChanges()
        {
            try
            {
                var userFinancialChanges = _context.UserFinancialChanges.ToList();
                return Ok(userFinancialChanges);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        [HttpGet("{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserFinancialChangesById(int requestId)
        {
            try
            {

                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Ensure that the user is updating their own profile
                // Fetch the request
                var request = _context.Requests.FirstOrDefault(r => r.Id == requestId);

                if (request == null)
                {
                    return NotFound(new { message = ResponseMessages.RequestNotFound });
                }

                // Ensure that the user associated with the request matches the token user ID
                if (request.UserId != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }

                var userFinancialChanges = _context.UserFinancialChanges.FirstOrDefault(u => u.RequestId == requestId && !u.IsDeleted);
                if (userFinancialChanges == null)
                {
                    return NotFound(new { message = ResponseMessages.UserFinancialChangeNotFound });
                }
                return Ok(userFinancialChanges);

            }

            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        [HttpPost]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateUserFinancialChanges([FromBody] UserFinancialChangesDto userFinancialChangesDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check if the UserId already exists
                var existingUserFinancialChange = _context.UserFinancialChanges
                                                          .FirstOrDefault(ufc => ufc.RequestId == userFinancialChangesDto.RequestId);
                if (existingUserFinancialChange != null)
                {
                    return BadRequest(new { message = ResponseMessages.UserFinancialChangeDuplicate });
                }

                var userFinancialChanges = new UserFinancialChanges
                {
                    RequestId = userFinancialChangesDto.RequestId,
                    Description = userFinancialChangesDto.Description
                };

                _context.UserFinancialChanges.Add(userFinancialChanges);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetUserFinancialChangesById), new { requestId = userFinancialChanges.Id }, userFinancialChanges);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        [HttpPut("{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserFinancialChanges(int requestId, [FromBody] UserFinancialChangesDto userFinancialChangesDto)
        {

            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Fetch the request
                var request = _context.Requests.FirstOrDefault(r => r.Id == requestId);

                if (request == null)
                {
                    return NotFound(new { message = ResponseMessages.RequestNotFound });
                }

                // Ensure that the user associated with the request matches the token user ID
                if (request.UserId != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }

                var userFinancialChanges = _context.UserFinancialChanges.FirstOrDefault(u => u.RequestId == requestId && !u.IsDeleted);
                if (userFinancialChanges == null)
                {
                    return NotFound(new { message = ResponseMessages.UserFinancialChangeNotFound });
                }

                userFinancialChanges.RequestId = userFinancialChangesDto.RequestId;
                userFinancialChanges.Description = userFinancialChangesDto.Description;

                _context.SaveChanges();

                return Ok(userFinancialChanges);

            }

            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

           
        }

        // PATCH: api/UserFinancialChanges/{requestId}
        [HttpPatch("{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult PatchUserFinancialChanges(int requestId, [FromBody] JsonPatchDocument<UserFinancialChangesDto> patchDoc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Fetch the request
                var request = _context.Requests.FirstOrDefault(r => r.Id == requestId);

                if (request == null)
                {
                    return NotFound(new { message = ResponseMessages.RequestNotFound });
                }

                // Ensure that the user associated with the request matches the token user ID
                if (request.UserId != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }

                var userFinancialChanges = _context.UserFinancialChanges.FirstOrDefault(u => u.RequestId == requestId && !u.IsDeleted);
                if (userFinancialChanges == null)
                {
                    return NotFound(new { message = ResponseMessages.UserFinancialChangeNotFound });
                }

                // Create a DTO to hold the current user financial changes information
                var userFinancialChangesDto = new UserFinancialChangesDto
                {
                    RequestId = userFinancialChanges.RequestId,
                    Description = userFinancialChanges.Description
                };

                // Apply the patch document to the DTO
                patchDoc.ApplyTo(userFinancialChangesDto, ModelState);

                // Validate the model after applying the patch
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Update the user financial changes properties based on the modified DTO
                userFinancialChanges.RequestId = userFinancialChangesDto.RequestId; // Update if present
                userFinancialChanges.Description = userFinancialChangesDto.Description; // Update if present

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


        [HttpDelete("{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserFinancialChanges(int requestId)
        {

            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);
                // Fetch the request
                var request = _context.Requests.FirstOrDefault(r => r.Id == requestId);

                if (request == null)
                {
                    return NotFound(new { message = ResponseMessages.RequestNotFound });
                }

                // Ensure that the user associated with the request matches the token user ID
                if (request.UserId != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }

                var userFinancialChanges = _context.UserFinancialChanges.FirstOrDefault(u => u.RequestId == requestId && !u.IsDeleted);
                if (userFinancialChanges == null)
                {
                    return NotFound(new { message = ResponseMessages.UserFinancialChangeNotFound });
                }

                userFinancialChanges.IsDeleted = true;
                _context.SaveChanges();

                return NoContent();

            }

            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult ClearUserFinancialChanges()
        {
            try
            {
                _context.UserFinancialChanges.RemoveRange(_context.UserFinancialChanges);
                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        // PUT: api/HaghighiUserProfile/complete/{id}
        [HttpPut("complete/{requestId}")]
        //[Authorize(Roles = "Admin")] // Assuming only admins can mark profiles as complete
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkFinancwChangeAsComplete(int requestId)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userFinancialChanges = _context.UserFinancialChanges.FirstOrDefault(u => u.RequestId == requestId);
                if (userFinancialChanges == null)
                {
                    return NotFound(new { message = ResponseMessages.UserFinancialChangeNotFound });
                }

                userFinancialChanges.IsComplete = true;
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }


        [HttpGet("isComplete/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CheckCompletionStatus(int requestId)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);
                // Fetch the request
                var request = _context.Requests.FirstOrDefault(r => r.Id == requestId);

                if (request == null)
                {
                    return NotFound(new { message = ResponseMessages.RequestNotFound });
                }

                // Ensure that the user associated with the request matches the token user ID
                if (request.UserId != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }


                var record = _context.UserFinancialChanges.FirstOrDefault(e => e.RequestId == requestId);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.UserFinancialChangeNotFound });
                }

                return Ok(new { isComplete = record.IsComplete });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }


        [HttpGet("Admin")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAllUserFinancialChangesAdmin()
        {

            try
            {
                var financialChanges = _context.UserFinancialChanges.ToList();
                var financialChangeDtos = financialChanges.Select(change => new UserFinancialChangesDto
                {
                    RequestId = change.RequestId,
                    Description = change.Description,
                    IsComplete = change.IsComplete,
                    IsDeleted = change.IsDeleted
                }).ToList();
                return Ok(financialChangeDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        [HttpGet("Admin/{requestId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserFinancialChangeByIdAdmin(int requestId)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var financialChange = _context.UserFinancialChanges.FirstOrDefault(change => change.RequestId == requestId && !change.IsDeleted);
                if (financialChange == null)
                {
                    return NotFound(new { message = ResponseMessages.UserFinancialChangeNotFound });
                }

                var financialChangeDto = new UserFinancialChangesDto
                {
                    RequestId = financialChange.RequestId,
                    Description = financialChange.Description,
                    IsComplete = financialChange.IsComplete,
                    IsDeleted = financialChange.IsDeleted
                };

                return Ok(financialChangeDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        [HttpPut("Admin/{requestId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserFinancialChangeAdmin(int requestId, [FromBody] UserFinancialChangesDto financialChangeDto)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var financialChange = _context.UserFinancialChanges.FirstOrDefault(change => change.RequestId == requestId && !change.IsDeleted);
                if (financialChange == null)
                {
                    return NotFound(new { message = ResponseMessages.UserFinancialChangeNotFound });
                }

                financialChange.Description = financialChangeDto.Description;
                //financialChange.IsComplete = financialChangeDto.IsComplete;
                //financialChange.IsDeleted = financialChangeDto.IsDeleted;

                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK});
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpDelete("Admin/{requestId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserFinancialChangeAdmin(int requestId)
        {
            try
            {

                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var financialChange = _context.UserFinancialChanges.FirstOrDefault(change => change.RequestId == requestId && !change.IsDeleted);
                if (financialChange == null)
                {
                    return NotFound(new { message = ResponseMessages.UserFinancialChangeNotFound });
                }

                financialChange.IsDeleted = true;
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        [HttpDelete("Admin/UserFinancialChanges/Clear")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearUserFinancialChangesAdmin()
        {
            try
            {
                _context.UserFinancialChanges.RemoveRange(_context.UserFinancialChanges);
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK }); ;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }


    }
}
