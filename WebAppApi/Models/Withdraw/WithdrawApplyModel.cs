using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Withdraw
{
    public class WithdrawApplyModel
    {
        public int UserId { get; set; }

        public int MerchantId { get; set; }

        public decimal Amount { get; set; }

        public int BankCardId { get; set; }

        public bool Confirm { get; set; }
    }
}