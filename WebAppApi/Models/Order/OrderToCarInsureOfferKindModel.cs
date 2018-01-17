using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Order
{
    public class OrderToCarInsureOfferKindModel
    {
        public OrderToCarInsureOfferKindModel()
        {

        }

        public OrderToCarInsureOfferKindModel(string field, string value)
        {
            this.Field = field;
            this.Value = value;
        }

        public string Field { get; set; }

        public string Value { get; set; }
    }
}