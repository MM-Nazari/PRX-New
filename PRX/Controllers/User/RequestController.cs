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
using PRX.Utils;
using System.Security.Cryptography;

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
        [HttpGet("{requestId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetRequestById(int requestId)
        {
            var request = await _context.Requests.FindAsync(requestId);
            if (request == null || request.IsDeleted)
            {
                return NotFound(new { message = ResponseMessages.RequestNotFound });
            }

            return Ok(request);
        }

        // GET: api/request/user/5
        [HttpGet("user/{userId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetRequestsByUserId(int userId)
        {
            var requests = await _context.Requests
                .Where(r => r.UserId == userId && !r.IsDeleted)
                .ToListAsync();

            if (requests == null || requests.Count == 0)
            {
                return NotFound(new { message = ResponseMessages.UserRequestNotFound });
            }

            return Ok(requests);
        }

        // GET: api/request/tracking/{trackingCode}
        [HttpGet("tracking/{trackingCode}")]
        [Authorize(Roles = "User,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRequestByTrackingCode(string trackingCode)
        {
            try
            {
                if (string.IsNullOrEmpty(trackingCode))
                {
                    return BadRequest(new { message = ResponseMessages.InvalidTrackingCode });
                }

                var request = await _context.Requests
                                            .Include(r => r.User)
                                            .FirstOrDefaultAsync(r => r.TrackingCode == trackingCode);

                if (request == null)
                {
                    return NotFound(new { message = ResponseMessages.RequestNotFound });
                }

                return Ok(request);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
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
                TrackingCode = "",
                RequestSentTime = DateTime.Now,
                BeneficiaryName = "",
                RequestState = "ناقص"
                
            };

            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRequestById), new { requestId = request.Id }, request);
        }

        // PUT: api/request/5
        [HttpPut("First/{requestId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> UpdateRequestFirst(int requestId, [FromBody] RequestDto requestDto)
        {

            var request = await _context.Requests.FindAsync(requestId);
            if (request == null || request.IsDeleted)
            {
                return NotFound(new { message = ResponseMessages.RequestNotFound });
            }

            request.BeneficiaryName = requestDto.BeneficiaryName;
        

            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // PUT: api/request/5
        [HttpPut("Finish/{requestId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> UpdateRequestFinish(int requestId, [FromBody] RequestDto requestDto)
        {

            var request = await _context.Requests.FindAsync(requestId);
            if (request == null || request.IsDeleted)
            {
                return NotFound(new { message = ResponseMessages.RequestNotFound });
            }

            request.RequestState = "ثبت شده";
            request.TrackingCode = GenerateTrackingCode();



            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        public static string GenerateTrackingCode()
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            var randomString = GenerateRandomString(6);

            return $"{timestamp}-{randomString}";
        }

        private static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            using (var crypto = new RNGCryptoServiceProvider())
            {
                var data = new byte[length];
                var buffer = new byte[128]; // larger buffer size to reduce entropy exhaustion
                int maxRandom = byte.MaxValue - ((byte.MaxValue + 1) % chars.Length);

                int count = 0;
                while (count < length)
                {
                    crypto.GetBytes(buffer);
                    for (int i = 0; i < buffer.Length && count < length; i++)
                    {
                        if (buffer[i] > maxRandom) continue;
                        data[count++] = (byte)(buffer[i] % chars.Length);
                    }
                }

                var result = new char[length];
                for (int i = 0; i < data.Length; i++)
                {
                    result[i] = chars[data[i]];
                }

                return new string(result);
            }
        }

        // PUT: api/request/5
        [HttpPut("{requestId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> UpdateRequest(int requestId, [FromBody] RequestDto requestDto)
        {

            var request = await _context.Requests.FindAsync(requestId);
            if (request == null || request.IsDeleted)
            {
                return NotFound(new { message = "Request not found" });
            }

            request.RequestType = requestDto.RequestType;
            //request.IsDeleted = requestDto.IsDeleted;
           // request.IsComplete = requestDto.IsComplete;

            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/request/5
        [HttpDelete("{requestId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> DeleteRequest(int requestId)
        {
            var request = await _context.Requests.FindAsync(requestId);
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