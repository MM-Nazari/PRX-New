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
        [HttpGet("{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHaghighiUserProfileById(int requestId)
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
                var profile = _context.HaghighiUserProfiles.FirstOrDefault(u => u.RequestId == requestId && !u.IsDeleted);
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
        [HttpPut("{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHaghighiUserProfile(int requestId, [FromBody] HaghighiUserProfileDto profileDto)
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

                var profile = _context.HaghighiUserProfiles.FirstOrDefault(u => u.RequestId == requestId && !u.IsDeleted);
                if (profile == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserProfileNotFound });
                }

                // Update profile properties
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        // DELETE: api/HaghighiUserProfile/5
        [HttpDelete("{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHaghighiUserProfile(int requestId)
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

                var profile = _context.HaghighiUserProfiles.FirstOrDefault(u => u.RequestId == requestId && !u.IsDeleted);
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
                    .FirstOrDefault(p => p.NationalNumber == profileDto.NationalNumber);

                if (existingProfile != null)
                {
                    return BadRequest(new { message = ResponseMessages.HaghighiUserProfileDuplicateBirthCertificate });
                }

                // Check for unique UserId
                var existingProfileByUserId = _context.HaghighiUserProfiles
                    .FirstOrDefault(p => p.RequestId == profileDto.RequestId);

                if (existingProfileByUserId != null)
                {
                    return BadRequest(new { message = ResponseMessages.HaghighiUserProfileDuplicate });
                }

                var profile = new HaghighiUserProfile
                {
                    RequestId = profileDto.RequestId,
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

                return CreatedAtAction(nameof(GetHaghighiUserProfileById), new { requestId = profile.Id }, profile);
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
        [HttpPut("complete/{requestId}")]
        //[Authorize(Roles = "Admin")] // Assuming only admins can mark profiles as complete
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkProfileAsComplete(int requestId)
        {

            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var profile = _context.HaghighiUserProfiles.FirstOrDefault(e => e.RequestId == requestId);
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


        [HttpGet("isComplete/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CheckCompletionStatus(int requestId)
        {
            try
            {
                if (requestId <= 0)
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

                var record = _context.HaghighiUserProfiles.FirstOrDefault(e => e.RequestId == requestId);
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
                    RequestId = profile.RequestId,
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

        [HttpGet("Admin/{requestId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHaghighiUserProfileByIdAdmin(int requestId)
        {

            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var profile = _context.HaghighiUserProfiles.FirstOrDefault(p => p.RequestId == requestId && !p.IsDeleted);
                if (profile == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserProfileNotFound });
                }

                var profileDto = new HaghighiUserProfileDto
                {
                    RequestId = profile.RequestId,
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

        [HttpPut("Admin/{requestId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHaghighiUserProfileAdmin(int requestId, [FromBody] HaghighiUserProfileDto profileDto)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var profile = _context.HaghighiUserProfiles.FirstOrDefault(p => p.RequestId == requestId && !p.IsDeleted);
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

        [HttpDelete("Admin/{requestId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHaghighiUserProfileAdmin(int requestId)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var profile = _context.HaghighiUserProfiles.FirstOrDefault(p => p.RequestId == requestId && !p.IsDeleted);
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
