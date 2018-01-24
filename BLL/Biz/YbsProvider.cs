using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class YbsProvider
    {
        //todo 正式环境要配置
        public YbsMerchantModel GetCarClaimMerchantInfo()
        {
            YbsMerchantModel model = new YbsMerchantModel();
            model.merchant_id = "861440348161109";
            model.ybs_mer_code = "";
            model.biz_code = "000002";
            model.merchant_name = "";
            return model;
        }

        //todo 正式环境要配置
        public YbsMerchantModel GetDepositRentMerchantInfo()
        {
            YbsMerchantModel model = new YbsMerchantModel();
            model.merchant_id = "861440348161109";
            model.ybs_mer_code = "";
            model.biz_code = "000001";
            model.merchant_name = "";
            return model;
        }

        public YbsMerchantModel GetCarInsureMerchantInfo(int insuranceCompanyId, string merchant_id, string ybs_mer_code, string merchant_name, string biz_code)
        {
            YbsMerchantModel model = new YbsMerchantModel();
            model.merchant_id = merchant_id;
            model.ybs_mer_code = ybs_mer_code;
            model.merchant_name = merchant_name;
            model.biz_code = biz_code;
            return model;
        }
    }
}
