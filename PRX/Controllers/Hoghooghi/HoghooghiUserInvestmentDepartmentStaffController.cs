﻿using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.Hoghooghi;
using PRX.Models.Hoghooghi;
using PRX.Utils;

namespace PRX.Controllers.Hoghooghi
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "HoghooghiUserInvestmentDepartmentStaff")]
    public class HoghooghiUserInvestmentDepartmentStaffController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public HoghooghiUserInvestmentDepartmentStaffController(PRXDbContext context)
        {
            _context = context;
        }

        // GET: api/HoghooghiUserInvestmentDepartmentStaff
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            try
            {
                var records = _context.HoghooghiUserInvestmentDepartmentStaff.ToList();
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // GET: api/HoghooghiUserInvestmentDepartmentStaff/5
        [HttpGet("{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int requestId)
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

                var record = _context.HoghooghiUserInvestmentDepartmentStaff.Where(e => e.RequestId == requestId && !e.IsDeleted).Select(r => new HoghooghiUserInvestmentDepartmentStaffDto 
                {
                    Id = r.Id,
                    RequestId = r.RequestId,
                    FullName = r.FullName,
                    Position = r.Position,
                    EducationalLevel = r.EducationalLevel,
                    FieldOfStudy = r.FieldOfStudy,
                    ExecutiveExperience = r.ExecutiveExperience,
                    FamiliarityWithCapitalMarket = r.FamiliarityWithCapitalMarket,
                    PersonalInvestmentExperienceInStockExchange = r.PersonalInvestmentExperienceInStockExchange,
                    IsComplete = r.IsComplete,
                    IsDeleted = r.IsDeleted

                }).ToList();
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiDepartmentNotFound});
                }
                return Ok(record);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }



        }

        // POST: api/HoghooghiUserInvestmentDepartmentStaff
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] HoghooghiUserInvestmentDepartmentStaffListDto dto)
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

                foreach (var staff in dto.Staff) 
                {
                    var record = new HoghooghiUserInvestmentDepartmentStaff
                    {
                        RequestId = dto.RequestId,
                        FullName = staff.FullName,
                        Position = staff.Position,
                        EducationalLevel = staff.EducationalLevel,
                        FieldOfStudy = staff.FieldOfStudy,
                        ExecutiveExperience = staff.ExecutiveExperience,
                        FamiliarityWithCapitalMarket = staff.FamiliarityWithCapitalMarket,
                        PersonalInvestmentExperienceInStockExchange = staff.PersonalInvestmentExperienceInStockExchange
                    };
                    _context.HoghooghiUserInvestmentDepartmentStaff.Add(record);
                }


               
                _context.SaveChanges();

                //return CreatedAtAction(nameof(GetById), new { requestId = record.Id }, record);
                return StatusCode(StatusCodes.Status201Created, new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // PUT: api/HoghooghiUserInvestmentDepartmentStaff/5
        [HttpPut("{id}/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(int id, int requestId, [FromBody] HoghooghiUserInvestmentDepartmentStaffDto dto)
        {

            try
            {
                if (id <=0 || requestId <= 0)
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

                var record = _context.HoghooghiUserInvestmentDepartmentStaff.FirstOrDefault(e => e.RequestId == requestId && e.Id == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiDepartmentNotFound });
                }

                record.RequestId = dto.RequestId;
                record.FullName = dto.FullName;
                record.Position = dto.Position;
                record.EducationalLevel = dto.EducationalLevel;
                record.FieldOfStudy = dto.FieldOfStudy;
                record.ExecutiveExperience = dto.ExecutiveExperience;
                record.FamiliarityWithCapitalMarket = dto.FamiliarityWithCapitalMarket;
                record.PersonalInvestmentExperienceInStockExchange = dto.PersonalInvestmentExperienceInStockExchange;

                _context.SaveChanges();

                return Ok(record);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }


        // PATCH: api/HoghooghiUserInvestmentDepartmentStaff/{id}/{requestId}
        [HttpPatch("{id}/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Patch(int id, int requestId, [FromBody] JsonPatchDocument<HoghooghiUserInvestmentDepartmentStaffDto> patchDoc)
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

                // Fetch the existing record
                var record = _context.HoghooghiUserInvestmentDepartmentStaff
                    .FirstOrDefault(e => e.RequestId == requestId && e.Id == id && !e.IsDeleted);

                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiDepartmentNotFound });
                }

                // Create a DTO to apply the patch
                var recordDto = new HoghooghiUserInvestmentDepartmentStaffDto
                {
                    RequestId = record.RequestId,
                    FullName = record.FullName,
                    Position = record.Position,
                    EducationalLevel = record.EducationalLevel,
                    FieldOfStudy = record.FieldOfStudy,
                    ExecutiveExperience = record.ExecutiveExperience,
                    FamiliarityWithCapitalMarket = record.FamiliarityWithCapitalMarket,
                    PersonalInvestmentExperienceInStockExchange = record.PersonalInvestmentExperienceInStockExchange
                };

                // Apply the patch document to the DTO
                patchDoc.ApplyTo(recordDto);

                // Update the record with modified values from the DTO
                record.RequestId = recordDto.RequestId;
                record.FullName = recordDto.FullName;
                record.Position = recordDto.Position;
                record.EducationalLevel = recordDto.EducationalLevel;
                record.FieldOfStudy = recordDto.FieldOfStudy;
                record.ExecutiveExperience = recordDto.ExecutiveExperience;
                record.FamiliarityWithCapitalMarket = recordDto.FamiliarityWithCapitalMarket;
                record.PersonalInvestmentExperienceInStockExchange = recordDto.PersonalInvestmentExperienceInStockExchange;

                // Save changes to the database
                _context.SaveChanges();

                return Ok(record);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }


        // DELETE: api/HoghooghiUserInvestmentDepartmentStaff/5
        [HttpDelete("{id}/{requestId}")]
        [Authorize(Roles = "User")]
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

                var record = _context.HoghooghiUserInvestmentDepartmentStaff.FirstOrDefault(e => e.RequestId == requestId && e.Id == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiDepartmentNotFound });
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

        // DELETE: api/HoghooghiUserInvestmentDepartmentStaff
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearAll()
        {
            try
            {
                _context.HoghooghiUserInvestmentDepartmentStaff.RemoveRange(_context.HoghooghiUserInvestmentDepartmentStaff);
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK});
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        [HttpPut("complete/{id}/{requestId}")]
        //[Authorize(Roles = "Admin")] // Assuming only admins can mark profiles as complete
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkCompaniesAsComplete(int id, int requestId)
        {
            try
            {
                if (id <= 0 || requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var record = _context.HoghooghiUserInvestmentDepartmentStaff.FirstOrDefault(e => e.RequestId == requestId && e.Id == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiDepartmentNotFound });
                }

                record.IsComplete = true;
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

                var record = _context.HoghooghiUserInvestmentDepartmentStaff.FirstOrDefault(e => e.RequestId == requestId && e.Id == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiDepartmentNotFound });
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
        public IActionResult GetAllHoghooghiUserInvestmentDepartmentStaff()
        {
            try
            {
                var users = _context.HoghooghiUserInvestmentDepartmentStaff.ToList();
                var userDtos = users.Select(user => new HoghooghiUserInvestmentDepartmentStaffDto
                {
                    Id = user.Id,
                    RequestId = user.RequestId,
                    FullName = user.FullName,
                    Position = user.Position,
                    EducationalLevel = user.EducationalLevel,
                    FieldOfStudy = user.FieldOfStudy,
                    ExecutiveExperience = user.ExecutiveExperience,
                    FamiliarityWithCapitalMarket = user.FamiliarityWithCapitalMarket,
                    PersonalInvestmentExperienceInStockExchange = user.PersonalInvestmentExperienceInStockExchange,
                    IsComplete = user.IsComplete,
                    IsDeleted = user.IsDeleted
                }).ToList();

                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        [HttpGet("Admin/{ID}/{requestId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHoghooghiUserInvestmentDepartmentStaffByIdAdmin(int id, int requestId)
        {
            try
            {
                if (id <= 0 || requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var user = _context.HoghooghiUserInvestmentDepartmentStaff.FirstOrDefault(u => u.RequestId == requestId && u.Id == id && !u.IsDeleted);
                if (user == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiDepartmentNotFound });
                }

                var userDto = new HoghooghiUserInvestmentDepartmentStaffDto
                {
                    Id = id,
                    RequestId = user.RequestId,
                    FullName = user.FullName,
                    Position = user.Position,
                    EducationalLevel = user.EducationalLevel,
                    FieldOfStudy = user.FieldOfStudy,
                    ExecutiveExperience = user.ExecutiveExperience,
                    FamiliarityWithCapitalMarket = user.FamiliarityWithCapitalMarket,
                    PersonalInvestmentExperienceInStockExchange = user.PersonalInvestmentExperienceInStockExchange,
                    IsComplete = user.IsComplete,
                    IsDeleted = user.IsDeleted
                };

                return Ok(userDto);
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
        public IActionResult UpdateHoghooghiUserInvestmentDepartmentStaffAdmin(int id, int requestId, [FromBody] HoghooghiUserInvestmentDepartmentStaffDto userDto)
        {
            try
            {
                if (id <= 0 || requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var user = _context.HoghooghiUserInvestmentDepartmentStaff.FirstOrDefault(u => u.RequestId == requestId && u.Id == id && !u.IsDeleted);
                if (user == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiDepartmentNotFound });
                }

                user.FullName = userDto.FullName;
                user.Position = userDto.Position;
                user.EducationalLevel = userDto.EducationalLevel;
                user.FieldOfStudy = userDto.FieldOfStudy;
                user.ExecutiveExperience = userDto.ExecutiveExperience;
                user.FamiliarityWithCapitalMarket = userDto.FamiliarityWithCapitalMarket;
                user.PersonalInvestmentExperienceInStockExchange = userDto.PersonalInvestmentExperienceInStockExchange;


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
        public IActionResult DeleteHoghooghiUserInvestmentDepartmentStaffAdmin(int id, int requestId)
        {
            try
            {
                if (id <= 0 || requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var user = _context.HoghooghiUserInvestmentDepartmentStaff.FirstOrDefault(u => u.RequestId == requestId && u.Id == id && !u.IsDeleted);
                if (user == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiDepartmentNotFound });
                }

                user.IsDeleted = true;
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
        public IActionResult ClearHoghooghiUserInvestmentDepartmentStaff()
        {
            try
            {
                _context.HoghooghiUserInvestmentDepartmentStaff.RemoveRange(_context.HoghooghiUserInvestmentDepartmentStaff);
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
