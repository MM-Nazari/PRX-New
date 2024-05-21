using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using PRX.Data;
using PRX.Dto.Hoghooghi;
using PRX.Models.Hoghooghis.Hoghooghi;
using PRX.Utils;

namespace PRX.Controllers.Hoghooghi
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "HoghooghiUsersAssets")]
    public class HoghooghiUserAssetIncomeStatusTwoYearsAgoController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public HoghooghiUserAssetIncomeStatusTwoYearsAgoController(PRXDbContext context)
        {
            _context = context;
        }

        // GET: api/HoghooghiUserAssetIncomeStatusTwoYearsAgo
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            try
            {
                var records = _context.HoghooghiUsersAssets.ToList();
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        // GET: api/HoghooghiUserAssetIncomeStatusTwoYearsAgo/5
        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
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
                var record = _context.HoghooghiUsersAssets.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiAssetIncomeNotfound});
                }
                return Ok(record);

            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        // POST: api/HoghooghiUserAssetIncomeStatusTwoYearsAgo
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] HoghooghiUserAssetIncomeStatusTwoYearsAgoDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var record = new HoghooghiUserAssetIncomeStatusTwoYearsAgo
                {
                    UserId = dto.UserId,
                    FiscalYear = dto.FiscalYear,
                    RegisteredCapital = dto.RegisteredCapital,
                    ApproximateAssetValue = dto.ApproximateAssetValue,
                    TotalLiabilities = dto.TotalLiabilities,
                    TotalInvestments = dto.TotalInvestments,
                    OperationalIncome = dto.OperationalIncome,
                    OtherIncome = dto.OtherIncome,
                    OperationalExpenses = dto.OperationalExpenses,
                    OtherExpenses = dto.OtherExpenses,
                    OperationalProfitOrLoss = dto.OperationalProfitOrLoss,
                    NetProfitOrLoss = dto.NetProfitOrLoss,
                    AccumulatedProfitOrLoss = dto.AccumulatedProfitOrLoss
                };

                _context.HoghooghiUsersAssets.Add(record);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetById), new { id = record.Id }, record);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        // PUT: api/HoghooghiUserAssetIncomeStatusTwoYearsAgo/5
        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(int id, [FromBody] HoghooghiUserAssetIncomeStatusTwoYearsAgoDto dto)
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
                var record = _context.HoghooghiUsersAssets.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiAssetIncomeNotfound });
                }

                record.UserId = dto.UserId;
                record.FiscalYear = dto.FiscalYear;
                record.RegisteredCapital = dto.RegisteredCapital;
                record.ApproximateAssetValue = dto.ApproximateAssetValue;
                record.TotalLiabilities = dto.TotalLiabilities;
                record.TotalInvestments = dto.TotalInvestments;
                record.OperationalIncome = dto.OperationalIncome;
                record.OtherIncome = dto.OtherIncome;
                record.OperationalExpenses = dto.OperationalExpenses;
                record.OtherExpenses = dto.OtherExpenses;
                record.OperationalProfitOrLoss = dto.OperationalProfitOrLoss;
                record.NetProfitOrLoss = dto.NetProfitOrLoss;
                record.AccumulatedProfitOrLoss = dto.AccumulatedProfitOrLoss;

                _context.SaveChanges();

                return Ok(record);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }




        }

        // DELETE: api/HoghooghiUserAssetIncomeStatusTwoYearsAgo/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
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
                var record = _context.HoghooghiUsersAssets.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiAssetIncomeNotfound });
                }

                record.IsDeleted = true;
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        // DELETE: api/HoghooghiUserAssetIncomeStatusTwoYearsAgo
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearAll()
        {
            try
            {
                _context.HoghooghiUsersAssets.RemoveRange(_context.HoghooghiUsersAssets);
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }


        [HttpPut("complete/{id}")]
        //[Authorize(Roles = "Admin")] // Assuming only admins can mark profiles as complete
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkRelationshipAsComplete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var record = _context.HoghooghiUsersAssets.FirstOrDefault(e => e.UserId == id);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiAssetIncomeNotfound });
                }

                record.IsComplete = true;
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


                var record = _context.HoghooghiUsersAssets.FirstOrDefault(e => e.UserId == id);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiAssetIncomeNotfound });
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
        public IActionResult GetAllHoghooghiUserAssetIncomeStatusTwoYearsAgo()
        {
            try
            {
                var statuses = _context.HoghooghiUsersAssets.ToList();
                var statusDtos = statuses.Select(status => new HoghooghiUserAssetIncomeStatusTwoYearsAgoDto
                {
                    UserId = status.UserId,
                    FiscalYear = status.FiscalYear,
                    RegisteredCapital = status.RegisteredCapital,
                    ApproximateAssetValue = status.ApproximateAssetValue,
                    TotalLiabilities = status.TotalLiabilities,
                    TotalInvestments = status.TotalInvestments,
                    OperationalIncome = status.OperationalIncome,
                    OtherIncome = status.OtherIncome,
                    OperationalExpenses = status.OperationalExpenses,
                    OtherExpenses = status.OtherExpenses,
                    OperationalProfitOrLoss = status.OperationalProfitOrLoss,
                    NetProfitOrLoss = status.NetProfitOrLoss,
                    AccumulatedProfitOrLoss = status.AccumulatedProfitOrLoss,
                    IsComplete = status.IsComplete,
                    IsDeleted = status.IsDeleted
                }).ToList();
                return Ok(statusDtos);
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
        public IActionResult GetHoghooghiUserAssetIncomeStatusTwoYearsAgoByIdAdmin(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var status = _context.HoghooghiUsersAssets.FirstOrDefault(s => s.UserId == id && !s.IsDeleted);
                if (status == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiAssetIncomeNotfound });
                }

                var statusDto = new HoghooghiUserAssetIncomeStatusTwoYearsAgoDto
                {
                    UserId = status.UserId,
                    FiscalYear = status.FiscalYear,
                    RegisteredCapital = status.RegisteredCapital,
                    ApproximateAssetValue = status.ApproximateAssetValue,
                    TotalLiabilities = status.TotalLiabilities,
                    TotalInvestments = status.TotalInvestments,
                    OperationalIncome = status.OperationalIncome,
                    OtherIncome = status.OtherIncome,
                    OperationalExpenses = status.OperationalExpenses,
                    OtherExpenses = status.OtherExpenses,
                    OperationalProfitOrLoss = status.OperationalProfitOrLoss,
                    NetProfitOrLoss = status.NetProfitOrLoss,
                    AccumulatedProfitOrLoss = status.AccumulatedProfitOrLoss,
                    IsComplete = status.IsComplete,
                    IsDeleted = status.IsDeleted
                };

                return Ok(statusDto);
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
        public IActionResult UpdateHoghooghiUserAssetIncomeStatusTwoYearsAgoAdmin(int id, [FromBody] HoghooghiUserAssetIncomeStatusTwoYearsAgoDto statusDto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var status = _context.HoghooghiUsersAssets.FirstOrDefault(s => s.UserId == id && !s.IsDeleted);
                if (status == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiAssetIncomeNotfound });
                }

                status.FiscalYear = statusDto.FiscalYear;
                status.RegisteredCapital = statusDto.RegisteredCapital;
                status.ApproximateAssetValue = statusDto.ApproximateAssetValue;
                status.TotalLiabilities = statusDto.TotalLiabilities;
                status.TotalInvestments = statusDto.TotalInvestments;
                status.OperationalIncome = statusDto.OperationalIncome;
                status.OtherIncome = statusDto.OtherIncome;
                status.OperationalExpenses = statusDto.OperationalExpenses;
                status.OtherExpenses = statusDto.OtherExpenses;
                status.OperationalProfitOrLoss = statusDto.OperationalProfitOrLoss;
                status.NetProfitOrLoss = statusDto.NetProfitOrLoss;
                status.AccumulatedProfitOrLoss = statusDto.AccumulatedProfitOrLoss;
                //status.IsComplete = statusDto.IsComplete;
                //status.IsDeleted = statusDto.IsDeleted;

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
        public IActionResult DeleteHoghooghiUserAssetIncomeStatusTwoYearsAgoAdmin(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var status = _context.HoghooghiUsersAssets.FirstOrDefault(s => s.UserId == id && !s.IsDeleted);
                if (status == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiAssetIncomeNotfound });
                }

                status.IsDeleted = true;
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
        public IActionResult ClearHoghooghiUserAssetIncomeStatusTwoYearsAgo()
        {
            try
            {
                _context.HoghooghiUsersAssets.RemoveRange(_context.HoghooghiUsersAssets);
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK});
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

    }
}
