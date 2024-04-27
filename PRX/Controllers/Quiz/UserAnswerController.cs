using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.Quiz;
using PRX.Models.Quiz;

namespace PRX.Controllers.Quiz
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "UserAnswers")]
    public class UserAnswerController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public UserAnswerController(PRXDbContext context)
        {
            _context = context;
        }

        // GET: api/UserAnswer
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        
        public IActionResult GetAll()
        {
            var records = _context.UserAnswers.ToList();
            return Ok(records);
        }

        // GET: api/UserAnswer/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var record = _context.UserAnswers.FirstOrDefault(e => e.Id == id);
            if (record == null)
            {
                return NotFound();
            }
            return Ok(record);
        }

        // POST: api/UserAnswer
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] UserAnswerDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var record = new UserAnswer
            {
                UserId = dto.UserId,
                AnswerOptionId = dto.AnswerOptionId,
                AnswerText = dto.AnswerText
            };

            _context.UserAnswers.Add(record);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = record.Id }, record);
        }

        // PUT: api/UserAnswer/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(int id, [FromBody] UserAnswerDto dto)
        {
            var record = _context.UserAnswers.FirstOrDefault(e => e.Id == id);
            if (record == null)
            {
                return NotFound();
            }

            record.UserId = dto.UserId;
            record.AnswerOptionId = dto.AnswerOptionId;
            record.AnswerText = dto.AnswerText;

            _context.SaveChanges();

            return Ok(record);
        }

        // DELETE: api/UserAnswer/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var record = _context.UserAnswers.FirstOrDefault(e => e.Id == id);
            if (record == null)
            {
                return NotFound();
            }

            _context.UserAnswers.Remove(record);
            _context.SaveChanges();

            return Ok();
        }

        // DELETE: api/UserAnswer
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearAll()
        {
            _context.UserAnswers.RemoveRange(_context.UserAnswers);
            _context.SaveChanges();

            return Ok();
        }
    }
}
