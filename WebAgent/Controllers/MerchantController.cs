using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAgent.Models.Biz.Merchant;
using Lumos.Common;
using Lumos.BLL;

namespace WebAgent.Controllers.Biz
{
    public class MerchantController : OwnBaseController
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


        public JsonResult GetList(MerchantSearchCondition condition)
        {
            string clientCode = condition.ClientCode.ToSearchString();
            string yYZZ_Name = condition.YYZZ_Name.ToSearchString();
            string yYZZ_RegisterNo = condition.YYZZ_RegisterNo.ToSearchString();
            var query = (from m in CurrentDb.Merchant
                         join u in CurrentDb.SysClientUser on m.UserId equals u.Id

                         where
                         m.AgentId == this.CurrentUserId &&
                         (clientCode.Length == 0 || u.ClientCode.Contains(clientCode)) &&
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

    }
}