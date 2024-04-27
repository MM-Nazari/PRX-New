using System.ComponentModel.DataAnnotations;

namespace PRX.Models.Haghighi
{
    public class HaghighiUserEducationStatus
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string LastDegree { get; set; }
        [Required]
        public string FieldOfStudy { get; set; }
        [Required]
        public int GraduationYear { get; set; }
        [Required]
        public string IssuingAuthority { get; set; }

        public bool IsComplete { get; set; } = false;

        public PRX.Models.User.User User { get; set; }
    }
}
