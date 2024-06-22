using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRX.Data;
using PRX.Dto.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRX.Data;
using PRX.Dto.User;
using PRX.Models.User;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PRX.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Requests")]
    public class RequestController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public RequestController(PRXDbContext context)
        {
            _context = context;
        }

        // GET: api/request
        [HttpGet]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetAllRequests()
        {
            var requests = await _context.Requests.Where(r => !r.IsDeleted).ToListAsync();
            return Ok(requests);
        }

        // GET: api/request/5
        [HttpGet("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetRequestById(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null || request.IsDeleted)
            {
                return NotFound(new { message = "Request not found" });
            }

            return Ok(request);
        }

        // POST: api/request
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> CreateRequest([FromBody] RequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var request = new Request
            {
                UserId = requestDto.UserId,
                RequestType = requestDto.RequestType,
                IsComplete = requestDto.IsComplete,
                IsDeleted = requestDto.IsDeleted
            };

            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRequestById), new { id = request.Id }, request);
        }

        // PUT: api/request/5
        [HttpPut("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> UpdateRequest(int id, [FromBody] RequestDto requestDto)
        {
            if (id != requestDto.UserId)
            {
                return BadRequest(new { message = "Request ID mismatch" });
            }

            var request = await _context.Requests.FindAsync(id);
            if (request == null || request.IsDeleted)
            {
                return NotFound(new { message = "Request not found" });
            }

            request.RequestType = requestDto.RequestType;
            request.IsComplete = requestDto.IsComplete;

            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/request/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null || request.IsDeleted)
            {
                return NotFound(new { message = "Request not found" });
            }

            request.IsDeleted = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}