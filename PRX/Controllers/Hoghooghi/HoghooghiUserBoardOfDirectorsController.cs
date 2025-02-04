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
    [ApiExplorerSettings(GroupName = "HoghooghiUserBoardOfDirectors")]
    public class HoghooghiUserBoardOfDirectorsController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public HoghooghiUserBoardOfDirectorsController(PRXDbContext context)
        {
            _context = context;
        }

        // GET: api/HoghooghiUserBoardOfDirectors
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            try
            {
                var records = _context.HoghooghiUserBoardOfDirectors.ToList();
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // GET: api/HoghooghiUserBoardOfDirectors/5
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
                var record = _context.HoghooghiUserBoardOfDirectors.Where(e => e.RequestId == requestId && !e.IsDeleted).Select(r => new HoghooghiUserBoardOfDirectorsDto 
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
                    return NotFound(new { message = ResponseMessages.HoghooghiDrectorsNotFound });
                }
                return Ok(record);

            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }



        }

        // POST: api/HoghooghiUserBoardOfDirectors
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] HoghooghiUserBoardOfDirectorsListDto dto)
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

                foreach (var board in dto.BoardOfDirectors)
                {
                    var record = new HoghooghiUserBoardOfDirectors
                    {
                        RequestId = dto.RequestId,
                        FullName = board.FullName,
                        Position = board.Position,
                        EducationalLevel = board.EducationalLevel,
                        FieldOfStudy = board.FieldOfStudy,
                        ExecutiveExperience = board.ExecutiveExperience,
                        FamiliarityWithCapitalMarket = board.FamiliarityWithCapitalMarket,
                        PersonalInvestmentExperienceInStockExchange = board.PersonalInvestmentExperienceInStockExchange
                    };
                    _context.HoghooghiUserBoardOfDirectors.Add(record);
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

        // PUT: api/HoghooghiUserBoardOfDirectors/5
        [HttpPut("{id}/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(int id, int requestId, [FromBody] HoghooghiUserBoardOfDirectorsDto dto)
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
                var record = _context.HoghooghiUserBoardOfDirectors.FirstOrDefault(e => e.RequestId == requestId && e.Id == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiDrectorsNotFound });
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


        // PATCH: api/HoghooghiUserBoardOfDirectors/{id}/{requestId}
        [HttpPatch("{id}/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Patch(int id, int requestId, [FromBody] JsonPatchDocument<HoghooghiUserBoardOfDirectorsDto> patchDoc)
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

                var record = _context.HoghooghiUserBoardOfDirectors.FirstOrDefault(e => e.RequestId == requestId && e.Id == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiDrectorsNotFound });
                }

                // Create a DTO from the existing record to apply patches
                var recordDto = new HoghooghiUserBoardOfDirectorsDto
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

                // Update the record with the modified values from the DTO
                record.RequestId = recordDto.RequestId;
                record.FullName = recordDto.FullName;
                record.Position = recordDto.Position;
                record.EducationalLevel = recordDto.EducationalLevel;
                record.FieldOfStudy = recordDto.FieldOfStudy;
                record.ExecutiveExperience = recordDto.ExecutiveExperience;
                record.FamiliarityWithCapitalMarket = recordDto.FamiliarityWithCapitalMarket;
                record.PersonalInvestmentExperienceInStockExchange = recordDto.PersonalInvestmentExperienceInStockExchange;

                _context.SaveChanges();

                return Ok(record);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }


        // DELETE: api/HoghooghiUserBoardOfDirectors/5
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
                var record = _context.HoghooghiUserBoardOfDirectors.FirstOrDefault(e => e.RequestId == requestId && e.Id == id &&!e.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiDrectorsNotFound });
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

        // DELETE: api/HoghooghiUserBoardOfDirectors
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearAll()
        {
            try
            {
                _context.HoghooghiUserBoardOfDirectors.RemoveRange(_context.HoghooghiUserBoardOfDirectors);
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
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
        public IActionResult MarkDirectorsAsComplete(int id, int requestId)
        {
            try
            {
                if (id <= 0 || requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var record = _context.HoghooghiUserBoardOfDirectors.FirstOrDefault(e => e.RequestId == requestId && e.Id == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiDrectorsNotFound });
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


                var record = _context.HoghooghiUserBoardOfDirectors.FirstOrDefault(e => e.RequestId == requestId && e.Id == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiDrectorsNotFound });
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
        public IActionResult GetAllHoghooghiUserBoardOfDirectors()
        {
            try
            {
                var directors = _context.HoghooghiUserBoardOfDirectors.ToList();
                var directorDtos = directors.Select(director => new HoghooghiUserBoardOfDirectorsDto
                {
                    Id = director.Id,
                    RequestId = director.RequestId,
                    FullName = director.FullName,
                    Position = director.Position,
                    EducationalLevel = director.EducationalLevel,
                    FieldOfStudy = director.FieldOfStudy,
                    ExecutiveExperience = director.ExecutiveExperience,
                    FamiliarityWithCapitalMarket = director.FamiliarityWithCapitalMarket,
                    PersonalInvestmentExperienceInStockExchange = director.PersonalInvestmentExperienceInStockExchange,
                    IsComplete = director.IsComplete,
                    IsDeleted = director.IsDeleted
                }).ToList();

                return Ok(directorDtos);
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
        public IActionResult GetHoghooghiUserBoardOfDirectorsByIdAdmin(int id, int requestId)
        {
            try
            {
                if (id <= 0 || requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var director = _context.HoghooghiUserBoardOfDirectors.FirstOrDefault(d => d.RequestId == requestId && d.Id == id && !d.IsDeleted);
                if (director == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiDrectorsNotFound });
                }

                var directorDto = new HoghooghiUserBoardOfDirectorsDto
                {
                    RequestId = director.RequestId,
                    FullName = director.FullName,
                    Position = director.Position,
                    EducationalLevel = director.EducationalLevel,
                    FieldOfStudy = director.FieldOfStudy,
                    ExecutiveExperience = director.ExecutiveExperience,
                    FamiliarityWithCapitalMarket = director.FamiliarityWithCapitalMarket,
                    PersonalInvestmentExperienceInStockExchange = director.PersonalInvestmentExperienceInStockExchange,
                    IsComplete = director.IsComplete,
                    IsDeleted = director.IsDeleted
                };

                return Ok(directorDto);
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
        public IActionResult UpdateHoghooghiUserBoardOfDirectorsAdmin(int id, int requestId, [FromBody] HoghooghiUserBoardOfDirectorsDto directorDto)
        {
            try
            {
                if (id <= 0 || requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var director = _context.HoghooghiUserBoardOfDirectors.FirstOrDefault(d => d.RequestId == requestId && d.Id == id && !d.IsDeleted);
                if (director == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiDrectorsNotFound });
                }

                director.FullName = directorDto.FullName;
                director.Position = directorDto.Position;
                director.EducationalLevel = directorDto.EducationalLevel;
                director.FieldOfStudy = directorDto.FieldOfStudy;
                director.ExecutiveExperience = directorDto.ExecutiveExperience;
                director.FamiliarityWithCapitalMarket = directorDto.FamiliarityWithCapitalMarket;
                director.PersonalInvestmentExperienceInStockExchange = directorDto.PersonalInvestmentExperienceInStockExchange;


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
        public IActionResult DeleteHoghooghiUserBoardOfDirectorsAdmin(int id, int requestId)
        {
            try
            {
                if (id <= 0 || requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var director = _context.HoghooghiUserBoardOfDirectors.FirstOrDefault(d => d.RequestId == requestId && d.Id == id && !d.IsDeleted);
                if (director == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiDrectorsNotFound});
                }

                director.IsDeleted = true;
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
        public IActionResult ClearHoghooghiUserBoardOfDirectors()
        {
            try
            {
                _context.HoghooghiUserBoardOfDirectors.RemoveRange(_context.HoghooghiUserBoardOfDirectors);
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
