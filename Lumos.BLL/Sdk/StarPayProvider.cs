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
        public CustomJsonResult UnifiedOrder(int operater, UnifiedOrderParams pms)
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

            var orderPayTrans = new OrderPayTrans();
            orderPayTrans.UserId = order.UserId;
            orderPayTrans.MerchantId = order.MerchantId;
            orderPayTrans.PosMachineId = order.PosMachineId;
            orderPayTrans.OrderId = order.Id;
            orderPayTrans.OrderSn = order.Sn;
            orderPayTrans.CreateTime = this.DateTime;
            orderPayTrans.Creator = operater;
            orderPayTrans.PayWay = pms.PayWay;
            orderPayTrans.Amount = starPayOrderInfo.Amount;
            orderPayTrans.TransTime = this.DateTime;
            CurrentDb.OrderPayTrans.Add(orderPayTrans);
            CurrentDb.SaveChanges();

            SnModel snModel = Sn.Build(SnType.OrderPayTrans, orderPayTrans.Id);

            orderPayTrans.Sn = snModel.Sn;


            starPayOrderInfo.TransSn = snModel.Sn;
            order.TransSn = snModel.Sn;
            var codeDownload_result = StarPayUtil.CodeDownload(starPayOrderInfo);

            if (codeDownload_result == null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "生成支付二维码失败");
            }

            if (string.IsNullOrEmpty(codeDownload_result.payCode))
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "生成支付二维码失败");
            }


            orderPayTrans.PartnerLogNo = codeDownload_result.logNo;
            orderPayTrans.PartnerOrderNo = codeDownload_result.orderNo;
            CurrentDb.SaveChanges();

            UnifiedOrderResult resultData = new UnifiedOrderResult();
            resultData.TransSn = order.TransSn;
            resultData.OrderSn = order.Sn;
            resultData.MwebUrl = codeDownload_result.payCode;
            resultData.PayWay = order.PayWay;

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", resultData);

            return null;
        }


        public void PayQuery(int operater, Order order)
        {
            var starPayOrderInfo = new StarPayOrderInfo();


            var orderPayTrans = CurrentDb.OrderPayTrans.Where(m => m.OrderId == order.Id).ToList();

            foreach (var item in orderPayTrans)
            {
                if (!string.IsNullOrEmpty(item.Sn))
                {
                    starPayOrderInfo.OrderSn = item.OrderSn;
                    starPayOrderInfo.TransSn = item.Sn;
                    starPayOrderInfo.TransTime = item.TransTime;

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
