﻿using Lumos.Entity;
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


            var orderToCarInsureOfferCompanys = CurrentDb.OrderToCarInsureOfferCompany.Where(m => m.OfferResult == Entity.Enumeration.OfferResult.WaitArtificialOffer).ToList();


            foreach (var item in orderToCarInsureOfferCompanys)
            {
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
                    item.TryGetApiOfferResultCount += 1;

                    if (item.TryGetApiOfferResultCount >= 5)
                    {
                        item.OfferResult = Enumeration.OfferResult.WaitStaffOffer;
                    }

                }

                CurrentDb.SaveChanges();
            }

            return result;

        }
    }
}