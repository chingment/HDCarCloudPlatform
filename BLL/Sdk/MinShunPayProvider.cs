using MinShunPaySdk;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumos.Mvc;
using Lumos.Entity.AppApi;

namespace Lumos.BLL
{
    public class MinShunPayProvider : BaseProvider
    {
        public CustomJsonResult QrCodeDownload(int operater, QrCodeDownloadParams pms)
        {
            CustomJsonResult result = new CustomJsonResult();

            var order = CurrentDb.Order.Where(m => m.UserId == pms.UserId && m.Sn == pms.OrderSn).FirstOrDefault();
            if (order == null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "找不到订单");
            }


            order.PayWay = pms.PayWay;

            MinShunPayOrderInfo orderInfo = new MinShunPayOrderInfo();

            orderInfo.OrderId = order.Sn;
            orderInfo.Price = order.Price;
            orderInfo.Remark = "";
            orderInfo.SubmitTime = order.SubmitTime;
            orderInfo.TermId = pms.TermId;
            orderInfo.SpbillIp = pms.SpbillIp;

            if (order.PayWay == Enumeration.OrderPayWay.Wechat)
            {
                orderInfo.TranType = "180000";
            }
            else if (order.PayWay == Enumeration.OrderPayWay.Alipay)
            {
                orderInfo.TranType = "280000";
            }

            var codeDownload_result = MinShunPayUtil.CodeDownload(orderInfo);
            if (string.IsNullOrEmpty(codeDownload_result.MWEB_URL))
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "生成支付二维码失败");
            }


            QrCodeDownloadResult resultData = new QrCodeDownloadResult();
            resultData.OrderSn = order.Sn;
            resultData.MwebUrl = order.Sn;

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", resultData);

            return result;
        }
    }
}
