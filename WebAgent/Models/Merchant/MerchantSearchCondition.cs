using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAgent.Models.Biz.Merchant
{
    public class MerchantSearchCondition:SearchCondition
    {
        public Enumeration.MerchantAuditStatus AuditStatus { get; set; }

        public string YYZZ_RegisterNo { get; set; }

        public string YYZZ_Name { get; set; }

    }
}