using Azure.Core;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.User;
using PRX.Models.User;
using PRX.Utils;
using System;
using System.Linq;

namespace PRX.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "UserDebts")]
    public class UserDebtsController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public UserDebtsController(PRXDbContext context)
        {
            _context = context;
        }

        [HttpGet("{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserDebtById(int requestId)
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

                var userDebt = _context.UserDebts.Where(u => u.RequestId == requestId && !u.IsDeleted).Select(r => new UserDebtDto 
                {
                    Id = r.Id,
                    RequestId = r.RequestId,
                    DebtTitle = r.DebtTitle,
                    DebtAmount = r.DebtAmount,
                    DebtDueDate = r.DebtDueDate,
                    DebtRepaymentPercentage = r.DebtRepaymentPercentage,
                    IsComplete = r.IsComplete,
                    IsDeleted = r.IsDeleted
                }
                ).ToList();

                if (userDebt == null)
                {
                    return NotFound(new { message = ResponseMessages.UserDebtNotFound});
                }

                return Ok(userDebt);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateUserDebt([FromBody] UserDebtListDto userDebtDto)
        {
            try
            {
                if (userDebtDto.RequestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                foreach (var debt in userDebtDto.DebtList) 
                {
                    var userDebt = new UserDebt
                    {
                        RequestId = userDebtDto.RequestId,
                        DebtTitle = debt.DebtTitle,
                        DebtAmount = debt.DebtAmount,
                        DebtDueDate = debt.DebtDueDate,
                        DebtRepaymentPercentage = debt.DebtRepaymentPercentage
                    };
                    _context.UserDebts.Add(userDebt);
                }


                
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status201Created, new { message = ResponseMessages.OK });
                //return CreatedAtAction(nameof(GetUserDebtById), new { requestId = userDebt.Id }, userDebt);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        [HttpPut("{id}/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateUserDebt(int id, int requestId, [FromBody] UserDebtDto userDebtDto)
        {
            try
            {
                if (id <= 0 || requestId <= 0)
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

                var userDebt = _context.UserDebts.FirstOrDefault(u => u.RequestId == requestId && u.Id == id && !u.IsDeleted);
                if (userDebt == null)
                {
                    return NotFound(new { message = ResponseMessages.UserDebtNotFound });
                }

                userDebt.RequestId = userDebtDto.RequestId;
                userDebt.DebtTitle = userDebtDto.DebtTitle;
                userDebt.DebtAmount = userDebtDto.DebtAmount;
                userDebt.DebtDueDate = userDebtDto.DebtDueDate;
                userDebt.DebtRepaymentPercentage = userDebtDto.DebtRepaymentPercentage;

                _context.SaveChanges();

                return Ok(userDebt);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        // PATCH: api/UserDebt/{id}/{requestId}
        [HttpPatch("{id}/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult PatchUserDebt(int id, int requestId, [FromBody] JsonPatchDocument<UserDebtDto> patchDoc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (id <= 0 || requestId <= 0)
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

                var userDebt = _context.UserDebts.FirstOrDefault(u => u.RequestId == requestId && u.Id == id && !u.IsDeleted);
                if (userDebt == null)
                {
                    return NotFound(new { message = ResponseMessages.UserDebtNotFound });
                }

                // Create a DTO to hold the current user debt information
                var userDebtDto = new UserDebtDto
                {
                    RequestId = userDebt.RequestId,
                    DebtTitle = userDebt.DebtTitle,
                    DebtAmount = userDebt.DebtAmount,
                    DebtDueDate = userDebt.DebtDueDate,
                    DebtRepaymentPercentage = userDebt.DebtRepaymentPercentage
                };

                // Apply the patch document to the DTO
                patchDoc.ApplyTo(userDebtDto, ModelState);

                // Validate the model after applying the patch
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Update the user debt properties based on the modified DTO
                userDebt.RequestId = userDebtDto.RequestId; // Update if present
                userDebt.DebtTitle = userDebtDto.DebtTitle; // Update if present
                userDebt.DebtAmount = userDebtDto.DebtAmount; // Update if present
                userDebt.DebtDueDate = userDebtDto.DebtDueDate; // Update if present
                userDebt.DebtRepaymentPercentage = userDebtDto.DebtRepaymentPercentage; // Update if present

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


        [HttpDelete("{id}/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteUserDebt(int id, int requestId)
        {
            try
            {
                if (id <= 0 || requestId <= 0)
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
                var userDebt = _context.UserDebts.FirstOrDefault(u => u.RequestId == requestId && u.Id == id && !u.IsDeleted);
                if (userDebt == null)
                {
                    return NotFound(new { message = ResponseMessages.UserDebtNotFound });
                }

                userDebt.IsDeleted = true;
                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        [HttpPut("complete/{id}/{requestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult MarkCompaniesAsComplete(int id, int requestId)
        {
            try
            {

                if (id <= 0 || requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var record = _context.UserDebts.FirstOrDefault(e => e.RequestId == requestId && e.Id == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.UserDebtNotFound });
                }

                record.IsComplete = true;
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        [HttpGet("isComplete/{id}/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CheckCompletionStatus(int id, int requestId)
        {
            try
            {
                if (id <= 0 || requestId <= 0)
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

                var record = _context.UserDebts.FirstOrDefault(e => e.RequestId == requestId && e.Id == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.UserDebtNotFound });
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllUserDebts()
        {
            try
            {
                var userDebts = _context.UserDebts.ToList();
                var userDebtDtos = userDebts.Select(debt => new UserDebtDto
                { 
                    Id = debt.Id,
                    RequestId = debt.RequestId,
                    DebtTitle = debt.DebtTitle,
                    DebtAmount = debt.DebtAmount,
                    DebtDueDate = debt.DebtDueDate,
                    DebtRepaymentPercentage = debt.DebtRepaymentPercentage,
                    IsComplete = debt.IsComplete,
                    IsDeleted = debt.IsDeleted
                }).ToList();

                return Ok(userDebtDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        [HttpGet("Admin/{id}/{requestId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserDebtByIdAdmin(int id, int requestId)
        {
            try
            {
                if (id <= 0 || requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userDebt = _context.UserDebts.FirstOrDefault(debt => debt.RequestId == requestId && debt.Id == id &&!debt.IsDeleted);
                if (userDebt == null)
                {
                    return NotFound(new { message = ResponseMessages.UserDebtNotFound });
                }

                var userDebtDto = new UserDebtDto
                {
                    
                    
                    DebtTitle = userDebt.DebtTitle,
                    DebtAmount = userDebt.DebtAmount,
                    DebtDueDate = userDebt.DebtDueDate,
                    DebtRepaymentPercentage = userDebt.DebtRepaymentPercentage,
                    IsComplete = userDebt.IsComplete,
                    IsDeleted = userDebt.IsDeleted
                };

                return Ok(userDebtDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        [HttpPut("Admin/{id}/{requestId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateUserDebtAdmin(int id, int requestId, [FromBody] UserDebtDto userDebtDto)
        {
            try
            {

                if (id <= 0 || requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userDebt = _context.UserDebts.FirstOrDefault(debt => debt.RequestId == requestId && debt.Id == id && !debt.IsDeleted);
                if (userDebt == null)
                {
                    return NotFound(new { message = ResponseMessages.UserDebtNotFound });
                }

                userDebt.DebtTitle = userDebtDto.DebtTitle;
                userDebt.DebtAmount = userDebtDto.DebtAmount;
                userDebt.DebtDueDate = userDebtDto.DebtDueDate;
                userDebt.DebtRepaymentPercentage = userDebtDto.DebtRepaymentPercentage;

                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        [HttpDelete("Admin/{id}/{requestId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteUserDebtAdmin(int id, int requestId)
        {
            try
            {
                if (id <= 0 || requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userDebt = _context.UserDebts.FirstOrDefault(debt => debt.RequestId == requestId && debt.Id == id && !debt.IsDeleted);
                if (userDebt == null)
                {
                    return NotFound(new { message = ResponseMessages.UserDebtNotFound });
                }

                userDebt.IsDeleted = true;
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ClearUserDebts()
        {
            try
            {
                _context.UserDebts.RemoveRange(_context.UserDebts);
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
