using System.ComponentModel.DataAnnotations;

namespace PRX.Models.User
{
    public class UserWithdrawal
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal WithdrawalAmount { get; set; }
        [Required]
        public DateTime WithdrawalDate { get; set; }
        [Required]
        public string WithdrawalReason { get; set; }
        public bool IsComplete { get; set; } = false;

        public User User { get; set; }
    }

}
