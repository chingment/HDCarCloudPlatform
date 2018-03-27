using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Order
{
    public class OrderBaseDetailsViewModel
    {
        public int Id { get; set; }

        public string Sn { get; set; }
        public Enumeration.OrderStatus Status { get; set; }

        public int FollowStatus { get; set; }

        public string StatusName { get; set; }

        public string SubmitTime { get; set; }

        public string CompleteTime { get; set; }

        public string CancleTime { get; set; }

        public string PayTime { get; set; }

        public string Price { get; set; }

        public string Remarks { get; set; }
    }
}