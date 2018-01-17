using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBack.Models.Biz.Withdraw;
using Lumos.Common;
using Lumos.Entity;
using Lumos.Mvc;
using Lumos.BLL;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;
using NPOI.SS.UserModel;
using System.Data;
using System.Text.RegularExpressions;

namespace WebBack.Controllers.Biz
{
    public class FeedbackErrorPoint
    {
        public FeedbackErrorPoint()
        {
            this.Point = new List<string>();
        }

        public string WithdrawSn { get; set; }

        public List<string> Point { get; set; }

    }

    public class FeedbackCheckErrorPoint
    {
        public List<FeedbackErrorPoint> ErrorPoint { get; set; }

        public FeedbackCheckErrorPoint()
        {
            this.ErrorPoint = new List<FeedbackErrorPoint>();
        }


        public void AddPoint(string withdrawSn, string point)
        {
            var u = this.ErrorPoint.Where(c => c.WithdrawSn == withdrawSn).FirstOrDefault();
            if (u == null)
            {
                u = new FeedbackErrorPoint();
                u.WithdrawSn = withdrawSn;
                u.Point.Add(point);
                this.ErrorPoint.Add(u);
            }
            else
            {
                if (!u.Point.Contains(point))
                {
                    u.Point.Add(point);
                }
            }
        }
    }

    public class WithdrawController : WebBackController
    {
        //
        // GET: /Withdraw/
        public ViewResult List()
        {
            return View();
        }

        public ViewResult Details(int id)
        {
            DetailsViewModel model = new DetailsViewModel(id);
            return View(model);
        }

        [OwnAuthorize(PermissionCode.提现截单)]
        public ViewResult CutOff()
        {
            return View();
        }

        [OwnAuthorize(PermissionCode.提现截单)]
        public ViewResult CutOffDetail()
        {
            return View();
        }

        [OwnAuthorize(PermissionCode.提现处理)]
        public ViewResult Dealt()
        {
            return View();
        }

        [OwnAuthorize(PermissionCode.提现处理)]
        public ViewResult Feedback()
        {
            return View();
        }


        public JsonResult GetList(WithdrawSearchCondition condition)
        {
            string sn = condition.Sn.ToSearchString();

            string clientCode = condition.ClientCode.ToSearchString();

            Enumeration.WithdrawStatus status = condition.Status;

            var query = (from m in CurrentDb.Withdraw
                        join u in CurrentDb.Merchant on m.MerchantId equals u.Id
                        where (clientCode.Length == 0 || u.ClientCode.Contains(clientCode)) &&
                           (sn.Length == 0 || m.Sn.Contains(sn))
                        select new { m.Id, m.Sn, u.ClientCode, m.Amount, m.AmountByAfterFee, m.Fee, m.SettlementStartTime, m.Status, m.CreateTime });



            if (status != Enumeration.WithdrawStatus.Unknow)
            {
                query = query.Where(m => m.Status == status);
            }

            int pageIndex = condition.PageIndex;
            int pageSize = 10;

            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            int total = query.Count();

           


            List<object> olist = new List<object>();

            foreach (var item in query)
            {
                olist.Add(new
                {
                    item.Id,
                    item.Sn,
                    item.ClientCode,
                    Amount = item.Amount.ToPrice(),
                    AmountByAfterFee = item.AmountByAfterFee.ToPrice(),
                    Fee = item.Fee.ToPrice(),
                    Status = item.Status.GetCnName(),
                    item.SettlementStartTime
                });
            }


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity, "");
        }


        public JsonResult GetCutOffAmountByTimePeriods()
        {
            List<CutOffAmountByTimePeriod> cutOffAmountByTimePeriods = new List<CutOffAmountByTimePeriod>();

            var withdrawList = CurrentDb.Withdraw.Where(m => m.Status == Enumeration.WithdrawStatus.SendRequest).ToList();

            if (withdrawList != null)
            {
                if (withdrawList.Count > 0)
                {
                    DateTime dateMin = (from p in withdrawList select p.SettlementStartTime).Min();
                    DateTime dateMax = (from p in withdrawList select p.SettlementStartTime).Max();

                    int intervalByMinutes = BizFactory.AppSettings.CutOffIntervalByMinutes;
                    DateTime dateInterval = dateMin;

                    if (dateMin == dateMax)
                    {
                        decimal sumAmount = 0;
                        decimal sumAmountByAfterFee = 0;

                        dateInterval = dateInterval.AddMinutes(intervalByMinutes);
                        sumAmount = (from s in withdrawList where s.SettlementStartTime >= dateMin && s.SettlementStartTime <= dateInterval select s.Amount).Sum();
                        sumAmountByAfterFee = (from s in withdrawList where s.SettlementStartTime >= dateMin && s.SettlementStartTime <= dateInterval select s.AmountByAfterFee).Sum();

                        CutOffAmountByTimePeriod cutoff = new CutOffAmountByTimePeriod();
                        cutoff.StartTime = dateMin.ToUnifiedFormatDateTime();
                        cutoff.EndTime = dateInterval.ToUnifiedFormatDateTime();
                        cutoff.Amount = sumAmount;
                        cutoff.AmountByAfterFee = sumAmountByAfterFee;
                        cutOffAmountByTimePeriods.Add(cutoff);
                    }
                    else
                    {
                        while (dateInterval < dateMax)
                        {
                            dateInterval = dateInterval.AddMinutes(intervalByMinutes);

                            decimal sumAmount = 0;
                            decimal sumAmountByAfterFee = 0;


                            sumAmount = (from s in withdrawList where s.SettlementStartTime >= dateMin && s.SettlementStartTime <= dateInterval select s.Amount).Sum();
                            sumAmountByAfterFee = (from s in withdrawList where s.SettlementStartTime >= dateMin && s.SettlementStartTime <= dateInterval select s.AmountByAfterFee).Sum();

                            CutOffAmountByTimePeriod cutoff = new CutOffAmountByTimePeriod();
                            cutoff.StartTime = dateMin.ToUnifiedFormatDateTime();
                            cutoff.EndTime = dateInterval.ToUnifiedFormatDateTime();
                            cutoff.Amount = sumAmount;
                            cutoff.AmountByAfterFee = sumAmountByAfterFee;
                            cutOffAmountByTimePeriods.Add(cutoff);

                        }
                    }
                }
            }

            return Json(ResultType.Success, cutOffAmountByTimePeriods, "");
        }

        [OwnAuthorize(PermissionCode.提现截单)]
        public JsonResult GetCutOffList(CutOffSearchCondition condition)
        {
            var list = (from u in CurrentDb.WithdrawCutOff
                        select new
                        {

                            u.Id,
                            u.BatchNo,
                            u.CutOffTime,
                            u.Amount,
                            u.AmountByAfterFee,
                            FailureCount = CurrentDb.WithdrawCutOffDetails.Where(q => q.WithdrawCutOffId == u.Id && q.WithdrawStatus == Enumeration.WithdrawStatus.Failure).Count(),
                            HandingCount = CurrentDb.WithdrawCutOffDetails.Where(q => q.WithdrawCutOffId == u.Id && q.WithdrawStatus == Enumeration.WithdrawStatus.Handing).Count(),
                            SuccessCount = CurrentDb.WithdrawCutOffDetails.Where(q => q.WithdrawCutOffId == u.Id && q.WithdrawStatus == Enumeration.WithdrawStatus.Success).Count(),
                            u.CreateTime
                        });


            int total = list.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            list = list.OrderByDescending(r => r.CutOffTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }

        [OwnAuthorize(PermissionCode.提现截单)]
        public JsonResult BuildCutOffData(DateTime startTime, DateTime endTime)
        {
            return BizFactory.Withdraw.BuildCutOffData(this.CurrentUserId, startTime, endTime);
        }

        [OwnAuthorize(PermissionCode.提现截单, PermissionCode.提现处理)]
        public JsonResult GetCutOffDetailsList(CutOffDetailsSearchCondition condition)
        {

            var list = (from u in CurrentDb.WithdrawCutOffDetails
                        join m in CurrentDb.Merchant on u.MerchantId equals m.Id

                        where u.WithdrawCutOffId == condition.WithdrawCutOffId

                        select new
                        {
                            u.Id,
                            m.ClientCode,
                            m.YYZZ_Name,
                            m.ContactName,
                            m.ContactPhoneNumber,
                            u.WithdrawBankName,
                            u.WithdrawBankAccountName,
                            u.WithdrawBankAccountNo,
                            u.WithdrawSn,
                            u.WithdrawAmount,
                            u.WithdrawFee,
                            u.WithdrawAmountByAfterFee,
                            u.WithdrawStatus,
                            u.WithdrawFailureReason,
                            u.WithdrawSettlementStartTime,
                            u.WithdrawSettlementEndTime
                        });




            int total = list.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            list = list.OrderBy(r => r.Id).Skip(pageSize * (pageIndex)).Take(pageSize);


            EnumerationRemarkConverter<Enumeration.WithdrawStatus> withdrawStatus = new EnumerationRemarkConverter<Enumeration.WithdrawStatus>();

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "", withdrawStatus);
        }


        [OwnAuthorize(PermissionCode.提现截单, PermissionCode.提现处理)]
        public FileResult DownloadCutOffDetail(int id)
        {
            var cutOff = CurrentDb.WithdrawCutOff.Where(m => m.Id == id).FirstOrDefault();


            var sheetName = cutOff.BatchNo;
            HSSFWorkbook workbook = new HSSFWorkbook();// 创建一个Excel文件  
            ISheet sheet = workbook.CreateSheet(sheetName);// 创建一个Excel的Sheet 

            IRow row0 = sheet.CreateRow(0);
            sheet.AddMergedRegion(new CellRangeAddress(0, 1, 0, 0));
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 1, 4));
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 5, 7));
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 8, 15));


            row0.CreateCell(0).SetCellValue("序号");
            row0.CreateCell(1).SetCellValue("商户信息");
            row0.CreateCell(5).SetCellValue("提现帐号信息");
            row0.CreateCell(8).SetCellValue("提现信息");


            IRow row1 = sheet.CreateRow(1);
            int cellIndex = 1;
            row1.CreateCell(cellIndex++).SetCellValue("商户代码");
            row1.CreateCell(cellIndex++).SetCellValue("商户名称 ");
            row1.CreateCell(cellIndex++).SetCellValue("联系人");
            row1.CreateCell(cellIndex++).SetCellValue("联系电话 ");
            row1.CreateCell(cellIndex++).SetCellValue("开户行");

            row1.CreateCell(cellIndex++).SetCellValue("持卡人");
            row1.CreateCell(cellIndex++).SetCellValue("开户帐号");
            row1.CreateCell(cellIndex++).SetCellValue("提现流水号");
            row1.CreateCell(cellIndex++).SetCellValue("提现金额");
            row1.CreateCell(cellIndex++).SetCellValue("手续费");

            row1.CreateCell(cellIndex++).SetCellValue("扣除手续费后的提现金额");
            row1.CreateCell(cellIndex++).SetCellValue("结算开始时间 ");
            row1.CreateCell(cellIndex++).SetCellValue("结算结束时间 ");
            row1.CreateCell(cellIndex++).SetCellValue("提现状态");
            row1.CreateCell(cellIndex++).SetCellValue("失败原因 ");


            var list = (from u in CurrentDb.WithdrawCutOffDetails
                        join m in CurrentDb.Merchant on u.MerchantId equals m.Id

                        where u.WithdrawCutOffId == id

                        select new
                        {
                            u.Id,
                            m.ClientCode,
                            m.YYZZ_Name,
                            m.ContactName,
                            m.ContactPhoneNumber,
                            u.WithdrawBankName,
                            u.WithdrawBankAccountName,
                            u.WithdrawBankAccountNo,
                            u.WithdrawSn,
                            u.WithdrawAmount,
                            u.WithdrawFee,
                            u.WithdrawAmountByAfterFee,
                            u.WithdrawStatus,
                            u.WithdrawFailureReason,
                            u.WithdrawSettlementStartTime,
                            u.WithdrawSettlementEndTime
                        }).ToList();



            for (int i = 0; i < list.Count; i++)
            {
                IRow row = new HSSFRow();
                row = sheet.CreateRow(i + 2);

                row.CreateCell(0).SetCellValue(i + 1);
                cellIndex = 1;
                row.CreateCell(cellIndex++).SetCellValue(list[i].ClientCode);
                row.CreateCell(cellIndex++).SetCellValue(list[i].YYZZ_Name);
                row.CreateCell(cellIndex++).SetCellValue(list[i].ContactName);
                row.CreateCell(cellIndex++).SetCellValue(list[i].ContactPhoneNumber);
                row.CreateCell(cellIndex++).SetCellValue(list[i].WithdrawBankName);

                row.CreateCell(cellIndex++).SetCellValue(list[i].WithdrawBankAccountName);
                row.CreateCell(cellIndex++).SetCellValue(list[i].WithdrawBankAccountNo);
                row.CreateCell(cellIndex++).SetCellValue(list[i].WithdrawSn);
                row.CreateCell(cellIndex++).SetCellValue(list[i].WithdrawAmount.ToPrice());
                row.CreateCell(cellIndex++).SetCellValue(list[i].WithdrawFee.ToPrice());

                row.CreateCell(cellIndex++).SetCellValue(list[i].WithdrawAmountByAfterFee.ToPrice());
                row.CreateCell(cellIndex++).SetCellValue(list[i].WithdrawSettlementStartTime.ToUnifiedFormatDateTime());
                row.CreateCell(cellIndex++).SetCellValue(list[i].WithdrawSettlementEndTime.ToUnifiedFormatDateTime());
                row.CreateCell(cellIndex++).SetCellValue(list[i].WithdrawStatus.GetCnName());
                row.CreateCell(cellIndex++).SetCellValue(list[i].WithdrawFailureReason);


            }

            // 写入到客户端  
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            workbook.Write(ms);

            ms.Seek(0, SeekOrigin.Begin);



            string fileName = "截单明细" + cutOff.BatchNo + ".xls";
            return File(ms, "application/vnd.ms-excel", fileName);

        }

        [OwnAuthorize(PermissionCode.提现截单, PermissionCode.提现处理)]
        [HttpPost]
        public JsonResult UploadFeedback()
        {
            CustomJsonResult r = new CustomJsonResult();
            r.ContentType = "text/html";

            try
            {
                HttpPostedFileBase file_upload = Request.Files[0];

                if (file_upload == null)
                    return Json("text/html", ResultType.Failure, "找不到上传的对象");

                if (file_upload.ContentLength == 0)
                    return Json("text/html", ResultType.Failure, "文件内容为空,请重新选择");

                System.IO.FileInfo file = new System.IO.FileInfo(file_upload.FileName);
                string ext = file.Extension.ToLower();

                if (ext != ".xls")
                {
                    return Json("text/html", ResultType.Failure, "上传的文件不是excel格式(xls)");
                }

                HSSFWorkbook workbook = new HSSFWorkbook(file_upload.InputStream);
                ISheet sheet = workbook.GetSheetAt(0);


                int rowCount = sheet.LastRowNum + 1;


                DataTable table = new DataTable();
                var cellCount = 16;
                var firstRow = 2;



                table.Columns.Add("Num", typeof(String));
                table.Columns.Add("ClientCode", typeof(String));
                table.Columns.Add("YYZZ_Name", typeof(String));
                table.Columns.Add("ContactName", typeof(String));
                table.Columns.Add("ContactPhoneNumber", typeof(String));

                table.Columns.Add("WithdrawBankName", typeof(String));
                table.Columns.Add("WithdrawBankAccountName", typeof(String));
                table.Columns.Add("WithdrawBankAccountNo", typeof(String));
                table.Columns.Add("WithdrawSn", typeof(String));
                table.Columns.Add("WithdrawAmount", typeof(String));

                table.Columns.Add("WithdrawFee", typeof(String));
                table.Columns.Add("WithdrawAmountByAfterFee", typeof(String));
                table.Columns.Add("WithdrawSettlementStartTime", typeof(String));
                table.Columns.Add("WithdrawSettlementEndTime", typeof(String));
                table.Columns.Add("WithdrawStatus", typeof(String));
                table.Columns.Add("WithdrawFailureReason", typeof(String));
                table.Columns.Add("Id", typeof(String));

                List<bool> flag = new List<bool>();
                for (int i = 0; i < 2; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (i == 0)
                    {
                        flag.Add(CheckCellTitle(row.GetCell(0), "序号"));
                        flag.Add(CheckCellTitle(row.GetCell(1), "商户信息"));
                        flag.Add(CheckCellTitle(row.GetCell(5), "提现帐号信息"));
                        flag.Add(CheckCellTitle(row.GetCell(8), "提现信息"));
                    }
                    else
                    {
                        flag.Add(CheckCellTitle(row.GetCell(1), "商户代码"));
                        flag.Add(CheckCellTitle(row.GetCell(2), "商户名称"));
                        flag.Add(CheckCellTitle(row.GetCell(3), "联系人"));
                        flag.Add(CheckCellTitle(row.GetCell(4), "联系电话"));
                        flag.Add(CheckCellTitle(row.GetCell(5), "开户行"));

                        flag.Add(CheckCellTitle(row.GetCell(6), "持卡人"));
                        flag.Add(CheckCellTitle(row.GetCell(7), "开户帐号"));
                        flag.Add(CheckCellTitle(row.GetCell(8), "提现流水号"));
                        flag.Add(CheckCellTitle(row.GetCell(9), "提现金额"));
                        flag.Add(CheckCellTitle(row.GetCell(10), "手续费"));

                        flag.Add(CheckCellTitle(row.GetCell(11), "扣除手续费后的提现金额"));
                        flag.Add(CheckCellTitle(row.GetCell(12), "结算开始时间"));
                        flag.Add(CheckCellTitle(row.GetCell(13), "结算结束时间"));
                        flag.Add(CheckCellTitle(row.GetCell(14), "提现状态"));
                        flag.Add(CheckCellTitle(row.GetCell(15), "失败原因"));


                    }
                }


                if (flag.Contains(false))
                {
                    r.Result = ResultType.Failure;
                    r.Data = table;
                    r.Message = "上传的文件模板错误,请使用提现反馈里下载的文件模板";
                    return r;
                }

                for (int i = firstRow; i < rowCount; i++)
                {
                    IRow row = sheet.GetRow(i);
                    DataRow dataRow = table.NewRow();

                    for (int j = 0; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            string s = row.GetCell(j).ToString();
                            if (CommonUtils.IsDecimal(s))
                            {
                                decimal s_d = decimal.Parse(s);
                                int s_i = s.IndexOf('.');
                                if (s_i == 0 && s_d < 1)
                                {
                                    dataRow[j] = "0" + row.GetCell(j).ToString();
                                }
                                else
                                {
                                    string s1 = row.GetCell(j).ToString();
                                    dataRow[j] = row.GetCell(j).ToString();
                                }
                            }
                            else
                            {
                                dataRow[j] = row.GetCell(j).ToString();
                            }

                        }
                        else
                        {
                            dataRow[j] = "";
                        }
                    }

                    table.Rows.Add(dataRow);
                }


                if (table.Rows.Count > 0)
                {
                    Session["UploadFeedback"] = table;

                    r.Result = ResultType.Success;
                    r.Data = table;
                    r.Message = "成功";
                }
                else
                {
                    r.Result = ResultType.Failure;
                    r.Data = table;
                    r.Message = "上传的数据为空,请检查后上传";
                }

            }
            catch
            {
                r.Result = ResultType.Failure;
                r.Message = "上传失败，请检查导入格式是否正确或大小是否超过50M";
            }

            return r;
        }

        [OwnAuthorize(PermissionCode.提现截单, PermissionCode.提现处理)]
        public JsonResult ConfirmUploadFeedback(int id)
        {
            CustomJsonResult result = new CustomJsonResult();

            List<WithdrawCutOffDetails> updateCutOffDetails = new List<WithdrawCutOffDetails>();
            FeedbackCheckErrorPoint checkErrorPoint = new FeedbackCheckErrorPoint();


            if (Session["UploadFeedback"] == null)
            {
                return Json(ResultType.Failure, "操作超时,请再次上传文件后操作");
            }


            DataTable table = Session["UploadFeedback"] as DataTable;

            if (table.Rows.Count == 0)
            {
                return Json(ResultType.Failure, "没有可截单的数据");
            }

            #region 上传前检查
            foreach (DataRow row in table.Rows)
            {
                WithdrawCutOffDetails cutOffDetail = new WithdrawCutOffDetails();
                cutOffDetail.WithdrawSn = row["WithdrawSn"].ToString().Trim();
                if (cutOffDetail.WithdrawSn.Length == 0)
                {
                    return Json(ResultType.Failure, "检查到有为空的提现流水号，请完善后再次上传");
                }

                DataRow[] dr = table.Select("WithdrawSn='" + cutOffDetail.WithdrawSn + "'");
                if (dr.Length > 1)
                {
                    return Json(ResultType.Failure, "检查到有重复的提现流水号:" + cutOffDetail.WithdrawSn + "，请检查后再次上传");
                }

                var isExistCutOffDetail = CurrentDb.WithdrawCutOffDetails.Where(m => m.WithdrawSn == cutOffDetail.WithdrawSn && m.WithdrawCutOffId == id).FirstOrDefault();
                if (isExistCutOffDetail == null)
                {
                    checkErrorPoint.AddPoint(cutOffDetail.WithdrawSn, "提现流水号不存在或不在同一批次");
                }
                else
                {

                    string withdrawFailureReason = row["WithdrawFailureReason"].ToString().Trim();
                    string withdrawStatus = row["WithdrawStatus"].ToString().Trim();

                    if (withdrawStatus == "成功")
                    {
                        if (isExistCutOffDetail.WithdrawStatus == Enumeration.WithdrawStatus.Failure)
                        {
                            checkErrorPoint.AddPoint(cutOffDetail.WithdrawSn, "提现状态已经为失败,不能改为成功");
                        }
                    }
                    else if (withdrawStatus == "失败")
                    {
                        if (isExistCutOffDetail.WithdrawStatus == Enumeration.WithdrawStatus.Success)
                        {
                            checkErrorPoint.AddPoint(cutOffDetail.WithdrawSn, "提现结果已经为成功,不能改为失败");
                        }

                        if (withdrawFailureReason.Length == 0 || withdrawFailureReason.Length >= 500)
                        {
                            checkErrorPoint.AddPoint(cutOffDetail.WithdrawSn, "提现结果为失败,必须填写失败原因,且小于或等于500个字符");
                        }

                        cutOffDetail.WithdrawFailureReason = withdrawFailureReason;
                    }
                    else if (withdrawStatus == "处理中")
                    {
                        if (isExistCutOffDetail.WithdrawStatus == Enumeration.WithdrawStatus.Success || isExistCutOffDetail.WithdrawStatus == Enumeration.WithdrawStatus.Failure)
                        {
                            checkErrorPoint.AddPoint(cutOffDetail.WithdrawSn, "提现结果已经为" + isExistCutOffDetail.WithdrawStatus.GetCnName() + ",不能更改");
                        }
                    }
                    else
                    {
                        checkErrorPoint.AddPoint(cutOffDetail.WithdrawSn, "提现状态不正确,可选择为(处理中,成功，失败)");
                    }
                }
            }
            #endregion

            if (checkErrorPoint.ErrorPoint.Count > 0)
            {
                return Json(ResultType.Failure, checkErrorPoint.ErrorPoint, "更新数据失败，检查到无效的数据");
            }

            #region 更新的数据
            foreach (DataRow row in table.Rows)
            {
                WithdrawCutOffDetails cutOffDetail = new WithdrawCutOffDetails();
                cutOffDetail.WithdrawSn = row["WithdrawSn"].ToString().Trim();
             

                string withdrawStatus = row["WithdrawStatus"].ToString().Trim();
                if (withdrawStatus == "处理中")
                {
                    cutOffDetail.WithdrawStatus = Enumeration.WithdrawStatus.Handing;
                }
                else if (withdrawStatus == "成功")
                {
                    cutOffDetail.WithdrawStatus = Enumeration.WithdrawStatus.Success;
                }
                else if (withdrawStatus == "失败")
                {
                    cutOffDetail.WithdrawStatus = Enumeration.WithdrawStatus.Failure;
                    cutOffDetail.WithdrawFailureReason = row["WithdrawFailureReason"].ToString().Trim();
                }
                updateCutOffDetails.Add(cutOffDetail);
            }
            #endregion

            result = BizFactory.Withdraw.Feedback(this.CurrentUserId, id, updateCutOffDetails);
            return result;
        }

        private bool CheckCellTitle(ICell cell, string title)
        {
            bool flag = true;
            if (cell == null)
            {
                flag = false;
                return flag;
            }

            string l_title = cell.ToString().Trim();
            if (l_title != title)
            {
                flag = false;
            }

            return flag;
        }

        private static bool IsNumberScale(decimal d, int scale)
        {

            string s = d.ToString();
            int s_index = s.IndexOf('.');
            if (s_index <= -1)
            {
                return true;
            }
            s_index = s_index + 1;
            string s1 = s.Substring(s_index, s.Length - s_index);

            if (s1.Length <= scale)
                return true;

            return false;
        }


        private static bool IsNumber(string s, int precision, int scale)
        {
            if ((precision == 0) && (scale == 0))
            {
                return false;
            }
            string pattern = @"(^\d{1," + precision + "}";
            if (scale > 0)
            {
                pattern += @"\.\d{0," + scale + "}$)|" + pattern;
            }
            pattern += "$)";
            return Regex.IsMatch(s, pattern);
        }
    }
}