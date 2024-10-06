namespace PRX.Dto.Haghighi
{
    public class HaghighiUserProfileDto
    {
        public int RequestId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FathersName { get; set; }
        public string NationalNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string BirthCertificateNumber { get; set; }
        public string MaritalStatus { get; set; }
        public string Gender { get; set; }
        public string PostalCode { get; set; }
        public string HomePhone { get; set; }
        public string Fax { get; set; }
        public string BestTimeToCall { get; set; }
        public string ResidentialAddress { get; set; }
        public string Email { get; set; }


        public bool IsComplete { get; set; }
        public bool IsDeleted { get; set; }
    }
}
