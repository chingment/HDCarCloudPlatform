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

    //{"total_amount":"1","amount":"1","message":"%E4%BA%A4%E6%98%93%E6%88%90%E5%8A%9F","sysTime":"20180521174712","result":"S","returnCode":"000000","orderNo":"000000045449433","payCode":"weixin://wxpay/bizpayurl?pr=GTUPgPT","logNo":"201805210364066601","tradeNo":"18050313180000001511","mercId":"800581000010155","signValue":"66637916cd7d71de453510db7c25dabf"}
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

            StarPayOrderInfo starPayOrderInfo = new StarPayOrderInfo();

            order.PayWay = pms.PayWay;


            if (BizFactory.AppSettings.IsTest)
            {
                starPayOrderInfo.Amount = "1";
            }
            else
            {
                starPayOrderInfo.Amount = Convert.ToInt32((order.Price * 100)).ToString();
            }

            starPayOrderInfo.TransTime = order.SubmitTime;
            starPayOrderInfo.OrderSn = order.Sn;

            if (order.PayWay == Enumeration.OrderPayWay.Wechat)
            {
                starPayOrderInfo.PayWay = "WXPAY";
            }
            else if (order.PayWay == Enumeration.OrderPayWay.Alipay)
            {
                starPayOrderInfo.PayWay = "ALIPAY";
            }

            var codeDownload_result = StarPayUtil.CodeDownload(starPayOrderInfo);

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


        public void PayQuery(int operater, Order order)
        {
            var starPayOrderInfo = new StarPayOrderInfo();


            starPayOrderInfo.OrderSn = order.Sn;
            starPayOrderInfo.TransTime = order.SubmitTime;

            string notifyFromResult = null;

            var payQuery_result = StarPayUtil.PayQuery(starPayOrderInfo, out notifyFromResult);

            bool isPaySuccess = false;
            if (payQuery_result != null)
            {
                if (!string.IsNullOrEmpty(payQuery_result.result))
                {
                    if (payQuery_result.result == "S")
                    {
                        isPaySuccess = true;
                    }
                }
            }

            BizFactory.Pay.ResultNotify(operater, order.Sn, isPaySuccess, Enumeration.PayResultNotifyType.PartnerPayOrgOrderQueryApi, "星大陆", notifyFromResult);
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
