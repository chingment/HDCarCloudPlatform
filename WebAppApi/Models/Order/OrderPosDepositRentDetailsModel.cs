using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Order
{
    public class OrderPosDepositRentDetailsModel
    {
        public int Id { get; set; }

        public string Sn { get; set; }

        public string Deposit { get; set; }

        public string RentTotal { get; set; }

        public string Price { get; set; }

        public string RentDueDate { get; set; }

        public string RentMonths { get; set; }
    }
}