namespace PRX.Models
{
    public class HaghighiUserFinancialProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public float MainContinuousIncome { get; set; }
        public float OtherIncomes { get; set; }
        public float SupportFromOthers { get; set; }
        public float ContinuousExpenses { get; set; }
        public float OccasionalExpenses { get; set; }
        public float ContributionToOthers { get; set; }
    }
}
