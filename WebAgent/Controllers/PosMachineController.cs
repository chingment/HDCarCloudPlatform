using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAgent.Models.PosMachine;
using Lumos.Common;
using Lumos.Entity;
using Lumos.Mvc;
using Lumos.BLL;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Data;
using System.Transactions;

namespace WebAgent.Controllers
{
    public class PosMachineController : WebBackController
    {
        //
        // GET: /PosMachine/
        public ViewResult List()
        {
            return View();
        }

        public ViewResult Details(int id)
        {
            DetailsViewModel model = new DetailsViewModel(id);
            return View(model);
        }


        public JsonResult GetList(PosMachineSearchCondition condition)
        {


            string fuselageNumber = condition.FuselageNumber.ToSearchString();
            string deviceId = condition.DeviceId.ToSearchString();
            string terminalNumber = condition.TerminalNumber.ToSearchString();
            var list = (from p in CurrentDb.PosMachine
                        where (fuselageNumber.Length == 0 || p.FuselageNumber.Contains(fuselageNumber)) &&
                                (deviceId.Length == 0 || p.DeviceId.Contains(deviceId)) &&
                                 (terminalNumber.Length == 0 || p.TerminalNumber.Contains(terminalNumber))
                                 && p.AgentId == this.CurrentUserId
                        select new { p.Id, p.DeviceId, p.FuselageNumber, p.TerminalNumber, p.AgentName, IsUse = (p.IsUse == true ? "是" : "否"), p.CreateTime, p.Version });

            int total = list.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            list = list.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }

    }
}