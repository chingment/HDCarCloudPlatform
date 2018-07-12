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
using YdtSdk;

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
                    case Enumeration.OrderType.PosMachineServiceFee:
                    case Enumeration.OrderType.LllegalQueryRecharge:
                    case Enumeration.OrderType.LllegalDealt:
                    case Enumeration.OrderType.Goods:
                        SdkFactory.StarPay.PayQuery(0, order);
                        break;
                    default:
                        break;
                }

            }

            CurrentDb.SaveChanges();


            return result;
        }
    }
}
