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
        public CartModel GetData(int userId)
        {
            var cartModel = new CartModel();


            var carts = CurrentDb.Cart.Where(m => m.UserId == userId && m.Status == Enumeration.CartStatus.WaitSettle).ToList();


            var skus = new List<CartProcudtSkuListModel>();

            foreach (var item in carts)
            {
                var skuModel = ServiceFactory.Product.GetSkuModel(item.ProductSkuId);
                if (skuModel != null)
                {
                    var cartProcudtSkuModel = new CartProcudtSkuListModel();
                    cartProcudtSkuModel.CartId = item.Id;
                    cartProcudtSkuModel.SkuId = skuModel.SkuId;
                    cartProcudtSkuModel.SkuName = skuModel.Name;
                    cartProcudtSkuModel.SkuMainImg = skuModel.MainImg;
                    cartProcudtSkuModel.UnitPrice = skuModel.UnitPrice;
                    cartProcudtSkuModel.Quantity = item.Quantity;
                    cartProcudtSkuModel.SumPrice = item.Quantity * skuModel.UnitPrice;
                    cartProcudtSkuModel.Selected = item.Selected;
                    cartProcudtSkuModel.ChannelId = item.ChannelId;
                    cartProcudtSkuModel.ChannelType = item.ChannelType;
                    skus.Add(cartProcudtSkuModel);
                }
            }


            var channels = (from c in carts select new { c.ChannelId, c.ChannelType }).Distinct();


            foreach (var item in channels)
            {

                var carBlock = new CartBlock();
                carBlock.ChannelId = item.ChannelId;
                carBlock.ChannelType = item.ChannelType;
                carBlock.Skus = skus.Where(m => m.ChannelId == item.ChannelId && m.ChannelType == item.ChannelType).ToList();

                switch (item.ChannelType)
                {
                    case Enumeration.ChannelType.SelfPick:
                        carBlock.TagName = "自提商品";
                        break;
                    case Enumeration.ChannelType.Express:
                        carBlock.TagName = "快递商品";
                        break;
                }

                cartModel.Block.Add(carBlock);
            }



            cartModel.Count = skus.Sum(m => m.Quantity);
            cartModel.SumPrice = skus.Sum(m => m.SumPrice);
            cartModel.SumPriceBySelected = skus.Where(m => m.Selected == true).Sum(m => m.SumPrice);
            cartModel.CountBySelected = skus.Where(m => m.Selected == true).Count();

            return cartModel;
        }

        private static readonly object operatelock = new object();
        public CustomJsonResult Operate(int operater, Enumeration.CartOperateType operate, int userId, List<CartProcudtSkuListByOperateModel> procudtSkus)
        {
            var result = new CustomJsonResult();

            lock (operatelock)
            {

                using (TransactionScope ts = new TransactionScope())
                {

                    foreach (var item in procudtSkus)
                    {
                        var mod_Cart = CurrentDb.Cart.Where(m => m.UserId == userId && m.ProductSkuId == item.SkuId && m.ChannelId == item.ChannelId && m.ChannelType == item.ChannelType && m.Status == Enumeration.CartStatus.WaitSettle).FirstOrDefault();

                        Log.Info("购物车操作：" + operate);
                        switch (operate)
                        {
                            case Enumeration.CartOperateType.Selected:
                                Log.Info("购物车操作：选择");
                                mod_Cart.Selected = item.Selected;
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
                                    mod_Cart.ChannelId = item.ChannelId;
                                    mod_Cart.ChannelType = item.ChannelType;
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

                    var cartModel = GetData(userId);

                    ts.Complete();

                    result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", cartModel);
                }
            }


            return result;
        }
    }
}
