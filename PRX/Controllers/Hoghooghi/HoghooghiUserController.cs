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
        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHoghooghiUserById(int id)
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
                var user = _context.HoghooghiUsers.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
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

                var user = new HoghooghiUser
                {
                    UserId = userDto.UserId,
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

                return CreatedAtAction(nameof(GetHoghooghiUserById), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // PUT: api/HoghooghiUser/5
        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHoghooghiUser(int id, [FromBody] HoghooghiUserDto userDto)
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

                var user = _context.HoghooghiUsers.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
                if (user == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiUserNotFound });
                }

                user.UserId = userDto.UserId;
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

        // DELETE: api/HoghooghiUser/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHoghooghiUser(int id)
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
                var user = _context.HoghooghiUsers.FirstOrDefault(e => e.UserId == id && !e.IsDeleted);
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

                var user = _context.HoghooghiUsers.FirstOrDefault(e => e.UserId == id);
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


                var record = _context.HoghooghiUsers.FirstOrDefault(e => e.UserId == id);
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
                    UserId = user.UserId,
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

        [HttpGet("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHoghooghiUserByIdAdmin(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var user = _context.HoghooghiUsers.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (user == null)
                {
                    return NotFound(new { message = ResponseMessages.HoghooghiUserNotFound });
                }

                var userDto = new HoghooghiUserDto
                {
                    UserId = user.UserId,
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

        [HttpPut("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHoghooghiUserAdmin(int id, [FromBody] HoghooghiUserDto userDto)
        {
            try
            {
                var user = _context.HoghooghiUsers.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
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

        [HttpDelete("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHoghooghiUserAdmin(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var user = _context.HoghooghiUsers.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
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
