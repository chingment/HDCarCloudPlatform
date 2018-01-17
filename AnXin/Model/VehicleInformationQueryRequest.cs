using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class VehicleInformationQueryRequest
    {
        public RequestHead RequestHead { set; get; }
        public VehicleInformationQueryRequestMain VehicleInformationQueryRequestMain { set; get; }
        public VehicleInformationQueryRequest()
        {
            RequestHead = new RequestHead();
            VehicleInformationQueryRequestMain = new VehicleInformationQueryRequestMain();


            DateTime today = DateTime.Now;
            RequestHead = new RequestHead();

            RequestHead.tradeTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            RequestHead.request_Type = "01";
            RequestHead.response_Type = "01";

            VehicleInformationQueryRequestMain.Channel.channelCode = "AEC16110001";
            VehicleInformationQueryRequestMain.Channel.channelTradeCode = "000015";
            VehicleInformationQueryRequestMain.Channel.channelRelationNo = RequestHead.tradeTime;
            VehicleInformationQueryRequestMain.Channel.channelTradeDate = RequestHead.tradeTime.Substring(0, 8);
        }

    }

}
