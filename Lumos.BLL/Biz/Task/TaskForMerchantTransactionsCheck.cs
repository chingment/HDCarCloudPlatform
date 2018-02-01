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
    class TaskForMerchantTransactionsCheck : BaseProvider, ITask
    {

        public string BulidOpendId(string merchantCode, string key, string secret)
        {

            byte[] result = Encoding.Default.GetBytes(merchantCode + key + secret);    //tbPass为输入密码的文本框  
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            string s_output = BitConverter.ToString(output).Replace("-", "");

            return s_output;
        }

        public CustomJsonResult Run()
        {
            CustomJsonResult result = new CustomJsonResult();


            var orders = CurrentDb.Order.Where(m => m.Status == Enumeration.OrderStatus.WaitPay).ToList();

            foreach (var order in orders)
            {

                switch (order.ProductType)
                {
                    case Enumeration.ProductType.InsureForCarForInsure:

                        //处理提交之后24内没有支付的车险订单,以报价完成时间
                        var orderToCarForInsure = CurrentDb.OrderToCarInsure.Where(m => m.Sn == order.Sn && SqlFunctions.DateDiff("hour", m.EndOfferTime, this.DateTime) >= m.AutoCancelByHour).FirstOrDefault();
                        if (orderToCarForInsure != null)
                        {
                            order.CancleTime = this.DateTime;

                            order.Status = Enumeration.OrderStatus.Cancled;
                            var l_bizProcessesAudit = CurrentDb.BizProcessesAudit.Where(c => c.AduitReferenceId == order.Id && c.AduitType == Enumeration.BizProcessesAuditType.CarInsure).FirstOrDefault();

                            BizFactory.BizProcessesAudit.ChangeAuditDetails(0, Enumeration.CarInsureOfferDealtStep.Cancle, l_bizProcessesAudit.Id, 0, null, "超过1天未支付，系统自动取消，请重新提交报价", this.DateTime);

                            BizFactory.BizProcessesAudit.ChangeCarInsureOfferDealtStatus(0, l_bizProcessesAudit.Id, Enumeration.CarInsureOfferDealtStatus.StaffCancle);

                            Log.InfoFormat("订单编号:{0}，在24小时内没有支付车险订单，系统自动取消", order.Sn);
                        }

                        break;
                    case Enumeration.ProductType.PosMachineServiceFee:

                        var receiveNotifyLog = SdkFactory.MinShunPay.PayQuery(0, order);

                        if (receiveNotifyLog == null)
                        {
                            Log.WarnFormat("订单编号:{0}，查询不到支付记录", order.Sn);
                        }
                        else
                        {
                            BizFactory.Pay.ResultNotify(0, Enumeration.PayResultNotifyParty.MinShunOrderQueryApi, receiveNotifyLog);
                        }



                        break;
                }

            }

            CurrentDb.SaveChanges();


            return result;
        }
    }
}
