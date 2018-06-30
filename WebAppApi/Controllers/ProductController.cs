using Lumos.BLL;
using Lumos.BLL.Service;
using Lumos.BLL.Service.Model;
using Lumos.DAL;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using Lumos.Entity.AppApi;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAppApi.Models;
using WebAppApi.Models.Account;
using WebAppApi.Models.CarService;

namespace WebAppApi.Controllers
{
    [BaseAuthorizeAttribute]
    public class ProductController : OwnBaseApiController
    {

        [HttpGet]
        public APIResponse GetSkuList(int userId, int merchantId, int posMachineId, int pageIndex, Enumeration.ProductType type, int categoryId, int kindId, string name)
        {
            var query = (from o in CurrentDb.Product
                         where
                         o.Status == Enumeration.ProductStatus.OnLine
                         select new { o.Id, o.BriefIntro, o.Type, o.Name, o.IsHot, o.IsMultiSpec, o.ProductCategoryId, o.ProductKindIds, o.DispalyImgs, o.Details, o.CreateTime }
                         );

            if (name != null && name.Length > 0)
            {
                query = query.Where(p => p.Name.Contains(name));
            }

            if (type != Enumeration.ProductType.Unknow)
            {
                query = query.Where(p => p.Type == type);
            }

            if (categoryId != 0)
            {
                query = query.Where(p => p.ProductCategoryId.ToString().StartsWith(categoryId.ToString()));
            }

            if (kindId != 0)
            {
                string strkindId = BizFactory.ProductSku.BuildProductKindIdForSearch(kindId.ToString());

                query = query.Where(p => SqlFunctions.CharIndex(strkindId, p.ProductKindIds) > 0);
            }

            int pageSize = 10;

            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            var list = query.ToList();

            List<ProductSkuModel> model = new List<ProductSkuModel>();

            foreach (var m in list)
            {
                var productSku = CurrentDb.ProductSku.Where(q => q.ProductId == m.Id).FirstOrDefault();
                if (productSku != null)
                {
                    ProductSkuModel productModel = new ProductSkuModel();
                    productModel.SkuId = productSku.Id;
                    productModel.Name = m.Name;
                    productModel.BriefIntro = m.BriefIntro;
                    productModel.IsHot = m.IsHot;
                    productModel.UnitPrice = productSku.Price;
                    productModel.ShowPrice = productSku.ShowPrice;
                    productModel.DispalyImgs = BizFactory.ProductSku.GetDispalyImgs(m.DispalyImgs);
                    productModel.MainImg = BizFactory.ProductSku.GetMainImg(m.DispalyImgs);
                    productModel.DetailsDesc = m.Details;
                    model.Add(productModel);
                }
            }

            APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "", Data = model };

            return new APIResponse(result);
        }

        //[HttpGet]
        //public APIResponse GetSkuDetails(int userId, int merchantId, int productSkuId)
        //{
        //    var model = ServiceFactory.Product.GetSkuModel(productSkuId);

        //    APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "", Data = model };

        //    return new APIResponse(result);
        //}

        [HttpGet]
        public APIResponse GetKinds(int userId, int merchantId, int posMachineId)
        {

            var model = ServiceFactory.Product.GetKinds();

            APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "获取成功", Data = model };

            return new APIResponse(result);
        }

    }
}