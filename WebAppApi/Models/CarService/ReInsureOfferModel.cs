using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.CarService
{
    public class ReInsureOfferModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int MerchantId { get; set; }

        public int OrderId { get; set; }
    }
}