using Microsoft.AspNetCore.Authorization;
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
        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
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
                var record = _context.HoghooghiUserBoardOfDirectors.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
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
        public IActionResult Create([FromBody] HoghooghiUserBoardOfDirectorsDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var record = new HoghooghiUserBoardOfDirectors
                {
                    UserId = dto.UserId,
                    FullName = dto.FullName,
                    Position = dto.Position,
                    EducationalLevel = dto.EducationalLevel,
                    FieldOfStudy = dto.FieldOfStudy,
                    ExecutiveExperience = dto.ExecutiveExperience,
                    FamiliarityWithCapitalMarket = dto.FamiliarityWithCapitalMarket,
                    PersonalInvestmentExperienceInStockExchange = dto.PersonalInvestmentExperienceInStockExchange
                };

                _context.HoghooghiUserBoardOfDirectors.Add(record);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetById), new { id = record.Id }, record);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // PUT: api/HoghooghiUserBoardOfDirectors/5
        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(int id, [FromBody] HoghooghiUserBoardOfDirectorsDto dto)
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
                var record = _context.HoghooghiUserBoardOfDirectors.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiDrectorsNotFound });
                }

                record.UserId = dto.UserId;
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

        // DELETE: api/HoghooghiUserBoardOfDirectors/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
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
                var record = _context.HoghooghiUserBoardOfDirectors.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
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


        [HttpPut("complete/{id}")]
        //[Authorize(Roles = "Admin")] // Assuming only admins can mark profiles as complete
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkDirectorsAsComplete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var record = _context.HoghooghiUserBoardOfDirectors.FirstOrDefault(e => e.UserId == id);
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
                    UserId = director.UserId,
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

        [HttpGet("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHoghooghiUserBoardOfDirectorsByIdAdmin(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var director = _context.HoghooghiUserBoardOfDirectors.FirstOrDefault(d => d.UserId == id && !d.IsDeleted);
                if (director == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiDrectorsNotFound });
                }

                var directorDto = new HoghooghiUserBoardOfDirectorsDto
                {
                    UserId = director.UserId,
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

        [HttpPut("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHoghooghiUserBoardOfDirectorsAdmin(int id, [FromBody] HoghooghiUserBoardOfDirectorsDto directorDto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var director = _context.HoghooghiUserBoardOfDirectors.FirstOrDefault(d => d.UserId == id && !d.IsDeleted);
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

        [HttpDelete("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHoghooghiUserBoardOfDirectorsAdmin(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var director = _context.HoghooghiUserBoardOfDirectors.FirstOrDefault(d => d.UserId == id && !d.IsDeleted);
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
