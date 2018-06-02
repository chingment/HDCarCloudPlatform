using Lumos.BLL;
using Lumos.Entity;
using Lumos.Entity.AppApi;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebAppApi.Models.Lllegal;

namespace WebAppApi.Controllers
{
    [BaseAuthorizeAttribute]
    public class InsPrdController : OwnBaseApiController
    {
        [HttpGet]
        public APIResponse GetPlan(int userId, int merchantId, int posMachineId, int productId)
        {
            var product = CurrentDb.Product.Where(m => m.Id == productId).FirstOrDefault();
            var productSkus = CurrentDb.ProductSku.Where(m => m.ProductId == productId).ToList();

            InsPlanModel model = new InsPlanModel();
            model.BannerImgUrl = product.MainImg;
            model.ProductId = productId;
            model.ProductName = product.Name;
            model.CompanyId = product.SupplierId;
            model.CompanyName = product.Supplier;

            foreach (var item in productSkus)
            {
                InsPlanProductSkuModel insPlanProductSkuModel = new InsPlanProductSkuModel();

                insPlanProductSkuModel.ProductSkuId = item.Id;
                insPlanProductSkuModel.ProductSkuName = item.Name;
                insPlanProductSkuModel.Price = item.Price.ToF2Price();

                if (!string.IsNullOrEmpty(item.AttrItems))
                {
                    insPlanProductSkuModel.AttrItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ItemField>>(item.AttrItems);
                }

                model.ProductSkus.Add(insPlanProductSkuModel);

            }

            return ResponseResult(ResultType.Success, ResultCode.Success, "获取成功", model);
        }
    }
}