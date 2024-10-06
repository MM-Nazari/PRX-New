using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRX.Models.User
{
    public class UserDeposit
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("Request")]
        public int RequestId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal DepositAmount { get; set; }

        [Required]
        public DateTime DepositDate { get; set; }

        [Required]
        public string DepositSource { get; set; }
        public bool IsComplete { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public Request Request { get; set; }
    }
}
