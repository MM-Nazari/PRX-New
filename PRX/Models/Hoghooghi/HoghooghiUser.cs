using PRX.Models.User;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRX.Models.Hoghooghi
{
    public class HoghooghiUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Request")]
        public int RequestId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string RegistrationNumber { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        [Required]
        public string RegistrationLocation { get; set; }

        [Required]
        [StringLength(10)] // Assuming a maximum length for national ID
        public string NationalId { get; set; }

        [Required]
        public string MainActivityBasedOnCharter { get; set; }

        [Required]
        public string MainActivityBasedOnPastThreeYearsPerformance { get; set; }

        [Required]
        [StringLength(10)] // Assuming a maximum length for postal code
        public string PostalCode { get; set; }

        [Required]
        public string LandlinePhone { get; set; }

        [Required]
        public string Fax { get; set; }

        [Required]
        public string BestTimeToCall { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string RepresentativeName { get; set; }

        [Required]
        [StringLength(10)] // Assuming a maximum length for representative national ID
        public string RepresentativeNationalId { get; set; }

        [Required]
        public string RepresentativeMobilePhone { get; set; }

        [Required]
        public bool IsComplete { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

        public Request Request { get; set; }
    }
}
