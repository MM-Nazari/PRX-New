using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.Quiz;
using PRX.Models.Quiz;

namespace PRX.Controllers.Quiz
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "UserAnswerOptions")]
    public class UserAnswerOptionController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public UserAnswerOptionController(PRXDbContext context)
        {
            _context = context;
        }

        // GET: api/UserAnswerOption
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var records = _context.UserAnswerOptions.ToList();
            return Ok(records);
        }

        // GET: api/UserAnswerOption/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var record = _context.UserAnswerOptions.FirstOrDefault(e => e.Id == id);
            if (record == null)
            {
                return NotFound();
            }
            return Ok(record);
        }

        // POST: api/UserAnswerOption
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] UserAnswerOptionDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var record = new UserAnswerOption
            {
                QuestionId = dto.QuestionId,
                Text = dto.Text
            };

            _context.UserAnswerOptions.Add(record);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = record.Id }, record);
        }

        // PUT: api/UserAnswerOption/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(int id, [FromBody] UserAnswerOptionDto dto)
        {
            var record = _context.UserAnswerOptions.FirstOrDefault(e => e.Id == id);
            if (record == null)
            {
                return NotFound();
            }

            record.QuestionId = dto.QuestionId;
            record.Text = dto.Text;

            _context.SaveChanges();

            return Ok(record);
        }

        // DELETE: api/UserAnswerOption/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var record = _context.UserAnswerOptions.FirstOrDefault(e => e.Id == id);
            if (record == null)
            {
                return NotFound();
            }

            _context.UserAnswerOptions.Remove(record);
            _context.SaveChanges();

            return Ok();
        }

        // DELETE: api/UserAnswerOption
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearAll()
        {
            _context.UserAnswerOptions.RemoveRange(_context.UserAnswerOptions);
            _context.SaveChanges();

            return Ok();
        }
    }
}
