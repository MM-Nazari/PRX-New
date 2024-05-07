﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.Haghighi;
using PRX.Models.Haghighi;

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
            var employmentHistory = _context.HaghighiUserEmploymentHistories.ToList();
            return Ok(employmentHistory);
        }

        // GET: api/HaghighiUserEmploymentHistory/5
        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHaghighiUserEmploymentHistoryById(int id)
        {
            try
            {

                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Ensure that the user is updating their own profile
                if (id != tokenUserId)
                {
                    return Forbid(); // Or return 403 Forbidden
                }
                var employmentHistory = _context.HaghighiUserEmploymentHistories.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
                if (employmentHistory == null)
                {
                    return NotFound();
                }
                return Ok(employmentHistory);

            }

            catch (Exception ex)
            {
               
                return BadRequest();
            }
       
        }

        // POST: api/HaghighiUserEmploymentHistory
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateHaghighiUserEmploymentHistory([FromBody] HaghighiUserEmploymentHistoryDto employmentHistoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employmentHistory = new HaghighiUserEmploymentHistory
            {
                UserId = employmentHistoryDto.UserId,
                EmployerLocation = employmentHistoryDto.EmployerLocation,
                MainActivity = employmentHistoryDto.MainActivity,
                Position = employmentHistoryDto.Position,
                StartDate = employmentHistoryDto.StartDate,
                EndDate = employmentHistoryDto.EndDate,
                WorkAddress = employmentHistoryDto.WorkAddress,
                WorkPhone = employmentHistoryDto.WorkPhone
            };

            _context.HaghighiUserEmploymentHistories.Add(employmentHistory);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetHaghighiUserEmploymentHistoryById), new { id = employmentHistory.Id }, employmentHistory);
        }

        // PUT: api/HaghighiUserEmploymentHistory/5
        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHaghighiUserEmploymentHistory(int id, [FromBody] HaghighiUserEmploymentHistoryDto employmentHistoryDto)
        {

            try
            {

                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Ensure that the user is updating their own profile
                if (id != tokenUserId)
                {
                    return Forbid(); // Or return 403 Forbidden
                }
                var employmentHistory = _context.HaghighiUserEmploymentHistories.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
                if (employmentHistory == null)
                {
                    return NotFound();
                }

                employmentHistory.UserId = employmentHistoryDto.UserId;
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

                return BadRequest();
            }


     
        }

        // DELETE: api/HaghighiUserEmploymentHistory/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHaghighiUserEmploymentHistory(int id)
        {
            try
            {

                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Ensure that the user is updating their own profile
                if (id != tokenUserId)
                {
                    return Forbid(); // Or return 403 Forbidden
                }

                var employmentHistory = _context.HaghighiUserEmploymentHistories.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
                if (employmentHistory == null)
                {
                    return NotFound();
                }

                _context.HaghighiUserEmploymentHistories.Remove(employmentHistory);
                _context.SaveChanges();

                return Ok();

            }

            catch (Exception ex)
            {
                
                return BadRequest();
            }


        
        }

        // DELETE: api/HaghighiUserEmploymentHistory
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearHaghighiUserEmploymentHistory()
        {
            var employmentHistory = _context.HaghighiUserEmploymentHistories.ToList();
            _context.HaghighiUserEmploymentHistories.RemoveRange(employmentHistory);
            _context.SaveChanges();
            return Ok();
        }


        // PUT: api/HaghighiUserProfile/complete/{id}
        [HttpPut("complete/{id}")]
        //[Authorize(Roles = "Admin")] // Assuming only admins can mark profiles as complete
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkEmploymentAsComplete(int id)
        {
            var employmentHistory = _context.HaghighiUserEmploymentHistories.FirstOrDefault(e => e.UserId == id);
            if (employmentHistory == null)
            {
                return NotFound();
            }

            employmentHistory.IsComplete = true;
            _context.SaveChanges();

            return Ok();
        }


        [HttpGet("Admin")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAllHaghighiUserEmploymentHistories()
        {
            var histories = _context.HaghighiUserEmploymentHistories.ToList();
            var historyDtos = histories.Select(history => new HaghighiUserEmploymentHistoryDto
            {
                UserId = history.UserId,
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

        [HttpGet("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHaghighiUserEmploymentHistoryByIdAdmin(int id)
        {
            var history = _context.HaghighiUserEmploymentHistories.FirstOrDefault(h => h.UserId == id && !h.IsDeleted);
            if (history == null)
            {
                return NotFound();
            }

            var historyDto = new HaghighiUserEmploymentHistoryDto
            {
                UserId = history.UserId,
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

        [HttpPut("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHaghighiUserEmploymentHistoryAdmin(int id, [FromBody] HaghighiUserEmploymentHistoryDto historyDto)
        {
            var history = _context.HaghighiUserEmploymentHistories.FirstOrDefault(h => h.UserId == id && !h.IsDeleted);
            if (history == null)
            {
                return NotFound();
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

            return Ok();
        }

        [HttpDelete("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHaghighiUserEmploymentHistoryAdmin(int id)
        {
            var history = _context.HaghighiUserEmploymentHistories.FirstOrDefault(h => h.UserId == id && !h.IsDeleted);
            if (history == null)
            {
                return NotFound();
            }

            history.IsDeleted = true;
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("Admin/Clear")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearHaghighiUserEmploymentHistories()
        {
            _context.HaghighiUserEmploymentHistories.RemoveRange(_context.HaghighiUserEmploymentHistories);
            _context.SaveChanges();

            return Ok();
        }

    }
}
