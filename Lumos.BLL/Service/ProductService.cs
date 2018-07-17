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

        public ProductKindPageModel GetKindPageData()
        {
            var productKindModels = new ProductKindPageModel();

            productKindModels.List = GetKinds();

            var productParentKindModels = new List<ProductKindModel>();

            return productKindModels;
        }


        public List<ProductKindModel> GetKinds()
        {

            var productParentKindModels = new List<ProductKindModel>();

            var productKinds = CurrentDb.ProductKind.Where(m => m.Status == Entity.Enumeration.ProductKindStatus.Valid && m.IsDelete == false).ToList();

            var productParentKinds = productKinds.Where(m => m.PId == 1).ToList();
            foreach (var item in productParentKinds)
            {
                var productParentKindModel = new ProductKindModel();
                productParentKindModel.Id = item.Id;
                productParentKindModel.Name = item.Name;
                productParentKindModel.ImgUrl = item.MainImg;
                productParentKindModel.Banners.Add(new BannerModel() { ImgUrl = item.MainImg });
                productParentKindModel.Selected = false;

                var productChildKinds = productKinds.Where(m => m.PId == item.Id).ToList();

                foreach (var item2 in productChildKinds)
                {
                    var productChildKindModel = new ProductChildKindModel();
                    productChildKindModel.Id = item2.Id;
                    productChildKindModel.Name = item2.Name;
                    productChildKindModel.ImgUrl = item2.MainImg;
                    productParentKindModel.Selected = false;
                    productParentKindModel.Childs.Add(productChildKindModel);
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

            return productParentKindModels;
        }

        public ProductSkuModel GetSkuModel(int productSkuId)
        {
            var productSkuModel = new ProductSkuModel();

            var productSku = CurrentDb.ProductSku.Where(m => m.Id == productSkuId).FirstOrDefault();
            var product = CurrentDb.Product.Where(m => m.Id == productSku.ProductId).FirstOrDefault();

            productSkuModel.SkuId = productSku.Id;
            productSkuModel.Name = productSku.Name;
            productSkuModel.ServiceDesc = product.ServiceDesc;
            productSkuModel.BriefIntro = product.BriefIntro;
            productSkuModel.DetailsDesc = product.Details;
            productSkuModel.UnitPrice = productSku.Price;
            productSkuModel.ShowPrice = productSku.ShowPrice;

            try
            {
                if (!string.IsNullOrEmpty(product.DispalyImgs))
                {
                    productSkuModel.DisplayImgs = BizFactory.ProductSku.GetDispalyImgs(product.DispalyImgs);
                    productSkuModel.DispalyImgs = BizFactory.ProductSku.GetDispalyImgs(product.DispalyImgs);
                    productSkuModel.MainImg = BizFactory.ProductSku.GetMainImg(product.DispalyImgs);
                }

                if (!string.IsNullOrEmpty(product.SpecsJson))
                {
                    productSkuModel.Specs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SpecModel>>(product.SpecsJson);
                }
            }
            catch
            {

            }

            return productSkuModel;
        }

    }
}
