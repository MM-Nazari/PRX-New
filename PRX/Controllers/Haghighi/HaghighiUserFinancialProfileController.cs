using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.Haghighi;
using PRX.Models.Haghighi;
using PRX.Utils;

namespace PRX.Controllers.Haghighi
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "HaghighiUserFinancialProfiles")]
    public class HaghighiUserFinancialProfileController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public HaghighiUserFinancialProfileController(PRXDbContext context)
        {
            _context = context;
        }

        // GET: api/HaghighiUserFinancialProfile
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllHaghighiUserFinancialProfiles()
        {
            try
            {
                var financialProfiles = _context.HaghighiUserFinancialProfiles.ToList();
                return Ok(financialProfiles);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        // GET: api/HaghighiUserFinancialProfile/5
        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHaghighiUserFinancialProfileById(int id)
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
                var financialProfile = _context.HaghighiUserFinancialProfiles.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
                if (financialProfile == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserFinancialProfileNotFound});
                }
                return Ok(financialProfile);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // POST: api/HaghighiUserFinancialProfile
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateHaghighiUserFinancialProfile([FromBody] HaghighiUserFinancialProfileDto financialProfileDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var financialProfile = new HaghighiUserFinancialProfile
                {
                    UserId = financialProfileDto.UserId,
                    MainContinuousIncome = financialProfileDto.MainContinuousIncome,
                    OtherIncomes = financialProfileDto.OtherIncomes,
                    SupportFromOthers = financialProfileDto.SupportFromOthers,
                    ContinuousExpenses = financialProfileDto.ContinuousExpenses,
                    OccasionalExpenses = financialProfileDto.OccasionalExpenses,
                    ContributionToOthers = financialProfileDto.ContributionToOthers
                };

                _context.HaghighiUserFinancialProfiles.Add(financialProfile);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetHaghighiUserFinancialProfileById), new { id = financialProfile.Id }, financialProfile);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        // PUT: api/HaghighiUserFinancialProfile/5
        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHaghighiUserFinancialProfile(int id, [FromBody] HaghighiUserFinancialProfileDto financialProfileDto)
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
                var financialProfile = _context.HaghighiUserFinancialProfiles.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
                if (financialProfile == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserFinancialProfileNotFound });
                }

                financialProfile.UserId = financialProfileDto.UserId;
                financialProfile.MainContinuousIncome = financialProfileDto.MainContinuousIncome;
                financialProfile.OtherIncomes = financialProfileDto.OtherIncomes;
                financialProfile.SupportFromOthers = financialProfileDto.SupportFromOthers;
                financialProfile.ContinuousExpenses = financialProfileDto.ContinuousExpenses;
                financialProfile.OccasionalExpenses = financialProfileDto.OccasionalExpenses;
                financialProfile.ContributionToOthers = financialProfileDto.ContributionToOthers;

                _context.SaveChanges();

                return Ok(financialProfile);

            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        // DELETE: api/HaghighiUserFinancialProfile/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHaghighiUserFinancialProfile(int id)
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
                var financialProfile = _context.HaghighiUserFinancialProfiles.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
                if (financialProfile == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserFinancialProfileNotFound });
                }

                financialProfile.IsDeleted = true;
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        // DELETE: api/HaghighiUserFinancialProfile/clear
        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearAllHaghighiUserFinancialProfiles()
        {
            try
            {
                _context.HaghighiUserFinancialProfiles.RemoveRange(_context.HaghighiUserFinancialProfiles);
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // PUT: api/HaghighiUserProfile/complete/{id}
        [HttpPut("complete/{id}")]
        //[Authorize(Roles = "Admin")] // Assuming only admins can mark profiles as complete
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkFinanceProfileAsComplete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var financialProfile = _context.HaghighiUserFinancialProfiles.FirstOrDefault(e => e.UserId == id);
                if (financialProfile == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserFinancialProfileNotFound });
                }

                financialProfile.IsComplete = true;
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


                var record = _context.HaghighiUserFinancialProfiles.FirstOrDefault(e => e.UserId == id);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserFinancialProfileNotFound });
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
        public IActionResult GetAllHaghighiUserFinancialProfilesAdmin()
        {

            try
            {
                var profiles = _context.HaghighiUserFinancialProfiles.ToList();
                var profileDtos = profiles.Select(profile => new HaghighiUserFinancialProfileDto
                {
                    UserId = profile.UserId,
                    MainContinuousIncome = profile.MainContinuousIncome,
                    OtherIncomes = profile.OtherIncomes,
                    SupportFromOthers = profile.SupportFromOthers,
                    ContinuousExpenses = profile.ContinuousExpenses,
                    OccasionalExpenses = profile.OccasionalExpenses,
                    ContributionToOthers = profile.ContributionToOthers,
                    IsComplete = profile.IsComplete,
                    IsDeleted = profile.IsDeleted
                }).ToList();
                return Ok(profileDtos);
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
        public IActionResult GetHaghighiUserFinancialProfileByIdAdmin(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var profile = _context.HaghighiUserFinancialProfiles.FirstOrDefault(p => p.UserId == id && !p.IsDeleted);
                if (profile == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserFinancialProfileNotFound });
                }

                var profileDto = new HaghighiUserFinancialProfileDto
                {
                    UserId = profile.UserId,
                    MainContinuousIncome = profile.MainContinuousIncome,
                    OtherIncomes = profile.OtherIncomes,
                    SupportFromOthers = profile.SupportFromOthers,
                    ContinuousExpenses = profile.ContinuousExpenses,
                    OccasionalExpenses = profile.OccasionalExpenses,
                    ContributionToOthers = profile.ContributionToOthers,
                    IsComplete = profile.IsComplete,
                    IsDeleted = profile.IsDeleted
                };

                return Ok(profileDto);
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
        public IActionResult UpdateHaghighiUserFinancialProfileAdmin(int id, [FromBody] HaghighiUserFinancialProfileDto profileDto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var profile = _context.HaghighiUserFinancialProfiles.FirstOrDefault(p => p.UserId == id && !p.IsDeleted);
                if (profile == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserFinancialProfileNotFound });
                }

                profile.MainContinuousIncome = profileDto.MainContinuousIncome;
                profile.OtherIncomes = profileDto.OtherIncomes;
                profile.SupportFromOthers = profileDto.SupportFromOthers;
                profile.ContinuousExpenses = profileDto.ContinuousExpenses;
                profile.OccasionalExpenses = profileDto.OccasionalExpenses;
                profile.ContributionToOthers = profileDto.ContributionToOthers;
                //profile.IsComplete = profileDto.IsComplete;
                //profile.IsDeleted = profileDto.IsDeleted;

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
        public IActionResult DeleteHaghighiUserFinancialProfileAdmin(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var profile = _context.HaghighiUserFinancialProfiles.FirstOrDefault(p => p.UserId == id && !p.IsDeleted);
                if (profile == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserFinancialProfileNotFound });
                }

                profile.IsDeleted = true;
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
        public IActionResult ClearHaghighiUserFinancialProfiles()
        {
            try
            {
                _context.HaghighiUserFinancialProfiles.RemoveRange(_context.HaghighiUserFinancialProfiles);
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
