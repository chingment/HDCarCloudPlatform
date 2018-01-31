using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.ApplyLossAssess
{
    public class ApplyLossAssessSearchCondition : SearchCondition
    {
        public Enumeration.ApplyLossAssessDealtStatus DealtStatus { get; set; }

        public Enumeration.OrderStatus Status { get; set; }
    }
}