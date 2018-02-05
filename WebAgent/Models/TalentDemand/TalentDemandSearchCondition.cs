using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAgent.Models.TalentDemand
{
    public class TalentDemandSearchCondition : SearchCondition
    {
        public Enumeration.TalentDemandDealtStatus DealtStatus { get; set; }

        public Enumeration.OrderStatus Status { get; set; }
    }
}