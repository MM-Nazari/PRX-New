using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using PRX.Data;
using PRX.Dto.User;
using PRX.Models.User;
using PRX.Utils;
using DocumentFormat.OpenXml.Office2010.Excel;
using Azure.Core;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.JsonPatch;

namespace PRX.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "UserInvestmentExperiences")]
    public class UserInvestmentExperienceController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public UserInvestmentExperienceController(PRXDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllUserInvestmentExperiences()
        {
            try
            {
                var userInvestmentExperiences = _context.UserInvestmentExperiences.ToList();
                return Ok(userInvestmentExperiences);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpGet("{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserInvestmentExperienceById(int requestId)
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

                var userInvestmentExperience = _context.UserInvestmentExperiences.Where(u => u.RequestId == requestId && !u.IsDeleted).Select(r => new UserInvestmentExperienceDto 
                {
                    Id = r.Id,
                    RequestId = r.RequestId,
                    InvestmentType = r.InvestmentType,
                    InvestmentAmount = r.InvestmentAmount,
                    InvestmentDurationMonths = r.InvestmentDurationMonths,
                    ProfitLossAmount = r.ProfitLossAmount,
                    ProfitLossDescription = r.ProfitLossDescription,
                    ConversionReason = r.ConversionReason,
                    IsComplete = r.IsComplete,
                    IsDeleted = r.IsDeleted

                }).ToList();

                if (userInvestmentExperience == null)
                {
                    return NotFound(new { message = ResponseMessages.UserInvestmentExperienceNotFound });
                }
                return Ok(userInvestmentExperience);

            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateUserInvestmentExperience([FromBody] UserInvestmentExperienceListDto userInvestmentExperienceDto)
        {
            try
            {
                if (userInvestmentExperienceDto.RequestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                foreach (var exp in userInvestmentExperienceDto.Experience) 
                {
                    var record = new UserInvestmentExperience
                    {
                        RequestId = userInvestmentExperienceDto.RequestId,
                        InvestmentType = exp.InvestmentType,
                        InvestmentAmount = exp.InvestmentAmount,
                        InvestmentDurationMonths = exp.InvestmentDurationMonths,
                        ProfitLossAmount = exp.ProfitLossAmount,
                        ProfitLossDescription = exp.ProfitLossDescription,
                        ConversionReason = exp.ConversionReason
                    };
                    _context.UserInvestmentExperiences.Add(record);
                }
  
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status201Created, new { message = ResponseMessages.OK });
                //return CreatedAtAction(nameof(GetUserInvestmentExperienceById), new { requestId = userInvestmentExperience.Id }, userInvestmentExperience);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpPut("{id}/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserInvestmentExperience(int id, int requestId, [FromBody] UserInvestmentExperienceDto userInvestmentExperienceDto)
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

                var userInvestmentExperience = _context.UserInvestmentExperiences.FirstOrDefault(u => u.RequestId == requestId && u.Id == id  && !u.IsDeleted);
                if (userInvestmentExperience == null)
                {
                    return NotFound(new { message = ResponseMessages.UserInvestmentExperienceNotFound });
                }

                
                userInvestmentExperience.InvestmentType = userInvestmentExperienceDto.InvestmentType;
                userInvestmentExperience.InvestmentAmount = userInvestmentExperienceDto.InvestmentAmount;
                userInvestmentExperience.InvestmentDurationMonths = userInvestmentExperienceDto.InvestmentDurationMonths;
                userInvestmentExperience.ProfitLossAmount = userInvestmentExperienceDto.ProfitLossAmount;
                userInvestmentExperience.ProfitLossDescription = userInvestmentExperienceDto.ProfitLossDescription;
                userInvestmentExperience.ConversionReason = userInvestmentExperienceDto.ConversionReason;

                _context.SaveChanges();

                return Ok(userInvestmentExperience);

            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }



        }

        // PATCH: api/UserInvestmentExperience/{id}/{requestId}
        [HttpPatch("{id}/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult PatchUserInvestmentExperience(int id, int requestId, [FromBody] JsonPatchDocument<UserInvestmentExperienceDto> patchDoc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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

                var userInvestmentExperience = _context.UserInvestmentExperiences
                    .FirstOrDefault(u => u.RequestId == requestId && u.Id == id && !u.IsDeleted);
                if (userInvestmentExperience == null)
                {
                    return NotFound(new { message = ResponseMessages.UserInvestmentExperienceNotFound });
                }

                // Create a DTO to hold the current user investment experience information
                var userInvestmentExperienceDto = new UserInvestmentExperienceDto
                {
                    InvestmentType = userInvestmentExperience.InvestmentType,
                    InvestmentAmount = userInvestmentExperience.InvestmentAmount,
                    InvestmentDurationMonths = userInvestmentExperience.InvestmentDurationMonths,
                    ProfitLossAmount = userInvestmentExperience.ProfitLossAmount,
                    ProfitLossDescription = userInvestmentExperience.ProfitLossDescription,
                    ConversionReason = userInvestmentExperience.ConversionReason
                };

                // Apply the patch document to the DTO
                patchDoc.ApplyTo(userInvestmentExperienceDto, ModelState);

                // Validate the model after applying the patch
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Update the user investment experience properties based on the modified DTO
                userInvestmentExperience.InvestmentType = userInvestmentExperienceDto.InvestmentType; // Update if present
                userInvestmentExperience.InvestmentAmount = userInvestmentExperienceDto.InvestmentAmount; // Update if present
                userInvestmentExperience.InvestmentDurationMonths = userInvestmentExperienceDto.InvestmentDurationMonths; // Update if present
                userInvestmentExperience.ProfitLossAmount = userInvestmentExperienceDto.ProfitLossAmount; // Update if present
                userInvestmentExperience.ProfitLossDescription = userInvestmentExperienceDto.ProfitLossDescription; // Update if present
                userInvestmentExperience.ConversionReason = userInvestmentExperienceDto.ConversionReason; // Update if present

                // Save changes to the database
                _context.SaveChanges();

                // Return 204 No Content
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }


        [HttpDelete("{id}/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserInvestmentExperience(int id, int requestId)
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

                var userInvestmentExperience = _context.UserInvestmentExperiences.FirstOrDefault(u => u.RequestId == requestId && u.Id == id && !u.IsDeleted);
                if (userInvestmentExperience == null)
                {
                    return NotFound(new { message = ResponseMessages.UserInvestmentExperienceNotFound });
                }

                userInvestmentExperience.IsDeleted = true;
                _context.SaveChanges();

                return NoContent();

            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult ClearUserInvestmentExperiences()
        {
            try
            {
                _context.UserInvestmentExperiences.RemoveRange(_context.UserInvestmentExperiences);
                _context.SaveChanges();

                return NoContent();
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
        public IActionResult MarkFinancwChangeAsComplete(int id, int requestId)
        {
            try
            {
                if (id <= 0 || requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userInvestmentExperiences = _context.UserInvestmentExperiences.FirstOrDefault(u => u.RequestId == requestId && u.Id == id && !u.IsDeleted);
                if (userInvestmentExperiences == null)
                {
                    return NotFound(new { message = ResponseMessages.UserInvestmentExperienceNotFound });
                }

                userInvestmentExperiences.IsComplete = true;
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


                var record = _context.UserInvestmentExperiences.FirstOrDefault(e => e.RequestId == requestId && e.Id == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.UserInvestmentExperienceNotFound });
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
        public IActionResult GetAllUserInvestmentExperiencesAdmin()
        {
            try
            {
                var experiences = _context.UserInvestmentExperiences.ToList();
                var experienceDtos = experiences.Select(experience => new UserInvestmentExperienceDto
                {
                    Id = experience.Id,
                    RequestId = experience.RequestId,
                    InvestmentType = experience.InvestmentType,
                    InvestmentAmount = experience.InvestmentAmount,
                    InvestmentDurationMonths = experience.InvestmentDurationMonths,
                    ProfitLossAmount = experience.ProfitLossAmount,
                    ProfitLossDescription = experience.ProfitLossDescription,
                    ConversionReason = experience.ConversionReason,
                    IsComplete = experience.IsComplete,
                    IsDeleted = experience.IsDeleted
                }).ToList();
                return Ok(experienceDtos);
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
        public IActionResult GetUserInvestmentExperienceByIdAdmin(int id)
        {
            try
            {

                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var experience = _context.UserInvestmentExperiences.FirstOrDefault(exp => exp.Id == id && !exp.IsDeleted);
                if (experience == null)
                {
                    return NotFound(new { message = ResponseMessages.UserInvestmentExperienceNotFound });
                }

                var experienceDto = new UserInvestmentExperienceDto
                {
                    RequestId = experience.RequestId,
                    InvestmentType = experience.InvestmentType,
                    InvestmentAmount = experience.InvestmentAmount,
                    InvestmentDurationMonths = experience.InvestmentDurationMonths,
                    ProfitLossAmount = experience.ProfitLossAmount,
                    ProfitLossDescription = experience.ProfitLossDescription,
                    ConversionReason = experience.ConversionReason,
                    IsComplete = experience.IsComplete,
                    IsDeleted = experience.IsDeleted
                };

                return Ok(experienceDto);
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
        public IActionResult UpdateUserInvestmentExperienceAdmin(int id, [FromBody] UserInvestmentExperienceDto experienceDto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var experience = _context.UserInvestmentExperiences.FirstOrDefault(exp => exp.Id == id && !exp.IsDeleted);
                if (experience == null)
                {
                    return NotFound(new { message = ResponseMessages.UserInvestmentExperienceNotFound });
                }

                experience.InvestmentType = experienceDto.InvestmentType;
                experience.InvestmentAmount = experienceDto.InvestmentAmount;
                experience.InvestmentDurationMonths = experienceDto.InvestmentDurationMonths;
                experience.ProfitLossAmount = experienceDto.ProfitLossAmount;
                experience.ProfitLossDescription = experienceDto.ProfitLossDescription;
                experience.ConversionReason = experienceDto.ConversionReason;

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
        public IActionResult DeleteUserInvestmentExperienceAdmin(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var experience = _context.UserInvestmentExperiences.FirstOrDefault(exp => exp.Id == id && !exp.IsDeleted);
                if (experience == null)
                {
                    return NotFound(new { message = ResponseMessages.UserInvestmentExperienceNotFound });
                }

                experience.IsDeleted = true;
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
        public IActionResult ClearUserInvestmentExperiencesAdmin()
        {
            try
            {
                _context.UserInvestmentExperiences.RemoveRange(_context.UserInvestmentExperiences);
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
