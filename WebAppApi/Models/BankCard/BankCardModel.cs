using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.BankCard
{
    public class BankCardModel
    {
        public int Id { get; set; }

        public int BankId{ get; set; }

        public string BankCode{ get; set; }

        public string BankName { get; set; }

        public string BankAccountNo { get; set; }
    }
}