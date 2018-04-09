using Lumos.BLL;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBack.Models.Biz.ProductCategory;


namespace WebBack.Controllers.Biz
{
    public class ProductCategoryController : OwnBaseController
    {

        public ActionResult List()
        {
            WebBack.Models.Biz.ProductCategory.ListViewModel model = new WebBack.Models.Biz.ProductCategory.ListViewModel(1);
            return View(model);
        }

        public ActionResult Add()
        {
            AddViewModel mode = new AddViewModel();
            return View(mode);
        }

        public ActionResult Edit()
        {
            EditViewModel mode = new EditViewModel();
            return View(mode);
        }

        public ActionResult Sort()
        {
            return View();
        }

        public CustomJsonResult GetTreeList(int pId)
        {
            ProductCategory[] arr;
            if (pId == 0)
            {
                arr = CurrentDb.ProductCategory.Where(m => m.IsDelete == false).OrderByDescending(m => m.Priority).ToArray();
            }
            else
            {
                arr = CurrentDb.ProductCategory.Where(m => m.PId == pId && m.IsDelete == false).OrderByDescending(m => m.Priority).ToArray();

            }

            object json = ConvertToZTreeJson(arr, "id", "pid", "name", "menu");
            return Json(ResultType.Success, json);
        }

        public CustomJsonResult GetDetails(int id)
        {
            DetailsViewModel model = new DetailsViewModel(id);
            return Json(ResultType.Success, model, "");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public CustomJsonResult Add(AddViewModel model)
        {

            return BizFactory.ProductCategory.Add(this.CurrentUserId, model.ProductCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public CustomJsonResult Edit(EditViewModel model)
        {
            return BizFactory.ProductCategory.Edit(this.CurrentUserId, model.ProductCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public CustomJsonResult Delete(int[] ids)
        {
            return BizFactory.ProductCategory.Delete(this.CurrentUserId, ids);
        }

        public CustomJsonResult GetProductList(ProductSearchCondition condition)
        {
            string name = condition.Name.ToSearchString();

            var list = (from p in CurrentDb.Product
                        where
                        p.ProductCategoryId.ToString().StartsWith(condition.CategoryId.ToString())&&
                                 (name.Length == 0 || p.Name.Contains(name))
                        select new { p.Id, p.Name, p.MainImg, p.CreateTime, p.Supplier, p.ProductCategory });

            int total = list.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            list = list.OrderBy(r => r.Id).Skip(pageSize * (pageIndex)).Take(pageSize);

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public CustomJsonResult Sort(int pId)
        {

            for (int i = 0; i < Request.Form.Count; i++)
            {
                string name = Request.Form.AllKeys[i].ToString();
                if (name.IndexOf("categoryId") > -1)
                {
                    int id = int.Parse(name.Split('_')[1].Trim());
                    int priority = int.Parse(Request.Form[i].Trim());
                    var model = CurrentDb.ProductCategory.Where(m => m.Id == id).FirstOrDefault();
                    if (model != null)
                    {
                        model.Priority = priority;
                        CurrentDb.SaveChanges();
                    }
                }
            }
            return Json(ResultType.Success, "保存成功");

        }
    }
}