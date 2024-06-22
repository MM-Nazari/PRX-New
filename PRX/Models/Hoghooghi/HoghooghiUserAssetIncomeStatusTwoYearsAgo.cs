using PRX.Models.User;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRX.Models.Hoghooghis.Hoghooghi
{
    public class HoghooghiUserAssetIncomeStatusTwoYearsAgo
    {
        [Key]
        public int Id { get; set; }

        //[Required]
        //[ForeignKey("User")]
        //public int UserId { get; set; }

        [Required]
        [ForeignKey("Request")]
        public int RequestId { get; set; }

        [Required]
        public int FiscalYear { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")] // Assuming 18 digits total, with 2 decimal places
        [Range(0, double.MaxValue)] // Ensures the value is positive
        public decimal RegisteredCapital { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, double.MaxValue)]
        public decimal ApproximateAssetValue { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, double.MaxValue)]
        public decimal TotalLiabilities { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, double.MaxValue)]
        public decimal TotalInvestments { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, double.MaxValue)]
        public decimal OperationalIncome { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, double.MaxValue)]
        public decimal OtherIncome { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, double.MaxValue)]
        public decimal OperationalExpenses { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, double.MaxValue)]
        public decimal OtherExpenses { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, double.MaxValue)]
        public decimal OperationalProfitOrLoss { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, double.MaxValue)]
        public decimal NetProfitOrLoss { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, double.MaxValue)]
        public decimal AccumulatedProfitOrLoss { get; set; }

        [Required]
        public bool IsComplete { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

        // Navigation property for the one-to-many relationship with User table
        //public PRX.Models.User.User User { get; set; }
        public Request Request { get; set; }
    }
}
