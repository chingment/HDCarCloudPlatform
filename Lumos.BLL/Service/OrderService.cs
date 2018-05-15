﻿using Lumos.BLL.Service.Model;
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

            var subtotalItem = new List<OrderConfirmSubtotalItemModel>();
            var skus = new List<OrderConfirmSkuModel>();

            decimal skuAmountByActual = 0;//总价
            decimal skuAmountByOriginal = 0;//总价
            decimal skuAmountByMemebr = 0;//普通用户总价
            decimal skuAmountByVip = 0;//会员用户总价

            if (confirm.Skus != null)
            {
                foreach (var item in confirm.Skus)
                {
                    var productSku = CurrentDb.ProductSku.Where(m => m.Id == item.SkuId).FirstOrDefault();
                    var product = CurrentDb.Product.Where(m => m.Id == productSku.ProductId).FirstOrDefault();
                    item.SkuMainImg = BizFactory.Product.GetMainImg(product.DispalyImgs);
                    item.SkuName = productSku.Name;
                    item.Price = productSku.Price.ToF2Price();
                    item.PriceByVip = (productSku.Price * 0.9m).ToF2Price();

                    skuAmountByOriginal += (productSku.Price * item.Quantity);
                    skuAmountByMemebr += (productSku.Price * item.Quantity);
                    skuAmountByVip += (productSku.Price * 0.9m * item.Quantity);

                    skus.Add(item);
                }
            }

            bool isVip = true;




            if (isVip)
            {
                skuAmountByActual = skuAmountByVip;

                var discount = "-" + (skuAmountByMemebr - skuAmountByVip).ToF2Price();
                subtotalItem.Add(new OrderConfirmSubtotalItemModel { ImgUrl = "", Name = "会员优惠", Amount = discount, IsDcrease = true });
            }
            else
            {
                skuAmountByActual = skuAmountByMemebr;
            }



            var orderBlock = new List<OrderBlock>();

            var skus_SelfExpress = skus.Where(m => m.ChannelType == Enumeration.ChannelType.Express).ToList();
            if (skus_SelfExpress.Count > 0)
            {
                var orderBlock_Express = new OrderBlock();
                orderBlock_Express.TagName = "快递商品";
                orderBlock_Express.Skus = skus_SelfExpress;
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
            }

            var skus_SelfPick = skus.Where(m => m.ChannelType == Enumeration.ChannelType.SelfPick).ToList();
            if (skus_SelfPick.Count > 0)
            {
                var orderBlock_SelfPick = new OrderBlock();
                orderBlock_SelfPick.TagName = "自提商品";
                orderBlock_SelfPick.Skus = skus_SelfPick;
                var shippingAddressModel2 = new ShippingAddressModel();
                shippingAddressModel2.Id = 0;
                shippingAddressModel2.Receiver = "邱庆文";
                shippingAddressModel2.PhoneNumber = "15989287032";
                shippingAddressModel2.Area = "";
                shippingAddressModel2.Address = "广州工商学院";
                shippingAddressModel2.CanSelectElse = false;

                orderBlock_SelfPick.ShippingAddress = shippingAddressModel2;

                orderBlock.Add(orderBlock_SelfPick);
            }

            model.Block = orderBlock;



            if (confirm.CouponId == null || confirm.CouponId.Count == 0)
            {
                var couponsCount = CurrentDb.Coupon.Where(m => m.UserId == confirm.UserId && m.Status == Entity.Enumeration.CouponStatus.WaitUse && m.EndTime > DateTime.Now).Count();

                if (couponsCount == 0)
                {
                    model.Coupon = new OrderConfirmCouponModel { TipMsg = "暂无可用优惠卷", TipType = TipType.NoCanUse };
                }
                else
                {
                    model.Coupon = new OrderConfirmCouponModel { TipMsg = string.Format("{0}个可用", couponsCount), TipType = TipType.CanUse };
                }
            }
            else
            {

                var coupons = CurrentDb.Coupon.Where(m => m.UserId == confirm.UserId && confirm.CouponId.Contains(m.Id)).ToList();

                foreach (var item in coupons)
                {
                    var amount = 0m;
                    switch (item.Type)
                    {
                        case Enumeration.CouponType.FullCut:
                        case Enumeration.CouponType.UnLimitedCut:
                            if (skuAmountByActual >= item.LimitAmount)
                            {
                                amount = -item.Discount;
                                skuAmountByActual = skuAmountByActual - item.Discount;

                                //subtotalItem.Add(new OrderConfirmSubtotalItemModel { ImgUrl = "", Name = item.Name, Amount = string.Format("{0}", amount.ToF2Price()), IsDcrease = true });

                                model.Coupon = new OrderConfirmCouponModel { TipMsg = string.Format("{0}", amount.ToF2Price()), TipType = TipType.InUse };

                            }

                            break;
                        case Enumeration.CouponType.Discount:

                            amount = skuAmountByActual - (skuAmountByActual * (item.Discount / 10));

                            skuAmountByActual = skuAmountByActual * (item.Discount / 10);

                            // subtotalItem.Add(new OrderConfirmSubtotalItemModel { ImgUrl = "", Name = item.Name, Amount = string.Format("{0}", amount.ToF2Price()), IsDcrease = true });
                            model.Coupon = new OrderConfirmCouponModel { TipMsg = string.Format("{0}", amount.ToF2Price()), TipType = TipType.InUse };
                            break;
                    }
                }

            }


            //subtotalItem.Add(new OrderConfirmSubtotalItemModel { ImgUrl = "", Name = "满5减3元", Amount = "-9", IsDcrease = true });
            //subtotalItem.Add(new OrderConfirmSubtotalItemModel { ImgUrl = "", Name = "优惠卷", Amount = "-10", IsDcrease = true });

            model.SubtotalItem = subtotalItem;


            model.ActualAmount = skuAmountByActual.ToF2Price();
            model.OriginalAmount = skuAmountByOriginal.ToF2Price();


            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", model);
        }

        public CustomJsonResult<Lumos.Entity.Order> GoSettle(int operater, OrderConfirmModel model)
        {
            var result = new CustomJsonResult<Lumos.Entity.Order>();

            Lumos.Entity.Order mod_Order = null;
            List<Lumos.Entity.OrderChildDetails> mod_OrderChildDetails = null;
            List<Lumos.Entity.OrderChildProductSkuDetails> mod_OrderChildProductSkuDetails = null;


            return result;
        }
    }
}