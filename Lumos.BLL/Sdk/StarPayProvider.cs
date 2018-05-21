using Lumos.Entity;
using Lumos.Entity.AppApi;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class StarPayProvider : BaseProvider
    {
        public CustomJsonResult QrCodeDownload(int operater, QrCodeDownloadParams pms)
        {
            CustomJsonResult result = new CustomJsonResult();

            var order = CurrentDb.Order.Where(m => m.UserId == pms.UserId && m.Sn == pms.OrderSn).FirstOrDefault();
            if (order == null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "找不到订单");
            }
            if (order.Status == Enumeration.OrderStatus.Completed)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "已经支付成功");
            }


            //order.PayWay = pms.PayWay;
            //order.TermId = pms.TermId;
            //order.SpbillIp = pms.SpbillIp;
            //order.Remarks = "测试商品";

            //MinShunPayOrderInfo orderInfo = new MinShunPayOrderInfo();

            //orderInfo.Price = 0.01m;
            //orderInfo.Remark = "测试商品";
            //orderInfo.SubmitTime = order.SubmitTime;
            //orderInfo.TermId = order.TermId;
            //orderInfo.SpbillIp = order.SpbillIp;

            //if (order.PayWay == Enumeration.OrderPayWay.Wechat)
            //{
            //    orderInfo.TranType = "180000";
            //    orderInfo.OrderId = order.TradeSnByWechat;
            //}
            //else if (order.PayWay == Enumeration.OrderPayWay.Alipay)
            //{
            //    orderInfo.TranType = "280000";
            //    orderInfo.OrderId = order.TradeSnByAlipay;
            //}
            //else
            //{
            //    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "不支持该支付方式");
            //}

            //var codeDownload_result = MinShunPayUtil.CodeDownload(orderInfo);
            //if (string.IsNullOrEmpty(codeDownload_result.MWEB_URL))
            //{
            //    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "生成支付二维码失败");
            //}


            //QrCodeDownloadResult resultData = new QrCodeDownloadResult();
            //resultData.OrderSn = order.Sn;
            //resultData.MwebUrl = codeDownload_result.MWEB_URL;
            //resultData.PayWay = order.PayWay;

            //CurrentDb.SaveChanges();

            //result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", resultData);

            return null;
        }


        public OrderPayResultNotifyByPartnerPayOrgLog PayQuery(int operater, Order pms)
        {

            //MinShunPayOrderInfo orderInfo = new MinShunPayOrderInfo();

            //orderInfo.Price = 0.01m;
            //orderInfo.Remark = pms.Remarks;
            //orderInfo.SubmitTime = pms.SubmitTime;
            //orderInfo.TermId = pms.TermId;
            //orderInfo.SpbillIp = pms.SpbillIp;

            //if (pms.PayWay == Enumeration.OrderPayWay.Wechat)
            //{
            //    orderInfo.TranType = "180020";
            //    orderInfo.OrderId = pms.TradeSnByWechat;
            //}
            //else if (pms.PayWay == Enumeration.OrderPayWay.Alipay)
            //{
            //    orderInfo.TranType = "280020";
            //    orderInfo.OrderId = pms.TradeSnByAlipay;
            //}

            //OrderPayResultNotifyByMinShunLog receiveNotifyLog = null;

            //var payQuery_result = MinShunPayUtil.PayQuery(orderInfo);
            //if (payQuery_result == null)
            //{

            //}
            //else
            //{
            //    receiveNotifyLog = new OrderPayResultNotifyByMinShunLog();
            //    receiveNotifyLog.OrderId = payQuery_result.ORDERID;
            //    receiveNotifyLog.Mercid = payQuery_result.MERCID;
            //    receiveNotifyLog.Termid = payQuery_result.TERMID;
            //    receiveNotifyLog.Txnamt = payQuery_result.TXNAMT;
            //    receiveNotifyLog.ResultCode = payQuery_result.RESULT_CODE;
            //    receiveNotifyLog.ResultCodeName = GetResultCodeName(payQuery_result.RESULT_CODE);
            //    receiveNotifyLog.ResultMsg = payQuery_result.RESULT_MSG;
            //    receiveNotifyLog.Sign = payQuery_result.SIGN;
            //    receiveNotifyLog.MwebUrl = payQuery_result.MWEB_URL;
            //    receiveNotifyLog.NotifyParty = Enumeration.PayResultNotifyParty.MinShunOrderQueryApi;
            //    receiveNotifyLog.NotifyPartyName = Enumeration.PayResultNotifyParty.MinShunOrderQueryApi.GetCnName();
            //    receiveNotifyLog.Creator = 0;
            //    receiveNotifyLog.CreateTime = DateTime.Now;
            //}


            return null;
        }
    }
}
