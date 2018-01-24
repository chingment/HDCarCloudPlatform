using Lumos.DAL;
using Lumos.Mvc;
using Lumos.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebBack.Models.Biz.Report;
using Lumos.Entity;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.IO;

namespace WebBack.Controllers.Biz
{
    public class ReportTable
    {
        public ReportTable()
        {

        }

        public ReportTable(string html)
        {
            this.Html = html;
        }

        public string Html
        {
            get;
            set;
        }
    }

    public class ReportController : WebBackController
    {


        public ActionResult Withdraw(WithdrawViewModel model)
        {
            StringBuilder sbTable = new StringBuilder();
            sbTable.Append("<table class='list-tb' cellspacing='0' cellpadding='0'>");
            sbTable.Append("<thead>");
            sbTable.Append("<tr>");
            sbTable.Append("<th>商户代码</th>");
            sbTable.Append("<th>商户名称</th>");
            sbTable.Append("<th>提现金额</th>");
            sbTable.Append("<th>申请时间</th>");
            sbTable.Append("<th>处理时间</th>");
            sbTable.Append("<th>结果</th>");
            sbTable.Append("</tr>");
            sbTable.Append("</thead>");
            sbTable.Append("<tbody>");
            sbTable.Append("{content}");
            sbTable.Append("</tbody>");
            sbTable.Append("</table>");

            if (Request.HttpMethod == "GET")
            {
                #region GET
                sbTable.Replace("{content}", "<tr><td colspan=\"6\"></td></tr>");

                model.TableHtml = sbTable.ToString();
                return View(model);

                #endregion
            }
            else
            {
                #region POST
                StringBuilder sql = new StringBuilder(" select b.ClientCode,b.YYZZ_Name,a.Amount,a.SettlementStartTime,a.SettlementEndTime,a.Status from Withdraw a left join  Merchant b on a.MerchantId=b.Id  ");
                sql.Append(" where 1=1 ");


                if (model.Status != Enumeration.WithdrawStatus.Unknow)
                {
                    sql.Append(" and  a.Status='" + (int)model.Status + "'");
                }

                if (!string.IsNullOrEmpty(model.ClientCode))
                {
                    sql.Append(" and  b.ClientCode='" + model.ClientCode + "'");
                }
                if (model.StartTime != null)
                {
                    sql.Append(" and  a.SettlementStartTime >='" + CommonUtils.ConverToShortDateStart(model.StartTime.Value) + "'"); ;
                }
                if (model.EndTime != null)
                {
                    sql.Append(" and  a.SettlementStartTime <='" + CommonUtils.ConverToShortDateEnd(model.EndTime.Value) + "'");
                }

                sql.Append(" order by a.SettlementStartTime desc ");


                DataTable dtData = DatabaseFactory.GetIDBOptionBySql().GetDataSet(sql.ToString()).Tables[0].ToStringDataTable();
                StringBuilder sbTableContent = new StringBuilder();
                for (int r = 0; r < dtData.Rows.Count; r++)
                {
                    sbTableContent.Append("<tr>");
                    for (int c = 0; c < dtData.Columns.Count; c++)
                    {
                        string td_value = "";

                        switch (c)
                        {
                            case 5:
                                td_value = GetWithdrawStatusName(dtData.Rows[r][c].ToString().Trim());
                                break;
                            default:
                                td_value = dtData.Rows[r][c].ToString().Trim();
                                break;
                        }

                        sbTableContent.Append("<td>" + td_value + "</td>");

                    }

                    sbTableContent.Append("</tr>");
                }

                sbTable.Replace("{content}", sbTableContent.ToString());

                ReportTable reportTable = new ReportTable(sbTable.ToString());

                if (model.Operate == Enumeration.OperateType.Serach)
                {
                    return Json(ResultType.Success, reportTable, "");
                }
                else
                {
                    NPOIExcelHelper.HtmlTable2Excel(reportTable.Html, "商户提现报表");

                    return Json(ResultType.Success, "");
                }
                #endregion
            }
        }

        public ActionResult Merchant(MerchantViewModel model)
        {
            StringBuilder sbTable = new StringBuilder();
            sbTable.Append("<table class='list-tb' cellspacing='0' cellpadding='0'>");
            sbTable.Append("<thead>");
            sbTable.Append("<tr>");
            sbTable.Append("<th>商户代码</th>");
            sbTable.Append("<th>商户名称</th>");
            sbTable.Append("<th>POS机ID</th>");
            sbTable.Append("<th>营业执照编号</th>");
            sbTable.Append("<th>商户地址</th>");
            sbTable.Append("<th>法人</th>");
            sbTable.Append("<th>法人身份证</th>");
            sbTable.Append("<th>联系人</th>");
            sbTable.Append("<th>联系方式</th>");
            sbTable.Append("<th>联系地址</th>");
            sbTable.Append("<th>绑定银行卡账号</th>");
            sbTable.Append("<th>持卡人</th>");
            sbTable.Append("<th>开户行</th>");
            sbTable.Append("<th>激活时间</th>");
            sbTable.Append("<th>注销时间</th>");
            sbTable.Append("<th>业务员</th>");
            sbTable.Append("</tr>");
            sbTable.Append("</thead>");
            sbTable.Append("<tbody>");
            sbTable.Append("{content}");
            sbTable.Append("</tbody>");
            sbTable.Append("</table>");

            if (Request.HttpMethod == "GET")
            {
                #region GET
                sbTable.Replace("{content}", "<tr><td colspan=\"16\"></td></tr>");

                model.TableHtml = sbTable.ToString();
                return View(model);

                #endregion
            }
            else
            {
                #region POST
                StringBuilder sql = new StringBuilder(" select b.ClientCode,b.YYZZ_Name,d.DeviceId,b.YYZZ_RegisterNo,b.YYZZ_Address,b.FR_Name,b.FR_IdCardNo,b.ContactName,b.ContactPhoneNumber,b.ContactAddress,c.BankAccountNo,c.BankAccountName,c.BankName,a.DepositPayTime,a.ReturnTime,e.FullName from [dbo].[MerchantPosMachine] a ");
                sql.Append(" left join[dbo].[Merchant] b on a.MerchantId = b.Id");
                sql.Append(" left join[dbo].[BankCard] c on a.BankCardId = c.Id");
                sql.Append(" left join[dbo].[PosMachine] d on a.PosMachineId = d.Id  ");
                sql.Append(" left join[dbo].[SysUser] e on b.SalesmanId = e.Id  ");

                sql.Append(" where 1=1 ");

                if (!string.IsNullOrEmpty(model.ClientCode))
                {
                    sql.Append(" and  b.ClientCode='" + model.ClientCode + "'");
                }
                if (model.StartTime != null)
                {
                    sql.Append(" and  a.DepositPayTime >='" + CommonUtils.ConverToShortDateStart(model.StartTime.Value) + "'"); ;
                }
                if (model.EndTime != null)
                {
                    sql.Append(" and  a.DepositPayTime <='" + CommonUtils.ConverToShortDateEnd(model.EndTime.Value) + "'");
                }

                sql.Append(" order by a.DepositPayTime desc ");


                DataTable dtData = DatabaseFactory.GetIDBOptionBySql().GetDataSet(sql.ToString()).Tables[0].ToStringDataTable();
                StringBuilder sbTableContent = new StringBuilder();
                for (int r = 0; r < dtData.Rows.Count; r++)
                {
                    sbTableContent.Append("<tr>");
                    for (int c = 0; c < dtData.Columns.Count; c++)
                    {
                        string td_value = "";

                        switch (c)
                        {
                            default:
                                td_value = dtData.Rows[r][c].ToString().Trim();
                                break;
                        }

                        sbTableContent.Append("<td>" + td_value + "</td>");

                    }

                    sbTableContent.Append("</tr>");
                }

                sbTable.Replace("{content}", sbTableContent.ToString());

                ReportTable reportTable = new ReportTable(sbTable.ToString());

                if (model.Operate == Enumeration.OperateType.Serach)
                {
                    return Json(ResultType.Success, reportTable, "");
                }
                else
                {
                    NPOIExcelHelper.HtmlTable2Excel(reportTable.Html, "商户信息登记报表");

                    return Json(ResultType.Success, "");
                }
                #endregion
            }
        }

        public ActionResult NoActiveAccount(MerchantViewModel model)
        {
            StringBuilder sbTable = new StringBuilder();
            sbTable.Append("<table class='list-tb' cellspacing='0' cellpadding='0'>");
            sbTable.Append("<thead>");
            sbTable.Append("<tr>");
            sbTable.Append("<th style=\"width:50%\">商户代码</th>");
            sbTable.Append("<th style=\"width:50%\">POS机ID</th>");
            sbTable.Append("</tr>");
            sbTable.Append("</thead>");
            sbTable.Append("<tbody>");
            sbTable.Append("{content}");
            sbTable.Append("</tbody>");
            sbTable.Append("</table>");

            if (Request.HttpMethod == "GET")
            {
                #region GET
                sbTable.Replace("{content}", "<tr><td colspan=\"2\"></td></tr>");

                model.TableHtml = sbTable.ToString();
                return View(model);

                #endregion
            }
            else
            {
                #region POST
                StringBuilder sql = new StringBuilder(" select b.ClientCode,d.DeviceId from [dbo].[MerchantPosMachine] a ");
                sql.Append(" left join[dbo].[Merchant] b on a.MerchantId = b.Id ");
                sql.Append(" ");
                sql.Append(" left join[dbo].[PosMachine] d on a.PosMachineId = d.Id  ");
                sql.Append(" where 1=1 and a.[Status]=2 ");

                if (!string.IsNullOrEmpty(model.ClientCode))
                {
                    sql.Append(" and  b.ClientCode='" + model.ClientCode + "'");
                }
                if (model.StartTime != null)
                {
                    sql.Append(" and  a.DepositPayTime >='" + CommonUtils.ConverToShortDateStart(model.StartTime.Value) + "'"); ;
                }
                if (model.EndTime != null)
                {
                    sql.Append(" and  a.DepositPayTime <='" + CommonUtils.ConverToShortDateEnd(model.EndTime.Value) + "'");
                }

                sql.Append(" order by a.DepositPayTime desc ");


                DataTable dtData = DatabaseFactory.GetIDBOptionBySql().GetDataSet(sql.ToString()).Tables[0].ToStringDataTable();
                StringBuilder sbTableContent = new StringBuilder();
                for (int r = 0; r < dtData.Rows.Count; r++)
                {
                    sbTableContent.Append("<tr>");
                    for (int c = 0; c < dtData.Columns.Count; c++)
                    {
                        string td_value = "";

                        switch (c)
                        {
                            default:
                                td_value = dtData.Rows[r][c].ToString().Trim();
                                break;
                        }

                        sbTableContent.Append("<td>" + td_value + "</td>");

                    }

                    sbTableContent.Append("</tr>");
                }

                sbTable.Replace("{content}", sbTableContent.ToString());

                ReportTable reportTable = new ReportTable(sbTable.ToString());

                if (model.Operate == Enumeration.OperateType.Serach)
                {
                    return Json(ResultType.Success, reportTable, "");
                }
                else
                {
                    NPOIExcelHelper.HtmlTable2Excel(reportTable.Html, "商户账号（未激活）");

                    return Json(ResultType.Success, "");
                }
                #endregion
            }
        }

        public ActionResult CarInsure(CarInsureViewModel model)
        {
            StringBuilder sbTable = new StringBuilder();
            sbTable.Append("<table class='list-tb' cellspacing='0' cellpadding='0'>");
            sbTable.Append("<thead>");
            sbTable.Append("<tr>");
            sbTable.Append("<th>商户代码</th>");
            sbTable.Append("<th>商户名称</th>");
            sbTable.Append("<th>订单号</th>");
            sbTable.Append("<th>保险公司</th>");
            sbTable.Append("<th>保单号</th>");
            sbTable.Append("<th>投保人</th>");
            sbTable.Append("<th>商业险保费金额</th>");
            sbTable.Append("<th>交强险保费金额</th>");
            sbTable.Append("<th>车船税金额</th>");
            sbTable.Append("<th>总保费金额</th>");
            sbTable.Append("<th>商业险佣金金额</th>");
            sbTable.Append("<th>提交时间</th>");
            sbTable.Append("<th>支付时间</th>");
            sbTable.Append("<th>取消时间</th>");
            sbTable.Append("<th>状态</th>");
            sbTable.Append("</tr>");
            sbTable.Append("</thead>");
            sbTable.Append("<tbody>");
            sbTable.Append("{content}");
            sbTable.Append("</tbody>");
            sbTable.Append("</table>");

            if (Request.HttpMethod == "GET")
            {
                #region GET
                sbTable.Replace("{content}", "<tr><td colspan=\"15\"></td></tr>");

                model.TableHtml = sbTable.ToString();
                return View(model);

                #endregion
            }
            else
            {
                #region POST
                StringBuilder sql = new StringBuilder("select c.ClientCode,c.YYZZ_Name,a.Sn,b.InsuranceCompanyName,b.InsuranceOrderId,b.CarOwner,b.CommercialPrice,b.CompulsoryPrice,b.TravelTaxPrice,a.Price,b.MerchantCommission,a.SubmitTime,a.PayTime,a.CancleTime,a.Status from  [dbo].[Order]  a ");
                sql.Append(" inner join [dbo].[OrderToCarInsure]  b on a.Id=b.Id ");
                sql.Append(" left join [dbo].[Merchant] c on a.MerchantId=c.Id ");

                sql.Append(" where 1=1 ");

                if (model.Status != Enumeration.OrderStatus.Unknow)
                {
                    sql.Append(" and  a.Status='" + (int)model.Status + "'");
                }

                if (!string.IsNullOrEmpty(model.ClientCode))
                {
                    sql.Append(" and  c.ClientCode='" + model.ClientCode + "'");
                }
                if (model.StartTime != null)
                {
                    sql.Append(" and  a.SubmitTime >='" + CommonUtils.ConverToShortDateStart(model.StartTime.Value) + "'"); ;
                }
                if (model.EndTime != null)
                {
                    sql.Append(" and  a.SubmitTime <='" + CommonUtils.ConverToShortDateEnd(model.EndTime.Value) + "'");
                }


                if (model.StartPayTime != null)
                {
                    sql.Append(" and  a.PayTime >='" + CommonUtils.ConverToShortDateStart(model.StartPayTime.Value) + "'"); ;
                }
                if (model.EndPayTime != null)
                {
                    sql.Append(" and  a.PayTime <='" + CommonUtils.ConverToShortDateEnd(model.EndPayTime.Value) + "'");
                }

                sql.Append(" order by a.SubmitTime desc ");


                DataTable dtData = DatabaseFactory.GetIDBOptionBySql().GetDataSet(sql.ToString()).Tables[0].ToStringDataTable();
                StringBuilder sbTableContent = new StringBuilder();
                for (int r = 0; r < dtData.Rows.Count; r++)
                {
                    sbTableContent.Append("<tr>");
                    for (int c = 0; c < dtData.Columns.Count; c++)
                    {
                        string td_value = "";

                        switch (c)
                        {
                            case 14:
                                td_value = GetOrderStatusName(dtData.Rows[r][c].ToString().Trim());
                                break;
                            default:
                                td_value = dtData.Rows[r][c].ToString().Trim();
                                break;
                        }

                        sbTableContent.Append("<td>" + td_value + "</td>");

                    }

                    sbTableContent.Append("</tr>");
                }

                sbTable.Replace("{content}", sbTableContent.ToString());

                ReportTable reportTable = new ReportTable(sbTable.ToString());

                if (model.Operate == Enumeration.OperateType.Serach)
                {
                    return Json(ResultType.Success, reportTable, "");
                }
                else
                {
                    NPOIExcelHelper.HtmlTable2Excel(reportTable.Html, "商户车险订单报表");

                    return Json(ResultType.Success, "");
                }
                #endregion
            }
        }

        //public ActionResult DepositRent(DepositRentViewModel model)
        //{
        //    StringBuilder sbTable = new StringBuilder();
        //    sbTable.Append("<table class='list-tb' cellspacing='0' cellpadding='0'>");
        //    sbTable.Append("<thead>");
        //    sbTable.Append("<tr>");
        //    sbTable.Append("<th>商户代码</th>");
        //    sbTable.Append("<th>商户名称</th>");
        //    sbTable.Append("<th>POS机ID</th>");
        //    sbTable.Append("<th>地址</th>");
        //    sbTable.Append("<th>联系人</th>");
        //    sbTable.Append("<th>联系电话</th>");
        //    sbTable.Append("<th>订单号</th>");
        //    sbTable.Append("<th>押金</th>");
        //    sbTable.Append("<th>租期(月)</th>");
        //    sbTable.Append("<th>租金/月</th>");
        //    sbTable.Append("<th>到期日期</th>");
        //    sbTable.Append("<th>合计租金</th>");
        //    sbTable.Append("<th>状态</th>");
        //    sbTable.Append("<th>缴费时间</th>");
        //    sbTable.Append("<th>合计</th>");
        //    sbTable.Append("</tr>");
        //    sbTable.Append("</thead>");
        //    sbTable.Append("<tbody>");
        //    sbTable.Append("{content}");
        //    sbTable.Append("</tbody>");
        //    sbTable.Append("</table>");

        //    if (Request.HttpMethod == "GET")
        //    {
        //        #region GET
        //        sbTable.Replace("{content}", "<tr><td colspan=\"15\"></td></tr>");

        //        model.TableHtml = sbTable.ToString();
        //        return View(model);

        //        #endregion
        //    }
        //    else
        //    {
        //        #region POST
        //        StringBuilder sql = new StringBuilder("select b.ClientCode,b.YYZZ_Name,c.DeviceId, b.YYZZ_Address, b.ContactName,b.ContactPhoneNumber,d.Sn, c.Deposit,e.RentMonths,e.MonthlyRent,e.RentDueDate, e.RentTotal,d.[Status], d.PayTime,d.Price from  MerchantPosMachine a ");
        //        sql.Append(" inner join Merchant b on a.MerchantId=b.Id inner join PosMachine c on a.PosMachineId=c.Id ");
        //        sql.Append(" inner join  [Order]  d  on  b.Id=d.MerchantId  inner join OrderToDepositRent e on d.Id=e.Id ");

        //        sql.Append(" where d.ProductType=" + (int)Enumeration.ProductType.PosMachineDepositRent + " ");



        //        if (!string.IsNullOrEmpty(model.ClientCode))
        //        {
        //            sql.Append(" and  b.ClientCode='" + model.ClientCode + "'");
        //        }
        //        if (model.StartTime != null)
        //        {
        //            sql.Append(" and  d.PayTime >='" + CommonUtils.ConverToShortDateStart(model.StartTime.Value) + "'"); ;
        //        }
        //        if (model.EndTime != null)
        //        {
        //            sql.Append(" and  d.PayTime <='" + CommonUtils.ConverToShortDateEnd(model.EndTime.Value) + "'");
        //        }

        //        sql.Append(" order by d.SubmitTime desc ");


        //        DataTable dtData = DatabaseFactory.GetIDBOptionBySql().GetDataSet(sql.ToString()).Tables[0].ToStringDataTable();
        //        StringBuilder sbTableContent = new StringBuilder();
        //        for (int r = 0; r < dtData.Rows.Count; r++)
        //        {
        //            sbTableContent.Append("<tr>");
        //            for (int c = 0; c < dtData.Columns.Count; c++)
        //            {
        //                string td_value = "";

        //                switch (c)
        //                {
        //                    case 12:
        //                        td_value = GetOrderStatusName(dtData.Rows[r][c].ToString().Trim());
        //                        break;
        //                    default:
        //                        td_value = dtData.Rows[r][c].ToString().Trim();
        //                        break;
        //                }

        //                sbTableContent.Append("<td>" + td_value + "</td>");

        //            }

        //            sbTableContent.Append("</tr>");
        //        }

        //        sbTable.Replace("{content}", sbTableContent.ToString());

        //        ReportTable reportTable = new ReportTable(sbTable.ToString());

        //        if (model.Operate == Enumeration.OperateType.Serach)
        //        {
        //            return Json(ResultType.Success, reportTable, "");
        //        }
        //        else
        //        {
        //            NPOIExcelHelper.HtmlTable2Excel(reportTable.Html, "商户押金租金报表");

        //            return Json(ResultType.Success, "");
        //        }
        //        #endregion
        //    }
        //}

        //public ActionResult Rent(DepositRentViewModel model)
        //{
        //    StringBuilder sbTable = new StringBuilder();
        //    sbTable.Append("<table class='list-tb' cellspacing='0' cellpadding='0'>");
        //    sbTable.Append("<thead>");
        //    sbTable.Append("<tr>");
        //    sbTable.Append("<th>商户代码</th>");
        //    sbTable.Append("<th>商户名称</th>");
        //    sbTable.Append("<th>POS机ID</th>");
        //    sbTable.Append("<th>地址</th>");
        //    sbTable.Append("<th>联系人</th>");
        //    sbTable.Append("<th>联系电话</th>");
        //    sbTable.Append("<th>续期(月)</th>");
        //    sbTable.Append("<th>租金/月</th>");
        //    sbTable.Append("<th>到期日期</th>");
        //    sbTable.Append("<th>状态</th>");
        //    sbTable.Append("<th>缴费时间</th>");
        //    sbTable.Append("<th>合计</th>");
        //    sbTable.Append("</tr>");
        //    sbTable.Append("</thead>");
        //    sbTable.Append("<tbody>");
        //    sbTable.Append("{content}");
        //    sbTable.Append("</tbody>");
        //    sbTable.Append("</table>");

        //    if (Request.HttpMethod == "GET")
        //    {
        //        #region GET
        //        sbTable.Replace("{content}", "<tr><td colspan=\"12\"></td></tr>");

        //        model.TableHtml = sbTable.ToString();
        //        return View(model);

        //        #endregion
        //    }
        //    else
        //    {
        //        #region POST
        //        StringBuilder sql = new StringBuilder("select b.ClientCode,b.YYZZ_Name,c.DeviceId, b.YYZZ_Address, b.ContactName,b.ContactPhoneNumber,e.RentMonths,e.MonthlyRent,e.RentDueDate,d.[Status], d.PayTime,d.Price from  MerchantPosMachine a ");
        //        sql.Append(" inner join Merchant b on a.MerchantId=b.Id inner join PosMachine c on a.PosMachineId=c.Id ");
        //        sql.Append(" inner join  [Order]  d  on  b.Id=d.MerchantId  inner join OrderToDepositRent e on d.Id=e.Id ");

        //        sql.Append(" where d.ProductType=" + (int)Enumeration.ProductType.PosMachineRent + " ");



        //        if (!string.IsNullOrEmpty(model.ClientCode))
        //        {
        //            sql.Append(" and  b.ClientCode='" + model.ClientCode + "'");
        //        }
        //        if (model.StartTime != null)
        //        {
        //            sql.Append(" and  d.PayTime >='" + CommonUtils.ConverToShortDateStart(model.StartTime.Value) + "'"); ;
        //        }
        //        if (model.EndTime != null)
        //        {
        //            sql.Append(" and  d.PayTime <='" + CommonUtils.ConverToShortDateEnd(model.EndTime.Value) + "'");
        //        }

        //        sql.Append(" order by d.SubmitTime desc ");


        //        DataTable dtData = DatabaseFactory.GetIDBOptionBySql().GetDataSet(sql.ToString()).Tables[0].ToStringDataTable();
        //        StringBuilder sbTableContent = new StringBuilder();
        //        for (int r = 0; r < dtData.Rows.Count; r++)
        //        {
        //            sbTableContent.Append("<tr>");
        //            for (int c = 0; c < dtData.Columns.Count; c++)
        //            {
        //                string td_value = "";

        //                switch (c)
        //                {
        //                    case 9:
        //                        td_value = GetOrderStatusName(dtData.Rows[r][c].ToString().Trim());
        //                        break;
        //                    default:
        //                        td_value = dtData.Rows[r][c].ToString().Trim();
        //                        break;
        //                }

        //                sbTableContent.Append("<td>" + td_value + "</td>");

        //            }

        //            sbTableContent.Append("</tr>");
        //        }

        //        sbTable.Replace("{content}", sbTableContent.ToString());

        //        ReportTable reportTable = new ReportTable(sbTable.ToString());

        //        if (model.Operate == Enumeration.OperateType.Serach)
        //        {
        //            return Json(ResultType.Success, reportTable, "");
        //        }
        //        else
        //        {
        //            NPOIExcelHelper.HtmlTable2Excel(reportTable.Html, "商户租金报表");

        //            return Json(ResultType.Success, "");
        //        }
        //        #endregion
        //    }
        //}

        public ActionResult SalesmanPos(SalesmanPosViewModel model)
        {
            StringBuilder sbTable = new StringBuilder();
            sbTable.Append("<table class='list-tb' cellspacing='0' cellpadding='0'>");
            sbTable.Append("<thead>");
            sbTable.Append("<tr>");
            sbTable.Append("<th>业务员账号</th>");
            sbTable.Append("<th>业务员姓名</th>");
            sbTable.Append("<th>商户名称</th>");
            sbTable.Append("<th>商户账号</th>");
            sbTable.Append("<th>POS机Id</th>");
            sbTable.Append("<th>状态</th>");
            sbTable.Append("<th>激活时间</th>");
            sbTable.Append("</tr>");
            sbTable.Append("</thead>");
            sbTable.Append("<tbody>");
            sbTable.Append("{content}");
            sbTable.Append("</tbody>");
            sbTable.Append("</table>");

            if (Request.HttpMethod == "GET")
            {
                #region GET
                sbTable.Replace("{content}", "<tr><td colspan=\"7\"></td></tr>");

                model.TableHtml = sbTable.ToString();
                return View(model);

                #endregion
            }
            else
            {
                #region POST
                StringBuilder sql = new StringBuilder(" select  d.UserName,d.FullName,b.YYZZ_Name,b.ClientCode,c.DeviceId,a.[Status],a.DepositPayTime from  ");
                sql.Append(" [dbo].[MerchantPosMachine] a inner join [dbo].[Merchant] b on a.MerchantId=b.Id  ");
                sql.Append(" inner join  [dbo].[PosMachine] c on a.PosMachineId=c.Id ");
                sql.Append(" inner join  [dbo].[SysUser] d on b.SalesmanId=d.Id ");

                sql.Append(" where 1=1 ");


                if (!string.IsNullOrEmpty(model.UserName))
                {
                    sql.Append(" and  d.UserName='" + model.UserName + "'");
                }
                if (model.StartTime != null)
                {
                    sql.Append(" and  a.DepositPayTime >='" + CommonUtils.ConverToShortDateStart(model.StartTime.Value) + "'"); ;
                }
                if (model.EndTime != null)
                {
                    sql.Append(" and  a.DepositPayTime <='" + CommonUtils.ConverToShortDateEnd(model.EndTime.Value) + "'");
                }

                sql.Append(" order by d.UserName desc ");


                DataTable dtData = DatabaseFactory.GetIDBOptionBySql().GetDataSet(sql.ToString()).Tables[0].ToStringDataTable();
                StringBuilder sbTableContent = new StringBuilder();
                for (int r = 0; r < dtData.Rows.Count; r++)
                {
                    sbTableContent.Append("<tr>");
                    for (int c = 0; c < dtData.Columns.Count; c++)
                    {
                        string td_value = "";

                        switch (c)
                        {
                            case 5:
                                td_value = GetPosMachineStatusName(dtData.Rows[r][c].ToString().Trim());
                                break;
                            default:
                                td_value = dtData.Rows[r][c].ToString().Trim();
                                break;
                        }

                        sbTableContent.Append("<td>" + td_value + "</td>");

                    }

                    sbTableContent.Append("</tr>");
                }

                sbTable.Replace("{content}", sbTableContent.ToString());

                ReportTable reportTable = new ReportTable(sbTable.ToString());

                if (model.Operate == Enumeration.OperateType.Serach)
                {
                    return Json(ResultType.Success, reportTable, "");
                }
                else
                {
                    NPOIExcelHelper.HtmlTable2Excel(reportTable.Html, "业务员对应POS机报表");

                    return Json(ResultType.Success, "");
                }
                #endregion
            }
        }


        public ActionResult PosList(PosListViewModel model)
        {
            StringBuilder sbTable = new StringBuilder();
            sbTable.Append("<table class='list-tb' cellspacing='0' cellpadding='0'>");
            sbTable.Append("<thead>");
            sbTable.Append("<tr>");
            sbTable.Append("<th>设备ID</th>");
            sbTable.Append("<th>机身号</th>");
            sbTable.Append("<th>终端号</th>");
            sbTable.Append("<th>版本号 </th>");
            sbTable.Append("<th>已使用</th>");
            sbTable.Append("<th>登记时间</th>");
            sbTable.Append("</tr>");
            sbTable.Append("</thead>");
            sbTable.Append("<tbody>");
            sbTable.Append("{content}");
            sbTable.Append("</tbody>");
            sbTable.Append("</table>");

            if (Request.HttpMethod == "GET")
            {
                #region GET
                sbTable.Replace("{content}", "<tr><td colspan=\"6\"></td></tr>");

                model.TableHtml = sbTable.ToString();
                return View(model);

                #endregion
            }
            else
            {
                #region POST
                StringBuilder sql = new StringBuilder(" select  a.DeviceId,a.FuselageNumber,a.TerminalNumber,a.Version,a.IsUse,a.CreateTime from  ");
                sql.Append(" PosMachine a  ");

                sql.Append(" where 1=1 ");


                if (model.StartTime != null)
                {
                    sql.Append(" and  a.CreateTime >='" + CommonUtils.ConverToShortDateStart(model.StartTime.Value) + "'"); ;
                }
                if (model.EndTime != null)
                {
                    sql.Append(" and  a.CreateTime <='" + CommonUtils.ConverToShortDateEnd(model.EndTime.Value) + "'");
                }

                sql.Append(" order by a.TerminalNumber desc ");


                DataTable dtData = DatabaseFactory.GetIDBOptionBySql().GetDataSet(sql.ToString()).Tables[0].ToStringDataTable();
                StringBuilder sbTableContent = new StringBuilder();
                for (int r = 0; r < dtData.Rows.Count; r++)
                {
                    sbTableContent.Append("<tr>");
                    for (int c = 0; c < dtData.Columns.Count; c++)
                    {
                        string td_value = "";

                        switch (c)
                        {
                            case 4:
                                td_value = GetBoolName(dtData.Rows[r][c].ToString().Trim());
                                break;
                            default:
                                td_value = dtData.Rows[r][c].ToString().Trim();
                                break;
                        }

                        sbTableContent.Append("<td>" + td_value + "</td>");

                    }

                    sbTableContent.Append("</tr>");
                }

                sbTable.Replace("{content}", sbTableContent.ToString());

                ReportTable reportTable = new ReportTable(sbTable.ToString());

                if (model.Operate == Enumeration.OperateType.Serach)
                {
                    return Json(ResultType.Success, reportTable, "");
                }
                else
                {
                    NPOIExcelHelper.HtmlTable2Excel(reportTable.Html, "POS机报表");

                    return Json(ResultType.Success, "");
                }
                #endregion
            }
        }


        private string GetWithdrawStatusName(string status)
        {
            if (status == null)
                return "未知";

            string name = "未知";
            if (status == "1")
            {
                name = "发起请求";
            }
            else if (status == "2")
            {
                name = "处理中";
            }
            else if (status == "3")
            {
                name = "成功";
            }
            else if (status == "4")
            {
                name = "失败";
            }

            return name;
        }

        private string GetOrderStatusName(string status)
        {
            if (status == null)
                return "未知";

            string name = "未知";
            if (status == "1")
            {
                name = "已提交";
            }
            else if (status == "2")
            {
                name = "跟进中";
            }
            else if (status == "3")
            {
                name = "待支付";
            }
            else if (status == "4")
            {
                name = "已完成";
            }
            else if (status == "5")
            {
                name = "已取消";
            }
            return name;
        }

        private string GetPosMachineStatusName(string status)
        {

            if (status == null)
                return "未知";

            string name = "未知";
            if (status == "1")
            {
                name = "正常";
            }
            else if (status == "2")
            {
                name = "未激活";
            }
            else if (status == "3")
            {
                name = "租金到期";
            }
            else if (status == "4")
            {
                name = "已注销";
            }
            else if (status == "5")
            {
                name = "账号与设备好不匹配";
            }
            return name;
        }

        private string GetBoolName(string val)
        {

            if (val == "True")
                return "是";

            return "否";
        }
    }
}