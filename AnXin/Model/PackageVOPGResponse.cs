using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class PackageVOPGResponse
    {
        public string cAppNo { set; get; }
        public string cProdNo { set; get; }
        public string cUdrMrk { set; get; }
        public string cerrRes { set; get; }
        public string flag { set; get; }
        public PackageVOPGResponse()
        {
            cAppNo = "";
            cProdNo = "";
            cUdrMrk = "";
            cerrRes = "";
            flag = "";
        }
    }
}
