using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class CarBusinessDetailInfoQueryRequestMain
    {
        public Channel Channel { set; get; }
        public string cPlyNo { set; get; }
        public string cAppNo { set; get; }
        public string cInsuredNme { set; get; }
        public string cCertfCls { set; get; }
        public string cCertfCde { set; get; }
        public string cPlateNo { set; get; }
        public string cFrmNo { set; get; }
        public string cAppTyp { set; get; }
        public CarBusinessDetailInfoQueryRequestMain()
        {
            Channel = new Channel();
            cPlyNo = "";
            cAppNo = "";
            cInsuredNme = "";
            cCertfCls = "";
            cCertfCde = "";
            cPlateNo = "";
            cFrmNo = "";
            cAppTyp = "";
        }

    }
}
