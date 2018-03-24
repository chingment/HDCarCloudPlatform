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
        public List<DataLllegalParams> DataLllegal { get; set; }
        public bool IsOffer { get; set; }
    }

    public class DataLllegalParams
    {
        public string bookNo { get; set; }
        public string bookType { get; set; }
        public string lllegalCode { get; set; }
        public string cityCode { get; set; }
        public string lllegalTime { get; set; }
        public string point { get; set; }
        public string fine { get; set; }
        public string lllegalAddress { get; set; }

        public bool NeedOffer { get; set; }

    }

    public class HeLianProvider : BaseProvider
    {
        public CustomJsonResult<List<CarQueryGetLllegalPrice_Result>> Query(int operater, LllegalQueryParams pms)
        {
            CustomJsonResult<List<CarQueryGetLllegalPrice_Result>> result = new CustomJsonResult<List<CarQueryGetLllegalPrice_Result>>();

            using (TransactionScope ts = new TransactionScope())
            {
                if (pms.EnginNo.Length < 6)
                {
                    return new CustomJsonResult<List<CarQueryGetLllegalPrice_Result>>(ResultType.Failure, ResultCode.Failure, "请输入发动机号后6位",null);
                }

                if (pms.RackNo.Length < 6)
                {
                    return new CustomJsonResult<List<CarQueryGetLllegalPrice_Result>>(ResultType.Failure, ResultCode.Failure, "请输入车架号后6位", null);
                }

                List<DataLllegal> dataLllegal = new List<DataLllegal>();

                if (pms.DataLllegal == null)
                {
                    if (pms.DataLllegal.Count == 0)
                    {
                        CarQueryDataList_Params p = new CarQueryDataList_Params();
                        p.carNo = pms.CarNo;
                        p.enginNo = pms.EnginNo;
                        p.rackNo = pms.RackNo;
                        p.isCompany = pms.IsCompany == true ? "true" : "false";
                        p.carType = pms.CarType;
                        var api_result = HeLianApi.CarQueryDataList(p);

                        if (api_result.data == null)
                        {
                            return new CustomJsonResult<List<CarQueryGetLllegalPrice_Result>>(ResultType.Failure, ResultCode.Failure, api_result.resultMsg, null);
                        }

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
                            dataLllegal.Add(dl);
                        }
                    }
                }
                else
                {
                    var d = pms.DataLllegal.Where(m => m.NeedOffer == true).ToList();

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
                        dl.lllegalAddress = m.lllegalAddress;
                        dataLllegal.Add(dl);
                    }
                }


                CarQueryGetLllegalPrice_Params p1 = new CarQueryGetLllegalPrice_Params();

                p1.carNo = pms.CarNo;
                p1.enginNo = pms.EnginNo;
                p1.rackNo = pms.RackNo;
                p1.isCompany = pms.IsCompany == true ? "true" : "false";
                p1.carType = pms.CarType;
                p1.dataLllegal = dataLllegal;

                var api_result1 = HeLianApi.CarQueryGetLllegalPrice(p1);

                if (api_result1.data == null)
                {
                    return new CustomJsonResult<List<CarQueryGetLllegalPrice_Result>>(ResultType.Failure, ResultCode.Failure, api_result1.resultMsg, null);
                }

                result= new CustomJsonResult<List<CarQueryGetLllegalPrice_Result>>(ResultType.Success, ResultCode.Success, api_result1.resultMsg, api_result1.data);
            }

            return result;
        }
    }
}
