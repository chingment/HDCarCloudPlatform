using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Account
{
    public class AddChildAccountModel
    {
        public int UserId { get; set; }

        public int MerchantId { get; set; }

        public string AccountFullName { get; set; }

        public string AccountPhone { get; set; }

        public string AccountPassword { get; set; }

        public string Token { get; set; }

        public string ValidCode { get; set; }

    }
}