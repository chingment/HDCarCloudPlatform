using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarPaySdk
{
    public class SdkBarcodePosPayRequest : IStarPayPostRequest<SdkBarcodePosPayResult>
    {
        public SdkBarcodePosPayRequest(object postdata)
        {
            this.PostData = postdata;
        }


        public object PostData { get; set; }


        public string ApiName
        {
            get
            {
                return "adpweb/ehpspos3/sdkBarcodePosPay.json";
            }
        }
    }
}
