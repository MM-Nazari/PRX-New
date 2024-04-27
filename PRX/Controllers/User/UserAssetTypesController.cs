using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.User;
using PRX.Models.User;

namespace PRX.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "UserAssetTypes")]
    public class UserAssetTypesController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public UserAssetTypesController(PRXDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllUserAssetTypes()
        {
            var userAssetTypes = _context.UserAssetTypes.ToList();
            var userAssetTypeDtos = userAssetTypes.Select(userAssetType => new UserAssetTypeDto
            {
                Id = userAssetType.Id,
                Name = userAssetType.Name
            }).ToList();
            return Ok(userAssetTypeDtos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserAssetTypeById(int id)
        {
            var userAssetType = _context.UserAssetTypes.FirstOrDefault(u => u.Id == id);
            if (userAssetType == null)
            {
                return NotFound();
            }
            var userAssetTypeDto = new UserAssetTypeDto
            {
                Id = userAssetType.Id,
                Name = userAssetType.Name
            };
            return Ok(userAssetTypeDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateUserAssetType([FromBody] UserAssetTypeDto userAssetTypeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userAssetType = new UserAssetType
            {
                Name = userAssetTypeDto.Name
            };

            _context.UserAssetTypes.Add(userAssetType);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUserAssetTypeById), new { id = userAssetType.Id }, userAssetType);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserAssetType(int id, [FromBody] UserAssetTypeDto userAssetTypeDto)
        {
            var userAssetType = _context.UserAssetTypes.FirstOrDefault(u => u.Id == id);
            if (userAssetType == null)
            {
                return NotFound();
            }

            userAssetType.Name = userAssetTypeDto.Name;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserAssetType(int id)
        {
            var userAssetType = _context.UserAssetTypes.FirstOrDefault(u => u.Id == id);
            if (userAssetType == null)
            {
                return NotFound();
            }

            _context.UserAssetTypes.Remove(userAssetType);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearUserAssetTypes()
        {
            _context.UserAssetTypes.RemoveRange(_context.UserAssetTypes);
            _context.SaveChanges();

            return Ok();
        }
    }
}
