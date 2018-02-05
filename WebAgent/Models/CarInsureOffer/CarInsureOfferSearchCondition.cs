using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lumos.Entity;

namespace WebAgent.Models.CarInsureOffer
{
    public class CarInsureOfferSearchCondition:SearchCondition
    {
        public Enumeration.CarInsureOfferDealtStatus DealtStatus { get; set; }

        public Enumeration.OrderStatus Status { get; set; }
    }
}