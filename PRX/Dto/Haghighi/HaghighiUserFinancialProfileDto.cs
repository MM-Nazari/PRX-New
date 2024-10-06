namespace PRX.Dto.Haghighi
{
    public class HaghighiUserFinancialProfileDto
    {
        public int RequestId { get; set; }
        public decimal MainContinuousIncome { get; set; }
        public decimal OtherIncomes { get; set; }
        public decimal SupportFromOthers { get; set; }
        public decimal ContinuousExpenses { get; set; }
        public decimal OccasionalExpenses { get; set; }
        public decimal ContributionToOthers { get; set; }


        public bool IsComplete { get; set; }
        public bool IsDeleted { get; set; }
    }
}
