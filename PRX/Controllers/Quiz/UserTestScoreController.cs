using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRX.Data;
using PRX.Dto.Quiz;
using PRX.Models.Quiz;

namespace PRX.Controllers.Quiz
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "UserTestScores")]
    public class UserTestScoreController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public UserTestScoreController(PRXDbContext context)
        {
            _context = context;
        }

        // GET: api/UserTestScore
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var records = _context.UserTestScores.ToList();
            return Ok(records);
        }

        // GET: api/UserTestScore/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var record = _context.UserTestScores.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
            if (record == null)
            {
                return NotFound();
            }
            return Ok(record);
        }

        // POST: api/UserTestScore
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] UserTestScoreDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var record = new UserTestScore
            {
                UserId = dto.UserId,
                QuizScore = dto.QuizScore
            };

            _context.UserTestScores.Add(record);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = record.Id }, record);
        }

        // PUT: api/UserTestScore/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(int id, [FromBody] UserTestScoreDto dto)
        {
            var record = _context.UserTestScores.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
            if (record == null)
            {
                return NotFound();
            }

            record.UserId = dto.UserId;
            record.QuizScore = dto.QuizScore;

            _context.SaveChanges();

            return Ok(record);
        }

        [HttpPost("CalculateQuizScore/{userId}")]
        public IActionResult CalculateQuizScore(int userId)
        {
            // Retrieve all user's answer options with their scores
            var answerOptions = _context.UserAnswers
                .Include(ua => ua.answerOption)
                .Where(ua => ua.UserId == userId && !ua.IsDeleted && !ua.answerOption.IsDeleted)
                .ToList();

            // Calculate total score
            int totalScore = 0;
            foreach (var answerOption in answerOptions)
            {
                totalScore += answerOption.answerOption.Score;
            }

            // Retrieve or create UserTestScore record
            var userTestScore = _context.UserTestScores
                .FirstOrDefault(uts => uts.UserId == userId && !uts.IsDeleted);

            if (userTestScore == null)
            {
                userTestScore = new UserTestScore
                {
                    UserId = userId,
                    QuizScore = totalScore
                };
                _context.UserTestScores.Add(userTestScore);
            }
            else
            {
                userTestScore.QuizScore = totalScore;
                _context.UserTestScores.Update(userTestScore);
            }

            // Save changes
            _context.SaveChanges();

            return Ok(userTestScore);
        }

        // DELETE: api/UserTestScore/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var record = _context.UserTestScores.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
            if (record == null)
            {
                return NotFound();
            }

            record.IsDeleted = true;
            _context.SaveChanges();

            return Ok();
        }

        // DELETE: api/UserTestScore
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearAll()
        {
            _context.UserTestScores.RemoveRange(_context.UserTestScores);
            _context.SaveChanges();

            return Ok();
        }
    }
}
