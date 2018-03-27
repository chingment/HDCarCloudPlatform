using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Merchant
{
    public class SearchCondition: BaseSearchCondition
    {
        public Enumeration.MerchantAuditStatus AuditStatus { get; set; }

        public string YYZZ_RegisterNo { get; set; }

        public string YYZZ_Name { get; set; }

    }
}