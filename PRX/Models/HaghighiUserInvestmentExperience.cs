namespace PRX.Models
{
    public class HaghighiUserInvestmentExperience
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string InvestmentType { get; set; }
        public float InvestmentAmount { get; set; }
        public float InvestmentDurationMonths { get; set; }
        public float ProfitLossAmount { get; set; }
        public string ProfitLossDescription { get; set; }
        public string ConversionReason { get; set; }
    }

}
