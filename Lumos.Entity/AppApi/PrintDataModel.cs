using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lumos.Entity.AppApi
{
    public class PrintDataModel
    {
        public string MerchantName { get; set; }
        public string MerchantCode { get; set; }
        public string OrderType { get; set; }
        public string TradeType { get; set; }
        public string TradeNo { get; set; }
        public string TradeDateTime { get; set; }
        public string TradePayMethod { get; set; }
        public string TradeAmount { get; set; }
        public string ServiceHotline { get; set; }
    }
}