using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class PayVO
    {
        public string nTms { set; get; }
        public string cCurCde { set; get; }
        public string nPayablePrm { set; get; }
        public string nResvNum2 { set; get; }
        public string opdate { set; get; }
        public string baserealamount { set; get; }
        public string receivableamount { set; get; }
        public PayVO()
        {
            nTms = "";
            cCurCde = "";
            nPayablePrm = "";
            nResvNum2 = "";
            opdate = "";
            baserealamount = "";
            receivableamount = "";
        }
    }
}
