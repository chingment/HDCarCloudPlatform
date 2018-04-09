using Lumos;
using Lumos.BLL;
using Lumos.Entity;
using Lumos.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBack.Models.Biz.Product;

namespace WebBack.Controllers.Biz
{
    public class ProductController : OwnBaseController
    {
        // GET: Product
        public ActionResult List()
        {
            return View();
        }

        public ActionResult ListByInsurance()
        {
            return View();
        }

        public ViewResult Edit(int id)
        {
            EditViewModel model = new EditViewModel();
            model.LoadData(id);

            return View(model);
        }

        public ViewResult Add()
        {
            AddViewModel model = new AddViewModel();
            model.LoadData();

            return View(model);
        }

        public ViewResult AddByInsurance()
        {
            AddByInsuranceViewModel model = new AddByInsuranceViewModel();
            model.LoadData();

            return View(model);
        }

        public ViewResult SelectSpec()
        {
            return View();
        }



        public CustomJsonResult GetSelectSpec()
        {
            SelectSpecViewModel model = new SelectSpecViewModel();

            return Json(ResultType.Success, model, "");
        }

        [HttpPost]
        public CustomJsonResult GetList(WebBack.Models.Biz.Product.SearchCondition condition)
        {
            var query = (from u in CurrentDb.Product
                         where (condition.Name == null || u.Name.Contains(condition.Name)) &&
                         SqlFunctions.StringConvert((double)u.Type).StartsWith("1")
                         select new { u.Id, u.Name, u.MainImg, u.CreateTime, u.Supplier, u.ProductCategory });

            int total = query.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            var list1 = query.ToList();

            List<object> list = new List<object>();

            foreach (var item in list1)
            {
                var skus = CurrentDb.ProductSku.Where(m => m.ProductId == item.Id).ToList();

                list.Add(new
                {
                    Id = item.Id,
                    Name = item.Name,
                    MainImg = item.MainImg,
                    Supplier = item.Supplier,
                    ProductCategory = item.ProductCategory,
                    CreateTime = item.CreateTime,
                    skus = skus
                });
            }


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }

        [HttpPost]
        public CustomJsonResult GetListByInsurance(WebBack.Models.Biz.Product.SearchCondition condition)
        {
            // u.ProductCategoryId.ToString().StartsWith(condition.CategoryId.ToString()) &&
            var query = (from u in CurrentDb.Product
                         where (condition.Name == null || u.Name.Contains(condition.Name)) &&
                          u.ProductCategoryId.ToString().StartsWith("1002")
                         select new { u.Id, u.Name, u.MainImg, u.CreateTime, u.Supplier, u.ProductCategory });

            int total = query.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            var list1 = query.ToList();

            List<object> list = new List<object>();

            foreach (var item in list1)
            {
                var skus = CurrentDb.ProductSku.Where(m => m.ProductId == item.Id).ToList();

                list.Add(new
                {
                    Id = item.Id,
                    Name = item.Name,
                    MainImg = item.MainImg,
                    Supplier = item.Supplier,
                    ProductCategory = item.ProductCategory,
                    CreateTime = item.CreateTime,
                    skus = skus
                });
            }


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }

        [HttpPost]
        public CustomJsonResult Add(AddViewModel model)
        {
            var settings = new JsonSerializerSettings() { ContractResolver = new NullToEmptyStringResolver() };

            model.Product.DispalyImgs = Newtonsoft.Json.JsonConvert.SerializeObject(model.DispalyImgs, settings);

            return BizFactory.Product.Add(this.CurrentUserId, model.Product, model.ProductSku);
        }

        [HttpPost]
        public CustomJsonResult AddByInsurance(AddViewModel model)
        {
            var settings = new JsonSerializerSettings() { ContractResolver = new NullToEmptyStringResolver() };

            model.Product.DispalyImgs = Newtonsoft.Json.JsonConvert.SerializeObject(model.DispalyImgs, settings);

            return BizFactory.Product.AddByInsurance(this.CurrentUserId, model.Product, model.ProductSku);
        }

        [HttpPost]
        public CustomJsonResult Edit(EditViewModel model)
        {
            var settings = new JsonSerializerSettings() { ContractResolver = new NullToEmptyStringResolver() };

            model.Product.DispalyImgs = Newtonsoft.Json.JsonConvert.SerializeObject(model.DispalyImgs, settings);

            return BizFactory.Product.Edit(this.CurrentUserId, model.Product, model.ProductSku);
        }
    }
}