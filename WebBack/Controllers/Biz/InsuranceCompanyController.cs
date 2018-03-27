using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lumos.Common;
using Lumos.Entity;
using Lumos.Mvc;
using Lumos.BLL;
using WebBack.Models.Biz.InsuranceCompany;
//using WebBack.Models.Biz.ExtendedApp;
using WebBack.Models;

namespace WebBack.Controllers.Biz
{
    [OwnAuthorize(PermissionCode.保险公司设置)]
    public class InsuranceCompanyController : OwnBaseController
    {
        //
        // GET: /InsuranceCompany/
        public ActionResult Index()
        {
            return View();
        }


        public ViewResult List()
        {
            return View();
        }
        public ViewResult Add()
        {
            return View();
        }


        [HttpPost]
        public CustomJsonResult Add(AddViewModel model)
        {
            CustomJsonResult reuslt = new CustomJsonResult();

            reuslt = BizFactory.InsuranceCompany.Add(this.CurrentUserId, model.InsuranceCompany);

            return reuslt;
        }
        public ViewResult Edit(int id=0)
        {
            EditViewModel model = new EditViewModel(id);
            return View(model);
        }

        [HttpPost]
        public CustomJsonResult Edit(AddViewModel model)
        {
            CustomJsonResult reuslt = new CustomJsonResult();

            reuslt = BizFactory.InsuranceCompany.Edit(this.CurrentUserId, model.InsuranceCompany);

            return reuslt;
        }


        public CustomJsonResult GetList(BaseSearchCondition condition)
        {
            string name = condition.Name.ToSearchString();
            var query = (from i in CurrentDb.InsuranceCompany
                         where
                                 (name.Length == 0 || i.Name.Contains(name))

                         select new { i.Id, i.ImgUrl, i.Name, i.LastUpdateTime, i.CreateTime });

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
                    item.Name,
                    item.CreateTime,
                });
            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }

	}
}