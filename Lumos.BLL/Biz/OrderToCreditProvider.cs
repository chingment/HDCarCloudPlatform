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
    public class OrderToCreditProvider : BaseProvider
    {
        public CustomJsonResult Submit(int operater, OrderToCredit orderToCredit)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                //用户信息
                var clientUser = CurrentDb.SysClientUser.Where(m => m.Id == orderToCredit.UserId).FirstOrDefault();
                //商户信息
                var merchant = CurrentDb.Merchant.Where(m => m.Id == clientUser.MerchantId).FirstOrDefault();


                orderToCredit.SalesmanId = merchant.SalesmanId ?? 0;
                orderToCredit.AgentId = merchant.AgentId ?? 0;
                orderToCredit.Type = Enumeration.OrderType.Credit;
                orderToCredit.TypeName = Enumeration.OrderType.Credit.GetCnName();
                orderToCredit.Status = Enumeration.OrderStatus.Submitted;
                orderToCredit.SubmitTime = this.DateTime;
                orderToCredit.CreateTime = this.DateTime;
                orderToCredit.Creator = operater;
                CurrentDb.OrderToCredit.Add(orderToCredit);
                CurrentDb.SaveChanges();


                SnModel snModel = Sn.Build(SnType.OrderToCredit, orderToCredit.Id);

                orderToCredit.Sn = snModel.Sn;
                orderToCredit.TradeSnByWechat = snModel.TradeSnByWechat;
                orderToCredit.TradeSnByAlipay = snModel.TradeSnByAlipay;

                var bizProcessesAudit = BizFactory.BizProcessesAudit.Add(operater, Enumeration.BizProcessesAuditType.OrderToCredit, orderToCredit.UserId, orderToCredit.MerchantId, orderToCredit.Id, Enumeration.AuditFlowV1Status.Submit);
                BizFactory.BizProcessesAudit.ChangeStatusByAuditFlowV1(bizProcessesAudit.Id, Enumeration.AuditFlowV1Status.Submit, operater, null, "提交订单，等待取单");

                orderToCredit.BizProcessesAuditId = bizProcessesAudit.Id;

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, "提交成功");
            }


            return result;
        }

        public CustomJsonResult Verify(int operater, Enumeration.OperateType operate, OrderToCredit orderToCredit, BizProcessesAudit bizProcessesAudit)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {

                var l_bizProcessesAudit = CurrentDb.BizProcessesAudit.Where(m => m.Id == bizProcessesAudit.Id && (m.Status == (int)Enumeration.AuditFlowV1Status.WaitVerify || m.Status == (int)Enumeration.AuditFlowV1Status.InVerify)).FirstOrDefault();

                if (l_bizProcessesAudit == null)
                {
                    return new CustomJsonResult(ResultType.Success, "该订单已经核实完成");
                }

                if (l_bizProcessesAudit.Auditor != null)
                {
                    if (l_bizProcessesAudit.Auditor.Value != operater)
                    {
                        return new CustomJsonResult(ResultType.Failure, "该订单其他用户正在核实");
                    }
                }

                var l_orderToCredit = CurrentDb.OrderToCredit.Where(m => m.Id == orderToCredit.Id).FirstOrDefault();

                l_orderToCredit.Remarks = bizProcessesAudit.TempAuditComments;

                switch (operate)
                {
                    case Enumeration.OperateType.Save:
                        BizFactory.BizProcessesAudit.SaveTempAuditComments(bizProcessesAudit.Id, operater, bizProcessesAudit.TempAuditComments);
                        result = new CustomJsonResult(ResultType.Success, "保存成功");
                        break;
                    case Enumeration.OperateType.Cancle:
                        l_orderToCredit.Status = Enumeration.OrderStatus.Cancled;
                        l_orderToCredit.CancleTime = this.DateTime;
                        BizFactory.BizProcessesAudit.ChangeStatusByAuditFlowV1(bizProcessesAudit.Id, Enumeration.AuditFlowV1Status.VerifyIncorrect, operater, bizProcessesAudit.TempAuditComments, "核实订单无效");
                        result = new CustomJsonResult(ResultType.Success, "提交成功");
                        break;
                    case Enumeration.OperateType.Submit:
                        l_orderToCredit.FollowStatus = 1;
                        BizFactory.BizProcessesAudit.ChangeStatusByAuditFlowV1(bizProcessesAudit.Id, Enumeration.AuditFlowV1Status.VerifyCorrect, operater, bizProcessesAudit.TempAuditComments, "核实订单正确，等待处理");
                        result = new CustomJsonResult(ResultType.Success, "提交成功");
                        break;
                }

                CurrentDb.SaveChanges();
                ts.Complete();
            }

            return result;

        }

        public CustomJsonResult Dealt(int operater, Enumeration.OperateType operate, OrderToCredit orderToCredit, BizProcessesAudit bizProcessesAudit)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {

                var l_bizProcessesAudit = CurrentDb.BizProcessesAudit.Where(m => m.Id == bizProcessesAudit.Id && (m.Status == (int)Enumeration.AuditFlowV1Status.WaitDealt || m.Status == (int)Enumeration.AuditFlowV1Status.InDealt)).FirstOrDefault();

                if (l_bizProcessesAudit == null)
                {
                    return new CustomJsonResult(ResultType.Success, "该订单已经处理完成");
                }

                if (l_bizProcessesAudit.Auditor != null)
                {
                    if (l_bizProcessesAudit.Auditor.Value != operater)
                    {
                        return new CustomJsonResult(ResultType.Failure, "该订单其他用户正在处理");
                    }
                }

                var l_orderToCredit = CurrentDb.OrderToCredit.Where(m => m.Id == orderToCredit.Id).FirstOrDefault();

                l_orderToCredit.Remarks = bizProcessesAudit.TempAuditComments;

                switch (operate)
                {
                    case Enumeration.OperateType.Save:
                        BizFactory.BizProcessesAudit.SaveTempAuditComments(bizProcessesAudit.Id, operater, bizProcessesAudit.TempAuditComments);
                        result = new CustomJsonResult(ResultType.Success, "保存成功");
                        break;
                    case Enumeration.OperateType.Cancle:
                        l_orderToCredit.Status = Enumeration.OrderStatus.Cancled;
                        l_orderToCredit.CancleTime = this.DateTime;
                        BizFactory.BizProcessesAudit.ChangeStatusByAuditFlowV1(bizProcessesAudit.Id, Enumeration.AuditFlowV1Status.DealtFailure, operater, bizProcessesAudit.TempAuditComments, "订单处理失败");
                        result = new CustomJsonResult(ResultType.Success, "提交成功");
                        break;
                    case Enumeration.OperateType.Reject:
                        BizFactory.BizProcessesAudit.ChangeStatusByAuditFlowV1(bizProcessesAudit.Id, Enumeration.AuditFlowV1Status.DealtReject, operater, bizProcessesAudit.TempAuditComments, "订单处理驳回");
                        result = new CustomJsonResult(ResultType.Success, "提交成功");

                        break;
                    case Enumeration.OperateType.Submit:
                        l_orderToCredit.Status = Enumeration.OrderStatus.Completed;
                        l_orderToCredit.CompleteTime = this.DateTime;
                        l_orderToCredit.FollowStatus = 1;
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
