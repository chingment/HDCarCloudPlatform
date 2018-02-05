using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAgent.Models.CarClaim
{
    public class CarClaimSearchCondition:SearchCondition
    {
        public Enumeration.CarClaimDealtStatus DealtStatus { get; set; }

        public Enumeration.OrderStatus Status { get; set; }

    }
}