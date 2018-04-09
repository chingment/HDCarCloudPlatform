using Lumos.BLL;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAgent.Models.ApplyPos;

namespace WebAgent.Controllers
{
    public class ApplyPosController : OwnBaseController
    {

        public ViewResult List()
        {
            return View();
        }

        public ViewResult Apply()
        {
            return View();
        }

        public ViewResult PosMachineList()
        {
            return View();
        }

        public CustomJsonResult GetList(ApplyPosSearchCondition condition)
        {

            string deviceId = condition.DeviceId.ToSearchString();
            string userName = condition.UserName.ToSearchString();
            var list = (from p in CurrentDb.SalesmanApplyPosRecord

                        where
                                (deviceId.Length == 0 || p.PosMachineDeviceId.Contains(deviceId))
                        select new { p.Id, p.PosMachineDeviceId, p.AgentName, p.SalesmanName, p.CreateTime });

            int total = list.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            list = list.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }

        public CustomJsonResult GetPosMachineList(Models.PosMachine.PosMachineSearchCondition condition)
        {

            string[] arrNoInDeviceId = condition.NoInDeviceIds == null ? new string[1] { "" } : condition.NoInDeviceIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            string deviceId = condition.DeviceId.ToSearchString();
            string userName = condition.UserName.ToSearchString();
            var list = (from p in CurrentDb.PosMachine
                        where (deviceId.Length == 0 || p.DeviceId.Contains(deviceId)) &&
                                !arrNoInDeviceId.Contains(p.DeviceId) &&
                                    p.AgentId == this.CurrentUserId
                                    && p.SalesmanId == null
                        select new { p.Id, p.FuselageNumber, p.TerminalNumber, p.CreateTime, p.Version, p.DeviceId });

            int total = list.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            list = list.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }


        [HttpPost]
        public CustomJsonResult Apply(ApplyModel model)
        {
            CustomJsonResult result = new CustomJsonResult();

            result = BizFactory.ApplyPos.Apply(this.CurrentUserId,this.CurrentUserId, model.SalesmanId, model.PosMachineIds);

            return result;
        }
    }
}