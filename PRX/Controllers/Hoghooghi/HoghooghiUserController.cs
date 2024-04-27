using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.Hoghooghi;
using PRX.Models.Hoghooghi;

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
            var users = _context.HoghooghiUsers.ToList();
            return Ok(users);
        }

        // GET: api/HoghooghiUser/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHoghooghiUserById(int id)
        {
            var user = _context.HoghooghiUsers.FirstOrDefault(e => e.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST: api/HoghooghiUser
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateHoghooghiUser([FromBody] HoghooghiUserDto userDto)
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

        // PUT: api/HoghooghiUser/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHoghooghiUser(int id, [FromBody] HoghooghiUserDto userDto)
        {
            var user = _context.HoghooghiUsers.FirstOrDefault(e => e.Id == id);
            if (user == null)
            {
                return NotFound();
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

        // DELETE: api/HoghooghiUser/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHoghooghiUser(int id)
        {
            var user = _context.HoghooghiUsers.FirstOrDefault(e => e.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            _context.HoghooghiUsers.Remove(user);
            _context.SaveChanges();

            return Ok();
        }

        // DELETE: api/HoghooghiUser
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearAllHoghooghiUsers()
        {
            _context.HoghooghiUsers.RemoveRange(_context.HoghooghiUsers);
            _context.SaveChanges();

            return Ok();
        }
    }
}
