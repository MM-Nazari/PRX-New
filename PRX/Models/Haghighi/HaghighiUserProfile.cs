using Microsoft.EntityFrameworkCore;
using PRX.Models.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRX.Models.Haghighi
{
    public class HaghighiUserProfile
    {

        public int Id { get; set; }

        [Required]
        [ForeignKey("Request")]
        public int RequestId { get; set; }
        //public int UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FathersName { get; set; }
        [Required]
        [MaxLength(10)]
        public string NationalNumber { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string BirthPlace { get; set; }
        [Required]
        [MaxLength(10)]
        public string BirthCertificateNumber { get; set; }
        [Required]
        public string MaritalStatus { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        [MaxLength(10)]
        public string PostalCode { get; set; }
        [Required]
        public string HomePhone { get; set; }
        [Required]
        public string Fax { get; set; }
        [Required]
        public string BestTimeToCall { get; set; }
        [Required]
        public string ResidentialAddress { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public bool IsComplete { get; set; } = false;
        public bool IsDeleted { get; set; } = false;



        // Navigation property for one-to-one relationship with User
        public Request Request { get; set; }
    }
}
