using Lumos.BLL;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBack.Models.Biz.TalentDemand;

namespace WebBack.Controllers.Biz
{
    public class TalentDemandController : OwnBaseController
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

        [OwnAuthorize(PermissionCode.人才需求核实)]
        public ViewResult ListByVerify()
        {
            return View();
        }

        [OwnAuthorize(PermissionCode.人才需求核实)]
        public ViewResult Verify(int id)
        {
            VerifyViewModel model = new VerifyViewModel(id);

            return View(model);
        }

        [OwnAuthorize(PermissionCode.人才需求核实)]
        public ViewResult ListByDealt()
        {
            return View();
        }

        [OwnAuthorize(PermissionCode.人才需求核实)]
        public ViewResult Dealt(int id)
        {
            DealtViewModel model = new DealtViewModel(id);
            return View(model);
        }

        public CustomJsonResult GetList(SearchCondition condition)
        {


            string sn = condition.Sn.ToSearchString();
            string clientCode = condition.ClientCode.ToSearchString();
            string clientName = condition.ClientName.ToSearchString();


            var query = (from o in CurrentDb.OrderToTalentDemand
                         join m in CurrentDb.Merchant on o.MerchantId equals m.Id
                         where o.PId == null &&
                         (o.Status == condition.Status || condition.Status == Enumeration.OrderStatus.Unknow) &&
                        (sn.Length == 0 || o.Sn.Contains(sn)) &&
                            (clientCode.Length == 0 || m.ClientCode.Contains(clientCode)) &&
                                (clientCode.Length == 0 || m.YYZZ_Name.Contains(clientCode))
                         select new { o.Id, m.ClientCode, o.Sn, m.YYZZ_Name, m.ContactName, m.ContactPhoneNumber, o.ProductName, o.Quantity, o.WorkJob, o.SubmitTime, o.Status, o.CreateTime });

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
                    item.Quantity,
                    item.ContactName,
                    item.ContactPhoneNumber,
                    WorkJob = item.WorkJob.GetCnName(),
                    item.ProductName,
                    item.SubmitTime,
                    Status = item.Status.GetCnName()
                });
            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }

        [OwnAuthorize(PermissionCode.人才需求核实)]
        public CustomJsonResult GetListByVerify(SearchCondition condition)
        {
            var waitCount = (from h in CurrentDb.BizProcessesAudit where (h.AduitType == Enumeration.BizProcessesAuditType.TalentDemand) && h.Status == (int)Enumeration.TalentDemandAuditStatus.WaitVerify select h.Id).Count();
            var inCount = (from h in CurrentDb.BizProcessesAudit where (h.AduitType == Enumeration.BizProcessesAuditType.TalentDemand) && h.Status == (int)Enumeration.TalentDemandAuditStatus.InVerify && h.Auditor == this.CurrentUserId select h.Id).Count();

            var query = (from b in CurrentDb.BizProcessesAudit
                         join o in CurrentDb.OrderToTalentDemand on
                         b.AduitReferenceId equals o.Id
                         join m in CurrentDb.Merchant on o.MerchantId equals m.Id
                         where b.AduitType == Enumeration.BizProcessesAuditType.TalentDemand


                         select new { b.Id, m.ClientCode, o.Sn, m.YYZZ_Name, m.ContactName, m.ContactPhoneNumber, o.ProductName, o.Quantity, o.WorkJob, o.SubmitTime, b.Status, b.CreateTime, b.Auditor });

            if (condition.AuditStatus == Enumeration.TalentDemandAuditStatus.WaitVerify)
            {
                query = query.Where(m => m.Status == (int)Enumeration.TalentDemandAuditStatus.WaitVerify);
            }
            else if (condition.AuditStatus == Enumeration.TalentDemandAuditStatus.InVerify)
            {
                query = query.Where(m => m.Status == (int)Enumeration.TalentDemandAuditStatus.InVerify && m.Auditor == this.CurrentUserId);
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
                    item.ContactName,
                    item.ContactPhoneNumber,
                    WorkJob = item.WorkJob.GetCnName(),
                    item.Quantity,
                    item.ProductName,
                    item.SubmitTime,
                    AuditStatus = item.Status
                });
            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list, Status = new { waitCount = waitCount, inCount = inCount } };

            return Json(ResultType.Success, pageEntity, "");
        }


        [OwnAuthorize(PermissionCode.人才需求核实)]
        public CustomJsonResult GetListByDealt(SearchCondition condition)
        {
            var waitCount = (from h in CurrentDb.BizProcessesAudit where (h.AduitType == Enumeration.BizProcessesAuditType.TalentDemand) && h.Status == (int)Enumeration.TalentDemandAuditStatus.WaitDealt select h.Id).Count();
            var inCount = (from h in CurrentDb.BizProcessesAudit where (h.AduitType == Enumeration.BizProcessesAuditType.TalentDemand) && h.Status == (int)Enumeration.TalentDemandAuditStatus.InDealt && h.Auditor == this.CurrentUserId select h.Id).Count();

            var query = (from b in CurrentDb.BizProcessesAudit
                         join o in CurrentDb.OrderToTalentDemand on
                         b.AduitReferenceId equals o.Id
                         join m in CurrentDb.Merchant on o.MerchantId equals m.Id
                         where b.AduitType == Enumeration.BizProcessesAuditType.TalentDemand


                         select new { b.Id, m.ClientCode, o.Sn, m.YYZZ_Name, m.ContactName, m.ContactPhoneNumber, o.ProductName, o.Quantity, o.WorkJob, o.SubmitTime, b.Status, b.CreateTime, b.Auditor });

            if (condition.AuditStatus == Enumeration.TalentDemandAuditStatus.WaitDealt)
            {
                query = query.Where(m => m.Status == (int)Enumeration.TalentDemandAuditStatus.WaitDealt);
            }
            else if (condition.AuditStatus == Enumeration.TalentDemandAuditStatus.InDealt)
            {
                query = query.Where(m => m.Status == (int)Enumeration.TalentDemandAuditStatus.InDealt && m.Auditor == this.CurrentUserId);
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
                    item.ContactName,
                    item.ContactPhoneNumber,
                    WorkJob = item.WorkJob.GetCnName(),
                    item.Quantity,
                    item.ProductName,
                    item.SubmitTime,
                    AuditStatus = item.Status
                });
            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list, Status = new { waitCount = waitCount, inCount = inCount } };

            return Json(ResultType.Success, pageEntity, "");
        }

        [OwnAuthorize(PermissionCode.人才需求核实)]
        [HttpPost]
        public CustomJsonResult Verify(VerifyViewModel model)
        {
            CustomJsonResult reuslt = new CustomJsonResult();

            reuslt = BizFactory.Order.VerifyTalentDemand(this.CurrentUserId, model.Operate, model.OrderToTalentDemand, model.BizProcessesAudit);

            return reuslt;
        }

        [OwnAuthorize(PermissionCode.人才需求核实)]
        [HttpPost]
        public CustomJsonResult Dealt(DealtViewModel model)
        {
            CustomJsonResult reuslt = new CustomJsonResult();

            reuslt = BizFactory.Order.DealtTalentDemand(this.CurrentUserId, model.Operate, model.OrderToTalentDemand, model.BizProcessesAudit);

            return reuslt;
        }
    }
}