using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.CarClaim
{
    public class SearchCondition: BaseSearchCondition
    {
        public Enumeration.CarClaimDealtStatus DealtStatus { get; set; }

        public Enumeration.OrderStatus Status { get; set; }

    }
}