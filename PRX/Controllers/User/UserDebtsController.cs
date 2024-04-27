using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.User;
using PRX.Models.User;

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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllUserDebts()
        {
            var userDebts = _context.UserDebts;
            return Ok(userDebts);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserDebtById(int id)
        {
            var userDebt = _context.UserDebts.Find(id);
            if (userDebt == null)
            {
                return NotFound();
            }
            return Ok(userDebt);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateUserDebt([FromBody] UserDebtDto userDebtDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userDebt = new UserDebt
            {
                UserId = userDebtDto.UserId,
                DebtTitle = userDebtDto.DebtTitle,
                DebtAmount = userDebtDto.DebtAmount,
                DebtDueDate = userDebtDto.DebtDueDate,
                DebtRepaymentPercentage = userDebtDto.DebtRepaymentPercentage
            };

            _context.UserDebts.Add(userDebt);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUserDebtById), new { id = userDebt.Id }, userDebt);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserDebt(int id, [FromBody] UserDebtDto userDebtDto)
        {
            var userDebt = _context.UserDebts.Find(id);
            if (userDebt == null)
            {
                return NotFound();
            }

            userDebt.UserId = userDebtDto.UserId;
            userDebt.DebtTitle = userDebtDto.DebtTitle;
            userDebt.DebtAmount = userDebtDto.DebtAmount;
            userDebt.DebtDueDate = userDebtDto.DebtDueDate;
            userDebt.DebtRepaymentPercentage = userDebtDto.DebtRepaymentPercentage;

            _context.SaveChanges();

            return Ok(userDebt);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserDebt(int id)
        {
            var userDebt = _context.UserDebts.Find(id);
            if (userDebt == null)
            {
                return NotFound();
            }

            _context.UserDebts.Remove(userDebt);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult ClearUserDebts()
        {
            _context.UserDebts.RemoveRange(_context.UserDebts);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
