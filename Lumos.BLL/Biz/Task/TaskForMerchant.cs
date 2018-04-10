using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Biz.Task
{
    public class TaskForMerchant : BaseProvider, ITask
    {
        public CustomJsonResult Run()
        {
            CustomJsonResult result = new CustomJsonResult();


            var merchantPosMachines = CurrentDb.MerchantPosMachine.Where(m => m.ExpiryTime < DateTime.Now).ToList();

            Log.InfoFormat("到期的机器数量有：{0}", merchantPosMachines.Count);

            foreach (var merchantPosMachine in merchantPosMachines)
            {
                merchantPosMachine.Status = Enumeration.MerchantPosMachineStatus.Expiry;

                var orderToServiceFee = CurrentDb.OrderToServiceFee.Where(m => m.Status == Enumeration.OrderStatus.WaitPay && m.Type == Entity.Enumeration.OrderType.PosMachineServiceFee && m.UserId == merchantPosMachine.UserId && m.MerchantId == merchantPosMachine.MerchantId && m.PosMachineId == merchantPosMachine.PosMachineId).FirstOrDefault();
                if (orderToServiceFee == null)
                {

                    CalculateServiceFee calculateServiceFee = new CalculateServiceFee();

                    orderToServiceFee = new OrderToServiceFee();

                    orderToServiceFee.MerchantId = merchantPosMachine.MerchantId;
                    orderToServiceFee.PosMachineId = merchantPosMachine.PosMachineId;
                    orderToServiceFee.UserId = merchantPosMachine.UserId;
                    orderToServiceFee.SubmitTime = this.DateTime;
                    orderToServiceFee.Type = Enumeration.OrderType.PosMachineServiceFee;
                    orderToServiceFee.TypeName = Enumeration.OrderType.PosMachineServiceFee.GetCnName();
                    orderToServiceFee.Deposit = 0;
                    orderToServiceFee.MobileTrafficFee = calculateServiceFee.MobileTrafficFee;
                    orderToServiceFee.PriceVersion = calculateServiceFee.Version;
                    orderToServiceFee.Price = calculateServiceFee.MobileTrafficFee;
                    orderToServiceFee.Status = Enumeration.OrderStatus.WaitPay;
                    orderToServiceFee.CreateTime = this.DateTime;
                    orderToServiceFee.Creator = 0;
                    CurrentDb.OrderToServiceFee.Add(orderToServiceFee);
                    CurrentDb.SaveChanges();

                    SnModel snModel = Sn.Build(SnType.OrderToServiceFee, orderToServiceFee.Id);
                    orderToServiceFee.Sn = snModel.Sn;
                    orderToServiceFee.TradeSnByWechat = snModel.TradeSnByWechat;
                    orderToServiceFee.TradeSnByAlipay = snModel.TradeSnByAlipay;

                    Log.InfoFormat("生成待支付订单号：{0}", orderToServiceFee.Sn);
                }

                SysFactory.SysItemCacheUpdateTime.UpdateUser(orderToServiceFee.UserId);

                CurrentDb.SaveChanges();

            }

            return result;

        }
    }
}
