using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{
    public class YdtInscarPayResultNotifyData
    {
        public string orderSeq { get; set; }

        public decimal premium { get; set; }

        public string message { get; set; }

        public string createDate { get; set; }
    }
}
