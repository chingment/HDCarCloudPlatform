using Lumos.BLL.Service;
using Lumos.BLL.Service.Model;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL
{
    public class OrderToShoppingProvider : BaseProvider
    {
        public CustomJsonResult Submit(int operater, SubmitShoppingPms pms)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var clientUser = CurrentDb.SysClientUser.Where(m => m.Id == pms.UserId).FirstOrDefault();
                var merchant = CurrentDb.Merchant.Where(m => m.Id == clientUser.MerchantId).FirstOrDefault();

                var l_OrderToShopping = CurrentDb.OrderToShopping.Where(m => m.Id == pms.OrderId).FirstOrDefault();
                if (l_OrderToShopping == null)
                {
                    l_OrderToShopping = new OrderToShopping();
                    l_OrderToShopping.UserId = pms.UserId;
                    l_OrderToShopping.MerchantId = pms.MerchantId;
                    l_OrderToShopping.PosMachineId = pms.PosMachineId;
                    l_OrderToShopping.SalesmanId = merchant.SalesmanId ?? 0;
                    l_OrderToShopping.AgentId = merchant.AgentId ?? 0;
                    l_OrderToShopping.Type = Enumeration.OrderType.Goods;
                    l_OrderToShopping.TypeName = Enumeration.OrderType.Goods.GetCnName();
                    l_OrderToShopping.Status = Enumeration.OrderStatus.WaitPay;
                    l_OrderToShopping.Recipient = pms.RecipientAddress.Recipient;
                    l_OrderToShopping.RecipientPhoneNumber = pms.RecipientAddress.PhoneNumber;
                    l_OrderToShopping.RecipientAreaCode = pms.RecipientAddress.AreaCode;
                    l_OrderToShopping.RecipientAreaName = pms.RecipientAddress.AreaName;
                    l_OrderToShopping.RecipientAddress = pms.RecipientAddress.Address;
                    l_OrderToShopping.SubmitTime = this.DateTime;
                    l_OrderToShopping.CreateTime = this.DateTime;
                    l_OrderToShopping.Creator = operater;
                    CurrentDb.OrderToShopping.Add(l_OrderToShopping);
                    CurrentDb.SaveChanges();
                    SnModel snModel = Sn.Build(SnType.OrderToGoods, l_OrderToShopping.Id);
                    l_OrderToShopping.Sn = snModel.Sn;
                    CurrentDb.SaveChanges();

                    decimal sumPrice = 0;
                    foreach (var item in pms.Skus)
                    {
                        var skuModel = ServiceFactory.Product.GetSkuModel(item.SkuId);
                        if (skuModel != null)
                        {
                            OrderToShoppingGoodsDetails l_OrderToShoppingGoodsDetails = new OrderToShoppingGoodsDetails();
                            l_OrderToShoppingGoodsDetails.OrderId = l_OrderToShopping.Id;
                            l_OrderToShoppingGoodsDetails.CartId = item.CartId;
                            l_OrderToShoppingGoodsDetails.ProductSkuId = skuModel.SkuId;
                            l_OrderToShoppingGoodsDetails.ProductSkuImgUrl = skuModel.MainImg;
                            l_OrderToShoppingGoodsDetails.ProductSkuName = skuModel.Name;
                            l_OrderToShoppingGoodsDetails.Quantity = item.Quantity;
                            l_OrderToShoppingGoodsDetails.UnitPrice = skuModel.UnitPrice;
                            l_OrderToShoppingGoodsDetails.SumPrice = item.Quantity * skuModel.UnitPrice;
                            l_OrderToShoppingGoodsDetails.CreateTime = this.DateTime;
                            l_OrderToShoppingGoodsDetails.Creator = operater;
                            CurrentDb.OrderToShoppingGoodsDetails.Add(l_OrderToShoppingGoodsDetails);
                            CurrentDb.SaveChanges();

                            sumPrice += l_OrderToShoppingGoodsDetails.SumPrice;
                        }

                        var cart = CurrentDb.Cart.Where(m => m.Id == item.CartId).FirstOrDefault();
                        if (cart != null)
                        {
                            cart.Status = Enumeration.CartStatus.Settling;
                            cart.LastUpdateTime = this.DateTime;
                            cart.Mender = operater;
                            CurrentDb.SaveChanges();
                        }


                    }

                    l_OrderToShopping.Price = sumPrice;
                }
                else
                {
                    l_OrderToShopping.Recipient = pms.RecipientAddress.Recipient;
                    l_OrderToShopping.RecipientPhoneNumber = pms.RecipientAddress.PhoneNumber;
                    l_OrderToShopping.RecipientAreaCode = pms.RecipientAddress.AreaCode;
                    l_OrderToShopping.RecipientAreaName = pms.RecipientAddress.AreaName;
                    l_OrderToShopping.RecipientAddress = pms.RecipientAddress.Address;
                }



                OrderConfirmInfo yOrder = new OrderConfirmInfo();

                yOrder.OrderId = l_OrderToShopping.Id;
                yOrder.OrderSn = l_OrderToShopping.Sn;
                yOrder.Remarks = l_OrderToShopping.Remarks;
                yOrder.OrderType = l_OrderToShopping.Type;
                yOrder.OrderTypeName = l_OrderToShopping.TypeName;

                yOrder.ConfirmField.Add(new Entity.AppApi.OrderField("订单编号", l_OrderToShopping.Sn.NullToEmpty()));
                yOrder.ConfirmField.Add(new Entity.AppApi.OrderField("支付金额", string.Format("{0}元", l_OrderToShopping.Price.NullToEmpty())));


                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "提交成功", yOrder);
            }


            return result;
        }


        public CustomJsonResult DealtWaitSend(int operater, OrderToShopping orderToShopping)
        {

            CustomJsonResult result = new CustomJsonResult();


            var l_orderToShopping = CurrentDb.OrderToShopping.Where(m => m.Id == orderToShopping.Id).FirstOrDefault();


            if (l_orderToShopping == null)
            {
                return new CustomJsonResult(ResultType.Success, "找不到该订单");
            }


            l_orderToShopping.ExpressCompany = orderToShopping.ExpressCompany;
            l_orderToShopping.ExpressOrderNo = orderToShopping.ExpressOrderNo;
            l_orderToShopping.FollowStatus = (int)Enumeration.ShoppingFollowStatus.Sended;
            l_orderToShopping.LastUpdateTime = this.DateTime;
            l_orderToShopping.Mender = operater;

            CurrentDb.SaveChanges();

            result = new CustomJsonResult(ResultType.Success, "提交成功");
            return result;
        }
    }
}
