using Lumos.BLL.Service.Model;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Service
{
    public class OrderService : BaseProvider
    {
        public CustomJsonResult Confrim(int operater, OrderConfirmModel confirm)
        {
            var result = new CustomJsonResult();

            var model = new OrderConfirmResultModel();

            if (confirm.Skus != null)
            {
                foreach (var item in confirm.Skus)
                {
                    var productSku = CurrentDb.ProductSku.Where(m => m.Id == item.ProductSkuId).FirstOrDefault();
                    var product = CurrentDb.Product.Where(m => m.Id == productSku.ProductId).FirstOrDefault();

                    item.ProductSkuMainImg = BizFactory.Product.GetMainImg(product.DispalyImgs);
                    item.ProductSkuName = productSku.Name;
                    item.Price = productSku.Price.ToF2Price();

                    model.Skus.Add(item);

                }
            }

            var shippingAddress = CurrentDb.ShippingAddress.Where(m => m.UserId == confirm.UserId && m.IsDefault == true).FirstOrDefault();
            if (shippingAddress != null)
            {
                var shippingAddressModel = new ShippingAddressModel();
                shippingAddressModel.Id = shippingAddress.Id;
                shippingAddressModel.Receiver = shippingAddress.Receiver;
                shippingAddressModel.PhoneNumber = shippingAddress.PhoneNumber;
                shippingAddressModel.Area = shippingAddress.Area;
                shippingAddressModel.Address = shippingAddress.Address;
                model.ShippingAddress = shippingAddressModel;

            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", model);
        }
    }
}
