using Microsoft.AspNetCore.Http;
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllUserAssetTypes()
        {
            try
            {
                var userAssetTypes = _context.UserAssetTypes.ToList();
                var userAssetTypeDtos = userAssetTypes.Select(userAssetType => new UserAssetTypeDto
                {
                    Id = userAssetType.Id,
                    Name = userAssetType.Name
                }).ToList();
                return Ok(userAssetTypeDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserAssetTypeById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userAssetType = _context.UserAssetTypes.FirstOrDefault(u => u.Id == id);
                if (userAssetType == null)
                {
                    return NotFound(new { message = ResponseMessages.UserAssetTypeNotFound });
                }
                var userAssetTypeDto = new UserAssetTypeDto
                {
                    Id = userAssetType.Id,
                    Name = userAssetType.Name
                };
                return Ok(userAssetTypeDto);
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
        public IActionResult CreateUserAssetType([FromBody] UserAssetTypeDto userAssetTypeDto)
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateUserAssetType(int id, [FromBody] UserAssetTypeDto userAssetTypeDto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userAssetType = _context.UserAssetTypes.FirstOrDefault(u => u.Id == id);
                if (userAssetType == null)
                {
                    return NotFound(new { message = ResponseMessages.UserAssetTypeNotFound });
                }

                userAssetType.Name = userAssetTypeDto.Name;

                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        // PATCH: api/UserAssetType/{id}
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult PatchUserAssetType(int id, [FromBody] JsonPatchDocument<UserAssetTypeDto> patchDoc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userAssetType = _context.UserAssetTypes.FirstOrDefault(u => u.Id == id);
                if (userAssetType == null)
                {
                    return NotFound(new { message = ResponseMessages.UserAssetTypeNotFound });
                }

                // Create a DTO to hold the current values
                var userAssetTypeDto = new UserAssetTypeDto
                {
                    Name = userAssetType.Name
                };

                // Apply the patch document to the DTO
                patchDoc.ApplyTo(userAssetTypeDto, ModelState);

                // Validate the model after applying the patch
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Update the user asset type properties based on the modified DTO
                userAssetType.Name = userAssetTypeDto.Name;

                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteUserAssetType(int id)
        {
            try
            {

                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var userAssetType = _context.UserAssetTypes.FirstOrDefault(u => u.Id == id);
                if (userAssetType == null)
                {
                    return NotFound(new { message = ResponseMessages.UserAssetTypeNotFound });
                }

                _context.UserAssetTypes.Remove(userAssetType);
                _context.SaveChanges();

                return Ok(new {message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }

        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ClearUserAssetTypes()
        {
            try
            {
                _context.UserAssetTypes.RemoveRange(_context.UserAssetTypes);
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

