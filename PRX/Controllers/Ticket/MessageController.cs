using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.Ticket;
using PRX.Models.Ticket;
using PRX.Utils;
using System;
using System.Linq;

namespace PRX.Controllers.Ticket
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Messages")]
    public class MessagesController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public MessagesController(PRXDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllMessages()
        {
            try
            {
                var messages = _context.Messages
                                      .Where(m => !m.IsDeleted)
                                      .Select(m => new MessageDto
                                      {
                                          Id = m.Id,
                                          TicketId = m.TicketId,
                                          SenderType = m.SenderType,
                                          SenderId = m.SenderId,
                                          Content = m.Content,
                                          SentAt = m.SentAt,
                                          IsDeleted = m.IsDeleted
                                      }).ToList();
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        [HttpGet("GetByUserId/{userId}")]
        [Authorize(Roles = "User, Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetMessagesByUserId(int userId)
        {
            try
            {
                var messages = _context.Messages
                                       .Where(m => m.SenderId == userId && m.SenderType == "User" && !m.IsDeleted)
                                       .Select(m => new MessageDto
                                       {
                                           Id = m.Id,
                                           TicketId = m.TicketId,
                                           SenderType = m.SenderType,
                                           SenderId = m.SenderId,
                                           Content = m.Content,
                                           SentAt = m.SentAt,
                                           IsDeleted = m.IsDeleted
                                       }).ToList();

                if (!messages.Any())
                {
                    return NotFound(new { message = ResponseMessages.MessageNotFound });
                }

                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        [HttpGet("GetByTicketId/{ticketId}")]
        [Authorize(Roles = "User, Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetMessagesByTicketId(int ticketId)
        {
            try
            {
                var messages = _context.Messages
                                       .Where(m => m.TicketId == ticketId && !m.IsDeleted)
                                       .Select(m => new MessageDto
                                       {
                                           Id = m.Id,
                                           TicketId = m.TicketId,
                                           SenderType = m.SenderType,
                                           SenderId = m.SenderId,
                                           Content = m.Content,
                                           SentAt = m.SentAt,
                                           IsDeleted = m.IsDeleted
                                       }).ToList();

                if (!messages.Any())
                {
                    return NotFound(new { message = ResponseMessages.MessageNotFound });
                }

                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        [HttpGet("GetByAdminId/{adminId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetMessagesByAdminId(int adminId)
        {
            try
            {
                var messages = _context.Messages
                                       .Where(m => m.SenderId == adminId && m.SenderType == "Admin" && !m.IsDeleted)
                                       .Select(m => new MessageDto
                                       {
                                           Id = m.Id,
                                           TicketId = m.TicketId,
                                           SenderType = m.SenderType,
                                           SenderId = m.SenderId,
                                           Content = m.Content,
                                           SentAt = m.SentAt,
                                           IsDeleted = m.IsDeleted
                                       }).ToList();

                if (!messages.Any())
                {
                    return NotFound(new { message = ResponseMessages.MessageNotFound });
                }

                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }


        [HttpPost("User")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult PostUserMessage([FromBody] MessageDto messageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userIdClaim = User.FindFirst("id");

                if (userIdClaim == null)
                {
                    return BadRequest(new { message = ResponseMessages.TokenIsInvalid });
                }

                var senderId = int.Parse(userIdClaim.Value);

                var message = new Message
                {
                    TicketId = messageDto.TicketId,
                    SenderType = "User",
                    SenderId = senderId,
                    Content = messageDto.Content,
                    SentAt = DateTime.UtcNow,
                    IsDeleted = false,
                    UserId = senderId,
                    AdminId = messageDto?.AdminId
                };

                _context.Messages.Add(message);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetMessagesByTicketId), new { ticketId = message.TicketId }, message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        [HttpPost("Admin")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult PostAdminMessage([FromBody] MessageDto messageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var adminIdClaim = User.FindFirst("id");

                if (adminIdClaim == null)
                {
                    return BadRequest(new { message = ResponseMessages.TokenIsInvalid });
                }

                var senderId = int.Parse(adminIdClaim.Value);

                var message = new Message
                {
                    TicketId = messageDto.TicketId,
                    SenderType = "Admin",
                    SenderId = senderId,
                    Content = messageDto.Content,
                    SentAt = DateTime.UtcNow,
                    IsDeleted = false,
                    UserId = messageDto.UserId,
                    AdminId = senderId
                };

                _context.Messages.Add(message);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetMessagesByTicketId), new { ticketId = message.TicketId }, message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "User, Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult PutMessage(int id, [FromBody] MessageDto messageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var message = _context.Messages.FirstOrDefault(m => m.Id == id && !m.IsDeleted);

                if (message == null)
                {
                    return NotFound(new { message = ResponseMessages.MessageNotFound });
                }

                var senderType = User.FindFirst("role")?.Value;
                var senderId = int.Parse(User.FindFirst("id")?.Value);

                if ((message.SenderType == "User" && senderType == "User" && message.SenderId == senderId) ||
                    (message.SenderType == "Admin" && senderType == "Admin" && message.SenderId == senderId))
                {
                    message.Content = messageDto.Content;
                    _context.SaveChanges();

                    return Ok(message);
                }

                return Unauthorized(new { message = ResponseMessages.Unauthorized });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        // PATCH: api/messages/{id}
        [HttpPatch("{id}")]
        [Authorize(Roles = "User, Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult PatchMessage(int id, [FromBody] JsonPatchDocument<MessageDto> patchDoc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var message = _context.Messages.FirstOrDefault(m => m.Id == id && !m.IsDeleted);

                if (message == null)
                {
                    return NotFound(new { message = ResponseMessages.MessageNotFound });
                }

                var senderType = User.FindFirst("role")?.Value;
                var senderId = int.Parse(User.FindFirst("id")?.Value);

                if ((message.SenderType == "User" && senderType == "User" && message.SenderId == senderId) ||
                    (message.SenderType == "Admin" && senderType == "Admin" && message.SenderId == senderId))
                {
                    // Create a DTO to hold the current values
                    var messageDto = new MessageDto
                    {
                        Content = message.Content
                    };

                    // Apply the patch document to the DTO
                    patchDoc.ApplyTo(messageDto, ModelState);

                    // Validate the model after applying the patch
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    // Update the message properties
                    message.Content = messageDto.Content;

                    _context.SaveChanges();

                    return Ok(message);
                }

                return Unauthorized(new { message = ResponseMessages.Unauthorized });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "User, Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteMessage(int id)
        {
            try
            {
                var message = _context.Messages.FirstOrDefault(m => m.Id == id && !m.IsDeleted);

                if (message == null)
                {
                    return NotFound(new { message = ResponseMessages.MessageNotFound });
                }

                var senderType = User.FindFirst("role")?.Value;
                var senderId = int.Parse(User.FindFirst("id")?.Value);

                if ((message.SenderType == "User" && senderType == "User" && message.SenderId == senderId) ||
                    (message.SenderType == "Admin" && senderType == "Admin" && message.SenderId == senderId))
                {
                    message.IsDeleted = true;
                    _context.SaveChanges();

                    return Ok(new { message = ResponseMessages.OK });
                }

                return Unauthorized(new { message = ResponseMessages.Unauthorized });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }
    }
}
