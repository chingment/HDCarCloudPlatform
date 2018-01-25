using Lumos.BLL;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Account
{
    public enum ClientLoginStatus
    {
        [Remark("未知")]
        Unknow = 0,
        [Remark("正常")]
        Normal = 1,
        [Remark("未激活")]
        NoActive = 2,
        [Remark("到期")]
        Expiry = 3
    }

    public class ClientLoginResultModel : BaseViewModel
    {

        public ClientLoginResultModel()
        {

        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public int MerchantId { get; set; }
        public string MerchantCode { get; set; }
        public bool IsTestAccount { get; set; }
        public int PosMachineId { get; set; }
        public ClientLoginStatus Status { get; set; }
        public OrderConfirmInfo OrderInfo { get; set; }
    }

}