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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHaghighiUserProfileById(int id)
        {
            var profile = _context.HaghighiUserProfiles.FirstOrDefault(e => e.Id == id);
            if (profile == null)
            {
                return NotFound();
            }
            return Ok(profile);
        }

        // POST: api/HaghighiUserProfile
        [HttpPost]
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

        // PUT: api/HaghighiUserProfile/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHaghighiUserProfile(int id, [FromBody] HaghighiUserProfileDto profileDto)
        {
            var profile = _context.HaghighiUserProfiles.FirstOrDefault(e => e.Id == id);
            if (profile == null)
            {
                return NotFound();
            }

            profile.UserId = profileDto.UserId;
            profile.FirstName = profileDto.FirstName;
            profile.LastName = profileDto.LastName;
            profile.FathersName = profileDto.FathersName;
            profile.NationalNumber = profileDto.NationalNumber;
            profile.BirthDate = profileDto.BirthDate;
            profile.BirthPlace = profileDto.BirthPlace;
            profile.BirthCertificateNumber = profileDto.BirthCertificateNumber;
            profile.MaritalStatus = profileDto.MaritalStatus;
            profile.Gender = profileDto.Gender;
            profile.PostalCode = profileDto.PostalCode;
            profile.HomePhone = profileDto.HomePhone;
            profile.Fax = profileDto.Fax;
            profile.BestTimeToCall = profileDto.BestTimeToCall;
            profile.ResidentialAddress = profileDto.ResidentialAddress;
            profile.Email = profileDto.Email;

            _context.SaveChanges();

            return Ok(profile);
        }

        // DELETE: api/HaghighiUserProfile/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHaghighiUserProfile(int id)
        {
            var profile = _context.HaghighiUserProfiles.FirstOrDefault(e => e.Id == id);
            if (profile == null)
            {
                return NotFound();
            }

            _context.HaghighiUserProfiles.Remove(profile);
            _context.SaveChanges();

            return Ok();
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
    }
}
