using HeLianSdk;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL
{
    public class LllegalQueryParams
    {
        public int UserId { get; set; }
        public int MerchantId { get; set; }
        public int PosMachineId { get; set; }
        public string CarNo { get; set; }
        public string CarType { get; set; }
        public string RackNo { get; set; }
        public string EnginNo { get; set; }
        public bool IsCompany { get; set; }
        //public List<DataLllegal> DataLllegal { get; set; }
        public bool IsOffer { get; set; }
    }

    public class HeLianProvider : BaseProvider
    {
        public CustomJsonResult Query(int operater, LllegalQueryParams pms)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                if (pms.EnginNo.Length < 6)
                {
                    return new CustomJsonResult(ResultType.Failure, "请输入发动机号后6位");
                }

                if (pms.RackNo.Length < 6)
                {
                    return new CustomJsonResult(ResultType.Failure, "请输入车架号后6位");
                }

                CarQueryDataList_Params p = new CarQueryDataList_Params();
                p.carNo = pms.CarNo;
                p.enginNo = pms.EnginNo;
                p.rackNo = pms.RackNo;
                p.isCompany = pms.IsCompany == true ? "true" : "false";
                p.carType = pms.CarType;
                var api_result = HeLianApi.CarQueryDataList(p);

                if (api_result.data == null)
                {
                    return new CustomJsonResult(ResultType.Failure, api_result.resultMsg);
                }

                if (pms.IsCompany)
                {
                    CarQueryGetLllegalPrice_Params p1 = new CarQueryGetLllegalPrice_Params();

                    p1.carNo = pms.CarNo;
                    p1.enginNo = pms.EnginNo;
                    p1.rackNo = pms.RackNo;
                    p1.isCompany = pms.IsCompany == true ? "true" : "false";
                    p1.carType = pms.CarType;

                    List<DataLllegal> r = new List<DataLllegal>();

                    var d = api_result.data;
                    foreach (var m in d)
                    {
                        DataLllegal dl = new DataLllegal();
                        dl.bookNo = m.bookNo;
                        dl.bookType = m.bookType;
                        dl.cityCode = m.cityCode;
                        dl.lllegalCode = m.lllegalCode;
                        dl.lllegalTime = m.lllegalTime;
                        dl.point = m.point;
                        dl.fine = m.fine;
                        dl.lllegalAddress = m.address;
                        r.Add(dl);
                    }

                    p1.dataLllegal = r;

                    var api_result1 = HeLianApi.CarQueryGetLllegalPrice(p1);
                }



            }

            return result;
        }
    }
}
