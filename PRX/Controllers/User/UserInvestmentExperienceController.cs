using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using PRX.Data;
using PRX.Dto.User;
using PRX.Models.User;

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
            var userInvestmentExperiences = _context.UserInvestmentExperiences.ToList();
            return Ok(userInvestmentExperiences);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserInvestmentExperienceById(int id)
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
                var userInvestmentExperience = _context.UserInvestmentExperiences.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userInvestmentExperience == null)
                {
                    return NotFound();
                }
                return Ok(userInvestmentExperience);

            }

            catch (Exception ex)
            {

                return BadRequest();
            }

 
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateUserInvestmentExperience([FromBody] UserInvestmentExperienceDto userInvestmentExperienceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userInvestmentExperience = new UserInvestmentExperience
            {
                UserId = userInvestmentExperienceDto.UserId,
                InvestmentType = userInvestmentExperienceDto.InvestmentType,
                InvestmentAmount = userInvestmentExperienceDto.InvestmentAmount,
                InvestmentDurationMonths = userInvestmentExperienceDto.InvestmentDurationMonths,
                ProfitLossAmount = userInvestmentExperienceDto.ProfitLossAmount,
                ProfitLossDescription = userInvestmentExperienceDto.ProfitLossDescription,
                ConversionReason = userInvestmentExperienceDto.ConversionReason
            };

            _context.UserInvestmentExperiences.Add(userInvestmentExperience);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUserInvestmentExperienceById), new { id = userInvestmentExperience.Id }, userInvestmentExperience);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserInvestmentExperience(int id, [FromBody] UserInvestmentExperienceDto userInvestmentExperienceDto)
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
                var userInvestmentExperience = _context.UserInvestmentExperiences.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userInvestmentExperience == null)
                {
                    return NotFound();
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
                
                return BadRequest();
            }



        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserInvestmentExperience(int id)
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
                var userInvestmentExperience = _context.UserInvestmentExperiences.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userInvestmentExperience == null)
                {
                    return NotFound();
                }

                userInvestmentExperience.IsDeleted = true;
                _context.SaveChanges();

                return NoContent();

            }

            catch (Exception ex)
            {
                
                return BadRequest();
            }


        }

        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult ClearUserInvestmentExperiences()
        {
            _context.UserInvestmentExperiences.RemoveRange(_context.UserInvestmentExperiences);
            _context.SaveChanges();

            return NoContent();
        }

        // PUT: api/HaghighiUserProfile/complete/{id}
        [HttpPut("complete/{id}")]
        //[Authorize(Roles = "Admin")] // Assuming only admins can mark profiles as complete
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkFinancwChangeAsComplete(int id)
        {
            var userFinancialChanges = _context.UserInvestmentExperiences.FirstOrDefault(u => u.UserId == id);
            if (userFinancialChanges == null)
            {
                return NotFound();
            }

            userFinancialChanges.IsComplete = true;
            _context.SaveChanges();

            return Ok();
        }

        [HttpGet("Admin")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAllUserInvestmentExperiencesAdmin()
        {
            var experiences = _context.UserInvestmentExperiences.ToList();
            var experienceDtos = experiences.Select(experience => new UserInvestmentExperienceDto
            {
                UserId = experience.UserId,
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

        [HttpGet("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserInvestmentExperienceByIdAdmin(int id)
        {
            var experience = _context.UserInvestmentExperiences.FirstOrDefault(exp => exp.UserId == id && !exp.IsDeleted);
            if (experience == null)
            {
                return NotFound();
            }

            var experienceDto = new UserInvestmentExperienceDto
            {
                UserId = experience.UserId,
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

        [HttpPut("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserInvestmentExperienceAdmin(int id, [FromBody] UserInvestmentExperienceDto experienceDto)
        {
            var experience = _context.UserInvestmentExperiences.FirstOrDefault(exp => exp.UserId == id && !exp.IsDeleted);
            if (experience == null)
            {
                return NotFound();
            }

            experience.InvestmentType = experienceDto.InvestmentType;
            experience.InvestmentAmount = experienceDto.InvestmentAmount;
            experience.InvestmentDurationMonths = experienceDto.InvestmentDurationMonths;
            experience.ProfitLossAmount = experienceDto.ProfitLossAmount;
            experience.ProfitLossDescription = experienceDto.ProfitLossDescription;
            experience.ConversionReason = experienceDto.ConversionReason;

            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserInvestmentExperienceAdmin(int id)
        {
            var experience = _context.UserInvestmentExperiences.FirstOrDefault(exp => exp.UserId == id && !exp.IsDeleted);
            if (experience == null)
            {
                return NotFound();
            }

            experience.IsDeleted = true;
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("Admin/Clear")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearUserInvestmentExperiencesAdmin()
        {
            _context.UserInvestmentExperiences.RemoveRange(_context.UserInvestmentExperiences);
            _context.SaveChanges();

            return Ok();
        }

    }
}
