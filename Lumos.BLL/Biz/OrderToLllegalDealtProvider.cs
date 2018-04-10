using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL
{
    public class OrderToLllegalDealtProvider : BaseProvider
    {
        public CustomJsonResult Submit(int operater, bool isGetConfirmInfo, OrderToLllegalDealt orderToLllegalDealt, List<OrderToLllegalDealtDetails> orderToLllegalDealtDetails)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var clientUser = CurrentDb.SysClientUser.Where(m => m.Id == orderToLllegalDealt.UserId).FirstOrDefault();
                var merchant = CurrentDb.Merchant.Where(m => m.Id == clientUser.MerchantId).FirstOrDefault();


                orderToLllegalDealt.SalesmanId = merchant.SalesmanId ?? 0;
                orderToLllegalDealt.AgentId = merchant.AgentId ?? 0;
                orderToLllegalDealt.Type = Enumeration.OrderType.LllegalDealt;
                orderToLllegalDealt.TypeName = Enumeration.OrderType.LllegalDealt.GetCnName();
                orderToLllegalDealt.SubmitTime = this.DateTime;
                orderToLllegalDealt.CreateTime = this.DateTime;
                orderToLllegalDealt.Creator = operater;
                orderToLllegalDealt.SumCount = orderToLllegalDealtDetails.Count();
                orderToLllegalDealt.SumFine = orderToLllegalDealtDetails.Sum(m => m.Fine);
                orderToLllegalDealt.SumPoint = orderToLllegalDealtDetails.Sum(m => m.Point);



                orderToLllegalDealt.SumServiceFees = orderToLllegalDealtDetails.Sum(m => m.ServiceFee);
                orderToLllegalDealt.SumLateFees = orderToLllegalDealtDetails.Sum(m => m.Late_fees);
                orderToLllegalDealt.Price = orderToLllegalDealt.SumFine + orderToLllegalDealt.SumServiceFees + orderToLllegalDealt.SumLateFees;
                CurrentDb.OrderToLllegalDealt.Add(orderToLllegalDealt);
                CurrentDb.SaveChanges();

                SnModel snModel = Sn.Build(SnType.OrderToLllegalQueryRecharge, orderToLllegalDealt.Id);

                orderToLllegalDealt.Sn = snModel.Sn;
                orderToLllegalDealt.TradeSnByWechat = snModel.TradeSnByWechat;
                orderToLllegalDealt.TradeSnByAlipay = snModel.TradeSnByAlipay;
                CurrentDb.SaveChanges();


                foreach (var item in orderToLllegalDealtDetails)
                {

                    item.OrderId = orderToLllegalDealt.Id;
                    item.Status = Enumeration.OrderToLllegalDealtDetailsStatus.WaitPay;
                    item.CreateTime = this.DateTime;
                    item.Creator = operater;

                    CurrentDb.OrderToLllegalDealtDetails.Add(item);
                    CurrentDb.SaveChanges();
                }



                OrderConfirmInfo yOrder = new OrderConfirmInfo();
                var bizProcessesAudit = BizFactory.BizProcessesAudit.Add(operater, Enumeration.BizProcessesAuditType.OrderToLllegalDealt, orderToLllegalDealt.Id, Enumeration.AuditFlowV1Status.Submit);
                if (isGetConfirmInfo)
                {
                    orderToLllegalDealt.Status = Enumeration.OrderStatus.WaitPay;

                    yOrder.OrderId = orderToLllegalDealt.Id;
                    yOrder.OrderSn = orderToLllegalDealt.Sn;
                    yOrder.remarks = orderToLllegalDealt.Remarks;
                    yOrder.orderType = orderToLllegalDealt.Type;
                    yOrder.orderTypeName = orderToLllegalDealt.TypeName;



                    //yOrder.confirmField.Add(new Entity.AppApi.OrderField("订单编号", orderToLllegalDealt.Sn.NullToEmpty()));
                    yOrder.confirmField.Add(new Entity.AppApi.OrderField("车牌号码", orderToLllegalDealt.CarNo.NullToEmpty()));
                    yOrder.confirmField.Add(new Entity.AppApi.OrderField("违章", string.Format("{0}次", orderToLllegalDealt.SumCount)));
                    yOrder.confirmField.Add(new Entity.AppApi.OrderField("扣分", orderToLllegalDealt.SumPoint.NullToEmpty()));
                    yOrder.confirmField.Add(new Entity.AppApi.OrderField("罚款", orderToLllegalDealt.SumFine.NullToEmpty()));
                    yOrder.confirmField.Add(new Entity.AppApi.OrderField("支付金额", string.Format("{0}元", orderToLllegalDealt.Price.NullToEmpty())));


                    #region 支持的支付方式
                    int[] payMethods = new int[1] { 1 };

                    foreach (var payWayId in payMethods)
                    {
                        var payWay = new PayWay();
                        payWay.id = payWayId;
                        yOrder.payMethod.Add(payWay);
                    }
                    #endregion

                    BizFactory.BizProcessesAudit.ChangeStatusByAuditFlowV1(bizProcessesAudit.Id, Enumeration.AuditFlowV1Status.waitGoDealt, operater, null, "提交订单，等待支付");

                }
                else
                {
                    orderToLllegalDealt.Status = Enumeration.OrderStatus.Submitted;
                    BizFactory.BizProcessesAudit.ChangeStatusByAuditFlowV1(bizProcessesAudit.Id, Enumeration.AuditFlowV1Status.Submit, operater, null, "提交订单，等待取单");
                }

                orderToLllegalDealt.BizProcessesAuditId = bizProcessesAudit.Id;


                CurrentDb.SaveChanges();

                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "提交成功", yOrder);
            }


            return result;
        }

        public CustomJsonResult Verify(int operater, Enumeration.OperateType operate, OrderToLllegalDealt orderToLllegalDealt, BizProcessesAudit bizProcessesAudit)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {

                var l_bizProcessesAudit = CurrentDb.BizProcessesAudit.Where(m => m.Id == bizProcessesAudit.Id && (m.Status == (int)Enumeration.AuditFlowV1Status.WaitVerify || m.Status == (int)Enumeration.AuditFlowV1Status.InVerify)).FirstOrDefault();

                if (bizProcessesAudit == null)
                {
                    return new CustomJsonResult(ResultType.Success, "该订单已经核实完成");
                }

                if (bizProcessesAudit.Auditor != null)
                {
                    if (bizProcessesAudit.Auditor.Value != operater)
                    {
                        return new CustomJsonResult(ResultType.Failure, "该订单其他用户正在核实");
                    }
                }


                var l_orderToLllegalDealt = CurrentDb.OrderToLllegalDealt.Where(m => m.Id == orderToLllegalDealt.Id).FirstOrDefault();


                l_orderToLllegalDealt.Remarks = bizProcessesAudit.TempAuditComments;

                switch (operate)
                {
                    case Enumeration.OperateType.Save:
                        result = new CustomJsonResult(ResultType.Success, "保存成功");
                        BizFactory.BizProcessesAudit.SaveTempAuditComments(bizProcessesAudit.Id, operater, bizProcessesAudit.TempAuditComments);
                        break;
                    case Enumeration.OperateType.Cancle:
                        BizFactory.BizProcessesAudit.ChangeStatusByAuditFlowV1(bizProcessesAudit.Id, Enumeration.AuditFlowV1Status.VerifyIncorrect, operater, bizProcessesAudit.TempAuditComments, "核实订单无效");

                        result = new CustomJsonResult(ResultType.Success, "提交成功");

                        break;
                    case Enumeration.OperateType.Submit:
                        l_orderToLllegalDealt.Status = Enumeration.OrderStatus.Completed;
                        l_orderToLllegalDealt.PayTime = this.DateTime;
                        l_orderToLllegalDealt.IsManVerifyPay = true;

                        var orderToLllegalDealtDetails = CurrentDb.OrderToLllegalDealtDetails.Where(m => m.OrderId == l_orderToLllegalDealt.Id).ToList();
                        if (orderToLllegalDealtDetails != null)
                        {
                            foreach (var item in orderToLllegalDealtDetails)
                            {
                                item.Status = Enumeration.OrderToLllegalDealtDetailsStatus.InDealt;
                                CurrentDb.SaveChanges();
                            }
                        }
                        BizFactory.BizProcessesAudit.ChangeStatusByAuditFlowV1(bizProcessesAudit.Id, Enumeration.AuditFlowV1Status.VerifyCorrect, operater, bizProcessesAudit.TempAuditComments, "核实订单正确，等待处理");
                        result = new CustomJsonResult(ResultType.Success, "提交成功");
                        break;
                }

                CurrentDb.SaveChanges();
                ts.Complete();
            }

            return result;

        }

        public CustomJsonResult Dealt(int operater, Enumeration.OperateType operate, OrderToLllegalDealt orderToLllegalDealt, BizProcessesAudit bizProcessesAudit)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {

                var l_bizProcessesAudit = CurrentDb.BizProcessesAudit.Where(m => m.Id == bizProcessesAudit.Id && (m.Status == (int)Enumeration.AuditFlowV1Status.WaitDealt || m.Status == (int)Enumeration.AuditFlowV1Status.InDealt)).FirstOrDefault();

                if (bizProcessesAudit == null)
                {
                    return new CustomJsonResult(ResultType.Success, "该订单已经处理完成");
                }

                if (bizProcessesAudit.Auditor != null)
                {
                    if (bizProcessesAudit.Auditor.Value != operater)
                    {
                        return new CustomJsonResult(ResultType.Failure, "该订单其他用户正在处理");
                    }
                }


                var l_orderToLllegalDealt = CurrentDb.OrderToLllegalDealt.Where(m => m.Id == orderToLllegalDealt.Id).FirstOrDefault();


                l_orderToLllegalDealt.Remarks = bizProcessesAudit.TempAuditComments;

                switch (operate)
                {
                    case Enumeration.OperateType.Save:
                        BizFactory.BizProcessesAudit.SaveTempAuditComments(bizProcessesAudit.Id, operater, bizProcessesAudit.TempAuditComments);
                        result = new CustomJsonResult(ResultType.Success, "保存成功");
                        break;
                    case Enumeration.OperateType.Cancle:
                        l_orderToLllegalDealt.Status = Enumeration.OrderStatus.Cancled;
                        l_orderToLllegalDealt.CancleTime = this.DateTime;
                        BizFactory.BizProcessesAudit.ChangeStatusByAuditFlowV1(bizProcessesAudit.Id, Enumeration.AuditFlowV1Status.DealtFailure, operater, bizProcessesAudit.TempAuditComments, "订单处理失败");
                        result = new CustomJsonResult(ResultType.Success, "提交成功");

                        break;
                    case Enumeration.OperateType.Submit:

                        l_orderToLllegalDealt.Status = Enumeration.OrderStatus.Completed;
                        l_orderToLllegalDealt.CompleteTime = this.DateTime;

                        var orderToLllegalDealtDetails = CurrentDb.OrderToLllegalDealtDetails.Where(m => m.OrderId == l_orderToLllegalDealt.Id).ToList();
                        if (orderToLllegalDealtDetails != null)
                        {
                            foreach (var item in orderToLllegalDealtDetails)
                            {
                                item.Status = Enumeration.OrderToLllegalDealtDetailsStatus.Completed;
                                CurrentDb.SaveChanges();
                            }
                        }
                        BizFactory.BizProcessesAudit.ChangeStatusByAuditFlowV1(bizProcessesAudit.Id, Enumeration.AuditFlowV1Status.DealtSuccess, operater, bizProcessesAudit.TempAuditComments, "订单处理成功");
                        result = new CustomJsonResult(ResultType.Success, "提交成功");
                        break;

                }

                CurrentDb.SaveChanges();
                ts.Complete();
            }

            return result;

        }

    }
}
