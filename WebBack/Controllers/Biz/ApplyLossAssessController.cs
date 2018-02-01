﻿using Lumos.BLL;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBack.Models.Biz.ApplyLossAssess;

namespace WebBack.Controllers.Biz
{
    public class ApplyLossAssessController : WebBackController
    {
        public ViewResult List()
        {
            return View();
        }

        public ViewResult Details(int id)
        {
            DetailsViewModel model = new DetailsViewModel(id);
            return View(model);
        }

        [OwnAuthorize(PermissionCode.定损点申请)]
        public ViewResult VerifyOrderList()
        {
            return View();
        }

        [OwnAuthorize(PermissionCode.定损点申请)]
        public ViewResult VerifyOrder(int id)
        {
            VerifyOrderViewModel model = new VerifyOrderViewModel(id);

            return View(model);
        }

        public JsonResult GetList(ApplyLossAssessSearchCondition condition)
        {


            string sn = condition.Sn.ToSearchString();
            string clientCode = condition.ClientCode.ToSearchString();
            string clientName = condition.ClientName.ToSearchString();


            var query = (from o in CurrentDb.OrderToApplyLossAssess
                         join m in CurrentDb.Merchant on o.MerchantId equals m.Id
                         where o.PId == null &&
                         (o.Status == condition.Status || condition.Status == Enumeration.OrderStatus.Unknow) &&
                        (sn.Length == 0 || o.Sn.Contains(sn)) &&
                            (clientCode.Length == 0 || m.ClientCode.Contains(clientCode)) &&
                                (clientCode.Length == 0 || m.YYZZ_Name.Contains(clientCode))
                         select new { o.Id, m.ClientCode, o.Sn, m.YYZZ_Name, m.ContactName, m.ContactPhoneNumber, o.ProductName, o.InsuranceCompanyId, o.InsuranceCompanyName, o.ApplyTime, o.SubmitTime, o.Status, o.CreateTime });

            int total = query.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            List<object> list = new List<object>();

            foreach (var item in query)
            {
                list.Add(new
                {
                    item.Id,
                    item.ClientCode,
                    item.Sn,
                    item.YYZZ_Name,
                    item.ProductName,
                    item.SubmitTime,
                    item.ContactName,
                    item.ContactPhoneNumber,
                    item.InsuranceCompanyId,
                    item.InsuranceCompanyName,
                    Status = item.Status.GetCnName()
                });
            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }

        [OwnAuthorize(PermissionCode.定损点申请)]
        public JsonResult GetVerifyOrderList(ApplyLossAssessSearchCondition condition)
        {
            var waitVerifyOrderCount = (from h in CurrentDb.BizProcessesAudit where (h.AduitType == Enumeration.BizProcessesAuditType.ApplyLossAssess) && h.Status == (int)Enumeration.ApplyLossAssessDealtStatus.WaitVerifyOrder select h.Id).Count();
            var inVerifyOrderCount = (from h in CurrentDb.BizProcessesAudit where (h.AduitType == Enumeration.BizProcessesAuditType.ApplyLossAssess) && h.Status == (int)Enumeration.ApplyLossAssessDealtStatus.InVerifyOrder && h.Auditor == this.CurrentUserId select h.Id).Count();

            var query = (from b in CurrentDb.BizProcessesAudit
                         join o in CurrentDb.OrderToApplyLossAssess on
                         b.AduitReferenceId equals o.Id
                         join m in CurrentDb.Merchant on o.MerchantId equals m.Id
                         where b.AduitType == Enumeration.BizProcessesAuditType.TalentDemand


                         select new { b.Id, m.ClientCode, o.Sn, m.YYZZ_Name, m.ContactName, m.ContactPhoneNumber, o.ProductName, o.InsuranceCompanyId, o.InsuranceCompanyName, o.ApplyTime, o.SubmitTime, b.Status, b.CreateTime, b.Auditor });

            if (condition.DealtStatus == Enumeration.ApplyLossAssessDealtStatus.WaitVerifyOrder)
            {
                query = query.Where(m => m.Status == (int)Enumeration.TalentDemandDealtStatus.WaitVerifyOrder);
            }
            else if (condition.DealtStatus == Enumeration.ApplyLossAssessDealtStatus.InVerifyOrder)
            {
                query = query.Where(m => m.Status == (int)Enumeration.ApplyLossAssessDealtStatus.InVerifyOrder && m.Auditor == this.CurrentUserId);
            }



            int total = query.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            query = query.OrderBy(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            List<object> list = new List<object>();

            foreach (var item in query)
            {
                list.Add(new
                {
                    item.Id,
                    item.ClientCode,
                    item.Sn,
                    item.YYZZ_Name,
                    item.ProductName,
                    item.SubmitTime,
                    item.ContactName,
                    item.ContactPhoneNumber,
                    item.InsuranceCompanyId,
                    item.InsuranceCompanyName,
                    DealtStatus = item.Status
                });
            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list, Status = new { waitVerifyOrderCount = waitVerifyOrderCount, inVerifyOrderCount = inVerifyOrderCount } };

            return Json(ResultType.Success, pageEntity, "");
        }


        [OwnAuthorize(PermissionCode.定损点申请)]
        [HttpPost]
        public JsonResult VerifyOrder(VerifyOrderViewModel model)
        {
            CustomJsonResult reuslt = new CustomJsonResult();

            reuslt = BizFactory.Order.VerifyApplyLossAssess(this.CurrentUserId, model.Operate, model.OrderToApplyLossAssess, model.BizProcessesAudit);

            return reuslt;
        }
    }
}