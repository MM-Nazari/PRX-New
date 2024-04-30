using System.ComponentModel.DataAnnotations;

namespace PRX.Models.User
{
    public class UserDebt
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string DebtTitle { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal DebtAmount { get; set; }
        [Required]
        public DateTime DebtDueDate { get; set; }
        [Required]
        [Range(0, 100)]
        public decimal DebtRepaymentPercentage { get; set; }
        public bool IsComplete { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

        public User User { get; set; }
    }

}
