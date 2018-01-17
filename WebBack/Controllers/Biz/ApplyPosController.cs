using Lumos.BLL;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBack.Models.Biz.ApplyPos;

namespace WebBack.Controllers.Biz
{
    public class ApplyPosController : WebBackController
    {
        [OwnAuthorize(PermissionCode.业务人员申领POS机登记)]
        public ViewResult List()
        {
            return View();
        }

        [OwnAuthorize(PermissionCode.业务人员申领POS机登记)]
        public ViewResult Apply()
        {
            return View();
        }

        [OwnAuthorize(PermissionCode.业务人员申领POS机登记)]
        public ViewResult PosMachineList()
        {
            return View();
        }

        [OwnAuthorize(PermissionCode.业务人员申领POS机登记)]
        public JsonResult GetList(ApplyPosSearchCondition condition)
        {

            string deviceId = condition.DeviceId.ToSearchString();
            string userName = condition.UserName.ToSearchString();
            var list = (from mp in CurrentDb.SalesmanApplyPosRecord
                        join p in CurrentDb.PosMachine on mp.PosMachineId equals p.Id
                        join m in CurrentDb.Merchant on mp.MerchantId equals m.Id

                        join u in CurrentDb.SysSalesmanUser on mp.SalesmanId equals u.Id
                        where
                                (deviceId.Length == 0 || p.DeviceId.Contains(deviceId)) &&
                                  (userName.Length == 0 || m.ClientCode.Contains(userName))
                        select new { m.ClientCode, m.YYZZ_Name, p.Id, p.DeviceId, u.FullName, mp.CreateTime });

            int total = list.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            list = list.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }

        public JsonResult GetPosMachineList(Models.Biz.PosMachine.PosMachineSearchCondition condition)
        {

            string[] arrNoInDeviceId = condition.NoInDeviceIds == null ? new string[1] { "" } : condition.NoInDeviceIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            string deviceId = condition.DeviceId.ToSearchString();
            string userName = condition.UserName.ToSearchString();
            var list = (from mp in CurrentDb.MerchantPosMachine
                        join m in CurrentDb.Merchant on mp.MerchantId equals m.Id
                        join p in CurrentDb.PosMachine on mp.PosMachineId equals p.Id
                        where (deviceId.Length == 0 || p.DeviceId.Contains(deviceId)) &&
                                (userName.Length == 0 || m.ClientCode.Contains(userName)) &&
                                m.SalesmanId == null&&
                                !arrNoInDeviceId.Contains(p.DeviceId)
                        select new { mp.Id, m.ClientCode, p.FuselageNumber, p.TerminalNumber, p.CreateTime, p.Version, p.DeviceId, p.Deposit, p.Rent });

            int total = list.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            list = list.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }

        [OwnAuthorize(PermissionCode.业务人员申领POS机登记)]
        [HttpPost]
        public JsonResult Apply(ApplyModel model)
        {
            CustomJsonResult result = new CustomJsonResult();

            result = BizFactory.ApplyPos.Apply(this.CurrentUserId, model.SalesmanId, model.MerchantPosMachineIds);

            return result;
        }
    }
}