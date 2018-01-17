using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class CvrgVO
    {
        public string cCvrgNo { set; get; }//险别代码
        public string nAmt { set; get; }//保额
        public string cDductMrk { set; get; }//是否不计免赔
        public string nLiabDaysLmt { set; get; }//数量（人数，天数）
        public string nPerAmt { set; get; }//保额(每人,每天)
        public string cCustCvrgNme { set; get; }
        public string cGlassTyp { set; get; }
        public string nrate032618 { set; get; }
        public string cResvTxt12 { set; get; }
        public string nRate { set; get; }
        public string nBasePrm { set; get; }
        public string nBefPrm { set; get; }
        public string nDductRate { set; get; }
        public string nPrm { set; get; }
        public string nAllPrm { set; get; }
        public string nDeductibleRate { set; get; }
        public string nResvNum13 { set; get; }
        public string nPureRiskPremiumFlag { set; get; }
        public string nDductPrm { set; get; }
        public string nBasePurePrm { set; get; }


        public CvrgVO()
        {
            cCvrgNo = "";//险别代码
            nAmt = "";//保额
            cDductMrk = "";//是否不计免赔
            nLiabDaysLmt = "";//数量（人数，天数）
            nPerAmt = "";//保额(每人,每天)
            cCustCvrgNme = "";
            cGlassTyp = "";
            nrate032618 = "";
            cResvTxt12 = "";
            nRate = "";
            nBasePrm = "";
            nBefPrm = "";
            nDductRate = "";
            nPrm = "";
            nAllPrm = "";
            nDeductibleRate = "";
            nResvNum13 = "";
            nPureRiskPremiumFlag = "";
            nDductPrm = "";
            nBasePurePrm = "";
        }
        public CvrgVO(string strcCvrgNo, string strnAmt, string strcDductMrk, string strnLiabDaysLmt, string strnPerAmt)
        {
            cCvrgNo = strcCvrgNo;
            nAmt = strnAmt;
            cDductMrk = strcDductMrk;
            nLiabDaysLmt = strnLiabDaysLmt;
            nPerAmt = strnPerAmt;
        }
    }
}
