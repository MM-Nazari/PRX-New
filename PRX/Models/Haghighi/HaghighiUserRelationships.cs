using PRX.Models.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRX.Models.Haghighi
{
    public class HaghighiUserRelationships
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("Request")]
        public int RequestId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string RelationshipStatus { get; set; }

        [Required]
        [Range(1300, 1400)]
        public int BirthYear { get; set; }

        [Required]
        public string EducationLevel { get; set; }

        [Required]
        public string EmploymentStatus { get; set; }

        [Required]
        [Range(0, double.PositiveInfinity)]
        public decimal AverageMonthlyIncome { get; set; }

        [Required]
        [Range(0, double.PositiveInfinity)]
        public decimal AverageMonthlyExpense { get; set; }

        [Required]
        [Range(0, double.PositiveInfinity)]
        public decimal ApproximateAssets { get; set; }

        [Required]
        [Range(0, double.PositiveInfinity)]
        public decimal ApproximateLiabilities { get; set; }

        public bool IsComplete { get; set; } = false;

        public bool IsDeleted { get; set; } = false;

        public Request Request { get; set; }
    }
}
