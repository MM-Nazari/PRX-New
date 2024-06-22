using System.ComponentModel.DataAnnotations;

namespace PRX.Dto.User
{
    public class UserFinancialChangesDto
    {
        
        public int RequestId { get; set; }
        public string Description { get; set; }

        public bool IsComplete { get; set; }
        public bool IsDeleted { get; set; }
    }
}
