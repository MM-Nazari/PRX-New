using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRX.Models.User
{
    public class UserInvestmentExperience
    {
        public int Id { get; set; }
        //[Required]
        //public int UserId { get; set; }
        [Required]
        [ForeignKey("Request")]
        public int RequestId { get; set; }
        [Required]
        public string InvestmentType { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal InvestmentAmount { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int InvestmentDurationMonths { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal ProfitLossAmount { get; set; }
        public string ProfitLossDescription { get; set; }
        public string ConversionReason { get; set; }
        public bool IsComplete { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

        //public User User { get; set; }
        public Request Request { get; set; }
    }

}
