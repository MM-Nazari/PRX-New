using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.Haghighi;
using PRX.Models.Haghighi;

namespace PRX.Controllers.Haghighi
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "HaghighiUserEducationStatuses")]
    public class HaghighiUserEducationStatusController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public HaghighiUserEducationStatusController(PRXDbContext context)
        {
            _context = context;
        }

        // GET: api/HaghighiUserEducationStatus
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllHaghighiUserEducationStatuses()
        {
            var educationStatuses = _context.HaghighiUserEducationStatuses.ToList();
            return Ok(educationStatuses);
        }

        // GET: api/HaghighiUserEducationStatus/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHaghighiUserEducationStatusById(int id)
        {
            var educationStatus = _context.HaghighiUserEducationStatuses.FirstOrDefault(e => e.Id == id);
            if (educationStatus == null)
            {
                return NotFound();
            }
            return Ok(educationStatus);
        }

        // POST: api/HaghighiUserEducationStatus
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateHaghighiUserEducationStatus([FromBody] HaghighiUserEducationStatusDto educationStatusDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var educationStatus = new HaghighiUserEducationStatus
            {
                UserId = educationStatusDto.UserId,
                LastDegree = educationStatusDto.LastDegree,
                FieldOfStudy = educationStatusDto.FieldOfStudy,
                GraduationYear = educationStatusDto.GraduationYear,
                IssuingAuthority = educationStatusDto.IssuingAuthority
            };

            _context.HaghighiUserEducationStatuses.Add(educationStatus);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetHaghighiUserEducationStatusById), new { id = educationStatus.Id }, educationStatus);
        }

        // PUT: api/HaghighiUserEducationStatus/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHaghighiUserEducationStatus(int id, [FromBody] HaghighiUserEducationStatusDto educationStatusDto)
        {
            var educationStatus = _context.HaghighiUserEducationStatuses.FirstOrDefault(e => e.Id == id);
            if (educationStatus == null)
            {
                return NotFound();
            }

            educationStatus.UserId = educationStatusDto.UserId;
            educationStatus.LastDegree = educationStatusDto.LastDegree;
            educationStatus.FieldOfStudy = educationStatusDto.FieldOfStudy;
            educationStatus.GraduationYear = educationStatusDto.GraduationYear;
            educationStatus.IssuingAuthority = educationStatusDto.IssuingAuthority;

            _context.SaveChanges();

            return Ok(educationStatus);
        }

        // DELETE: api/HaghighiUserEducationStatus/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHaghighiUserEducationStatus(int id)
        {
            var educationStatus = _context.HaghighiUserEducationStatuses.FirstOrDefault(e => e.Id == id);
            if (educationStatus == null)
            {
                return NotFound();
            }

            _context.HaghighiUserEducationStatuses.Remove(educationStatus);
            _context.SaveChanges();

            return Ok();
        }

        // DELETE: api/HaghighiUserEducationStatus
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearHaghighiUserEducationStatuses()
        {
            var educationStatuses = _context.HaghighiUserEducationStatuses.ToList();
            _context.HaghighiUserEducationStatuses.RemoveRange(educationStatuses);
            _context.SaveChanges();
            return Ok();
        }
    }
}
