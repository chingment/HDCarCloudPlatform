using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Order
{
    public class SubmitTalentDemandModel
    {
        public int UserId { get; set; }

        public int MerchantId { get; set; }

        public int PosMachineId { get; set; }

        public Lumos.Entity.Enumeration.WorkJob WorkJob { get; set; }

        public int Quantity { get; set; }
    }
}