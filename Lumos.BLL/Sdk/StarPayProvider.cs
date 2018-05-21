using Lumos.Entity;
using Lumos.Entity.AppApi;
using Lumos.Mvc;
using StarPaySdk;
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

            if (pms.PayWay != Enumeration.OrderPayWay.Wechat || pms.PayWay == Enumeration.OrderPayWay.Alipay)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "不支持该支付方式");
            }

            StarPayOrderInfo orderInfo = new StarPayOrderInfo();

            order.PayWay = pms.PayWay;
            order.TermId = pms.TermId;
            order.SpbillIp = pms.SpbillIp;
            order.Remarks = "";

            if (BizFactory.AppSettings.IsTest)
            {
                orderInfo.Amount = "1";
            }
            else
            {
                orderInfo.Amount = Convert.ToInt32((order.Price * 100)).ToString();
            }

            orderInfo.TransTime = order.SubmitTime;
            orderInfo.TermId = order.TermId;
            orderInfo.OrderId = order.Sn;

            if (order.PayWay == Enumeration.OrderPayWay.Wechat)
            {
                orderInfo.PayWay = "WXPAY";
            }
            else if (order.PayWay == Enumeration.OrderPayWay.Alipay)
            {
                orderInfo.PayWay = "ALIPAY";
            }

            var codeDownload_result = StarPayUtil.CodeDownload(orderInfo);

            if (string.IsNullOrEmpty(codeDownload_result.payCode))
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "生成支付二维码失败");
            }

            CurrentDb.SaveChanges();

            QrCodeDownloadResult resultData = new QrCodeDownloadResult();
            resultData.OrderSn = order.Sn;
            resultData.MwebUrl = codeDownload_result.payCode;
            resultData.PayWay = order.PayWay;

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", resultData);

            return null;
        }


        public void PayQuery(int operater, Order pms)
        {
            var starPayOrderInfo = new StarPayOrderInfo();

            var payQuery_result = StarPayUtil.PayQuery(starPayOrderInfo);

            if (payQuery_result == null)
            {
                return;
            }

            var resultlog = new OrderPayResultNotifyByPartnerPayOrgLog();
            resultlog.OrderId = payQuery_result.orderNo;
            resultlog.Mercid = payQuery_result.mercId;
            resultlog.Termid = starPayOrderInfo.TermId;
            resultlog.Amount = payQuery_result.amount;
            resultlog.TotalAmount = payQuery_result.total_amount;
            resultlog.ResultCode = payQuery_result.result;
            resultlog.ResultCodeName = GetResultCodeName(payQuery_result.result);
            resultlog.ResultMsg = payQuery_result.message;
            resultlog.NotifyType = Enumeration.PayResultNotifyType.PartnerPayOrgOrderQueryApi;
            resultlog.PartnerPayOrgName = "星大陆";
            resultlog.PartnerPayOrgResult = "";
            resultlog.Creator = 0;
            resultlog.CreateTime = DateTime.Now;
            CurrentDb.OrderPayResultNotifyByPartnerPayOrgLog.Add(resultlog);
            CurrentDb.SaveChanges();

            if (payQuery_result.result == "S")
            {
                BizFactory.Pay.ResultNotify(0, Enumeration.PayResultNotifyType.PartnerPayOrgOrderQueryApi, resultlog);
            }
        }

        public string GetResultCodeName(string resultcode)
        {
            string name = "未知状态";
            switch (resultcode)
            {
                case "S":
                    name = "交易成功";
                    break;
                case "F":
                    name = "交易失败";
                    break;
                case "A":
                    name = "等待授权";
                    break;
                case "Z":
                    name = "交易未知";
                    break;
                case "D":
                    name = "订单已撤销 ";
                    break;
            }
            return name;
        }
    }
}
