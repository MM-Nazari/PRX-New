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
    [ApiExplorerSettings(GroupName = "HoghooghiUserCompaniesWithMajorInvestors")]
    public class HoghooghiUserCompaniesWithMajorInvestorsController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public HoghooghiUserCompaniesWithMajorInvestorsController(PRXDbContext context)
        {
            _context = context;
        }

        // GET: api/HoghooghiUserCompaniesWithMajorInvestors
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            try
            {
                var records = _context.HoghooghiUserCompaniesWithMajorInvestors.ToList();
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // GET: api/HoghooghiUserCompaniesWithMajorInvestors/5
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
                var record = _context.HoghooghiUserCompaniesWithMajorInvestors.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound();
                }
                return Ok(record);

            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        // POST: api/HoghooghiUserCompaniesWithMajorInvestors
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] HoghooghiUserCompaniesWithMajorInvestorsDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var record = new HoghooghiUserCompaniesWithMajorInvestors
                {
                    UserId = dto.UserId,
                    CompanyName = dto.CompanyName,
                    CompanySubject = dto.CompanySubject,
                    PercentageOfTotal = dto.PercentageOfTotal
                };

                _context.HoghooghiUserCompaniesWithMajorInvestors.Add(record);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetById), new { id = record.Id }, record);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // PUT: api/HoghooghiUserCompaniesWithMajorInvestors/5
        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(int id, [FromBody] HoghooghiUserCompaniesWithMajorInvestorsDto dto)
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
                var record = _context.HoghooghiUserCompaniesWithMajorInvestors.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiCompaniesNotFound });
                }

                record.UserId = dto.UserId;
                record.CompanyName = dto.CompanyName;
                record.CompanySubject = dto.CompanySubject;
                record.PercentageOfTotal = dto.PercentageOfTotal;

                _context.SaveChanges();

                return Ok(record);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }



        }

        // DELETE: api/HoghooghiUserCompaniesWithMajorInvestors/5
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
                var record = _context.HoghooghiUserCompaniesWithMajorInvestors.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiCompaniesNotFound });
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

        // DELETE: api/HoghooghiUserCompaniesWithMajorInvestors
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearAll()
        {
            try
            {
                _context.HoghooghiUserCompaniesWithMajorInvestors.RemoveRange(_context.HoghooghiUserCompaniesWithMajorInvestors);
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
        public IActionResult MarkCompaniesAsComplete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var record = _context.HoghooghiUserCompaniesWithMajorInvestors.FirstOrDefault(e => e.UserId == id);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiCompaniesNotFound });
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

        [HttpGet("isComplete/{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CheckCompletionStatus(int id)
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


                var record = _context.HoghooghiUserCompaniesWithMajorInvestors.FirstOrDefault(e => e.UserId == id);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiCompaniesNotFound });
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
        public IActionResult GetAllHoghooghiUserCompaniesWithMajorInvestors()
        {
            try
            {
                var companies = _context.HoghooghiUserCompaniesWithMajorInvestors.ToList();
                var companyDtos = companies.Select(company => new HoghooghiUserCompaniesWithMajorInvestorsDto
                {
                    UserId = company.UserId,
                    CompanyName = company.CompanyName,
                    CompanySubject = company.CompanySubject,
                    PercentageOfTotal = company.PercentageOfTotal,
                    IsComplete = company.IsComplete,
                    IsDeleted = company.IsDeleted
                }).ToList();
                return Ok(companyDtos);
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
        public IActionResult GetHoghooghiUserCompaniesWithMajorInvestorsByIdAdmin(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var company = _context.HoghooghiUserCompaniesWithMajorInvestors.FirstOrDefault(c => c.UserId == id && !c.IsDeleted);
                if (company == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiCompaniesNotFound});
                }

                var companyDto = new HoghooghiUserCompaniesWithMajorInvestorsDto
                {
                    UserId = company.UserId,
                    CompanyName = company.CompanyName,
                    CompanySubject = company.CompanySubject,
                    PercentageOfTotal = company.PercentageOfTotal,
                    IsComplete = company.IsComplete,
                    IsDeleted = company.IsDeleted
                };

                return Ok(companyDto);
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
        public IActionResult UpdateHoghooghiUserCompaniesWithMajorInvestorsAdmin(int id, [FromBody] HoghooghiUserCompaniesWithMajorInvestorsDto companyDto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var company = _context.HoghooghiUserCompaniesWithMajorInvestors.FirstOrDefault(c => c.UserId == id && !c.IsDeleted);
                if (company == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiCompaniesNotFound });
                }

                company.CompanyName = companyDto.CompanyName;
                company.CompanySubject = companyDto.CompanySubject;
                company.PercentageOfTotal = companyDto.PercentageOfTotal;


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
        public IActionResult DeleteHoghooghiUserCompaniesWithMajorInvestorsAdmin(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var company = _context.HoghooghiUserCompaniesWithMajorInvestors.FirstOrDefault(c => c.UserId == id && !c.IsDeleted);
                if (company == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiCompaniesNotFound});
                }

                company.IsDeleted = true;
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
        public IActionResult ClearHoghooghiUserCompaniesWithMajorInvestors()
        {
            try
            {
                _context.HoghooghiUserCompaniesWithMajorInvestors.RemoveRange(_context.HoghooghiUserCompaniesWithMajorInvestors);
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
