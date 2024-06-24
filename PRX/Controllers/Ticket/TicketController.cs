using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.Ticket;
using PRX.Models.Ticket;
using PRX.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace PRX.Controllers.Ticket
{

        [Route("api/[controller]")]
        [ApiController]
        [ApiExplorerSettings(GroupName = "Tickets")]
        public class TicketsController : ControllerBase
        {
            private readonly PRXDbContext _context;

            public TicketsController(PRXDbContext context)
            {
                _context = context;
            }

            [HttpGet]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            public IActionResult GetAllTickets()
            {
                try
                {
                    var tickets = _context.Tickets
                                          .Where(t => !t.IsDeleted)
                                          .Select(t => new TicketDto
                                          {
                                              UserId = t.UserId,
                                              TrackingCode = t.TrackingCode,
                                              Subject = t.Subject,
                                              Description = t.Description,
                                              Status = t.Status,
                                              CreatedAt = t.CreatedAt,
                                              Category = t.Category,
                                              IsDeleted = t.IsDeleted
                                          }).ToList();
                    return Ok(tickets);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
                }
            }

            [HttpGet("{id}")]
            [Authorize(Roles = "User")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            public IActionResult GetTicketById(int id)
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

                    var ticket = _context.Tickets
                                         .Where(t => t.UserId == id && !t.IsDeleted)
                                         .Select(t => new TicketDto
                                         {
                                             UserId = t.UserId,
                                             TrackingCode = t.TrackingCode,
                                             Subject = t.Subject,
                                             Description = t.Description,
                                             Status = t.Status,
                                             CreatedAt = t.CreatedAt,
                                             Category = t.Category,
                                             IsDeleted = t.IsDeleted
                                         }).FirstOrDefault();

                    if (ticket == null)
                    {
                        return NotFound(new { message = ResponseMessages.TicketNotFound });
                    }

                    return Ok(ticket);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
                }
            }

            [HttpGet("GetByTrackingCode/{trackingCode}")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public IActionResult GetTicketByTrackingCode(string trackingCode)
            {
                var ticket = _context.Tickets.FirstOrDefault(t => t.TrackingCode == trackingCode);

                if (ticket == null)
                {
                    return NotFound(new { message = ResponseMessages.TicketNotFound });
                }

                return Ok(ticket);
            }

            [HttpPost]
            [Authorize(Roles = "User")]
            [ProducesResponseType(StatusCodes.Status201Created)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            public IActionResult CreateTicket([FromBody] TicketDto ticketDto)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                try
                {

                    var ticket = new PRX.Models.Ticket.Ticket
                    {
                        UserId = ticketDto.UserId,
                        TrackingCode = GenerateTrackingCode(),
                        Subject = ticketDto.Subject,
                        Description = ticketDto.Description,
                        Status = ticketDto.Status,
                        CreatedAt = DateTime.UtcNow,
                        Category = ticketDto.Category,
                        IsDeleted = false
                    };

                    _context.Tickets.Add(ticket);
                    _context.SaveChanges();

                    return CreatedAtAction(nameof(GetTicketById), new { id = ticket.Id }, ticket);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
                }
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

            [HttpPut("{id}")]
            [Authorize(Roles = "User")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            public IActionResult UpdateTicket(int id, [FromBody] TicketDto ticketDto)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

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

                    var ticket = _context.Tickets.FirstOrDefault(t => t.UserId == id && !t.IsDeleted);

                    if (ticket == null)
                    {
                        return NotFound(new { message = ResponseMessages.TicketNotFound });
                    }

                    ticket.TrackingCode = ticketDto.TrackingCode;
                    ticket.Subject = ticketDto.Subject;
                    ticket.Description = ticketDto.Description;
                    ticket.Status = ticketDto.Status;
                    ticket.Category = ticketDto.Category;

                    _context.SaveChanges();

                    return Ok(ticket);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
                }
            }

            [HttpDelete("{id}")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            public IActionResult SoftDeleteTicket(int id)
            {
                try
                {

                    if (id <= 0)
                    {
                        return BadRequest(new { message = ResponseMessages.InvalidId });
                    }
                    var ticket = _context.Tickets.FirstOrDefault(t => t.UserId == id && !t.IsDeleted);

                    if (ticket == null)
                    {
                        return NotFound(new { message = ResponseMessages.TicketNotFound });
                    }

                    ticket.IsDeleted = true;
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
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            public IActionResult GetAllTicketsAdmin()
            {
                try
                {
                    var tickets = _context.Tickets
                                          .Select(t => new TicketDto
                                          {
                                              UserId = t.UserId,
                                              TrackingCode = t.TrackingCode,
                                              Subject = t.Subject,
                                              Description = t.Description,
                                              Status = t.Status,
                                              CreatedAt = t.CreatedAt,
                                              Category = t.Category,
                                              IsDeleted = t.IsDeleted
                                          }).ToList();
                    return Ok(tickets);
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
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            public IActionResult GetTicketByIdAdmin(int id)
            {
                try
                {
                    if (id <= 0)
                    {
                        return BadRequest(new { message = ResponseMessages.InvalidId });
                    }

                    var ticket = _context.Tickets
                                         .Where(t => t.UserId == id)
                                         .Select(t => new TicketDto
                                         {
                                             UserId = t.UserId,
                                             TrackingCode = t.TrackingCode,
                                             Subject = t.Subject,
                                             Description = t.Description,
                                             Status = t.Status,
                                             CreatedAt = t.CreatedAt,
                                             Category = t.Category,
                                             IsDeleted = t.IsDeleted
                                         }).FirstOrDefault();

                    if (ticket == null)
                    {
                        return NotFound(new { message = ResponseMessages.TicketNotFound });
                    }

                    return Ok(ticket);
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
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            public IActionResult UpdateTicketAdmin(int id, [FromBody] TicketDto ticketDto)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                try
                {

                    var ticket = _context.Tickets.FirstOrDefault(t => t.UserId == id);

                    if (ticket == null)
                    {
                        return NotFound(new { message = ResponseMessages.TicketNotFound });
                    }

                    ticket.TrackingCode = ticketDto.TrackingCode;
                    ticket.Subject = ticketDto.Subject;
                    ticket.Description = ticketDto.Description;
                    ticket.Status = ticketDto.Status;
                    ticket.Category = ticketDto.Category;

                    _context.SaveChanges();

                    return Ok(ticket);
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
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            public IActionResult DeleteTicketAdmin(int id)
            {
                try
                {
                    if (id <= 0)
                    {
                        return BadRequest(new { message = ResponseMessages.InvalidId });
                    }
                    var ticket = _context.Tickets.FirstOrDefault(t => t.UserId == id);

                    if (ticket == null)
                    {
                        return NotFound(new { message = ResponseMessages.TicketNotFound });
                    }

                    ticket.IsDeleted = true;
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
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            public IActionResult ClearTickets()
            {
                try
                {
                    _context.Tickets.RemoveRange(_context.Tickets);
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
