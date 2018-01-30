using Lumos.Entity;
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

        public Enumeration.OrderStatus Status { get; set; }

        public int FollowStatus { get; set; }

        public string StatusName { get; set; }

        public string Price { get; set; }

        public string Remarks { get; set; }

        public DateTime SubmitTime { get; set; }

        public DateTime? CompleteTime { get; set; }

        public DateTime? CancleTime { get; set; }

        public DateTime? PayTime { get; set; }

        public string Deposit { get; set; }
        public string MobileTrafficFee { get; set; }

        public string ExpiryTime { get; set; }

    }
}