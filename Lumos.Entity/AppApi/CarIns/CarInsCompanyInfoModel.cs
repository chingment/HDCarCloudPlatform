using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    public class CarInsComanyModel
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        public string Name { get; set; }
        public string Descp { get; set; }
        public int PartnerChannelId { get; set; }
        public string PartnerCode { get; set; }
        public List<ItemParentField> OfferInquirys { get; set; }
        public int OfferResult { get; set; }
        public decimal OfferSumPremium { get; set; }
        public int OfferId { get; set; }
        public string OfferMessage { get; set; }
    }
}
