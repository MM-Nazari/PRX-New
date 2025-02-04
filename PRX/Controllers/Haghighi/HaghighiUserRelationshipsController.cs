﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.Haghighi;
using PRX.Models.Haghighi;
using PRX.Utils;

namespace PRX.Controllers.Haghighi
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "HaghighiUserRelationships")]
    public class HaghighiUserRelationshipsController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public HaghighiUserRelationshipsController(PRXDbContext context)
        {
            _context = context;
        }

        // GET: api/HaghighiUserRelationships
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllHaghighiUserRelationships()
        {
            try
            {
                var relationships = _context.HaghighiUserRelationships.ToList();
                return Ok(relationships);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        // GET: api/HaghighiUserRelationships/5
        [HttpGet("{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHaghighiUserRelationshipsById(int requestId)
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

                // Fetch all relationships associated with the request
                //var relationships = _context.HaghighiUserRelationships
                //    .Where(u => u.RequestId == requestId && !u.IsDeleted)
                //    .ToList();

                // Fetch all relationships associated with the request
                var relationships = _context.HaghighiUserRelationships
                    .Where(u => u.RequestId == requestId && !u.IsDeleted)
                    .Select(r => new HaghighiUserRelationshipsDto
                    {
                        Id = r.Id,
                        RequestId = r.RequestId,
                        FullName = r.FullName,
                        RelationshipStatus = r.RelationshipStatus,
                        BirthYear = r.BirthYear,
                        EducationLevel = r.EducationLevel,
                        EmploymentStatus = r.EmploymentStatus,
                        AverageMonthlyIncome = r.AverageMonthlyIncome,
                        AverageMonthlyExpense = r.AverageMonthlyExpense,
                        ApproximateAssets = r.ApproximateAssets,
                        ApproximateLiabilities = r.ApproximateLiabilities,
                        IsComplete = r.IsComplete,
                        IsDeleted = r.IsDeleted
                    })
                    .ToList();

                if (relationships == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserRelationNotFound});
                }
                return Ok(relationships);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        // POST: api/HaghighiUserRelationships
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateHaghighiUserRelationships([FromBody] HaghighiUserRelationshipsListDto relationshipDto)
        {
            try
            {
                if (relationshipDto.RequestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                foreach (var rel in relationshipDto.Relations) 
                {
                    var relationship = new HaghighiUserRelationships
                    {
                        RequestId = relationshipDto.RequestId,
                        FullName = rel.FullName,
                        RelationshipStatus = rel.RelationshipStatus,
                        BirthYear = rel.BirthYear,
                        EducationLevel = rel.EducationLevel,
                        EmploymentStatus = rel.EmploymentStatus,
                        AverageMonthlyIncome = rel.AverageMonthlyIncome,
                        AverageMonthlyExpense = rel.AverageMonthlyExpense,
                        ApproximateAssets = rel.ApproximateAssets,
                        ApproximateLiabilities = rel.ApproximateLiabilities
                    };
                    _context.HaghighiUserRelationships.Add(relationship);
                }


                
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status201Created, new { message = ResponseMessages.OK });
                //return CreatedAtAction(nameof(GetHaghighiUserRelationshipsById), new { requestId = relationship.Id }, relationship);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        // PUT: api/HaghighiUserRelationships/5
        [HttpPut("{id}/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHaghighiUserRelationships(int id, int requestId, [FromBody] HaghighiUserRelationshipsDto relationshipDto)
        {

            try
            {
                if (id <= 0 || requestId <= 0)
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

                var relationship = _context.HaghighiUserRelationships.FirstOrDefault(u => u.RequestId == requestId && u.Id == id && !u.IsDeleted);
                if (relationship == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserRelationNotFound });
                }

                relationship.RequestId = relationshipDto.RequestId;
                relationship.FullName = relationshipDto.FullName;
                relationship.RelationshipStatus = relationshipDto.RelationshipStatus;
                relationship.BirthYear = relationshipDto.BirthYear;
                relationship.EducationLevel = relationshipDto.EducationLevel;
                relationship.EmploymentStatus = relationshipDto.EmploymentStatus;
                relationship.AverageMonthlyIncome = relationshipDto.AverageMonthlyIncome;
                relationship.AverageMonthlyExpense = relationshipDto.AverageMonthlyExpense;
                relationship.ApproximateAssets = relationshipDto.ApproximateAssets;
                relationship.ApproximateLiabilities = relationshipDto.ApproximateLiabilities;

                _context.SaveChanges();

                return Ok(relationship);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        // PATCH: api/HaghighiUserRelationships/5
        [HttpPatch("{id}/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult PatchHaghighiUserRelationships(int id, int requestId, [FromBody] JsonPatchDocument<HaghighiUserRelationshipsDto> patchDoc)
        {
            try
            {
                if (id <= 0 || requestId <= 0)
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

                var relationship = _context.HaghighiUserRelationships.FirstOrDefault(u => u.RequestId == requestId && u.Id == id && !u.IsDeleted);
                if (relationship == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserRelationNotFound });
                }

                // Convert the entity to a DTO object for patching
                var relationshipDto = new HaghighiUserRelationshipsDto
                {
                    RequestId = relationship.RequestId,
                    FullName = relationship.FullName,
                    RelationshipStatus = relationship.RelationshipStatus,
                    BirthYear = relationship.BirthYear,
                    EducationLevel = relationship.EducationLevel,
                    EmploymentStatus = relationship.EmploymentStatus,
                    AverageMonthlyIncome = relationship.AverageMonthlyIncome,
                    AverageMonthlyExpense = relationship.AverageMonthlyExpense,
                    ApproximateAssets = relationship.ApproximateAssets,
                    ApproximateLiabilities = relationship.ApproximateLiabilities
                };

                // Apply the patch to the DTO
                patchDoc.ApplyTo(relationshipDto, ModelState);

                // Check for validation errors after patch is applied
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Update the original entity with the patched values
                relationship.RequestId = relationshipDto.RequestId;
                relationship.FullName = relationshipDto.FullName;
                relationship.RelationshipStatus = relationshipDto.RelationshipStatus;
                relationship.BirthYear = relationshipDto.BirthYear;
                relationship.EducationLevel = relationshipDto.EducationLevel;
                relationship.EmploymentStatus = relationshipDto.EmploymentStatus;
                relationship.AverageMonthlyIncome = relationshipDto.AverageMonthlyIncome;
                relationship.AverageMonthlyExpense = relationshipDto.AverageMonthlyExpense;
                relationship.ApproximateAssets = relationshipDto.ApproximateAssets;
                relationship.ApproximateLiabilities = relationshipDto.ApproximateLiabilities;

                // Save changes to the database
                _context.SaveChanges();

                return Ok(relationship);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }
        }


        // DELETE: api/HaghighiUserRelationships/5
        [HttpDelete("{id}/{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHaghighiUserRelationships(int id, int requestId)
        {
            try
            {
                if (id <= 0 || requestId <= 0)
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

                var relationship = _context.HaghighiUserRelationships.FirstOrDefault(u => u.RequestId == requestId && u.Id == id && !u.IsDeleted );
                if (relationship == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserRelationNotFound });
                }

                relationship.IsDeleted = true;
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }

        }

        // DELETE: api/HaghighiUserRelationships/clear
        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearAllHaghighiUserRelationships()
        {
            try
            {
                _context.HaghighiUserRelationships.RemoveRange(_context.HaghighiUserRelationships);
                _context.SaveChanges();

                return Ok(new { message = ResponseMessages.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }


        
        [HttpPut("complete/{id}/{requestId}")]
        //[Authorize(Roles = "Admin")] // Assuming only admins can mark profiles as complete
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkRelationshipAsComplete(int id, int requestId)
        {
            try
            {

                if (id <= 0 || requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var relationship = _context.HaghighiUserRelationships.FirstOrDefault(e => e.RequestId == requestId && e.Id == id);
                if (relationship == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserRelationNotFound });
                }

                relationship.IsComplete = true;
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

                var record = _context.HaghighiUserRelationships.FirstOrDefault(u => u.RequestId == requestId && u.Id == id && !u.IsDeleted);
                if (record == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserRelationNotFound });
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
        public IActionResult GetAllHaghighiUserRelationshipsAdmin()
        {
            try
            {
                var relationships = _context.HaghighiUserRelationships.ToList();
                var relationshipDtos = relationships.Select(relationship => new HaghighiUserRelationshipsDto
                {
                    Id = relationship.Id,
                    RequestId = relationship.RequestId,
                    FullName = relationship.FullName,
                    RelationshipStatus = relationship.RelationshipStatus,
                    BirthYear = relationship.BirthYear,
                    EducationLevel = relationship.EducationLevel,
                    EmploymentStatus = relationship.EmploymentStatus,
                    AverageMonthlyIncome = relationship.AverageMonthlyIncome,
                    AverageMonthlyExpense = relationship.AverageMonthlyExpense,
                    ApproximateAssets = relationship.ApproximateAssets,
                    ApproximateLiabilities = relationship.ApproximateLiabilities,
                    IsComplete = relationship.IsComplete,
                    IsDeleted = relationship.IsDeleted
                }).ToList();
                return Ok(relationshipDtos);
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
        public IActionResult GetHaghighiUserRelationshipByIdAdmin(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var relationship = _context.HaghighiUserRelationships.FirstOrDefault(r => r.Id == id && !r.IsDeleted);
                if (relationship == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserRelationNotFound });
                }

                var relationshipDto = new HaghighiUserRelationshipsDto
                {
                    Id = relationship.Id,
                    RequestId = relationship.RequestId,
                    FullName = relationship.FullName,
                    RelationshipStatus = relationship.RelationshipStatus,
                    BirthYear = relationship.BirthYear,
                    EducationLevel = relationship.EducationLevel,
                    EmploymentStatus = relationship.EmploymentStatus,
                    AverageMonthlyIncome = relationship.AverageMonthlyIncome,
                    AverageMonthlyExpense = relationship.AverageMonthlyExpense,
                    ApproximateAssets = relationship.ApproximateAssets,
                    ApproximateLiabilities = relationship.ApproximateLiabilities,
                    IsComplete = relationship.IsComplete,
                    IsDeleted = relationship.IsDeleted
                };

                return Ok(relationshipDto);
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
        public IActionResult UpdateHaghighiUserRelationshipAdmin(int id, [FromBody] HaghighiUserRelationshipsDto relationshipDto)
        {

            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var relationship = _context.HaghighiUserRelationships.FirstOrDefault(r => r.Id == id && !r.IsDeleted);
                if (relationship == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserRelationNotFound });
                }

                relationship.FullName = relationshipDto.FullName;
                relationship.RelationshipStatus = relationshipDto.RelationshipStatus;
                relationship.BirthYear = relationshipDto.BirthYear;
                relationship.EducationLevel = relationshipDto.EducationLevel;
                relationship.EmploymentStatus = relationshipDto.EmploymentStatus;
                relationship.AverageMonthlyIncome = relationshipDto.AverageMonthlyIncome;
                relationship.AverageMonthlyExpense = relationshipDto.AverageMonthlyExpense;
                relationship.ApproximateAssets = relationshipDto.ApproximateAssets;
                relationship.ApproximateLiabilities = relationshipDto.ApproximateLiabilities;
                //relationship.IsComplete = relationshipDto.IsComplete;
                //relationship.IsDeleted = relationshipDto.IsDeleted;

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
        public IActionResult DeleteHaghighiUserRelationshipAdmin(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var relationship = _context.HaghighiUserRelationships.FirstOrDefault(r => r.Id == id && !r.IsDeleted);
                if (relationship == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserRelationNotFound });
                }

                relationship.IsDeleted = true;
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
        public IActionResult ClearHaghighiUserRelationships()
        {
            try
            {
                _context.HaghighiUserRelationships.RemoveRange(_context.HaghighiUserRelationships);
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
