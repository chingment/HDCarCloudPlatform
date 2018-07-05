using Lumos.DAL;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using YdtSdk;

namespace Lumos.BLL.Biz.Task
{
    public class TaskForMerchantTransactionsCheck : BaseProvider, ITask
    {
        public CustomJsonResult Run()
        {
            CustomJsonResult result = new CustomJsonResult();


            var orders = CurrentDb.Order.Where(m => m.Status == Enumeration.OrderStatus.WaitPay).ToList();

            foreach (var order in orders)
            {
                SdkFactory.StarPay.PayQuery(0, order);

                switch (order.Type)
                {
                    case Enumeration.OrderType.InsureForCarForInsure:


                        var orderToCarInsureOfferCompanys = CurrentDb.OrderToCarInsureOfferCompany.Where(m => m.OrderId == order.Id).ToList();

                        foreach (var item in orderToCarInsureOfferCompanys)
                        {
                            if (!string.IsNullOrEmpty(item.PartnerPayId))
                            {

                                YdtInscarPayQueryPms ydtInscarPayQueryPms = new YdtInscarPayQueryPms();

                                ydtInscarPayQueryPms.orderSeq = item.PartnerOrderId;
                                ydtInscarPayQueryPms.inquirySeq = item.PartnerInquiryId;
                                ydtInscarPayQueryPms.insureSeq = item.PartnerInsureId;
                                ydtInscarPayQueryPms.paySeq = item.PartnerPayId;
                                var payQueryResult = YdtUtils.PayQuery(ydtInscarPayQueryPms);

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


                                        BizFactory.Pay.ResultNotify(0, order.Sn, isPaySuccess, Enumeration.PayResultNotifyType.PartnerPayOrgOrderQueryApi, "易点通", resultText);
                                    }
                                }
                            }
                        }
                        break;
                    case Enumeration.OrderType.PosMachineServiceFee:
                    case Enumeration.OrderType.LllegalQueryRecharge:
                    case Enumeration.OrderType.LllegalDealt:
                    case Enumeration.OrderType.Goods:
                        SdkFactory.StarPay.PayQuery(0, order);
                        break;
                    default:
                        break;
                }

            }

            CurrentDb.SaveChanges();


            return result;
        }
    }
}
