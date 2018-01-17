using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class PolicyGenerateRequest
    {
        public RequestHead RequestHead { set; get; }
        public PolicyGenerateRequestMain PolicyGenerateRequestMain { set; get; }
        public PolicyGenerateRequest()
        {
            RequestHead = new RequestHead();
            PolicyGenerateRequestMain = new PolicyGenerateRequestMain();

            RequestHead.tradeTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            RequestHead.request_Type = "01";
            RequestHead.response_Type = "01";

            PolicyGenerateRequestMain.Channel.channelCode = "AEC16110001";
            PolicyGenerateRequestMain.Channel.channelTradeCode = "000004";
            PolicyGenerateRequestMain.Channel.channelRelationNo = RequestHead.tradeTime;
            PolicyGenerateRequestMain.Channel.channelTradeDate = RequestHead.tradeTime.Substring(0, 8);
        }
    }
}
