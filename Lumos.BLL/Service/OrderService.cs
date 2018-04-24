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
                shippingAddressModel.TagName = "快递地址";
                shippingAddressModel.CanSelectElse = true;
                model.ShippingAddress.Add(shippingAddressModel);

            }

            var shippingAddressModel2 = new ShippingAddressModel();
            shippingAddressModel2.Id = 0;
            shippingAddressModel2.Receiver = "邱庆文";
            shippingAddressModel2.PhoneNumber = "15989287032";
            shippingAddressModel2.Area = "";
            shippingAddressModel2.Address = "广州工商学院";
            shippingAddressModel2.TagName = "自提地址";
            shippingAddressModel2.CanSelectElse = false;
            model.ShippingAddress.Add(shippingAddressModel2);

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", model);
        }
    }
}
