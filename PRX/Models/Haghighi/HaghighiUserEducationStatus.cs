using PRX.Models.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRX.Models.Haghighi
{
    public class HaghighiUserEducationStatus
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("Request")]
        public int RequestId { get; set; }

        [Required]
        public string LastDegree { get; set; }

        [Required]
        public string FieldOfStudy { get; set; }

        [Required]
        public int GraduationYear { get; set; }

        [Required]
        public string IssuingAuthority { get; set; }

        public bool IsComplete { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

        public Request Request { get; set; }
    }
}
