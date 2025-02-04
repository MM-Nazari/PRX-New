﻿using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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
        [HttpGet("GetByRequestId/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetByUserId(int requestId)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);
                // Fetch the request
                var request = _context.Requests.FirstOrDefault(r => r.Id == requestId);

                if (request == null)
                {
                    return NotFound(new { message = ResponseMessages.RequestNotFound });
                }

                // Ensure that the user associated with the request matches the token user ID
                if (request.UserId != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }

                var record = _context.UserAnswers.Where(e => e.RequestId == requestId && !e.IsDeleted).Select(r => new UserAnswerDto
                {
                    Id = r.Id,
                    RequestId = requestId,
                    AnswerOptionId = r.AnswerOptionId,
                    AnswerText = r.AnswerText,
                    IsDeleted = r.IsDeleted
                }).ToList();

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
        [HttpGet("GetOneById/{id}")]
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

                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);
                if (id != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
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


        [HttpPost]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] UserAnswerListDto dto)
        {
            try
            {
                if (dto.RequestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                foreach (var answer in dto.Answers)
                {
                    var record = new UserAnswer
                    {
                        RequestId = dto.RequestId,
                        AnswerOptionId = answer.AnswerOptionId,
                        AnswerText = answer.AnswerText
                    };

                    _context.UserAnswers.Add(record);
                }

                _context.SaveChanges();

                return StatusCode(StatusCodes.Status201Created, new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }


        // PUT: api/UserAnswer/5
        [HttpPut("{id}/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(int id, int requestId, [FromBody] UserAnswerDto dto)
        {
            try
            {
                if (id <= 0 || requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);
                // Fetch the request
                var request = _context.Requests.FirstOrDefault(r => r.Id == requestId);

                if (request == null)
                {
                    return NotFound(new { message = ResponseMessages.RequestNotFound });
                }

                // Ensure that the user associated with the request matches the token user ID
                if (request.UserId != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }

                var record = _context.UserAnswers.FirstOrDefault(e => e.RequestId == requestId && e.Id == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.QuizAnswerNotFound});
                }

                record.RequestId = dto.RequestId;
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


        // PATCH: api/UserAnswer/{id}/{requestId}
        [HttpPatch("{id}/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Patch(int id, int requestId, [FromBody] JsonPatchDocument<UserAnswerDto> patchDoc)
        {
            try
            {
                if (id <= 0 || requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Fetch the request
                var request = _context.Requests.FirstOrDefault(r => r.Id == requestId);

                if (request == null)
                {
                    return NotFound(new { message = ResponseMessages.RequestNotFound });
                }

                // Ensure that the user associated with the request matches the token user ID
                if (request.UserId != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }

                // Fetch the existing record
                var record = _context.UserAnswers.FirstOrDefault(e => e.RequestId == requestId && e.Id == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.QuizAnswerNotFound });
                }

                // Create a DTO to apply the patch
                var recordDto = new UserAnswerDto
                {
                    RequestId = record.RequestId,
                    AnswerOptionId = record.AnswerOptionId,
                    AnswerText = record.AnswerText
                };

                // Apply the patch document to the DTO
                patchDoc.ApplyTo(recordDto);

                // Update the record with modified values from the DTO
                record.RequestId = recordDto.RequestId;
                record.AnswerOptionId = recordDto.AnswerOptionId;
                record.AnswerText = recordDto.AnswerText;

                // Save changes to the database
                _context.SaveChanges();

                return Ok(record);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }


        // DELETE: api/UserAnswer/5
        [HttpDelete("{id}/{requestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id, int requestId)
        {
            try
            {
                if (id <= 0 || requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var record = _context.UserAnswers.FirstOrDefault(e => e.RequestId == requestId && e.Id == id && !e.IsDeleted);
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
