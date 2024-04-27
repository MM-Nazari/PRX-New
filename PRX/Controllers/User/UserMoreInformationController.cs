using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.User;
using PRX.Models.User;

namespace PRX.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "UserMoreInformations")]
    public class UserMoreInformationController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public UserMoreInformationController(PRXDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllUserMoreInformation()
        {
            var userMoreInformation = _context.UserMoreInformations.ToList();
            return Ok(userMoreInformation);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserMoreInformationById(int id)
        {
            var userMoreInformation = _context.UserMoreInformations.Find(id);
            if (userMoreInformation == null)
            {
                return NotFound();
            }
            return Ok(userMoreInformation);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateUserMoreInformation([FromBody] UserMoreInformationDto userMoreInformationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userMoreInformation = new UserMoreInformation
            {
                UserId = userMoreInformationDto.UserId,
                Info = userMoreInformationDto.Info
            };

            _context.UserMoreInformations.Add(userMoreInformation);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUserMoreInformationById), new { id = userMoreInformation.Id }, userMoreInformation);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserMoreInformation(int id, [FromBody] UserMoreInformationDto userMoreInformationDto)
        {
            var userMoreInformation = _context.UserMoreInformations.Find(id);
            if (userMoreInformation == null)
            {
                return NotFound();
            }

            userMoreInformation.UserId = userMoreInformationDto.UserId;
            userMoreInformation.Info = userMoreInformationDto.Info;

            _context.SaveChanges();

            return Ok(userMoreInformation);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserMoreInformation(int id)
        {
            var userMoreInformation = _context.UserMoreInformations.Find(id);
            if (userMoreInformation == null)
            {
                return NotFound();
            }

            _context.UserMoreInformations.Remove(userMoreInformation);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult ClearUserMoreInformations()
        {
            _context.UserMoreInformations.RemoveRange(_context.UserMoreInformations);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
