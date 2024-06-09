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
            try
            {
                var profiles = _context.HaghighiUserProfiles.ToList();
                return Ok(profiles);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // GET: api/HaghighiUserProfile/5
        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHaghighiUserProfileById(int id)
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
                var profile = _context.HaghighiUserProfiles.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (profile == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserProfileNotFound});
                }
                return Ok(profile);

            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }


        // PUT: api/HaghighiUserProfile/5
        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHaghighiUserProfile(int id, [FromBody] HaghighiUserProfileDto profileDto)
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

                var profile = _context.HaghighiUserProfiles.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (profile == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserProfileNotFound });
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
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        // DELETE: api/HaghighiUserProfile/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHaghighiUserProfile(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }
                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Ensure that the user is deleting their own profile
                if (id != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }

                var profile = _context.HaghighiUserProfiles.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (profile == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserProfileNotFound });
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


        // POST: api/HaghighiUserProfile
        [HttpPost]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateHaghighiUserProfile([FromBody] HaghighiUserProfileDto profileDto)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check for unique BirthCertificateNumber
                var existingProfile = _context.HaghighiUserProfiles
                    .FirstOrDefault(p => p.BirthCertificateNumber == profileDto.BirthCertificateNumber);

                if (existingProfile != null)
                {
                    return BadRequest(new { message = ResponseMessages.HaghighiUserProfileDuplicateBirthCertificate });
                }

                // Check for unique UserId
                var existingProfileByUserId = _context.HaghighiUserProfiles
                    .FirstOrDefault(p => p.UserId == profileDto.UserId);

                if (existingProfileByUserId != null)
                {
                    return BadRequest(new { message = ResponseMessages.HaghighiUserProfileDuplicate });
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

  
        // DELETE: api/HaghighiUserProfile/clear
        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearAllHaghighiUserProfiles()
        {
            try
            {
                _context.HaghighiUserProfiles.RemoveRange(_context.HaghighiUserProfiles);
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
        public IActionResult MarkProfileAsComplete(int id)
        {

            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var profile = _context.HaghighiUserProfiles.FirstOrDefault(e => e.UserId == id);
                if (profile == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserProfileNotFound });
                }

                profile.IsComplete = true;
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


                var record = _context.HaghighiUserProfiles.FirstOrDefault(e => e.UserId == id);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserProfileNotFound });
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
        public IActionResult GetAllHaghighiUserProfilesAdmin()
        {

            try
            {
                var profiles = _context.HaghighiUserProfiles.ToList();
                var profileDtos = profiles.Select(profile => new HaghighiUserProfileDto
                {
                    UserId = profile.UserId,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    FathersName = profile.FathersName,
                    NationalNumber = profile.NationalNumber,
                    BirthDate = profile.BirthDate,
                    BirthPlace = profile.BirthPlace,
                    BirthCertificateNumber = profile.BirthCertificateNumber,
                    MaritalStatus = profile.MaritalStatus,
                    Gender = profile.Gender,
                    PostalCode = profile.PostalCode,
                    HomePhone = profile.HomePhone,
                    Fax = profile.Fax,
                    BestTimeToCall = profile.BestTimeToCall,
                    ResidentialAddress = profile.ResidentialAddress,
                    Email = profile.Email,
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
        public IActionResult GetHaghighiUserProfileByIdAdmin(int id)
        {

            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var profile = _context.HaghighiUserProfiles.FirstOrDefault(p => p.UserId == id && !p.IsDeleted);
                if (profile == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserProfileNotFound });
                }

                var profileDto = new HaghighiUserProfileDto
                {
                    UserId = profile.UserId,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    FathersName = profile.FathersName,
                    NationalNumber = profile.NationalNumber,
                    BirthDate = profile.BirthDate,
                    BirthPlace = profile.BirthPlace,
                    BirthCertificateNumber = profile.BirthCertificateNumber,
                    MaritalStatus = profile.MaritalStatus,
                    Gender = profile.Gender,
                    PostalCode = profile.PostalCode,
                    HomePhone = profile.HomePhone,
                    Fax = profile.Fax,
                    BestTimeToCall = profile.BestTimeToCall,
                    ResidentialAddress = profile.ResidentialAddress,
                    Email = profile.Email,
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
        public IActionResult UpdateHaghighiUserProfileAdmin(int id, [FromBody] HaghighiUserProfileDto profileDto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var profile = _context.HaghighiUserProfiles.FirstOrDefault(p => p.UserId == id && !p.IsDeleted);
                if (profile == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserProfileNotFound });
                }

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
        public IActionResult DeleteHaghighiUserProfileAdmin(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var profile = _context.HaghighiUserProfiles.FirstOrDefault(p => p.UserId == id && !p.IsDeleted);
                if (profile == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserProfileNotFound });
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
        public IActionResult ClearHaghighiUserProfiles()
        {
            try
            {
                _context.HaghighiUserProfiles.RemoveRange(_context.HaghighiUserProfiles);
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
