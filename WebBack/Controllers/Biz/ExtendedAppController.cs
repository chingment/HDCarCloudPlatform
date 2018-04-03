using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBack.Models.Biz.ExtendedApp;
using Lumos.Common;
using Lumos.Entity;
using Lumos.Mvc;
using Lumos.BLL;

namespace WebBack.Controllers.Biz
{
    public class ExtendedAppController : OwnBaseController
    {
        public ViewResult List()
        {
            return View();
        }

        public ViewResult Details()
        {
            return View();
        }

        public CustomJsonResult GetList(SearchCondition condition)
        {
            string name = condition.Name.ToSearchString();
            var query = (from e in CurrentDb.ExtendedApp
                         where
                                 (name.Length == 0 || e.Name.Contains(name))

                         select new { e.Id, e.ImgUrl, e.LinkUrl, e.Name, e.Status, e.CreateTime, e.IsDisplay });

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
                    item.ImgUrl,
                    item.LinkUrl,
                    item.Name,
                    Status = item.Status.GetCnName(),
                    item.CreateTime

                });
            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }

        [OwnAuthorize(PermissionCode.扩展应用申请)]
        [HttpPost]
        public CustomJsonResult Add(ApplyOnViewModel model)
        {
            CustomJsonResult reuslt = new CustomJsonResult();

            reuslt = BizFactory.ExtendedApp.Add(this.CurrentUserId, model.ExtendedApp);

            return reuslt;
        }

    }
}