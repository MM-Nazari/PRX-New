using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using PRX.Data;
using PRX.Dto.Hoghooghi;
using PRX.Models.Hoghooghis.Hoghooghi;
using PRX.Utils;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;

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
        [HttpGet("{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int requestId)
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

                var record = _context.HoghooghiUsersAssets.Where(e => e.RequestId == requestId && !e.IsDeleted).Select(r => new HoghooghiUserAssetIncomeStatusTwoYearsAgoDto 
                {
                    Id = r.Id,
                    RequestId = requestId,
                    FiscalYear = r.FiscalYear,
                    RegisteredCapital = r.RegisteredCapital,
                    ApproximateAssetValue = r.ApproximateAssetValue,
                    TotalLiabilities = r.TotalLiabilities,
                    TotalInvestments = r.TotalInvestments,
                    OperationalIncome = r.OperationalIncome,
                    OperationalExpenses = r.OperationalExpenses,
                    OtherExpenses = r.OtherExpenses,
                    OperationalProfitOrLoss = r.OperationalProfitOrLoss,
                    NetProfitOrLoss = r.NetProfitOrLoss,
                    AccumulatedProfitOrLoss = r.AccumulatedProfitOrLoss,
                    IsComplete = r.IsComplete,
                    IsDeleted = r.IsDeleted

                }).ToList();

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
        public IActionResult Create([FromBody] HoghooghiUserAssetIncomeStatusTwoYearsAgoListDto dto)
        {
            try
            {
                if (dto.RequestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                foreach (var asset in dto.AssetIncome) 
                {
                    var record = new HoghooghiUserAssetIncomeStatusTwoYearsAgo
                    {
                        RequestId = dto.RequestId,
                        FiscalYear = asset.FiscalYear,
                        RegisteredCapital = asset.RegisteredCapital,
                        ApproximateAssetValue = asset.ApproximateAssetValue,
                        TotalLiabilities = asset.TotalLiabilities,
                        TotalInvestments = asset.TotalInvestments,
                        OperationalIncome = asset.OperationalIncome,
                        OtherIncome = asset.OtherIncome,
                        OperationalExpenses = asset.OperationalExpenses,
                        OtherExpenses = asset.OtherExpenses,
                        OperationalProfitOrLoss = asset.OperationalProfitOrLoss,
                        NetProfitOrLoss = asset.NetProfitOrLoss,
                        AccumulatedProfitOrLoss = asset.AccumulatedProfitOrLoss
                    };
                    _context.HoghooghiUsersAssets.Add(record);
                }


                
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status201Created, new { message = ResponseMessages.OK });
                //return CreatedAtAction(nameof(GetById), new { requestId = record.Id }, record);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        // PUT: api/HoghooghiUserAssetIncomeStatusTwoYearsAgo
        [HttpPut("{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(int requestId, [FromBody] HoghooghiUserAssetIncomeStatusTwoYearsAgoListDto dto)
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

                // Fetch existing records for the request ID
                var existingRecords = _context.HoghooghiUsersAssets.Where(e => e.RequestId == requestId && !e.IsDeleted).ToList();

                foreach (var asset in dto.AssetIncome)
                {
                    var record = existingRecords.FirstOrDefault(e => e.Id == asset.Id);

                    // Update existing record
                    record.FiscalYear = asset.FiscalYear;
                    record.RegisteredCapital = asset.RegisteredCapital;
                    record.ApproximateAssetValue = asset.ApproximateAssetValue;
                    record.TotalLiabilities = asset.TotalLiabilities;
                    record.TotalInvestments = asset.TotalInvestments;
                    record.OperationalIncome = asset.OperationalIncome;
                    record.OtherIncome = asset.OtherIncome;
                    record.OperationalExpenses = asset.OperationalExpenses;
                    record.OtherExpenses = asset.OtherExpenses;
                    record.OperationalProfitOrLoss = asset.OperationalProfitOrLoss;
                    record.NetProfitOrLoss = asset.NetProfitOrLoss;
                    record.AccumulatedProfitOrLoss = asset.AccumulatedProfitOrLoss;

                    
                }

                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }


        // DELETE: api/HoghooghiUserAssetIncomeStatusTwoYearsAgo/5
        [HttpDelete("{id}/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id, int requestId)
        {
            try
            {
                if (id <= 0 || requestId <= 0)
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

                var record = _context.HoghooghiUsersAssets.FirstOrDefault(e => e.RequestId == requestId && e.Id == id && !e.IsDeleted);
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


        [HttpPut("complete/{id}/{requestId}")]
        //[Authorize(Roles = "Admin")] // Assuming only admins can mark profiles as complete
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkRelationshipAsComplete(int id, int requestId)
        {
            try
            {
                if (id <= 0 || requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var record = _context.HoghooghiUsersAssets.FirstOrDefault(e => e.RequestId == requestId && e.Id == id && !e.IsDeleted) ;
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

        [HttpGet("isComplete/{id}/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CheckCompletionStatus(int id, int requestId)
        {
            try
            {
                if (id <= 0 || requestId <= 0)
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

                var record = _context.HoghooghiUsersAssets.FirstOrDefault(e => e.RequestId == requestId && e.Id == id && !e.IsDeleted);
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
                    Id = status.Id,
                    RequestId = status.RequestId,
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

        [HttpGet("Admin/{id}/{requestId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHoghooghiUserAssetIncomeStatusTwoYearsAgoByIdAdmin(int id, int requestId)
        {
            try
            {
                if (id <= 0 || requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var status = _context.HoghooghiUsersAssets.FirstOrDefault(s => s.RequestId == requestId && s.Id == id && !s.IsDeleted);
                if (status == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiAssetIncomeNotfound });
                }

                var statusDto = new HoghooghiUserAssetIncomeStatusTwoYearsAgoDto
                {
                    Id = id,
                    RequestId = status.RequestId,
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

                var status = _context.HoghooghiUsersAssets.FirstOrDefault(s => s.Id == id && !s.IsDeleted);
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

                var status = _context.HoghooghiUsersAssets.FirstOrDefault(s => s.Id == id && !s.IsDeleted);
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
