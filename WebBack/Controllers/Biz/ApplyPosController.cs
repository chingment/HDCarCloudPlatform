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
    public class ApplyPosController : OwnBaseController
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
            var list = (from p in CurrentDb.SalesmanApplyPosRecord
                        where
                                (deviceId.Length == 0 || p.PosMachineDeviceId.Contains(deviceId))
                        select new { p.Id, p.PosMachineDeviceId, p.SalesmanName, p.AgentName, p.CreateTime });

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
            var list = (from p in CurrentDb.PosMachine
                        where (deviceId.Length == 0 || p.DeviceId.Contains(deviceId)) &&
                                p.SalesmanId == null &&
                                !arrNoInDeviceId.Contains(p.DeviceId)
                                &&
                                p.AgentId == condition.AgentId
                        select new { p.Id, p.FuselageNumber, p.TerminalNumber, p.CreateTime, p.Version, p.DeviceId, p.AgentId });

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

            result = BizFactory.ApplyPos.Apply(this.CurrentUserId, model.AgentId, model.SalesmanId, model.MerchantPosMachineIds);

            return result;
        }
    }
}