namespace PRX.Models
{
    public class HaghighiUserWithdrawal
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public float WithdrawalAmount { get; set; }
        public DateTime WithdrawalDate { get; set; }
        public string WithdrawalReason { get; set; }
    }

}
