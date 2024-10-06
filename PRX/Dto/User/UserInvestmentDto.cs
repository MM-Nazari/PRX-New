using System.ComponentModel.DataAnnotations;

namespace PRX.Dto.User
{
    public class UserInvestmentDto
    {
        public int RequestId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Amount must be greater than or equal to 0")]
        public decimal Amount { get; set; }

        public bool IsComplete { get; set; }
        public bool IsDeleted { get; set; }
    }
}
