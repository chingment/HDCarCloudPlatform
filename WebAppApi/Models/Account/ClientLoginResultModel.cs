using Lumos.BLL;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Account
{

    public class ClientLoginResultModel : BaseViewModel
    {

        public ClientLoginResultModel()
        {

        }

        public ClientLoginResultModel(SysClientUser sysClientUser, string deviceId)
        {
            DateTime nowDate = DateTime.Now;

            this.UserId = sysClientUser.Id;
            this.UserName = sysClientUser.UserName;
            this.MerchantId = sysClientUser.MerchantId;
            this.MerchantCode = sysClientUser.ClientCode;
            this.IsTestAccount = sysClientUser.IsTestAccount;
            this.PosMachineId = 0;


            var orderToServiceFee = CurrentDb.OrderToServiceFee.Where(m => m.UserId == sysClientUser.Id && m.Status == Enumeration.OrderStatus.WaitPay).FirstOrDefault();
            if (orderToServiceFee != null)
            {
                this.OrderInfo = BizFactory.Merchant.GetOrderConfirmInfoByServiceFee(orderToServiceFee.Sn);
            }
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public int MerchantId { get; set; }
        public string MerchantCode { get; set; }
        public bool IsTestAccount { get; set; }
        public int PosMachineId { get; set; }
        public Enumeration.MerchantPosMachineStatus PosMachineStatus { get; set; }

        public OrderConfirmInfo OrderInfo { get; set; }
    }

}