﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using PRX.Data;
using PRX.Dto.User;
using PRX.Models.User;
using PRX.Utils;
using Azure.Core;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.JsonPatch;

namespace PRX.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "UserWithdrawals")]
    public class UserWithdrawalsController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public UserWithdrawalsController(PRXDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllUserWithdrawals()
        {
            try
            {

                var userWithdrawals = _context.UserWithdrawals.ToList();
                var userWithdrawalDtos = userWithdrawals.Select(userWithdrawal => new UserWithdrawalDto
                {
                    Id = userWithdrawal.Id,
                    RequestId = userWithdrawal.RequestId,
                    WithdrawalAmount = userWithdrawal.WithdrawalAmount,
                    WithdrawalDate = userWithdrawal.WithdrawalDate,
                    WithdrawalReason = userWithdrawal.WithdrawalReason
                }).ToList();
                return Ok(userWithdrawalDtos);
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
        public IActionResult GetUserWithdrawalById(int requestId)
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

                var userWithdrawal = _context.UserWithdrawals.Where(u => u.RequestId == requestId && !u.IsDeleted).Select(r => new UserWithdrawalDto 
                {
                    Id = r.Id,
                    RequestId = r.RequestId,
                    WithdrawalAmount = r.WithdrawalAmount,
                    WithdrawalDate = r.WithdrawalDate,
                    WithdrawalReason = r.WithdrawalReason,
                    IsComplete = r.IsComplete,
                    IsDeleted = r.IsDeleted
                }
                ).ToList();


                if (userWithdrawal == null)
                {
                    return NotFound(new { message = ResponseMessages.UserWithdrawlNotFound });
                }

                return Ok(userWithdrawal);

            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateUserWithdrawal([FromBody] UserWithdrawalListDto userWithdrawalDto)
        {
            try
            {
                if (userWithdrawalDto.RequestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                foreach (var withdrawl in userWithdrawalDto.withdrawlList) 
                {
                    var userWithdrawal = new UserWithdrawal
                    {
                        RequestId = userWithdrawalDto.RequestId,
                        WithdrawalAmount = withdrawl.WithdrawalAmount,
                        WithdrawalDate = withdrawl.WithdrawalDate,
                        WithdrawalReason = withdrawl.WithdrawalReason
                    };
                    _context.UserWithdrawals.Add(userWithdrawal);
                }



                _context.SaveChanges();

                return StatusCode(StatusCodes.Status201Created, new { message = ResponseMessages.OK });
                //return CreatedAtAction(nameof(GetUserWithdrawalById), new { requestId = userWithdrawal.Id }, userWithdrawal);
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
        public IActionResult UpdateUserWithdrawal(int id, int requestId, [FromBody] UserWithdrawalDto userWithdrawalDto)
        {
            try
            {
                if (id <= 0 || requestId <= 0)
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

                var userWithdrawal = _context.UserWithdrawals.FirstOrDefault(u => u.Id == id && u.RequestId == requestId && !u.IsDeleted);
                if (userWithdrawal == null)
                {
                    return NotFound(new { message = ResponseMessages.UserWithdrawlNotFound });
                }

                userWithdrawal.WithdrawalAmount = userWithdrawalDto.WithdrawalAmount;
                userWithdrawal.WithdrawalDate = userWithdrawalDto.WithdrawalDate;
                userWithdrawal.WithdrawalReason = userWithdrawalDto.WithdrawalReason;

                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        // PATCH: api/UserWithdrawal/Patch/{id}/{requestId}
        [HttpPatch("{id}/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult PatchUserWithdrawal(int id, int requestId, [FromBody] JsonPatchDocument<UserWithdrawalDto> patchDoc)
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

                var userWithdrawal = _context.UserWithdrawals.FirstOrDefault(u => u.Id == id && u.RequestId == requestId && !u.IsDeleted);
                if (userWithdrawal == null)
                {
                    return NotFound(new { message = ResponseMessages.UserWithdrawlNotFound });
                }

                // Create a DTO to hold the current user withdrawal
                var userWithdrawalDto = new UserWithdrawalDto
                {
                    WithdrawalAmount = userWithdrawal.WithdrawalAmount,
                    WithdrawalDate = userWithdrawal.WithdrawalDate,
                    WithdrawalReason = userWithdrawal.WithdrawalReason
                };

                // Apply the patch document to the DTO
                patchDoc.ApplyTo(userWithdrawalDto, ModelState);

                // Validate the model after applying the patch
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Update the user withdrawal properties based on the modified DTO
                userWithdrawal.WithdrawalAmount = userWithdrawalDto.WithdrawalAmount; // Update if present
                userWithdrawal.WithdrawalDate = userWithdrawalDto.WithdrawalDate; // Update if present
                userWithdrawal.WithdrawalReason = userWithdrawalDto.WithdrawalReason; // Update if present

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserWithdrawal(int id, int requestId)
        {
            try
            {
                if (id <= 0 || requestId <= 0)
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

                var userWithdrawal = _context.UserWithdrawals.FirstOrDefault(u => u.RequestId == requestId && u.Id == id && !u.IsDeleted);
                if (userWithdrawal == null)
                {
                    return NotFound(new { message = ResponseMessages.UserWithdrawlNotFound });
                }

                userWithdrawal.IsDeleted = true;
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
        public IActionResult ClearUserWithdrawals()
        {
            try
            {
                _context.UserWithdrawals.RemoveRange(_context.UserWithdrawals);
                _context.SaveChanges();
                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }


        // PUT: api/HaghighiUserProfile/complete/{id}
        [HttpPut("complete/{id}/{requestId}")]
        //[Authorize(Roles = "Admin")] // Assuming only admins can mark profiles as complete
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkFinancwChangeAsComplete(int id, int requestId)
        {
            try
            {
                if (id <= 0 || requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userWithdrawl = _context.UserWithdrawals.FirstOrDefault(u => u.RequestId == requestId && u.Id == id && !u.IsDeleted);
                if (userWithdrawl == null)
                {
                    return NotFound(new { message = ResponseMessages.UserWithdrawlNotFound });
                }

                userWithdrawl.IsComplete = true;
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
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


                var record = _context.UserWithdrawals.FirstOrDefault(u => u.RequestId == requestId && u.Id == id && !u.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.UserWithdrawlNotFound });
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
        public IActionResult GetAllUserWithdrawalsAdmin()
        {
            try
            {
                var withdrawals = _context.UserWithdrawals.ToList();
                var withdrawalDtos = withdrawals.Select(withdrawal => new UserWithdrawalDto
                {
                    Id = withdrawal.Id,
                    RequestId = withdrawal.RequestId,
                    WithdrawalAmount = withdrawal.WithdrawalAmount,
                    WithdrawalDate = withdrawal.WithdrawalDate,
                    WithdrawalReason = withdrawal.WithdrawalReason,
                    IsComplete = withdrawal.IsComplete,
                    IsDeleted = withdrawal.IsDeleted
                }).ToList();
                return Ok(withdrawalDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpGet("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserWithdrawalByIdAdmin(int id)
        {
            try
            {

                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var withdrawal = _context.UserWithdrawals.FirstOrDefault(w => w.Id == id && !w.IsDeleted);
                if (withdrawal == null)
                {
                    return NotFound(new { message = ResponseMessages.UserWithdrawlNotFound });
                }

                var withdrawalDto = new UserWithdrawalDto
                {
                    Id = withdrawal.Id,
                    RequestId = withdrawal.RequestId,
                    WithdrawalAmount = withdrawal.WithdrawalAmount,
                    WithdrawalDate = withdrawal.WithdrawalDate,
                    WithdrawalReason = withdrawal.WithdrawalReason,
                    IsComplete = withdrawal.IsComplete,
                    IsDeleted = withdrawal.IsDeleted
                };

                return Ok(withdrawalDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        [HttpPut("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserWithdrawalAdmin(int id, [FromBody] UserWithdrawalDto withdrawalDto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var withdrawal = _context.UserWithdrawals.FirstOrDefault(w => w.Id == id && !w.IsDeleted);
                if (withdrawal == null)
                {
                    return NotFound(new { message = ResponseMessages.UserWithdrawlNotFound });
                }

                withdrawal.RequestId = withdrawalDto.RequestId;
                withdrawal.WithdrawalAmount = withdrawalDto.WithdrawalAmount;
                withdrawal.WithdrawalDate = withdrawalDto.WithdrawalDate;
                withdrawal.WithdrawalReason = withdrawalDto.WithdrawalReason;

                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpDelete("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserWithdrawalAdmin(int id)
        {
            try
            {

                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var withdrawal = _context.UserWithdrawals.FirstOrDefault(w => w.Id == id && !w.IsDeleted);
                if (withdrawal == null)
                {
                    return NotFound(new { message = ResponseMessages.UserWithdrawlNotFound });
                }

                withdrawal.IsDeleted = true;
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
        public IActionResult ClearUserWithdrawalsAdmin()
        {
            try
            {
                _context.UserWithdrawals.RemoveRange(_context.UserWithdrawals);
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
