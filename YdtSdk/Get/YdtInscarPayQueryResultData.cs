using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{
    public class YdtInscarPayQueryResultData
    {
        public string paySeq { get; set; }
        public string insureSeq { get; set; }
        public string orderSeq { get; set; }
        public int result { get; set; }
        public string message { get; set; }
        public string biProposalNo { get; set; }
        public string ciProposalNo { get; set; }

        public string code { get; set; }
    }
}
