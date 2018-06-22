using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YdtSdk;

namespace Lumos.BLL.Biz.Task
{
    public class TaskForCarInsQueryInquiry : BaseProvider, ITask
    {
        public CustomJsonResult Run()
        {
            CustomJsonResult result = new CustomJsonResult();

            var orderToCarInsures = CurrentDb.OrderToCarInsure.Where(m => m.FollowStatus == (int)Enumeration.OrderToCarInsureFollowStatus.WaitArtificialOffer || m.FollowStatus == (int)Enumeration.OrderToCarInsureFollowStatus.WaitArtificialInsure || m.FollowStatus == (int)Enumeration.OrderToCarInsureFollowStatus.WaitAutoApplyPay).ToList();

            foreach (var item in orderToCarInsures)
            {
                var orderToCarInsureOfferCompany = CurrentDb.OrderToCarInsureOfferCompany.Where(m => m.OrderId == item.Id).FirstOrDefault();

                switch (item.FollowStatus)
                {
                    case (int)Enumeration.OrderToCarInsureFollowStatus.WaitArtificialOffer:
                        #region  获取人工报价结果

                        var result_QueryInquiry = YdtUtils.QueryInquiry(item.PartnerOrderId, item.PartnerInquiryId);

                        if (result_QueryInquiry.Result == ResultType.Success)
                        {
                            var result_QueryInquiryData = result_QueryInquiry.Data;

                            var updateOrderOfferPms = new UpdateOrderOfferPms();
                            updateOrderOfferPms.Auto = 0;
                            updateOrderOfferPms.PartnerOrderId = item.PartnerOrderId;
                            updateOrderOfferPms.PartnerInquirySeq = item.PartnerInquiryId;
                            updateOrderOfferPms.PartnerChannelId = result_QueryInquiryData.channel.channelId;
                            updateOrderOfferPms.PartnerCompanyId = result_QueryInquiryData.channel.code;
                            updateOrderOfferPms.BiStartDate = result_QueryInquiryData.inquiry.biStartDate;
                            updateOrderOfferPms.CiStartDate = result_QueryInquiryData.inquiry.ciStartDate;
                            updateOrderOfferPms.Coverages = result_QueryInquiryData.inquiry.coverages;
                            updateOrderOfferPms.OfferResult = Enumeration.OfferResult.ArtificialOfferSuccess;
                            updateOrderOfferPms.Inquirys = result_QueryInquiryData.inquiry.inquirys;

                            BizFactory.InsCar.UpdateOfferByAfter(0, updateOrderOfferPms);
                        }
                        else
                        {
                            orderToCarInsureOfferCompany.TryGetApiOfferResultCount += 1;

                            if (orderToCarInsureOfferCompany.TryGetApiOfferResultCount >= 5)
                            {
                                orderToCarInsureOfferCompany.OfferResult = Enumeration.OfferResult.WaitStaffOffer;
                                item.FollowStatus = (int)Enumeration.OrderToCarInsureFollowStatus.WaitStaffOffer;
                                BizFactory.BizProcessesAudit.ChangeCarInsureStatus(item.BizProcessesAuditId, Enumeration.CarInsureAuditStatus.Sumbit, 0, null, "由于接口报价失败，重试了5次，需人工报价");
                            }

                        }
                        #endregion
                        break;
                    case (int)Enumeration.OrderToCarInsureFollowStatus.WaitArtificialInsure:
                        #region 获取人工核保结果

                        //var result_QueryInquiry = YdtUtils.QueryInquiry(item.PartnerOrderId, item.PartnerInquiryId);

                        #endregion
                        break;
                    case (int)Enumeration.OrderToCarInsureFollowStatus.WaitAutoApplyPay:

                        #region 获取人工申请支付结果

                        #endregion
                        break;
                }

            }

            CurrentDb.SaveChanges();
            return result;
        }
    }

}