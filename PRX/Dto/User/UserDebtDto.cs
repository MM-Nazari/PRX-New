using System.ComponentModel.DataAnnotations;

namespace PRX.Dto.User
{
    public class UserDebtDto
    {

        //public int UserId { get; set; }
        public int RequestId { get; set; }

        public string DebtTitle { get; set; }
        
        public decimal DebtAmount { get; set; }
        
        public DateTime DebtDueDate { get; set; }
        
        public decimal DebtRepaymentPercentage { get; set; }


        public bool IsComplete { get; set; }
        public bool IsDeleted { get; set; }


    }
}
