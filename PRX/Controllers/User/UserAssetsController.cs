﻿using Microsoft.AspNetCore.Mvc;
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
                Id = userAsset.Id,
                UserId = userAsset.UserId,
                AssetTypeId = userAsset.AssetTypeId,
                AssetValue = userAsset.AssetValue,
                AssetPercentage = userAsset.AssetPercentage,
                IsComplete = userAsset.IsComplete
            }).ToList();
            return Ok(userAssetDtos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserAssetById(int id)
        {
            var userAsset = _context.UserAssets.FirstOrDefault(u => u.Id == id);
            if (userAsset == null)
            {
                return NotFound();
            }
            var userAssetDto = new UserAssetDto
            {
                Id = userAsset.Id,
                UserId = userAsset.UserId,
                AssetTypeId = userAsset.AssetTypeId,
                AssetValue = userAsset.AssetValue,
                AssetPercentage = userAsset.AssetPercentage,
                IsComplete = userAsset.IsComplete
            };
            return Ok(userAssetDto);
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
                AssetPercentage = userAssetDto.AssetPercentage,
                IsComplete = userAssetDto.IsComplete
            };

            _context.UserAssets.Add(userAsset);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUserAssetById), new { id = userAsset.Id }, userAsset);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateUserAsset(int id, [FromBody] UserAssetDto userAssetDto)
        {
            var userAsset = _context.UserAssets.FirstOrDefault(u => u.Id == id);
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

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUserAsset(int id)
        {
            var userAsset = _context.UserAssets.FirstOrDefault(u => u.Id == id);
            if (userAsset == null)
            {
                return NotFound();
            }

            _context.UserAssets.Remove(userAsset);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearUserAssets()
        {
            _context.UserAssets.RemoveRange(_context.UserAssets);
            _context.SaveChanges();

            return Ok();
        }
    }
}
