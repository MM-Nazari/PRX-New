using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserDebtById(int id)
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
                var userDebt = _context.UserDebts.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userDebt == null)
                {
                    return NotFound();
                }
                return Ok(userDebt);

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
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserDebt(int id, [FromBody] UserDebtDto userDebtDto)
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
                var userDebt = _context.UserDebts.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
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

            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex}");
                return BadRequest(new { Message = "Failed to update user profile." });
            }


        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserDebt(int id)
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
                var userDebt = _context.UserDebts.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userDebt == null)
                {
                    return NotFound();
                }

                userDebt.IsDeleted = true;
                _context.SaveChanges();

                return NoContent();

            }

            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex}");
                return BadRequest(new { Message = "Failed to update user profile." });
            }

        }

        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult ClearUserDebts()
        {
            _context.UserDebts.RemoveRange(_context.UserDebts);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut("complete/{id}")]
        //[Authorize(Roles = "Admin")] // Assuming only admins can mark profiles as complete
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkCompaniesAsComplete(int id)
        {
            var record = _context.UserDebts.FirstOrDefault(e => e.UserId == id);
            if (record == null)
            {
                return NotFound();
            }

            record.IsComplete = true;
            _context.SaveChanges();

            return Ok();
        }

    }
}
