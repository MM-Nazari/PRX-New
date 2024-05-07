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
    [ApiExplorerSettings(GroupName = "UserDeposits")]
    public class UserDepositsController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public UserDepositsController(PRXDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllUserDeposits()
        {
            var userDeposits = _context.UserDeposits.ToList();
            return Ok(userDeposits);
        }

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserDepositById(int id)
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
                var userDeposit = _context.UserDeposits.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userDeposit == null)
                {
                    return NotFound();
                }
                return Ok(userDeposit);

            }

            catch (Exception ex)
            {
                
                return BadRequest();
            }


        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateUserDeposit([FromBody] UserDepositDto userDepositDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userDeposit = new UserDeposit
            {
                UserId = userDepositDto.UserId,
                DepositAmount = userDepositDto.DepositAmount,
                DepositDate = userDepositDto.DepositDate,
                DepositSource = userDepositDto.DepositSource
            };

            _context.UserDeposits.Add(userDeposit);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUserDepositById), new { id = userDeposit.Id }, userDeposit);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserDeposit(int id, [FromBody] UserDepositDto userDepositDto)
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
                var userDeposit = _context.UserDeposits.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userDeposit == null)
                {
                    return NotFound();
                }

                userDeposit.UserId = userDepositDto.UserId;
                userDeposit.DepositAmount = userDepositDto.DepositAmount;
                userDeposit.DepositDate = userDepositDto.DepositDate;
                userDeposit.DepositSource = userDepositDto.DepositSource;

                _context.SaveChanges();

                return Ok(userDeposit);

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
        public IActionResult DeleteUserDeposit(int id)
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
                var userDeposit = _context.UserDeposits.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userDeposit == null)
                {
                    return NotFound();
                }

                userDeposit.IsDeleted = true;
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
        public IActionResult ClearUserDeposits()
        {
            _context.UserDeposits.RemoveRange(_context.UserDeposits);
            _context.SaveChanges();

            return NoContent();
        }


        [HttpPut("complete/{id}")]
        //[Authorize(Roles = "Admin")] // Assuming only admins can mark profiles as complete
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkCompaniesAsComplete(int id)
        {
            var record = _context.UserDeposits.FirstOrDefault(e => e.UserId == id);
            if (record == null)
            {
                return NotFound();
            }

            record.IsComplete = true;
            _context.SaveChanges();

            return Ok();
        }


        [HttpGet("Admin")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAllUserDepositsAdmin()
        {
            var deposits = _context.UserDeposits.ToList();
            var depositDtos = deposits.Select(deposit => new UserDepositDto
            {
                UserId = deposit.UserId,
                DepositAmount = deposit.DepositAmount,
                DepositDate = deposit.DepositDate,
                DepositSource = deposit.DepositSource,
                IsComplete = deposit.IsComplete,
                IsDeleted = deposit.IsDeleted
            }).ToList();
            return Ok(depositDtos);
        }

        [HttpGet("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserDepositByIdAdmin(int id)
        {
            var deposit = _context.UserDeposits.FirstOrDefault(d => d.UserId == id && !d.IsDeleted);
            if (deposit == null)
            {
                return NotFound();
            }

            var depositDto = new UserDepositDto
            {
                UserId = deposit.UserId,
                DepositAmount = deposit.DepositAmount,
                DepositDate = deposit.DepositDate,
                DepositSource = deposit.DepositSource,
                IsComplete = deposit.IsComplete,
                IsDeleted = deposit.IsDeleted
            };

            return Ok(depositDto);
        }

        [HttpPut("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserDepositAdmin(int id, [FromBody] UserDepositDto depositDto)
        {
            var deposit = _context.UserDeposits.FirstOrDefault(d => d.UserId == id && !d.IsDeleted);
            if (deposit == null)
            {
                return NotFound();
            }

            deposit.DepositAmount = depositDto.DepositAmount;
            deposit.DepositDate = depositDto.DepositDate;
            deposit.DepositSource = depositDto.DepositSource;
  

            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserDepositAdmin(int id)
        {
            var deposit = _context.UserDeposits.FirstOrDefault(d => d.UserId == id && !d.IsDeleted);
            if (deposit == null)
            {
                return NotFound();
            }

            deposit.IsDeleted = true;
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("Admin/Clear")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearUserDepositsAdmin()
        {
            _context.UserDeposits.RemoveRange(_context.UserDeposits);
            _context.SaveChanges();

            return Ok();
        }

    }
}
