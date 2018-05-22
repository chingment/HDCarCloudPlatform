using Lumos.DAL;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Biz.Task
{
    public class TaskForMerchantTransactionsCheck : BaseProvider, ITask
    {
        public CustomJsonResult Run()
        {
            CustomJsonResult result = new CustomJsonResult();


            var orders = CurrentDb.Order.Where(m => m.Status == Enumeration.OrderStatus.WaitPay).ToList();

            foreach (var order in orders)
            {
                SdkFactory.StarPay.PayQuery(0, order);

                switch (order.Type)
                {
                    case Enumeration.OrderType.InsureForCarForInsure:

                        //处理提交之后24内没有支付的车险订单,以报价完成时间
                        var orderToCarForInsure = CurrentDb.OrderToCarInsure.Where(m => m.Sn == order.Sn && SqlFunctions.DateDiff("hour", m.EndOfferTime, this.DateTime) >= m.AutoCancelByHour).FirstOrDefault();
                        if (orderToCarForInsure != null)
                        {
                            order.CancleTime = this.DateTime;

                            order.Status = Enumeration.OrderStatus.Cancled;
                            var l_bizProcessesAudit = CurrentDb.BizProcessesAudit.Where(c => c.AduitReferenceId == order.Id && c.AduitType == Enumeration.BizProcessesAuditType.OrderToCarInsure).FirstOrDefault();

                            ///BizFactory.BizProcessesAudit.ChangeAuditDetails(0, Enumeration.CarInsureOfferDealtStep.Cancle, l_bizProcessesAudit.Id, 0, null, "超过1天未支付，系统自动取消，请重新提交报价", this.DateTime);

                            //BizFactory.BizProcessesAudit.ChangeCarInsureOfferDealtStatus(0, l_bizProcessesAudit.Id, Enumeration.CarInsureOfferDealtStatus.StaffCancle);

                            Log.InfoFormat("订单编号:{0}，在24小时内没有支付车险订单，系统自动取消", order.Sn);
                        }

                        break;
                    case Enumeration.OrderType.PosMachineServiceFee:

                        break;
                }

            }

            CurrentDb.SaveChanges();


            return result;
        }
    }
}
