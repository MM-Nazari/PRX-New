﻿using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpGet("{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserAssetById(int requestId)
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

                //var userAssets = _context.UserAssets.Where(u => u.RequestId == requestId && !u.IsDeleted).ToList();
                //if (!userAssets.Any())
                //{
                //    return NotFound(new { message = ResponseMessages.UserAssetNotFound });
                //}

                //var userAssetDtos = userAssets.Select(userAsset => new UserAssetDto
                var userAssetDtos = _context.UserAssets.Where(u => u.RequestId == requestId && !u.IsDeleted).Select(userAsset => new UserAssetDto
                {
                    Id = userAsset.Id,
                    RequestId = userAsset.RequestId,
                    AssetTypeId = userAsset.AssetTypeId,
                    AssetValue = userAsset.AssetValue,
                    AssetPercentage = userAsset.AssetPercentage,
                    IsComplete = userAsset.IsComplete,
                    IsDeleted = userAsset.IsDeleted
                }).ToList();
                if ( userAssetDtos == null) 
                {
                    return NotFound(new { message = ResponseMessages.UserAssetNotFound });
                }

                return Ok(userAssetDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateUserAsset([FromBody] UserAssetListDto userAssetDto)
        {

            try
            {

                if (userAssetDto.RequestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                foreach ( var asset in  userAssetDto.Assets ) 
                {
                    var userAsset = new UserAsset
                    {
                        RequestId = userAssetDto.RequestId,
                        AssetTypeId = asset.AssetTypeId,
                        AssetValue = asset.AssetValue
                    };
                    _context.UserAssets.Add(userAsset);
                }



                _context.SaveChanges();

                // Recalculate the percentages
                CalculateAndSetAssetPercentages(userAssetDto.RequestId);

                return StatusCode(StatusCodes.Status201Created, new { message = ResponseMessages.OK });
                //return CreatedAtAction(nameof(GetUserAssetById), new { requestId = userAsset.Id }, userAsset);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }


        [HttpPut("{id}/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateUserAsset(int id, int requestId, [FromBody] UserAssetDto userAssetDto)
        {
            try
            {
                if (id <= 0 || requestId <= 0)
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

                var userAsset = _context.UserAssets.FirstOrDefault(u => u.RequestId == requestId && u.Id == id && !u.IsDeleted);
                if (userAsset == null)
                {
                    return NotFound(new { message = ResponseMessages.UserAssetNotFound });
                }

               
                userAsset.AssetTypeId = userAssetDto.AssetTypeId;
                userAsset.AssetValue = userAssetDto.AssetValue;

                _context.SaveChanges();

                // Recalculate the percentages
                CalculateAndSetAssetPercentages(userAssetDto.RequestId);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        // PATCH: api/UserAsset/{id}/{requestId}
        [HttpPatch("{id}/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult PatchUserAsset(int id, int requestId, [FromBody] JsonPatchDocument<UserAssetDto> patchDoc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (id <= 0 || requestId <= 0)
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

                var userAsset = _context.UserAssets.FirstOrDefault(u => u.RequestId == requestId && u.Id == id && !u.IsDeleted);
                if (userAsset == null)
                {
                    return NotFound(new { message = ResponseMessages.UserAssetNotFound });
                }

                // Create a DTO to hold the current values
                var userAssetDto = new UserAssetDto
                {
                    AssetTypeId = userAsset.AssetTypeId,
                    AssetValue = userAsset.AssetValue,
                    RequestId = userAsset.RequestId // Ensure we pass the RequestId for further calculations if needed
                };

                // Apply the patch document to the DTO
                patchDoc.ApplyTo(userAssetDto, ModelState);

                // Validate the model after applying the patch
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Update the user asset properties based on the modified DTO
                userAsset.AssetTypeId = userAssetDto.AssetTypeId;
                userAsset.AssetValue = userAssetDto.AssetValue;

                _context.SaveChanges();

                // Recalculate the percentages
                CalculateAndSetAssetPercentages(userAssetDto.RequestId);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }



        [HttpDelete("{id}/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteUserAsset(int id, int requestId)
        {
            try
            {
                if (id <= 0 || requestId <= 0)
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

                var userAsset = _context.UserAssets.FirstOrDefault(u => u.RequestId == requestId && u.Id == id && !u.IsDeleted);
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

        [HttpPut("complete/{id}/{requestId}")]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult MarkCompaniesAsComplete(int id, int requestId)
        {
            try
            {
                var record = _context.UserAssets.FirstOrDefault(e => e.RequestId == requestId && e.Id == id && !e.IsDeleted);
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

        [HttpGet("isComplete/{id}/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CheckCompletionStatus(int id, int requestId)
        {
            try
            {
                if (id <= 0 || requestId <= 0)
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


                var record = _context.UserAssets.FirstOrDefault(e => e.RequestId == requestId && e.Id == id && !e.IsDeleted);
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
                    Id = asset.Id,
                    RequestId = asset.RequestId,
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
                    RequestId = userAsset.RequestId,
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

                userAsset.RequestId = userAssetDto.RequestId;
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
            var userAssets = _context.UserAssets.Where(u => u.RequestId == userId && !u.IsDeleted).ToList();
            var totalAssetValue = userAssets.Sum(a => a.AssetValue);

            foreach (var asset in userAssets)
            {
                asset.AssetPercentage = totalAssetValue > 0 ? (asset.AssetValue / totalAssetValue) * 100 : 0;
            }

            _context.SaveChanges();
        }

    }
}

