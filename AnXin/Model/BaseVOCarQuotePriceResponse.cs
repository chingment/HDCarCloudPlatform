using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class BaseVOCarQuotePriceResponse
    {
        public string cDptCde { set; get; }
        public string cBrkrCde { set; get; }
        public string cOprCde { set; get; }
        public string cChaType { set; get; }
        public string cSlsId { set; get; }
        public string cBrkSlsCde { set; get; }
        public string cAgtAgrNo { set; get; }
        public string cDataSrc { set; get; }
        public string cApproveCde { set; get; }
        public string cProvince { set; get; }
        public string cAreaFlag { set; get; }
        public string cCountryFlag { set; get; }
        public string cAppAreaCode { set; get; }
        public string cImmeffMrk { set; get; }
        public string cResvTxt13 { set; get; }
        public string cResvTxt14 { set; get; }
        public string cResvTxt17 { set; get; }
        public string cResvTxt18 { set; get; }
        public BaseVOCarQuotePriceResponse()
        {
            cDptCde = "";
            cBrkrCde = "";
            cOprCde = "";
            cChaType = "";
            cSlsId = "";
            cBrkSlsCde = "";
            cAgtAgrNo = "";
            cDataSrc = "";
            cApproveCde = "";
            cProvince = "";
            cAreaFlag = "";
            cCountryFlag = "";
            cAppAreaCode = "";
            cImmeffMrk = "";
            cResvTxt13 = "";
            cResvTxt14 = "";
            cResvTxt17 = "";
            cResvTxt18 = "";
        }
    }
}
