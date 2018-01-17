using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class CarModelQueryRequestMain
    {
        public Channel Channel { set; get; }

        public string requestId { set; get; }//请求标识
        public string productRequestType { set; get; }//请求类型D-直接查找  F-模糊查找
        public string serviceType { set; get; }//业务类型,业务类型： C-乘用车  A-传统车型
        public string pagingFlag { set; get; }//分页类型 ：T-分页   F-不分页 
        public string pageNo { set; get; }//页码
        public string pageSize { set; get; }//每页显示数量
        public string vehicleName { set; get; }//车型名
        public string brandId { set; get; }//品牌ID
        public string familyId { set; get; }//车系ID
        public string gearboxType { set; get; }//驱动型式ID
        public string engineDesc { set; get; }//发动机描述ID

        
        public CarModelQueryRequestMain()
        {
            Channel = new Channel();
            requestId = "VEH_02";
            productRequestType = "F";
            serviceType = "C";
            pagingFlag = "F";
            pageNo = "";
            pageSize = "";
            vehicleName = "";
            brandId = "";
            familyId = "";
            gearboxType = "";
            engineDesc = "";


        }

    }
}
