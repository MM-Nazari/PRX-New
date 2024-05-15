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
        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHaghighiUserRelationshipsById(int id)
        {
            try 
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }
                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Ensure that the user is deleting their own data
                if (id != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }
                var relationship = _context.HaghighiUserRelationships.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (relationship == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserRelationNotFound});
                }
                return Ok(relationship);
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
                    UserId = relationshipDto.UserId,
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

                return CreatedAtAction(nameof(GetHaghighiUserRelationshipsById), new { id = relationship.Id }, relationship);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ResponseMessages.InternalServerError, detail = ex.Message });
            }


        }

        // PUT: api/HaghighiUserRelationships/5
        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHaghighiUserRelationships(int id, [FromBody] HaghighiUserRelationshipsDto relationshipDto)
        {

            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }
                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Ensure that the user is deleting their own data
                if (id != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }
                var relationship = _context.HaghighiUserRelationships.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (relationship == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserRelationNotFound });
                }

                relationship.UserId = relationshipDto.UserId;
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
        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHaghighiUserRelationships(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }
                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Ensure that the user is deleting their own data
                if (id != tokenUserId)
                {
                    return Unauthorized(new { message = ResponseMessages.Unauthorized });
                }
                var relationship = _context.HaghighiUserRelationships.FirstOrDefault(u => u.UserId == id && !u.IsDeleted );
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


        
        [HttpPut("complete/{id}")]
        //[Authorize(Roles = "Admin")] // Assuming only admins can mark profiles as complete
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkRelationshipAsComplete(int id)
        {
            try
            {

                if (id <= 0)
                {
                    return BadRequest(new { message = ResponseMessages.InvalidId });
                }

                var relationship = _context.HaghighiUserRelationships.FirstOrDefault(e => e.UserId == id);
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
                    UserId = relationship.UserId,
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

                var relationship = _context.HaghighiUserRelationships.FirstOrDefault(r => r.UserId == id && !r.IsDeleted);
                if (relationship == null)
                {
                    return NotFound(new { message = ResponseMessages.HaghighiUserRelationNotFound });
                }

                var relationshipDto = new HaghighiUserRelationshipsDto
                {
                    UserId = relationship.UserId,
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

                var relationship = _context.HaghighiUserRelationships.FirstOrDefault(r => r.UserId == id && !r.IsDeleted);
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

                var relationship = _context.HaghighiUserRelationships.FirstOrDefault(r => r.UserId == id && !r.IsDeleted);
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
