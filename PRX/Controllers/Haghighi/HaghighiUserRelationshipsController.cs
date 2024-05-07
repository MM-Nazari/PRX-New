using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRX.Data;
using PRX.Dto.Haghighi;
using PRX.Models.Haghighi;

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
            var relationships = _context.HaghighiUserRelationships.ToList();
            return Ok(relationships);
        }

        // GET: api/HaghighiUserRelationships/5
        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHaghighiUserRelationshipsById(int id)
        {
            try {
                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Ensure that the user is deleting their own data
                if (id != tokenUserId)
                {
                    return Forbid(); // Or return 403 Forbidden
                }
                var relationship = _context.HaghighiUserRelationships.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (relationship == null)
                {
                    return NotFound();
                }
                return Ok(relationship);
            }
            catch (Exception ex)
            {
                
                return BadRequest();
            }

  
        }

        // POST: api/HaghighiUserRelationships
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateHaghighiUserRelationships([FromBody] HaghighiUserRelationshipsDto relationshipDto)
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
                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Ensure that the user is deleting their own data
                if (id != tokenUserId)
                {
                    return Forbid(); // Or return 403 Forbidden
                }
                var relationship = _context.HaghighiUserRelationships.FirstOrDefault(u => u.UserId == id && !u.IsDeleted);
                if (relationship == null)
                {
                    return NotFound();
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
                
                return BadRequest();
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
                // Retrieve the user ID from the token
                var tokenUserId = int.Parse(User.FindFirst("id")?.Value);

                // Ensure that the user is deleting their own data
                if (id != tokenUserId)
                {
                    return Forbid(); // Or return 403 Forbidden
                }
                var relationship = _context.HaghighiUserRelationships.FirstOrDefault(u => u.UserId == id && !u.IsDeleted );
                if (relationship == null)
                {
                    return NotFound();
                }

                relationship.IsDeleted = true;
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                
                return BadRequest();
            }

        }

        // DELETE: api/HaghighiUserRelationships/clear
        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearAllHaghighiUserRelationships()
        {
            _context.HaghighiUserRelationships.RemoveRange(_context.HaghighiUserRelationships);
            _context.SaveChanges();

            return Ok();
        }


        
        [HttpPut("complete/{id}")]
        //[Authorize(Roles = "Admin")] // Assuming only admins can mark profiles as complete
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkRelationshipAsComplete(int id)
        {
            var relationship = _context.HaghighiUserRelationships.FirstOrDefault(e => e.UserId == id);
            if (relationship == null)
            {
                return NotFound();
            }

            relationship.IsComplete = true;
            _context.SaveChanges();

            return Ok();
        }


        [HttpGet("Admin")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAllHaghighiUserRelationshipsAdmin()
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

        [HttpGet("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHaghighiUserRelationshipByIdAdmin(int id)
        {
            var relationship = _context.HaghighiUserRelationships.FirstOrDefault(r => r.UserId == id && !r.IsDeleted);
            if (relationship == null)
            {
                return NotFound();
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

        [HttpPut("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHaghighiUserRelationshipAdmin(int id, [FromBody] HaghighiUserRelationshipsDto relationshipDto)
        {
            var relationship = _context.HaghighiUserRelationships.FirstOrDefault(r => r.UserId == id && !r.IsDeleted);
            if (relationship == null)
            {
                return NotFound();
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

            return Ok();
        }

        [HttpDelete("Admin/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHaghighiUserRelationshipAdmin(int id)
        {
            var relationship = _context.HaghighiUserRelationships.FirstOrDefault(r => r.UserId == id && !r.IsDeleted);
            if (relationship == null)
            {
                return NotFound();
            }

            relationship.IsDeleted = true;
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("Admin/Clear")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ClearHaghighiUserRelationships()
        {
            _context.HaghighiUserRelationships.RemoveRange(_context.HaghighiUserRelationships);
            _context.SaveChanges();

            return Ok();
        }


    }
}
