using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class  CarQuotePriceRequest
    {
        public RequestHead RequestHead { set; get; }
        public CarQuotePriceRequestMain CarQuotePriceRequestMain { set; get; }
        public  CarQuotePriceRequest()
        {
            DateTime today = DateTime.Now;
            RequestHead = new RequestHead();
            CarQuotePriceRequestMain = new CarQuotePriceRequestMain();

            RequestHead.tradeTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            RequestHead.request_Type = "01";

            CarQuotePriceRequestMain.Channel.channelCode = "AEC16110001";
            CarQuotePriceRequestMain.Channel.channelTradeCode = "000002";
            CarQuotePriceRequestMain.Channel.channelRelationNo = RequestHead.tradeTime;
            CarQuotePriceRequestMain.Channel.channelTradeDate = RequestHead.tradeTime.Substring(0, 8);

            CarQuotePriceRequestMain.BaseVO.cprovince = "440000";//省
            CarQuotePriceRequestMain.BaseVO.cAreaFlag = "441900";//市
            CarQuotePriceRequestMain.BaseVO.cCountryFlag = "999999";//区

            CarQuotePriceRequestMain.VhlownerVO.cCertfCls = "120001";//居民身份证120001

            CarQuotePriceRequestMain.PackageJQVO.cProdNo = "033011";
            CarQuotePriceRequestMain.PackageJQVO.tInsrncBgnTm = today.ToString("yyyy-MM-dd 00:00:00");
            CarQuotePriceRequestMain.PackageJQVO.TInsrncEndTm = today.AddYears(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
            CarQuotePriceRequestMain.PackageJQVO.VhlVO.cProdPlace = "1";//0进口/1国产 
            CarQuotePriceRequestMain.PackageJQVO.VhlVO.cUsageCde = "364111001";//家庭自用汽车
            CarQuotePriceRequestMain.PackageJQVO.VhlVO.cVhlTyp = "365001";//6座以下客车
            CarQuotePriceRequestMain.PackageJQVO.VhlVO.cTravelAreaCde = "0";//全国
            CarQuotePriceRequestMain.PackageJQVO.VhlVO.cGlassTyp = "2";//2国产玻璃
            CarQuotePriceRequestMain.PackageJQVO.VhlVO.cDevice1Mrk = "0";//是否过户（1-是 0-否）
            CarQuotePriceRequestMain.PackageJQVO.VhlVO.cPlateTyp = "02";//号牌种类(02小型汽车号牌)
            CarQuotePriceRequestMain.PackageJQVO.VhlVO.cRegVhlTyp = "K33";//交管车辆类型(K33轿车)
            CarQuotePriceRequestMain.PackageJQVO.VhlVO.cEcdemicMrk = "0";//是否外地车(1-是 0-否)
            CarQuotePriceRequestMain.PackageJQVO.VhlVO.cRiskDesc = "T001";//特异车型标识(T001常用车型)
            CarQuotePriceRequestMain.PackageJQVO.VhlVO.cFleetMrk = "0";//是否车队车辆(1-是 0-否)

            CarQuotePriceRequestMain.PackageSYVO.cProdNo = "033011";
            CarQuotePriceRequestMain.PackageSYVO.tInsrncBgnTm = today.ToString("yyyy-MM-dd 00:00:00");
            CarQuotePriceRequestMain.PackageSYVO.TInsrncEndTm = today.AddYears(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
            CarQuotePriceRequestMain.PackageSYVO.VhlVO.cProdPlace = "1";//0进口/1国产 
            CarQuotePriceRequestMain.PackageSYVO.VhlVO.cUsageCde = "364111001";//家庭自用汽车
            CarQuotePriceRequestMain.PackageSYVO.VhlVO.cVhlTyp = "365001";//6座以下客车
            CarQuotePriceRequestMain.PackageSYVO.VhlVO.cTravelAreaCde = "0";//全国
            CarQuotePriceRequestMain.PackageSYVO.VhlVO.cGlassTyp = "2";//2国产玻璃
            CarQuotePriceRequestMain.PackageSYVO.VhlVO.cDevice1Mrk = "0";//是否过户（1-是 0-否）
            CarQuotePriceRequestMain.PackageSYVO.VhlVO.cPlateTyp = "02";//号牌种类(02小型汽车号牌)
            CarQuotePriceRequestMain.PackageSYVO.VhlVO.cRegVhlTyp = "K33";//交管车辆类型(K33轿车)
            CarQuotePriceRequestMain.PackageSYVO.VhlVO.cEcdemicMrk = "0";//是否外地车(1-是 0-否)
            CarQuotePriceRequestMain.PackageSYVO.VhlVO.cRiskDesc = "T001";//特异车型标识(T001常用车型)
            CarQuotePriceRequestMain.PackageSYVO.VhlVO.cFleetMrk = "0";//是否车队车辆(1-是 0-否)



        }

    }
}
