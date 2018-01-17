using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Fund
{
    public class BankCard
    {
        public int Id { get; set; }

        public string BankName { get; set; }

        public string BankAccountNo { get; set; }

        public string BankLogo{ get; set; }

    }

    public class BalanceResultModel
    {
        public decimal Balance { get; set; }

        public BankCard BankCard { get; set; }
    }
}