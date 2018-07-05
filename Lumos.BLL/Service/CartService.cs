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
    public class CartService : BaseProvider
    {
        public CartPageDataModel GetPageData(int userId, int merchantId, int posMachineId)
        {

            var cartPageDataModel = new CartPageDataModel();


            cartPageDataModel.ShoppingData = GetShoppingData(userId, merchantId, posMachineId);

            return cartPageDataModel;
        }


        public CartShoppingDataModel GetShoppingData(int userId, int merchantId, int posMachineId)
        {
            var cartShoppingDataModel = new CartShoppingDataModel();

            var carts = CurrentDb.Cart.Where(m => m.UserId == userId && m.Status == Enumeration.CartStatus.WaitSettle).ToList();


            var skus = new List<CartProcudtSkuModel>();

            foreach (var item in carts)
            {
                var skuModel = ServiceFactory.Product.GetSkuModel(item.ProductSkuId);
                if (skuModel != null)
                {
                    var cartProcudtSkuModel = new CartProcudtSkuModel();
                    cartProcudtSkuModel.CartId = item.Id;
                    cartProcudtSkuModel.SkuId = skuModel.SkuId;
                    cartProcudtSkuModel.Name = skuModel.Name;
                    cartProcudtSkuModel.MainImg = skuModel.MainImg;
                    cartProcudtSkuModel.UnitPrice = skuModel.UnitPrice;
                    cartProcudtSkuModel.Quantity = item.Quantity;
                    cartProcudtSkuModel.SumPrice = item.Quantity * skuModel.UnitPrice;
                    cartProcudtSkuModel.Selected = item.Selected;
                    skus.Add(cartProcudtSkuModel);
                }
            }

            cartShoppingDataModel.Skus = skus;

            cartShoppingDataModel.Count = skus.Sum(m => m.Quantity);
            cartShoppingDataModel.SumPrice = skus.Sum(m => m.SumPrice);
            cartShoppingDataModel.SumPriceBySelected = skus.Where(m => m.Selected == true).Sum(m => m.SumPrice);
            cartShoppingDataModel.CountBySelected = skus.Where(m => m.Selected == true).Count();

            return cartShoppingDataModel;
        }




        private static readonly object operatelock = new object();
        public CustomJsonResult Operate(int operater, Enumeration.CartOperateType operate, int userId, int merchantId, int posMachineId, List<CartProcudtSkuByOperateModel> procudtSkus)
        {
            var result = new CustomJsonResult();

            lock (operatelock)
            {

                using (TransactionScope ts = new TransactionScope())
                {

                    foreach (var item in procudtSkus)
                    {
                        var mod_Cart = CurrentDb.Cart.Where(m => m.UserId == userId && m.ProductSkuId == item.SkuId && m.Status == Enumeration.CartStatus.WaitSettle).FirstOrDefault();

                        Log.Info("购物车操作：" + operate);
                        switch (operate)
                        {
                            case Enumeration.CartOperateType.Selected:
                                Log.Info("购物车操作：选择");

                                if (mod_Cart.Selected)
                                {
                                    mod_Cart.Selected = false;
                                }
                                else
                                {
                                    mod_Cart.Selected = true;
                                }

                                break;
                            case Enumeration.CartOperateType.Decrease:
                                Log.Info("购物车操作：减少");
                                if (mod_Cart.Quantity >= 2)
                                {
                                    mod_Cart.Quantity -= 1;
                                    mod_Cart.LastUpdateTime = this.DateTime;
                                    mod_Cart.Mender = operater;
                                }
                                break;
                            case Enumeration.CartOperateType.Increase:
                                Log.Info("购物车操作：增加");
                                var skuModel = ServiceFactory.Product.GetSkuModel(item.SkuId);

                                if (mod_Cart == null)
                                {
                                    mod_Cart = new Cart();
                                    mod_Cart.UserId = userId;
                                    mod_Cart.ProductSkuId = skuModel.SkuId;
                                    mod_Cart.ProductSkuName = skuModel.Name;
                                    mod_Cart.ProductSkuMainImg = skuModel.MainImg;
                                    mod_Cart.CreateTime = this.DateTime;
                                    mod_Cart.Creator = operater;
                                    mod_Cart.Quantity = 1;
                                    mod_Cart.Status = Enumeration.CartStatus.WaitSettle;
                                    CurrentDb.Cart.Add(mod_Cart);
                                }
                                else
                                {
                                    mod_Cart.Quantity += 1;
                                    mod_Cart.LastUpdateTime = this.DateTime;
                                    mod_Cart.Mender = operater;
                                }
                                break;
                            case Enumeration.CartOperateType.Delete:
                                Log.Info("购物车操作：删除");
                                mod_Cart.Status = Enumeration.CartStatus.Deleted;
                                mod_Cart.LastUpdateTime = this.DateTime;
                                mod_Cart.Mender = operater;
                                break;
                        }
                    }

                    CurrentDb.SaveChanges();

                    var cartShoppingDataModel = GetShoppingData(userId, merchantId, posMachineId);

                    ts.Complete();

                    result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", cartShoppingDataModel);
                }
            }


            return result;
        }


        public CustomJsonResult GetComfirmOrderData(int operater, int userId, int merchantId, int posMachineId, List<CartProcudtSkuByOperateModel> procudtSkus)
        {
            CartComfirmOrderDataModel model = new CartComfirmOrderDataModel();




            var skus = new List<CartProcudtSkuModel>();

            foreach (var item in procudtSkus)
            {
                var skuModel = ServiceFactory.Product.GetSkuModel(item.SkuId);
                if (skuModel != null)
                {
                    var cartProcudtSkuModel = new CartProcudtSkuModel();
                    cartProcudtSkuModel.CartId = item.CartId;
                    cartProcudtSkuModel.SkuId = skuModel.SkuId;
                    cartProcudtSkuModel.Name = skuModel.Name;
                    cartProcudtSkuModel.MainImg = skuModel.MainImg;
                    cartProcudtSkuModel.UnitPrice = skuModel.UnitPrice;
                    cartProcudtSkuModel.Quantity = item.Quantity;
                    cartProcudtSkuModel.SumPrice = item.Quantity * skuModel.UnitPrice;
                    skus.Add(cartProcudtSkuModel);
                }
            }

            model.Skus = skus;

            model.ActualAmount = skus.Sum(m => m.SumPrice).ToF2Price();

            var merchant = CurrentDb.Merchant.Where(m => m.Id == merchantId).FirstOrDefault();

            if (merchant != null)
            {
                model.ShippingAddress.Address = merchant.ContactAddress;
                model.ShippingAddress.Receiver = merchant.ContactName;
                model.ShippingAddress.PhoneNumber = merchant.ContactPhoneNumber;
                model.ShippingAddress.Area = merchant.Area;
                model.ShippingAddress.AreaCode = merchant.AreaCode;
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", model);
        }
    }
}
