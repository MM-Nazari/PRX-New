namespace PRX.Dto.User
{
    public class UserWithdrawalDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal WithdrawalAmount { get; set; }
        public DateTime WithdrawalDate { get; set; }
        public string WithdrawalReason { get; set; }
    }
}
