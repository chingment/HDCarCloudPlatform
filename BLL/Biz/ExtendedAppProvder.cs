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
    public class ExtendedAppProvder : BaseProvider
    {
        public CustomJsonResult ApplyOn(int operater, ExtendedApp extendedApp)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var isExsits = CurrentDb.ExtendedApp.Where(m => m.Name == extendedApp.Name || m.LinkUrl == extendedApp.LinkUrl).Count();
                if (isExsits > 0)
                {
                    return new CustomJsonResult(ResultType.Failure, "该已存在相同应用的名称或链接");
                }

                var isExsitsAppKey = CurrentDb.ExtendedApp.Where(m => m.AppKey == extendedApp.AppKey).Count();
                if (isExsitsAppKey > 0)
                {
                    return new CustomJsonResult(ResultType.Failure, "该已存在相同应用AppKey");
                }

                extendedApp.CreateTime = this.DateTime;
                extendedApp.Creator = operater;
                extendedApp.Status = Enumeration.ExtendedAppStatus.AuditOn;
                extendedApp.Type = Enumeration.ExtendedAppType.ThirdPartyApp;
                extendedApp.AppSecret = Guid.NewGuid().ToString().Replace("-", "");
                CurrentDb.ExtendedApp.Add(extendedApp);
                CurrentDb.SaveChanges();

                BizFactory.BizProcessesAudit.Add(operater, Enumeration.BizProcessesAuditType.ExtendedAppOn, extendedApp.Id, Enumeration.ExtendedAppAuditStatus.WaitAudit, "上架新应用");

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, "申请成功");
            }

            return result;
        }


        public CustomJsonResult ApplyOff(int operater, int extendedAppId, string remarks)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var extendedApp = CurrentDb.ExtendedApp.Where(m => m.Id == extendedAppId).FirstOrDefault();

                if (extendedApp.Status == Enumeration.ExtendedAppStatus.AuditOff)
                {
                    return new CustomJsonResult(ResultType.Failure, "该应用已经在下架申请中");
                }


                extendedApp.LastUpdateTime = this.DateTime;
                extendedApp.Mender = operater;
                extendedApp.Status = Enumeration.ExtendedAppStatus.AuditOff;


                CurrentDb.SaveChanges();

                BizFactory.BizProcessesAudit.Add(operater, Enumeration.BizProcessesAuditType.ExtendedAppOff, extendedApp.Id, Enumeration.ExtendedAppAuditStatus.WaitAudit, remarks);

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, "申请成功");
            }

            return result;
        }


        public CustomJsonResult ApplyRecovery(int operater, int extendedAppId, string remarks)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var extendedApp = CurrentDb.ExtendedApp.Where(m => m.Id == extendedAppId).FirstOrDefault();

                if (extendedApp.Status == Enumeration.ExtendedAppStatus.AuditRecovery)
                {
                    return new CustomJsonResult(ResultType.Failure, "该应用已经在恢复审核中");
                }

                extendedApp.LastUpdateTime = this.DateTime;
                extendedApp.Mender = operater;
                extendedApp.Status = Enumeration.ExtendedAppStatus.AuditRecovery;


                CurrentDb.SaveChanges();

                BizFactory.BizProcessesAudit.Add(operater, Enumeration.BizProcessesAuditType.ExtendedAppRecovery, extendedApp.Id, Enumeration.ExtendedAppAuditStatus.WaitAudit, remarks);

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, "申请成功");
            }

            return result;
        }

        public CustomJsonResult PrimaryAudit(int operater, Enumeration.OperateType operate, ExtendedApp extendedApp, BizProcessesAuditDetails bizProcessesAuditDetails)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var bizProcessesAudit = CurrentDb.BizProcessesAudit.Where(m => m.Id == bizProcessesAuditDetails.BizProcessesAuditId && (m.Status == (int)Enumeration.ExtendedAppAuditStatus.WaitAudit || m.Status == (int)Enumeration.ExtendedAppAuditStatus.InAudit)).FirstOrDefault();

                if (bizProcessesAudit == null)
                {
                    return new CustomJsonResult(ResultType.Success, "该应用已经审核过");
                }

                if (bizProcessesAudit.Auditor != null)
                {
                    if (bizProcessesAudit.Auditor.Value != operater)
                    {
                        return new CustomJsonResult(ResultType.Failure, "该应用其他用户正在审核");
                    }
                }

                switch (operate)
                {
                    case Enumeration.OperateType.Save:

                        BizFactory.BizProcessesAudit.ChangeAuditDetails(operate, Enumeration.ExtendedAppAuditStep.PrimaryAudit, bizProcessesAudit.Id, operater, bizProcessesAuditDetails.AuditComments, null);

                        result = new CustomJsonResult(ResultType.Success, "保存成功");
                        break;
                    case Enumeration.OperateType.Submit:

                        BizFactory.BizProcessesAudit.ChangeAuditDetails(operate, Enumeration.ExtendedAppAuditStep.PrimaryAudit, bizProcessesAudit.Id, operater, bizProcessesAuditDetails.AuditComments, null);

                        BizFactory.BizProcessesAudit.ChangeExtendedAppAuditStatus(operater, bizProcessesAudit.Id, Enumeration.ExtendedAppAuditStatus.WaitReview);

                        result = new CustomJsonResult(ResultType.Success, "提交成功");
                        break;
                }

                CurrentDb.SaveChanges();
                ts.Complete();
            }

            return result;
        }

        public CustomJsonResult SeniorAudit(int operater, Enumeration.OperateType operate, BizProcessesAuditDetails auditCommentsCurrent)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var bizProcessesAudit = CurrentDb.BizProcessesAudit.Where(m => m.Id == auditCommentsCurrent.BizProcessesAuditId && (m.Status == (int)Enumeration.ExtendedAppAuditStatus.WaitReview || m.Status == (int)Enumeration.ExtendedAppAuditStatus.InReview)).FirstOrDefault();

                if (bizProcessesAudit == null)
                {
                    return new CustomJsonResult(ResultType.Success, "该应用已经审核过");
                }

                if (bizProcessesAudit.Auditor != null)
                {
                    if (bizProcessesAudit.Auditor.Value != operater)
                    {
                        return new CustomJsonResult(ResultType.Failure, "该应用其他用户正在审核");
                    }
                }

                var extendedApp = CurrentDb.ExtendedApp.Where(m => m.Id == bizProcessesAudit.AduitReferenceId).FirstOrDefault();

                switch (operate)
                {
                    case Enumeration.OperateType.Save:

                        BizFactory.BizProcessesAudit.ChangeAuditDetails(operate, Enumeration.ExtendedAppAuditStep.SeniorAudit, bizProcessesAudit.Id, operater, auditCommentsCurrent.AuditComments, null);

                        result = new CustomJsonResult(ResultType.Success, "保存成功");
                        break;
                    case Enumeration.OperateType.Pass:

                        BizFactory.BizProcessesAudit.ChangeAuditDetails(operate, Enumeration.ExtendedAppAuditStep.PrimaryAudit, bizProcessesAudit.Id, operater, auditCommentsCurrent.AuditComments, null, this.DateTime);

                        BizFactory.BizProcessesAudit.ChangeExtendedAppAuditStatus(operater, bizProcessesAudit.Id, Enumeration.ExtendedAppAuditStatus.ReviewPass);

                        if (bizProcessesAudit.AduitType == Enumeration.BizProcessesAuditType.ExtendedAppOn)
                        {
                            extendedApp.Status = Enumeration.ExtendedAppStatus.AuditOnPass;
                            extendedApp.IsDisplay = true;
                        }
                        else if (bizProcessesAudit.AduitType == Enumeration.BizProcessesAuditType.ExtendedAppOff)
                        {
                            extendedApp.Status = Enumeration.ExtendedAppStatus.AuditOffPass;
                            extendedApp.IsDisplay = false;
                        }
                        else if (bizProcessesAudit.AduitType == Enumeration.BizProcessesAuditType.ExtendedAppRecovery)
                        {
                            extendedApp.Status = Enumeration.ExtendedAppStatus.AuditRecoveryPass;
                            extendedApp.IsDisplay = true;
                        }

                        extendedApp.LastUpdateTime = this.DateTime;
                        extendedApp.Mender = operater;

                        result = new CustomJsonResult(ResultType.Success, "审核通过");
                        break;
                    case Enumeration.OperateType.Refuse:

                        BizFactory.BizProcessesAudit.ChangeAuditDetails(operate, Enumeration.ExtendedAppAuditStep.SeniorAudit, bizProcessesAudit.Id, operater, auditCommentsCurrent.AuditComments, null, this.DateTime);

                        BizFactory.BizProcessesAudit.ChangeExtendedAppAuditStatus(operater, bizProcessesAudit.Id, Enumeration.ExtendedAppAuditStatus.ReviewRefuse);


                        if (bizProcessesAudit.AduitType == Enumeration.BizProcessesAuditType.ExtendedAppOn)
                        {
                            extendedApp.Status = Enumeration.ExtendedAppStatus.AuditOnRefuse;
                        }
                        else if (bizProcessesAudit.AduitType == Enumeration.BizProcessesAuditType.ExtendedAppOff)
                        {
                            extendedApp.Status = Enumeration.ExtendedAppStatus.AuditOffRefuse;
                        }
                        else if (bizProcessesAudit.AduitType == Enumeration.BizProcessesAuditType.ExtendedAppRecovery)
                        {
                            extendedApp.Status = Enumeration.ExtendedAppStatus.AuditRecoveryRefuse;
                        }

                        extendedApp.LastUpdateTime = this.DateTime;
                        extendedApp.Mender = operater;

                        result = new CustomJsonResult(ResultType.Success, "审核拒绝");
                        break;
                }

                CurrentDb.SaveChanges();
                ts.Complete();
            }

            return result;
        }

    }
}
