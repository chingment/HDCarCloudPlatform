using Lumos.BLL.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service
{
    public class ProductService : BaseProvider
    {

        public ProductKindModel GetKinds()
        {
            var productKindModels = new ProductKindModel();


            var productParentKindModels = new List<ProductParentKindModel>();

            var productKinds = CurrentDb.ProductKind.Where(m => m.Status == Entity.Enumeration.ProductKindStatus.Valid && m.IsDelete == false).ToList();

            var productParentKinds = productKinds.Where(m => m.PId == 1).ToList();
            foreach (var item in productParentKinds)
            {
                var productParentKindModel = new ProductParentKindModel();
                productParentKindModel.Id = item.Id;
                productParentKindModel.Name = item.Name;
                productParentKindModel.ImgUrl = item.MainImg;
                productParentKindModel.Selected = false;
                var productChildKinds = productKinds.Where(m => m.PId == item.Id).ToList();

                foreach (var item2 in productChildKinds)
                {
                    var productChildKindModel = new ProductChildKindModel();

                    productChildKindModel.Id = item2.Id;
                    productChildKindModel.Name = item2.Name;
                    productChildKindModel.ImgUrl = item2.MainImg;
                    productParentKindModel.Selected = false;

                    productParentKindModel.Child.Add(productChildKindModel);
                }

                productParentKindModels.Add(productParentKindModel);

            }

            var selectedCount = productParentKindModels.Where(m => m.Selected == true).Count();
            if (selectedCount == 0)
            {
                if (productParentKindModels.Count > 0)
                {
                    productParentKindModels[0].Selected = true;
                }
            }

            productKindModels.List = productParentKindModels;

            return productKindModels;
        }


        public ProductSkuModel GetSkuModel(int productSkuId)
        {
            var productSkuModel = new ProductSkuModel();

            var prdSku = CurrentDb.ProductSku.Where(m => m.Id == productSkuId).FirstOrDefault();
            var prd = CurrentDb.Product.Where(m => m.Id == prdSku.ProductId).FirstOrDefault();
            productSkuModel.Id = prdSku.Id;
            productSkuModel.ProductId = prdSku.ProductId;
            productSkuModel.Name = prdSku.Name;
            productSkuModel.ProductId = prdSku.ProductId;
            productSkuModel.DispalyImgs = BizFactory.Product.GetDispalyImgs(prd.DispalyImgs);
            productSkuModel.MainImg = BizFactory.Product.GetMainImg(prd.DispalyImgs);
            productSkuModel.UnitPrice = prdSku.Price;

            return productSkuModel;
        }


        public ProductSkuDetailsModel GetSkuDetals(int productSkuId)
        {
            var productSkuDetailsModel = new ProductSkuDetailsModel();

            var productSku = CurrentDb.ProductSku.Where(m => m.Id == productSkuId).FirstOrDefault();
            var product = CurrentDb.Product.Where(m => m.Id == productSku.ProductId).FirstOrDefault();

            productSkuDetailsModel.Id = productSku.Id;
            productSkuDetailsModel.Name = productSku.Name;
            productSkuDetailsModel.ProductId = productSku.ProductId;
            productSkuDetailsModel.ServiceDesc = product.ServiceDesc;
            productSkuDetailsModel.BriefIntro = product.BriefIntro;
            productSkuDetailsModel.Details = product.Details;
            productSkuDetailsModel.UnitPrice = productSku.Price.ToF2Price();
            productSkuDetailsModel.ShowPrice = productSku.ShowPrice.ToF2Price();


            try
            {
                if (!string.IsNullOrEmpty(product.DispalyImgs))
                {

                    var dispalyImgs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Lumos.Entity.ImgSet>>(product.DispalyImgs);

                    dispalyImgs = dispalyImgs.Where(m => m.ImgUrl != null && m.ImgUrl.Length > 0).ToList();
                    productSkuDetailsModel.DispalyImgs = dispalyImgs;

                }

                if (!string.IsNullOrEmpty(product.SpecsJson))
                {
                    productSkuDetailsModel.Specs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SpecModel>>(product.SpecsJson);
                }
            }
            catch
            {

            }

            return productSkuDetailsModel;
        }

    }
}
