using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
   public  class CarBusinessDetailInfoQueryRequest
    {
       public RequestHead RequestHead { set; get; }
       public CarBusinessDetailInfoQueryRequestMain CarBusinessDetailInfoQueryRequestMain { set; get; }
       public CarBusinessDetailInfoQueryRequest()
       {
           DateTime today = DateTime.Now;
           RequestHead = new RequestHead();
           CarBusinessDetailInfoQueryRequestMain = new CarBusinessDetailInfoQueryRequestMain();

           RequestHead.tradeTime = DateTime.Now.ToString("yyyyMMddHHmmss");
           RequestHead.request_Type = "01";

           CarBusinessDetailInfoQueryRequestMain.Channel.channelCode = "AEC16110001";
           CarBusinessDetailInfoQueryRequestMain.Channel.channelTradeCode = "000005";
           CarBusinessDetailInfoQueryRequestMain.Channel.channelRelationNo = RequestHead.tradeTime;
           CarBusinessDetailInfoQueryRequestMain.Channel.channelTradeDate = RequestHead.tradeTime.Substring(0, 8);
       }

    }
}
