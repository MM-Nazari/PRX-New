using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.Haghighi;
using PRX.Models.Haghighi;

namespace PRX.Controllers.Haghighi
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "HaghighiUserRelationships")]
    public class HaghighiUserRelationshipsController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public HaghighiUserRelationshipsController(PRXDbContext context)
        {
            _context = context;
        }

        // GET: api/HaghighiUserRelationships
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllHaghighiUserRelationships()
        {
            var relationships = _context.HaghighiUserRelationships.ToList();
            return Ok(relationships);
        }

        // GET: api/HaghighiUserRelationships/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHaghighiUserRelationshipsById(int id)
        {
            var relationship = _context.HaghighiUserRelationships.FirstOrDefault(e => e.Id == id);
            if (relationship == null)
            {
                return NotFound();
            }
            return Ok(relationship);
        }

        // POST: api/HaghighiUserRelationships
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateHaghighiUserRelationships([FromBody] HaghighiUserRelationshipsDto relationshipDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var relationship = new HaghighiUserRelationships
            {
                UserId = relationshipDto.UserId,
                FullName = relationshipDto.FullName,
                RelationshipStatus = relationshipDto.RelationshipStatus,
                BirthYear = relationshipDto.BirthYear,
                EducationLevel = relationshipDto.EducationLevel,
                EmploymentStatus = relationshipDto.EmploymentStatus,
                AverageMonthlyIncome = relationshipDto.AverageMonthlyIncome,
                AverageMonthlyExpense = relationshipDto.AverageMonthlyExpense,
                ApproximateAssets = relationshipDto.ApproximateAssets,
                ApproximateLiabilities = relationshipDto.ApproximateLiabilities
            };

            _context.HaghighiUserRelationships.Add(relationship);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetHaghighiUserRelationshipsById), new { id = relationship.Id }, relationship);
        }

        // PUT: api/HaghighiUserRelationships/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHaghighiUserRelationships(int id, [FromBody] HaghighiUserRelationshipsDto relationshipDto)
        {
            var relationship = _context.HaghighiUserRelationships.FirstOrDefault(e => e.Id == id);
            if (relationship == null)
            {
                return NotFound();
            }

            relationship.UserId = relationshipDto.UserId;
            relationship.FullName = relationshipDto.FullName;
            relationship.RelationshipStatus = relationshipDto.RelationshipStatus;
            relationship.BirthYear = relationshipDto.BirthYear;
            relationship.EducationLevel = relationshipDto.EducationLevel;
            relationship.EmploymentStatus = relationshipDto.EmploymentStatus;
            relationship.AverageMonthlyIncome = relationshipDto.AverageMonthlyIncome;
            relationship.AverageMonthlyExpense = relationshipDto.AverageMonthlyExpense;
            relationship.ApproximateAssets = relationshipDto.ApproximateAssets;
            relationship.ApproximateLiabilities = relationshipDto.ApproximateLiabilities;

            _context.SaveChanges();

            return Ok(relationship);
        }

        // DELETE: api/HaghighiUserRelationships/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHaghighiUserRelationships(int id)
        {
            var relationship = _context.HaghighiUserRelationships.FirstOrDefault(e => e.Id == id);
            if (relationship == null)
            {
                return NotFound();
            }

            _context.HaghighiUserRelationships.Remove(relationship);
            _context.SaveChanges();

            return Ok();
        }

        // DELETE: api/HaghighiUserRelationships/clear
        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearAllHaghighiUserRelationships()
        {
            _context.HaghighiUserRelationships.RemoveRange(_context.HaghighiUserRelationships);
            _context.SaveChanges();

            return Ok();
        }
    }
}
