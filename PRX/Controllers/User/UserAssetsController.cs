using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.User;
using PRX.Models.User;

namespace PRX.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "UserAssets")]
    public class UserAssetsController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public UserAssetsController(PRXDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAllUserAssets()
        {
            var userAssets = _context.UserAssets.ToList();
            var userAssetDtos = userAssets.Select(userAsset => new UserAssetDto
            {
                UserId = userAsset.UserId,
                AssetTypeId = userAsset.AssetTypeId,
                AssetValue = userAsset.AssetValue,
                AssetPercentage = userAsset.AssetPercentage,
                IsComplete = userAsset.IsComplete
            }).ToList();
            return Ok(userAssetDtos);
        }

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserAssetById(int id)
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
                var userAsset = _context.UserAssets.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userAsset == null)
                {
                    return NotFound();
                }
                var userAssetDto = new UserAssetDto
                {

                    UserId = userAsset.UserId,
                    AssetTypeId = userAsset.AssetTypeId,
                    AssetValue = userAsset.AssetValue,
                    AssetPercentage = userAsset.AssetPercentage,
                    IsComplete = userAsset.IsComplete
                };
                return Ok(userAssetDto);

            }

            catch (Exception ex)
            {

                return BadRequest();
            }


            
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateUserAsset([FromBody] UserAssetDto userAssetDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userAsset = new UserAsset
            {
                UserId = userAssetDto.UserId,
                AssetTypeId = userAssetDto.AssetTypeId,
                AssetValue = userAssetDto.AssetValue,
                AssetPercentage = userAssetDto.AssetPercentage
            };

            _context.UserAssets.Add(userAsset);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUserAssetById), new { id = userAsset.Id }, userAsset);
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserAsset(int id, [FromBody] UserAssetDto userAssetDto)
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

                var userAsset = _context.UserAssets.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userAsset == null)
                {
                    return NotFound();
                }

                userAsset.UserId = userAssetDto.UserId;
                userAsset.AssetTypeId = userAssetDto.AssetTypeId;
                userAsset.AssetValue = userAssetDto.AssetValue;
                userAsset.AssetPercentage = userAssetDto.AssetPercentage;
                userAsset.IsComplete = userAssetDto.IsComplete;

                _context.SaveChanges();

                return NoContent();

            }

            catch (Exception ex)
            {

                return BadRequest();
            }

        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserAsset(int id)
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

                var userAsset = _context.UserAssets.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userAsset == null)
                {
                    return NotFound();
                }

                userAsset.IsDeleted = true;
                _context.SaveChanges();

                return Ok();

            }

            catch (Exception ex)
            {

                return BadRequest();
            }


            
        }

        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearUserAssets()
        {
            _context.UserAssets.RemoveRange(_context.UserAssets);
            _context.SaveChanges();

            return Ok();
        }


        [HttpPut("complete/{id}")]
        //[Authorize(Roles = "Admin")] // Assuming only admins can mark profiles as complete
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkCompaniesAsComplete(int id)
        {
            var record = _context.UserAssets.FirstOrDefault(e => e.UserId == id);
            if (record == null)
            {
                return NotFound();
            }

            record.IsComplete = true;
            _context.SaveChanges();

            return Ok();
        }
    }
}
