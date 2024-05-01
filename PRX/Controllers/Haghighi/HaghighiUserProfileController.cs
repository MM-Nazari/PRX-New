using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.Haghighi;
using PRX.Models.Haghighi;

namespace PRX.Controllers.Haghighi
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "HaghighiUserProfiles")]
    public class HaghighiUserProfileController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public HaghighiUserProfileController(PRXDbContext context)
        {
            _context = context;
        }

        // GET: api/HaghighiUserProfile
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllHaghighiUserProfiles()
        {
            var profiles = _context.HaghighiUserProfiles.ToList();
            return Ok(profiles);
        }

        // GET: api/HaghighiUserProfile/5
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHaghighiUserProfileById(int id)
        {
            try {

                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Ensure that the user is updating their own profile
                if (id != tokenUserId)
                {
                    return Forbid(); // Or return 403 Forbidden
                }
                var profile = _context.HaghighiUserProfiles.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (profile == null)
                {
                    return NotFound();
                }
                return Ok(profile);

            }

            catch (Exception ex)
            {
                
                return BadRequest();
            }
        }


        // PUT: api/HaghighiUserProfile/5
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHaghighiUserProfile(int id, [FromBody] HaghighiUserProfileDto profileDto)
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

                var profile = _context.HaghighiUserProfiles.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (profile == null)
                {
                    return NotFound();
                }

                // Update profile properties
                profile.FirstName = profileDto.FirstName;
                profile.LastName = profileDto.LastName;
                // Update other properties as needed...

                _context.SaveChanges();

                return Ok(profile);
            }
            catch (Exception ex)
            {
                
                return BadRequest();
            }
        }

        // DELETE: api/HaghighiUserProfile/5
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHaghighiUserProfile(int id)
        {
            try
            {
                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Ensure that the user is deleting their own profile
                if (id != tokenUserId)
                {
                    return Forbid(); // Or return 403 Forbidden
                }

                var profile = _context.HaghighiUserProfiles.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (profile == null)
                {
                    return NotFound();
                }

                profile.IsDeleted = true;
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                
                return BadRequest();
            }
        }


        // POST: api/HaghighiUserProfile
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateHaghighiUserProfile([FromBody] HaghighiUserProfileDto profileDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var profile = new HaghighiUserProfile
            {
                UserId = profileDto.UserId,
                FirstName = profileDto.FirstName,
                LastName = profileDto.LastName,
                FathersName = profileDto.FathersName,
                NationalNumber = profileDto.NationalNumber,
                BirthDate = profileDto.BirthDate,
                BirthPlace = profileDto.BirthPlace,
                BirthCertificateNumber = profileDto.BirthCertificateNumber,
                MaritalStatus = profileDto.MaritalStatus,
                Gender = profileDto.Gender,
                PostalCode = profileDto.PostalCode,
                HomePhone = profileDto.HomePhone,
                Fax = profileDto.Fax,
                BestTimeToCall = profileDto.BestTimeToCall,
                ResidentialAddress = profileDto.ResidentialAddress,
                Email = profileDto.Email
            };

            _context.HaghighiUserProfiles.Add(profile);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetHaghighiUserProfileById), new { id = profile.Id }, profile);
        }

  
        // DELETE: api/HaghighiUserProfile/clear
        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearAllHaghighiUserProfiles()
        {
            _context.HaghighiUserProfiles.RemoveRange(_context.HaghighiUserProfiles);
            _context.SaveChanges();

            return Ok();
        }

        // PUT: api/HaghighiUserProfile/complete/{id}
        [HttpPut("complete/{id}")]
        //[Authorize(Roles = "Admin")] // Assuming only admins can mark profiles as complete
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkProfileAsComplete(int id)
        {
            var profile = _context.HaghighiUserProfiles.FirstOrDefault(e => e.UserId == id);
            if (profile == null)
            {
                return NotFound();
            }

            profile.IsComplete = true;
            _context.SaveChanges();

            return Ok();
        }

    }
}
