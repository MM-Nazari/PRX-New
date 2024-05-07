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
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserInvestmentById(int id)
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
                var userInvestment = _context.UserInvestments.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userInvestment == null)
                {
                    return NotFound();
                }
                return Ok(userInvestment);

            }

            catch (Exception ex)
            {
                
                return BadRequest();
            }


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
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserInvestment(int id, [FromBody] UserInvestmentDto userInvestmentDto)
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
                var userInvestment = _context.UserInvestments.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userInvestment == null)
                {
                    return NotFound();
                }

                userInvestment.UserId = userInvestmentDto.UserId;
                userInvestment.Amount = userInvestmentDto.Amount;

                _context.SaveChanges();

                return Ok(userInvestment);

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
        public IActionResult DeleteUserInvestment(int id)
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
                var userInvestment = _context.UserInvestments.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userInvestment == null)
                {
                    return NotFound();
                }

                userInvestment.IsDeleted = true;
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
        public IActionResult ClearUserInvestments()
        {
            _context.UserInvestments.RemoveRange(_context.UserInvestments);
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
            var userFinancialChanges = _context.UserInvestments.FirstOrDefault(u => u.UserId == id);
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
        public IActionResult GetAllUserInvestmentsAdmin()
        {
            var investments = _context.UserInvestments.ToList();
            var investmentDtos = investments.Select(investment => new UserInvestmentDto
            {
                UserId = investment.UserId,
                Amount = investment.Amount,
                IsComplete = investment.IsComplete,
                IsDeleted = investment.IsDeleted
            }).ToList();
            return Ok(investmentDtos);
        }

        [HttpGet("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserInvestmentByIdAdmin(int id)
        {
            var investment = _context.UserInvestments.FirstOrDefault(inv => inv.UserId == id && !inv.IsDeleted);
            if (investment == null)
            {
                return NotFound();
            }

            var investmentDto = new UserInvestmentDto
            {
                UserId = investment.UserId,
                Amount = investment.Amount,
                IsComplete = investment.IsComplete,
                IsDeleted = investment.IsDeleted
            };

            return Ok(investmentDto);
        }

        [HttpPut("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserInvestmentAdmin(int id, [FromBody] UserInvestmentDto investmentDto)
        {
            var investment = _context.UserInvestments.FirstOrDefault(inv => inv.UserId == id && !inv.IsDeleted);
            if (investment == null)
            {
                return NotFound();
            }

            investment.Amount = investmentDto.Amount;

            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserInvestmentAdmin(int id)
        {
            var investment = _context.UserInvestments.FirstOrDefault(inv => inv.UserId == id && !inv.IsDeleted);
            if (investment == null)
            {
                return NotFound();
            }

            investment.IsDeleted = true;
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("Admin/Clear")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearUserInvestmentsAdmin()
        {
            _context.UserInvestments.RemoveRange(_context.UserInvestments);
            _context.SaveChanges();

            return Ok();
        }

    }
}
