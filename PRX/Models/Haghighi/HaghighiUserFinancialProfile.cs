using System.ComponentModel.DataAnnotations;

namespace PRX.Models.Haghighi
{
    public class HaghighiUserFinancialProfile
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal MainContinuousIncome { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal OtherIncomes { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal SupportFromOthers { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal ContinuousExpenses { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal OccasionalExpenses { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal ContributionToOthers { get; set; }

        public bool IsComplete { get; set; } = false;

        public PRX.Models.User.User User { get; set; }
    }
}
