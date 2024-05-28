using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.User;
using PRX.Models.User;
using PRX.Utils;
using System;
using System.Linq;

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

        //[HttpGet("{id}")]
        //[Authorize(Roles = "User")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public IActionResult GetUserAssetById(int id)
        //{
        //    try
        //    {
        //        if (id <= 0)
        //        {
        //            return BadRequest(new { message = ResponseMessages.InvalidId });
        //        }

        //        var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

        //        if (id != tokenUserId)
        //        {
        //            return Unauthorized(new { message = ResponseMessages.Unauthorized });
        //        }

        //        var userAsset = _context.UserAssets.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
        //        if (userAsset == null)
        //        {
        //            return NotFound(new { message = ResponseMessages.UserAssetNotFound });
        //        }

        //        var userAssetDto = new UserAssetDto
        //        {
        //            UserId = userAsset.UserId,
        //            AssetTypeId = userAsset.AssetTypeId,
        //            AssetValue = userAsset.AssetValue,
        //            AssetPercentage = userAsset.AssetPercentage,
        //            IsComplete = userAsset.IsComplete
        //        };
        //        return Ok(userAssetDto);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
        //    }
        //}

        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserAssetById(int id)
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

                var userAssets = _context.UserAssets.Where(u => u.UserId == id && !u.IsDeleted).ToList();
                if (!userAssets.Any())
                {
                    return NotFound(new { message = ResponseMessages.UserAssetNotFound });
                }

                var userAssetDtos = userAssets.Select(userAsset => new UserAssetDto
                {
                    UserId = userAsset.UserId,
                    AssetTypeId = userAsset.AssetTypeId,
                    AssetValue = userAsset.AssetValue,
                    AssetPercentage = userAsset.AssetPercentage,
                    IsComplete = userAsset.IsComplete,
                    IsDeleted = userAsset.IsDeleted
                }).ToList();

                return Ok(userAssetDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }



        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public IActionResult CreateUserAsset([FromBody] UserAssetDto userAssetDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    try
        //    {
        //        var userAsset = new UserAsset
        //        {
        //            UserId = userAssetDto.UserId,
        //            AssetTypeId = userAssetDto.AssetTypeId,
        //            AssetValue = userAssetDto.AssetValue,
        //            AssetPercentage = userAssetDto.AssetPercentage
        //        };

        //        _context.UserAssets.Add(userAsset);
        //        _context.SaveChanges();

        //        return CreatedAtAction(nameof(GetUserAssetById), new { id = userAsset.Id }, userAsset);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
        //    }
        //}


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateUserAsset([FromBody] UserAssetDto userAssetDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userAsset = new UserAsset
                {
                    UserId = userAssetDto.UserId,
                    AssetTypeId = userAssetDto.AssetTypeId,
                    AssetValue = userAssetDto.AssetValue
                };

                _context.UserAssets.Add(userAsset);
                _context.SaveChanges();

                // Recalculate the percentages
                CalculateAndSetAssetPercentages(userAssetDto.UserId);

                return CreatedAtAction(nameof(GetUserAssetById), new { id = userAsset.Id }, userAsset);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }


        //[HttpPut("{id}")]
        //[Authorize(Roles = "User")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public IActionResult UpdateUserAsset(int id, [FromBody] UserAssetDto userAssetDto)
        //{
        //    try
        //    {
        //        if (id <= 0)
        //        {
        //            return BadRequest(new { message = ResponseMessages.InvalidId });
        //        }

        //        var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

        //        if (id != tokenUserId)
        //        {
        //            return Unauthorized(new { message = ResponseMessages.Unauthorized });
        //        }

        //        var userAsset = _context.UserAssets.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
        //        if (userAsset == null)
        //        {
        //            return NotFound(new { message = ResponseMessages.UserAssetNotFound });
        //        }

        //        userAsset.UserId = userAssetDto.UserId;
        //        userAsset.AssetTypeId = userAssetDto.AssetTypeId;
        //        userAsset.AssetValue = userAssetDto.AssetValue;
        //        userAsset.AssetPercentage = userAssetDto.AssetPercentage;

        //        _context.SaveChanges();

        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
        //    }
        //}

        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateUserAsset(int id, [FromBody] UserAssetDto userAssetDto)
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

                var userAsset = _context.UserAssets.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userAsset == null)
                {
                    return NotFound(new { message = ResponseMessages.UserAssetNotFound });
                }

                userAsset.UserId = userAssetDto.UserId;
                userAsset.AssetTypeId = userAssetDto.AssetTypeId;
                userAsset.AssetValue = userAssetDto.AssetValue;

                _context.SaveChanges();

                // Recalculate the percentages
                CalculateAndSetAssetPercentages(userAssetDto.UserId);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }



        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteUserAsset(int id)
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

                var userAsset = _context.UserAssets.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (userAsset == null)
                {
                    return NotFound(new { message = ResponseMessages.UserAssetNotFound });
                }

                userAsset.IsDeleted = true;
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        [HttpPut("complete/{id}")]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult MarkCompaniesAsComplete(int id)
        {
            try
            {
                var record = _context.UserAssets.FirstOrDefault(e => e.UserId == id);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.UserAssetNotFound });
                }

                record.IsComplete = true;
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


                var record = _context.UserAssets.FirstOrDefault(e => e.UserId == id);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.UserAssetNotFound });
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllUserAssetsAdmin()
        {
            try
            {
                var userAssets = _context.UserAssets.ToList();
                var userAssetDtos = userAssets.Select(asset => new UserAssetDto
                {
                    UserId = asset.UserId,
                    AssetTypeId = asset.AssetTypeId,
                    AssetValue = asset.AssetValue,
                    AssetPercentage = asset.AssetPercentage,
                    IsComplete = asset.IsComplete,
                    IsDeleted = asset.IsDeleted
                }).ToList();

                return Ok(userAssetDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        [HttpGet("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserAssetByIdAdmin(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userAsset = _context.UserAssets.FirstOrDefault(asset => asset.Id == id && !asset.IsDeleted);
                if (userAsset == null)
                {
                    return NotFound(new { message = ResponseMessages.UserAssetNotFound });
                }

                var userAssetDto = new UserAssetDto
                {
                    UserId = userAsset.UserId,
                    AssetTypeId = userAsset.AssetTypeId,
                    AssetValue = userAsset.AssetValue,
                    AssetPercentage = userAsset.AssetPercentage,
                    IsComplete = userAsset.IsComplete,
                    IsDeleted = userAsset.IsDeleted
                };

                return Ok(userAssetDto);
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateUserAssetAdmin(int id, [FromBody] UserAssetDto userAssetDto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userAsset = _context.UserAssets.FirstOrDefault(asset => asset.Id == id && !asset.IsDeleted);
                if (userAsset == null)
                {
                    return NotFound(new { message = ResponseMessages.UserAssetNotFound });
                }

                userAsset.UserId = userAssetDto.UserId;
                userAsset.AssetTypeId = userAssetDto.AssetTypeId;
                userAsset.AssetValue = userAssetDto.AssetValue;
                userAsset.AssetPercentage = userAssetDto.AssetPercentage;
                userAsset.IsComplete = userAssetDto.IsComplete;
                userAsset.IsDeleted = userAssetDto.IsDeleted;

                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        private void CalculateAndSetAssetPercentages(int userId)
        {
            var userAssets = _context.UserAssets.Where(u => u.UserId == userId && !u.IsDeleted).ToList();
            var totalAssetValue = userAssets.Sum(a => a.AssetValue);

            foreach (var asset in userAssets)
            {
                asset.AssetPercentage = totalAssetValue > 0 ? (asset.AssetValue / totalAssetValue) * 100 : 0;
            }

            _context.SaveChanges();
        }

    }
}

