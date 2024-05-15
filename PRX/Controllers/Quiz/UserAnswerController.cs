using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PRX.Data;
using PRX.Dto.Quiz;
using PRX.Models.Quiz;
using PRX.Utils;

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
            try 
            {
                var records = _context.UserAnswers.ToList();
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // GET: api/UserAnswer/5
        [HttpGet("GetOneByUserId{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetByUserId(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var record = _context.UserAnswers.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.QuizAnswerNotFound});
                }
                return Ok(record);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }


        // GET: api/UserAnswer/5
        [HttpGet("GetOneById{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var record = _context.UserAnswers.FirstOrDefault(e => e.Id == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.QuizAnswerNotFound });
                }
                return Ok(record);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }


        // GET: api/UserAnswer/5
        [HttpGet("GetAllByUserId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetByUserIds(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var records = _context.UserAnswers.Where(e => e.UserId == id && !e.IsDeleted).ToList();
                if (records == null || records.Count == 0)
                {
                    return NotFound(new { message = ResponseMessages.QuizAnswerNotFound });
                }
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create(int userId, [FromBody] UserAnswerDto dto)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Find the corresponding UserAnswerOption
                var userAnswer = _context.UserAnswers
                    .FirstOrDefault(o => o.UserId == userId && !o.IsDeleted);

                if (userAnswer == null)
                {
                    return BadRequest(new { message = ResponseMessages.QuizAnswerIsNull});
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }


        // PUT: api/UserAnswer/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(int id, [FromBody] UserAnswerDto dto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var record = _context.UserAnswers.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.QuizAnswerNotFound});
                }

                record.UserId = dto.UserId;
                record.AnswerOptionId = dto.AnswerOptionId;
                record.AnswerText = dto.AnswerText;

                _context.SaveChanges();

                return Ok(record);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // DELETE: api/UserAnswer/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var record = _context.UserAnswers.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.QuizAnswerNotFound });
                }

                record.IsDeleted = true;
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // DELETE: api/UserAnswer
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearAll()
        {
            try
            {
                _context.UserAnswers.RemoveRange(_context.UserAnswers);
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }
    }
}
