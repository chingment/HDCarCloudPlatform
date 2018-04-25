using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{
    public class plan
    {
        public int areaId { get; set; }
        public string areaName { get; set; }
    }

    public class channel
    {
        public int channelId { get; set; }
        public int code { get; set; }
        public string descp { get; set; }
        public string inquiry { get; set; }
        public string message { get; set; }
        public string name { get; set; }
        public string opType { get; set; }
        public string remote { get; set; }
        public string sort { get; set; }

    }

    public class YdtInscarGetInquiryInfoResultData
    {
        public string areaId { get; set; }
        public string licensePic { get; set; }
        public List<channel> channelList { get; set; }
        public List<plan> planList { get; set; }


    }
}
