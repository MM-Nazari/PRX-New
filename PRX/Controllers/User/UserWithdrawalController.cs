using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using PRX.Data;
using PRX.Dto.User;
using PRX.Models.User;
using PRX.Utils;
using Azure.Core;
using DocumentFormat.OpenXml.Wordprocessing;

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
                    //Id = userWithdrawal.Id,
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
                //var userWithdrawalDto = new UserWithdrawalDto
                //{
                //    Id = userWithdrawal.Id,
                //    RequestId = userWithdrawal.RequestId,
                //    WithdrawalAmount = userWithdrawal.WithdrawalAmount,
                //    WithdrawalDate = userWithdrawal.WithdrawalDate,
                //    WithdrawalReason = userWithdrawal.WithdrawalReason
                //};
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
        public IActionResult CreateUserWithdrawal([FromBody] UserWithdrawalDto userWithdrawalDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userWithdrawal = new UserWithdrawal
                {
                    RequestId = userWithdrawalDto.RequestId,
                    WithdrawalAmount = userWithdrawalDto.WithdrawalAmount,
                    WithdrawalDate = userWithdrawalDto.WithdrawalDate,
                    WithdrawalReason = userWithdrawalDto.WithdrawalReason
                };

                _context.UserWithdrawals.Add(userWithdrawal);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetUserWithdrawalById), new { requestId = userWithdrawal.Id }, userWithdrawal);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserWithdrawal(int id, [FromBody] UserWithdrawalDto userWithdrawalDto)
        {

            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Fetch the request
                var request = _context.Requests.FirstOrDefault(r => r.Id == id);

                if (request == null)
                {
                    return NotFound(new { message = ResponseMessages.RequestNotFound });
                }

                // Ensure that the user associated with the request matches the token user ID
                if (request.UserId != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }

                var userWithdrawal = _context.UserWithdrawals.FirstOrDefault(u => u.RequestId == id && !u.IsDeleted);
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserWithdrawal(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Fetch the request
                var request = _context.Requests.FirstOrDefault(r => r.Id == id);

                if (request == null)
                {
                    return NotFound(new { message = ResponseMessages.RequestNotFound });
                }

                // Ensure that the user associated with the request matches the token user ID
                if (request.UserId != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }

                var userWithdrawal = _context.UserWithdrawals.FirstOrDefault(u => u.RequestId == id && !u.IsDeleted);
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
        [HttpPut("complete/{id}")]
        //[Authorize(Roles = "Admin")] // Assuming only admins can mark profiles as complete
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkFinancwChangeAsComplete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userWithdrawl = _context.UserWithdrawals.FirstOrDefault(u => u.RequestId == id);
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

        [HttpGet("isComplete/{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CheckCompletionStatus(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);
                // Fetch the request
                var request = _context.Requests.FirstOrDefault(r => r.Id == id);

                if (request == null)
                {
                    return NotFound(new { message = ResponseMessages.RequestNotFound });
                }

                // Ensure that the user associated with the request matches the token user ID
                if (request.UserId != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }


                var record = _context.UserWithdrawals.FirstOrDefault(e => e.RequestId == id);
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
                    //Id = withdrawal.Id,
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
                    //Id = withdrawal.Id,
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
