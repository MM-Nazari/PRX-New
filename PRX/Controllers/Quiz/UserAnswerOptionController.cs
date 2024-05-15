using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.Quiz;
using PRX.Models.Quiz;
using PRX.Utils;

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
            try
            {
                var records = _context.UserAnswerOptions.ToList();
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // GET: api/UserAnswerOption/5
        [HttpGet("GetById/{id}")]
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

                var record = _context.UserAnswerOptions.FirstOrDefault(e => e.Id == id);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.QuizAnswerOptionNotFound});
                }
                return Ok(record);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // GET: api/UserAnswerOption/GetByQuestionId/5
        [HttpGet("GetByQuestionId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetByQuestionId(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var records = _context.UserAnswerOptions.Where(e => e.QuestionId == id).ToList();
                if (records == null || records.Count == 0)
                {
                    return NotFound(new { message = ResponseMessages.QuizAnswerOptionNotFound });
                }
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // POST: api/UserAnswerOption
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] UserAnswerOptionDto dto)
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // PUT: api/UserAnswerOption/5
        [HttpPut("PutById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(int id, [FromBody] UserAnswerOptionDto dto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var record = _context.UserAnswerOptions.FirstOrDefault(e => e.Id == id);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.QuizAnswerOptionNotFound });
                }

                record.QuestionId = dto.QuestionId;
                record.Text = dto.Text;

                _context.SaveChanges();

                return Ok(record);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // DELETE: api/UserAnswerOption/5
        [HttpDelete("DeleteById/{id}")]
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

                var record = _context.UserAnswerOptions.FirstOrDefault(e => e.Id == id);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.QuizAnswerOptionNotFound });
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

        // DELETE: api/UserAnswerOption/DeleteByQuestionId/5
        [HttpDelete("DeleteByQuestionId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteByQuestionId(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var records = _context.UserAnswerOptions.Where(e => e.QuestionId == id).ToList();
                if (records == null || records.Count == 0)
                {
                    return NotFound(new { message = ResponseMessages.QuizAnswerOptionNotFound }); ;
                }

                foreach (var record in records)
                {
                    record.IsDeleted = true;
                }

                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }


        // DELETE: api/UserAnswerOption
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearAll()
        {
            try
            {
                _context.UserAnswerOptions.RemoveRange(_context.UserAnswerOptions);
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
