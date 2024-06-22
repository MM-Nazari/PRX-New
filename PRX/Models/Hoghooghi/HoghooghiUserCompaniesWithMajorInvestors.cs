﻿using PRX.Models.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRX.Models.Hoghooghi
{
    public class HoghooghiUserCompaniesWithMajorInvestors
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
        public string CompanyName { get; set; }

        [Required]
        public string CompanySubject { get; set; }

        [Required]
        [Range(0, double.MaxValue)] // Ensures the value is positive
        public decimal PercentageOfTotal { get; set; }

        [Required]
        public bool IsComplete { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        // Navigation property for the one-to-many relationship with User table
        //public PRX.Models.User.User User { get; set; }
        public Request Request { get; set; }
    }
}
