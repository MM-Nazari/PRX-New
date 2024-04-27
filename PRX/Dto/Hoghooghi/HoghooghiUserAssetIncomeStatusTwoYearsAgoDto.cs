namespace PRX.Dto.Hoghooghi
{
    public class HoghooghiUserAssetIncomeStatusTwoYearsAgoDto
    {
        public int UserId { get; set; }
        public int FiscalYear { get; set; }
        public decimal RegisteredCapital { get; set; }
        public decimal ApproximateAssetValue { get; set; }
        public decimal TotalLiabilities { get; set; }
        public decimal TotalInvestments { get; set; }
        public decimal OperationalIncome { get; set; }
        public decimal OtherIncome { get; set; }
        public decimal OperationalExpenses { get; set; }
        public decimal OtherExpenses { get; set; }
        public decimal OperationalProfitOrLoss { get; set; }
        public decimal NetProfitOrLoss { get; set; }
        public decimal AccumulatedProfitOrLoss { get; set; }
    }
}
