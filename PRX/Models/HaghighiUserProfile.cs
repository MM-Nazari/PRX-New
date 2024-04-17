namespace PRX.Models
{
    public class HaghighiUserProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FathersName { get; set; }
        public string NationalNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string BirthCertificateNumber { get; set; }
        public string MaritalStatus { get; set; }
        public string Gender { get; set; }
        public int PostalCode { get; set; }
        public string HomePhone { get; set; }
        public string Fax { get; set; }
        public DateTime BestTimeToCall { get; set; }
        public string ResidentialAddress { get; set; }
        public string Email { get; set; }


        // Navigation property for one-to-one relationship with User
        public User User { get; set; }
    }
}
