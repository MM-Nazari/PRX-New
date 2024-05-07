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
        [Authorize(Roles = "User")]
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
                
                return BadRequest();
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
        [Authorize(Roles = "User")]
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
                
                return BadRequest();
            }

           
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
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
               
                return BadRequest();
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


        [HttpGet("Admin")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAllUserFinancialChangesAdmin()
        {
            var financialChanges = _context.UserFinancialChanges.ToList();
            var financialChangeDtos = financialChanges.Select(change => new UserFinancialChangesDto
            {
                UserId = change.UserId,
                Description = change.Description,
                IsComplete = change.IsComplete,
                IsDeleted = change.IsDeleted
            }).ToList();
            return Ok(financialChangeDtos);
        }

        [HttpGet("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserFinancialChangeByIdAdmin(int id)
        {
            var financialChange = _context.UserFinancialChanges.FirstOrDefault(change => change.UserId == id && !change.IsDeleted);
            if (financialChange == null)
            {
                return NotFound();
            }

            var financialChangeDto = new UserFinancialChangesDto
            {
                UserId = financialChange.UserId,
                Description = financialChange.Description,
                IsComplete = financialChange.IsComplete,
                IsDeleted = financialChange.IsDeleted
            };

            return Ok(financialChangeDto);
        }

        [HttpPut("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserFinancialChangeAdmin(int id, [FromBody] UserFinancialChangesDto financialChangeDto)
        {
            var financialChange = _context.UserFinancialChanges.FirstOrDefault(change => change.UserId == id && !change.IsDeleted);
            if (financialChange == null)
            {
                return NotFound();
            }

            financialChange.Description = financialChangeDto.Description;
            //financialChange.IsComplete = financialChangeDto.IsComplete;
            //financialChange.IsDeleted = financialChangeDto.IsDeleted;

            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserFinancialChangeAdmin(int id)
        {
            var financialChange = _context.UserFinancialChanges.FirstOrDefault(change => change.UserId == id && !change.IsDeleted);
            if (financialChange == null)
            {
                return NotFound();
            }

            financialChange.IsDeleted = true;
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("Admin/UserFinancialChanges/Clear")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearUserFinancialChangesAdmin()
        {
            _context.UserFinancialChanges.RemoveRange(_context.UserFinancialChanges);
            _context.SaveChanges();

            return Ok();
        }


    }
}
