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
    public class YiBanShiOrderInfo
    {
        public string order_no { get; set; }

        public string company { get; set; }

        public string product_type { get; set; }

        public string customer_id_type { get; set; }

        public string customer_id { get; set; }

        public string customer_sex { get; set; }

        public string customer_name { get; set; }

        public string customer_mobile_no { get; set; }

        public string customer_birthdate { get; set; }

        public string teacct { get; set; }

        public string car_type { get; set; }

        public string car_license { get; set; }

        public string car_frame_no { get; set; }

        public string payer_id_type { get; set; }

        public string payer_id { get; set; }

        public string payer_name { get; set; }

        public string payer_mobile_no { get; set; }

        public string payer_address { get; set; }

        public string ybs_mer_code { get; set; }

        public string merchant_id { get; set; }

        public string merchant_name { get; set; }

        public string phone_no { get; set; }

        public string cashier_id { get; set; }

        public string teller_id { get; set; }

        //商业险金额
        public string ci_amt { get; set; }

        //交强险金额
        public string tci_amt { get; set; }

        //车船税金额
        public string vvt_amt { get; set; }

        public string biz_code { get; set; }

        public string pay_type { get; set; }

    }

    public class OrderConfirmInfo
    {
        public OrderConfirmInfo()
        {
            this.confirmField = new List<OrderField>();
            this.payMethod = new List<PayWay>();
        }

        public int OrderId { get; set; }

        public string OrderSn { get; set; }

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

                        yOrder = BizFactory.Merchant.GetOrderConfirmInfoByServiceFee(model.OrderSn);

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
                        case Enumeration.PayResultNotifyParty.MinShunNotifyUrl:
                            result = MinShun_ResultNotify(operater, (MinShun_ReceiveNotifyLog)model);
                            break;
                        case Enumeration.PayResultNotifyParty.MinShunOrderQueryApi:
                            //result = YBS_ResultNotify(operater, (MinShun_ReceiveNotifyLog)model);
                            break;
                        case Enumeration.PayResultNotifyParty.Staff:
                            result = Staff_ResultNotify(operater, (OrderPayResultNotifyByStaffLog)model);
                            break;
                    }

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

        //private CustomJsonResult App_ResultNotify(int operater, PayResultModel model)
        //{
        //    CustomJsonResult result = new CustomJsonResult();

        //    try
        //    {
        //        using (TransactionScope ts = new TransactionScope())
        //        {
        //            OrderPayResultNotifyLog resultLog = new OrderPayResultNotifyLog();
        //            resultLog.SysOrderId = model.OrderId;
        //            resultLog.SysOrderSn = model.OrderSn;
        //            resultLog.CreateTime = this.DateTime;
        //            resultLog.Creator = model.UserId;

        //            if (model.Params.Result == PayResultType.Success)
        //            {
        //                resultLog.Result = "1";
        //                resultLog.OrderNo = model.Params.MerchantInfo.order_no.NullStringToNullObject();
        //                resultLog.MerchantId = model.Params.MerchantId.NullStringToNullObject();
        //                resultLog.Amount = model.Params.Amount.NullStringToNullObject();
        //                resultLog.TerminalId = model.Params.TerminalId.NullStringToNullObject();
        //                resultLog.MerchantNo = model.Params.MerchantNo.NullStringToNullObject();
        //                resultLog.BatchNo = model.Params.BatchNo.NullStringToNullObject();
        //                resultLog.MerchantName = model.Params.MerchantName.NullStringToNullObject();
        //                resultLog.Issue = model.Params.Issue.NullStringToNullObject();
        //                resultLog.TraceNo = model.Params.TraceNo.NullStringToNullObject();
        //                resultLog.ReferenceNo = model.Params.ReferenceNo.NullStringToNullObject();
        //                resultLog.Type = model.Params.Type.NullStringToNullObject();
        //                resultLog.CardNo = model.Params.CardNo.NullStringToNullObject();

        //                switch (model.ProductType)
        //                {
        //                    case Enumeration.ProductType.PosMachineServiceFee:
        //                        result = PayServiceFeeCompleted(operater, model.OrderSn);
        //                        break;
        //                }

        //            }
        //            else if (model.Params.Result == PayResultType.Failure)
        //            {

        //                resultLog.Result = "2";
        //                resultLog.FailureReason = model.Params.FailureReason;

        //                Log.WarnFormat("订单:{0},支付失败，原因：{1}", model.OrderSn, model.Params.FailureReason);

        //                result = new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "支付失败");
        //            }

        //            CurrentDb.OrderPayResultNotifyLog.Add(resultLog);
        //            CurrentDb.SaveChanges();

        //            ts.Complete();

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.ErrorFormat("订单号({0})结果反馈发生异常，原因：{1}", model.OrderSn, ex.StackTrace);

        //        result = new CustomJsonResult(ResultType.Exception, ResultCode.Exception, "支付失败");
        //    }

        //    return result;
        //}

        private CustomJsonResult MinShun_ResultNotify(int operater, MinShun_ReceiveNotifyLog receiveNotifyLog)
        {
            CustomJsonResult result = new CustomJsonResult();

            try
            {
                using (TransactionScope ts = new TransactionScope())
                {


                    var order = CurrentDb.Order.Where(m => m.Sn == receiveNotifyLog.OrderId).FirstOrDefault();

                    if (order == null)
                    {
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

                    CurrentDb.MinShun_ReceiveNotifyLog.Add(receiveNotifyLog);
                    CurrentDb.SaveChanges();

                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("订单号({0})结果反馈发生异常，易办事调用原因：{1}", receiveNotifyLog.OrderId, ex.StackTrace);

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
                Log.ErrorFormat("订单号({0})结果反馈发生异常，易办事调用原因：{1}", notifyLog.OrderSn, ex.StackTrace);

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

                var UplinkFund = CurrentDb.Fund.Where(m => m.UserId == (int)Enumeration.UserAccount.Uplink).FirstOrDefault();
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
                UplinkFundTrans.Sn = Sn.Build(SnType.Transactions, UplinkFundTrans.Id);
                CurrentDb.SaveChanges();


                var merchantPosMachine = CurrentDb.MerchantPosMachine.Where(m => m.MerchantId == orderToServiceFee.MerchantId && m.Id == orderToServiceFee.MerchantPosMachineId).FirstOrDefault();
                merchantPosMachine.ExpiryTime = orderToServiceFee.ExpiryTime;
                merchantPosMachine.Status = Enumeration.MerchantPosMachineStatus.Normal;
                merchantPosMachine.LastUpdateTime = this.DateTime;
                merchantPosMachine.Mender = operater;


                orderToServiceFee.Status = Enumeration.OrderStatus.Completed;
                orderToServiceFee.PayTime = this.DateTime;
                orderToServiceFee.CompleteTime = this.DateTime;
                orderToServiceFee.LastUpdateTime = this.DateTime;
                orderToServiceFee.Mender = operater;



                CurrentDb.SaveChanges();

                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "该订单支付结果反馈成功");
            }

            return result;
        }

    }
}
