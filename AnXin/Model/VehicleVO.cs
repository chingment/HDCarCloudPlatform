using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class VehicleVO
    {
        public string vehicleCode { set; get; }
        public string vehicleName { set; get; }
        public string brandName { set; get; }
        public string familyName { set; get; }
        public string purchasePrice { set; get; }
        public string purchasePriceTax { set; get; }
        public string seat { set; get; }
        public string yearPattern { set; get; }
        public string gearboxType { set; get; }
        public string engineDesc { set; get; }
        public string configName { set; get; }
        public string displacement { set; get; }
        public string vehicleImport { set; get; }
        public string marketDate { set; get; }
        public string kindredPrice { set; get; }
        public string kindredPriceTax { set; get; }
        //以上字段为车型查询输入
        //===========================================================
        //以下字段为车辆信息查询返回
        public string ownerName { set; get; }//车主姓名
        public string carNo { set; get; }//车牌号
        public string engineNo { set; get; }//发动机号
        public string vin { set; get; }//车架号
        public string modelName { set; get; }//车型
        public string registerDate { set; get; }//注册日期
        public string forceExpireDate { set; get; }//交强险到期时间 
        public string businessExpireDate { set; get; }//商业险到期时间

        public VehicleVO()
        {
            vehicleCode = "";
            vehicleName = "";
            brandName = "";
            familyName = "";
            purchasePrice = "";
            purchasePriceTax = "";
            seat = "";
            yearPattern = "";
            gearboxType = "";
            engineDesc = "";
            configName = "";
            displacement = "";
            vehicleImport = "";
            marketDate = "";
            kindredPrice = "";
            kindredPriceTax = "";
            ownerName = "";//车主姓名
            carNo = "";//车牌号
            engineNo = "";//发动机号
            vin = "";//车架号
            modelName = "";//车型
            registerDate = "";//注册日期
            forceExpireDate = "";//交强险到期时间 
            businessExpireDate = "";//商业险到期时间
        }

    }
}
