using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.Quiz;
using PRX.Models.Quiz;

namespace PRX.Controllers.Quiz
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "UserQuestions")]
    public class UserQuestionController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public UserQuestionController(PRXDbContext context)
        {
            _context = context;
        }

        // GET: api/UserQuestion
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var records = _context.UserQuestions.ToList();
            return Ok(records);
        }

        // GET: api/UserQuestion/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var record = _context.UserQuestions.FirstOrDefault(e => e.Id == id);
            if (record == null)
            {
                return NotFound();
            }
            return Ok(record);
        }

        // POST: api/UserQuestion
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] UserQuestionDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var record = new UserQuestion
            {
                Text = dto.Text
            };

            _context.UserQuestions.Add(record);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = record.Id }, record);
        }

        // PUT: api/UserQuestion/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(int id, [FromBody] UserQuestionDto dto)
        {
            var record = _context.UserQuestions.FirstOrDefault(e => e.Id == id);
            if (record == null)
            {
                return NotFound();
            }

            record.Text = dto.Text;

            _context.SaveChanges();

            return Ok(record);
        }

        // DELETE: api/UserQuestion/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var record = _context.UserQuestions.FirstOrDefault(e => e.Id == id);
            if (record == null)
            {
                return NotFound();
            }

            _context.UserQuestions.Remove(record);
            _context.SaveChanges();

            return Ok();
        }

        // DELETE: api/UserQuestion
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearAll()
        {
            _context.UserQuestions.RemoveRange(_context.UserQuestions);
            _context.SaveChanges();

            return Ok();
        }
    }
}
