namespace PRX.Dto.Hoghooghi
{
    public class HoghooghiUserBoardOfDirectorsDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public string EducationalLevel { get; set; }
        public string FieldOfStudy { get; set; }
        public string ExecutiveExperience { get; set; }
        public string FamiliarityWithCapitalMarket { get; set; }
        public string PersonalInvestmentExperienceInStockExchange { get; set; }


        public bool IsComplete { get; set; }
        public bool IsDeleted { get; set; }
    }
}
