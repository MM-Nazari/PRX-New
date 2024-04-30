namespace PRX.Dto.Hoghooghi
{
    public class HoghooghiUserCompaniesWithMajorInvestorsDto
    {
        public int UserId { get; set; }
        public string CompanyName { get; set; }
        public string CompanySubject { get; set; }
        public decimal PercentageOfTotal { get; set; }


        public bool IsComplete { get; set; }
        public bool IsDeleted { get; set; }
    }
}
