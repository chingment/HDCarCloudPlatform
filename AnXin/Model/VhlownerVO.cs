using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class VhlownerVO
    {

        public string cOwnerCde { set; get; }//
        public string cOwnerNme { set; get; }//车主名称
        public string cCertfCls { set; get; }//居民身份证120001
        public string cCertfCde { set; get; }//车主证件号码
        public string cResidAddr { set; get; }//车主地址
        public string cZipCde { set; get; }//车主邮政编码
        public string cOwnerCls { set; get; }//车主性质
        public string cClntAddr { set; get; }
        public VhlownerVO()
        {
            cOwnerCde = "";
            cOwnerNme = "";
            cCertfCls = "120001";
            cCertfCde = "";
            cResidAddr = "";
            cZipCde = "";
            cOwnerCls = "";
            cClntAddr = "";
        }
    }
}
