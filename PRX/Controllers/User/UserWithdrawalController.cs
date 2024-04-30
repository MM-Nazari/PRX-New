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
        [Authorize]
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
                Console.WriteLine($"Exception occurred: {ex}");
                return BadRequest(new { Message = "Failed to update user profile." });
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
        [Authorize]
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
                Console.WriteLine($"Exception occurred: {ex}");
                return BadRequest(new { Message = "Failed to update user profile." });
            }


        }

        [HttpDelete("{id}")]
        [Authorize]
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
                Console.WriteLine($"Exception occurred: {ex}");
                return BadRequest(new { Message = "Failed to update user profile." });
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
    }
}
