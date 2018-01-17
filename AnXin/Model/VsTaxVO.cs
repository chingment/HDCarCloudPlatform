using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class VsTaxVO
    {
        public string tTaxEffBgnTm { set; get; }
        public string tTaxEffEndTm { set; get; }
        public string tCertificateDate { set; get; }
        public string cDepartmentNonLocal { set; get; }
        public string nAnnUnitTaxAmt { set; get; }
        public string nTaxableAmt { set; get; }
        public string nLastYear { set; get; }
        public string nOverdueAmt { set; get; }
        public string cTaxUnit { set; get; }
        public string nAggTax { set; get; }
        public string cTaxdptVhltyp { set; get; }
        public string cPaytaxTyp { set; get; }
        public string cVsTaxMrk { set; get; }
        public string cTaxableTyp { set; get; }
        public string cTaxPaymentRecptNo { set; get; }
        public VsTaxVO()
        {
            tTaxEffBgnTm = "";
            tTaxEffEndTm = "";
            tCertificateDate = "";
            cDepartmentNonLocal = "";
            nAnnUnitTaxAmt = "";
            nTaxableAmt = "";
            nLastYear = "";
            nOverdueAmt = "";
            cTaxUnit = "";
            nAggTax = "";
            cTaxdptVhltyp = "";
            cPaytaxTyp = "";
            cVsTaxMrk = "";
            cTaxableTyp = "";
            cTaxPaymentRecptNo = "";
        }
    }
}
