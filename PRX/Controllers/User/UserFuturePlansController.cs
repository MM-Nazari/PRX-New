using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using PRX.Data;
using PRX.Dto.User;
using PRX.Models.User;
using PRX.Utils;
using Azure.Core;
using Microsoft.AspNetCore.JsonPatch;

namespace PRX.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "UserFuturePlans")]
    public class UserFuturePlansController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public UserFuturePlansController(PRXDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllUserFuturePlans()
        {
            try
            {
                var userFuturePlans = _context.UserFuturePlans.ToList();
                return Ok(userFuturePlans);
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
        public IActionResult GetUserFuturePlansById(int requestId)
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

                var userFuturePlans = _context.UserFuturePlans.FirstOrDefault(u => u.RequestId == requestId && !u.IsDeleted);
                if (userFuturePlans == null)
                {
                    return NotFound(new { message = ResponseMessages.UserFuturePlanNotFound });
                }
                return Ok(userFuturePlans);

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
        public IActionResult CreateUserFuturePlans([FromBody] UserFuturePlansDto userFuturePlansDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check if the UserId already exists
                var existingUserFuturePlan = _context.UserFuturePlans
                                                     .FirstOrDefault(ufp => ufp.RequestId == userFuturePlansDto.RequestId);
                if (existingUserFuturePlan != null)
                {
                    return BadRequest(new { message = ResponseMessages.UserFuturePlanDuplicate });
                }

                var userFuturePlans = new UserFuturePlans
                {
                    RequestId = userFuturePlansDto.RequestId,
                    Description = userFuturePlansDto.Description
                };

                _context.UserFuturePlans.Add(userFuturePlans);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetUserFuturePlansById), new { requestId = userFuturePlans.Id }, userFuturePlans);
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
        public IActionResult UpdateUserFuturePlans(int requestId, [FromBody] UserFuturePlansDto userFuturePlansDto)
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

                var userFuturePlans = _context.UserFuturePlans.FirstOrDefault(u => u.RequestId == requestId && !u.IsDeleted);
                if (userFuturePlans == null)
                {
                    return NotFound(new { message = ResponseMessages.UserFuturePlanNotFound });
                }

                userFuturePlans.RequestId = userFuturePlansDto.RequestId;
                userFuturePlans.Description = userFuturePlansDto.Description;

                _context.SaveChanges();

                return Ok(userFuturePlans);

            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        // PATCH: api/UserFuturePlans/{requestId}
        [HttpPatch("{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult PatchUserFuturePlans(int requestId, [FromBody] JsonPatchDocument<UserFuturePlansDto> patchDoc)
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

                var userFuturePlans = _context.UserFuturePlans.FirstOrDefault(u => u.RequestId == requestId && !u.IsDeleted);
                if (userFuturePlans == null)
                {
                    return NotFound(new { message = ResponseMessages.UserFuturePlanNotFound });
                }

                // Create a DTO to hold the current user future plans information
                var userFuturePlansDto = new UserFuturePlansDto
                {
                    RequestId = userFuturePlans.RequestId,
                    Description = userFuturePlans.Description
                };

                // Apply the patch document to the DTO
                patchDoc.ApplyTo(userFuturePlansDto, ModelState);

                // Validate the model after applying the patch
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Update the user future plans properties based on the modified DTO
                userFuturePlans.RequestId = userFuturePlansDto.RequestId; // Update if present
                userFuturePlans.Description = userFuturePlansDto.Description; // Update if present

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
        public IActionResult DeleteUserFuturePlans(int requestId)
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

                var userFuturePlans = _context.UserFuturePlans.FirstOrDefault(u => u.RequestId == requestId && !u.IsDeleted);
                if (userFuturePlans == null)
                {
                    return NotFound(new { message = ResponseMessages.UserFuturePlanNotFound });
                }

                userFuturePlans.IsDeleted = true;
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
        public IActionResult ClearUserFuturePlans()
        {
            try
            {
                _context.UserFuturePlans.RemoveRange(_context.UserFuturePlans);
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

                var userFinancialChanges = _context.UserFuturePlans.FirstOrDefault(u => u.RequestId == requestId);
                if (userFinancialChanges == null)
                {
                    return NotFound(new { message = ResponseMessages.UserFuturePlanNotFound });
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

                var record = _context.UserFuturePlans.FirstOrDefault(e => e.RequestId == requestId);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.UserFuturePlanNotFound });
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
        public IActionResult GetAllUserFuturePlansAdmin()
        {
            try
            {
                var futurePlans = _context.UserFuturePlans.ToList();
                var futurePlanDtos = futurePlans.Select(plan => new UserFuturePlansDto
                {
                    RequestId = plan.RequestId,
                    Description = plan.Description,
                    IsComplete = plan.IsComplete,
                    IsDeleted = plan.IsDeleted
                }).ToList();
                return Ok(futurePlanDtos);
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
        public IActionResult GetUserFuturePlansByIdAdmin(int requestId)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var futurePlan = _context.UserFuturePlans.FirstOrDefault(plan => plan.RequestId == requestId && !plan.IsDeleted);
                if (futurePlan == null)
                {
                    return NotFound(new { message = ResponseMessages.UserFuturePlanNotFound });
                }

                var futurePlanDto = new UserFuturePlansDto
                {
                    RequestId = futurePlan.RequestId,
                    Description = futurePlan.Description,
                    IsComplete = futurePlan.IsComplete,
                    IsDeleted = futurePlan.IsDeleted
                };

                return Ok(futurePlanDto);
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
        public IActionResult UpdateUserFuturePlansAdmin(int requestId, [FromBody] UserFuturePlansDto futurePlanDto)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var futurePlan = _context.UserFuturePlans.FirstOrDefault(plan => plan.RequestId == requestId && !plan.IsDeleted);
                if (futurePlan == null)
                {
                    return NotFound(new { message = ResponseMessages.UserFuturePlanNotFound });
                }

                futurePlan.Description = futurePlanDto.Description;

                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
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
        public IActionResult DeleteUserFuturePlansAdmin(int requestId)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var futurePlan = _context.UserFuturePlans.FirstOrDefault(plan => plan.RequestId == requestId && !plan.IsDeleted);
                if (futurePlan == null)
                {
                    return NotFound(new { message = ResponseMessages.UserFuturePlanNotFound });
                }

                futurePlan.IsDeleted = true;
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpDelete("Admin/Clear")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearUserFuturePlansAdmin()
        {
            try
            {
                _context.UserFuturePlans.RemoveRange(_context.UserFuturePlans);
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
