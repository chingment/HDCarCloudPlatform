using Lumos.BLL;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Account
{
    public class SalesmanLoginResultModel : BaseViewModel
    {
        public SalesmanLoginResultModel()
        {

        }

        public SalesmanLoginResultModel(SysSalesmanUser sysClientUser, string deviceId, int merchantId)
        {
            DateTime nowDate = DateTime.Now;

            this.UserId = sysClientUser.Id;
            this.UserName = sysClientUser.UserName;
            this.FullName = sysClientUser.FullName;
            this.AccountType = Enumeration.ClientAccountType.MasterAccount;
            this.MerchantId = merchantId;
            this.MerchantCode = "88888888";
            this.IsTestAccount = true;
            this.DeviceId = deviceId;
            this.PosMachineStatus = Enumeration.MerchantPosMachineStatus.Normal;
            this.PosMachineId = 0;
        }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public bool IsTestAccount { get; set; }

        public string DeviceId { get; set; }

        public int PosMachineId { get; set; }

        public Enumeration.ClientAccountType AccountType { get; set; }

        public Enumeration.MerchantPosMachineStatus PosMachineStatus { get; set; }

        public int MerchantId { get; set; }

        public string MerchantCode { get; set; }

        public OrderDepositRentInfo OrderInfo { get; set; }
    }
}