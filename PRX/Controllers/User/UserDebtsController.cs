using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.User;
using PRX.Models.User;
using PRX.Utils;
using System;
using System.Linq;

namespace PRX.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "UserDebts")]
    public class UserDebtsController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public UserDebtsController(PRXDbContext context)
        {
            _context = context;
        }

        [HttpGet("{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserDebtById(int requestId)
        {
            try
            {

                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);
                // Fetch the request
                var request = _context.Requests.FirstOrDefault(r => r.Id == requestId);

                if (request == null)
                {
                    return NotFound(new { message = ResponseMessages.RequestNotFound });
                }

                // Ensure that the user associated with the request matches the token user ID
                if (request.UserId != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }

                var userDebt = _context.UserDebts.FirstOrDefault(u => u.RequestId == requestId && !u.IsDeleted);
                if (userDebt == null)
                {
                    return NotFound(new { message = ResponseMessages.UserDebtNotFound});
                }

                return Ok(userDebt);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateUserDebt([FromBody] UserDebtDto userDebtDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var userDebt = new UserDebt
                {
                    RequestId = userDebtDto.RequestId,
                    DebtTitle = userDebtDto.DebtTitle,
                    DebtAmount = userDebtDto.DebtAmount,
                    DebtDueDate = userDebtDto.DebtDueDate,
                    DebtRepaymentPercentage = userDebtDto.DebtRepaymentPercentage
                };

                _context.UserDebts.Add(userDebt);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetUserDebtById), new { id = userDebt.Id }, userDebt);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        [HttpPut("{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateUserDebt(int requestId, [FromBody] UserDebtDto userDebtDto)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);
                // Fetch the request
                var request = _context.Requests.FirstOrDefault(r => r.Id == requestId);

                if (request == null)
                {
                    return NotFound(new { message = ResponseMessages.RequestNotFound });
                }

                // Ensure that the user associated with the request matches the token user ID
                if (request.UserId != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }

                var userDebt = _context.UserDebts.FirstOrDefault(u => u.RequestId == requestId && !u.IsDeleted);
                if (userDebt == null)
                {
                    return NotFound(new { message = ResponseMessages.UserDebtNotFound });
                }

                userDebt.RequestId = userDebtDto.RequestId;
                userDebt.DebtTitle = userDebtDto.DebtTitle;
                userDebt.DebtAmount = userDebtDto.DebtAmount;
                userDebt.DebtDueDate = userDebtDto.DebtDueDate;
                userDebt.DebtRepaymentPercentage = userDebtDto.DebtRepaymentPercentage;

                _context.SaveChanges();

                return Ok(userDebt);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        [HttpDelete("{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteUserDebt(int requestId)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);
                // Fetch the request
                var request = _context.Requests.FirstOrDefault(r => r.Id == requestId);

                if (request == null)
                {
                    return NotFound(new { message = ResponseMessages.RequestNotFound });
                }

                // Ensure that the user associated with the request matches the token user ID
                if (request.UserId != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }
                var userDebt = _context.UserDebts.FirstOrDefault(u => u.RequestId == requestId && !u.IsDeleted);
                if (userDebt == null)
                {
                    return NotFound(new { message = ResponseMessages.UserDebtNotFound });
                }

                userDebt.IsDeleted = true;
                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        [HttpPut("complete/{requestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult MarkCompaniesAsComplete(int requestId)
        {
            try
            {

                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var record = _context.UserDebts.FirstOrDefault(e => e.RequestId == requestId);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.UserDebtNotFound });
                }

                record.IsComplete = true;
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        [HttpGet("isComplete/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CheckCompletionStatus(int requestId)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);
                // Fetch the request
                var request = _context.Requests.FirstOrDefault(r => r.Id == requestId);

                if (request == null)
                {
                    return NotFound(new { message = ResponseMessages.RequestNotFound });
                }

                // Ensure that the user associated with the request matches the token user ID
                if (request.UserId != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }

                var record = _context.UserDebts.FirstOrDefault(e => e.RequestId == requestId);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.UserDebtNotFound });
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllUserDebts()
        {
            try
            {
                var userDebts = _context.UserDebts.ToList();
                var userDebtDtos = userDebts.Select(debt => new UserDebtDto
                {
                    RequestId = debt.RequestId,
                    DebtTitle = debt.DebtTitle,
                    DebtAmount = debt.DebtAmount,
                    DebtDueDate = debt.DebtDueDate,
                    DebtRepaymentPercentage = debt.DebtRepaymentPercentage,
                    IsComplete = debt.IsComplete,
                    IsDeleted = debt.IsDeleted
                }).ToList();
                return Ok(userDebtDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        [HttpGet("Admin/{requestId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserDebtByIdAdmin(int requestId)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userDebt = _context.UserDebts.FirstOrDefault(debt => debt.RequestId == requestId && !debt.IsDeleted);
                if (userDebt == null)
                {
                    return NotFound(new { message = ResponseMessages.UserDebtNotFound });
                }

                var userDebtDto = new UserDebtDto
                {
                    RequestId = userDebt.RequestId,
                    DebtTitle = userDebt.DebtTitle,
                    DebtAmount = userDebt.DebtAmount,
                    DebtDueDate = userDebt.DebtDueDate,
                    DebtRepaymentPercentage = userDebt.DebtRepaymentPercentage,
                    IsComplete = userDebt.IsComplete,
                    IsDeleted = userDebt.IsDeleted
                };

                return Ok(userDebtDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        [HttpPut("Admin/{requestId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateUserDebtAdmin(int requestId, [FromBody] UserDebtDto userDebtDto)
        {
            try
            {

                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userDebt = _context.UserDebts.FirstOrDefault(debt => debt.RequestId == requestId && !debt.IsDeleted);
                if (userDebt == null)
                {
                    return NotFound(new { message = ResponseMessages.UserDebtNotFound });
                }

                userDebt.DebtTitle = userDebtDto.DebtTitle;
                userDebt.DebtAmount = userDebtDto.DebtAmount;
                userDebt.DebtDueDate = userDebtDto.DebtDueDate;
                userDebt.DebtRepaymentPercentage = userDebtDto.DebtRepaymentPercentage;

                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        [HttpDelete("Admin/{requestId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteUserDebtAdmin(int requestId)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userDebt = _context.UserDebts.FirstOrDefault(debt => debt.RequestId == requestId && !debt.IsDeleted);
                if (userDebt == null)
                {
                    return NotFound(new { message = ResponseMessages.UserDebtNotFound });
                }

                userDebt.IsDeleted = true;
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ClearUserDebts()
        {
            try
            {
                _context.UserDebts.RemoveRange(_context.UserDebts);
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        // Other actions...
    }
}


//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authorization;
//using PRX.Data;
//using PRX.Dto.User;
//using PRX.Models.User;

//namespace PRX.Controllers.User
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    [ApiExplorerSettings(GroupName = "UserDebts")]
//    public class UserDebtsController : ControllerBase
//    {
//        private readonly PRXDbContext _context;

//        public UserDebtsController(PRXDbContext context)
//        {
//            _context = context;
//        }


//        [HttpGet("{id}")]
//        [Authorize(Roles = "User")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public IActionResult GetUserDebtById(int id)
//        {

//            try
//            {

//                // Retrieve the user ID from the token
//                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

//                // Ensure that the user is updating their own profile
//                if (id != tokenUserId)
//                {
//                    return Forbid(); // Or return 403 Forbidden
//                }
//                var userDebt = _context.UserDebts.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
//                if (userDebt == null)
//                {
//                    return NotFound();
//                }
//                return Ok(userDebt);

//            }

//            catch (Exception ex)
//            {

//                return BadRequest();
//            }

//        }

//        [HttpPost]
//        [ProducesResponseType(StatusCodes.Status201Created)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        public IActionResult CreateUserDebt([FromBody] UserDebtDto userDebtDto)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            var userDebt = new UserDebt
//            {
//                UserId = userDebtDto.UserId,
//                DebtTitle = userDebtDto.DebtTitle,
//                DebtAmount = userDebtDto.DebtAmount,
//                DebtDueDate = userDebtDto.DebtDueDate,
//                DebtRepaymentPercentage = userDebtDto.DebtRepaymentPercentage
//            };

//            _context.UserDebts.Add(userDebt);
//            _context.SaveChanges();

//            return CreatedAtAction(nameof(GetUserDebtById), new { id = userDebt.Id }, userDebt);
//        }

//        [HttpPut("{id}")]
//        [Authorize(Roles = "User")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public IActionResult UpdateUserDebt(int id, [FromBody] UserDebtDto userDebtDto)
//        {

//            try
//            {

//                // Retrieve the user ID from the token
//                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

//                // Ensure that the user is updating their own profile
//                if (id != tokenUserId)
//                {
//                    return Forbid(); // Or return 403 Forbidden
//                }
//                var userDebt = _context.UserDebts.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
//                if (userDebt == null)
//                {
//                    return NotFound();
//                }

//                userDebt.UserId = userDebtDto.UserId;
//                userDebt.DebtTitle = userDebtDto.DebtTitle;
//                userDebt.DebtAmount = userDebtDto.DebtAmount;
//                userDebt.DebtDueDate = userDebtDto.DebtDueDate;
//                userDebt.DebtRepaymentPercentage = userDebtDto.DebtRepaymentPercentage;

//                _context.SaveChanges();

//                return Ok(userDebt);

//            }

//            catch (Exception ex)
//            {

//                return BadRequest();
//            }


//        }

//        [HttpDelete("{id}")]
//        [Authorize(Roles = "User")]
//        [ProducesResponseType(StatusCodes.Status204NoContent)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public IActionResult DeleteUserDebt(int id)
//        {

//            try
//            {

//                // Retrieve the user ID from the token
//                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

//                // Ensure that the user is updating their own profile
//                if (id != tokenUserId)
//                {
//                    return Forbid(); // Or return 403 Forbidden
//                }
//                var userDebt = _context.UserDebts.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
//                if (userDebt == null)
//                {
//                    return NotFound();
//                }

//                userDebt.IsDeleted = true;
//                _context.SaveChanges();

//                return NoContent();

//            }

//            catch (Exception ex)
//            {

//                return BadRequest();
//            }

//        }


//[HttpPut("complete/{id}")]
//        //[Authorize(Roles = "Admin")] // Assuming only admins can mark profiles as complete
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status403Forbidden)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public IActionResult MarkCompaniesAsComplete(int id)
//        {
//            var record = _context.UserDebts.FirstOrDefault(e => e.UserId == id);
//            if (record == null)
//            {
//                return NotFound();
//            }

//            record.IsComplete = true;
//            _context.SaveChanges();

//            return Ok();
//        }

//        [HttpGet("Admin")]
//        [Authorize(Roles = "Admin")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public IActionResult GetAllUserDebts()
//        {
//            var userDebts = _context.UserDebts.ToList();
//            var userDebtDtos = userDebts.Select(debt => new UserDebtDto
//            {
//                UserId = debt.UserId,
//                DebtTitle = debt.DebtTitle,
//                DebtAmount = debt.DebtAmount,
//                DebtDueDate = debt.DebtDueDate,
//                DebtRepaymentPercentage = debt.DebtRepaymentPercentage,
//                IsComplete = debt.IsComplete,
//                IsDeleted = debt.IsDeleted
//            }).ToList();
//            return Ok(userDebtDtos);
//        }

//        [HttpGet("Admin/{id}")]
//        [Authorize(Roles = "Admin")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public IActionResult GetUserDebtByIdAdmin(int id)
//        {
//            var userDebt = _context.UserDebts.FirstOrDefault(debt => debt.UserId == id && !debt.IsDeleted);
//            if (userDebt == null)
//            {
//                return NotFound();
//            }

//            var userDebtDto = new UserDebtDto
//            {
//                UserId = userDebt.UserId,
//                DebtTitle = userDebt.DebtTitle,
//                DebtAmount = userDebt.DebtAmount,
//                DebtDueDate = userDebt.DebtDueDate,
//                DebtRepaymentPercentage = userDebt.DebtRepaymentPercentage,
//                IsComplete = userDebt.IsComplete,
//                IsDeleted = userDebt.IsDeleted
//            };

//            return Ok(userDebtDto);
//        }

//        [HttpPut("Admin/{id}")]
//        [Authorize(Roles = "Admin")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public IActionResult UpdateUserDebtAdmin(int id, [FromBody] UserDebtDto userDebtDto)
//        {
//            var userDebt = _context.UserDebts.FirstOrDefault(debt => debt.UserId == id && !debt.IsDeleted);
//            if (userDebt == null)
//            {
//                return NotFound();
//            }

//            userDebt.DebtTitle = userDebtDto.DebtTitle;
//            userDebt.DebtAmount = userDebtDto.DebtAmount;
//            userDebt.DebtDueDate = userDebtDto.DebtDueDate;
//            userDebt.DebtRepaymentPercentage = userDebtDto.DebtRepaymentPercentage;
//            //userDebt.IsComplete = userDebtDto.IsComplete;
//            //userDebt.IsDeleted = userDebtDto.IsDeleted;

//            _context.SaveChanges();

//            return Ok();
//        }

//        [HttpDelete("Admin/{id}")]
//        [Authorize(Roles = "Admin")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public IActionResult DeleteUserDebtAdmin(int id)
//        {
//            var userDebt = _context.UserDebts.FirstOrDefault(debt => debt.UserId == id && !debt.IsDeleted);
//            if (userDebt == null)
//            {
//                return NotFound();
//            }

//            userDebt.IsDeleted = true;
//            _context.SaveChanges();

//            return Ok();
//        }

//        [HttpDelete("Admin/Clear")]
//        [Authorize(Roles = "Admin")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        public IActionResult ClearUserDebts()
//        {
//            _context.UserDebts.RemoveRange(_context.UserDebts);
//            _context.SaveChanges();

//            return Ok();
//        }


//    }
//}
