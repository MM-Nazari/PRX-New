using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.Hoghooghi;
using PRX.Models.Hoghooghi;

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
            var records = _context.HoghooghiUserInvestmentDepartmentStaff.ToList();
            return Ok(records);
        }

        // GET: api/HoghooghiUserInvestmentDepartmentStaff/5
        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
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
                var record = _context.HoghooghiUserInvestmentDepartmentStaff.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound();
                }
                return Ok(record);

            }

            catch (Exception ex)
            {
                
                return BadRequest();
            }


      
        }

        // POST: api/HoghooghiUserInvestmentDepartmentStaff
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] HoghooghiUserInvestmentDepartmentStaffDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var record = new HoghooghiUserInvestmentDepartmentStaff
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

            _context.HoghooghiUserInvestmentDepartmentStaff.Add(record);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = record.Id }, record);
        }

        // PUT: api/HoghooghiUserInvestmentDepartmentStaff/5
        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(int id, [FromBody] HoghooghiUserInvestmentDepartmentStaffDto dto)
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
                var record = _context.HoghooghiUserInvestmentDepartmentStaff.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound();
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
                
                return BadRequest();
            }


        }

        // DELETE: api/HoghooghiUserInvestmentDepartmentStaff/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
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
                var record = _context.HoghooghiUserInvestmentDepartmentStaff.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
                if (record == null)
                {
                    return NotFound();
                }

                record.IsDeleted = true;
                _context.SaveChanges();

                return Ok();

            }

            catch (Exception ex)
            {
                
                return BadRequest();
            }

     
        }

        // DELETE: api/HoghooghiUserInvestmentDepartmentStaff
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearAll()
        {
            _context.HoghooghiUserInvestmentDepartmentStaff.RemoveRange(_context.HoghooghiUserInvestmentDepartmentStaff);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPut("complete/{id}")]
        //[Authorize(Roles = "Admin")] // Assuming only admins can mark profiles as complete
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkCompaniesAsComplete(int id)
        {
            var record = _context.HoghooghiUserInvestmentDepartmentStaff.FirstOrDefault(e => e.UserId == id);
            if (record == null)
            {
                return NotFound();
            }

            record.IsComplete = true;
            _context.SaveChanges();

            return Ok();
        }

        [HttpGet("Admin")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAllHoghooghiUserInvestmentDepartmentStaff()
        {
            var users = _context.HoghooghiUserInvestmentDepartmentStaff.ToList();
            var userDtos = users.Select(user => new HoghooghiUserInvestmentDepartmentStaffDto
            {
                UserId = user.UserId,
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

        [HttpGet("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHoghooghiUserInvestmentDepartmentStaffByIdAdmin(int id)
        {
            var user = _context.HoghooghiUserInvestmentDepartmentStaff.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
            if (user == null)
            {
                return NotFound();
            }

            var userDto = new HoghooghiUserInvestmentDepartmentStaffDto
            {
                UserId = user.UserId,
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

        [HttpPut("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHoghooghiUserInvestmentDepartmentStaffAdmin(int id, [FromBody] HoghooghiUserInvestmentDepartmentStaffDto userDto)
        {
            var user = _context.HoghooghiUserInvestmentDepartmentStaff.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
            if (user == null)
            {
                return NotFound();
            }

            user.FullName = userDto.FullName;
            user.Position = userDto.Position;
            user.EducationalLevel = userDto.EducationalLevel;
            user.FieldOfStudy = userDto.FieldOfStudy;
            user.ExecutiveExperience = userDto.ExecutiveExperience;
            user.FamiliarityWithCapitalMarket = userDto.FamiliarityWithCapitalMarket;
            user.PersonalInvestmentExperienceInStockExchange = userDto.PersonalInvestmentExperienceInStockExchange;


            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHoghooghiUserInvestmentDepartmentStaffAdmin(int id)
        {
            var user = _context.HoghooghiUserInvestmentDepartmentStaff.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
            if (user == null)
            {
                return NotFound();
            }

            user.IsDeleted = true;
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("Admin/Clear")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearHoghooghiUserInvestmentDepartmentStaff()
        {
            _context.HoghooghiUserInvestmentDepartmentStaff.RemoveRange(_context.HoghooghiUserInvestmentDepartmentStaff);
            _context.SaveChanges();

            return Ok();
        }

    }
}
