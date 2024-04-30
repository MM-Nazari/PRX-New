using System.ComponentModel.DataAnnotations;

namespace PRX.Dto.User
{
    public class UserDepositDto
    {
        
        public int UserId { get; set; }

        
        public decimal DepositAmount { get; set; }

        
        public DateTime DepositDate { get; set; }
        public string DepositSource { get; set; }

        public bool IsComplete { get; set; }
        public bool IsDeleted { get; set; }
    }
}
