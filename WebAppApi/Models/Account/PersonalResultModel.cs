using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Account
{
    public class FundDetails
    {
        public decimal Balance { get; set; }
        public decimal Arrearage { get; set; }

    }

    public class OrderStatusCount
    {
        public int Follow { get; set; }

        public int WaitPay { get; set; }
    }

    public class RentDue
    {
        public string DueDate { get; set; }

        public decimal Rent { get; set; }
    }


    public class PersonalInfo
    {
        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string MerchantName { get; set; }

        public string MerchantAddress { get; set; }

    }

    public class PersonalResultModel
    {
        public PersonalResultModel()
        {
            this.PersonalInfo = new PersonalInfo();
        }

        public PersonalInfo PersonalInfo { get; set; }

        public FundDetails Fund { get; set; }

        public string WithdrawRuleUrl { get; set; }

        public OrderStatusCount OrderStatusCount { get; set; }

        public int RenewalCount { get; set; }

        public OrderDepositRentInfo RentDue { get; set; }

        public string CustomerPhone { get; set; }

        public Enumeration.MerchantPosMachineStatus PosMachineStatus { get; set; }

        public string TestSetttigPwd { get; set; }
    }
}