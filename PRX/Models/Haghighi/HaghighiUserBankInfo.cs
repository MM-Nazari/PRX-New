using PRX.Models.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRX.Models.Haghighi
{
    public class HaghighiUserBankInfo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Request")]
        public int RequestId { get; set; }
        //public int UserId { get; set; }

        [Required]
        public string TradeCode { get; set; } // کد معاملاتی

        public string SejamCode { get; set; } // کد سجام

        [Required]
        public string BankName { get; set; } // نام بانک

        public string BranchCode { get; set; } // کد شعبه

        public string BranchName { get; set; } // نام شعبه

        public string BranchCity { get; set; } // نام شهر محل شعبه

        [Required]
        public string AccountNumber { get; set; } // شماره حساب

        [Required]
        public string IBAN { get; set; } // شماره شبا

        public string AccountType { get; set; } // نوع حساب

        public decimal CapitalAmount { get; set; } // میزان سرمایه

        public string CapitalType { get; set; } // نوع سرمایه


        public bool IsComplete { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

        public Request Request { get; set; }

    }
}
