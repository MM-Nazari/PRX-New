using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.User;
using PRX.Models.User;

namespace PRX.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "UserDeposits")]
    public class UserDepositsController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public UserDepositsController(PRXDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllUserDeposits()
        {
            var userDeposits = _context.UserDeposits.ToList();
            return Ok(userDeposits);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserDepositById(int id)
        {
            var userDeposit = _context.UserDeposits.Find(id);
            if (userDeposit == null)
            {
                return NotFound();
            }
            return Ok(userDeposit);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateUserDeposit([FromBody] UserDepositDto userDepositDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userDeposit = new UserDeposit
            {
                UserId = userDepositDto.UserId,
                DepositAmount = userDepositDto.DepositAmount,
                DepositDate = userDepositDto.DepositDate,
                DepositSource = userDepositDto.DepositSource
            };

            _context.UserDeposits.Add(userDeposit);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUserDepositById), new { id = userDeposit.Id }, userDeposit);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserDeposit(int id, [FromBody] UserDepositDto userDepositDto)
        {
            var userDeposit = _context.UserDeposits.Find(id);
            if (userDeposit == null)
            {
                return NotFound();
            }

            userDeposit.UserId = userDepositDto.UserId;
            userDeposit.DepositAmount = userDepositDto.DepositAmount;
            userDeposit.DepositDate = userDepositDto.DepositDate;
            userDeposit.DepositSource = userDepositDto.DepositSource;

            _context.SaveChanges();

            return Ok(userDeposit);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserDeposit(int id)
        {
            var userDeposit = _context.UserDeposits.Find(id);
            if (userDeposit == null)
            {
                return NotFound();
            }

            _context.UserDeposits.Remove(userDeposit);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult ClearUserDeposits()
        {
            _context.UserDeposits.RemoveRange(_context.UserDeposits);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
