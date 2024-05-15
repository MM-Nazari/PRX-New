﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.Haghighi;
using PRX.Models.Haghighi;
using PRX.Utils;

namespace PRX.Controllers.Haghighi
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "HaghighiUserEducationStatuses")]
    public class HaghighiUserEducationStatusController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public HaghighiUserEducationStatusController(PRXDbContext context)
        {
            _context = context;
        }

        // GET: api/HaghighiUserEducationStatus
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllHaghighiUserEducationStatuses()
        {
            try 
            {

                var educationStatuses = _context.HaghighiUserEducationStatuses.ToList();
                return Ok(educationStatuses);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        // GET: api/HaghighiUserEducationStatus/5
        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHaghighiUserEducationStatusById(int id)
        {

            try
            {

                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }
                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Ensure that the user is updating their own profile
                if (id != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }
                var educationStatus = _context.HaghighiUserEducationStatuses.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
                if (educationStatus == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserEducationStatusNotFound});
                }
                return Ok(educationStatus);

            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        // POST: api/HaghighiUserEducationStatus
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateHaghighiUserEducationStatus([FromBody] HaghighiUserEducationStatusDto educationStatusDto)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var educationStatus = new HaghighiUserEducationStatus
                {
                    UserId = educationStatusDto.UserId,
                    LastDegree = educationStatusDto.LastDegree,
                    FieldOfStudy = educationStatusDto.FieldOfStudy,
                    GraduationYear = educationStatusDto.GraduationYear,
                    IssuingAuthority = educationStatusDto.IssuingAuthority
                };

                _context.HaghighiUserEducationStatuses.Add(educationStatus);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetHaghighiUserEducationStatusById), new { id = educationStatus.Id }, educationStatus);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        // PUT: api/HaghighiUserEducationStatus/5
        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHaghighiUserEducationStatus(int id, [FromBody] HaghighiUserEducationStatusDto educationStatusDto)
        {
            try
            {

                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }
                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Ensure that the user is updating their own profile
                if (id != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }
                var educationStatus = _context.HaghighiUserEducationStatuses.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
                if (educationStatus == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserEducationStatusNotFound });
                }

                educationStatus.UserId = educationStatusDto.UserId;
                educationStatus.LastDegree = educationStatusDto.LastDegree;
                educationStatus.FieldOfStudy = educationStatusDto.FieldOfStudy;
                educationStatus.GraduationYear = educationStatusDto.GraduationYear;
                educationStatus.IssuingAuthority = educationStatusDto.IssuingAuthority;

                _context.SaveChanges();

                return Ok(educationStatus);

            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }



        }

        // DELETE: api/HaghighiUserEducationStatus/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHaghighiUserEducationStatus(int id)
        {
            try
            {

                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }
                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Ensure that the user is updating their own profile
                if (id != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }
                var educationStatus = _context.HaghighiUserEducationStatuses.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
                if (educationStatus == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserEducationStatusNotFound });
                }

                _context.HaghighiUserEducationStatuses.Remove(educationStatus);
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });

            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // DELETE: api/HaghighiUserEducationStatus
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearHaghighiUserEducationStatuses()
        {
            try
            {
                var educationStatuses = _context.HaghighiUserEducationStatuses.ToList();
                _context.HaghighiUserEducationStatuses.RemoveRange(educationStatuses);
                _context.SaveChanges();
                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }


        // PUT: api/HaghighiUserProfile/complete/{id}
        [HttpPut("complete/{id}")]
        //[Authorize(Roles = "Admin")] // Assuming only admins can mark profiles as complete
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkEducationStatusAsComplete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var educationStatus = _context.HaghighiUserEducationStatuses.FirstOrDefault(e => e.UserId == id);
                if (educationStatus == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserEducationStatusNotFound });
                }

                educationStatus.IsComplete = true;
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }


        [HttpGet("Admin")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAllHaghighiUserEducationStatusesAdmin()
        {

            try
            {
                var statuses = _context.HaghighiUserEducationStatuses.ToList();
                var statusDtos = statuses.Select(status => new HaghighiUserEducationStatusDto
                {
                    UserId = status.UserId,
                    LastDegree = status.LastDegree,
                    FieldOfStudy = status.FieldOfStudy,
                    GraduationYear = status.GraduationYear,
                    IssuingAuthority = status.IssuingAuthority,
                    IsComplete = status.IsComplete,
                    IsDeleted = status.IsDeleted
                }).ToList();
                return Ok(statusDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpGet("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHaghighiUserEducationStatusByIdAdmin(int id)
        {

            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var status = _context.HaghighiUserEducationStatuses.FirstOrDefault(s => s.UserId == id && !s.IsDeleted);
                if (status == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserEducationStatusNotFound });
                }

                var statusDto = new HaghighiUserEducationStatusDto
                {
                    UserId = status.UserId,
                    LastDegree = status.LastDegree,
                    FieldOfStudy = status.FieldOfStudy,
                    GraduationYear = status.GraduationYear,
                    IssuingAuthority = status.IssuingAuthority,
                    IsComplete = status.IsComplete,
                    IsDeleted = status.IsDeleted
                };

                return Ok(statusDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpPut("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHaghighiUserEducationStatusAdmin(int id, [FromBody] HaghighiUserEducationStatusDto statusDto)
        {

            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var status = _context.HaghighiUserEducationStatuses.FirstOrDefault(s => s.UserId == id && !s.IsDeleted);
                if (status == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserEducationStatusNotFound });
                }

                status.LastDegree = statusDto.LastDegree;
                status.FieldOfStudy = statusDto.FieldOfStudy;
                status.GraduationYear = statusDto.GraduationYear;
                status.IssuingAuthority = statusDto.IssuingAuthority;
                //status.IsComplete = statusDto.IsComplete;
                //status.IsDeleted = statusDto.IsDeleted;

                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpDelete("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHaghighiUserEducationStatusAdmin(int id)
        {

            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var status = _context.HaghighiUserEducationStatuses.FirstOrDefault(s => s.UserId == id && !s.IsDeleted);
                if (status == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserEducationStatusNotFound });
                }

                status.IsDeleted = true;
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpDelete("Admin/Clear")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearHaghighiUserEducationStatusesAdmin()
        {

            try
            {

                _context.HaghighiUserEducationStatuses.RemoveRange(_context.HaghighiUserEducationStatuses);
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
