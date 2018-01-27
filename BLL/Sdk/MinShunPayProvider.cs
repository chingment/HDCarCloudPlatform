using MinShunPaySdk;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Sdk
{
    public class MinShunPayProvider : BaseProvider
    {
        public void CodeDownload(int orderId, Enumeration.OrderPayWay payway)
        {
            var order = CurrentDb.Order.Where(m => m.Id == orderId).FirstOrDefault();

            order.PayWay = payway;

            MinShunPayOrderInfo orderInfo = new MinShunPayOrderInfo();

            orderInfo.OrderId = order.Sn;
            orderInfo.Price = order.Price;
            orderInfo.Remark = "";
            orderInfo.SubmitTime = order.SubmitTime;
            orderInfo.TermId = "";
            orderInfo.SpbillIp = "";

            if (order.PayWay == Enumeration.OrderPayWay.Wechat)
            {
                orderInfo.TranType = "180000";
            }
            else if (order.PayWay == Enumeration.OrderPayWay.Alipay)
            {
                orderInfo.TranType = "280000";
            }

            var codeDownload_result = MinShunPayUtil.CodeDownload(orderInfo);



        }
    }
}
