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

            var orderToCarInsures = CurrentDb.OrderToCarInsure.Where(m => m.FollowStatus == (int)Enumeration.OrderToCarInsureFollowStatus.WaitArtificialOffer || m.FollowStatus == (int)Enumeration.OrderToCarInsureFollowStatus.WaitArtificialInsure || m.FollowStatus == (int)Enumeration.OrderToCarInsureFollowStatus.WaitPay || m.PartnerPayId != null).ToList();


            LogUtil.Info("待处理的总数量：" + orderToCarInsures.Count);

            var waitArtificialOfferCount = orderToCarInsures.Where(m => m.FollowStatus == (int)Enumeration.OrderToCarInsureFollowStatus.WaitArtificialOffer).Count();
            var waitArtificialInsureCount = orderToCarInsures.Where(m => m.FollowStatus == (int)Enumeration.OrderToCarInsureFollowStatus.WaitArtificialInsure).Count();
            var waitAutoApplyPayCount = orderToCarInsures.Where(m => m.FollowStatus == (int)Enumeration.OrderToCarInsureFollowStatus.WaitPay).Count();
            LogUtil.Info("待处理的数量（等待人工报价）：" + waitArtificialOfferCount);
            LogUtil.Info("待处理的数量（等待人工核保）：" + waitArtificialInsureCount);
            LogUtil.Info("待处理的数量（等待人工申请支付）：" + waitAutoApplyPayCount);
            foreach (var item in orderToCarInsures)
            {
                var orderToCarInsureOfferCompany = CurrentDb.OrderToCarInsureOfferCompany.Where(m => m.OrderId == item.Id).FirstOrDefault();

                LogUtil.InfoFormat("处理订单号:{0}，跟进状态：{1}", item.Sn, item.FollowStatus);

                switch (item.FollowStatus)
                {
                    case (int)Enumeration.OrderToCarInsureFollowStatus.WaitArtificialOffer:
                        #region  获取人工报价结果
                        LogUtil.InfoFormat("处理订单号:{0}，查询人工报价结果", item.Sn);

                        if (string.IsNullOrEmpty(item.PartnerOrderId))
                        {
                            LogUtil.InfoFormat("处理订单号:{0}，易点通订单号为空", item.Sn);
                        }

                        if (string.IsNullOrEmpty(item.PartnerInquiryId))
                        {
                            LogUtil.InfoFormat("处理订单号:{0}，易点通询价号为空", item.Sn);
                        }

                        if (!string.IsNullOrEmpty(item.PartnerInquiryId) && !string.IsNullOrEmpty(item.PartnerOrderId))
                        {
                            var result_QueryInquiry = YdtUtils.QueryInquiry(item.PartnerOrderId, item.PartnerInquiryId);

                            var updateOrderOfferPms = new UpdateOrderOfferPms();

                            updateOrderOfferPms.Auto = 0;
                            updateOrderOfferPms.PartnerOrderId = item.PartnerOrderId;
                            updateOrderOfferPms.PartnerInquiryId = item.PartnerInquiryId;

                            if (result_QueryInquiry.Result == ResultType.Success)
                            {
                                LogUtil.InfoFormat("处理订单号:{0}，查询人工报价结果成功,返回报价数据", item.Sn);
                                var result_QueryInquiryData = result_QueryInquiry.Data;

                                updateOrderOfferPms.PartnerChannelId = result_QueryInquiryData.channel.channelId;
                                updateOrderOfferPms.PartnerCompanyId = result_QueryInquiryData.channel.code;
                                updateOrderOfferPms.BiStartDate = result_QueryInquiryData.inquiry.biStartDate;
                                updateOrderOfferPms.CiStartDate = result_QueryInquiryData.inquiry.ciStartDate;
                                updateOrderOfferPms.Coverages = result_QueryInquiryData.inquiry.coverages;
                                updateOrderOfferPms.Inquirys = result_QueryInquiryData.inquiry.inquirys;
                                updateOrderOfferPms.OfferResult = Enumeration.OfferResult.ArtificialOfferSuccess;//人工报价成功
                                BizFactory.InsCar.UpdateOfferByAfter(0, updateOrderOfferPms);
                            }
                            else
                            {
                                LogUtil.InfoFormat("处理订单号:{0}，查询人工报价结果失败,返回报价为空", item.Sn);
                                orderToCarInsureOfferCompany.TryGetApiOfferResultCount += 1;
                            }
                        }


                        #endregion
                        break;
                    case (int)Enumeration.OrderToCarInsureFollowStatus.WaitArtificialInsure:
                        #region 获取人工核保结果

                        var result_QueryInsurey = YdtUtils.QueryInsure(item.PartnerOrderId, item.PartnerInquiryId);

                        #endregion
                        break;
                    case (int)Enumeration.OrderToCarInsureFollowStatus.WaitPay:
                        #region 获取支付结果

                        var payQueryResult = YdtUtils.PayQuery(item.PartnerOrderId, item.PartnerInquiryId, item.PartnerInsureId, item.PartnerPayId);

                        if (payQueryResult != null)
                        {
                            if (payQueryResult.Data != null)
                            {
                                string resultText = Newtonsoft.Json.JsonConvert.SerializeObject(payQueryResult);
                                bool isPaySuccess = false;

                                if (payQueryResult.Data.result == 1)
                                {
                                    isPaySuccess = true;
                                }

                                BizFactory.Pay.ResultNotify(0, item.Sn, isPaySuccess, Enumeration.PayResultNotifyType.PartnerPayOrgOrderQueryApi, "易点通", resultText);
                            }
                        }


                        #endregion
                        break;
                }
            }

            CurrentDb.SaveChanges();
            return result;
        }
    }

}