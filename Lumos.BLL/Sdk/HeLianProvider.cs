using HeLianSdk;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL
{

    public class LllegalQueryRecord
    {
        public string CarNo { get; set; }
        public string CarType { get; set; }
        public string RackNo { get; set; }
        public string EnginNo { get; set; }
        public string IsCompany { get; set; }
    }

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
    }

    public class LllegalRecord
    {
        public string carNo { get; set; }

        public string bookNo { get; set; }
        public string bookType { get; set; }

        public string bookTypeName { get; set; }

        public string lllegalCode { get; set; }
        public string cityCode { get; set; }
        public string lllegalTime { get; set; }
        public string point { get; set; }
        public string offerType { get; set; }
        public string ofserTypeName { get; set; }
        public string fine { get; set; }
        public string serviceFee { get; set; }
        public string late_fees { get; set; }
        public string content { get; set; }
        public string lllegalDesc { get; set; }
        public string lllegalCity { get; set; }
        public string address { get; set; }

        public bool needDealt { get; set; }
    }

    public class HeLianProvider : BaseProvider
    {
        public CustomJsonResult<List<LllegalRecord>> Query(int operater, LllegalQueryParams pms)
        {
            CustomJsonResult<List<LllegalRecord>> result = new CustomJsonResult<List<LllegalRecord>>();

            using (TransactionScope ts = new TransactionScope())
            {
                if (pms.EnginNo.Length < 6)
                {
                    return new CustomJsonResult<List<LllegalRecord>>(ResultType.Failure, ResultCode.Failure, "请输入发动机号后6位", null);
                }

                if (pms.RackNo.Length < 6)
                {
                    return new CustomJsonResult<List<LllegalRecord>>(ResultType.Failure, ResultCode.Failure, "请输入车架号后6位", null);
                }

                var lllegalQueryScore = CurrentDb.LllegalQueryScore.Where(m => m.UserId == pms.UserId && m.MerchantId == pms.MerchantId).FirstOrDefault();

                if (lllegalQueryScore.Score == 0)
                {
                    return new CustomJsonResult<List<LllegalRecord>>(ResultType.Failure, ResultCode.Failure, "当前的查询积分为0，请充值积分", null);
                }


                List<DataLllegal> dataLllegal = new List<DataLllegal>();


                CarQueryDataList_Params p = new CarQueryDataList_Params();
                p.carNo = pms.CarNo;
                p.enginNo = pms.EnginNo;
                p.rackNo = pms.RackNo;
                p.isCompany = pms.IsCompany == true ? "true" : "false";
                p.carType = pms.CarType;
                var api_result = HeLianApi.CarQueryDataList(p);


                if (api_result.resultCode != "0")
                {
                    return new CustomJsonResult<List<LllegalRecord>>(ResultType.Failure, ResultCode.Failure, api_result.resultMsg, null);
                }

                if (api_result.data == null || api_result.data.Count == 0)
                {
                    return new CustomJsonResult<List<LllegalRecord>>(ResultType.Failure, ResultCode.Failure, api_result.resultMsg, null);
                }

                List<LllegalRecord> lllegalRecords = new List<LllegalRecord>();

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

                    LllegalRecord lllegalRecord = new LllegalRecord();
                    lllegalRecord.carNo = pms.CarNo;
                    lllegalRecord.bookNo = m.bookNo;
                    lllegalRecord.bookType = m.bookType;
                    lllegalRecord.bookTypeName = GetBookTypeName(m.bookType);
                    lllegalRecord.offerType = m.offerType;
                    lllegalRecord.ofserTypeName = GetOfferTypeName(m.offerType);
                    lllegalRecord.lllegalDesc = m.lllegalDesc;
                    lllegalRecord.cityCode = m.cityCode;
                    lllegalRecord.lllegalCode = m.lllegalCode;
                    lllegalRecord.lllegalCity = m.lllegalCity;
                    lllegalRecord.lllegalTime = m.lllegalTime;
                    lllegalRecord.point = m.point;
                    lllegalRecord.fine = m.fine;
                    lllegalRecord.address = m.address;
                    lllegalRecord.content = m.content;
                    lllegalRecord.serviceFee = m.serviceFee;
                    lllegalRecord.fine = m.fine;
                    lllegalRecord.late_fees = m.late_fees;

                    lllegalRecords.Add(lllegalRecord);

                }

                CarQueryGetLllegalPrice_Params p1 = new CarQueryGetLllegalPrice_Params();

                p1.carNo = pms.CarNo;
                p1.enginNo = pms.EnginNo;
                p1.rackNo = pms.RackNo;
                p1.isCompany = pms.IsCompany == true ? "true" : "false";
                p1.carType = pms.CarType;
                p1.dataLllegal = dataLllegal;

                var api_result1 = HeLianApi.CarQueryGetLllegalPrice(p1);

                if (api_result1.resultCode != "0")
                {
                    return new CustomJsonResult<List<LllegalRecord>>(ResultType.Failure, ResultCode.Failure, api_result1.resultMsg, null);
                }

                if (api_result1.data == null || api_result1.data.Count == 0)
                {
                    return new CustomJsonResult<List<LllegalRecord>>(ResultType.Failure, ResultCode.Failure, api_result1.resultMsg, null);
                }

                var d1 = api_result1.data;
                foreach (var record in lllegalRecords)
                {
                    var priceresult = d1.Where(m => m.bookNo == record.bookNo).FirstOrDefault();
                    if (priceresult != null)
                    {
                        record.serviceFee = priceresult.serviceFee;
                        record.fine = priceresult.fine;
                        record.late_fees = priceresult.fine;
                    }
                }


                var lllegalQueryLog = new LllegalQueryLog();

                lllegalQueryLog.UserId = pms.UserId;
                lllegalQueryLog.MerchantId = pms.MerchantId;
                lllegalQueryLog.CarNo = pms.CarNo;
                lllegalQueryLog.EnginNo = pms.EnginNo;
                lllegalQueryLog.RackNo = pms.RackNo;
                lllegalQueryLog.IsCompany = (pms.IsCompany == true ? "true" : "false");
                lllegalQueryLog.CarType = pms.CarType;
                lllegalQueryLog.Result = Newtonsoft.Json.JsonConvert.SerializeObject(lllegalRecords);
                lllegalQueryLog.Creator = operater;
                lllegalQueryLog.CreateTime = this.DateTime;
                CurrentDb.LllegalQueryLog.Add(lllegalQueryLog);

                var changeScore = -1;
                lllegalQueryScore.Score += changeScore;
                lllegalQueryScore.Mender = operater;
                lllegalQueryScore.LastUpdateTime = this.DateTime;

                var lllegalQueryScoreTrans = new LllegalQueryScoreTrans();
                lllegalQueryScoreTrans.UserId = pms.UserId;
                lllegalQueryScoreTrans.ChangeScore = changeScore;
                lllegalQueryScoreTrans.Score = lllegalQueryScore.Score;
                lllegalQueryScoreTrans.Type = Enumeration.LllegalQueryScoreTransType.DecreaseByQuery;
                lllegalQueryScoreTrans.Description = string.Format("查询违章扣除积分:{0}", changeScore);
                lllegalQueryScoreTrans.Creator = operater;
                lllegalQueryScoreTrans.CreateTime = this.DateTime;
                CurrentDb.LllegalQueryScoreTrans.Add(lllegalQueryScoreTrans);
                CurrentDb.SaveChanges();
                lllegalQueryScoreTrans.Sn = Sn.Build(SnType.LllegalQueryScoreTrans, lllegalQueryScoreTrans.Id).Sn;
                CurrentDb.SaveChanges();

                ts.Complete();

                result = new CustomJsonResult<List<LllegalRecord>>(ResultType.Success, ResultCode.Success, api_result1.resultMsg, lllegalRecords);
            }

            return result;
        }

        public CustomJsonResult<List<LllegalQueryRecord>> QueryLog(int operater, int userId, int merchantId, int posMachineId)
        {
            CustomJsonResult<List<LllegalQueryRecord>> result = new CustomJsonResult<List<LllegalQueryRecord>>();

            using (TransactionScope ts = new TransactionScope())
            {

                var list = (from u in CurrentDb.LllegalQueryLog
                            where u.UserId==userId
                            select new { u.CarNo, u.CarType, u.EnginNo, u.RackNo, u.IsCompany }).Distinct();

                List<LllegalQueryRecord> logs = new List<LllegalQueryRecord>();
                foreach (var m in list)
                {
                    var log = new LllegalQueryRecord();

                    log.CarNo = m.CarNo;
                    log.CarType = m.CarType;
                    log.EnginNo = m.EnginNo;
                    log.RackNo = m.RackNo;
                    log.IsCompany = m.IsCompany;

                    logs.Add(log);
                }


                result = new CustomJsonResult<List<LllegalQueryRecord>>(ResultType.Success, ResultCode.Success, "", logs);
            }

            return result;
        }

        public string GetOfferTypeName(string offerType)
        {
            string s = "";
            switch (offerType)
            {
                case "0":
                    s = "无法处理";
                    break;
                case "1":
                    s = "联网单";
                    break;
                case "2":
                    s = "当地单";
                    break;
                case "3":
                    s = "扣分单";
                    break;
                case "4":
                    s = "行政处罚";
                    break;
                case "5":
                    s = "公司单";
                    break;
                case "6":
                    s = "特殊扣分单";
                    break;
                case "7":
                    s = "特殊当地单";
                    break;
                case "8":
                    s = "现场当地单";
                    break;
            }
            return s;
        }

        public string GetBookTypeName(string bookType)
        {
            string s = "";
            switch (bookType)
            {
                case "1001":
                    s = "现场（未交款）数";
                    break;
                case "6001A":
                    s = "非现场未处理违章数据";
                    break;
                case "6001B":
                    s = "非现场已处理未交款数据";
                    break;
            }
            return s;
        }
    }
}
