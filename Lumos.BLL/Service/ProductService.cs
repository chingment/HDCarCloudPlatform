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

        public List<ProductKindModel> GetKinds()
        {
            var productKindModels = new List<ProductKindModel>();

            var productKinds = CurrentDb.ProductKind.Where(m => m.Status == Entity.Enumeration.ProductKindStatus.Valid).ToList();

            var productParentKinds = productKinds.Where(m => m.PId == 1).ToList();
            foreach (var item in productKinds)
            {
                var productKindModel = new ProductKindModel();
                productKindModel.Id = item.Id;
                productKindModel.Name = item.Name;
                productKindModel.ImgUrl = item.MainImg;

                var productChildKinds = productKinds.Where(m => m.PId == item.Id).ToList();

                foreach (var item2 in productChildKinds)
                {
                    var productChildKindModel = new ProductChildKindModel();

                    productChildKindModel.Id = item2.Id;
                    productChildKindModel.Name = item2.Name;
                    productChildKindModel.ImgUrl = item2.MainImg;

                    productKindModel.Child.Add(productChildKindModel);
                }

                productKindModels.Add(productKindModel);

            }

            return productKindModels;
        }
    }
}
