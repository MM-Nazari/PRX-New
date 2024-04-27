using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.User;
using PRX.Models.User;

namespace PRX.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "UserInvestments")]
    public class UserInvestmentController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public UserInvestmentController(PRXDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllUserInvestments()
        {
            var userInvestments = _context.UserInvestments.ToList();
            return Ok(userInvestments);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserInvestmentById(int id)
        {
            var userInvestment = _context.UserInvestments.Find(id);
            if (userInvestment == null)
            {
                return NotFound();
            }
            return Ok(userInvestment);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateUserInvestment([FromBody] UserInvestmentDto userInvestmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userInvestment = new UserInvestment
            {
                UserId = userInvestmentDto.UserId,
                Amount = userInvestmentDto.Amount
            };

            _context.UserInvestments.Add(userInvestment);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUserInvestmentById), new { id = userInvestment.Id }, userInvestment);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserInvestment(int id, [FromBody] UserInvestmentDto userInvestmentDto)
        {
            var userInvestment = _context.UserInvestments.Find(id);
            if (userInvestment == null)
            {
                return NotFound();
            }

            userInvestment.UserId = userInvestmentDto.UserId;
            userInvestment.Amount = userInvestmentDto.Amount;

            _context.SaveChanges();

            return Ok(userInvestment);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserInvestment(int id)
        {
            var userInvestment = _context.UserInvestments.Find(id);
            if (userInvestment == null)
            {
                return NotFound();
            }

            _context.UserInvestments.Remove(userInvestment);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult ClearUserInvestments()
        {
            _context.UserInvestments.RemoveRange(_context.UserInvestments);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
