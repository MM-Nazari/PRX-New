using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.Hoghooghi;
using PRX.Models.Hoghooghi;

namespace PRX.Controllers.Hoghooghi
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "HoghooghiUserCompaniesWithMajorInvestors")]
    public class HoghooghiUserCompaniesWithMajorInvestorsController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public HoghooghiUserCompaniesWithMajorInvestorsController(PRXDbContext context)
        {
            _context = context;
        }

        // GET: api/HoghooghiUserCompaniesWithMajorInvestors
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var records = _context.HoghooghiUserCompaniesWithMajorInvestors.ToList();
            return Ok(records);
        }

        // GET: api/HoghooghiUserCompaniesWithMajorInvestors/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var record = _context.HoghooghiUserCompaniesWithMajorInvestors.FirstOrDefault(e => e.Id == id);
            if (record == null)
            {
                return NotFound();
            }
            return Ok(record);
        }

        // POST: api/HoghooghiUserCompaniesWithMajorInvestors
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] HoghooghiUserCompaniesWithMajorInvestorsDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var record = new HoghooghiUserCompaniesWithMajorInvestors
            {
                UserId = dto.UserId,
                CompanyName = dto.CompanyName,
                CompanySubject = dto.CompanySubject,
                PercentageOfTotal = dto.PercentageOfTotal
            };

            _context.HoghooghiUserCompaniesWithMajorInvestors.Add(record);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = record.Id }, record);
        }

        // PUT: api/HoghooghiUserCompaniesWithMajorInvestors/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(int id, [FromBody] HoghooghiUserCompaniesWithMajorInvestorsDto dto)
        {
            var record = _context.HoghooghiUserCompaniesWithMajorInvestors.FirstOrDefault(e => e.Id == id);
            if (record == null)
            {
                return NotFound();
            }

            record.UserId = dto.UserId;
            record.CompanyName = dto.CompanyName;
            record.CompanySubject = dto.CompanySubject;
            record.PercentageOfTotal = dto.PercentageOfTotal;

            _context.SaveChanges();

            return Ok(record);
        }

        // DELETE: api/HoghooghiUserCompaniesWithMajorInvestors/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var record = _context.HoghooghiUserCompaniesWithMajorInvestors.FirstOrDefault(e => e.Id == id);
            if (record == null)
            {
                return NotFound();
            }

            _context.HoghooghiUserCompaniesWithMajorInvestors.Remove(record);
            _context.SaveChanges();

            return Ok();
        }

        // DELETE: api/HoghooghiUserCompaniesWithMajorInvestors
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearAll()
        {
            _context.HoghooghiUserCompaniesWithMajorInvestors.RemoveRange(_context.HoghooghiUserCompaniesWithMajorInvestors);
            _context.SaveChanges();

            return Ok();
        }
    }
}
