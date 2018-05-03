using Lumos.Common;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using log4net;
using System.Web.Mvc;
using WebBack.Models.Home;
using Lumos.Mvc;
using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using Lumos.BLL;
using System.Security.Cryptography;
using System.Text;
using MySDK;
using System.Linq;
using System.Data.Entity.Core.Objects;

namespace WebBack.Controllers
{

    public class HomeController : OwnBaseController
    {
        public ViewResult Index()
        {
            return View();
        }

        public ViewResult Main()
        {
            return View();
        }


        public ViewResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            OwnRequest.Quit();

            return Redirect(OwnWebSettingUtils.GetLoginPage(""));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public CustomJsonResult ChangePassword(ChangePasswordModel model)
        {

            SysFactory.AuthorizeRelay.ChangePassword(this.CurrentUserId, this.CurrentUserId, model.OldPassword, model.NewPassword);

            return Json(ResultType.Success, "点击<a href=\"" + OwnWebSettingUtils.GetLoginPage("") + "\">登录</a>");

        }


        public CustomJsonResult GetTaskByCarInsOffer(TaskByHandleSearchCondition condition)
        {
            var waitCount = (from m in CurrentDb.BizProcessesAudit
                             where

(m.AduitType == Enumeration.BizProcessesAuditType.OrderToCarInsure && m.Status == 1)

                             select m.Id).Count();


            var inCount = (from m in CurrentDb.BizProcessesAudit
                           where

(m.AduitType == Enumeration.BizProcessesAuditType.OrderToCarInsure && m.Status == 2)
&& m.Auditor == this.CurrentUserId



                           select m.Id).Count();

            var query = (from b in CurrentDb.BizProcessesAudit
                         join m in CurrentDb.Merchant on b.MerchantId equals m.Id
                         join e in CurrentDb.Order on b.AduitReferenceId equals e.Id
                         select new { b.Id, e.Sn, m.ClientCode, m.YYZZ_Name, m.ContactName, m.ContactPhoneNumber, b.Status, b.AduitType, b.AduitReferenceId, b.AduitTypeEnumName, b.CreateTime, b.Auditor });

            if (condition.AuditStatus == 1)
            {
                query = query.Where(m => (m.AduitType == Enumeration.BizProcessesAuditType.OrderToCarInsure && m.Status == 1)

                );
            }
            else if (condition.AuditStatus == 2)
            {
                query = query.Where(m => (m.AduitType == Enumeration.BizProcessesAuditType.OrderToCarInsure && m.Status == 2)
              && m.Auditor == this.CurrentUserId

               );
            }



            int total = query.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 5;
            query = query.OrderBy(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            List<object> list = new List<object>();

            foreach (var item in query)
            {
                string aduitTypeName = item.AduitType.GetCnName();

                string statusName = "";
                switch (item.AduitType)
                {
                    case Enumeration.BizProcessesAuditType.OrderToCarInsure:
                        switch (item.Status)
                        {
                            case 1:
                                statusName = "待报价";
                                break;
                            case 2:
                                statusName = "报价中";
                                break;
                        }
                        break;
                    case Enumeration.BizProcessesAuditType.OrderToCarClaim:
                        switch (item.Status)
                        {
                            case 1:
                                statusName = "待核实需求";
                                break;
                            case 2:
                                statusName = "需求核实中";
                                break;
                            case 4:
                                statusName = "待核实金额";
                                break;
                            case 5:
                                statusName = "核实金额中";
                                break;
                        }
                        break;
                    case Enumeration.BizProcessesAuditType.MerchantAudit:
                        switch (item.Status)
                        {
                            case 2:
                                statusName = "待初审";
                                break;
                            case 3:
                                statusName = "初审中";
                                break;
                            case 4:
                                statusName = "待复审";
                                break;
                            case 5:
                                statusName = "复审中";
                                break;
                        }
                        break;
                    case Enumeration.BizProcessesAuditType.OrderToTalentDemand:
                    case Enumeration.BizProcessesAuditType.OrderToApplyLossAssess:
                    case Enumeration.BizProcessesAuditType.OrderToLllegalDealt:
                    case Enumeration.BizProcessesAuditType.OrderToCredit:
                    case Enumeration.BizProcessesAuditType.OrderToInsurance:
                        switch (item.Status)
                        {
                            case 2:
                                statusName = "待核实";
                                break;
                            case 3:
                                statusName = "核实中";
                                break;
                            case 8:
                                statusName = "待处理";
                                break;
                            case 9:
                                statusName = "处理中";
                                break;
                        }
                        break;
                }


                list.Add(new
                {
                    item.Id,
                    item.Sn,
                    item.ClientCode,
                    item.YYZZ_Name,
                    item.ContactName,
                    item.ContactPhoneNumber,
                    item.AduitReferenceId,
                    aduitTypeName,
                    item.AduitType,
                    item.AduitTypeEnumName,
                    item.CreateTime,
                    auditStatus = item.Status,
                    statusName
                });
            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list, Status = new { waitCount = waitCount, inCount = inCount } };

            return Json(ResultType.Success, pageEntity, "");
        }

        public CustomJsonResult GetTaskByVerify(TaskByHandleSearchCondition condition)
        {
            var waitCount = (from m in CurrentDb.BizProcessesAudit
                             where
(m.AduitType == Enumeration.BizProcessesAuditType.OrderToCarClaim && (m.Status == 1)) ||
((m.AduitType == Enumeration.BizProcessesAuditType.OrderToTalentDemand || m.AduitType == Enumeration.BizProcessesAuditType.OrderToTalentDemand ||
m.AduitType == Enumeration.BizProcessesAuditType.OrderToApplyLossAssess ||
m.AduitType == Enumeration.BizProcessesAuditType.OrderToLllegalDealt ||
m.AduitType == Enumeration.BizProcessesAuditType.OrderToCredit ||
m.AduitType == Enumeration.BizProcessesAuditType.OrderToInsurance

) && (m.Status == 2 || m.Status == 8))

                             select m.Id).Count();


            var inCount = (from m in CurrentDb.BizProcessesAudit
                           where
(m.AduitType == Enumeration.BizProcessesAuditType.OrderToCarClaim && (m.Status == 2)) ||
((m.AduitType == Enumeration.BizProcessesAuditType.OrderToTalentDemand || m.AduitType == Enumeration.BizProcessesAuditType.OrderToTalentDemand ||
m.AduitType == Enumeration.BizProcessesAuditType.OrderToApplyLossAssess ||
m.AduitType == Enumeration.BizProcessesAuditType.OrderToLllegalDealt ||
m.AduitType == Enumeration.BizProcessesAuditType.OrderToCredit ||
m.AduitType == Enumeration.BizProcessesAuditType.OrderToInsurance

) && (m.Status == 3 || m.Status == 9))
&& m.Auditor == this.CurrentUserId



                           select m.Id).Count();

            var query = (from b in CurrentDb.BizProcessesAudit
                         join m in CurrentDb.Merchant on b.MerchantId equals m.Id
                         join e in CurrentDb.Order on b.AduitReferenceId equals e.Id
                         select new { b.Id, e.Sn, m.ClientCode, m.YYZZ_Name, m.ContactName, m.ContactPhoneNumber, b.Status, b.AduitType, b.AduitReferenceId, b.AduitTypeEnumName, b.CreateTime, b.Auditor });

            if (condition.AuditStatus == 1)
            {
                query = query.Where(m =>
                (m.AduitType == Enumeration.BizProcessesAuditType.OrderToCarClaim && (m.Status == 1)) ||
                ((m.AduitType == Enumeration.BizProcessesAuditType.OrderToTalentDemand ||
                m.AduitType == Enumeration.BizProcessesAuditType.OrderToApplyLossAssess ||
                m.AduitType == Enumeration.BizProcessesAuditType.OrderToLllegalDealt ||
                m.AduitType == Enumeration.BizProcessesAuditType.OrderToCredit ||
                 m.AduitType == Enumeration.BizProcessesAuditType.OrderToInsurance

                ) && (m.Status == 2 || m.Status == 8))


                );
            }
            else if (condition.AuditStatus == 2)
            {
                query = query.Where(m => (
               (m.AduitType == Enumeration.BizProcessesAuditType.OrderToCarClaim && (m.Status == 2)) ||
               ((m.AduitType == Enumeration.BizProcessesAuditType.OrderToTalentDemand ||
               m.AduitType == Enumeration.BizProcessesAuditType.OrderToApplyLossAssess ||
               m.AduitType == Enumeration.BizProcessesAuditType.OrderToLllegalDealt ||
               m.AduitType == Enumeration.BizProcessesAuditType.OrderToCredit ||
                m.AduitType == Enumeration.BizProcessesAuditType.OrderToInsurance

               ) && (m.Status == 3 || m.Status == 9)))
              && m.Auditor == this.CurrentUserId


               );
            }



            int total = query.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 5;
            query = query.OrderBy(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            List<object> list = new List<object>();

            foreach (var item in query)
            {
                string aduitTypeName = item.AduitType.GetCnName();

                string statusName = "";
                switch (item.AduitType)
                {
                    case Enumeration.BizProcessesAuditType.OrderToCarInsure:
                        switch (item.Status)
                        {
                            case 1:
                                statusName = "待报价";
                                break;
                            case 2:
                                statusName = "报价中";
                                break;
                        }
                        break;
                    case Enumeration.BizProcessesAuditType.OrderToCarClaim:
                        switch (item.Status)
                        {
                            case 1:
                                statusName = "待核实需求";
                                break;
                            case 2:
                                statusName = "需求核实中";
                                break;
                            case 4:
                                statusName = "待核实金额";
                                break;
                            case 5:
                                statusName = "核实金额中";
                                break;
                        }
                        break;
                    case Enumeration.BizProcessesAuditType.MerchantAudit:
                        switch (item.Status)
                        {
                            case 2:
                                statusName = "待初审";
                                break;
                            case 3:
                                statusName = "初审中";
                                break;
                            case 4:
                                statusName = "待复审";
                                break;
                            case 5:
                                statusName = "复审中";
                                break;
                        }
                        break;
                    case Enumeration.BizProcessesAuditType.OrderToTalentDemand:
                    case Enumeration.BizProcessesAuditType.OrderToApplyLossAssess:
                    case Enumeration.BizProcessesAuditType.OrderToLllegalDealt:
                    case Enumeration.BizProcessesAuditType.OrderToCredit:
                    case Enumeration.BizProcessesAuditType.OrderToInsurance:
                        switch (item.Status)
                        {
                            case 2:
                                statusName = "待核实";
                                break;
                            case 3:
                                statusName = "核实中";
                                break;
                            case 8:
                                statusName = "待处理";
                                break;
                            case 9:
                                statusName = "处理中";
                                break;
                        }
                        break;
                }


                list.Add(new
                {
                    item.Id,
                    item.Sn,
                    item.ClientCode,
                    item.YYZZ_Name,
                    item.ContactName,
                    item.ContactPhoneNumber,
                    item.AduitReferenceId,
                    aduitTypeName,
                    item.AduitType,
                    item.AduitTypeEnumName,
                    item.CreateTime,
                    auditStatus = item.Status,
                    statusName
                });
            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list, Status = new { waitCount = waitCount, inCount = inCount } };

            return Json(ResultType.Success, pageEntity, "");
        }

        public CustomJsonResult GetTaskByDealt(TaskByHandleSearchCondition condition)
        {
            var waitCount = (from m in CurrentDb.BizProcessesAudit
                             where

((m.AduitType == Enumeration.BizProcessesAuditType.OrderToTalentDemand ||
m.AduitType == Enumeration.BizProcessesAuditType.OrderToApplyLossAssess ||
m.AduitType == Enumeration.BizProcessesAuditType.OrderToLllegalDealt ||
m.AduitType == Enumeration.BizProcessesAuditType.OrderToCredit ||
m.AduitType == Enumeration.BizProcessesAuditType.OrderToInsurance

) && (m.Status == 8))

                             select m.Id).Count();


            var inCount = (from m in CurrentDb.BizProcessesAudit
                           where
((m.AduitType == Enumeration.BizProcessesAuditType.OrderToTalentDemand ||
m.AduitType == Enumeration.BizProcessesAuditType.OrderToApplyLossAssess ||
m.AduitType == Enumeration.BizProcessesAuditType.OrderToLllegalDealt ||
m.AduitType == Enumeration.BizProcessesAuditType.OrderToCredit ||
m.AduitType == Enumeration.BizProcessesAuditType.OrderToInsurance

) && (m.Status == 9))
&& m.Auditor == this.CurrentUserId



                           select m.Id).Count();

            var query = (from b in CurrentDb.BizProcessesAudit
                         join m in CurrentDb.Merchant on b.MerchantId equals m.Id
                         join e in CurrentDb.Order on b.AduitReferenceId equals e.Id
                         select new { b.Id, e.Sn, m.ClientCode, m.YYZZ_Name, m.ContactName, m.ContactPhoneNumber, b.Status, b.AduitType, b.AduitReferenceId, b.AduitTypeEnumName, b.CreateTime, b.Auditor });

            if (condition.AuditStatus == 1)
            {
                query = query.Where(m =>

                ((m.AduitType == Enumeration.BizProcessesAuditType.OrderToTalentDemand ||
                m.AduitType == Enumeration.BizProcessesAuditType.OrderToApplyLossAssess ||
                m.AduitType == Enumeration.BizProcessesAuditType.OrderToLllegalDealt ||
                m.AduitType == Enumeration.BizProcessesAuditType.OrderToCredit ||
                 m.AduitType == Enumeration.BizProcessesAuditType.OrderToInsurance

                ) && (m.Status == 8))


                );
            }
            else if (condition.AuditStatus == 2)
            {
                query = query.Where(m =>
               ((m.AduitType == Enumeration.BizProcessesAuditType.OrderToTalentDemand ||
               m.AduitType == Enumeration.BizProcessesAuditType.OrderToApplyLossAssess ||
               m.AduitType == Enumeration.BizProcessesAuditType.OrderToLllegalDealt ||
               m.AduitType == Enumeration.BizProcessesAuditType.OrderToCredit ||
                m.AduitType == Enumeration.BizProcessesAuditType.OrderToInsurance
               ) && (m.Status == 9))
              && m.Auditor == this.CurrentUserId


               );
            }



            int total = query.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 5;
            query = query.OrderBy(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            List<object> list = new List<object>();

            foreach (var item in query)
            {
                string aduitTypeName = item.AduitType.GetCnName();

                string statusName = "";
                switch (item.AduitType)
                {
                    case Enumeration.BizProcessesAuditType.OrderToCarInsure:
                        switch (item.Status)
                        {
                            case 1:
                                statusName = "待报价";
                                break;
                            case 2:
                                statusName = "报价中";
                                break;
                        }
                        break;
                    case Enumeration.BizProcessesAuditType.OrderToCarClaim:
                        switch (item.Status)
                        {
                            case 1:
                                statusName = "待核实需求";
                                break;
                            case 2:
                                statusName = "需求核实中";
                                break;
                            case 4:
                                statusName = "待核实金额";
                                break;
                            case 5:
                                statusName = "核实金额中";
                                break;
                        }
                        break;
                    case Enumeration.BizProcessesAuditType.MerchantAudit:
                        switch (item.Status)
                        {
                            case 2:
                                statusName = "待初审";
                                break;
                            case 3:
                                statusName = "初审中";
                                break;
                            case 4:
                                statusName = "待复审";
                                break;
                            case 5:
                                statusName = "复审中";
                                break;
                        }
                        break;
                    case Enumeration.BizProcessesAuditType.OrderToTalentDemand:
                    case Enumeration.BizProcessesAuditType.OrderToApplyLossAssess:
                    case Enumeration.BizProcessesAuditType.OrderToLllegalDealt:
                    case Enumeration.BizProcessesAuditType.OrderToCredit:
                    case Enumeration.BizProcessesAuditType.OrderToInsurance:
                        switch (item.Status)
                        {
                            case 2:
                                statusName = "待核实";
                                break;
                            case 3:
                                statusName = "核实中";
                                break;
                            case 8:
                                statusName = "待处理";
                                break;
                            case 9:
                                statusName = "处理中";
                                break;
                        }
                        break;
                }


                list.Add(new
                {
                    item.Id,
                    item.Sn,
                    item.ClientCode,
                    item.YYZZ_Name,
                    item.ContactName,
                    item.ContactPhoneNumber,
                    item.AduitReferenceId,
                    aduitTypeName,
                    item.AduitType,
                    item.AduitTypeEnumName,
                    item.CreateTime,
                    auditStatus = item.Status,
                    statusName
                });
            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list, Status = new { waitCount = waitCount, inCount = inCount } };

            return Json(ResultType.Success, pageEntity, "");
        }
    }
}