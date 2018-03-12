using Lumos.Entity;
using Lumos.Entity.AppApi;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL
{
    public class OrderConfirmInfo
    {
        public OrderConfirmInfo()
        {
            this.confirmField = new List<OrderField>();
            this.payMethod = new List<PayWay>();
        }

        public int OrderId { get; set; }

        public string OrderSn { get; set; }

        public Enumeration.ProductType productType { get; set; }

        public string productName { get; set; }

        public string transName { get; set; }

        public string amount { get; set; }

        public string remarks { get; set; }

        //public YiBanShiOrderInfo orderInfo { get; set; }

        public List<OrderField> confirmField
        {
            get;
            set;
        }

        public List<PayWay> payMethod
        {
            get;
            set;
        }

    }


    public class PayWay
    {
        public int id { get; set; }

        public string param { get; set; }

    }


    public class PayProvider : BaseProvider
    {

        public static decimal GetPayDecimal(decimal d)
        {
            return Math.Round(d, 2);
        }

        public CustomJsonResult ResultQuery(int operater, PayQueryParams pms)
        {
            CustomJsonResult result = new CustomJsonResult();



            var order = CurrentDb.Order.Where(m => m.UserId == pms.UserId && m.Sn == pms.OrderSn).FirstOrDefault();

            if (order == null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "找不到订单");
            }

            PayQueryResult resultData = new PayQueryResult();

            resultData.OrderSn = order.Sn;
            resultData.ProductType = order.ProductType;
            resultData.Status = (int)order.Status;
            resultData.Remarks = order.Status.GetCnName();


            PrintDataModel printData = new PrintDataModel();

            printData.MerchantName = "好易联";
            printData.MerchantCode = "354422";
            printData.ProductName = order.ProductName;
            printData.TradeType = "消费";
            printData.TradeNo = order.Sn;
            printData.TradePayMethod = order.PayWay.GetCnName();
            printData.TradeAmount = order.Price.ToF2Price();
            printData.TradeDateTime = order.PayTime.ToUnifiedFormatDateTime();
            printData.ServiceHotline = "4400000000";

            resultData.PrintData = printData;


            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", resultData);

            return result;
        }


        public CustomJsonResult Confirm(int operater, PayConfirmModel model)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                OrderConfirmInfo yOrder = new OrderConfirmInfo();

                switch (model.ProductType)
                {
                    case Enumeration.ProductType.PosMachineServiceFee:
                        #region 服务费
                        var orderToServiceFee = CurrentDb.OrderToServiceFee.Where(m => m.Sn == model.OrderSn).FirstOrDefault();
                        yOrder = BizFactory.Merchant.GetOrderConfirmInfoByServiceFee(orderToServiceFee);

                        result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "确认成功", yOrder);
                        #endregion
                        break;
                }

                CurrentDb.SaveChanges();
                ts.Complete();
            }

            return result;
        }

        public CustomJsonResult ConfirmPayByStaff(int operater, int orderId)
        {
            CustomJsonResult result = new CustomJsonResult();

            OrderPayResultNotifyByStaffLog notifyLog = new OrderPayResultNotifyByStaffLog();
            notifyLog.OrderId = orderId;
            result = ResultNotify(operater, Enumeration.PayResultNotifyParty.Staff, notifyLog);
            return result;
        }

        public CustomJsonResult ResultNotify(int operater, Enumeration.PayResultNotifyParty notifyParty, object model)
        {
            CustomJsonResult result = new CustomJsonResult();
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    switch (notifyParty)
                    {
                        case Enumeration.PayResultNotifyParty.AppNotify:
                            result = App_ResultNotify(operater, (OrderPayResultNotifyByAppLog)model);
                            break;
                        case Enumeration.PayResultNotifyParty.MinShunNotifyUrl:
                            result = MinShun_ResultNotify(operater, (OrderPayResultNotifyByMinShunLog)model);
                            break;
                        case Enumeration.PayResultNotifyParty.MinShunOrderQueryApi:
                            result = MinShun_ResultNotify(operater, (OrderPayResultNotifyByMinShunLog)model);
                            break;
                        case Enumeration.PayResultNotifyParty.Staff:
                            result = Staff_ResultNotify(operater, (OrderPayResultNotifyByStaffLog)model);
                            break;
                    }

                    CurrentDb.SaveChanges();
                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("{0}订单号结果反馈发生异常,原因：{1}", notifyParty.GetCnName(), ex.StackTrace);

                result = new CustomJsonResult(ex, "支付失败");
            }

            return result;

        }

        private CustomJsonResult App_ResultNotify(int operater, OrderPayResultNotifyByAppLog receiveNotifyLog)
        {
            CustomJsonResult result = new CustomJsonResult();

            try
            {
                using (TransactionScope ts = new TransactionScope())
                {

                    var order = CurrentDb.Order.Where(m => m.Sn == receiveNotifyLog.Order).FirstOrDefault();

                    if (order == null)
                    {
                        if (string.IsNullOrEmpty(receiveNotifyLog.Order))
                        {
                            receiveNotifyLog.Creator = 0;
                            receiveNotifyLog.CreateTime = DateTime.Now;
                            CurrentDb.OrderPayResultNotifyByAppLog.Add(receiveNotifyLog);
                            CurrentDb.SaveChanges();
                            ts.Complete();
                        }

                        return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "找不到对应的订单号");
                    }
                    receiveNotifyLog.MerchantId = order.MerchantId;
                    receiveNotifyLog.PosMachineId = order.PosMachineId;
                    receiveNotifyLog.UserId = order.UserId;
                    receiveNotifyLog.Creator = 0;
                    receiveNotifyLog.CreateTime = DateTime.Now;
                    switch (order.ProductType)
                    {
                        case Enumeration.ProductType.PosMachineServiceFee:
                            result = PayServiceFeeCompleted(operater, order.Sn);
                            break;
                    }


                    CurrentDb.OrderPayResultNotifyByAppLog.Add(receiveNotifyLog);
                    CurrentDb.SaveChanges();

                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("订单号({0})结果反馈发生异常，原因：{1}", receiveNotifyLog.Order, ex.StackTrace);

                result = new CustomJsonResult(ResultType.Exception, ResultCode.Exception, "支付通知失败");
            }

            return result;
        }


        private CustomJsonResult MinShun_ResultNotify(int operater, OrderPayResultNotifyByMinShunLog receiveNotifyLog)
        {
            CustomJsonResult result = new CustomJsonResult();

            try
            {
                using (TransactionScope ts = new TransactionScope())
                {

                    string orderId = receiveNotifyLog.OrderId.Substring(0, receiveNotifyLog.OrderId.Length - 1);

                    var order = CurrentDb.Order.Where(m => m.Sn == orderId).FirstOrDefault();

                    if (order == null)
                    {
                        CurrentDb.OrderPayResultNotifyByMinShunLog.Add(receiveNotifyLog);
                        CurrentDb.SaveChanges();
                        ts.Complete();
                        return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "找不到对应的订单号");
                    }

                    if (receiveNotifyLog.ResultCode == "00" || receiveNotifyLog.ResultCode == "T5")
                    {
                        switch (order.ProductType)
                        {
                            case Enumeration.ProductType.PosMachineServiceFee:
                                result = PayServiceFeeCompleted(operater, order.Sn);
                                break;
                        }
                    }

                    CurrentDb.OrderPayResultNotifyByMinShunLog.Add(receiveNotifyLog);
                    CurrentDb.SaveChanges();

                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("订单号({0})结果反馈发生异常，原因：{1}", receiveNotifyLog.OrderId, ex.StackTrace);

                result = new CustomJsonResult(ResultType.Exception, ResultCode.Exception, "支付失败");
            }

            return result;
        }


        private CustomJsonResult Staff_ResultNotify(int operater, OrderPayResultNotifyByStaffLog notifyLog)
        {
            CustomJsonResult result = new CustomJsonResult();

            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    var order = CurrentDb.Order.Where(m => m.Id == notifyLog.OrderId).FirstOrDefault();
                    if (order != null)
                    {
                        notifyLog.Amount = order.Price;
                        notifyLog.MerchantId = order.MerchantId;
                        notifyLog.UserId = order.UserId;
                        notifyLog.OrderSn = order.Sn;
                        notifyLog.CreateTime = this.DateTime;
                        notifyLog.Creator = operater;

                        switch (order.ProductType)
                        {
                            case Enumeration.ProductType.PosMachineServiceFee:
                                result = PayServiceFeeCompleted(operater, order.Sn);
                                break;
                        }


                        if (result.Result == Lumos.Mvc.ResultType.Success)
                        {
                            result.Data = notifyLog;
                        }

                    }

                    CurrentDb.OrderPayResultNotifyByStaffLog.Add(notifyLog);
                    CurrentDb.SaveChanges();

                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("后台订单支付确认订单号({0})结果反馈发生异常，原因：{1}", notifyLog.OrderSn, ex.StackTrace);

                result = new CustomJsonResult(ResultType.Exception, ResultCode.Exception, "支付失败");
            }

            return result;
        }

        private CustomJsonResult PayServiceFeeCompleted(int operater, string orderSn)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var orderToServiceFee = CurrentDb.OrderToServiceFee.Where(m => m.Sn == orderSn).FirstOrDefault();


                if (orderToServiceFee.Status == Enumeration.OrderStatus.Completed)
                {
                    ts.Complete();
                    return new CustomJsonResult(ResultType.Success, ResultCode.Success, "该订单已经支付完成");
                }


                if (orderToServiceFee.Status != Enumeration.OrderStatus.WaitPay)
                {
                    ts.Complete();
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "该订单未在就绪支付状态");
                }

                var UplinkFund = CurrentDb.Fund.Where(m => m.UserId == (int)Enumeration.UserAccount.HaoYiLian).FirstOrDefault();
                UplinkFund.Balance += orderToServiceFee.Price;
                UplinkFund.Mender = operater;
                UplinkFund.LastUpdateTime = this.DateTime;

                var UplinkFundTrans = new Transactions();
                UplinkFundTrans.UserId = UplinkFund.UserId;
                UplinkFundTrans.ChangeAmount = orderToServiceFee.Price;
                UplinkFundTrans.Balance = UplinkFund.Balance;
                UplinkFundTrans.Type = Enumeration.TransactionsType.Rent;
                UplinkFundTrans.Description = string.Format("订单号:{0},押金:{1}元,流量费用:{2}元,合计:{3}元", orderSn, orderToServiceFee.Deposit, orderToServiceFee.MobileTrafficFee, orderToServiceFee.Price);
                UplinkFundTrans.Creator = operater;
                UplinkFundTrans.CreateTime = this.DateTime;
                CurrentDb.Transactions.Add(UplinkFundTrans);
                CurrentDb.SaveChanges();
                UplinkFundTrans.Sn = Sn.Build(SnType.Transactions, UplinkFundTrans.Id).Sn;
                CurrentDb.SaveChanges();


                var merchantPosMachine = CurrentDb.MerchantPosMachine.Where(m => m.MerchantId == orderToServiceFee.MerchantId && m.PosMachineId == orderToServiceFee.PosMachineId).FirstOrDefault();
                merchantPosMachine.ExpiryTime = this.DateTime.AddYears(1);

                if (merchantPosMachine.ActiveTime == null)
                {
                    merchantPosMachine.ActiveTime = this.DateTime;
                }

                merchantPosMachine.Status = Enumeration.MerchantPosMachineStatus.Normal;
                merchantPosMachine.LastUpdateTime = this.DateTime;
                merchantPosMachine.Mender = operater;

                orderToServiceFee.ExpiryTime = this.DateTime.AddYears(1);
                orderToServiceFee.Status = Enumeration.OrderStatus.Completed;
                orderToServiceFee.PayTime = this.DateTime;
                orderToServiceFee.CompleteTime = this.DateTime;
                orderToServiceFee.LastUpdateTime = this.DateTime;
                orderToServiceFee.Mender = operater;


                var merchant = CurrentDb.Merchant.Where(m => m.Id == orderToServiceFee.MerchantId).FirstOrDefault();
                var posMachine = CurrentDb.PosMachine.Where(m => m.Id == orderToServiceFee.PosMachineId).FirstOrDefault();

                if (orderToServiceFee.SalesmanId == null)
                {
                    orderToServiceFee.SalesmanId = posMachine.SalesmanId;
                }
                if (orderToServiceFee.AgentId == null)
                {
                    orderToServiceFee.AgentId = posMachine.AgentId;
                }

                if (orderToServiceFee.Deposit > 0)
                {
                    merchant.SalesmanId = posMachine.SalesmanId;
                    merchant.AgentId = posMachine.AgentId;

                    posMachine.IsUse = true;

                    BizFactory.BizProcessesAudit.Add(operater, Enumeration.BizProcessesAuditType.MerchantAudit, orderToServiceFee.MerchantId, Enumeration.MerchantAuditStatus.WaitPrimaryAudit, "");

                    CurrentDb.SaveChanges();
                }


                CurrentDb.SaveChanges();

                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "该订单支付结果反馈成功");
            }

            return result;
        }

    }
}
