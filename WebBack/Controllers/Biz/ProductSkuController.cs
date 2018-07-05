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
    public class ProductSkuController : OwnBaseController
    {
        // GET: Product
        public ActionResult ListByGoods()
        {
            return View();
        }

        public ActionResult ListByInsurance()
        {
            return View();
        }

        public ViewResult EditByGoods(int id)
        {
            EditViewModel model = new EditViewModel();
            model.LoadData(id);

            return View(model);
        }

        public ViewResult AddByGoods()
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

        [HttpPost]
        public CustomJsonResult GetListByGoods(WebBack.Models.Biz.Product.SearchCondition condition)
        {
            var query = (from u in CurrentDb.ProductSku

                         join p in CurrentDb.Product on u.ProductId equals p.Id

                         where (condition.Name == null || u.Name.Contains(condition.Name)) &&

                           p.Type == Enumeration.ProductType.Goods

                         select new { u.Id, u.Name, p.MainImg, p.CreateTime, p.Supplier, p.ProductCategory, u.Price, p.ProductKindNames });

            int total = query.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            var list1 = query.ToList();

            List<object> list = new List<object>();

            foreach (var item in list1)
            {
                list.Add(new
                {
                    Id = item.Id,
                    Name = item.Name,
                    MainImg = item.MainImg,
                    Supplier = item.Supplier,
                    ProductCategory = item.ProductCategory,
                    CreateTime = item.CreateTime,
                    Price = item.Price,
                    ProductKindNames = item.ProductKindNames
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
        public CustomJsonResult AddByGoods(AddViewModel model)
        {
            var settings = new JsonSerializerSettings() { ContractResolver = new NullToEmptyStringResolver() };

            model.Product.DisplayImgs = Newtonsoft.Json.JsonConvert.SerializeObject(model.DispalyImgs, settings);

            return BizFactory.ProductSku.AddByGoods(this.CurrentUserId, model.Product, model.ProductSku);
        }

        [HttpPost]
        public CustomJsonResult AddByInsurance(AddViewModel model)
        {
            var settings = new JsonSerializerSettings() { ContractResolver = new NullToEmptyStringResolver() };

            model.Product.DisplayImgs = Newtonsoft.Json.JsonConvert.SerializeObject(model.DispalyImgs, settings);

            return BizFactory.ProductSku.AddByInsurance(this.CurrentUserId, model.Product, model.ProductSku);
        }

        [HttpPost]
        public CustomJsonResult EditByGoods(EditViewModel model)
        {
            var settings = new JsonSerializerSettings() { ContractResolver = new NullToEmptyStringResolver() };

            model.Product.DisplayImgs = Newtonsoft.Json.JsonConvert.SerializeObject(model.DispalyImgs, settings);

            return BizFactory.ProductSku.EditByGoods(this.CurrentUserId, model.Product, model.ProductSku);
        }
    }
}