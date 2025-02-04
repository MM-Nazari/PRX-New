﻿using PRX.Models.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRX.Models.Hoghooghi
{
    public class HoghooghiUserBoardOfDirectors
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Request")]
        public int RequestId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Position { get; set; }

        [Required]
        public string EducationalLevel { get; set; }

        [Required]
        public string FieldOfStudy { get; set; }

        [Required]
        public string ExecutiveExperience { get; set; }

        [Required]
        public string FamiliarityWithCapitalMarket { get; set; }

        [Required]
        public string PersonalInvestmentExperienceInStockExchange { get; set; }

        [Required]
        public bool IsComplete { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public Request Request { get; set; }
    }
}
