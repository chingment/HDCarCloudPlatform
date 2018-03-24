using Lumos.BLL;
using Lumos.Entity;
using Lumos.Mvc;
using Lumos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBack.Models.Biz.CarClaim;

namespace WebBack.Controllers.Biz
{
    public class CarClaimController : OwnBaseController
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

        [OwnAuthorize(PermissionCode.理赔需求核实)]
        public ViewResult VerifyOrderList()
        {
            return View();
        }

        [OwnAuthorize(PermissionCode.理赔需求核实)]
        public ViewResult VerifyOrder(int id)
        {
            VerifyOrderViewModel model = new VerifyOrderViewModel(id);

            return View(model);
        }

        [OwnAuthorize(PermissionCode.理赔金额核实)]
        public ViewResult VerifyAmountList()
        {
            return View();
        }

        [OwnAuthorize(PermissionCode.理赔金额核实)]
        public ViewResult VerifyAmount(int id)
        {
            VerifyAmountViewModel model = new VerifyAmountViewModel(id);

            return View(model);
        }



        public CustomJsonResult GetList(CarClaimSearchCondition condition)
        {


            string sn = condition.Sn.ToSearchString();
            string clientCode = condition.ClientCode.ToSearchString();
            string clientName = condition.ClientName.ToSearchString();


            var query = (from o in CurrentDb.OrderToCarClaim
                         join m in CurrentDb.Merchant on o.MerchantId equals m.Id
                         where o.PId == null &&
                         (o.Status == condition.Status || condition.Status == Enumeration.OrderStatus.Unknow) &&
                        (sn.Length == 0 || o.Sn.Contains(sn)) &&
                            (clientCode.Length == 0 || m.ClientCode.Contains(clientCode)) &&
                                (clientCode.Length == 0 || m.YYZZ_Name.Contains(clientCode))
                         select new { o.Id, m.ClientCode, o.Sn, m.YYZZ_Name, o.ProductName, o.HandPerson, o.HandPersonPhone, o.CarPlateNo, o.SubmitTime, o.Status, o.CreateTime, o.InsuranceCompanyName });

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
                    item.HandPerson,
                    item.HandPersonPhone,
                    item.CarPlateNo,
                    item.ProductName,
                    item.SubmitTime,
                    item.InsuranceCompanyName,
                    Status = item.Status.GetCnName()
                });
            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }

        [OwnAuthorize(PermissionCode.理赔需求核实)]
        public CustomJsonResult GetVerifyOrderList(CarClaimSearchCondition condition)
        {
            var waitVerifyOrderCount = (from h in CurrentDb.BizProcessesAudit where (h.AduitType == Enumeration.BizProcessesAuditType.CarClaim) && h.Status == (int)Enumeration.CarClaimDealtStatus.WaitVerifyOrder select h.Id).Count();
            var inVerifyOrderCount = (from h in CurrentDb.BizProcessesAudit where (h.AduitType == Enumeration.BizProcessesAuditType.CarClaim) && h.Status == (int)Enumeration.CarClaimDealtStatus.InVerifyOrder && h.Auditor == this.CurrentUserId select h.Id).Count();

            var query = (from b in CurrentDb.BizProcessesAudit
                         join o in CurrentDb.OrderToCarClaim on
                         b.AduitReferenceId equals o.Id
                         join m in CurrentDb.Merchant on o.MerchantId equals m.Id
                         where b.AduitType == Enumeration.BizProcessesAuditType.CarClaim


                         select new { b.Id, m.ClientCode, o.Sn, m.YYZZ_Name, o.ProductName, o.HandPerson, o.HandPersonPhone, o.CarPlateNo, o.SubmitTime, b.Status, b.CreateTime, b.Auditor, o.InsuranceCompanyName });

            if (condition.DealtStatus == Enumeration.CarClaimDealtStatus.WaitVerifyOrder)
            {
                query = query.Where(m => m.Status == (int)Enumeration.CarClaimDealtStatus.WaitVerifyOrder);
            }
            else if (condition.DealtStatus == Enumeration.CarClaimDealtStatus.InVerifyOrder)
            {
                query = query.Where(m => m.Status == (int)Enumeration.CarClaimDealtStatus.InVerifyOrder && m.Auditor == this.CurrentUserId);
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
                    item.HandPerson,
                    item.HandPersonPhone,
                    item.CarPlateNo,
                    item.ProductName,
                    item.SubmitTime,
                    item.InsuranceCompanyName,
                    DealtStatus = item.Status
                });
            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list, Status = new { waitVerifyOrderCount = waitVerifyOrderCount, inVerifyOrderCount = inVerifyOrderCount } };

            return Json(ResultType.Success, pageEntity, "");
        }

        public CustomJsonResult GetRepairMerchantList(Models.Biz.Merchant.RepairMerchantSearchCondition condition)
        {

            var list = (from m in CurrentDb.Merchant
                        join c in CurrentDb.MerchantEstimateCompany on m.Id equals c.MerchantId
                        where
                         c.InsuranceCompanyId == condition.InsuranceCompanyId &&
                         m.RepairCapacity == Enumeration.RepairCapacity.EstimateAndRepair
                         &&
                         m.Id!=condition.MerchantId

                        select new { m.Id, m.ClientCode, m.YYZZ_Name, m.ContactPhoneNumber, m.ContactAddress, m.ContactName, m.CreateTime });

            int total = list.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            list = list.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }

        [OwnAuthorize(PermissionCode.理赔需求核实)]
        [HttpPost]
        public CustomJsonResult VerifyOrder(VerifyOrderViewModel model)
        {
            CustomJsonResult reuslt = new CustomJsonResult();

            reuslt = BizFactory.Order.VerifyClaimOrder(this.CurrentUserId, model.Operate, model.OrderToCarClaim, model.EstimateMerchantId,model.EstimateMerchantRemarks, model.BizProcessesAudit);

            return reuslt;
        }

        [OwnAuthorize(PermissionCode.理赔金额核实)]
        public CustomJsonResult GetVerifyAmountList(CarClaimSearchCondition condition)
        {
            var waitVerifyOrderCount = (from h in CurrentDb.BizProcessesAudit where (h.AduitType == Enumeration.BizProcessesAuditType.CarClaim) && h.Status == (int)Enumeration.CarClaimDealtStatus.WaitVerifyAmount select h.Id).Count();
            var inVerifyOrderCount = (from h in CurrentDb.BizProcessesAudit where (h.AduitType == Enumeration.BizProcessesAuditType.CarClaim) && h.Status == (int)Enumeration.CarClaimDealtStatus.InVerifyAmount && h.Auditor == this.CurrentUserId select h.Id).Count();

            var query = (from b in CurrentDb.BizProcessesAudit
                         join o in CurrentDb.OrderToCarClaim on
                         b.AduitReferenceId equals o.Id
                         join m in CurrentDb.Merchant on o.MerchantId equals m.Id
                         where b.AduitType == Enumeration.BizProcessesAuditType.CarClaim


                         select new { b.Id, m.ClientCode, o.Sn, m.YYZZ_Name, o.ProductName, o.HandPerson, o.HandPersonPhone, o.CarPlateNo, o.SubmitTime, b.Status, b.CreateTime, b.Auditor, o.InsuranceCompanyName });

            if (condition.DealtStatus == Enumeration.CarClaimDealtStatus.WaitVerifyAmount)
            {
                query = query.Where(m => m.Status == (int)Enumeration.CarClaimDealtStatus.WaitVerifyAmount);
            }
            else if (condition.DealtStatus == Enumeration.CarClaimDealtStatus.InVerifyAmount)
            {
                query = query.Where(m => m.Status == (int)Enumeration.CarClaimDealtStatus.InVerifyAmount && m.Auditor == this.CurrentUserId);
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
                    item.HandPerson,
                    item.HandPersonPhone,
                    item.CarPlateNo,
                    item.ProductName,
                    item.SubmitTime,
                    item.InsuranceCompanyName,
                    DealtStatus = item.Status
                });
            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list, Status = new { waitVerifyOrderCount = waitVerifyOrderCount, inVerifyOrderCount = inVerifyOrderCount } };

            return Json(ResultType.Success, pageEntity, "");
        }

        [OwnAuthorize(PermissionCode.理赔金额核实)]
        [HttpPost]
        public CustomJsonResult VerifyAmount(VerifyAmountViewModel model)
        {
            CustomJsonResult reuslt = new CustomJsonResult();

            reuslt = BizFactory.Order.VerifyClaimAmount(this.CurrentUserId, model.Operate, model.OrderToCarClaim, model.BizProcessesAudit);

            return reuslt;
        }
    }
}