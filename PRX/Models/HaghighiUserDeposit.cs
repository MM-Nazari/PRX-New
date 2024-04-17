namespace PRX.Models
{
    public class HaghighiUserDeposit
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public float DepositAmount { get; set; }
        public DateTime DepositDate { get; set; }
        public string DepositSource { get; set; }
    }
}
