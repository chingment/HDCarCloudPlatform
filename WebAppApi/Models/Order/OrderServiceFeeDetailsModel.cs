using Lumos.Entity;
using Lumos.Entity.AppApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Order
{
    public class OrderServiceFeeDetailsModel : OrderBaseDetailsViewModel
    {
        public string Deposit { get; set; }
        public string MobileTrafficFee { get; set; }

        public string ExpiryTime { get; set; }

        public PrintDataModel PrintData { get; set; }

    }
}