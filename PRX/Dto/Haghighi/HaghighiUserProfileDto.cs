namespace PRX.Dto.Haghighi
{
    public class HaghighiUserProfileDto
    {
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


        public bool IsComplete { get; set; }
        public bool IsDeleted { get; set; }
    }
}
