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

            var productKinds = CurrentDb.ProductKind.Where(m => m.Status == Entity.Enumeration.ProductKindStatus.Valid).ToList();

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
            productSkuModel.Id = prdSku.Id;
            productSkuModel.ProductId = prdSku.ProductId;
            productSkuModel.Name = prdSku.Name;
            productSkuModel.ProductId = prdSku.ProductId;
            productSkuModel.DispalyImgs = BizFactory.Product.GetDispalyImgs(prdSku.DispalyImgs);
            productSkuModel.MainImg = BizFactory.Product.GetMainImg(prdSku.DispalyImgs);
            productSkuModel.UnitPrice = prdSku.Price;

            return productSkuModel;
        }
    }
}
