﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.Hoghooghi;
using PRX.Models.Hoghooghi;
using PRX.Utils;

namespace PRX.Controllers.Hoghooghi
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "HoghooghiUsers")]
    public class HoghooghiUserController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public HoghooghiUserController(PRXDbContext context)
        {
            _context = context;
        }

        // GET: api/HoghooghiUser
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllHoghooghiUsers()
        {
            try
            {
                var users = _context.HoghooghiUsers.ToList();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
  
        }

        // GET: api/HoghooghiUser/5
        [HttpGet("{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHoghooghiUserById(int requestId)
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

                var user = _context.HoghooghiUsers.FirstOrDefault(e => e.RequestId == requestId && !e.IsDeleted);
                if (user == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiUserNotFound});
                }
                return Ok(user);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        // POST: api/HoghooghiUser
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateHoghooghiUser([FromBody] HoghooghiUserDto userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check for unique UserId
                var existingUser = _context.HoghooghiUsers
                                           .FirstOrDefault(u => u.RequestId == userDto.RequestId);

                if (existingUser != null)
                {
                    return BadRequest(new { message = ResponseMessages.HoghooghiUserDuplicate });
                }

                var user = new HoghooghiUser
                {
                    RequestId = userDto.RequestId,
                    Name = userDto.Name,
                    RegistrationNumber = userDto.RegistrationNumber,
                    RegistrationDate = userDto.RegistrationDate,
                    RegistrationLocation = userDto.RegistrationLocation,
                    NationalId = userDto.NationalId,
                    MainActivityBasedOnCharter = userDto.MainActivityBasedOnCharter,
                    MainActivityBasedOnPastThreeYearsPerformance = userDto.MainActivityBasedOnPastThreeYearsPerformance,
                    PostalCode = userDto.PostalCode,
                    LandlinePhone = userDto.LandlinePhone,
                    Fax = userDto.Fax,
                    BestTimeToCall = userDto.BestTimeToCall,
                    Address = userDto.Address,
                    Email = userDto.Email,
                    RepresentativeName = userDto.RepresentativeName,
                    RepresentativeNationalId = userDto.RepresentativeNationalId,
                    RepresentativeMobilePhone = userDto.RepresentativeMobilePhone
                };

                _context.HoghooghiUsers.Add(user);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetHoghooghiUserById), new { requestId = user.Id }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // PUT: api/HoghooghiUser/5
        [HttpPut("{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHoghooghiUser(int requestId, [FromBody] HoghooghiUserDto userDto)
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

                var user = _context.HoghooghiUsers.FirstOrDefault(e => e.RequestId == requestId && !e.IsDeleted);
                if (user == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiUserNotFound });
                }

                user.RequestId = userDto.RequestId;
                user.Name = userDto.Name;
                user.RegistrationNumber = userDto.RegistrationNumber;
                user.RegistrationDate = userDto.RegistrationDate;
                user.RegistrationLocation = userDto.RegistrationLocation;
                user.NationalId = userDto.NationalId;
                user.MainActivityBasedOnCharter = userDto.MainActivityBasedOnCharter;
                user.MainActivityBasedOnPastThreeYearsPerformance = userDto.MainActivityBasedOnPastThreeYearsPerformance;
                user.PostalCode = userDto.PostalCode;
                user.LandlinePhone = userDto.LandlinePhone;
                user.Fax = userDto.Fax;
                user.BestTimeToCall = userDto.BestTimeToCall;
                user.Address = userDto.Address;
                user.Email = userDto.Email;
                user.RepresentativeName = userDto.RepresentativeName;
                user.RepresentativeNationalId = userDto.RepresentativeNationalId;
                user.RepresentativeMobilePhone = userDto.RepresentativeMobilePhone;

                _context.SaveChanges();

                return Ok(user);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        // PATCH: api/HoghooghiUser/{requestId}
        [HttpPatch("{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult PatchHoghooghiUser(int requestId, [FromBody] JsonPatchDocument<HoghooghiUserDto> patchDoc)
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

                // Fetch the existing user
                var user = _context.HoghooghiUsers.FirstOrDefault(e => e.RequestId == requestId && !e.IsDeleted);
                if (user == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiUserNotFound });
                }

                // Create a DTO to apply the patch
                var userDto = new HoghooghiUserDto
                {
                    RequestId = user.RequestId,
                    Name = user.Name,
                    RegistrationNumber = user.RegistrationNumber,
                    RegistrationDate = user.RegistrationDate,
                    RegistrationLocation = user.RegistrationLocation,
                    NationalId = user.NationalId,
                    MainActivityBasedOnCharter = user.MainActivityBasedOnCharter,
                    MainActivityBasedOnPastThreeYearsPerformance = user.MainActivityBasedOnPastThreeYearsPerformance,
                    PostalCode = user.PostalCode,
                    LandlinePhone = user.LandlinePhone,
                    Fax = user.Fax,
                    BestTimeToCall = user.BestTimeToCall,
                    Address = user.Address,
                    Email = user.Email,
                    RepresentativeName = user.RepresentativeName,
                    RepresentativeNationalId = user.RepresentativeNationalId,
                    RepresentativeMobilePhone = user.RepresentativeMobilePhone
                };

                // Apply the patch document to the DTO
                patchDoc.ApplyTo(userDto);

                // Update the user with modified values from the DTO
                user.RequestId = userDto.RequestId;
                user.Name = userDto.Name;
                user.RegistrationNumber = userDto.RegistrationNumber;
                user.RegistrationDate = userDto.RegistrationDate;
                user.RegistrationLocation = userDto.RegistrationLocation;
                user.NationalId = userDto.NationalId;
                user.MainActivityBasedOnCharter = userDto.MainActivityBasedOnCharter;
                user.MainActivityBasedOnPastThreeYearsPerformance = userDto.MainActivityBasedOnPastThreeYearsPerformance;
                user.PostalCode = userDto.PostalCode;
                user.LandlinePhone = userDto.LandlinePhone;
                user.Fax = userDto.Fax;
                user.BestTimeToCall = userDto.BestTimeToCall;
                user.Address = userDto.Address;
                user.Email = userDto.Email;
                user.RepresentativeName = userDto.RepresentativeName;
                user.RepresentativeNationalId = userDto.RepresentativeNationalId;
                user.RepresentativeMobilePhone = userDto.RepresentativeMobilePhone;

                // Save changes to the database
                _context.SaveChanges();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }


        // DELETE: api/HoghooghiUser/5
        [HttpDelete("{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHoghooghiUser(int requestId)
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

                var user = _context.HoghooghiUsers.FirstOrDefault(e => e.RequestId == requestId && !e.IsDeleted);
                if (user == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiUserNotFound });
                }

                user.IsDeleted = true;
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });

            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        // DELETE: api/HoghooghiUser
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearAllHoghooghiUsers()
        {
            try
            {
                _context.HoghooghiUsers.RemoveRange(_context.HoghooghiUsers);
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }


        [HttpPut("complete/{requestId}")]
        //[Authorize(Roles = "Admin")] // Assuming only admins can mark profiles as complete
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkCompaniesAsComplete(int requestId)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var user = _context.HoghooghiUsers.FirstOrDefault(e => e.RequestId == requestId);
                if (user == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiUserNotFound });
                }

                user.IsComplete = true;
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

                var record = _context.HoghooghiUsers.FirstOrDefault(e => e.RequestId == requestId);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiUserNotFound });
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
        public IActionResult GetAllHoghooghiUsersAdmin()
        {
            try
            {
                var users = _context.HoghooghiUsers.ToList();
                var userDtos = users.Select(user => new HoghooghiUserDto
                {
                    RequestId = user.RequestId,
                    Name = user.Name,
                    RegistrationNumber = user.RegistrationNumber,
                    RegistrationDate = user.RegistrationDate,
                    RegistrationLocation = user.RegistrationLocation,
                    NationalId = user.NationalId,
                    MainActivityBasedOnCharter = user.MainActivityBasedOnCharter,
                    MainActivityBasedOnPastThreeYearsPerformance = user.MainActivityBasedOnPastThreeYearsPerformance,
                    PostalCode = user.PostalCode,
                    LandlinePhone = user.LandlinePhone,
                    Fax = user.Fax,
                    BestTimeToCall = user.BestTimeToCall,
                    Address = user.Address,
                    Email = user.Email,
                    RepresentativeName = user.RepresentativeName,
                    RepresentativeNationalId = user.RepresentativeNationalId,
                    RepresentativeMobilePhone = user.RepresentativeMobilePhone,
                    IsComplete = user.IsComplete,
                    IsDeleted = user.IsDeleted
                }).ToList();
                return Ok(userDtos);
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
        public IActionResult GetHoghooghiUserByIdAdmin(int requestId)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var user = _context.HoghooghiUsers.FirstOrDefault(u => u.RequestId == requestId && !u.IsDeleted);
                if (user == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiUserNotFound });
                }

                var userDto = new HoghooghiUserDto
                {
                    RequestId = user.RequestId,
                    Name = user.Name,
                    RegistrationNumber = user.RegistrationNumber,
                    RegistrationDate = user.RegistrationDate,
                    RegistrationLocation = user.RegistrationLocation,
                    NationalId = user.NationalId,
                    MainActivityBasedOnCharter = user.MainActivityBasedOnCharter,
                    MainActivityBasedOnPastThreeYearsPerformance = user.MainActivityBasedOnPastThreeYearsPerformance,
                    PostalCode = user.PostalCode,
                    LandlinePhone = user.LandlinePhone,
                    Fax = user.Fax,
                    BestTimeToCall = user.BestTimeToCall,
                    Address = user.Address,
                    Email = user.Email,
                    RepresentativeName = user.RepresentativeName,
                    RepresentativeNationalId = user.RepresentativeNationalId,
                    RepresentativeMobilePhone = user.RepresentativeMobilePhone,
                    IsComplete = user.IsComplete,
                    IsDeleted = user.IsDeleted
                };

                return Ok(userDto);
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
        public IActionResult UpdateHoghooghiUserAdmin(int requestId, [FromBody] HoghooghiUserDto userDto)
        {
            try
            {
                var user = _context.HoghooghiUsers.FirstOrDefault(u => u.RequestId == requestId && !u.IsDeleted);
                if (user == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiUserNotFound });
                }

                user.Name = userDto.Name;
                user.RegistrationNumber = userDto.RegistrationNumber;
                user.RegistrationDate = userDto.RegistrationDate;
                user.RegistrationLocation = userDto.RegistrationLocation;
                user.NationalId = userDto.NationalId;
                user.MainActivityBasedOnCharter = userDto.MainActivityBasedOnCharter;
                user.MainActivityBasedOnPastThreeYearsPerformance = userDto.MainActivityBasedOnPastThreeYearsPerformance;
                user.PostalCode = userDto.PostalCode;
                user.LandlinePhone = userDto.LandlinePhone;
                user.Fax = userDto.Fax;
                user.BestTimeToCall = userDto.BestTimeToCall;
                user.Address = userDto.Address;
                user.Email = userDto.Email;
                user.RepresentativeName = userDto.RepresentativeName;
                user.RepresentativeNationalId = userDto.RepresentativeNationalId;
                user.RepresentativeMobilePhone = userDto.RepresentativeMobilePhone;


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
        public IActionResult DeleteHoghooghiUserAdmin(int requestId)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var user = _context.HoghooghiUsers.FirstOrDefault(u => u.RequestId == requestId && !u.IsDeleted);
                if (user == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiUserNotFound });
                }

                user.IsDeleted = true;
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
        public IActionResult ClearHoghooghiUsers()
        {
            try
            {
                _context.HoghooghiUsers.RemoveRange(_context.HoghooghiUsers);
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
