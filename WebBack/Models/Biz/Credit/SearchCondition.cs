using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Credit
{
    public class SearchCondition : BaseSearchCondition
    {
        public Enumeration.AuditFlowV1Status AuditStatus { get; set; }

        public Enumeration.OrderStatus Status { get; set; }
    }
}