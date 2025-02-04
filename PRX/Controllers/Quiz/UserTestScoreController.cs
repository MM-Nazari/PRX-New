﻿using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRX.Data;
using PRX.Dto.Quiz;
using PRX.Models.Quiz;
using PRX.Utils;

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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAll()
        {
            try
            {
                var records = _context.UserTestScores.ToList();
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // GET: api/UserTestScore/5
        [HttpGet("{requestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetById(int requestId)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var record = _context.UserTestScores.FirstOrDefault(e => e.RequestId == requestId && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.QuizScoreNotFound});
                }
                return Ok(record);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // POST: api/UserTestScore
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Create([FromBody] UserTestScoreDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var record = new UserTestScore
                {
                    RequestId = dto.RequestId,
                    QuizScore = dto.QuizScore
                };

                _context.UserTestScores.Add(record);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetById), new { requestId = record.Id }, record);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // PUT: api/UserTestScore/5
        [HttpPut("{requestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Update(int requestId, [FromBody] UserTestScoreDto dto)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var record = _context.UserTestScores.FirstOrDefault(e => e.RequestId == requestId && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.QuizScoreNotFound });
                }

                record.RequestId = dto.RequestId;
                record.QuizScore = dto.QuizScore;

                _context.SaveChanges();

                return Ok(record);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // PATCH: api/UserTestScore/{requestId}
        [HttpPatch("{requestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Patch(int requestId, [FromBody] JsonPatchDocument<UserTestScoreDto> patchDoc)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                // Fetch the existing record
                var record = _context.UserTestScores.FirstOrDefault(e => e.RequestId == requestId && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.QuizScoreNotFound });
                }

                // Create a DTO to apply the patch
                var recordDto = new UserTestScoreDto
                {
                    RequestId = record.RequestId,
                    QuizScore = record.QuizScore
                };

                // Apply the patch document to the DTO
                patchDoc.ApplyTo(recordDto);

                // Update the record with modified values from the DTO
                record.RequestId = recordDto.RequestId;
                record.QuizScore = recordDto.QuizScore;

                // Save changes to the database
                _context.SaveChanges();

                return Ok(record);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }


        [HttpPost("CalculateQuizScore/{requestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CalculateQuizScore(int requestId)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                // Retrieve all user's answer options with their scores
                var answerOptions = _context.UserAnswers
                    .Include(ua => ua.answerOption)
                    .Where(ua => ua.RequestId == requestId && !ua.IsDeleted && !ua.answerOption.IsDeleted)
                    .ToList();

                // Calculate total score
                int totalScore = 0;
                foreach (var answerOption in answerOptions)
                {
                    totalScore += answerOption.answerOption.Score;
                }

                // Retrieve or create UserTestScore record
                var userTestScore = _context.UserTestScores
                    .FirstOrDefault(uts => uts.RequestId == requestId && !uts.IsDeleted);

                if (userTestScore == null)
                {
                    userTestScore = new UserTestScore
                    {
                        RequestId = requestId,
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // DELETE: api/UserTestScore/5
        [HttpDelete("{requestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Delete(int requestId)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var record = _context.UserTestScores.FirstOrDefault(e => e.RequestId == requestId && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.QuizScoreNotFound });
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

        // DELETE: api/UserTestScore
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ClearAll()
        {
            try
            {
                _context.UserTestScores.RemoveRange(_context.UserTestScores);
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
