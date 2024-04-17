namespace PRX.Models
{
    public class HaghighiUserDebt
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string DebtTitle { get; set; }
        public float DebtAmount { get; set; }
        public DateTime DebtDueDate { get; set; }
        public float DebtRepaymentPercentage { get; set; }
    }

}
