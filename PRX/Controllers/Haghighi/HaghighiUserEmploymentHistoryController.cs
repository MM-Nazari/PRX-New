using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.Haghighi;
using PRX.Models.Haghighi;

namespace PRX.Controllers.Haghighi
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "HaghighiUserEmploymentHistories")]
    public class HaghighiUserEmploymentHistoryController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public HaghighiUserEmploymentHistoryController(PRXDbContext context)
        {
            _context = context;
        }

        // GET: api/HaghighiUserEmploymentHistory
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllHaghighiUserEmploymentHistory()
        {
            var employmentHistory = _context.HaghighiUserEmploymentHistories.ToList();
            return Ok(employmentHistory);
        }

        // GET: api/HaghighiUserEmploymentHistory/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHaghighiUserEmploymentHistoryById(int id)
        {
            var employmentHistory = _context.HaghighiUserEmploymentHistories.FirstOrDefault(e => e.Id == id);
            if (employmentHistory == null)
            {
                return NotFound();
            }
            return Ok(employmentHistory);
        }

        // POST: api/HaghighiUserEmploymentHistory
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateHaghighiUserEmploymentHistory([FromBody] HaghighiUserEmploymentHistoryDto employmentHistoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employmentHistory = new HaghighiUserEmploymentHistory
            {
                UserId = employmentHistoryDto.UserId,
                EmployerLocation = employmentHistoryDto.EmployerLocation,
                MainActivity = employmentHistoryDto.MainActivity,
                Position = employmentHistoryDto.Position,
                StartDate = employmentHistoryDto.StartDate,
                EndDate = employmentHistoryDto.EndDate,
                WorkAddress = employmentHistoryDto.WorkAddress,
                WorkPhone = employmentHistoryDto.WorkPhone
            };

            _context.HaghighiUserEmploymentHistories.Add(employmentHistory);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetHaghighiUserEmploymentHistoryById), new { id = employmentHistory.Id }, employmentHistory);
        }

        // PUT: api/HaghighiUserEmploymentHistory/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHaghighiUserEmploymentHistory(int id, [FromBody] HaghighiUserEmploymentHistoryDto employmentHistoryDto)
        {
            var employmentHistory = _context.HaghighiUserEmploymentHistories.FirstOrDefault(e => e.Id == id);
            if (employmentHistory == null)
            {
                return NotFound();
            }

            employmentHistory.UserId = employmentHistoryDto.UserId;
            employmentHistory.EmployerLocation = employmentHistoryDto.EmployerLocation;
            employmentHistory.MainActivity = employmentHistoryDto.MainActivity;
            employmentHistory.Position = employmentHistoryDto.Position;
            employmentHistory.StartDate = employmentHistoryDto.StartDate;
            employmentHistory.EndDate = employmentHistoryDto.EndDate;
            employmentHistory.WorkAddress = employmentHistoryDto.WorkAddress;
            employmentHistory.WorkPhone = employmentHistoryDto.WorkPhone;

            _context.SaveChanges();

            return Ok(employmentHistory);
        }

        // DELETE: api/HaghighiUserEmploymentHistory/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHaghighiUserEmploymentHistory(int id)
        {
            var employmentHistory = _context.HaghighiUserEmploymentHistories.FirstOrDefault(e => e.Id == id);
            if (employmentHistory == null)
            {
                return NotFound();
            }

            _context.HaghighiUserEmploymentHistories.Remove(employmentHistory);
            _context.SaveChanges();

            return Ok();
        }

        // DELETE: api/HaghighiUserEmploymentHistory
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearHaghighiUserEmploymentHistory()
        {
            var employmentHistory = _context.HaghighiUserEmploymentHistories.ToList();
            _context.HaghighiUserEmploymentHistories.RemoveRange(employmentHistory);
            _context.SaveChanges();
            return Ok();
        }
    }
}
