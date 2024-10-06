using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.User;
using PRX.Models.Haghighi;
using PRX.Models.User;
using PRX.Utils;
using System;
using System.Linq;
using System.Net;

namespace PRX.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "HaghighiUserBankInfo")]
    public class UserBankInfoController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public UserBankInfoController(PRXDbContext context)
        {
            _context = context;
        }

        // GET: api/HaghighiUserBankInfo
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllHaghighiUserBankInfo()
        {
            try
            {
                var bankInfos = _context.UserBankInfos.ToList();
                return Ok(bankInfos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        // GET: api/HaghighiUserBankInfo/5
        [HttpGet("{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHaghighiUserBankInfoById(int requestId)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                // Retrieve the user ID from the token
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

                var bankInfo = _context.UserBankInfos.FirstOrDefault(e => e.RequestId == requestId && !e.IsDeleted);
                if (bankInfo == null)
                {
                    return NotFound(new { message = ResponseMessages.UserBankInfoNotFound });
                }
                return Ok(bankInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        // POST: api/HaghighiUserBankInfo
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateHaghighiUserBankInfo([FromBody] UserBankInfoDto bankInfoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check for unique UserId
                var existingBankInfo = _context.UserBankInfos
                                               .FirstOrDefault(bi => bi.RequestId == bankInfoDto.RequestId);

                if (existingBankInfo != null)
                {
                    return BadRequest(new { message = "UserId already exists." });
                }

                var bankInfo = new UserBankInfo
                {
                    RequestId = bankInfoDto.RequestId,
                    TradeCode = bankInfoDto.TradeCode,
                    SejamCode = bankInfoDto.SejamCode,
                    BankName = bankInfoDto.BankName,
                    BranchCode = bankInfoDto.BranchCode,
                    BranchName = bankInfoDto.BranchName,
                    BranchCity = bankInfoDto.BranchCity,
                    AccountNumber = bankInfoDto.AccountNumber,
                    IBAN = bankInfoDto.IBAN,
                    AccountType = bankInfoDto.AccountType,
                    CapitalAmount = bankInfoDto.CapitalAmount,
                    CapitalType = bankInfoDto.CapitalType
                };

                _context.UserBankInfos.Add(bankInfo);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetHaghighiUserBankInfoById), new { requestId = bankInfo.Id }, bankInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        // PATCH: api/HaghighiUserBankInfo/5
        [HttpPatch("{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult PatchHaghighiUserBankInfo(int requestId, [FromBody] JsonPatchDocument<UserBankInfoDto> patchDoc)
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

                var bankInfo = _context.UserBankInfos.FirstOrDefault(e => e.RequestId == requestId && !e.IsDeleted);
                if (bankInfo == null)
                {
                    return NotFound(new { message = ResponseMessages.UserBankInfoNotFound });
                }

                // Create a DTO from the existing entity
                var bankInfoDto = new UserBankInfoDto
                {
                    TradeCode = bankInfo.TradeCode,
                    SejamCode = bankInfo.SejamCode,
                    BankName = bankInfo.BankName,
                    BranchCode = bankInfo.BranchCode,
                    BranchName = bankInfo.BranchName,
                    BranchCity = bankInfo.BranchCity,
                    AccountNumber = bankInfo.AccountNumber,
                    IBAN = bankInfo.IBAN,
                    AccountType = bankInfo.AccountType,
                    CapitalAmount = bankInfo.CapitalAmount,
                    CapitalType = bankInfo.CapitalType
                };

                // Apply the patch to the DTO
                patchDoc.ApplyTo(bankInfoDto);

                // Check if the model state is valid after applying the patch
                if (!TryValidateModel(bankInfoDto))
                {
                    return BadRequest(ModelState);
                }

                // Update the entity with patched data
                bankInfo.TradeCode = bankInfoDto.TradeCode;
                bankInfo.SejamCode = bankInfoDto.SejamCode;
                bankInfo.BankName = bankInfoDto.BankName;
                bankInfo.BranchCode = bankInfoDto.BranchCode;
                bankInfo.BranchName = bankInfoDto.BranchName;
                bankInfo.BranchCity = bankInfoDto.BranchCity;
                bankInfo.AccountNumber = bankInfoDto.AccountNumber;
                bankInfo.IBAN = bankInfoDto.IBAN;
                bankInfo.AccountType = bankInfoDto.AccountType;
                bankInfo.CapitalAmount = bankInfoDto.CapitalAmount;
                bankInfo.CapitalType = bankInfoDto.CapitalType;

                _context.SaveChanges();

                return Ok(bankInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        // PUT: api/HaghighiUserBankInfo/5
        [HttpPut("{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHaghighiUserBankInfo(int requestId, [FromBody] UserBankInfoDto bankInfoDto)
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

                var bankInfo = _context.UserBankInfos.FirstOrDefault(e => e.RequestId == requestId && !e.IsDeleted);
                if (bankInfo == null)
                {
                    return NotFound(new { message = ResponseMessages.UserBankInfoNotFound });
                }

                bankInfo.TradeCode = bankInfoDto.TradeCode;
                bankInfo.SejamCode = bankInfoDto.SejamCode;
                bankInfo.BankName = bankInfoDto.BankName;
                bankInfo.BranchCode = bankInfoDto.BranchCode;
                bankInfo.BranchName = bankInfoDto.BranchName;
                bankInfo.BranchCity = bankInfoDto.BranchCity;
                bankInfo.AccountNumber = bankInfoDto.AccountNumber;
                bankInfo.IBAN = bankInfoDto.IBAN;
                bankInfo.AccountType = bankInfoDto.AccountType;
                bankInfo.CapitalAmount = bankInfoDto.CapitalAmount;
                bankInfo.CapitalType = bankInfoDto.CapitalType;

                _context.SaveChanges();

                return Ok(bankInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        // DELETE: api/HaghighiUserBankInfo/5
        [HttpDelete("{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHaghighiUserBankInfo(int requestId)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                // Retrieve the user ID from the token
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

                var bankInfo = _context.UserBankInfos.FirstOrDefault(e => e.RequestId == requestId && !e.IsDeleted);
                if (bankInfo == null)
                {
                    return NotFound(new { message = ResponseMessages.UserBankInfoNotFound });
                }

                bankInfo.IsDeleted = true;
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        // PUT: api/HaghighiUserBankInfo/complete/{id}
        [HttpPut("complete/{requestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkBankInfoAsComplete(int requestId)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var bankInfo = _context.UserBankInfos.FirstOrDefault(e => e.RequestId == requestId);
                if (bankInfo == null)
                {
                    return NotFound(new { message = ResponseMessages.UserBankInfoNotFound });
                }

                bankInfo.IsComplete = true;
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
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


                var record = _context.UserBankInfos.FirstOrDefault(e => e.RequestId == requestId);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.UserBankInfoNotFound });
                }

                return Ok(new { isComplete = record.IsComplete });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        // DELETE: api/HaghighiUserFinancialProfile/clear
        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearAllHaghighiUsersBankInfos()
        {
            try
            {
                _context.UserBankInfos.RemoveRange(_context.UserBankInfos);
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // GET: api/HaghighiUserBankInfo/Admin
        [HttpGet("Admin")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllHaghighiUserBankInfoAdmin()
        {
            try
            {
                var bankInfos = _context.UserBankInfos.ToList();
                var bankInfoDtos = bankInfos.Select(bankInfo => new UserBankInfoDto
                {
                    RequestId = bankInfo.RequestId,
                    TradeCode = bankInfo.TradeCode,
                    SejamCode = bankInfo.SejamCode,
                    BankName = bankInfo.BankName,
                    BranchCode = bankInfo.BranchCode,
                    BranchName = bankInfo.BranchName,
                    BranchCity = bankInfo.BranchCity,
                    AccountNumber = bankInfo.AccountNumber,
                    IBAN = bankInfo.IBAN,
                    AccountType = bankInfo.AccountType,
                    CapitalAmount = bankInfo.CapitalAmount,
                    CapitalType = bankInfo.CapitalType,
                    IsComplete = bankInfo.IsComplete,
                    IsDeleted = bankInfo.IsDeleted
                }).ToList();
                return Ok(bankInfoDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        // GET: api/HaghighiUserBankInfo/Admin/{id}
        [HttpGet("Admin/{requestId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHaghighiUserBankInfoByIdAdmin(int requestId)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var bankInfo = _context.UserBankInfos.FirstOrDefault(b => b.RequestId == requestId);
                if (bankInfo == null)
                {
                    return NotFound(new { message = ResponseMessages.UserBankInfoNotFound });
                }

                var bankInfoDto = new UserBankInfoDto
                {
                    RequestId = bankInfo.RequestId,
                    TradeCode = bankInfo.TradeCode,
                    SejamCode = bankInfo.SejamCode,
                    BankName = bankInfo.BankName,
                    BranchCode = bankInfo.BranchCode,
                    BranchName = bankInfo.BranchName,
                    BranchCity = bankInfo.BranchCity,
                    AccountNumber = bankInfo.AccountNumber,
                    IBAN = bankInfo.IBAN,
                    AccountType = bankInfo.AccountType,
                    CapitalAmount = bankInfo.CapitalAmount,
                    CapitalType = bankInfo.CapitalType,
                    IsComplete = bankInfo.IsComplete,
                    IsDeleted = bankInfo.IsDeleted
                };

                return Ok(bankInfoDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        // PUT: api/HaghighiUserBankInfo/Admin/{id}
        [HttpPut("Admin/{requestId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHaghighiUserBankInfoAdmin(int requestId, [FromBody] UserBankInfoDto bankInfoDto)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var bankInfo = _context.UserBankInfos.FirstOrDefault(b => b.RequestId == requestId && !b.IsDeleted);
                if (bankInfo == null)
                {
                    return NotFound(new { message = ResponseMessages.UserBankInfoNotFound });
                }

                bankInfo.TradeCode = bankInfoDto.TradeCode;
                bankInfo.SejamCode = bankInfoDto.SejamCode;
                bankInfo.BankName = bankInfoDto.BankName;
                bankInfo.BranchCode = bankInfoDto.BranchCode;
                bankInfo.BranchName = bankInfoDto.BranchName;
                bankInfo.BranchCity = bankInfoDto.BranchCity;
                bankInfo.AccountNumber = bankInfoDto.AccountNumber;
                bankInfo.IBAN = bankInfoDto.IBAN;
                bankInfo.AccountType = bankInfoDto.AccountType;
                bankInfo.CapitalAmount = bankInfoDto.CapitalAmount;
                bankInfo.CapitalType = bankInfoDto.CapitalType;

                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        // DELETE: api/HaghighiUserBankInfo/Admin/{id}
        [HttpDelete("Admin/{requestId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHaghighiUserBankInfoAdmin(int requestId)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var bankInfo = _context.UserBankInfos.FirstOrDefault(b => b.RequestId == requestId && !b.IsDeleted);
                if (bankInfo == null)
                {
                    return NotFound(new { message = ResponseMessages.UserBankInfoNotFound });
                }

                bankInfo.IsDeleted = true;
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        // DELETE: api/HaghighiUserBankInfo/Admin/Clear
        [HttpDelete("Admin/Clear")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearHaghighiUserBankInfos()
        {
            try
            {
                _context.UserBankInfos.RemoveRange(_context.UserBankInfos);
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

