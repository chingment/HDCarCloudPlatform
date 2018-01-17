using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.BankCard
{
    public class BindBankCardModel
    {
        public int UserId { get; set; }

        public int MerchantId { get; set; }

        public int BankId { get; set; }

        public string BankAccountPhone { get; set; }

        public string BankAccountName { get; set; }

        public string BankAccountNo { get; set; }
    }
}