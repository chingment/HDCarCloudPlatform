using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class PackageVOCarQuotePriceResponse
    {
        public string flag { set; get; }
        public string cerrRes { set; get; }
        public string cAppNo { set; get; }
        public string cProdNo { set; get; }
        public string tInsrncBgnTm { set; get; }
        public string tInsrncEndTm { set; get; }
        public string nIrrRatio { set; get; }
        public string nBasePrm { set; get; }
        public string nPrm { set; get; }
        public string nFundRate { set; get; }
        public string nFundAmount { set; get; }
        public string cQryCde { set; get; }
        public BaseVOCarQuotePriceResponse BaseVO { set; get; }
        public List<CvrgVO> CvrgList { set; get; }
        public PrmCoefVO PrmCoefVO { set; get; }
        public VhlVO VhlVO { set; get; }
        public VsTaxVO VsTaxVO { set; get; }
        public PackageVOCarQuotePriceResponse()
        {
            flag = "";
            cerrRes = "";
            cAppNo = "";
            cProdNo = "";
            tInsrncBgnTm = "";
            tInsrncEndTm = "";
            nIrrRatio = "";
            nBasePrm = "";
            nPrm = "";
            nFundRate = "";
            nFundAmount = "";
            cQryCde = "";
            BaseVO = new BaseVOCarQuotePriceResponse();
            CvrgList = new List<CvrgVO>();
            PrmCoefVO = new PrmCoefVO();
            VhlVO = new VhlVO();
            VsTaxVO = new VsTaxVO();
        }
    }
}
