namespace PRX.Dto.Hoghooghi
{
    public class HoghooghiUserCompaniesWithMajorInvestorsListDto
    {
        public int RequestId { get; set; }
        public List<HoghooghiUserCompaniesWithMajorInvestorsDto> MajorInvestors { get; set; }
    }
}
