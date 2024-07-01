using Microsoft.AspNetCore.Authorization;
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
        public IActionResult CreateHaghighiUserRelationships([FromBody] HaghighiUserRelationshipsDto relationshipDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var relationship = new HaghighiUserRelationships
                {
                    RequestId = relationshipDto.RequestId,
                    FullName = relationshipDto.FullName,
                    RelationshipStatus = relationshipDto.RelationshipStatus,
                    BirthYear = relationshipDto.BirthYear,
                    EducationLevel = relationshipDto.EducationLevel,
                    EmploymentStatus = relationshipDto.EmploymentStatus,
                    AverageMonthlyIncome = relationshipDto.AverageMonthlyIncome,
                    AverageMonthlyExpense = relationshipDto.AverageMonthlyExpense,
                    ApproximateAssets = relationshipDto.ApproximateAssets,
                    ApproximateLiabilities = relationshipDto.ApproximateLiabilities
                };

                _context.HaghighiUserRelationships.Add(relationship);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetHaghighiUserRelationshipsById), new { requestId = relationship.Id }, relationship);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        // PUT: api/HaghighiUserRelationships/5
        [HttpPut("{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHaghighiUserRelationships(int requestId, [FromBody] HaghighiUserRelationshipsDto relationshipDto)
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

                var relationship = _context.HaghighiUserRelationships.FirstOrDefault(u => u.RequestId == requestId && !u.IsDeleted);
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

        // DELETE: api/HaghighiUserRelationships/5
        [HttpDelete("{requestId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHaghighiUserRelationships(int requestId)
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

                var relationship = _context.HaghighiUserRelationships.FirstOrDefault(u => u.RequestId == requestId && !u.IsDeleted );
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


        
        [HttpPut("complete/{requestId}")]
        //[Authorize(Roles = "Admin")] // Assuming only admins can mark profiles as complete
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkRelationshipAsComplete(int requestId)
        {
            try
            {

                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var relationship = _context.HaghighiUserRelationships.FirstOrDefault(e => e.RequestId == requestId);
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

                var record = _context.HaghighiUserRelationships.FirstOrDefault(e => e.RequestId == requestId);
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

        [HttpGet("Admin/{requestId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHaghighiUserRelationshipByIdAdmin(int requestId)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var relationship = _context.HaghighiUserRelationships.FirstOrDefault(r => r.RequestId == requestId && !r.IsDeleted);
                if (relationship == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserRelationNotFound });
                }

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

        [HttpPut("Admin/{requestId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHaghighiUserRelationshipAdmin(int requestId, [FromBody] HaghighiUserRelationshipsDto relationshipDto)
        {

            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var relationship = _context.HaghighiUserRelationships.FirstOrDefault(r => r.RequestId == requestId && !r.IsDeleted);
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

        [HttpDelete("Admin/{requestId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHaghighiUserRelationshipAdmin(int requestId)
        {
            try
            {
                if (requestId <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var relationship = _context.HaghighiUserRelationships.FirstOrDefault(r => r.RequestId == requestId && !r.IsDeleted);
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
