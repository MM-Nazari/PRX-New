using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using PRX.Data;
using PRX.Dto.User;
using PRX.Models.User;
using PRX.Utils;

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
            try
            {
                var userInvestments = _context.UserInvestments.ToList();
                return Ok(userInvestments);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserInvestmentById(int id)
        {

            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Ensure that the user is updating their own profile
                if (id != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }
                var userInvestment = _context.UserInvestments.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userInvestment == null)
                {
                    return NotFound(new { message = ResponseMessages.UserInvestmentNotFound});
                }
                return Ok(userInvestment);

            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateUserInvestment([FromBody] UserInvestmentDto userInvestmentDto)
        {

            try
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

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
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Ensure that the user is updating their own profile
                if (id != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }
                var userInvestment = _context.UserInvestments.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userInvestment == null)
                {
                    return NotFound(new { message = ResponseMessages.UserInvestmentNotFound });
                }

                userInvestment.UserId = userInvestmentDto.UserId;
                userInvestment.Amount = userInvestmentDto.Amount;

                _context.SaveChanges();

                return Ok(userInvestment);

            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
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
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Ensure that the user is updating their own profile
                if (id != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }
                var userInvestment = _context.UserInvestments.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userInvestment == null)
                {
                    return NotFound(new { message = ResponseMessages.UserInvestmentNotFound });
                }

                userInvestment.IsDeleted = true;
                _context.SaveChanges();

                return NoContent();

            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult ClearUserInvestments()
        {
            try
            {
                _context.UserInvestments.RemoveRange(_context.UserInvestments);
                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        // PUT: api/HaghighiUserProfile/complete/{id}
        [HttpPut("complete/{id}")]
        //[Authorize(Roles = "Admin")] // Assuming only admins can mark profiles as complete
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkFinancwChangeAsComplete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userInvestments = _context.UserInvestments.FirstOrDefault(u => u.UserId == id);
                if (userInvestments == null)
                {
                    return NotFound(new { message = ResponseMessages.UserInvestmentNotFound });
                }

                userInvestments.IsComplete = true;
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpGet("Admin")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAllUserInvestmentsAdmin()
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpGet("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserInvestmentByIdAdmin(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var investment = _context.UserInvestments.FirstOrDefault(inv => inv.UserId == id && !inv.IsDeleted);
                if (investment == null)
                {
                    return NotFound(new { message = ResponseMessages.UserInvestmentNotFound });
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpPut("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserInvestmentAdmin(int id, [FromBody] UserInvestmentDto investmentDto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var investment = _context.UserInvestments.FirstOrDefault(inv => inv.UserId == id && !inv.IsDeleted);
                if (investment == null)
                {
                    return NotFound(new { message = ResponseMessages.UserInvestmentNotFound });
                }

                investment.Amount = investmentDto.Amount;

                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpDelete("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserInvestmentAdmin(int id)
        {
            try
            {

                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var investment = _context.UserInvestments.FirstOrDefault(inv => inv.UserId == id && !inv.IsDeleted);
                if (investment == null)
                {
                    return NotFound(new { message = ResponseMessages.UserInvestmentNotFound });
                }

                investment.IsDeleted = true;
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        [HttpDelete("Admin/Clear")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearUserInvestmentsAdmin()
        {
            try
            {
                _context.UserInvestments.RemoveRange(_context.UserInvestments);
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

    }
}
