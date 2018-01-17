using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class PackageVO
    {
        public string cProdNo { set; get; }
        public string tInsrncBgnTm { set; get; }//2016-12-20 00:00:00
        public string TInsrncEndTm { set; get; }//2017-12-19 23:59:59
        public VhlVO VhlVO { set; get; }
        public List<CvrgVO> CvrgList { set; get; }

        public PackageVO()
        {
           VhlVO = new VhlVO();
           CvrgList = new List<CvrgVO>();
        }
    }
}
