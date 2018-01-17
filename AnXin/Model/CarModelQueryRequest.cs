using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class CarModelQueryRequest
    {
        public RequestHead RequestHead { set; get; }
        public CarModelQueryRequestMain CarModelQueryRequestMain { set; get; }

        public CarModelQueryRequest()
        {
            DateTime today = DateTime.Now;
            RequestHead = new RequestHead();
            CarModelQueryRequestMain = new CarModelQueryRequestMain();

            RequestHead.tradeTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            RequestHead.request_Type = "01";
            RequestHead.response_Type = "01";

            CarModelQueryRequestMain.Channel.channelCode = "AEC16110001";
            CarModelQueryRequestMain.Channel.channelTradeCode = "000001";
            CarModelQueryRequestMain.Channel.channelRelationNo = RequestHead.tradeTime;
            CarModelQueryRequestMain.Channel.channelTradeDate = RequestHead.tradeTime.Substring(0, 8);
        }
    }
}
