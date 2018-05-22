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
    public class OrderToInsuranceProvider : BaseProvider
    {
        public CustomJsonResult Submit(int operater, OrderToInsurance orderToInsurance)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var clientUser = CurrentDb.SysClientUser.Where(m => m.Id == orderToInsurance.UserId).FirstOrDefault();
                var merchant = CurrentDb.Merchant.Where(m => m.Id == clientUser.MerchantId).FirstOrDefault();
                var productSku = CurrentDb.ProductSku.Where(m => m.Id == orderToInsurance.ProductSkuId).FirstOrDefault();
                var product = CurrentDb.Product.Where(m => m.Id == productSku.ProductId).FirstOrDefault();

                orderToInsurance.SalesmanId = merchant.SalesmanId ?? 0;
                orderToInsurance.AgentId = merchant.AgentId ?? 0;
                orderToInsurance.Type = Enumeration.OrderType.Insure;
                orderToInsurance.TypeName = Enumeration.OrderType.Insure.GetCnName();
                orderToInsurance.ProductId = product.Id;
                orderToInsurance.ProductType = product.Type;
                orderToInsurance.ProductName = product.Name;
                orderToInsurance.ProductSkuId = productSku.Id;
                orderToInsurance.ProductSkuName = productSku.Name;
                orderToInsurance.InsuranceCompanyId = product.SupplierId;
                orderToInsurance.InsuranceCompanyName = product.Supplier;
                orderToInsurance.Status = Enumeration.OrderStatus.Submitted;
                orderToInsurance.SubmitTime = this.DateTime;
                orderToInsurance.CreateTime = this.DateTime;
                orderToInsurance.Creator = operater;
                CurrentDb.OrderToInsurance.Add(orderToInsurance);
                CurrentDb.SaveChanges();


                SnModel snModel = Sn.Build(SnType.OrderToCredit, orderToInsurance.Id);

                orderToInsurance.Sn = snModel.Sn;


                var bizProcessesAudit = BizFactory.BizProcessesAudit.Add(operater, Enumeration.BizProcessesAuditType.OrderToInsurance, orderToInsurance.UserId, orderToInsurance.MerchantId, orderToInsurance.Id, Enumeration.AuditFlowV1Status.Submit);
                BizFactory.BizProcessesAudit.ChangeStatusByAuditFlowV1(bizProcessesAudit.Id, Enumeration.AuditFlowV1Status.Submit, operater, null, "提交订单，等待取单");

                orderToInsurance.BizProcessesAuditId = bizProcessesAudit.Id;

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, "提交成功");
            }


            return result;
        }

        public CustomJsonResult Verify(int operater, Enumeration.OperateType operate, OrderToInsurance orderToInsurance, BizProcessesAudit bizProcessesAudit)
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

                var l_orderToInsurance = CurrentDb.OrderToInsurance.Where(m => m.Id == orderToInsurance.Id).FirstOrDefault();

                l_orderToInsurance.Remarks = bizProcessesAudit.TempAuditComments;

                switch (operate)
                {
                    case Enumeration.OperateType.Save:
                        BizFactory.BizProcessesAudit.SaveTempAuditComments(bizProcessesAudit.Id, operater, bizProcessesAudit.TempAuditComments);
                        result = new CustomJsonResult(ResultType.Success, "保存成功");
                        break;
                    case Enumeration.OperateType.Cancle:
                        l_orderToInsurance.Status = Enumeration.OrderStatus.Cancled;
                        l_orderToInsurance.CancleTime = this.DateTime;
                        BizFactory.BizProcessesAudit.ChangeStatusByAuditFlowV1(bizProcessesAudit.Id, Enumeration.AuditFlowV1Status.VerifyIncorrect, operater, bizProcessesAudit.TempAuditComments, "核实订单无效");
                        result = new CustomJsonResult(ResultType.Success, "提交成功");
                        break;
                    case Enumeration.OperateType.Submit:
                        l_orderToInsurance.FollowStatus = 1;
                        BizFactory.BizProcessesAudit.ChangeStatusByAuditFlowV1(bizProcessesAudit.Id, Enumeration.AuditFlowV1Status.VerifyCorrect, operater, bizProcessesAudit.TempAuditComments, "核实订单正确，等待处理");
                        result = new CustomJsonResult(ResultType.Success, "提交成功");
                        break;
                }

                CurrentDb.SaveChanges();
                ts.Complete();
            }

            return result;

        }

        public CustomJsonResult Dealt(int operater, Enumeration.OperateType operate, OrderToInsurance orderToInsurance, BizProcessesAudit bizProcessesAudit)
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

                var l_orderToInsurance = CurrentDb.OrderToInsurance.Where(m => m.Id == orderToInsurance.Id).FirstOrDefault();

                l_orderToInsurance.Remarks = bizProcessesAudit.TempAuditComments;

                switch (operate)
                {
                    case Enumeration.OperateType.Save:
                        BizFactory.BizProcessesAudit.SaveTempAuditComments(bizProcessesAudit.Id, operater, bizProcessesAudit.TempAuditComments);
                        result = new CustomJsonResult(ResultType.Success, "保存成功");
                        break;
                    case Enumeration.OperateType.Cancle:
                        l_orderToInsurance.Status = Enumeration.OrderStatus.Cancled;
                        l_orderToInsurance.CancleTime = this.DateTime;
                        BizFactory.BizProcessesAudit.ChangeStatusByAuditFlowV1(bizProcessesAudit.Id, Enumeration.AuditFlowV1Status.DealtFailure, operater, bizProcessesAudit.TempAuditComments, "订单处理失败");
                        result = new CustomJsonResult(ResultType.Success, "提交成功");
                        break;
                    case Enumeration.OperateType.Reject:
                        BizFactory.BizProcessesAudit.ChangeStatusByAuditFlowV1(bizProcessesAudit.Id, Enumeration.AuditFlowV1Status.DealtReject, operater, bizProcessesAudit.TempAuditComments, "订单处理驳回");
                        result = new CustomJsonResult(ResultType.Success, "提交成功");

                        break;
                    case Enumeration.OperateType.Submit:
                        l_orderToInsurance.Status = Enumeration.OrderStatus.Completed;
                        l_orderToInsurance.CompleteTime = this.DateTime;
                        l_orderToInsurance.FollowStatus = 1;
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
