using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class ProposalGenerateResponse
    {
        public ResponseHead ResponseHead { set; get; }
        public ProposalGenerateResponseMain ProposalGenerateResponseMain { set; get; }
        public ProposalGenerateResponse()
        {
            ResponseHead = new ResponseHead();
            ProposalGenerateResponseMain = new ProposalGenerateResponseMain();
        }
    }
}
