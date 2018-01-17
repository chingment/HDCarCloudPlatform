using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class VhlVO
    {
        public string cProdPlace { get; set; }//进口/国产 
        public string cPlateNo { get; set; }//车牌号码
        public string cEngNo { get; set; }//发动机号
        public string cFrmNo { get; set; }//车架号
        public string cModelCde { get; set; }//车辆型号
        public string cUsageCde { get; set; }//使用性质
        public string cVhlTyp { get; set; }//车辆种类
        public string cFstRegYm { get; set; }//初登日期
        public string cTravelAreaCde { get; set; }//行驶区域
        public string cGlassTyp { get; set; }//玻璃类型
        public string cDevice1Mrk { get; set; }//是否过户
        public string cPlateTyp { get; set; }//号牌种类
        public string cRegVhlTyp { get; set; }//交管车辆类型
        public string cEcdemicMrk { get; set; }//是否外地车
        public string cRiskDesc { get; set; }//特异车型标识
        public string cFleetMrk { get; set; }//车队标志
        public string cModelNme { get; set; }
        public string nNewPurchaseValue { get; set; }
        public string nSeatNum { get; set; }
        public string nTonage { get; set; }
        public string cDisplacementLvl { get; set; }
        public string nDisplacement { get; set; }
        public string nActualValue { get; set; }
        public string cTfiSpecialMrk { get; set; }
        public string cBrandId { get; set; }
        public string cAliasId { get; set; }
        public string tTransferDate { get; set; }
        public string cPlateBrandId { get; set; }
        public string cPlateModelCde { get; set; }
        public string nDespRate { get; set; }
        public string cVhlPkgNO { get; set; }
        public string cNewVhlFlag { get; set; }
        public string nPoWeight { get; set; }
        public string cFuelType { get; set; }
        public string tCertificateDate { get; set; }
        public string cQryCde { get; set; }
        public string cProconfirmNo { get; set; }
        public string cRenewalFlag { get; set; }
        public string cInspectorNme { get; set; }
        public string cInspectionCde { get; set; }
        public string cInspectRec { get; set; }
        public string cLoanVehicleFlag { get; set; }
        public string nResvNum2 { get; set; }
        public string cUseYear { get; set; }
        public string nNewPurchaseRate { get; set; }
        public string cFamilyNme { get; set; }
        public string cCertificateType { get; set; }
        public string cCertificateNo { get; set; }
        public string nNoDamageYears { get; set; }
        public string cVhlInsuredRel { get; set; }
        public string cPlateRecde { get; set; }
        public string cNoticeType { get; set; }
        public string cTradeName { get; set; }
        public string cSeriesKindName { get; set; }
        public string nNagoActualValue { get; set; }
        public string cModelLibraryTyp { get; set; }


        public VhlVO()
        {
            cProdPlace = "";
            cPlateNo = "";
            cEngNo = "";
            cFrmNo = "";
            cModelCde = "";
            cUsageCde = "";
            cVhlTyp = "";
            cFstRegYm = "";
            cTravelAreaCde = "";
            cGlassTyp = "";
            cDevice1Mrk = "";
            cPlateTyp = "";
            cRegVhlTyp = "";
            cEcdemicMrk = "";
            cRiskDesc = "";
            cFleetMrk = "";
        }
    }
}
