﻿using Lumos.Entity;
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
        public CustomJsonResult Submit(int operater, OrderToShopping orderToShopping, OrderToShoppingGoodsDetails orderToShoppingGoodsDetails)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var clientUser = CurrentDb.SysClientUser.Where(m => m.Id == orderToShopping.UserId).FirstOrDefault();
                var merchant = CurrentDb.Merchant.Where(m => m.Id == clientUser.MerchantId).FirstOrDefault();



                var l_OrderToShopping = CurrentDb.OrderToShopping.Where(m => m.Id == orderToShopping.Id).FirstOrDefault();
                if (l_OrderToShopping == null)
                {
                    l_OrderToShopping = new OrderToShopping();

                    l_OrderToShopping.SalesmanId = merchant.SalesmanId ?? 0;
                    l_OrderToShopping.AgentId = merchant.AgentId ?? 0;
                    l_OrderToShopping.Type = Enumeration.OrderType.Goods;
                    l_OrderToShopping.TypeName = Enumeration.OrderType.Goods.GetCnName();
                    l_OrderToShopping.Status = Enumeration.OrderStatus.WaitPay;
                    l_OrderToShopping.SubmitTime = this.DateTime;
                    l_OrderToShopping.CreateTime = this.DateTime;
                    l_OrderToShopping.Creator = operater;

                    CurrentDb.OrderToShopping.Add(l_OrderToShopping);

                    SnModel snModel = Sn.Build(SnType.OrderToGoods, l_OrderToShopping.Id);
                    l_OrderToShopping.Sn = snModel.Sn;
                    CurrentDb.SaveChanges();
                }
                else
                {
                    l_OrderToShopping.LastUpdateTime = this.DateTime;
                    l_OrderToShopping.Mender = operater;
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

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "提交成功", null);
            }


            return result;
        }
    }
}
