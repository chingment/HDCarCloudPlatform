using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Order
{
    public class GetPayTranSnParams
    {
        public int MerchantId { get; set; }

        public int PosMachineId { get; set; }

        public int UserId { get; set; }

        public string OrderSn { get; set; }

        public int OrderId { get; set; }

        public int TransType { get; set; }
    }
}