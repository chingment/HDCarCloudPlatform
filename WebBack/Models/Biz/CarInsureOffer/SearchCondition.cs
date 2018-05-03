using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lumos.Entity;

namespace WebBack.Models.Biz.CarInsureOffer
{
    public class SearchCondition: BaseSearchCondition
    {
        public Enumeration.CarInsureOfferDealtStatus AuditStatus { get; set; }

        public Enumeration.OrderStatus Status { get; set; }
    }
}