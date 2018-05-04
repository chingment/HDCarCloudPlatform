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
        public string code { get; set; }
        public string descp { get; set; }
        public string inquiry { get; set; }
        public string message { get; set; }
        public string name { get; set; }
        public int opType { get; set; }
        public int remote { get; set; }
        public int sort { get; set; }

    }

    public class YdtInscarGetInquiryInfoResultData
    {
        public int areaId { get; set; }
        public string licensePic { get; set; }
        public List<channel> channelList { get; set; }
        public List<plan> planList { get; set; }


    }
}
