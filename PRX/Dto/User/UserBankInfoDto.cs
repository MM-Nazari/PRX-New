using System.ComponentModel.DataAnnotations;

namespace PRX.Dto.User
{
    public class UserBankInfoDto
    {
        public int RequestId { get; set; }

        public string TradeCode { get; set; } // کد معاملاتی

        public string SejamCode { get; set; } // کد سجام

        public string BankName { get; set; } // نام بانک

        public string BranchCode { get; set; } // کد شعبه

        public string BranchName { get; set; } // نام شعبه

        public string BranchCity { get; set; } // نام شهر محل شعبه


        public string AccountNumber { get; set; } // شماره حساب


        public string IBAN { get; set; } // شماره شبا

        public string AccountType { get; set; } // نوع حساب

        public decimal CapitalAmount { get; set; } // میزان سرمایه

        public string CapitalType { get; set; } // نوع سرمایه


        public bool IsComplete { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
}
