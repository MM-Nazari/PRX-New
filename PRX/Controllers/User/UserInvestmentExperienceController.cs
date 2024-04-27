using Microsoft.AspNetCore.Mvc;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserInvestmentExperienceById(int id)
        {
            var userInvestmentExperience = _context.UserInvestmentExperiences.Find(id);
            if (userInvestmentExperience == null)
            {
                return NotFound();
            }
            return Ok(userInvestmentExperience);
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserInvestmentExperience(int id, [FromBody] UserInvestmentExperienceDto userInvestmentExperienceDto)
        {
            var userInvestmentExperience = _context.UserInvestmentExperiences.Find(id);
            if (userInvestmentExperience == null)
            {
                return NotFound();
            }

            userInvestmentExperience.UserId = userInvestmentExperienceDto.UserId;
            userInvestmentExperience.InvestmentType = userInvestmentExperienceDto.InvestmentType;
            userInvestmentExperience.InvestmentAmount = userInvestmentExperienceDto.InvestmentAmount;
            userInvestmentExperience.InvestmentDurationMonths = userInvestmentExperienceDto.InvestmentDurationMonths;
            userInvestmentExperience.ProfitLossAmount = userInvestmentExperienceDto.ProfitLossAmount;
            userInvestmentExperience.ProfitLossDescription = userInvestmentExperienceDto.ProfitLossDescription;
            userInvestmentExperience.ConversionReason = userInvestmentExperienceDto.ConversionReason;

            _context.SaveChanges();

            return Ok(userInvestmentExperience);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserInvestmentExperience(int id)
        {
            var userInvestmentExperience = _context.UserInvestmentExperiences.Find(id);
            if (userInvestmentExperience == null)
            {
                return NotFound();
            }

            _context.UserInvestmentExperiences.Remove(userInvestmentExperience);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult ClearUserInvestmentExperiences()
        {
            _context.UserInvestmentExperiences.RemoveRange(_context.UserInvestmentExperiences);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
