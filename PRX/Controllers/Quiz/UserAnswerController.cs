using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        [HttpGet("GetOneByUserId{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetByUserId(int id)
        {
            var record = _context.UserAnswers.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
            if (record == null)
            {
                return NotFound();
            }
            return Ok(record);
        }


        // GET: api/UserAnswer/5
        [HttpGet("GetOneById{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var record = _context.UserAnswers.FirstOrDefault(e => e.Id == id && !e.IsDeleted);
            if (record == null)
            {
                return NotFound();
            }
            return Ok(record);
        }


        // GET: api/UserAnswer/5
        [HttpGet("GetAllByUserId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetByUserIds(int id)
        {
            var records = _context.UserAnswers.Where(e => e.UserId == id && !e.IsDeleted).ToList();
            if (records == null || records.Count == 0)
            {
                return NotFound();
            }
            return Ok(records);
        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create(int userId, [FromBody] UserAnswerDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Find the corresponding UserAnswerOption
            var userAnswer = _context.UserAnswers
                .FirstOrDefault(o => o.UserId == userId && !o.IsDeleted);

            if (userAnswer == null)
            {
                return BadRequest();
            }

            var record = new UserAnswer
            {
                UserId = userId, // Assign userId parameter here
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
            var record = _context.UserAnswers.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
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
            var record = _context.UserAnswers.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
            if (record == null)
            {
                return NotFound();
            }

            record.IsDeleted = true;
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
