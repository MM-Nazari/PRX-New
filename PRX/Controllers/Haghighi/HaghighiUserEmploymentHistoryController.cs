﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.Haghighi;
using PRX.Models.Haghighi;
using PRX.Utils;

namespace PRX.Controllers.Haghighi
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "HaghighiUserEmploymentHistories")]
    public class HaghighiUserEmploymentHistoryController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public HaghighiUserEmploymentHistoryController(PRXDbContext context)
        {
            _context = context;
        }

        // GET: api/HaghighiUserEmploymentHistory
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllHaghighiUserEmploymentHistory()
        {
            try
            {

                var employmentHistory = _context.HaghighiUserEmploymentHistories.ToList();
                return Ok(employmentHistory);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // GET: api/HaghighiUserEmploymentHistory/5
        [HttpGet("{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHaghighiUserEmploymentHistoryById(int requestId)
        {
            try
            {

                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }
                // Retrieve the user ID from the token
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

                var employmentHistory = _context.HaghighiUserEmploymentHistories.Where(e => e.RequestId == requestId && !e.IsDeleted).Select(r => new HaghighiUserEmploymentHistoryDto {
                    Id = r.Id,
                    RequestId = r.RequestId,
                    EmployerLocation = r.EmployerLocation,
                    MainActivity = r.MainActivity,
                    Position = r.Position,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    WorkAddress = r.WorkAddress,
                    WorkPhone = r.WorkPhone,
                    IsComplete = r.IsComplete,
                    IsDeleted = r.IsDeleted
                }).ToList();

                if (employmentHistory == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserEmploymentHistoryNotFound });
                }
                return Ok(employmentHistory);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // POST: api/HaghighiUserEmploymentHistory
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateHaghighiUserEmploymentHistory([FromBody] HaghighiUserEmploymentHistoryListDto employmentHistoryDto)
        {

            try
            {
                if (employmentHistoryDto.RequestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                foreach (var history in employmentHistoryDto.EmployementHistory) 
                {
                    var record = new HaghighiUserEmploymentHistory
                    {
                        RequestId = employmentHistoryDto.RequestId,
                        EmployerLocation = history.EmployerLocation,
                        MainActivity = history.MainActivity,
                        Position = history.Position,
                        StartDate = history.StartDate,
                        EndDate = history.EndDate,
                        WorkAddress = history.WorkAddress,
                        WorkPhone = history.WorkPhone
                    };
                    _context.HaghighiUserEmploymentHistories.Add(record);
                }


               
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status201Created, new { message = ResponseMessages.OK });
                //return CreatedAtAction(nameof(GetHaghighiUserEmploymentHistoryById), new { requestId = employmentHistory.Id }, employmentHistory);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        // PUT: api/HaghighiUserEmploymentHistory/5
        [HttpPut("{id}/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHaghighiUserEmploymentHistory(int id, int requestId, [FromBody] HaghighiUserEmploymentHistoryDto employmentHistoryDto)
        {

            try
            {

                if (id <= 0 && requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }
                // Retrieve the user ID from the token
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

                var employmentHistory = _context.HaghighiUserEmploymentHistories.FirstOrDefault(e => e.RequestId == requestId && e.Id == id && !e.IsDeleted);
                if (employmentHistory == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserEmploymentHistoryNotFound });
                }

                employmentHistory.RequestId = employmentHistoryDto.RequestId;
                employmentHistory.EmployerLocation = employmentHistoryDto.EmployerLocation;
                employmentHistory.MainActivity = employmentHistoryDto.MainActivity;
                employmentHistory.Position = employmentHistoryDto.Position;
                employmentHistory.StartDate = employmentHistoryDto.StartDate;
                employmentHistory.EndDate = employmentHistoryDto.EndDate;
                employmentHistory.WorkAddress = employmentHistoryDto.WorkAddress;
                employmentHistory.WorkPhone = employmentHistoryDto.WorkPhone;

                _context.SaveChanges();

                return Ok(employmentHistory);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }



        }

        // PATCH: api/HaghighiUserEmploymentHistory/5
        [HttpPatch("{id}/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult PatchHaghighiUserEmploymentHistory(int id, int requestId, [FromBody] JsonPatchDocument<HaghighiUserEmploymentHistoryDto> patchDoc)
        {
            try
            {
                if (id <= 0 || requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                // Retrieve the user ID from the token
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

                // Fetch the employment history record
                var employmentHistory = _context.HaghighiUserEmploymentHistories
                    .FirstOrDefault(e => e.RequestId == requestId && e.Id == id && !e.IsDeleted);

                if (employmentHistory == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserEmploymentHistoryNotFound });
                }

                // Convert the entity to a DTO object to apply the patch
                var employmentHistoryDto = new HaghighiUserEmploymentHistoryDto
                {
                    RequestId = employmentHistory.RequestId,
                    EmployerLocation = employmentHistory.EmployerLocation,
                    MainActivity = employmentHistory.MainActivity,
                    Position = employmentHistory.Position,
                    StartDate = employmentHistory.StartDate,
                    EndDate = employmentHistory.EndDate,
                    WorkAddress = employmentHistory.WorkAddress,
                    WorkPhone = employmentHistory.WorkPhone
                };

                // Apply the patch to the DTO
                patchDoc.ApplyTo(employmentHistoryDto, ModelState);

                // Check for validation errors after patch is applied
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Update the original entity with the patched values
                employmentHistory.RequestId = employmentHistoryDto.RequestId;
                employmentHistory.EmployerLocation = employmentHistoryDto.EmployerLocation;
                employmentHistory.MainActivity = employmentHistoryDto.MainActivity;
                employmentHistory.Position = employmentHistoryDto.Position;
                employmentHistory.StartDate = employmentHistoryDto.StartDate;
                employmentHistory.EndDate = employmentHistoryDto.EndDate;
                employmentHistory.WorkAddress = employmentHistoryDto.WorkAddress;
                employmentHistory.WorkPhone = employmentHistoryDto.WorkPhone;

                // Save changes to the database
                _context.SaveChanges();

                return Ok(employmentHistory);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }


        // DELETE: api/HaghighiUserEmploymentHistory/5
        [HttpDelete("{id}/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHaghighiUserEmploymentHistory(int id, int requestId)
        {
            try
            {

                if (id <= 0 && requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }
                // Retrieve the user ID from the token
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

                var employmentHistory = _context.HaghighiUserEmploymentHistories.FirstOrDefault(e => e.RequestId == requestId && e.Id == id && !e.IsDeleted);
                if (employmentHistory == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserEmploymentHistoryNotFound });
                }

                _context.HaghighiUserEmploymentHistories.Remove(employmentHistory);
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }



        }

        // DELETE: api/HaghighiUserEmploymentHistory
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearHaghighiUserEmploymentHistory()
        {
            try
            {

                var employmentHistory = _context.HaghighiUserEmploymentHistories.ToList();
                _context.HaghighiUserEmploymentHistories.RemoveRange(employmentHistory);
                _context.SaveChanges();
                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }


        // PUT: api/HaghighiUserProfile/complete/{id}
        [HttpPut("complete/{id}/{requestId}")]
        //[Authorize(Roles = "Admin")] // Assuming only admins can mark profiles as complete
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkEmploymentAsComplete(int id, int requestId)
        {

            try
            {
                if (id <= 0 && requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var employmentHistory = _context.HaghighiUserEmploymentHistories.FirstOrDefault(e => e.RequestId == requestId && e.Id == id && !e.IsDeleted);
                if (employmentHistory == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserEmploymentHistoryNotFound });
                }

                employmentHistory.IsComplete = true;
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpGet("isComplete/{id}/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CheckCompletionStatus(int id, int requestId)
        {
            try
            {
                if (id <= 0 && requestId <= 0)
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

                var record = _context.HaghighiUserEmploymentHistories.FirstOrDefault(e => e.RequestId == requestId && e.Id == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserEmploymentHistoryNotFound });
                }

                return Ok(new { isComplete = record.IsComplete });
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
        public IActionResult GetAllHaghighiUserEmploymentHistories()
        {
            try
            {
                var histories = _context.HaghighiUserEmploymentHistories.ToList();
                var historyDtos = histories.Select(history => new HaghighiUserEmploymentHistoryDto
                {
                    Id = history.Id,
                    RequestId = history.RequestId,
                    EmployerLocation = history.EmployerLocation,
                    MainActivity = history.MainActivity,
                    Position = history.Position,
                    StartDate = history.StartDate,
                    EndDate = history.EndDate,
                    WorkAddress = history.WorkAddress,
                    WorkPhone = history.WorkPhone,
                    IsComplete = history.IsComplete,
                    IsDeleted = history.IsDeleted
                }).ToList();

                return Ok(historyDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpGet("Admin/{id}/{requestId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHaghighiUserEmploymentHistoryByIdAdmin(int id, int requestId)
        {

            try
            {
                if (id <= 0 && requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var history = _context.HaghighiUserEmploymentHistories.FirstOrDefault(h => h.RequestId == requestId && h.Id == id && !h.IsDeleted);
                if (history == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserEmploymentHistoryNotFound });
                }

                var historyDto = new HaghighiUserEmploymentHistoryDto
                {
                    RequestId = history.RequestId,
                    EmployerLocation = history.EmployerLocation,
                    MainActivity = history.MainActivity,
                    Position = history.Position,
                    StartDate = history.StartDate,
                    EndDate = history.EndDate,
                    WorkAddress = history.WorkAddress,
                    WorkPhone = history.WorkPhone,
                    IsComplete = history.IsComplete,
                    IsDeleted = history.IsDeleted
                };

                return Ok(historyDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpPut("Admin/{id}/{requestId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHaghighiUserEmploymentHistoryAdmin(int id, int requestId, [FromBody] HaghighiUserEmploymentHistoryDto historyDto)
        {

            try
            {
                if (id <= 0 && requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var history = _context.HaghighiUserEmploymentHistories.FirstOrDefault(h => h.RequestId == requestId && h.Id == id && !h.IsDeleted);
                if (history == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserEmploymentHistoryNotFound });
                }

                history.EmployerLocation = historyDto.EmployerLocation;
                history.MainActivity = historyDto.MainActivity;
                history.Position = historyDto.Position;
                history.StartDate = historyDto.StartDate;
                history.EndDate = historyDto.EndDate;
                history.WorkAddress = historyDto.WorkAddress;
                history.WorkPhone = historyDto.WorkPhone;
                //history.IsComplete = historyDto.IsComplete;
                //history.IsDeleted = historyDto.IsDeleted;

                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpDelete("Admin/{id}/{requestId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHaghighiUserEmploymentHistoryAdmin(int id, int requestId)
        {
            try
            {
                if (id <= 0 && requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var history = _context.HaghighiUserEmploymentHistories.FirstOrDefault(h => h.RequestId == requestId && h.Id == id && !h.IsDeleted);
                if (history == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserEmploymentHistoryNotFound });
                }

                history.IsDeleted = true;
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
        public IActionResult ClearHaghighiUserEmploymentHistories()
        {
            try
            {
                _context.HaghighiUserEmploymentHistories.RemoveRange(_context.HaghighiUserEmploymentHistories);
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
