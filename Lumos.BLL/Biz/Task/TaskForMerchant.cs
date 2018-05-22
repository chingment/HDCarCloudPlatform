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


                    Log.InfoFormat("生成待支付订单号：{0}", orderToServiceFee.Sn);
                }

                SysFactory.SysItemCacheUpdateTime.UpdateUser(orderToServiceFee.UserId);

                CurrentDb.SaveChanges();

            }


            //var bizProcessesAudit = CurrentDb.BizProcessesAudit.ToList();

            //foreach (var item in bizProcessesAudit)
            //{
            //    Order order
            //    switch (item.AduitType)
            //    {
            //        case Enumeration.BizProcessesAuditType.OrderToCarInsure:
            //            order = CurrentDb.or.Where(m => m.Id == item.AduitReferenceId).FirstOrDefault();
            //            if(or)
            //            break;
            //        case Enumeration.BizProcessesAuditType.OrderToCarClaim:
            //            break;
            //        case Enumeration.BizProcessesAuditType.MerchantAudit:
            //            break;
            //        case Enumeration.BizProcessesAuditType.OrderToTalentDemand:
            //            break;
            //        case Enumeration.BizProcessesAuditType.OrderToApplyLossAssess:
            //            break;
            //        case Enumeration.BizProcessesAuditType.OrderToLllegalDealt:
            //            break;
            //        case Enumeration.BizProcessesAuditType.OrderToCredit:
            //            break;
            //        case Enumeration.BizProcessesAuditType.OrderToInsurance:
            //            break;
            //    }
            //}

            return result;

        }
    }
}
