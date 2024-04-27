using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.Hoghooghi;
using PRX.Models.Hoghooghi;

namespace PRX.Controllers.Hoghooghi
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "HoghooghiUserBoardOfDirectors")]
    public class HoghooghiUserBoardOfDirectorsController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public HoghooghiUserBoardOfDirectorsController(PRXDbContext context)
        {
            _context = context;
        }

        // GET: api/HoghooghiUserBoardOfDirectors
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var records = _context.HoghooghiUserBoardOfDirectors.ToList();
            return Ok(records);
        }

        // GET: api/HoghooghiUserBoardOfDirectors/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var record = _context.HoghooghiUserBoardOfDirectors.FirstOrDefault(e => e.Id == id);
            if (record == null)
            {
                return NotFound();
            }
            return Ok(record);
        }

        // POST: api/HoghooghiUserBoardOfDirectors
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] HoghooghiUserBoardOfDirectorsDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var record = new HoghooghiUserBoardOfDirectors
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

            _context.HoghooghiUserBoardOfDirectors.Add(record);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = record.Id }, record);
        }

        // PUT: api/HoghooghiUserBoardOfDirectors/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(int id, [FromBody] HoghooghiUserBoardOfDirectorsDto dto)
        {
            var record = _context.HoghooghiUserBoardOfDirectors.FirstOrDefault(e => e.Id == id);
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

        // DELETE: api/HoghooghiUserBoardOfDirectors/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var record = _context.HoghooghiUserBoardOfDirectors.FirstOrDefault(e => e.Id == id);
            if (record == null)
            {
                return NotFound();
            }

            _context.HoghooghiUserBoardOfDirectors.Remove(record);
            _context.SaveChanges();

            return Ok();
        }

        // DELETE: api/HoghooghiUserBoardOfDirectors
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearAll()
        {
            _context.HoghooghiUserBoardOfDirectors.RemoveRange(_context.HoghooghiUserBoardOfDirectors);
            _context.SaveChanges();

            return Ok();
        }
    }
}
