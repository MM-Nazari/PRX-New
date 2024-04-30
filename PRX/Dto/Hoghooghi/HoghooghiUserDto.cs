namespace PRX.Dto.Hoghooghi
{
    public class HoghooghiUserDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string RegistrationLocation { get; set; }
        public string NationalId { get; set; }
        public string MainActivityBasedOnCharter { get; set; }
        public string MainActivityBasedOnPastThreeYearsPerformance { get; set; }
        public string PostalCode { get; set; }
        public string LandlinePhone { get; set; }
        public string Fax { get; set; }
        public string BestTimeToCall { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string RepresentativeName { get; set; }
        public string RepresentativeNationalId { get; set; }
        public string RepresentativeMobilePhone { get; set; }


        public bool IsComplete { get; set; }
        public bool IsDeleted { get; set; }
    }
}
