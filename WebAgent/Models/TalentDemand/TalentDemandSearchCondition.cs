using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAgent.Models.TalentDemand
{
    public class TalentDemandSearchCondition : SearchCondition
    {
        public Enumeration.AuditFlowV1Status AuditStatu { get; set; }

        public Enumeration.OrderStatus Status { get; set; }
    }
}