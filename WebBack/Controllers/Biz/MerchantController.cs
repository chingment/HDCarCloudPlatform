using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBack.Models.Biz.Merchant;
using Lumos.Common;
using Lumos.BLL;

namespace WebBack.Controllers.Biz
{
    public class MerchantController : OwnBaseController
    {
        // GET: Merchant
        public ViewResult OpenAccountList()
        {
            return View();
        }

        public ViewResult OpenAccount()
        {
            return View();
        }

        [OwnAuthorize(PermissionCode.商户资料初审)]

        public ViewResult PrimaryAuditList()
        {
            return View();
        }

        [OwnAuthorize(PermissionCode.商户资料初审)]
        public ViewResult PrimaryAudit(int id)
        {
            PrimaryAuditViewModel model = new PrimaryAuditViewModel(id);
            return View(model);
        }

        [OwnAuthorize(PermissionCode.商户资料复审)]
        public ViewResult SeniorAuditList()
        {
            return View();
        }

        [OwnAuthorize(PermissionCode.商户资料复审)]

        public ViewResult SeniorAudit(int id)
        {
            SeniorAuditViewModel model = new SeniorAuditViewModel(id);
            return View(model);
        }

        [OwnAuthorize(PermissionCode.商户资料维护)]
        public ViewResult EditList()
        {
            return View();
        }

        public ViewResult List()
        {
            return View();
        }

        public ViewResult PosMachineList()
        {
            return View();
        }

        public ViewResult Details(int id)
        {
            DetailsViewModel model = new DetailsViewModel(id);
            return View(model);
        }

        [OwnAuthorize(PermissionCode.商户资料维护)]
        public ViewResult Edit(int id)
        {
            EditViewModel model = new EditViewModel(id);
            return View(model);
        }


        public ViewResult TransactionsDetails(int id)
        {
            EditViewModel model = new EditViewModel(id);
            return View(model);
        }



        public CustomJsonResult GetPosMachineList(Models.Biz.PosMachine.SearchCondition condition)
        {

            string clientCode = condition.ClientCode.ToSearchString();
            string merchantName = condition.MerchantName.ToSearchString();
            string posMerchantNumber = condition.PosMerchantNumber.ToSearchString();
            string deviceId = condition.DeviceId.ToSearchString();
            var list = (from p in CurrentDb.MerchantPosMachine
                        join u in CurrentDb.Merchant on p.MerchantId equals u.Id
                        join c in CurrentDb.PosMachine on p.PosMachineId equals c.Id
                        join e in CurrentDb.SysClientUser on p.UserId equals e.Id
                        where (merchantName.Length == 0 || u.YYZZ_Name.Contains(merchantName)) &&
                        (clientCode.Length == 0 || u.ClientCode.Contains(clientCode)) &&
                                (posMerchantNumber.Length == 0 || p.PosMerchantNumber.Contains(posMerchantNumber)) &&
                                   (deviceId.Length == 0 || c.DeviceId.Contains(deviceId))
                        select new { u.Id, PosMachineId = c.Id, MerchantName = u.YYZZ_Name, u.ClientCode, p.PosMerchantNumber, c.DeviceId, u.CreateTime, u.ContactName, u.ContactPhoneNumber, e.UserName });

            int total = list.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            list = list.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }

        public CustomJsonResult GetOpenAccountList(SearchCondition condition)
        {
            string clientCode = condition.ClientCode.ToSearchString();
            string yYZZ_Name = condition.YYZZ_Name.ToSearchString();
            string yYZZ_RegisterNo = condition.YYZZ_RegisterNo.ToSearchString();
            var query = (from m in CurrentDb.Merchant
                         join u in CurrentDb.SysClientUser on m.UserId equals u.Id
                         where (clientCode.Length == 0 || u.ClientCode.Contains(clientCode)) &&
                                 (yYZZ_Name.Length == 0 || m.YYZZ_Name.Contains(yYZZ_Name)) &&
                                 (yYZZ_RegisterNo.Length == 0 || m.YYZZ_RegisterNo.Contains(yYZZ_RegisterNo))
                         select new { m.Id, u.ClientCode, m.YYZZ_Name, m.FR_Name, m.ContactName, m.ContactPhoneNumber, m.CreateTime });

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
                    item.YYZZ_Name,
                    item.ContactName,
                    item.ContactPhoneNumber,
                    DeviceId = GetDeviceId(item.Id),
                    item.CreateTime
                });


            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }

        public string GetDeviceId(int merchantId)
        {
            Lumos.DAL.LumosDbContext db = new Lumos.DAL.LumosDbContext();
            string deviceId = "";
            var merchantPosMachine = db.MerchantPosMachine.Where(m => m.MerchantId == merchantId).FirstOrDefault();
            if (merchantPosMachine != null)
            {
                var posMachine = db.PosMachine.Where(m => m.Id == merchantPosMachine.PosMachineId).FirstOrDefault();
                if (posMachine != null)
                {
                    deviceId = posMachine.DeviceId;
                }
            }

            return deviceId;
        }

        public CustomJsonResult GetList(SearchCondition condition)
        {
            string clientCode = condition.ClientCode.ToSearchString();
            string yYZZ_Name = condition.YYZZ_Name.ToSearchString();
            string yYZZ_RegisterNo = condition.YYZZ_RegisterNo.ToSearchString();
            var query = (from m in CurrentDb.Merchant
                         join u in CurrentDb.SysClientUser on m.UserId equals u.Id
                         where (clientCode.Length == 0 || u.ClientCode.Contains(clientCode)) &&
                                 (yYZZ_Name.Length == 0 || m.YYZZ_Name.Contains(yYZZ_Name)) &&
                                 (yYZZ_RegisterNo.Length == 0 || m.YYZZ_RegisterNo.Contains(yYZZ_RegisterNo))
                         select new { m.Id, u.ClientCode, m.Type, m.RepairCapacity, m.Area, m.YYZZ_Name, m.FR_Name, m.ContactName, m.CreateTime });

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
                    item.YYZZ_Name,
                    item.ContactName,
                    Type = item.Type.GetCnName(),
                    RepairCapacity = item.RepairCapacity.GetCnName(),
                    item.Area,
                    item.CreateTime
                });


            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }


        [OwnAuthorize(PermissionCode.商户资料维护)]
        public CustomJsonResult GetEditList(SearchCondition condition)
        {
            string clientCode = condition.ClientCode.ToSearchString();
            string yYZZ_Name = condition.YYZZ_Name.ToSearchString();
            string yYZZ_RegisterNo = condition.YYZZ_RegisterNo.ToSearchString();
            var query = (from m in CurrentDb.Merchant
                         join u in CurrentDb.SysClientUser on m.UserId equals u.Id
                         where (clientCode.Length == 0 || u.ClientCode.Contains(clientCode)) &&
                                 (yYZZ_Name.Length == 0 || m.YYZZ_Name.Contains(yYZZ_Name)) &&
                                 (yYZZ_RegisterNo.Length == 0 || m.YYZZ_RegisterNo.Contains(yYZZ_RegisterNo))
                         select new { m.Id, u.ClientCode, m.Type, m.RepairCapacity, m.Area, m.YYZZ_Name, m.FR_Name, m.ContactName, m.CreateTime });

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
                    item.YYZZ_Name,
                    item.ContactName,
                    Type = item.Type.GetCnName(),
                    RepairCapacity = item.RepairCapacity.GetCnName(),
                    item.Area,
                    item.CreateTime
                });
            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }

        [OwnAuthorize(PermissionCode.商户资料维护)]
        [HttpPost]
        public CustomJsonResult Edit(EditViewModel model)
        {
            return BizFactory.Merchant.Edit(this.CurrentUserId, model.Merchant, model.EstimateInsuranceCompanyIds, model.MerchantPosMachine, model.BankCard);
        }

        [OwnAuthorize(PermissionCode.商户资料初审)]
        public CustomJsonResult GetPrimaryAuditList(SearchCondition condition)
        {
            var waitAuditCount = (from h in CurrentDb.BizProcessesAudit where (h.AduitType == Enumeration.BizProcessesAuditType.MerchantAudit) && h.Status == (int)Enumeration.MerchantAuditStatus.WaitPrimaryAudit select h.Id).Count();
            var inAuditCount = (from h in CurrentDb.BizProcessesAudit where (h.AduitType == Enumeration.BizProcessesAuditType.MerchantAudit) && h.Status == (int)Enumeration.MerchantAuditStatus.InPrimaryAudit && h.Auditor == this.CurrentUserId select h.Id).Count();


            var query = (from b in CurrentDb.BizProcessesAudit
                         join m in CurrentDb.Merchant on
                        b.AduitReferenceId equals m.Id

                         join u in CurrentDb.SysClientUser on m.UserId equals u.Id


                         where (b.AduitType == Enumeration.BizProcessesAuditType.MerchantAudit)
                         select new { b.Id, b.Status, b.Auditor, u.UserName, m.ClientCode, m.Type, m.RepairCapacity, m.Area, m.YYZZ_Name, m.FR_Name, m.ContactName, m.CreateTime });

            if (condition.AuditStatus == Enumeration.MerchantAuditStatus.WaitPrimaryAudit)
            {
                query = query.Where(m => m.Status == (int)Enumeration.MerchantAuditStatus.WaitPrimaryAudit);
            }
            else if (condition.AuditStatus == Enumeration.MerchantAuditStatus.InPrimaryAudit)
            {
                query = query.Where(m => m.Status == (int)Enumeration.MerchantAuditStatus.InPrimaryAudit && m.Auditor == this.CurrentUserId);
            }

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
                    item.UserName,
                    item.ClientCode,
                    item.YYZZ_Name,
                    item.ContactName,
                    Type = item.Type.GetCnName(),
                    RepairCapacity = item.RepairCapacity.GetCnName(),
                    item.Area,
                    item.CreateTime,
                    AuditStatus = item.Status
                });
            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list, Status = new { waitAuditCount = waitAuditCount, inAuditCount = inAuditCount } };

            return Json(ResultType.Success, pageEntity, "");
        }

        [OwnAuthorize(PermissionCode.商户资料初审)]
        [HttpPost]
        public CustomJsonResult PrimaryAudit(PrimaryAuditViewModel model)
        {
            return BizFactory.Merchant.PrimaryAudit(this.CurrentUserId, model.Operate, model.Merchant, model.EstimateInsuranceCompanyIds, model.MerchantPosMachine, model.BankCard, model.BizProcessesAudit);
        }

        [OwnAuthorize(PermissionCode.商户资料复审)]
        public CustomJsonResult GetSeniorAuditList(SearchCondition condition)
        {
            var waitAuditCount = (from h in CurrentDb.BizProcessesAudit where (h.AduitType == Enumeration.BizProcessesAuditType.MerchantAudit) && h.Status == (int)Enumeration.MerchantAuditStatus.WaitSeniorAudit select h.Id).Count();
            var inAuditCount = (from h in CurrentDb.BizProcessesAudit where (h.AduitType == Enumeration.BizProcessesAuditType.MerchantAudit) && h.Status == (int)Enumeration.MerchantAuditStatus.InSeniorAudit && h.Auditor == this.CurrentUserId select h.Id).Count();


            var query = (from b in CurrentDb.BizProcessesAudit
                         join m in CurrentDb.Merchant on
                        b.AduitReferenceId equals m.Id
                         where (b.AduitType == Enumeration.BizProcessesAuditType.MerchantAudit)
                         select new { b.Id, b.Status, b.Auditor, m.ClientCode, m.Type, m.RepairCapacity, m.Area, m.YYZZ_Name, m.FR_Name, m.ContactName, m.CreateTime });

            if (condition.AuditStatus == Enumeration.MerchantAuditStatus.WaitSeniorAudit)
            {
                query = query.Where(m => m.Status == (int)Enumeration.MerchantAuditStatus.WaitSeniorAudit);
            }
            else if (condition.AuditStatus == Enumeration.MerchantAuditStatus.InSeniorAudit)
            {
                query = query.Where(m => m.Status == (int)Enumeration.MerchantAuditStatus.InSeniorAudit && m.Auditor == this.CurrentUserId);
            }

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
                    item.YYZZ_Name,
                    item.ContactName,
                    Type = item.Type.GetCnName(),
                    RepairCapacity = item.RepairCapacity.GetCnName(),
                    item.Area,
                    item.CreateTime,
                    AuditStatus = item.Status
                });
            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list, Status = new { waitAuditCount = waitAuditCount, inAuditCount = inAuditCount } };

            return Json(ResultType.Success, pageEntity, "");
        }

        [OwnAuthorize(PermissionCode.商户资料复审)]
        [HttpPost]
        public CustomJsonResult SeniorAudit(SeniorAuditViewModel model)
        {
            return BizFactory.Merchant.SeniorAudit(this.CurrentUserId, model.Operate, model.Merchant.Id, model.BizProcessesAudit);
        }

        public CustomJsonResult GetTransactionsList(Models.Biz.Transactions.SearchCondition condition)
        {

            var q = (from u in CurrentDb.FundTrans
                     where u.UserId == condition.UserId

                     select new
                     {
                         u.Id,
                         u.Sn,
                         u.CreateTime,
                         u.ChangeAmount,
                         u.Balance,
                         u.Type
                     });

            int total = q.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = condition.PageSize;
            q = q.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            var list = q.ToList();

            List<object> rList = new List<object>();

            foreach (var item in list)
            {
                rList.Add(new
                {
                    Id = item.Id,
                    Sn = item.Sn,
                    CreateTime = item.CreateTime,
                    ChangeAmount = item.ChangeAmount.ToPrice(),
                    Balance = item.Balance.ToPrice(),
                    Type = item.Type.GetCnName()
                });
            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = rList };

            return Json(ResultType.Success, pageEntity, "");
        }

    }
}