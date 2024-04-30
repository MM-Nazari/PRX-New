using PRX.Models.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRX.Models.Hoghooghi
{
    public class HoghooghiUserBoardOfDirectors
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

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
        // Navigation property for the one-to-many relationship with User table
        public PRX.Models.User.User User { get; set; }
    }
}
