using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Merchant
{
    public class RepairMerchantSearchCondition: BaseSearchCondition
    {
        public int InsuranceCompanyId { get; set; }

        public int MerchantId { get; set; }
    }
}