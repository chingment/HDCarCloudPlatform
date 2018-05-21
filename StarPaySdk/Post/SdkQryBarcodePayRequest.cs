using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarPaySdk
{
    public class SdkQryBarcodePayRequest : IStarPayPostRequest<SdkQryBarcodePayResult>
    {
        public SdkQryBarcodePayRequest(object postdata)
        {
            this.PostData = postdata;
        }


        public object PostData { get; set; }


        public string ApiName
        {
            get
            {
                return "adpweb/ehpspos3/sdkQryBarcodePay.json";
            }
        }
    }
}
