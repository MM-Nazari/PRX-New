using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.User;
using PRX.Models.User;
using PRX.Utils;

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
            try
            {
                var userFinancialChanges = _context.UserFinancialChanges.ToList();
                return Ok(userFinancialChanges);
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
        public IActionResult GetUserFinancialChangesById(int id)
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
                var userFinancialChanges = _context.UserFinancialChanges.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userFinancialChanges == null)
                {
                    return NotFound(new { message = ResponseMessages.UserFinancialChangeNotFound });
                }
                return Ok(userFinancialChanges);

            }

            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateUserFinancialChanges([FromBody] UserFinancialChangesDto userFinancialChangesDto)
        {
            try
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
        public IActionResult UpdateUserFinancialChanges(int id, [FromBody] UserFinancialChangesDto userFinancialChangesDto)
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
                var userFinancialChanges = _context.UserFinancialChanges.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userFinancialChanges == null)
                {
                    return NotFound(new { message = ResponseMessages.UserFinancialChangeNotFound });
                }

                userFinancialChanges.UserId = userFinancialChangesDto.UserId;
                userFinancialChanges.Description = userFinancialChangesDto.Description;

                _context.SaveChanges();

                return Ok(userFinancialChanges);

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
        public IActionResult DeleteUserFinancialChanges(int id)
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
                var userFinancialChanges = _context.UserFinancialChanges.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userFinancialChanges == null)
                {
                    return NotFound(new { message = ResponseMessages.UserFinancialChangeNotFound });
                }

                userFinancialChanges.IsDeleted = true;
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
        public IActionResult ClearUserFinancialChanges()
        {
            try
            {
                _context.UserFinancialChanges.RemoveRange(_context.UserFinancialChanges);
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

                var userFinancialChanges = _context.UserFinancialChanges.FirstOrDefault(u => u.UserId == id);
                if (userFinancialChanges == null)
                {
                    return NotFound(new { message = ResponseMessages.UserFinancialChangeNotFound });
                }

                userFinancialChanges.IsComplete = true;
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }


        [HttpGet("isComplete/{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CheckCompletionStatus(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);
                if (id != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }


                var record = _context.UserFinancialChanges.FirstOrDefault(e => e.UserId == id);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.UserFinancialChangeNotFound });
                }

                return Ok(new { isComplete = record.IsComplete });
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
        public IActionResult GetAllUserFinancialChangesAdmin()
        {

            try
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        [HttpGet("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserFinancialChangeByIdAdmin(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var financialChange = _context.UserFinancialChanges.FirstOrDefault(change => change.UserId == id && !change.IsDeleted);
                if (financialChange == null)
                {
                    return NotFound(new { message = ResponseMessages.UserFinancialChangeNotFound });
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
        public IActionResult UpdateUserFinancialChangeAdmin(int id, [FromBody] UserFinancialChangesDto financialChangeDto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var financialChange = _context.UserFinancialChanges.FirstOrDefault(change => change.UserId == id && !change.IsDeleted);
                if (financialChange == null)
                {
                    return NotFound(new { message = ResponseMessages.UserFinancialChangeNotFound });
                }

                financialChange.Description = financialChangeDto.Description;
                //financialChange.IsComplete = financialChangeDto.IsComplete;
                //financialChange.IsDeleted = financialChangeDto.IsDeleted;

                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK});
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
        public IActionResult DeleteUserFinancialChangeAdmin(int id)
        {
            try
            {

                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var financialChange = _context.UserFinancialChanges.FirstOrDefault(change => change.UserId == id && !change.IsDeleted);
                if (financialChange == null)
                {
                    return NotFound(new { message = ResponseMessages.UserFinancialChangeNotFound });
                }

                financialChange.IsDeleted = true;
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        [HttpDelete("Admin/UserFinancialChanges/Clear")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearUserFinancialChangesAdmin()
        {
            try
            {
                _context.UserFinancialChanges.RemoveRange(_context.UserFinancialChanges);
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK }); ;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }


    }
}
