using System.ComponentModel.DataAnnotations;

namespace PRX.Dto.User
{
    public class UserInvestmentDto
    {
        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be greater than or equal to 0")]
        public decimal Amount { get; set; }
    }
}
