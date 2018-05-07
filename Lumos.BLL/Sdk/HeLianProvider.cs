using HeLianSdk;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
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
        public string IsCompany { get; set; }

        public string IsOfferPrice { get; set; }
    }

    public class LllegalRecord
    {
        public string bookNo { get; set; }
        public string bookType { get; set; }

        public string bookTypeName { get; set; }

        public string lllegalCode { get; set; }
        public string cityCode { get; set; }
        public string lllegalTime { get; set; }
        public int point { get; set; }
        public string offerType { get; set; }
        public string ofserTypeName { get; set; }
        public decimal fine { get; set; }
        public decimal serviceFee { get; set; }
        public decimal late_fees { get; set; }
        public string content { get; set; }
        public string lllegalDesc { get; set; }
        public string lllegalCity { get; set; }
        public string address { get; set; }

        public bool needDealt { get; set; }

        public string status { get; set; }

        public bool canDealt { get; set; }

        public bool canUrgent { get; set; }

        public decimal urgentFee { get; set; }

        public bool needUrgent { get; set; }

    }


    public class LllegalQueryResult
    {
        public string DealtTip { get; set; }

        public int QueryScore { get; set; }

        public string CarNo { get; set; }

        public string SumCount { get; set; }

        public string SumPoint { get; set; }

        public string SumFine { get; set; }

        public List<LllegalRecord> Record { get; set; }

        public bool IsOfferPrice { get; set; }
    }

    public class ServiceFeeModel
    {
        public decimal ServiceFee { get; set; }

        public decimal UrgentFee { get; set; }

        public string Version { get; set; }

        public bool CanUrgentFee { get; set; }

        public bool CanDealt { get; set; }
    }

    public class HeLianProvider : BaseProvider
    {


        public static ServiceFeeModel CalServiceFee(string offerType, decimal origServiceFee, decimal point)
        {
            ServiceFeeModel feeModel = new ServiceFeeModel();
            decimal addfee = 0;
            ////处理服务费规则
            //switch (offerType)
            //{
            //    case "0"://无法处理
            //        feeModel.CanDealt = false;
            //        break;
            //    case "1"://联网单
            //        feeModel.CanDealt = true;
            //        feeModel.CanUrgentFee = false;
            //        feeModel.UrgentFee = 5;
            //        break;
            //    case "2"://当地单
            //    case "7":
            //    case "8":
            //        feeModel.CanDealt = true;
            //        feeModel.CanUrgentFee = true;
            //        addfee = 3;
            //        feeModel.ServiceFee = origServiceFee + addfee;//非扣分单
            //        break;
            //    case "3"://扣分单
            //    case "6":
            //        feeModel.CanDealt = true;
            //        feeModel.CanUrgentFee = false;
            //        addfee = point * 5;
            //        feeModel.ServiceFee = origServiceFee + addfee;
            //        break;
            //    case "4"://行政处罚
            //        feeModel.CanDealt = true;
            //        feeModel.CanUrgentFee = false;
            //        addfee = 300;
            //        feeModel.ServiceFee = origServiceFee + addfee;
            //        break;
            //}



            if (point == 0)
            {
                addfee = 3;
                feeModel.ServiceFee = origServiceFee + addfee;//非扣分单
                feeModel.CanDealt = true;
            }
            else
            {
                addfee = point * 5;
                feeModel.ServiceFee = origServiceFee + addfee;
                feeModel.CanDealt = false;
            }

            return feeModel;
        }


        public CustomJsonResult<LllegalQueryResult> Query(int operater, LllegalQueryParams pms)
        {
            CustomJsonResult<LllegalQueryResult> result = new CustomJsonResult<LllegalQueryResult>();

            using (TransactionScope ts = new TransactionScope())
            {

                LllegalQueryResult queryResult = null;

                if (BizFactory.AppSettings.IsTest)
                {
                    #region  测试
                    string strout = "";

                    string strfile = "Test_Lllegal.txt";


                    using (StreamReader sr = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(strfile), System.Text.Encoding.Default))
                    {
                        strout = sr.ReadToEnd();
                    }

                    var t_lllegalRecords = new List<LllegalRecord>();

                    if (!string.IsNullOrEmpty(strout))
                    {
                        t_lllegalRecords = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LllegalRecord>>(strout);
                    }

                    queryResult = new LllegalQueryResult();

                    queryResult.IsOfferPrice = pms.IsOfferPrice == "true" ? true : false;
                    queryResult.CarNo = pms.CarNo;





                    foreach (var record in t_lllegalRecords)
                    {

                        var serviceFeeModel = CalServiceFee(record.offerType, record.serviceFee, record.point);

                        record.serviceFee = serviceFeeModel.ServiceFee;
                        record.urgentFee = serviceFeeModel.UrgentFee;
                        record.canUrgent = serviceFeeModel.CanUrgentFee;
                        record.canDealt = serviceFeeModel.CanDealt;
                        record.status = "待处理";


                        var details = CurrentDb.OrderToLllegalDealtDetails.Where(m => m.BookNo == record.bookNo).ToList();
                        if (details != null)
                        {
                            var hasDealt = details.Where(m => m.Status == Enumeration.OrderToLllegalDealtDetailsStatus.InDealt).Count();
                            var hasCompleted = details.Where(m => m.Status == Enumeration.OrderToLllegalDealtDetailsStatus.Completed).Count();


                            if (hasDealt > 0)
                            {
                                record.status = "处理中";
                                record.canDealt = false;
                            }

                            if (hasCompleted > 0)
                            {
                                record.status = "完成";
                                record.canDealt = false;
                            }

                        }
                    }
                    queryResult.Record = t_lllegalRecords;
                    queryResult.DealtTip = "扣分单需处理，请咨询客服";
                    queryResult.SumCount = t_lllegalRecords.Count().ToString();
                    queryResult.SumFine = t_lllegalRecords.Sum(m => m.fine).ToString();
                    queryResult.SumPoint = t_lllegalRecords.Sum(m => m.point).ToString();


                    return new CustomJsonResult<LllegalQueryResult>(ResultType.Success, ResultCode.Success, "查询成功", queryResult);

                    #endregion
                }

                if (pms.EnginNo.Length < 6)
                {
                    return new CustomJsonResult<LllegalQueryResult>(ResultType.Failure, ResultCode.Failure, "请输入发动机号后6位", null);
                }

                if (pms.RackNo.Length < 6)
                {
                    return new CustomJsonResult<LllegalQueryResult>(ResultType.Failure, ResultCode.Failure, "请输入车架号后6位", null);
                }

                var lllegalQueryScore = CurrentDb.LllegalQueryScore.Where(m => m.UserId == pms.UserId && m.MerchantId == pms.MerchantId).FirstOrDefault();

                if (lllegalQueryScore.Score == 0)
                {
                    return new CustomJsonResult<LllegalQueryResult>(ResultType.Failure, ResultCode.LllegalQueryNotEnoughScore, "当前的查询积分为0，请充值积分", null);
                }

                pms.CarNo = pms.CarNo.ToUpper();
                pms.EnginNo = pms.EnginNo.ToUpper();
                pms.RackNo = pms.RackNo.ToUpper();

                List<DataLllegal> dataLllegal = new List<DataLllegal>();


                CarQueryDataList_Params p = new CarQueryDataList_Params();
                p.carNo = pms.CarNo;
                p.enginNo = pms.EnginNo;
                p.rackNo = pms.RackNo;
                p.isCompany = pms.IsCompany;
                p.carType = pms.CarType;
                var api_result = HeLianApi.CarQueryDataList(p);


                if (api_result.resultCode != "0")
                {
                    return new CustomJsonResult<LllegalQueryResult>(ResultType.Failure, ResultCode.Failure, api_result.resultMsg, null);
                }

                List<LllegalRecord> lllegalRecords = new List<LllegalRecord>();


                var d = api_result.data;
                if (d != null)
                {
                    foreach (var m in d)
                    {
                        LllegalRecord lllegalRecord = new LllegalRecord();
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
                        lllegalRecord.late_fees = m.late_fees;
                        lllegalRecord.address = m.address;
                        lllegalRecord.content = m.content;
                        lllegalRecords.Add(lllegalRecord);

                    }
                }

                queryResult = new LllegalQueryResult();

                queryResult.IsOfferPrice = pms.IsOfferPrice == "true" ? true : false;
                queryResult.CarNo = pms.CarNo;

                string msg = "";
                bool isFlag = true;
                if (queryResult.IsOfferPrice)
                {
                    if (api_result.data == null)
                    {
                        msg = api_result.resultMsg;
                    }
                    else if (api_result.data.Count == 0)
                    {
                        msg = api_result.resultMsg;
                    }
                    else
                    {
                        CarQueryGetLllegalPrice_Params p1 = new CarQueryGetLllegalPrice_Params();

                        p1.carNo = pms.CarNo;
                        p1.enginNo = pms.EnginNo;
                        p1.rackNo = pms.RackNo;
                        p1.isCompany = pms.IsCompany;
                        p1.carType = pms.CarType;

                        foreach (var record in lllegalRecords)
                        {
                            DataLllegal dl = new DataLllegal();
                            dl.bookNo = record.bookNo;
                            dl.bookType = record.bookType;
                            dl.cityCode = record.cityCode;
                            dl.lllegalCode = record.lllegalCode;
                            dl.lllegalTime = record.lllegalTime;
                            dl.point = record.point;
                            dl.fine = record.fine;
                            dl.lllegalAddress = record.address;
                            dataLllegal.Add(dl);
                        }

                        p1.dataLllegal = dataLllegal;

                        var api_result1 = HeLianApi.CarQueryGetLllegalPrice(p1);

                        if (api_result1.resultCode != "0")
                        {
                            isFlag = false;
                            msg = api_result1.resultMsg;
                            //return new CustomJsonResult<LllegalQueryResult>(ResultType.Failure, ResultCode.Failure, api_result1.resultMsg, null);
                        }

                        if (isFlag)
                        {
                            msg = api_result1.resultMsg;

                            var d1 = api_result1.data;

                            if (d1 != null)
                            {

                                foreach (var record in lllegalRecords)
                                {
                                    var priceresult = d1.Where(m => m.bookNo == record.bookNo).FirstOrDefault();
                                    if (priceresult != null)
                                    {
                                        record.fine = priceresult.fine;
                                        record.late_fees = priceresult.late_fees;
                                        record.serviceFee = priceresult.serviceFee;
                                        record.point = priceresult.point;
                                    }

                                    var serviceFeeModel = CalServiceFee(record.offerType, record.serviceFee, record.point);


                                    record.serviceFee = serviceFeeModel.ServiceFee;
                                    record.urgentFee = serviceFeeModel.UrgentFee;
                                    record.canUrgent = serviceFeeModel.CanUrgentFee;
                                    record.canDealt = serviceFeeModel.CanDealt;
                                    record.status = "待处理";


                                    var details = CurrentDb.OrderToLllegalDealtDetails.Where(m => m.BookNo == record.bookNo).ToList();
                                    if (details != null)
                                    {
                                        var hasDealt = details.Where(m => m.Status == Enumeration.OrderToLllegalDealtDetailsStatus.InDealt).Count();
                                        var hasCompleted = details.Where(m => m.Status == Enumeration.OrderToLllegalDealtDetailsStatus.Completed).Count();

                                        if (hasDealt > 0)
                                        {
                                            record.status = "处理中";
                                            record.canDealt = false;
                                        }

                                        if (hasCompleted > 0)
                                        {
                                            record.status = "完成";
                                            record.canDealt = false;
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    msg = api_result.resultMsg;
                }

                queryResult.DealtTip = "扣分单需处理，请咨询客服";
                queryResult.SumCount = lllegalRecords.Count().ToString();
                queryResult.SumFine = lllegalRecords.Sum(m => m.fine).ToString();
                queryResult.SumPoint = lllegalRecords.Sum(m => m.point).ToString();


                var lllegalQueryLog = new LllegalQueryLog();

                lllegalQueryLog.UserId = pms.UserId;
                lllegalQueryLog.MerchantId = pms.MerchantId;
                lllegalQueryLog.CarNo = pms.CarNo;
                lllegalQueryLog.EnginNo = pms.EnginNo;
                lllegalQueryLog.RackNo = pms.RackNo;
                lllegalQueryLog.IsCompany = pms.IsCompany;
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

                queryResult.QueryScore = lllegalQueryScore.Score;

                queryResult.Record = lllegalRecords;


                result = new CustomJsonResult<LllegalQueryResult>(ResultType.Success, ResultCode.Success, msg, queryResult);
            }

            return result;
        }

        public CustomJsonResult<List<LllegalQueryRecord>> QueryLog(int operater, int userId, int merchantId, int posMachineId)
        {
            CustomJsonResult<List<LllegalQueryRecord>> result = new CustomJsonResult<List<LllegalQueryRecord>>();

            using (TransactionScope ts = new TransactionScope())
            {

                var list = (from u in CurrentDb.LllegalQueryLog
                            where u.UserId == userId
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
