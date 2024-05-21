using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using PRX.Data;
using PRX.Dto.User;
using PRX.Models.User;
using PRX.Utils;

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
                    UserId = userWithdrawal.UserId,
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

        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserWithdrawalById(int id)
        {

            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Ensure that the user is updating their own profile
                if (id != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }
                var userWithdrawal = _context.UserWithdrawals.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userWithdrawal == null)
                {
                    return NotFound(new { message = ResponseMessages.UserWithdrawlNotFound });
                }
                var userWithdrawalDto = new UserWithdrawalDto
                {
                    Id = userWithdrawal.Id,
                    UserId = userWithdrawal.UserId,
                    WithdrawalAmount = userWithdrawal.WithdrawalAmount,
                    WithdrawalDate = userWithdrawal.WithdrawalDate,
                    WithdrawalReason = userWithdrawal.WithdrawalReason
                };
                return Ok(userWithdrawalDto);

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
                    UserId = userWithdrawalDto.UserId,
                    WithdrawalAmount = userWithdrawalDto.WithdrawalAmount,
                    WithdrawalDate = userWithdrawalDto.WithdrawalDate,
                    WithdrawalReason = userWithdrawalDto.WithdrawalReason
                };

                _context.UserWithdrawals.Add(userWithdrawal);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetUserWithdrawalById), new { id = userWithdrawal.Id }, userWithdrawal);
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

                // Ensure that the user is updating their own profile
                if (id != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }
                var userWithdrawal = _context.UserWithdrawals.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
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

                // Ensure that the user is updating their own profile
                if (id != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }
                var userWithdrawal = _context.UserWithdrawals.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
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

                var userWithdrawl = _context.UserWithdrawals.FirstOrDefault(u => u.UserId == id);
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
                if (id != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }


                var record = _context.UserWithdrawals.FirstOrDefault(e => e.UserId == id);
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
                    UserId = withdrawal.UserId,
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
                    UserId = withdrawal.UserId,
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

                withdrawal.UserId = withdrawalDto.UserId;
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
