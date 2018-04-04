using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class BizProcessesAuditProvider : BaseProvider
    {
        public BizProcessesAudit Add(int operater, Enumeration.BizProcessesAuditType type, int referenceid)
        {
            DateTime nowDate = DateTime.Now;
            BizProcessesAudit bizProcessesAudit = new BizProcessesAudit();
            bizProcessesAudit.AduitType = type;
            string aduitTypeEnumName = "" + type.GetType().FullName + ", Lumos.Entity";
            bizProcessesAudit.AduitTypeEnumName = aduitTypeEnumName;
            bizProcessesAudit.AduitReferenceId = referenceid;
            bizProcessesAudit.Status = 0;
            bizProcessesAudit.StartTime = nowDate;
            bizProcessesAudit.Creator = operater;
            bizProcessesAudit.CreateTime = nowDate;
            CurrentDb.BizProcessesAudit.Add(bizProcessesAudit);
            CurrentDb.SaveChanges();
            return bizProcessesAudit;
        }

        public void ChangeAuditDetails(Enumeration.OperateType operate, object auditStep, int bizProcessesAuditId, int operater, string auditComments, string description, DateTime? auditTime = null)
        {
            DateTime nowDate = DateTime.Now;
            var bizProcessesAudit = CurrentDb.BizProcessesAudit.Where(m => m.Id == bizProcessesAuditId).FirstOrDefault();
            if (bizProcessesAudit != null)
            {
                if (bizProcessesAudit.EndTime == null)
                {
                    var detailsHistory = CurrentDb.BizProcessesAuditDetails.Where(m => m.AuditStep == (int)auditStep && m.BizProcessesAuditId == bizProcessesAudit.Id).OrderByDescending(m => m.CreateTime).Take(1).FirstOrDefault();
                    if (detailsHistory == null)
                    {
                        detailsHistory = new BizProcessesAuditDetails();
                        detailsHistory.AuditStep = (int)auditStep;
                        string suditStepenumName = "" + auditStep.GetType().FullName + ", Lumos.Entity";
                        detailsHistory.AuditStepEnumName = suditStepenumName;
                        detailsHistory.BizProcessesAuditId = bizProcessesAuditId;
                        detailsHistory.Auditor = operater;
                        detailsHistory.AuditComments = auditComments;
                        detailsHistory.Description = description;
                        if (operate != Enumeration.OperateType.Save)
                        {
                            detailsHistory.AuditTime = nowDate;
                        }
                        detailsHistory.CreateTime = nowDate;
                        detailsHistory.Creator = operater;
                        CurrentDb.BizProcessesAuditDetails.Add(detailsHistory);
                    }
                    else
                    {
                        if (detailsHistory.AuditTime == null)
                        {
                            detailsHistory.Auditor = operater;
                            detailsHistory.AuditComments = auditComments;
                            detailsHistory.Description = description;
                            if (operate != Enumeration.OperateType.Save)
                            {
                                detailsHistory.AuditTime = nowDate;
                            }

                        }
                        else
                        {
                            detailsHistory = new BizProcessesAuditDetails();
                            detailsHistory.AuditStep = (int)auditStep;
                            string suditStepenumName = "" + auditStep.GetType().FullName + ", Lumos.Entity";
                            detailsHistory.AuditStepEnumName = suditStepenumName;
                            detailsHistory.BizProcessesAuditId = bizProcessesAuditId;
                            detailsHistory.Auditor = operater;
                            detailsHistory.AuditComments = auditComments;
                            detailsHistory.Description = description;
                            if (operate != Enumeration.OperateType.Save)
                            {
                                detailsHistory.AuditTime = nowDate;
                            }
                            detailsHistory.CreateTime = nowDate;
                            detailsHistory.Creator = operater;
                            CurrentDb.BizProcessesAuditDetails.Add(detailsHistory);
                        }
                    }


                    CurrentDb.SaveChanges();
                }
            }

        }

        public void ChangeAuditDetailsAuditComments(int operater, int bizProcessesAuditDetailsId, string auditComments, string description, DateTime? auditTime = null)
        {
            DateTime nowDate = DateTime.Now;
            var detailsHistory = CurrentDb.BizProcessesAuditDetails.Where(m => m.Id == bizProcessesAuditDetailsId).FirstOrDefault();
            if (detailsHistory != null)
            {
                if (detailsHistory.Auditor == null)
                {
                    detailsHistory.Auditor = operater;
                }

                if (auditTime != null)
                {
                    detailsHistory.AuditTime = auditTime;
                }

                detailsHistory.AuditComments = auditComments;
                detailsHistory.Description = description;

            }

            CurrentDb.SaveChanges();
        }

        public List<BizProcessesAuditDetails> GetDetails(Enumeration.BizProcessesAuditType type, int referenceid)
        {
            List<BizProcessesAuditDetails> auditDetails = new List<BizProcessesAuditDetails>();
            var bizProcessesAudits = CurrentDb.BizProcessesAudit.Where(m => m.AduitType == type && m.AduitReferenceId == referenceid).OrderBy(m => m.CreateTime).ToList();
            if (bizProcessesAudits != null)
            {
                foreach (var bizProcessesAudit in bizProcessesAudits)
                {
                    var bizProcessesAuditDetails = CurrentDb.BizProcessesAuditDetails.Where(m => m.BizProcessesAuditId == bizProcessesAudit.Id).OrderBy(m => m.CreateTime).ToList();
                    if (bizProcessesAuditDetails != null)
                    {
                        foreach (var bizProcessesAuditDetail in bizProcessesAuditDetails)
                        {
                            auditDetails.Add(bizProcessesAuditDetail);
                        }
                    }
                }
            }

            return auditDetails;

        }

        public BizProcessesAudit ChangeMerchantAuditStatus(int operater, int bizProcessesAuditId, Enumeration.MerchantAuditStatus changestatus, DateTime? endTime = null)
        {

            var bizProcessesAudit = CurrentDb.BizProcessesAudit.Where(m => m.Id == bizProcessesAuditId).FirstOrDefault();
            if (bizProcessesAudit != null)
            {
                if (bizProcessesAudit.EndTime == null)
                {

                    Enumeration.MerchantAuditStatus old_Status = (Enumeration.MerchantAuditStatus)bizProcessesAudit.Status;
                    bizProcessesAudit.Mender = operater;
                    bizProcessesAudit.LastUpdateTime = DateTime.Now;


                    if (endTime != null)
                    {
                        bizProcessesAudit.EndTime = endTime.Value;
                    }

                    if (changestatus == Enumeration.MerchantAuditStatus.InPrimaryAudit)
                    {
                        bizProcessesAudit.Status = (int)Enumeration.MerchantAuditStatus.InPrimaryAudit;

                        if (bizProcessesAudit.Auditor == null)
                        {
                            bizProcessesAudit.Auditor = operater;

                            ChangeAuditDetails(Enumeration.OperateType.Save, Enumeration.MerchantAuditStep.PrimaryAudit, bizProcessesAudit.Id, operater, null, null, null);

                            var merchant = CurrentDb.Merchant.Where(m => m.Id == bizProcessesAudit.AduitReferenceId).FirstOrDefault();
                            if (merchant != null)
                            {
                                merchant.Status = Enumeration.MerchantStatus.InFill;
                                CurrentDb.SaveChanges();
                            }
                        }
                    }
                    else if (changestatus == Enumeration.MerchantAuditStatus.WaitSeniorAudit)
                    {

                        var bizProcessesAuditDetails = CurrentDb.BizProcessesAuditDetails.Where(m => m.BizProcessesAuditId == bizProcessesAudit.Id && m.AuditStep == (int)Enumeration.MerchantAuditStep.SeniorAudit).OrderByDescending(m => m.CreateTime).Take(1).FirstOrDefault();
                        if (bizProcessesAuditDetails == null)
                        {
                            bizProcessesAudit.Status = (int)Enumeration.MerchantAuditStatus.WaitSeniorAudit;
                            bizProcessesAudit.Auditor = null;

                        }
                        else
                        {
                            bizProcessesAudit.Status = (int)Enumeration.MerchantAuditStatus.InSeniorAudit;
                            bizProcessesAudit.Auditor = bizProcessesAuditDetails.Auditor;

                            ChangeAuditDetails(Enumeration.OperateType.Save, Enumeration.MerchantAuditStep.SeniorAudit, bizProcessesAudit.Id, operater, null, null);
                        }
                    }
                    else if (changestatus == Enumeration.MerchantAuditStatus.InSeniorAudit)
                    {
                        bizProcessesAudit.Status = (int)Enumeration.MerchantAuditStatus.InSeniorAudit;
                        if (bizProcessesAudit.Auditor == null)
                        {
                            bizProcessesAudit.Auditor = operater;
                            ChangeAuditDetails(Enumeration.OperateType.Save, Enumeration.MerchantAuditStep.SeniorAudit, bizProcessesAudit.Id, operater, null, null);
                        }
                    }
                    else if (changestatus == Enumeration.MerchantAuditStatus.SeniorAuditReject)
                    {
                        var bizProcessesAuditDetails = CurrentDb.BizProcessesAuditDetails.Where(m => m.BizProcessesAuditId == bizProcessesAudit.Id && m.AuditStep == (int)Enumeration.MerchantAuditStep.PrimaryAudit).OrderByDescending(m => m.CreateTime).Take(1).FirstOrDefault();
                        if (bizProcessesAuditDetails != null)
                        {
                            bizProcessesAudit.Status = (int)Enumeration.MerchantAuditStatus.InPrimaryAudit;
                            bizProcessesAudit.Auditor = bizProcessesAuditDetails.Auditor;

                            ChangeAuditDetails(Enumeration.OperateType.Save, Enumeration.MerchantAuditStep.PrimaryAudit, bizProcessesAudit.Id, bizProcessesAuditDetails.Auditor.Value, null, null);
                        }
                    }
                    else if (changestatus == Enumeration.MerchantAuditStatus.SeniorAuditPass)
                    {
                        bizProcessesAudit.Status = (int)Enumeration.MerchantAuditStatus.SeniorAuditPass;
                        bizProcessesAudit.Auditor = operater;
                    }



                    CurrentDb.SaveChanges();
                }

                var historicalDetails = CurrentDb.BizProcessesAuditDetails.Where(m => m.BizProcessesAuditId == bizProcessesAudit.Id).OrderByDescending(m => m.AuditTime).ToList();

                bizProcessesAudit.HistoricalDetails = historicalDetails.Where(m => m.AuditTime != null).ToList();


                Enumeration.MerchantAuditStep merchantAuditStep = Enumeration.MerchantAuditStep.Unknow;
                if (changestatus == Enumeration.MerchantAuditStatus.WaitPrimaryAudit || changestatus == Enumeration.MerchantAuditStatus.InPrimaryAudit)
                {
                    merchantAuditStep = Enumeration.MerchantAuditStep.PrimaryAudit;
                }
                else if (changestatus == Enumeration.MerchantAuditStatus.WaitSeniorAudit || changestatus == Enumeration.MerchantAuditStatus.InSeniorAudit)
                {
                    merchantAuditStep = Enumeration.MerchantAuditStep.SeniorAudit;
                }

                var currentDetails = historicalDetails.Where(m => m.BizProcessesAuditId == bizProcessesAudit.Id && m.AuditStep == (int)merchantAuditStep).OrderByDescending(m => m.CreateTime).Take(1).FirstOrDefault();
                if (currentDetails != null)
                {
                    bizProcessesAudit.CurrentDetails = currentDetails;

                    var auditComments = historicalDetails.Where(m => m.BizProcessesAuditId == bizProcessesAudit.Id && m.AuditStep == (int)merchantAuditStep && m.AuditComments != null).OrderByDescending(m => m.CreateTime).Take(1).FirstOrDefault();
                    if (auditComments != null)
                    {
                        bizProcessesAudit.CurrentDetails.AuditComments = auditComments.AuditComments;
                    }
                }

            }

            return bizProcessesAudit;
        }

        public BizProcessesAudit ChangeCarInsureOfferDealtStatus(int operater, int bizProcessesAuditId, Enumeration.CarInsureOfferDealtStatus changestatus, string description = null, DateTime? endTime = null)
        {

            var bizProcessesAudit = CurrentDb.BizProcessesAudit.Where(m => m.Id == bizProcessesAuditId).FirstOrDefault();
            if (bizProcessesAudit != null)
            {
                if (bizProcessesAudit.EndTime == null)
                {
                    if (
                       bizProcessesAudit.Status != (int)Enumeration.CarInsureOfferDealtStatus.Complete
                        && bizProcessesAudit.Status != (int)Enumeration.CarInsureOfferDealtStatus.ClientCancle
                           && bizProcessesAudit.Status != (int)Enumeration.CarInsureOfferDealtStatus.StaffCancle
                        )
                    {
                        Enumeration.CarInsureOfferDealtStatus old_Status = (Enumeration.CarInsureOfferDealtStatus)bizProcessesAudit.Status;
                        bizProcessesAudit.Mender = operater;
                        bizProcessesAudit.LastUpdateTime = DateTime.Now;


                        if (endTime != null)
                        {
                            bizProcessesAudit.EndTime = endTime.Value;
                        }

                        if (changestatus == Enumeration.CarInsureOfferDealtStatus.WaitOffer)
                        {

                            var bizProcessesAuditDetails = CurrentDb.BizProcessesAuditDetails.Where(m => m.BizProcessesAuditId == bizProcessesAudit.Id && m.AuditStep == (int)Enumeration.CarInsureOfferDealtStep.Offer).OrderByDescending(m => m.CreateTime).Take(1).FirstOrDefault();
                            if (bizProcessesAuditDetails == null)
                            {
                                bizProcessesAudit.Status = (int)Enumeration.CarInsureOfferDealtStatus.WaitOffer;
                                bizProcessesAudit.Auditor = null;

                            }
                            else
                            {
                                bizProcessesAudit.Status = (int)Enumeration.CarInsureOfferDealtStatus.InOffer;
                                bizProcessesAudit.Auditor = bizProcessesAuditDetails.Auditor;

                                ChangeAuditDetails(Enumeration.OperateType.Save, Enumeration.CarInsureOfferDealtStep.Offer, bizProcessesAudit.Id, bizProcessesAuditDetails.Auditor.Value, null, "后台人员正在处理");
                            }

                        }
                        else if (changestatus == Enumeration.CarInsureOfferDealtStatus.InOffer)
                        {
                            bizProcessesAudit.Status = (int)Enumeration.CarInsureOfferDealtStatus.InOffer;
                            if (bizProcessesAudit.Auditor == null)
                            {
                                bizProcessesAudit.Auditor = operater;

                                ChangeAuditDetails(Enumeration.OperateType.Save, Enumeration.CarInsureOfferDealtStep.Offer, bizProcessesAudit.Id, operater, null, description);

                            }

                        }
                        else if (changestatus == Enumeration.CarInsureOfferDealtStatus.ClientFllow)
                        {

                            var bizProcessesAuditDetails = CurrentDb.BizProcessesAuditDetails.Where(m => m.BizProcessesAuditId == bizProcessesAudit.Id && (m.AuditStep == (int)Enumeration.CarInsureOfferDealtStep.Submit || m.AuditStep == (int)Enumeration.CarInsureOfferDealtStep.Fllow)).OrderByDescending(m => m.CreateTime).Take(1).FirstOrDefault();
                            if (bizProcessesAuditDetails != null)
                            {
                                bizProcessesAudit.Status = (int)Enumeration.CarInsureOfferDealtStatus.ClientFllow;
                                bizProcessesAudit.Auditor = bizProcessesAuditDetails.Auditor;

                                ChangeAuditDetails(Enumeration.OperateType.Save, Enumeration.CarInsureOfferDealtStep.Fllow, bizProcessesAudit.Id, bizProcessesAuditDetails.Auditor.Value, null, description);
                            }



                        }
                        else if (changestatus == Enumeration.CarInsureOfferDealtStatus.ClientCancle)
                        {
                            bizProcessesAudit.Status = (int)Enumeration.CarInsureOfferDealtStatus.ClientCancle;
                            bizProcessesAudit.Auditor = operater;
                            bizProcessesAudit.EndTime = this.DateTime;

                            ChangeAuditDetails(Enumeration.OperateType.Cancle, Enumeration.CarInsureOfferDealtStep.Offer, bizProcessesAudit.Id, bizProcessesAudit.Auditor.Value, null, description);

                        }
                        else if (changestatus == Enumeration.CarInsureOfferDealtStatus.StaffCancle)
                        {
                            bizProcessesAudit.Status = (int)Enumeration.CarInsureOfferDealtStatus.StaffCancle;
                            bizProcessesAudit.Auditor = operater;
                            bizProcessesAudit.EndTime = this.DateTime;

                            ChangeAuditDetails(Enumeration.OperateType.Cancle, Enumeration.CarInsureOfferDealtStep.Offer, bizProcessesAudit.Id, bizProcessesAudit.Auditor.Value, null, description);
                        }
                        else if (changestatus == Enumeration.CarInsureOfferDealtStatus.OfferComplete)
                        {
                            bizProcessesAudit.Status = (int)Enumeration.CarInsureOfferDealtStatus.Complete;
                            bizProcessesAudit.Auditor = operater;


                            ChangeAuditDetails(Enumeration.OperateType.Submit, Enumeration.CarInsureOfferDealtStep.Offer, bizProcessesAudit.Id, bizProcessesAudit.Auditor.Value, null, description);
                        }
                        else if (changestatus == Enumeration.CarInsureOfferDealtStatus.Complete)
                        {
                            bizProcessesAudit.Status = (int)Enumeration.CarInsureOfferDealtStatus.Complete;
                            bizProcessesAudit.Auditor = operater;
                            bizProcessesAudit.EndTime = this.DateTime;

                            ChangeAuditDetails(Enumeration.OperateType.Submit, Enumeration.CarInsureOfferDealtStep.Complete, bizProcessesAudit.Id, bizProcessesAudit.Auditor.Value, null, description);
                        }
                    }

                    CurrentDb.SaveChanges();
                }

                var historicalDetails = CurrentDb.BizProcessesAuditDetails.Where(m => m.BizProcessesAuditId == bizProcessesAudit.Id).OrderByDescending(m => m.AuditTime).ToList();

                bizProcessesAudit.HistoricalDetails = historicalDetails.Where(m => m.AuditTime != null).ToList();


                Enumeration.CarInsureOfferDealtStep merchantAuditStep = Enumeration.CarInsureOfferDealtStep.Unknow;
                if (changestatus == Enumeration.CarInsureOfferDealtStatus.WaitOffer || changestatus == Enumeration.CarInsureOfferDealtStatus.InOffer)
                {
                    merchantAuditStep = Enumeration.CarInsureOfferDealtStep.Offer;
                }

                var currentDetails = historicalDetails.Where(m => m.BizProcessesAuditId == bizProcessesAudit.Id && m.AuditStep == (int)merchantAuditStep).OrderByDescending(m => m.CreateTime).Take(1).FirstOrDefault();
                if (currentDetails != null)
                {
                    bizProcessesAudit.CurrentDetails = currentDetails;

                    var auditComments = historicalDetails.Where(m => m.BizProcessesAuditId == bizProcessesAudit.Id && m.AuditStep == (int)merchantAuditStep && m.AuditComments != null).OrderByDescending(m => m.CreateTime).Take(1).FirstOrDefault();
                    if (auditComments != null)
                    {
                        bizProcessesAudit.CurrentDetails.AuditComments = auditComments.AuditComments;
                    }
                }


            }

            return bizProcessesAudit;
        }

        public BizProcessesAudit ChangeCarClaimDealtStatus(int operater, int bizProcessesAuditId, Enumeration.CarClaimDealtStatus changestatus, string description = null, DateTime? endTime = null)
        {

            var bizProcessesAudit = CurrentDb.BizProcessesAudit.Where(m => m.Id == bizProcessesAuditId).FirstOrDefault();
            if (bizProcessesAudit != null)
            {
                if (bizProcessesAudit.EndTime == null)
                {
                    if (
                       bizProcessesAudit.Status != (int)Enumeration.CarClaimDealtStatus.Complete
                        && bizProcessesAudit.Status != (int)Enumeration.CarClaimDealtStatus.ClientCancle
                           && bizProcessesAudit.Status != (int)Enumeration.CarClaimDealtStatus.StaffCancle
                        )
                    {
                        Enumeration.CarClaimDealtStatus old_Status = (Enumeration.CarClaimDealtStatus)bizProcessesAudit.Status;
                        bizProcessesAudit.Mender = operater;
                        bizProcessesAudit.LastUpdateTime = DateTime.Now;


                        if (endTime != null)
                        {
                            bizProcessesAudit.EndTime = endTime.Value;
                        }

                        if (changestatus == Enumeration.CarClaimDealtStatus.WaitVerifyOrder)
                        {

                            var bizProcessesAuditDetails = CurrentDb.BizProcessesAuditDetails.Where(m => m.BizProcessesAuditId == bizProcessesAudit.Id && m.AuditStep == (int)Enumeration.CarInsureOfferDealtStep.Offer).OrderByDescending(m => m.CreateTime).Take(1).FirstOrDefault();
                            if (bizProcessesAuditDetails == null)
                            {
                                bizProcessesAudit.Status = (int)Enumeration.CarClaimDealtStatus.WaitVerifyOrder;
                                bizProcessesAudit.Auditor = null;

                            }
                            else
                            {
                                bizProcessesAudit.Status = (int)Enumeration.CarClaimDealtStatus.InVerifyOrder;
                                bizProcessesAudit.Auditor = bizProcessesAuditDetails.Auditor;

                                ChangeAuditDetails(Enumeration.OperateType.Save, Enumeration.CarClaimDealtStep.VerifyOrder, bizProcessesAudit.Id, operater, null, null);
                            }

                        }
                        else if (changestatus == Enumeration.CarClaimDealtStatus.InVerifyOrder)
                        {
                            bizProcessesAudit.Status = (int)Enumeration.CarClaimDealtStatus.InVerifyOrder;
                            if (bizProcessesAudit.Auditor == null)
                            {
                                bizProcessesAudit.Auditor = operater;

                                ChangeAuditDetails(Enumeration.OperateType.Save, Enumeration.CarClaimDealtStep.VerifyOrder, bizProcessesAudit.Id, operater, null, null);

                            }

                        }
                        else if (changestatus == Enumeration.CarClaimDealtStatus.FllowUploadEstimateListImg)
                        {
                            var bizProcessesAuditDetails = CurrentDb.BizProcessesAuditDetails.Where(m => m.BizProcessesAuditId == bizProcessesAudit.Id && (m.AuditStep == (int)Enumeration.CarClaimDealtStep.Submit || m.AuditStep == (int)Enumeration.CarClaimDealtStep.UploadEstimateListImg)).OrderByDescending(m => m.CreateTime).Take(1).FirstOrDefault();
                            if (bizProcessesAuditDetails != null)
                            {
                                bizProcessesAudit.Status = (int)Enumeration.CarClaimDealtStatus.FllowUploadEstimateListImg;
                                bizProcessesAudit.Auditor = bizProcessesAuditDetails.Auditor;

                                ChangeAuditDetails(Enumeration.OperateType.Save, Enumeration.CarClaimDealtStep.UploadEstimateListImg, bizProcessesAudit.Id, bizProcessesAuditDetails.Auditor.Value, null, description);
                            }

                        }
                        else if (changestatus == Enumeration.CarClaimDealtStatus.WaitVerifyAmount)
                        {
                            bizProcessesAudit.Status = (int)Enumeration.CarClaimDealtStatus.WaitVerifyAmount;
                            bizProcessesAudit.Auditor = null;

                        }
                        else if (changestatus == Enumeration.CarClaimDealtStatus.InVerifyAmount)
                        {
                            bizProcessesAudit.Status = (int)Enumeration.CarClaimDealtStatus.InVerifyAmount;
                            if (bizProcessesAudit.Auditor == null)
                            {
                                bizProcessesAudit.Auditor = operater;

                                ChangeAuditDetails(Enumeration.OperateType.Save, Enumeration.CarClaimDealtStep.VerifyAmount, bizProcessesAudit.Id, operater, null, null);

                            }

                        }
                        else if (changestatus == Enumeration.CarClaimDealtStatus.ClientCancle)
                        {
                            bizProcessesAudit.Status = (int)Enumeration.CarClaimDealtStatus.ClientCancle;
                            bizProcessesAudit.Auditor = operater;

                            ChangeAuditDetails(Enumeration.OperateType.Cancle, Enumeration.CarClaimDealtStep.VerifyOrder, bizProcessesAudit.Id, bizProcessesAudit.Auditor.Value, null, null);

                        }
                        else if (changestatus == Enumeration.CarClaimDealtStatus.StaffCancle)
                        {
                            bizProcessesAudit.Status = (int)Enumeration.CarClaimDealtStatus.StaffCancle;
                            bizProcessesAudit.Auditor = operater;
                            bizProcessesAudit.EndTime = this.DateTime;

                            ChangeAuditDetails(Enumeration.OperateType.Cancle, Enumeration.CarClaimDealtStep.VerifyOrder, bizProcessesAudit.Id, bizProcessesAudit.Auditor.Value, null, null);
                        }
                        else if (changestatus == Enumeration.CarClaimDealtStatus.Complete)
                        {
                            bizProcessesAudit.Status = (int)Enumeration.CarClaimDealtStatus.Complete;
                            bizProcessesAudit.Auditor = operater;
                            bizProcessesAudit.EndTime = this.DateTime;

                            ChangeAuditDetails(Enumeration.OperateType.Submit, Enumeration.CarClaimDealtStep.VerifyOrder, bizProcessesAudit.Id, bizProcessesAudit.Auditor.Value, null, null);

                        }
                    }

                    CurrentDb.SaveChanges();
                }


                var historicalDetails = CurrentDb.BizProcessesAuditDetails.Where(m => m.BizProcessesAuditId == bizProcessesAudit.Id).OrderByDescending(m => m.AuditTime).ToList();

                bizProcessesAudit.HistoricalDetails = historicalDetails.Where(m => m.AuditTime != null).ToList();


                Enumeration.CarClaimDealtStep merchantAuditStep = Enumeration.CarClaimDealtStep.Unknow;
                if (changestatus == Enumeration.CarClaimDealtStatus.WaitVerifyOrder || changestatus == Enumeration.CarClaimDealtStatus.InVerifyOrder)
                {
                    merchantAuditStep = Enumeration.CarClaimDealtStep.VerifyOrder;
                }
                else if (changestatus == Enumeration.CarClaimDealtStatus.WaitVerifyAmount || changestatus == Enumeration.CarClaimDealtStatus.InVerifyAmount)
                {
                    merchantAuditStep = Enumeration.CarClaimDealtStep.VerifyAmount;
                }
                var currentDetails = historicalDetails.Where(m => m.BizProcessesAuditId == bizProcessesAudit.Id && m.AuditStep == (int)merchantAuditStep).OrderByDescending(m => m.CreateTime).Take(1).FirstOrDefault();
                if (currentDetails != null)
                {
                    bizProcessesAudit.CurrentDetails = currentDetails;

                    var auditComments = historicalDetails.Where(m => m.BizProcessesAuditId == bizProcessesAudit.Id && m.AuditStep == (int)merchantAuditStep && m.AuditComments != null).OrderByDescending(m => m.CreateTime).Take(1).FirstOrDefault();
                    if (auditComments != null)
                    {
                        bizProcessesAudit.CurrentDetails.AuditComments = auditComments.AuditComments;
                    }
                }


            }

            return bizProcessesAudit;
        }


        public BizProcessesAudit ChangeLllegalDealStatus(int operater, int bizProcessesAuditId, Enumeration.LllegalDealtStatus changestatus, string description = null, DateTime? endTime = null)
        {

            var bizProcessesAudit = CurrentDb.BizProcessesAudit.Where(m => m.Id == bizProcessesAuditId).FirstOrDefault();
            if (bizProcessesAudit != null)
            {
                if (bizProcessesAudit.EndTime == null)
                {
                    if (
                       bizProcessesAudit.Status != (int)Enumeration.LllegalDealtStatus.Complete
                        && bizProcessesAudit.Status != (int)Enumeration.LllegalDealtStatus.ClientCancle
                           && bizProcessesAudit.Status != (int)Enumeration.LllegalDealtStatus.StaffCancle
                        )
                    {
                        Enumeration.LllegalDealtStatus old_Status = (Enumeration.LllegalDealtStatus)bizProcessesAudit.Status;
                        bizProcessesAudit.Mender = operater;
                        bizProcessesAudit.LastUpdateTime = DateTime.Now;


                        if (endTime != null)
                        {
                            bizProcessesAudit.EndTime = endTime.Value;
                        }

                        if (changestatus == Enumeration.LllegalDealtStatus.WaitDealt)
                        {

                            var bizProcessesAuditDetails = CurrentDb.BizProcessesAuditDetails.Where(m => m.BizProcessesAuditId == bizProcessesAudit.Id && m.AuditStep == (int)Enumeration.LllegalDealtStep.Dealt).OrderByDescending(m => m.CreateTime).Take(1).FirstOrDefault();
                            if (bizProcessesAuditDetails == null)
                            {
                                bizProcessesAudit.Status = (int)Enumeration.LllegalDealtStatus.WaitDealt;
                                bizProcessesAudit.Auditor = null;

                            }
                            else
                            {
                                bizProcessesAudit.Status = (int)Enumeration.LllegalDealtStatus.InDealt;
                                bizProcessesAudit.Auditor = bizProcessesAuditDetails.Auditor;

                                ChangeAuditDetails(Enumeration.OperateType.Save, Enumeration.LllegalDealtStep.Dealt, bizProcessesAudit.Id, operater, null, description);
                            }

                        }
                        else if (changestatus == Enumeration.LllegalDealtStatus.InDealt)
                        {
                            bizProcessesAudit.Status = (int)Enumeration.LllegalDealtStatus.InDealt;
                            if (bizProcessesAudit.Auditor == null)
                            {
                                bizProcessesAudit.Auditor = operater;

                                ChangeAuditDetails(Enumeration.OperateType.Save, Enumeration.LllegalDealtStep.Dealt, bizProcessesAudit.Id, operater, null, description);

                            }

                        }

                        else if (changestatus == Enumeration.LllegalDealtStatus.ClientCancle)
                        {
                            bizProcessesAudit.Status = (int)Enumeration.LllegalDealtStatus.ClientCancle;
                            bizProcessesAudit.Auditor = operater;

                            ChangeAuditDetails(Enumeration.OperateType.Cancle, Enumeration.LllegalDealtStep.Complete, bizProcessesAudit.Id, bizProcessesAudit.Auditor.Value, null, description);

                        }
                        else if (changestatus == Enumeration.LllegalDealtStatus.StaffCancle)
                        {
                            bizProcessesAudit.Status = (int)Enumeration.LllegalDealtStatus.StaffCancle;
                            bizProcessesAudit.Auditor = operater;
                            bizProcessesAudit.EndTime = this.DateTime;

                            ChangeAuditDetails(Enumeration.OperateType.Cancle, Enumeration.LllegalDealtStep.Complete, bizProcessesAudit.Id, bizProcessesAudit.Auditor.Value, null, description);
                        }
                        else if (changestatus == Enumeration.LllegalDealtStatus.Complete)
                        {
                            bizProcessesAudit.Status = (int)Enumeration.LllegalDealtStatus.Complete;
                            bizProcessesAudit.Auditor = operater;
                            bizProcessesAudit.EndTime = this.DateTime;

                            ChangeAuditDetails(Enumeration.OperateType.Submit, Enumeration.LllegalDealtStep.Complete, bizProcessesAudit.Id, bizProcessesAudit.Auditor.Value, null, description);

                        }
                    }

                    CurrentDb.SaveChanges();
                }


                var historicalDetails = CurrentDb.BizProcessesAuditDetails.Where(m => m.BizProcessesAuditId == bizProcessesAudit.Id).OrderByDescending(m => m.AuditTime).ToList();

                bizProcessesAudit.HistoricalDetails = historicalDetails.Where(m => m.AuditTime != null).ToList();


                Enumeration.LllegalDealtStep merchantAuditStep = Enumeration.LllegalDealtStep.Unknow;
                if (changestatus == Enumeration.LllegalDealtStatus.WaitDealt || changestatus == Enumeration.LllegalDealtStatus.InDealt)
                {
                    merchantAuditStep = Enumeration.LllegalDealtStep.Dealt;
                }
                var currentDetails = historicalDetails.Where(m => m.BizProcessesAuditId == bizProcessesAudit.Id && m.AuditStep == (int)merchantAuditStep).OrderByDescending(m => m.CreateTime).Take(1).FirstOrDefault();
                if (currentDetails != null)
                {
                    bizProcessesAudit.CurrentDetails = currentDetails;

                    var auditComments = historicalDetails.Where(m => m.BizProcessesAuditId == bizProcessesAudit.Id && m.AuditStep == (int)merchantAuditStep && m.AuditComments != null).OrderByDescending(m => m.CreateTime).Take(1).FirstOrDefault();
                    if (auditComments != null)
                    {
                        bizProcessesAudit.CurrentDetails.AuditComments = auditComments.AuditComments;
                    }
                }


            }

            return bizProcessesAudit;
        }



        //public BizProcessesAudit ChangeStatus(int bizProcessesAuditId, object auditStatus, int auditor, string auditComments, string description = null)
        //{
        //    DateTime nowDate = DateTime.Now;
        //    var bizProcessesAudit = CurrentDb.BizProcessesAudit.Where(m => m.Id == bizProcessesAuditId).FirstOrDefault();
        //    if (bizProcessesAudit != null)
        //    {
        //        bizProcessesAudit.Mender = auditor;
        //        bizProcessesAudit.LastUpdateTime = nowDate;

        //        switch (bizProcessesAudit.AduitType)
        //        {
        //            case Enumeration.BizProcessesAuditType.TalentDemand:
        //                #region TalentDemand
        //                Enumeration.TalentDemandAuditStatus talentDemandDealtStatus = (Enumeration.TalentDemandAuditStatus)auditStatus;
        //                switch (talentDemandDealtStatus)
        //                {
        //                    case Enumeration.TalentDemandAuditStatus.Submit:
        //                        //提交订单将订单转为待核实状态，审核人空
        //                        ChangeAuditDetails(Enumeration.OperateType.Submit, Enumeration.TalentDemandDealtStep.Submit, bizProcessesAudit.Id, auditor, auditComments, description);
        //                        bizProcessesAudit.Status = (int)Enumeration.TalentDemandAuditStatus.WaitVerify;
        //                        bizProcessesAudit.Auditor = null;
        //                        break;
        //                    case Enumeration.TalentDemandAuditStatus.InVerify:
        //                        //提交订单将订单转为待核实中，判断当前审核人是否为空，若空设置
        //                        bizProcessesAudit.Status = (int)Enumeration.TalentDemandAuditStatus.InVerify;

        //                        if (bizProcessesAudit.Auditor == null)
        //                        {
        //                            ChangeAuditDetails(Enumeration.OperateType.Submit, Enumeration.TalentDemandDealtStep.InVerify, bizProcessesAudit.Id, auditor, auditComments, description);
        //                            bizProcessesAudit.Auditor = auditor;
        //                        }
        //                        break;
        //                    case Enumeration.TalentDemandAuditStatus.VerifyCorrect:
        //                        //订单核实正确将订单转为待处理，审核人空

        //                        ChangeAuditDetails(Enumeration.OperateType.Submit, Enumeration.TalentDemandDealtStep.VerifyedComplete, bizProcessesAudit.Id, auditor, auditComments, description);
        //                        var bizProcessesAuditDetailsDealted = CurrentDb.BizProcessesAuditDetails.Where(m => m.BizProcessesAuditId == bizProcessesAudit.Id && m.AuditStep == (int)Enumeration.TalentDemandDealtStep.DealtedReject).OrderByDescending(m => m.CreateTime).Take(1).FirstOrDefault();
        //                        if (bizProcessesAuditDetailsDealted == null)
        //                        {
        //                            bizProcessesAudit.Status = (int)Enumeration.TalentDemandAuditStatus.WaitDealt;
        //                            bizProcessesAudit.Auditor = null;
        //                            bizProcessesAudit.TempAuditComments = null;
        //                        }
        //                        else
        //                        {
        //                            bizProcessesAudit.Status = (int)Enumeration.TalentDemandAuditStatus.InDealt;
        //                            bizProcessesAudit.Auditor = bizProcessesAuditDetailsDealted.Auditor;
        //                        }
        //                        break;
        //                    case Enumeration.TalentDemandAuditStatus.VerifyIncorrect:
        //                        ChangeAuditDetails(Enumeration.OperateType.Submit, Enumeration.TalentDemandDealtStep.VerifyedComplete, bizProcessesAudit.Id, auditor, auditComments, description);
        //                        bizProcessesAudit.Status = (int)Enumeration.TalentDemandAuditStatus.VerifyIncorrect;
        //                        bizProcessesAudit.Auditor = auditor;
        //                        bizProcessesAudit.EndTime = this.DateTime;
        //                        bizProcessesAudit.TempAuditComments = null;
        //                        break;
        //                    case Enumeration.TalentDemandAuditStatus.InDealt:
        //                        //提交订单将订单转为待处理中，判断当前审核人是否为空，若空设置
        //                        bizProcessesAudit.Status = (int)Enumeration.TalentDemandAuditStatus.InDealt;
        //                        if (bizProcessesAudit.Auditor == null)
        //                        {
        //                            ChangeAuditDetails(Enumeration.OperateType.Submit, Enumeration.TalentDemandDealtStep.InDealt, bizProcessesAudit.Id, auditor, auditComments, description);
        //                            bizProcessesAudit.Auditor = auditor;
        //                        }
        //                        break;
        //                    case Enumeration.TalentDemandAuditStatus.DealtSuccess:
        //                        //订单核实正确将订单转为待处理，审核人空
        //                        ChangeAuditDetails(Enumeration.OperateType.Submit, Enumeration.TalentDemandDealtStep.DealtedComplete, bizProcessesAudit.Id, auditor, auditComments, description);
        //                        bizProcessesAudit.Status = (int)Enumeration.TalentDemandAuditStatus.DealtSuccess;
        //                        bizProcessesAudit.Auditor = auditor;
        //                        bizProcessesAudit.EndTime = this.DateTime;
        //                        bizProcessesAudit.TempAuditComments = null;
        //                        break;
        //                    case Enumeration.TalentDemandAuditStatus.DealtFailure:
        //                        ChangeAuditDetails(Enumeration.OperateType.Submit, Enumeration.TalentDemandDealtStep.DealtedComplete, bizProcessesAudit.Id, auditor, auditComments, description);
        //                        bizProcessesAudit.Status = (int)Enumeration.TalentDemandAuditStatus.DealtFailure;
        //                        bizProcessesAudit.Auditor = auditor;
        //                        bizProcessesAudit.EndTime = this.DateTime;
        //                        bizProcessesAudit.TempAuditComments = null;
        //                        break;
        //                    case Enumeration.TalentDemandAuditStatus.DealtReject:
        //                        var bizProcessesAuditDetailsVerifyed = CurrentDb.BizProcessesAuditDetails.Where(m => m.BizProcessesAuditId == bizProcessesAudit.Id && m.AuditStep == (int)Enumeration.TalentDemandDealtStep.VerifyedComplete).OrderByDescending(m => m.CreateTime).Take(1).FirstOrDefault();
        //                        if (bizProcessesAuditDetailsVerifyed != null)
        //                        {
        //                            bizProcessesAudit.Status = (int)Enumeration.TalentDemandAuditStatus.InVerify;
        //                            bizProcessesAudit.Auditor = bizProcessesAuditDetailsVerifyed.Auditor;
        //                            bizProcessesAudit.TempAuditComments = null;
        //                            ChangeAuditDetails(Enumeration.OperateType.Submit, Enumeration.TalentDemandDealtStep.DealtedReject, bizProcessesAudit.Id, auditor, auditComments, description);
        //                        }
        //                        break;
        //                }
        //                #endregion
        //                break;
        //        }

        //        CurrentDb.SaveChanges();
        //        var historicalDetails = CurrentDb.BizProcessesAuditDetails.Where(m => m.BizProcessesAuditId == bizProcessesAudit.Id).OrderBy(m => m.CreateTime).ToList();
        //        bizProcessesAudit.HistoricalDetails = historicalDetails.Where(m => m.AuditTime != null).ToList();
        //    }

        //    return bizProcessesAudit;
        //}


        public BizProcessesAudit ChangeStatusByAuditFlowV1(int bizProcessesAuditId, Enumeration.AuditFlowV1Status auditStatus, int auditor, string auditComments, string description = null)
        {
            DateTime nowDate = DateTime.Now;
            var bizProcessesAudit = CurrentDb.BizProcessesAudit.Where(m => m.Id == bizProcessesAuditId).FirstOrDefault();
            if (bizProcessesAudit != null)
            {
                bizProcessesAudit.Mender = auditor;
                bizProcessesAudit.LastUpdateTime = nowDate;


                #region TalentDemand
                switch (auditStatus)
                {
                    case Enumeration.AuditFlowV1Status.Submit:
                        //提交订单将订单转为待核实状态，审核人空
                        ChangeAuditDetails(Enumeration.OperateType.Submit, Enumeration.AuditFlowV1Step.Submit, bizProcessesAudit.Id, auditor, auditComments, description);
                        bizProcessesAudit.Status = (int)Enumeration.AuditFlowV1Status.WaitVerify;
                        bizProcessesAudit.Auditor = null;
                        break;
                    case Enumeration.AuditFlowV1Status.InVerify:
                        //提交订单将订单转为待核实中，判断当前审核人是否为空，若空设置
                        bizProcessesAudit.Status = (int)Enumeration.AuditFlowV1Status.InVerify;

                        if (bizProcessesAudit.Auditor == null)
                        {
                            ChangeAuditDetails(Enumeration.OperateType.Submit, Enumeration.AuditFlowV1Step.InVerify, bizProcessesAudit.Id, auditor, auditComments, description);
                            bizProcessesAudit.Auditor = auditor;
                        }
                        break;
                    case Enumeration.AuditFlowV1Status.VerifyCorrect:
                        //订单核实正确将订单转为待处理，审核人空

                        ChangeAuditDetails(Enumeration.OperateType.Submit, Enumeration.AuditFlowV1Step.VerifyedComplete, bizProcessesAudit.Id, auditor, auditComments, description);
                        var bizProcessesAuditDetailsDealted = CurrentDb.BizProcessesAuditDetails.Where(m => m.BizProcessesAuditId == bizProcessesAudit.Id && m.AuditStep == (int)Enumeration.AuditFlowV1Step.DealtedReject).OrderByDescending(m => m.CreateTime).Take(1).FirstOrDefault();
                        if (bizProcessesAuditDetailsDealted == null)
                        {
                            bizProcessesAudit.Status = (int)Enumeration.AuditFlowV1Status.WaitDealt;
                            bizProcessesAudit.Auditor = null;
                            bizProcessesAudit.TempAuditComments = null;
                        }
                        else
                        {
                            bizProcessesAudit.Status = (int)Enumeration.AuditFlowV1Status.InDealt;
                            bizProcessesAudit.Auditor = bizProcessesAuditDetailsDealted.Auditor;
                        }
                        break;
                    case Enumeration.AuditFlowV1Status.VerifyIncorrect:
                        ChangeAuditDetails(Enumeration.OperateType.Submit, Enumeration.AuditFlowV1Step.VerifyedComplete, bizProcessesAudit.Id, auditor, auditComments, description);
                        bizProcessesAudit.Status = (int)Enumeration.AuditFlowV1Status.VerifyIncorrect;
                        bizProcessesAudit.Auditor = auditor;
                        bizProcessesAudit.EndTime = this.DateTime;
                        bizProcessesAudit.TempAuditComments = null;
                        break;
                    case Enumeration.AuditFlowV1Status.InDealt:
                        //提交订单将订单转为待处理中，判断当前审核人是否为空，若空设置
                        bizProcessesAudit.Status = (int)Enumeration.AuditFlowV1Status.InDealt;
                        if (bizProcessesAudit.Auditor == null)
                        {
                            ChangeAuditDetails(Enumeration.OperateType.Submit, Enumeration.AuditFlowV1Step.InDealt, bizProcessesAudit.Id, auditor, auditComments, description);
                            bizProcessesAudit.Auditor = auditor;
                        }
                        break;
                    case Enumeration.AuditFlowV1Status.DealtSuccess:
                        //订单核实正确将订单转为待处理，审核人空
                        ChangeAuditDetails(Enumeration.OperateType.Submit, Enumeration.AuditFlowV1Step.DealtedComplete, bizProcessesAudit.Id, auditor, auditComments, description);
                        bizProcessesAudit.Status = (int)Enumeration.AuditFlowV1Status.DealtSuccess;
                        bizProcessesAudit.Auditor = auditor;
                        bizProcessesAudit.EndTime = this.DateTime;
                        bizProcessesAudit.TempAuditComments = null;
                        break;
                    case Enumeration.AuditFlowV1Status.DealtFailure:
                        ChangeAuditDetails(Enumeration.OperateType.Submit, Enumeration.AuditFlowV1Step.DealtedComplete, bizProcessesAudit.Id, auditor, auditComments, description);
                        bizProcessesAudit.Status = (int)Enumeration.AuditFlowV1Status.DealtFailure;
                        bizProcessesAudit.Auditor = auditor;
                        bizProcessesAudit.EndTime = this.DateTime;
                        bizProcessesAudit.TempAuditComments = null;
                        break;
                    case Enumeration.AuditFlowV1Status.DealtReject:
                        var bizProcessesAuditDetailsVerifyed = CurrentDb.BizProcessesAuditDetails.Where(m => m.BizProcessesAuditId == bizProcessesAudit.Id && m.AuditStep == (int)Enumeration.AuditFlowV1Step.VerifyedComplete).OrderByDescending(m => m.CreateTime).Take(1).FirstOrDefault();
                        if (bizProcessesAuditDetailsVerifyed != null)
                        {
                            bizProcessesAudit.Status = (int)Enumeration.AuditFlowV1Status.InVerify;
                            bizProcessesAudit.Auditor = bizProcessesAuditDetailsVerifyed.Auditor;
                            bizProcessesAudit.TempAuditComments = null;
                            ChangeAuditDetails(Enumeration.OperateType.Submit, Enumeration.AuditFlowV1Step.DealtedReject, bizProcessesAudit.Id, auditor, auditComments, description);
                        }
                        break;
                }
                #endregion


                CurrentDb.SaveChanges();
                var historicalDetails = CurrentDb.BizProcessesAuditDetails.Where(m => m.BizProcessesAuditId == bizProcessesAudit.Id).OrderBy(m => m.CreateTime).ToList();
                bizProcessesAudit.HistoricalDetails = historicalDetails.Where(m => m.AuditTime != null).ToList();
            }

            return bizProcessesAudit;
        }



        public BizProcessesAudit SaveTempAuditComments(int bizProcessesAuditId, int auditor, string auditComments)
        {
            DateTime nowDate = DateTime.Now;
            var bizProcessesAudit = CurrentDb.BizProcessesAudit.Where(m => m.Id == bizProcessesAuditId).FirstOrDefault();
            if (bizProcessesAudit != null)
            {
                bizProcessesAudit.TempAuditComments = auditComments;
                bizProcessesAudit.Mender = auditor;
                bizProcessesAudit.LastUpdateTime = nowDate;
                CurrentDb.SaveChanges();
            }

            return bizProcessesAudit;
        }

    }

}

