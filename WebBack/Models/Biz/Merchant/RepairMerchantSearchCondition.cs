using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Merchant
{
    public class RepairMerchantSearchCondition:SearchCondition
    {
        public int InsuranceCompanyId { get; set; }

        public int MerchantId { get; set; }
    }
}