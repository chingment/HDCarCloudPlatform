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
    public class OrderToApplyLossAssessProvider : BaseProvider
    {
        public CustomJsonResult Submit(int operater, OrderToApplyLossAssess orderToApplyLossAssess)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var clientUser = CurrentDb.SysClientUser.Where(m => m.Id == orderToApplyLossAssess.UserId).FirstOrDefault();
                var merchant = CurrentDb.Merchant.Where(m => m.Id == clientUser.MerchantId).FirstOrDefault();
                var product = CurrentDb.Product.Where(m => m.Id == (int)Enumeration.ProductType.ApplyLossAssess).FirstOrDefault();

                var insuranceCompany = CurrentDb.InsuranceCompany.Where(m => m.Id == orderToApplyLossAssess.InsuranceCompanyId).FirstOrDefault();
                orderToApplyLossAssess.SalesmanId = merchant.SalesmanId ?? 0;
                orderToApplyLossAssess.AgentId = merchant.AgentId ?? 0;
                orderToApplyLossAssess.ProductId = product.Id;
                orderToApplyLossAssess.ProductType = product.Type;
                orderToApplyLossAssess.ProductName = product.Name;
                orderToApplyLossAssess.InsuranceCompanyName = insuranceCompany.Name;
                orderToApplyLossAssess.Status = Enumeration.OrderStatus.Submitted;
                orderToApplyLossAssess.ApplyTime = this.DateTime;
                orderToApplyLossAssess.SubmitTime = this.DateTime;
                orderToApplyLossAssess.CreateTime = this.DateTime;
                orderToApplyLossAssess.Creator = operater;
                CurrentDb.OrderToApplyLossAssess.Add(orderToApplyLossAssess);
                CurrentDb.SaveChanges();

                SnModel snModel = Sn.Build(SnType.OrderToApplyLossAssess, orderToApplyLossAssess.Id);

                orderToApplyLossAssess.Sn = snModel.Sn;
                orderToApplyLossAssess.TradeSnByWechat = snModel.TradeSnByWechat;
                orderToApplyLossAssess.TradeSnByAlipay = snModel.TradeSnByAlipay;

                var bizProcessesAudit = BizFactory.BizProcessesAudit.Add(operater, Enumeration.BizProcessesAuditType.OrderToApplyLossAssess, orderToApplyLossAssess.Id, Enumeration.AuditFlowV1Status.Submit);
                BizFactory.BizProcessesAudit.ChangeStatusByAuditFlowV1(bizProcessesAudit.Id, Enumeration.AuditFlowV1Status.Submit, operater, null, "提交订单，等待取单");

                orderToApplyLossAssess.BizProcessesAuditId = bizProcessesAudit.Id;
                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, "提交成功");
            }


            return result;
        }

        public CustomJsonResult Verify(int operater, Enumeration.OperateType operate, OrderToApplyLossAssess orderToApplyLossAssess, BizProcessesAudit bizProcessesAudit)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {

                var l_bizProcessesAudit = CurrentDb.BizProcessesAudit.Where(m => m.Id == bizProcessesAudit.Id & (m.Status == (int)Enumeration.AuditFlowV1Status.WaitVerify || m.Status == (int)Enumeration.AuditFlowV1Status.InVerify)).FirstOrDefault();

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


                var l_orderToApplyLossAssess = CurrentDb.OrderToApplyLossAssess.Where(m => m.Id == orderToApplyLossAssess.Id).FirstOrDefault();


                l_orderToApplyLossAssess.Remarks = bizProcessesAudit.TempAuditComments;

                switch (operate)
                {
                    case Enumeration.OperateType.Save:
                        BizFactory.BizProcessesAudit.SaveTempAuditComments(bizProcessesAudit.Id, operater, bizProcessesAudit.TempAuditComments);
                        result = new CustomJsonResult(ResultType.Success, "保存成功");
                        break;
                    case Enumeration.OperateType.Cancle:
                        l_orderToApplyLossAssess.Status = Enumeration.OrderStatus.Cancled;
                        l_orderToApplyLossAssess.CancleTime = this.DateTime;
                        BizFactory.BizProcessesAudit.ChangeStatusByAuditFlowV1(bizProcessesAudit.Id, Enumeration.AuditFlowV1Status.VerifyIncorrect, operater, bizProcessesAudit.TempAuditComments, "核实订单无效");
                        result = new CustomJsonResult(ResultType.Success, "提交成功");
                        break;
                    case Enumeration.OperateType.Submit:
                        l_orderToApplyLossAssess.FollowStatus = 1;
                        BizFactory.BizProcessesAudit.ChangeStatusByAuditFlowV1(bizProcessesAudit.Id, Enumeration.AuditFlowV1Status.VerifyCorrect, operater, bizProcessesAudit.TempAuditComments, "核实订单正确，等待处理");
                        result = new CustomJsonResult(ResultType.Success, "提交成功");
                        break;
                }

                CurrentDb.SaveChanges();
                ts.Complete();
            }

            return result;

        }


        public CustomJsonResult Dealt(int operater, Enumeration.OperateType operate, OrderToApplyLossAssess orderToApplyLossAssess, BizProcessesAudit bizProcessesAudit)
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


                var l_orderToApplyLossAssess = CurrentDb.OrderToApplyLossAssess.Where(m => m.Id == orderToApplyLossAssess.Id).FirstOrDefault();


                l_orderToApplyLossAssess.Remarks = bizProcessesAudit.TempAuditComments;


                switch (operate)
                {
                    case Enumeration.OperateType.Save:
                        BizFactory.BizProcessesAudit.SaveTempAuditComments(bizProcessesAudit.Id, operater, bizProcessesAudit.TempAuditComments);
                        result = new CustomJsonResult(ResultType.Success, "保存成功");
                        break;
                    case Enumeration.OperateType.Cancle:
                        l_orderToApplyLossAssess.Status = Enumeration.OrderStatus.Cancled;
                        l_orderToApplyLossAssess.CancleTime = this.DateTime;
                        BizFactory.BizProcessesAudit.ChangeStatusByAuditFlowV1(bizProcessesAudit.Id, Enumeration.AuditFlowV1Status.DealtFailure, operater, bizProcessesAudit.TempAuditComments, "订单处理失败");
                        result = new CustomJsonResult(ResultType.Success, "提交成功");
                        break;
                    case Enumeration.OperateType.Submit:
                        l_orderToApplyLossAssess.Status = Enumeration.OrderStatus.Completed;
                        l_orderToApplyLossAssess.CompleteTime = this.DateTime;
                        l_orderToApplyLossAssess.FollowStatus = 1;
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
