using PRX.Models.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRX.Models.Haghighi
{
    public class HaghighiUserFinancialProfile
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("Request")]
        public int RequestId { get; set; }

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
        public bool IsDeleted { get; set; } = false;

        public Request Request { get; set; }
    }
}
