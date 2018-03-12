using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    public class PayConfirmModel
    {

        public int UserId { get; set; }

        public int MerchantId { get; set; }

        public int PosMachineId { get; set; }

        public int OrderId { get; set; }

        public string OrderSn { get; set; }

        public Enumeration.ProductType ProductType { get; set; }

        public object Params { get; set; }
    }


    public enum PayResultType
    {
        Unknown = 0,
        Success = 1,
        Failure = 2
    }


    public class PayResultModel
    {
        public PayResultModel()
        {
            this.Params = new PayResultDataModel();
        }

        public int UserId { get; set; }

        public int MerchantId { get; set; }

        public int OrderId { get; set; }

        public string OrderSn { get; set; }

        public Enumeration.ProductType ProductType { get; set; }

        public PayResultDataModel Params { get; set; }
    }


    public class PayResultMerchantInfo
    {
        public string order_no { get; set; }
    }

    public class PayResultDataModel
    {
        public PayResultDataModel()
        {
            this.MerchantInfo = new PayResultMerchantInfo();
        }

        public PayResultType Result { get; set; }

        public string Amount { get; set; }

        public string TraceNo { get; set; }

        public string ReferenceNo { get; set; }

        public string MerchantNo { get; set; }

        public string CardNo { get; set; }

        public string Type { get; set; }

        public string Issue { get; set; }

        public string BatchNo { get; set; }

        public string TerminalId { get; set; }

        public string MerchantId { get; set; }

        public string MerchantName { get; set; }

        public string FailureReason { get; set; }

        public PayResultMerchantInfo MerchantInfo { get; set; }
    }
}
