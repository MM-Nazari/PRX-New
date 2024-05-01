using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using PRX.Data;
using PRX.Dto.Hoghooghi;
using PRX.Models.Hoghooghis.Hoghooghi;

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
            var records = _context.HoghooghiUsersAssets.ToList();
            return Ok(records);
        }

        // GET: api/HoghooghiUserAssetIncomeStatusTwoYearsAgo/5
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
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
                var record = _context.HoghooghiUsersAssets.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound();
                }
                return Ok(record);

            }

            catch (Exception ex)
            {
                
                return BadRequest();
            }


        }

        // POST: api/HoghooghiUserAssetIncomeStatusTwoYearsAgo
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] HoghooghiUserAssetIncomeStatusTwoYearsAgoDto dto)
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

        // PUT: api/HoghooghiUserAssetIncomeStatusTwoYearsAgo/5
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(int id, [FromBody] HoghooghiUserAssetIncomeStatusTwoYearsAgoDto dto)
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
                var record = _context.HoghooghiUsersAssets.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound();
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
                
                return BadRequest();
            }




        }

        // DELETE: api/HoghooghiUserAssetIncomeStatusTwoYearsAgo/5
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
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
                var record = _context.HoghooghiUsersAssets.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound();
                }

                record.IsDeleted = true;
                _context.SaveChanges();

                return Ok();

            }

            catch (Exception ex)
            {
                
                return BadRequest();
            }


        }

        // DELETE: api/HoghooghiUserAssetIncomeStatusTwoYearsAgo
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearAll()
        {
            _context.HoghooghiUsersAssets.RemoveRange(_context.HoghooghiUsersAssets);
            _context.SaveChanges();

            return Ok();
        }


        [HttpPut("complete/{id}")]
        //[Authorize(Roles = "Admin")] // Assuming only admins can mark profiles as complete
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkRelationshipAsComplete(int id)
        {
            var record = _context.HoghooghiUsersAssets.FirstOrDefault(e => e.UserId == id);
            if (record == null)
            {
                return NotFound();
            }

            record.IsComplete = true;
            _context.SaveChanges();

            return Ok();
        }
    }
}
