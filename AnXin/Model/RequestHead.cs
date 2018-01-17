using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class RequestHead
    {
        public string request_Type { get; set; }

        public string user { get; set; }

        public string password { get; set; }

        public string queryId { get; set; }

        public string sourceSystemCode { get; set; }

        public string versionNo { get; set; }

        public string areaCode { get; set; }

        public string areaName { get; set; }

        public string tradeTime { get; set; }

        public string response_Type { get; set; }

        public string signData { get; set; }
        public RequestHead()
        {
            request_Type = "";
            user = "";
            password = "";
            queryId = "";
            sourceSystemCode = "";
            versionNo = "";
            areaCode = "";
            areaName = "";
            tradeTime = "";//20151102092648
            response_Type = "01";
            signData = "";
        }
    }
}
