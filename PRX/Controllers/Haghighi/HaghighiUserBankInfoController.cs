using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.Haghighi;
using PRX.Models.Haghighi;
using PRX.Utils;
using System;
using System.Linq;
using System.Net;

namespace PRX.Controllers.Haghighi
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "HaghighiUserBankInfo")]
    public class HaghighiUserBankInfoController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public HaghighiUserBankInfoController(PRXDbContext context)
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
                var bankInfos = _context.HaghighiUserBankInfos.ToList();
                return Ok(bankInfos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        // GET: api/HaghighiUserBankInfo/5
        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHaghighiUserBankInfoById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Fetch the request
                var request = _context.Requests.FirstOrDefault(r => r.Id == id);

                if (request == null)
                {
                    return NotFound(new { message = ResponseMessages.RequestNotFound });
                }

                // Ensure that the user associated with the request matches the token user ID
                if (request.UserId != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }

                var bankInfo = _context.HaghighiUserBankInfos.FirstOrDefault(e => e.RequestId == id && !e.IsDeleted);
                if (bankInfo == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserBankInfoNotFound });
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
        public IActionResult CreateHaghighiUserBankInfo([FromBody] HaghighiUserBankInfoDto bankInfoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check for unique UserId
                var existingBankInfo = _context.HaghighiUserBankInfos
                                               .FirstOrDefault(bi => bi.RequestId == bankInfoDto.RequestId);

                if (existingBankInfo != null)
                {
                    return BadRequest(new { message = "UserId already exists." });
                }

                var bankInfo = new HaghighiUserBankInfo
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

                _context.HaghighiUserBankInfos.Add(bankInfo);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetHaghighiUserBankInfoById), new { id = bankInfo.Id }, bankInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        // PUT: api/HaghighiUserBankInfo/5
        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHaghighiUserBankInfo(int id, [FromBody] HaghighiUserBankInfoDto bankInfoDto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Fetch the request
                var request = _context.Requests.FirstOrDefault(r => r.Id == id);

                if (request == null)
                {
                    return NotFound(new { message = ResponseMessages.RequestNotFound });
                }

                // Ensure that the user associated with the request matches the token user ID
                if (request.UserId != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }

                var bankInfo = _context.HaghighiUserBankInfos.FirstOrDefault(e => e.RequestId == id && !e.IsDeleted);
                if (bankInfo == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserBankInfoNotFound });
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
        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHaghighiUserBankInfo(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Fetch the request
                var request = _context.Requests.FirstOrDefault(r => r.Id == id);

                if (request == null)
                {
                    return NotFound(new { message = ResponseMessages.RequestNotFound });
                }

                // Ensure that the user associated with the request matches the token user ID
                if (request.UserId != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }

                var bankInfo = _context.HaghighiUserBankInfos.FirstOrDefault(e => e.RequestId == id && !e.IsDeleted);
                if (bankInfo == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserBankInfoNotFound });
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
        [HttpPut("complete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkBankInfoAsComplete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var bankInfo = _context.HaghighiUserBankInfos.FirstOrDefault(e => e.RequestId == id);
                if (bankInfo == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserBankInfoNotFound });
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
                // Fetch the request
                var request = _context.Requests.FirstOrDefault(r => r.Id == id);

                if (request == null)
                {
                    return NotFound(new { message = ResponseMessages.RequestNotFound });
                }

                // Ensure that the user associated with the request matches the token user ID
                if (request.UserId != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }


                var record = _context.HaghighiUserBankInfos.FirstOrDefault(e => e.RequestId == id);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserBankInfoNotFound });
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
                _context.HaghighiUserBankInfos.RemoveRange(_context.HaghighiUserBankInfos);
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
                var bankInfos = _context.HaghighiUserBankInfos.ToList();
                var bankInfoDtos = bankInfos.Select(bankInfo => new HaghighiUserBankInfoDto
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
        [HttpGet("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHaghighiUserBankInfoByIdAdmin(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var bankInfo = _context.HaghighiUserBankInfos.FirstOrDefault(b => b.RequestId == id);
                if (bankInfo == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserBankInfoNotFound });
                }

                var bankInfoDto = new HaghighiUserBankInfoDto
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
        [HttpPut("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHaghighiUserBankInfoAdmin(int id, [FromBody] HaghighiUserBankInfoDto bankInfoDto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var bankInfo = _context.HaghighiUserBankInfos.FirstOrDefault(b => b.RequestId == id && !b.IsDeleted);
                if (bankInfo == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserBankInfoNotFound });
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
        [HttpDelete("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHaghighiUserBankInfoAdmin(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var bankInfo = _context.HaghighiUserBankInfos.FirstOrDefault(b => b.RequestId == id && !b.IsDeleted);
                if (bankInfo == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserBankInfoNotFound });
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
                _context.HaghighiUserBankInfos.RemoveRange(_context.HaghighiUserBankInfos);
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

