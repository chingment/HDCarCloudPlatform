using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class Channel
    {
        public string channelCode { set; get; }//AEC16110001
        public string channelName { set; get; }
        public string channelComCode { set; get; }
        public string channelComName { set; get; }
        public string channelProductCode { set; get; }
        public string channelOperateCode { set; get; }
        public string channelTradeCode { set; get; }//000002
        public string channelTradeSerialNo { set; get; }//20151102092648
        public string channelRelationNo { set; get; }
        public string channelTradeDate { set; get; }//20151102
        public Channel()
        {
            channelCode = "";
            channelName = "";
            channelComCode = "";
            channelComName = "";
            channelProductCode = "";
            channelOperateCode = "";
            channelTradeCode = "";
            channelTradeSerialNo = "";
            channelRelationNo = "";
            channelTradeDate = "";
        }
    }
}
