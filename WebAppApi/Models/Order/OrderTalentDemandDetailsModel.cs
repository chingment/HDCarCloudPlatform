using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Order
{
    public class OrderTalentDemandDetailsModel : OrderBaseDetailsViewModel
    {

        public string WorkJob { get; set; }

        public int Quantity { get; set; }

        public string UseStartTime { get; set; }

        public string UseEndTime { get; set; }

    }
}