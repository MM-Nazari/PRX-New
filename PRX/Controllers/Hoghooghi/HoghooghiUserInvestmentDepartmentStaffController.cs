using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.Hoghooghi;
using PRX.Models.Hoghooghi;

namespace PRX.Controllers.Hoghooghi
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "HoghooghiUserInvestmentDepartmentStaff")]
    public class HoghooghiUserInvestmentDepartmentStaffController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public HoghooghiUserInvestmentDepartmentStaffController(PRXDbContext context)
        {
            _context = context;
        }

        // GET: api/HoghooghiUserInvestmentDepartmentStaff
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var records = _context.HoghooghiUserInvestmentDepartmentStaff.ToList();
            return Ok(records);
        }

        // GET: api/HoghooghiUserInvestmentDepartmentStaff/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var record = _context.HoghooghiUserInvestmentDepartmentStaff.FirstOrDefault(e => e.Id == id);
            if (record == null)
            {
                return NotFound();
            }
            return Ok(record);
        }

        // POST: api/HoghooghiUserInvestmentDepartmentStaff
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] HoghooghiUserInvestmentDepartmentStaffDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var record = new HoghooghiUserInvestmentDepartmentStaff
            {
                UserId = dto.UserId,
                FullName = dto.FullName,
                Position = dto.Position,
                EducationalLevel = dto.EducationalLevel,
                FieldOfStudy = dto.FieldOfStudy,
                ExecutiveExperience = dto.ExecutiveExperience,
                FamiliarityWithCapitalMarket = dto.FamiliarityWithCapitalMarket,
                PersonalInvestmentExperienceInStockExchange = dto.PersonalInvestmentExperienceInStockExchange
            };

            _context.HoghooghiUserInvestmentDepartmentStaff.Add(record);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = record.Id }, record);
        }

        // PUT: api/HoghooghiUserInvestmentDepartmentStaff/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(int id, [FromBody] HoghooghiUserInvestmentDepartmentStaffDto dto)
        {
            var record = _context.HoghooghiUserInvestmentDepartmentStaff.FirstOrDefault(e => e.Id == id);
            if (record == null)
            {
                return NotFound();
            }

            record.UserId = dto.UserId;
            record.FullName = dto.FullName;
            record.Position = dto.Position;
            record.EducationalLevel = dto.EducationalLevel;
            record.FieldOfStudy = dto.FieldOfStudy;
            record.ExecutiveExperience = dto.ExecutiveExperience;
            record.FamiliarityWithCapitalMarket = dto.FamiliarityWithCapitalMarket;
            record.PersonalInvestmentExperienceInStockExchange = dto.PersonalInvestmentExperienceInStockExchange;

            _context.SaveChanges();

            return Ok(record);
        }

        // DELETE: api/HoghooghiUserInvestmentDepartmentStaff/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var record = _context.HoghooghiUserInvestmentDepartmentStaff.FirstOrDefault(e => e.Id == id);
            if (record == null)
            {
                return NotFound();
            }

            _context.HoghooghiUserInvestmentDepartmentStaff.Remove(record);
            _context.SaveChanges();

            return Ok();
        }

        // DELETE: api/HoghooghiUserInvestmentDepartmentStaff
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearAll()
        {
            _context.HoghooghiUserInvestmentDepartmentStaff.RemoveRange(_context.HoghooghiUserInvestmentDepartmentStaff);
            _context.SaveChanges();

            return Ok();
        }
    }
}
