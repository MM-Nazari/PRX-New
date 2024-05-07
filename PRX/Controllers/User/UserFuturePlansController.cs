using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
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
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserFuturePlansById(int id)
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
                var userFuturePlans = _context.UserFuturePlans.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userFuturePlans == null)
                {
                    return NotFound();
                }
                return Ok(userFuturePlans);

            }

            catch (Exception ex)
            {
                
                return BadRequest();
            }



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
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserFuturePlans(int id, [FromBody] UserFuturePlansDto userFuturePlansDto)
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
                var userFuturePlans = _context.UserFuturePlans.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userFuturePlans == null)
                {
                    return NotFound();
                }

                userFuturePlans.UserId = userFuturePlansDto.UserId;
                userFuturePlans.Description = userFuturePlansDto.Description;

                _context.SaveChanges();

                return Ok(userFuturePlans);

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
        public IActionResult DeleteUserFuturePlans(int id)
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
                var userFuturePlans = _context.UserFuturePlans.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userFuturePlans == null)
                {
                    return NotFound();
                }

                userFuturePlans.IsDeleted = true;
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
        public IActionResult ClearUserFuturePlans()
        {
            _context.UserFuturePlans.RemoveRange(_context.UserFuturePlans);
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
            var userFinancialChanges = _context.UserFuturePlans.FirstOrDefault(u => u.UserId == id);
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
        public IActionResult GetAllUserFuturePlansAdmin()
        {
            var futurePlans = _context.UserFuturePlans.ToList();
            var futurePlanDtos = futurePlans.Select(plan => new UserFuturePlansDto
            {
                UserId = plan.UserId,
                Description = plan.Description,
                IsComplete = plan.IsComplete,
                IsDeleted = plan.IsDeleted
            }).ToList();
            return Ok(futurePlanDtos);
        }

        [HttpGet("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserFuturePlansByIdAdmin(int id)
        {
            var futurePlan = _context.UserFuturePlans.FirstOrDefault(plan => plan.UserId == id && !plan.IsDeleted);
            if (futurePlan == null)
            {
                return NotFound();
            }

            var futurePlanDto = new UserFuturePlansDto
            {
                UserId = futurePlan.UserId,
                Description = futurePlan.Description,
                IsComplete = futurePlan.IsComplete,
                IsDeleted = futurePlan.IsDeleted
            };

            return Ok(futurePlanDto);
        }

        [HttpPut("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserFuturePlansAdmin(int id, [FromBody] UserFuturePlansDto futurePlanDto)
        {
            var futurePlan = _context.UserFuturePlans.FirstOrDefault(plan => plan.UserId == id && !plan.IsDeleted);
            if (futurePlan == null)
            {
                return NotFound();
            }

            futurePlan.Description = futurePlanDto.Description;

            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserFuturePlansAdmin(int id)
        {
            var futurePlan = _context.UserFuturePlans.FirstOrDefault(plan => plan.UserId == id && !plan.IsDeleted);
            if (futurePlan == null)
            {
                return NotFound();
            }

            futurePlan.IsDeleted = true;
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("Admin/Clear")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearUserFuturePlansAdmin()
        {
            _context.UserFuturePlans.RemoveRange(_context.UserFuturePlans);
            _context.SaveChanges();

            return Ok();
        }

    }
}
