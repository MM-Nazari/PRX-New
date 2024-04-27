using System.ComponentModel.DataAnnotations;

namespace PRX.Dto.User
{
    public class UserInvestmentExperienceDto
    {
        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Investment Type is required")]
        public string InvestmentType { get; set; }

        [Required(ErrorMessage = "Investment Amount is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Investment Amount must be greater than or equal to 0")]
        public decimal InvestmentAmount { get; set; }

        [Required(ErrorMessage = "Investment Duration in Months is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Investment Duration in Months must be greater than or equal to 0")]
        public int InvestmentDurationMonths { get; set; }

        [Required(ErrorMessage = "Profit/Loss Amount is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Profit/Loss Amount must be greater than or equal to 0")]
        public decimal ProfitLossAmount { get; set; }

        public string ProfitLossDescription { get; set; }

        public string ConversionReason { get; set; }
    }
}
