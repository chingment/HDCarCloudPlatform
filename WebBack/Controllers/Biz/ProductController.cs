using Lumos.BLL;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBack.Models.Biz.Product;

namespace WebBack.Controllers.Biz
{
    [OwnAuthorize(PermissionCode.商品设置)]
    public class ProductController : WebBackController
    {
        // GET: Prodcut
        public ViewResult GoodsList()
        {
            return View();
        }

        public ViewResult AddGoods()
        {
            return View();
        }

        public ViewResult EditGoods(int id)
        {
            EditGoodsViewModel model = new EditGoodsViewModel(id);
            return View(model);
        }

        public ViewResult AppGoodsDetails()
        {
            return View();
        }

        public JsonResult GetGoodsList(ProductSearchCondition condition)
        {
            var query = (from p in CurrentDb.Product
                         where ((int)p.Type).ToString().StartsWith("1")
                         select new { p.Id, p.Name, p.Type, p.Price, p.MainImgUrl, p.CreateTime, p.Status });

            int total = query.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);



            List<object> list = new List<object>();

            foreach (var item in query)
            {
                list.Add(new
                {
                    Id = item.Id,
                    Name = item.Name,
                    Type = item.Type.GetCnName(),
                    Price = item.Price.ToPrice(),
                    ImgUrl = item.MainImgUrl,
                    LinkUrl = BizFactory.Product.GetLinkUrl(item.Type,"88888", item.Id),
                    Status = item.Status,
                    StatusName = item.Status.GetCnName(),
                    CreateTime = item.CreateTime

                });
            }


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }

        [HttpPost]
        public JsonResult AddGoods(AddGoodsViewModel model)
        {
            CustomJsonResult reuslt = new CustomJsonResult();
            model.Product.ElseImgUrls = Newtonsoft.Json.JsonConvert.SerializeObject(model.ElseImgUrls);
            reuslt = BizFactory.Product.Add(this.CurrentUserId, model.Product);

            return reuslt;
        }

        [HttpPost]
        public JsonResult EditGoods(EditGoodsViewModel model)
        {
            CustomJsonResult reuslt = new CustomJsonResult();
            model.Product.ElseImgUrls = Newtonsoft.Json.JsonConvert.SerializeObject(model.ElseImgUrls);
            reuslt = BizFactory.Product.Edit(this.CurrentUserId, model.Product);

            return reuslt;
        }

        [HttpPost]
        public JsonResult OffLine(int id)
        {
            CustomJsonResult reuslt = new CustomJsonResult();

            reuslt = BizFactory.Product.OffLine(this.CurrentUserId, id);

            return reuslt;
        }

    }
}