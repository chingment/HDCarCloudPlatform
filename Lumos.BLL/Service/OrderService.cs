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


            var skus = new List<OrderConfirmSkuModel>();

            if (confirm.Skus != null)
            {
                foreach (var item in confirm.Skus)
                {
                    var productSku = CurrentDb.ProductSku.Where(m => m.Id == item.ProductSkuId).FirstOrDefault();
                    var product = CurrentDb.Product.Where(m => m.Id == productSku.ProductId).FirstOrDefault();

                    item.ProductSkuMainImg = BizFactory.Product.GetMainImg(product.DispalyImgs);
                    item.ProductSkuName = productSku.Name;
                    item.Price = productSku.Price.ToF2Price();

                    skus.Add(item);

                }
            }


            var orderBlock = new List<OrderBlock>();

            var orderBlock_Express = new OrderBlock();
            orderBlock_Express.TagName = "快递商品";
            orderBlock_Express.Skus = skus;
            var shippingAddressModel = new ShippingAddressModel();
            var shippingAddress = CurrentDb.ShippingAddress.Where(m => m.UserId == confirm.UserId && m.IsDefault == true).FirstOrDefault();
            if (shippingAddress != null)
            {
                shippingAddressModel.Id = shippingAddress.Id;
                shippingAddressModel.Receiver = shippingAddress.Receiver;
                shippingAddressModel.PhoneNumber = shippingAddress.PhoneNumber;
                shippingAddressModel.Area = shippingAddress.Area;
                shippingAddressModel.Address = shippingAddress.Address;
                shippingAddressModel.CanSelectElse = true;
            }
            orderBlock_Express.ShippingAddress = shippingAddressModel;
            orderBlock.Add(orderBlock_Express);


            var orderBlock_SelfPick = new OrderBlock();
            orderBlock_SelfPick.TagName = "自提商品";
            orderBlock_SelfPick.Skus = skus;
            var shippingAddressModel2 = new ShippingAddressModel();
            shippingAddressModel2.Id = 0;
            shippingAddressModel2.Receiver = "邱庆文";
            shippingAddressModel2.PhoneNumber = "15989287032";
            shippingAddressModel2.Area = "";
            shippingAddressModel2.Address = "广州工商学院";
            shippingAddressModel2.CanSelectElse = false;

            orderBlock_SelfPick.ShippingAddress = shippingAddressModel2;

            orderBlock.Add(orderBlock_SelfPick);

            model.OrderBlock = orderBlock;



            var subtotalItem = new List<SubtotalItem>();


            subtotalItem.Add(new SubtotalItem { ImgUrl = "", Name = "满5减3元", Amount = "-9", IsDcrease = true });
            subtotalItem.Add(new SubtotalItem { ImgUrl = "", Name = "优惠卷", Amount = "-10", IsDcrease = true });

            model.SubtotalItem = subtotalItem;


            model.ActualAmount = "101元";
            model.OriginalAmount = "120元";


            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", model);
        }
    }
}
