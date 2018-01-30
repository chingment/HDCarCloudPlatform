using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinShunPaySdk
{
    public class CodeDownload_Result: MinShunPayApiBaseResult
    {
        public string MERCID { get; set; }
        public string MWEB_URL { get; set; }
        public string TERMID { get; set; }
        public string ORDERID { get; set; }
        public string TXNAMT { get; set; }
    }
}
