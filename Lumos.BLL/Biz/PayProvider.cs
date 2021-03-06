﻿using Lumos.Entity;
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

        public Enumeration.OrderType orderType { get; set; }

        public string orderTypeName { get; set; }

        public string remarks { get; set; }


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



            //var order = CurrentDb.Order.Where(m => m.UserId == pms.UserId && m.Sn == pms.OrderSn).FirstOrDefault();

            //if (order == null)
            //{
            //    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "找不到订单");
            //}

            //PayQueryResult resultData = new PayQueryResult();

            //resultData.OrderSn = order.Sn;
            //resultData.ProductType = order.ProductType;
            //resultData.Status = (int)order.Status;
            //resultData.Remarks = order.Status.GetCnName();


            //PrintDataModel printData = new PrintDataModel();

            //printData.MerchantName = "好易联";
            //printData.MerchantCode = "354422";
            //printData.ProductName = order.ProductName;
            //printData.TradeType = "消费";
            //printData.TradeNo = order.Sn;
            //printData.TradePayMethod = order.PayWay.GetCnName();
            //printData.TradeAmount = order.Price.ToF2Price();
            //printData.TradeDateTime = order.PayTime.ToUnifiedFormatDateTime();
            //printData.ServiceHotline = "4400000000";

            //resultData.PrintData = printData;


            //result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", resultData);

            return result;
        }


        public CustomJsonResult Confirm(int operater, PayConfirmModel model)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                OrderConfirmInfo yOrder = new OrderConfirmInfo();

                switch (model.OrderType)
                {
                    case Enumeration.OrderType.PosMachineServiceFee:
                        #region 服务费
                        var orderToServiceFee = CurrentDb.OrderToServiceFee.Where(m => m.Sn == model.OrderSn).FirstOrDefault();
                        //yOrder = BizFactory.Merchant.GetOrderConfirmInfoByServiceFee(orderToServiceFee);

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
                        //case Enumeration.PayResultNotifyParty.MinShunNotifyUrl:
                        //    result = MinShun_ResultNotify(operater, (OrderPayResultNotifyByMinShunLog)model);
                        //    break;
                        //case Enumeration.PayResultNotifyParty.MinShunOrderQueryApi:
                        //    result = MinShun_ResultNotify(operater, (OrderPayResultNotifyByMinShunLog)model);
                        //    break;
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

                result = new CustomJsonResult(ResultType.Exception, "支付失败");
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

                    var order = CurrentDb.Order.Where(m => m.Sn == receiveNotifyLog.OrderSn).FirstOrDefault();

                    if (order == null)
                    {
                        if (!string.IsNullOrEmpty(receiveNotifyLog.Order))
                        {
                            receiveNotifyLog.Creator = 0;
                            receiveNotifyLog.CreateTime = DateTime.Now;
                            CurrentDb.OrderPayResultNotifyByAppLog.Add(receiveNotifyLog);
                            CurrentDb.SaveChanges();
                        }
                        ts.Complete();

                        return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "找不到对应的订单号");
                    }

                    receiveNotifyLog.MerchantId = order.MerchantId;
                    receiveNotifyLog.PosMachineId = order.PosMachineId;
                    receiveNotifyLog.UserId = order.UserId;
                    receiveNotifyLog.Creator = 0;
                    receiveNotifyLog.CreateTime = DateTime.Now;
                    switch (order.Type)
                    {
                        case Enumeration.OrderType.PosMachineServiceFee:
                            result = PayServiceFeeCompleted(operater, order.Sn);
                            break;
                        case Enumeration.OrderType.LllegalQueryRecharge:
                            result = PayLllegalQueryRechargeCompleted(operater, order.Sn);
                            break;
                        case Enumeration.OrderType.LllegalDealt:
                            result = PayLllegalDealtCompleted(operater, order.Sn);
                            break;
                    }


                    CurrentDb.OrderPayResultNotifyByAppLog.Add(receiveNotifyLog);
                    CurrentDb.SaveChanges();

                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("订单号({0})结果反馈发生异常，原因：{1}", receiveNotifyLog.OrderSn, ex.StackTrace);

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
                        switch (order.Type)
                        {
                            case Enumeration.OrderType.PosMachineServiceFee:
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

                    if (order == null)
                    {
                        Log.Warn("订单找不到");
                        return new CustomJsonResult(ResultType.Exception, ResultCode.Exception, "订单找不到");
                    }

                    if (order != null)
                    {
                        notifyLog.Amount = order.Price;
                        notifyLog.MerchantId = order.MerchantId;
                        notifyLog.UserId = order.UserId;
                        notifyLog.OrderSn = order.Sn;
                        notifyLog.CreateTime = this.DateTime;
                        notifyLog.Creator = operater;

                        switch (order.Type)
                        {
                            case Enumeration.OrderType.PosMachineServiceFee:
                                result = PayServiceFeeCompleted(operater, order.Sn);
                                break;
                            case Enumeration.OrderType.InsureForCarForInsure:
                                result = PayCarInsureCompleted(operater, order.Sn);
                                break;
                            case Enumeration.OrderType.LllegalQueryRecharge:
                                result = PayLllegalQueryRechargeCompleted(operater, order.Sn);
                                break;
                            case Enumeration.OrderType.LllegalDealt:
                                result = PayLllegalDealtCompleted(operater, order.Sn);
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
                Log.ErrorFormat("后台订单支付确认订单号({0})结果反馈发生异常，原因：{1}", notifyLog.OrderSn, ex.InnerException);

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

                if (orderToServiceFee == null)
                {
                    ts.Complete();
                    return new CustomJsonResult(ResultType.Success, ResultCode.Success, "找不到订单号");
                }

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


                var haoYiLianFund = CurrentDb.Fund.Where(m => m.UserId == (int)Enumeration.UserAccount.HaoYiLian).FirstOrDefault();

                if (haoYiLianFund == null)
                {
                    Log.Warn("找不到haoYiLianFund");
                }

                haoYiLianFund.Balance += orderToServiceFee.Price;
                haoYiLianFund.Mender = operater;
                haoYiLianFund.LastUpdateTime = this.DateTime;

                var haoYiLianFundTrans = new FundTrans();
                haoYiLianFundTrans.UserId = haoYiLianFund.UserId;
                haoYiLianFundTrans.ChangeAmount = orderToServiceFee.Price;
                haoYiLianFundTrans.Balance = haoYiLianFund.Balance;
                haoYiLianFundTrans.Type = Enumeration.TransactionsType.ServiceFee;
                haoYiLianFundTrans.Description = string.Format("订单号:{0},押金:{1}元,流量费用:{2}元,合计:{3}元", orderSn, orderToServiceFee.Deposit, orderToServiceFee.MobileTrafficFee, orderToServiceFee.Price);
                haoYiLianFundTrans.Creator = operater;
                haoYiLianFundTrans.CreateTime = this.DateTime;
                CurrentDb.FundTrans.Add(haoYiLianFundTrans);
                CurrentDb.SaveChanges();
                haoYiLianFundTrans.Sn = Sn.Build(SnType.FundTrans, haoYiLianFundTrans.Id).Sn;
                CurrentDb.SaveChanges();


                var merchantPosMachine = CurrentDb.MerchantPosMachine.Where(m => m.MerchantId == orderToServiceFee.MerchantId && m.PosMachineId == orderToServiceFee.PosMachineId).FirstOrDefault();

                if (merchantPosMachine == null)
                {
                    Log.Warn("找不到merchantPosMachine");
                }

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

                if (merchant == null)
                {
                    Log.Warn("找不到merchant");
                }

                var posMachine = CurrentDb.PosMachine.Where(m => m.Id == orderToServiceFee.PosMachineId).FirstOrDefault();

                if (posMachine == null)
                {
                    Log.Warn("找不到posMachine");
                }

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

                    BizFactory.BizProcessesAudit.Add(operater, Enumeration.BizProcessesAuditType.MerchantAudit, orderToServiceFee.MerchantId, Enumeration.MerchantAuditStatus.WaitPrimaryAudit);

                    CurrentDb.SaveChanges();
                }


                CurrentDb.SaveChanges();

                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "该订单支付结果反馈成功");
            }

            return result;
        }

        private CustomJsonResult PayLllegalQueryRechargeCompleted(int operater, string orderSn)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var orderToLllegalQueryRecharge = CurrentDb.OrderToLllegalQueryRecharge.Where(m => m.Sn == orderSn).FirstOrDefault();

                if (orderToLllegalQueryRecharge == null)
                {
                    ts.Complete();
                    return new CustomJsonResult(ResultType.Success, ResultCode.Success, "找不到订单号");
                }

                if (orderToLllegalQueryRecharge.Status == Enumeration.OrderStatus.Completed)
                {
                    ts.Complete();
                    return new CustomJsonResult(ResultType.Success, ResultCode.Success, "该订单已经支付完成");
                }


                if (orderToLllegalQueryRecharge.Status != Enumeration.OrderStatus.WaitPay)
                {
                    ts.Complete();
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "该订单未在就绪支付状态");
                }

                orderToLllegalQueryRecharge.Status = Enumeration.OrderStatus.Completed;
                orderToLllegalQueryRecharge.PayTime = this.DateTime;
                orderToLllegalQueryRecharge.CompleteTime = this.DateTime;
                orderToLllegalQueryRecharge.LastUpdateTime = this.DateTime;
                orderToLllegalQueryRecharge.Mender = operater;

                var haoYiLianFund = CurrentDb.Fund.Where(m => m.UserId == (int)Enumeration.UserAccount.HaoYiLian).FirstOrDefault();
                haoYiLianFund.Balance += orderToLllegalQueryRecharge.Price;
                haoYiLianFund.Mender = operater;
                haoYiLianFund.LastUpdateTime = this.DateTime;

                var haoYiLianFundTrans = new FundTrans();
                haoYiLianFundTrans.UserId = haoYiLianFund.UserId;
                haoYiLianFundTrans.ChangeAmount = orderToLllegalQueryRecharge.Price;
                haoYiLianFundTrans.Balance = haoYiLianFund.Balance;
                haoYiLianFundTrans.Type = Enumeration.TransactionsType.LllegalQueryRecharg;
                haoYiLianFundTrans.Description = string.Format("订单号:{0},充值违章查询积分:{1}元", orderSn, orderToLllegalQueryRecharge.Price);
                haoYiLianFundTrans.Creator = operater;
                haoYiLianFundTrans.CreateTime = this.DateTime;
                CurrentDb.FundTrans.Add(haoYiLianFundTrans);
                CurrentDb.SaveChanges();
                haoYiLianFundTrans.Sn = Sn.Build(SnType.FundTrans, haoYiLianFundTrans.Id).Sn;
                CurrentDb.SaveChanges();



                var lllegalQueryScore = CurrentDb.LllegalQueryScore.Where(m => m.UserId == orderToLllegalQueryRecharge.UserId && m.MerchantId == orderToLllegalQueryRecharge.MerchantId).FirstOrDefault();
                lllegalQueryScore.Score += orderToLllegalQueryRecharge.Score;
                lllegalQueryScore.Mender = operater;
                lllegalQueryScore.LastUpdateTime = this.DateTime;
                CurrentDb.SaveChanges();


                var lllegalQueryScoreTrans = new LllegalQueryScoreTrans();
                lllegalQueryScoreTrans.UserId = orderToLllegalQueryRecharge.UserId;
                lllegalQueryScoreTrans.ChangeScore = orderToLllegalQueryRecharge.Score;
                lllegalQueryScoreTrans.Score = lllegalQueryScore.Score;
                lllegalQueryScoreTrans.Type = Enumeration.LllegalQueryScoreTransType.IncreaseByRecharge;
                lllegalQueryScoreTrans.Description = string.Format("充值{0}元，得到违章查询积分:{1}", orderToLllegalQueryRecharge.Price.ToF2Price(), orderToLllegalQueryRecharge.Score);
                lllegalQueryScoreTrans.Creator = operater;
                lllegalQueryScoreTrans.CreateTime = this.DateTime;
                CurrentDb.LllegalQueryScoreTrans.Add(lllegalQueryScoreTrans);
                CurrentDb.SaveChanges();
                lllegalQueryScoreTrans.Sn = Sn.Build(SnType.LllegalQueryScoreTrans, lllegalQueryScoreTrans.Id).Sn;
                CurrentDb.SaveChanges();

                ts.Complete();


                var score = new { score = lllegalQueryScore.Score };


                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "该订单支付结果反馈成功", score);
            }

            return result;
        }

        private CustomJsonResult PayLllegalDealtCompleted(int operater, string orderSn)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var orderToLllegalDealt = CurrentDb.OrderToLllegalDealt.Where(m => m.Sn == orderSn).FirstOrDefault();

                if (orderToLllegalDealt == null)
                {
                    ts.Complete();
                    return new CustomJsonResult(ResultType.Success, ResultCode.Success, "找不到订单号");
                }

                if (orderToLllegalDealt.Status == Enumeration.OrderStatus.Completed)
                {
                    ts.Complete();
                    return new CustomJsonResult(ResultType.Success, ResultCode.Success, "该订单已经支付完成");
                }


                if (orderToLllegalDealt.Status != Enumeration.OrderStatus.WaitPay)
                {
                    ts.Complete();
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "该订单未在就绪支付状态");
                }

                orderToLllegalDealt.Status = Enumeration.OrderStatus.Completed;
                orderToLllegalDealt.PayTime = this.DateTime;
                orderToLllegalDealt.CompleteTime = this.DateTime;
                orderToLllegalDealt.LastUpdateTime = this.DateTime;
                orderToLllegalDealt.Mender = operater;


                var orderToLllegalDealtDetails = CurrentDb.OrderToLllegalDealtDetails.Where(m => m.OrderId == orderToLllegalDealt.Id).ToList();

                foreach (var item in orderToLllegalDealtDetails)
                {
                    item.Status = Enumeration.OrderToLllegalDealtDetailsStatus.InDealt;
                    CurrentDb.SaveChanges();
                }

                var haoYiLianFund = CurrentDb.Fund.Where(m => m.UserId == (int)Enumeration.UserAccount.HaoYiLian).FirstOrDefault();
                haoYiLianFund.Balance += orderToLllegalDealt.Price;
                haoYiLianFund.Mender = operater;
                haoYiLianFund.LastUpdateTime = this.DateTime;

                var haoYiLianFundTrans = new FundTrans();
                haoYiLianFundTrans.UserId = haoYiLianFund.UserId;
                haoYiLianFundTrans.ChangeAmount = orderToLllegalDealt.Price;
                haoYiLianFundTrans.Balance = haoYiLianFund.Balance;
                haoYiLianFundTrans.Type = Enumeration.TransactionsType.LllegalDealt;
                haoYiLianFundTrans.Description = string.Format("订单号:{0},处理违章:{1}元", orderSn, orderToLllegalDealt.Price.ToF2Price());
                haoYiLianFundTrans.Creator = operater;
                haoYiLianFundTrans.CreateTime = this.DateTime;
                CurrentDb.FundTrans.Add(haoYiLianFundTrans);
                CurrentDb.SaveChanges();
                haoYiLianFundTrans.Sn = Sn.Build(SnType.FundTrans, haoYiLianFundTrans.Id).Sn;
                CurrentDb.SaveChanges();


                BizFactory.BizProcessesAudit.ChangeStatusByAuditFlowV1(orderToLllegalDealt.BizProcessesAuditId, Enumeration.AuditFlowV1Status.WaitDealt, operater, "", "订单已经支付，等待处理");


                ts.Complete();



                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "该订单支付结果反馈成功", null);
            }

            return result;
        }

        private CustomJsonResult PayCarInsureCompleted(int operater, string orderSn)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var orderToCarInsure = CurrentDb.OrderToCarInsure.Where(m => m.Sn == orderSn).FirstOrDefault();


                if (orderToCarInsure.Status == Enumeration.OrderStatus.Completed)
                {
                    ts.Complete();
                    return new CustomJsonResult(ResultType.Success, ResultCode.Success, "该订单已经支付完成");
                }

                if (orderToCarInsure.Status != Enumeration.OrderStatus.WaitPay)
                {
                    ts.Complete();
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "该订单未在就绪支付状态");
                }


                orderToCarInsure.Status = Enumeration.OrderStatus.Completed;
                orderToCarInsure.PayTime = this.DateTime;
                orderToCarInsure.CompleteTime = this.DateTime;
                orderToCarInsure.LastUpdateTime = this.DateTime;
                orderToCarInsure.Mender = operater;


                var bizProcessesAudit = CurrentDb.BizProcessesAudit.Where(m => m.AduitType == Enumeration.BizProcessesAuditType.OrderToCarInsure && m.AduitReferenceId == orderToCarInsure.Id).FirstOrDefault();
                if (bizProcessesAudit != null)
                {
                    BizFactory.BizProcessesAudit.ChangeAuditDetails(Enumeration.OperateType.Submit, Enumeration.CarInsureOfferDealtStep.Complete, bizProcessesAudit.Id, orderToCarInsure.Creator, null, "支付完成", this.DateTime);
                    BizFactory.BizProcessesAudit.ChangeCarInsureOfferDealtStatus(orderToCarInsure.Creator, bizProcessesAudit.Id, Enumeration.CarInsureOfferDealtStatus.Complete);
                }

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "该订单支付结果反馈成功");

            }

            return result;
        }

        private CustomJsonResult PayCarClaimCompleted(int operater, string orderSn)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var orderToCarClaim = CurrentDb.OrderToCarClaim.Where(m => m.Sn == orderSn).FirstOrDefault();


                if (orderToCarClaim.Status == Enumeration.OrderStatus.Completed)
                {
                    ts.Complete();
                    return new CustomJsonResult(ResultType.Success, ResultCode.Success, "该订单已经支付完成");
                }


                if (orderToCarClaim.Status != Enumeration.OrderStatus.WaitPay)
                {
                    ts.Complete();
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "该订单未在就绪支付状态");
                }




                orderToCarClaim.Status = Enumeration.OrderStatus.Completed;
                orderToCarClaim.PayTime = this.DateTime;
                orderToCarClaim.CompleteTime = this.DateTime;
                orderToCarClaim.LastUpdateTime = this.DateTime;
                orderToCarClaim.Mender = operater;




                if (orderToCarClaim.HandOrderId != null)
                {
                    var handOrder = CurrentDb.OrderToCarClaim.Where(m => m.Id == orderToCarClaim.HandOrderId.Value).FirstOrDefault();

                    handOrder.Status = Enumeration.OrderStatus.Completed;
                    handOrder.PayTime = this.DateTime;
                    handOrder.CompleteTime = this.DateTime;
                    handOrder.LastUpdateTime = this.DateTime;
                    handOrder.Mender = operater;
                    CurrentDb.SaveChanges();


                    var bizProcessesAudit = CurrentDb.BizProcessesAudit.Where(m => m.AduitType == Enumeration.BizProcessesAuditType.OrderToCarClaim && m.AduitReferenceId == orderToCarClaim.HandOrderId.Value).FirstOrDefault();
                    if (bizProcessesAudit != null)
                    {
                        BizFactory.BizProcessesAudit.ChangeAuditDetails(Enumeration.OperateType.Submit, Enumeration.CarClaimDealtStep.Complete, bizProcessesAudit.Id, operater, null, "支付完成", this.DateTime);
                        BizFactory.BizProcessesAudit.ChangeCarClaimDealtStatus(operater, bizProcessesAudit.Id, Enumeration.CarClaimDealtStatus.Complete);
                    }

                }

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "该订单支付结果反馈成功");
            }

            return result;
        }
    }
}
