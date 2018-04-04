using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.TalentDemand
{
    public class SearchCondition : BaseSearchCondition
    {
        public Enumeration.TalentDemandAuditStatus AuditStatus { get; set; }

        public Enumeration.OrderStatus Status { get; set; }
    }
}