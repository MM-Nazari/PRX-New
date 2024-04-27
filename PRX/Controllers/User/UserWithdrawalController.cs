using Microsoft.AspNetCore.Mvc;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserWithdrawalById(int id)
        {
            var userWithdrawal = _context.UserWithdrawals.FirstOrDefault(u => u.Id == id);
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserWithdrawal(int id, [FromBody] UserWithdrawalDto userWithdrawalDto)
        {
            var userWithdrawal = _context.UserWithdrawals.FirstOrDefault(u => u.Id == id);
            if (userWithdrawal == null)
            {
                return NotFound();
            }

            userWithdrawal.UserId = userWithdrawalDto.UserId;
            userWithdrawal.WithdrawalAmount = userWithdrawalDto.WithdrawalAmount;
            userWithdrawal.WithdrawalDate = userWithdrawalDto.WithdrawalDate;
            userWithdrawal.WithdrawalReason = userWithdrawalDto.WithdrawalReason;

            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserWithdrawal(int id)
        {
            var userWithdrawal = _context.UserWithdrawals.FirstOrDefault(u => u.Id == id);
            if (userWithdrawal == null)
            {
                return NotFound();
            }

            _context.UserWithdrawals.Remove(userWithdrawal);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearUserWithdrawals()
        {
            _context.UserWithdrawals.RemoveRange(_context.UserWithdrawals);
            _context.SaveChanges();
            return Ok();
        }
    }
}
