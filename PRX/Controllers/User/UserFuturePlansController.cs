using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.User;
using PRX.Models.User;

namespace PRX.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "UserFuturePlans")]
    public class UserFuturePlansController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public UserFuturePlansController(PRXDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllUserFuturePlans()
        {
            var userFuturePlans = _context.UserFuturePlans.ToList();
            return Ok(userFuturePlans);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserFuturePlansById(int id)
        {
            var userFuturePlans = _context.UserFuturePlans.Find(id);
            if (userFuturePlans == null)
            {
                return NotFound();
            }
            return Ok(userFuturePlans);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateUserFuturePlans([FromBody] UserFuturePlansDto userFuturePlansDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userFuturePlans = new UserFuturePlans
            {
                UserId = userFuturePlansDto.UserId,
                Description = userFuturePlansDto.Description
            };

            _context.UserFuturePlans.Add(userFuturePlans);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUserFuturePlansById), new { id = userFuturePlans.Id }, userFuturePlans);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserFuturePlans(int id, [FromBody] UserFuturePlansDto userFuturePlansDto)
        {
            var userFuturePlans = _context.UserFuturePlans.Find(id);
            if (userFuturePlans == null)
            {
                return NotFound();
            }

            userFuturePlans.UserId = userFuturePlansDto.UserId;
            userFuturePlans.Description = userFuturePlansDto.Description;

            _context.SaveChanges();

            return Ok(userFuturePlans);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserFuturePlans(int id)
        {
            var userFuturePlans = _context.UserFuturePlans.Find(id);
            if (userFuturePlans == null)
            {
                return NotFound();
            }

            _context.UserFuturePlans.Remove(userFuturePlans);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult ClearUserFuturePlans()
        {
            _context.UserFuturePlans.RemoveRange(_context.UserFuturePlans);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
