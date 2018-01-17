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
            this.FullName = sysClientUser.FullName;
            this.AccountType = sysClientUser.ClientAccountType;
            this.MerchantId = sysClientUser.MerchantId;
            this.MerchantCode = sysClientUser.ClientCode;
            this.IsTestAccount = sysClientUser.IsTestAccount;
            this.DeviceId = deviceId;


            var posMachine = CurrentDb.PosMachine.Where(m => m.DeviceId == deviceId).FirstOrDefault();


            if (posMachine == null)
            {
                this.PosMachineStatus = Enumeration.MerchantPosMachineStatus.NotMatch;
                return;
            }


            var merchant = CurrentDb.Merchant.Where(m => m.Id == sysClientUser.MerchantId).FirstOrDefault();


            if (merchant == null)
            {
                this.PosMachineStatus = Enumeration.MerchantPosMachineStatus.NotMatch;
                return;
            }

            var merchantPosMachine = CurrentDb.MerchantPosMachine.Where(m => m.PosMachineId == posMachine.Id && m.MerchantId == sysClientUser.MerchantId).FirstOrDefault();


            if (merchantPosMachine == null)
            {
                this.PosMachineStatus = Enumeration.MerchantPosMachineStatus.NotMatch;
                return;
            }


            this.PosMachineId = merchantPosMachine.Id;

            this.PosMachineStatus = merchantPosMachine.Status;

            if (merchantPosMachine.Status == Enumeration.MerchantPosMachineStatus.NoActive)
            {
                this.OrderInfo = BizFactory.Merchant.GetDepositRentOrder(merchant.Id, merchantPosMachine.Id);
            }
            else if (merchantPosMachine.Status == Enumeration.MerchantPosMachineStatus.Rentdue)
            {
                this.OrderInfo = BizFactory.Merchant.GetRentOrder(merchant.Id, merchantPosMachine.Id);

            }

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