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
            order.TermId = pms.TermId;
            order.SpbillIp = pms.SpbillIp;
            order.Remarks = "测试商品";

            MinShunPayOrderInfo orderInfo = new MinShunPayOrderInfo();

            orderInfo.OrderId = order.Sn;
            orderInfo.Price = 0.01m;
            orderInfo.Remark = "测试商品";
            orderInfo.SubmitTime = order.SubmitTime;
            orderInfo.TermId = order.TermId;
            orderInfo.SpbillIp = order.SpbillIp;

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
            resultData.MwebUrl = codeDownload_result.MWEB_URL;
            resultData.PayWay = order.PayWay;

            CurrentDb.SaveChanges();

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", resultData);

            return result;
        }


        public OrderPayResultNotifyByMinShunLog PayQuery(int operater, OrderToServiceFee pms)
        {

            MinShunPayOrderInfo orderInfo = new MinShunPayOrderInfo();

            orderInfo.OrderId = pms.Sn;
            orderInfo.Price = 0.01m;
            orderInfo.Remark = pms.Remarks;
            orderInfo.SubmitTime = pms.SubmitTime;
            orderInfo.TermId = pms.TermId;
            orderInfo.SpbillIp = pms.SpbillIp;

            if (pms.PayWay == Enumeration.OrderPayWay.Wechat)
            {
                orderInfo.TranType = "180020";
            }
            else if (pms.PayWay == Enumeration.OrderPayWay.Alipay)
            {
                orderInfo.TranType = "280020";
            }

            OrderPayResultNotifyByMinShunLog receiveNotifyLog = null;

            var payQuery_result = MinShunPayUtil.PayQuery(orderInfo);
            if (payQuery_result == null)
            {

            }
            else
            {
                receiveNotifyLog = new OrderPayResultNotifyByMinShunLog();
                receiveNotifyLog.OrderId = payQuery_result.ORDERID;
                receiveNotifyLog.Mercid = payQuery_result.MERCID;
                receiveNotifyLog.Termid = payQuery_result.TERMID;
                receiveNotifyLog.Txnamt = payQuery_result.TXNAMT;
                receiveNotifyLog.ResultCode = payQuery_result.RESULT_CODE;
                receiveNotifyLog.ResultCodeName =GetResultCodeName(payQuery_result.RESULT_CODE);
                receiveNotifyLog.ResultMsg = payQuery_result.RESULT_MSG;
                receiveNotifyLog.Sign = payQuery_result.SIGN;
                receiveNotifyLog.MwebUrl = payQuery_result.MWEB_URL;
                receiveNotifyLog.NotifyParty = Enumeration.PayResultNotifyParty.MinShunOrderQueryApi;
                receiveNotifyLog.NotifyPartyName = Enumeration.PayResultNotifyParty.MinShunOrderQueryApi.GetCnName();
                receiveNotifyLog.Creator = 0;
                receiveNotifyLog.CreateTime = DateTime.Now;
            }


            return receiveNotifyLog;
        }

        public string GetResultCodeName(string resultcode)
        {
            string name = "";
            switch (resultcode)
            {
                case "00":
                    name = "成功";
                    break;
                case "S1":
                    name = "交易已发起退货";
                    break;
                case "S2":
                    name = "未支付";
                    break;
                case "S3":
                    name = "交易关闭";
                    break;
                case "S4":
                    name = "交易已冲正 ";
                    break;
                case "S5":
                    name = "正在支付";
                    break;
                case "S6":
                    name = "支付失败";
                    break;
                case "T1":
                    name = "终端未入库";
                    break;
                case "T2":
                    name = "终端未绑定";
                    break;
                case "T3":
                    name = "商户无此接口权限";
                    break;
                case "T4":
                    name = "用户余额不足";
                    break;
                case "T5":
                    name = "订单已支付";
                    break;
                case "T6":
                    name = "订单已关闭";
                    break;
                case "T7":
                    name = "系统错误";
                    break;
                case "T8":
                    name = "缺少参数";
                    break;
            }
            return name;
        }
    }
}
