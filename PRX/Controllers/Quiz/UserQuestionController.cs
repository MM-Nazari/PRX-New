using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.Quiz;
using PRX.Models.Quiz;
using PRX.Utils;

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
            try
            {
                var records = _context.UserQuestions.ToList();
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // GET: api/UserQuestion/5
        [HttpGet("{id}")]
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

                var record = _context.UserQuestions.FirstOrDefault(e => e.Id == id);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.QuizQuestionNotFound});
                }
                return Ok(record);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // POST: api/UserQuestion
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Create([FromBody] UserQuestionDto dto)
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // GET: api/UserQuestion/GetByFilter
        [HttpGet("GetByTypeOrNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetByFilter([FromQuery] QuestionsFilterDto filterDto)
        {
            //try
            //{
            //    if (!ModelState.IsValid)
            //    {
            //        return BadRequest(ModelState);
            //    }

            //    var query = _context.UserQuestions.AsQueryable();

            //    if (!string.IsNullOrEmpty(filterDto.Type))
            //    {
            //        query = query.Where(q => q.Type == filterDto.Type);
            //    }

            //    if (filterDto.QuestionNumber > 0)
            //    {
            //        query = query.Where(q => q.QuestionNumber == filterDto.QuestionNumber);
            //    }

            //    var userQuestions = query.ToList();

            //    if (userQuestions == null || userQuestions.Count == 0)
            //    {
            //        return NotFound(new { message = "User questions not found for the specified filter criteria." });
            //    }

            //    return Ok(userQuestions);
            //}

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var query = _context.UserQuestions.AsQueryable();

                if (!string.IsNullOrEmpty(filterDto.Type))
                {
                    query = query.Where(q => q.Type == filterDto.Type);
                }

                if (filterDto.QuestionNumber > 0 && string.IsNullOrEmpty(filterDto.Type))
                {
                    // If no type is specified and question number is provided, filter by question number only
                    query = query.Where(q => q.QuestionNumber == filterDto.QuestionNumber);
                }
                else if (filterDto.QuestionNumber > 0)
                {
                    // If type is specified and question number is provided, filter by both type and question number
                    query = query.Where(q => q.Type == filterDto.Type && q.QuestionNumber == filterDto.QuestionNumber);
                }

                var userQuestions = query.ToList();

                if (userQuestions == null || userQuestions.Count == 0)
                {
                    return NotFound(new { message = ResponseMessages.QuizQuestionFilterNotFound });
                }

                return Ok(userQuestions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }



        // PUT: api/UserQuestion/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Update(int id, [FromBody] UserQuestionDto dto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var record = _context.UserQuestions.FirstOrDefault(e => e.Id == id);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.QuizQuestionNotFound });
                }

                record.Text = dto.Text;

                _context.SaveChanges();

                return Ok(record);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // DELETE: api/UserQuestion/5
        [HttpDelete("{id}")]
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

                var record = _context.UserQuestions.FirstOrDefault(e => e.Id == id);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.QuizQuestionNotFound });
                }

                _context.UserQuestions.Remove(record);
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // DELETE: api/UserQuestion
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ClearAll()
        {
            try
            {
                _context.UserQuestions.RemoveRange(_context.UserQuestions);
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
