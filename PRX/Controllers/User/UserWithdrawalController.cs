using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using PRX.Data;
using PRX.Dto.User;
using PRX.Models.User;

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

        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserWithdrawalById(int id)
        {

            try
            {

                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Ensure that the user is updating their own profile
                if (id != tokenUserId)
                {
                    return Forbid(); // Or return 403 Forbidden
                }
                var userWithdrawal = _context.UserWithdrawals.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userWithdrawal == null)
                {
                    return NotFound();
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
                
                return BadRequest();
            }


        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateUserWithdrawal([FromBody] UserWithdrawalDto userWithdrawalDto)
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

        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserWithdrawal(int id, [FromBody] UserWithdrawalDto userWithdrawalDto)
        {

            try
            {

                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Ensure that the user is updating their own profile
                if (id != tokenUserId)
                {
                    return Forbid(); // Or return 403 Forbidden
                }
                var userWithdrawal = _context.UserWithdrawals.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userWithdrawal == null)
                {
                    return NotFound();
                }

                
                userWithdrawal.WithdrawalAmount = userWithdrawalDto.WithdrawalAmount;
                userWithdrawal.WithdrawalDate = userWithdrawalDto.WithdrawalDate;
                userWithdrawal.WithdrawalReason = userWithdrawalDto.WithdrawalReason;

                _context.SaveChanges();

                return Ok();

            }

            catch (Exception ex)
            {
                return BadRequest();
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

                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Ensure that the user is updating their own profile
                if (id != tokenUserId)
                {
                    return Forbid(); // Or return 403 Forbidden
                }
                var userWithdrawal = _context.UserWithdrawals.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userWithdrawal == null)
                {
                    return NotFound();
                }

                userWithdrawal.IsDeleted = true;
                _context.SaveChanges();

                return Ok();

            }

            catch (Exception ex)
            {
                return BadRequest();
            }


        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearUserWithdrawals()
        {
            _context.UserWithdrawals.RemoveRange(_context.UserWithdrawals);
            _context.SaveChanges();
            return Ok();
        }


        // PUT: api/HaghighiUserProfile/complete/{id}
        [HttpPut("complete/{id}")]
        //[Authorize(Roles = "Admin")] // Assuming only admins can mark profiles as complete
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkFinancwChangeAsComplete(int id)
        {
            var userFinancialChanges = _context.UserWithdrawals.FirstOrDefault(u => u.UserId == id);
            if (userFinancialChanges == null)
            {
                return NotFound();
            }

            userFinancialChanges.IsComplete = true;
            _context.SaveChanges();

            return Ok();
        }

        [HttpGet("Admin")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAllUserWithdrawalsAdmin()
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

        [HttpGet("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserWithdrawalByIdAdmin(int id)
        {
            var withdrawal = _context.UserWithdrawals.FirstOrDefault(w => w.Id == id && !w.IsDeleted);
            if (withdrawal == null)
            {
                return NotFound();
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

        [HttpPut("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserWithdrawalAdmin(int id, [FromBody] UserWithdrawalDto withdrawalDto)
        {
            var withdrawal = _context.UserWithdrawals.FirstOrDefault(w => w.Id == id && !w.IsDeleted);
            if (withdrawal == null)
            {
                return NotFound();
            }

            withdrawal.UserId = withdrawalDto.UserId;
            withdrawal.WithdrawalAmount = withdrawalDto.WithdrawalAmount;
            withdrawal.WithdrawalDate = withdrawalDto.WithdrawalDate;
            withdrawal.WithdrawalReason = withdrawalDto.WithdrawalReason;

            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserWithdrawalAdmin(int id)
        {
            var withdrawal = _context.UserWithdrawals.FirstOrDefault(w => w.Id == id && !w.IsDeleted);
            if (withdrawal == null)
            {
                return NotFound();
            }

            withdrawal.IsDeleted = true;
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("Admin/Clear")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearUserWithdrawalsAdmin()
        {
            _context.UserWithdrawals.RemoveRange(_context.UserWithdrawals);
            _context.SaveChanges();

            return Ok();
        }

    }
}
