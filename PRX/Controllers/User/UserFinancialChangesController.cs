using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserFinancialChangesById(int id)
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
                var userFinancialChanges = _context.UserFinancialChanges.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userFinancialChanges == null)
                {
                    return NotFound();
                }
                return Ok(userFinancialChanges);

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
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserFinancialChanges(int id, [FromBody] UserFinancialChangesDto userFinancialChangesDto)
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
                var userFinancialChanges = _context.UserFinancialChanges.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userFinancialChanges == null)
                {
                    return NotFound();
                }

                userFinancialChanges.UserId = userFinancialChangesDto.UserId;
                userFinancialChanges.Description = userFinancialChangesDto.Description;

                _context.SaveChanges();

                return Ok(userFinancialChanges);

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
        public IActionResult DeleteUserFinancialChanges(int id)
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
                var userFinancialChanges = _context.UserFinancialChanges.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userFinancialChanges == null)
                {
                    return NotFound();
                }

                userFinancialChanges.IsDeleted = true;
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
        public IActionResult ClearUserFinancialChanges()
        {
            _context.UserFinancialChanges.RemoveRange(_context.UserFinancialChanges);
            _context.SaveChanges();

            return NoContent();
        }

        // PUT: api/HaghighiUserProfile/complete/{id}
        [HttpPut("complete/{id}")]
        //[Authorize(Roles = "Admin")] // Assuming only admins can mark profiles as complete
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkFinancwChangeAsComplete(int id)
        {
            var userFinancialChanges = _context.UserFinancialChanges.FirstOrDefault(u => u.UserId == id);
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
