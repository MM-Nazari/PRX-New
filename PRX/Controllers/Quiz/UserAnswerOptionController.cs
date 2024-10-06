using Microsoft.AspNetCore.JsonPatch;
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        // POST: api/UserAnswerOption/GetByQuestionType
        [HttpPost("GetByQuestionType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetByQuestionType([FromBody] AnswerOptionsFilterDto filterDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var questions = _context.UserQuestions.Where(q => q.Type == filterDto.Type).ToList();
                if (questions == null || questions.Count == 0)
                {
                    return NotFound(new { message = $"Questions with type '{filterDto.Type}' not found." });
                }

                //var questionIds = questions.Select(q => q.Id).ToList();
                //var answerOptions = _context.UserAnswerOptions.Where(ao => questionIds.Contains(ao.QuestionId)).ToList();
                //if (answerOptions == null || answerOptions.Count == 0)
                //{
                //    return NotFound(new { message = "Answer options not found for the specified question type." });
                //}

                var questionIds = questions.Select(q => q.Id).ToList();
                var answerOptions = _context.UserAnswerOptions
                    .Where(ao => questionIds.Contains(ao.QuestionId))
                    .Select(ao => new AnswerOptionsFilterDto
                    {
                        QuestionId = ao.QuestionId,
                        Text = ao.Text
                    })
                    .ToList();

                return Ok(answerOptions);
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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


        // PATCH: api/UserAnswerOption/{id}
        [HttpPatch("PatchById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Patch(int id, [FromBody] JsonPatchDocument<UserAnswerOptionDto> patchDoc)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                // Fetch the existing record
                var record = _context.UserAnswerOptions.FirstOrDefault(e => e.Id == id);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.QuizAnswerOptionNotFound });
                }

                // Create a DTO to apply the patch
                var recordDto = new UserAnswerOptionDto
                {
                    QuestionId = record.QuestionId,
                    Text = record.Text
                };

                // Apply the patch document to the DTO
                patchDoc.ApplyTo(recordDto);

                // Update the record with modified values from the DTO
                record.QuestionId = recordDto.QuestionId;
                record.Text = recordDto.Text;

                // Save changes to the database
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
