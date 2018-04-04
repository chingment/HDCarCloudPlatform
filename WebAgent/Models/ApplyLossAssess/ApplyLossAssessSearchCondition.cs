using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAgent.Models.ApplyLossAssess
{
    public class ApplyLossAssessSearchCondition : SearchCondition
    {
        public Enumeration.AuditFlowV1Status AuditStatus { get; set; }

        public Enumeration.OrderStatus Status { get; set; }
    }
}