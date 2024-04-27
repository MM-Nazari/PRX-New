using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.User;
using PRX.Models.User;

namespace PRX.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "UserFinancialChanges")]
    public class UserFinancialChangesController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public UserFinancialChangesController(PRXDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllUserFinancialChanges()
        {
            var userFinancialChanges = _context.UserFinancialChanges.ToList();
            return Ok(userFinancialChanges);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserFinancialChangesById(int id)
        {
            var userFinancialChanges = _context.UserFinancialChanges.Find(id);
            if (userFinancialChanges == null)
            {
                return NotFound();
            }
            return Ok(userFinancialChanges);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateUserFinancialChanges([FromBody] UserFinancialChangesDto userFinancialChangesDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userFinancialChanges = new UserFinancialChanges
            {
                UserId = userFinancialChangesDto.UserId,
                Description = userFinancialChangesDto.Description
            };

            _context.UserFinancialChanges.Add(userFinancialChanges);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUserFinancialChangesById), new { id = userFinancialChanges.Id }, userFinancialChanges);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserFinancialChanges(int id, [FromBody] UserFinancialChangesDto userFinancialChangesDto)
        {
            var userFinancialChanges = _context.UserFinancialChanges.Find(id);
            if (userFinancialChanges == null)
            {
                return NotFound();
            }

            userFinancialChanges.UserId = userFinancialChangesDto.UserId;
            userFinancialChanges.Description = userFinancialChangesDto.Description;

            _context.SaveChanges();

            return Ok(userFinancialChanges);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserFinancialChanges(int id)
        {
            var userFinancialChanges = _context.UserFinancialChanges.Find(id);
            if (userFinancialChanges == null)
            {
                return NotFound();
            }

            _context.UserFinancialChanges.Remove(userFinancialChanges);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult ClearUserFinancialChanges()
        {
            _context.UserFinancialChanges.RemoveRange(_context.UserFinancialChanges);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
