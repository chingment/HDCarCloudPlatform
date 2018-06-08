using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{
    public class YdtInsCarQueryInquiryResultData
    {
        public string employeeName { get; set; }

        public string code { get; set; }

        public string createTime { get; set; }

        public string carOwner { get; set; }

        public YdtInscarInquiryPms inquiry { get; set; }

        public YdtInscarInfoModel car { get; set; }

        public channel channel { get; set; }

    }
}
