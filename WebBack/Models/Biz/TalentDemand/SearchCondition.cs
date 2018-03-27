using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.TalentDemand
{
    public class SearchCondition : BaseSearchCondition
    {
        public Enumeration.TalentDemandDealtStatus DealtStatus { get; set; }

        public Enumeration.OrderStatus Status { get; set; }
    }
}