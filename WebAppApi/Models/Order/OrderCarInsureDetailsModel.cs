using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.Order
{
    public class ZjModel
    {
        public ZjModel()
        {

        }

        public ZjModel(string name,string url)
        {
            this.Name = name;
            this.Url = url;
        }
        public string Name { get; set; }

        public string Url { get; set; }
    }
    public class OrderCarInsureDetailsModel:OrderBaseDetailsViewModel
    {
        public OrderCarInsureDetailsModel()
        {
            this.OfferCompany = new List<OrderCarInsureOfferCompanyModel>();
            this.OfferKind = new List<OrderToCarInsureOfferKindModel>();
            this.RecipientAddressList = new List<string>();
            this.ZJ = new List<ZjModel>();
        }

        public List<OrderCarInsureOfferCompanyModel> OfferCompany { get; set; }

        public List<OrderToCarInsureOfferKindModel> OfferKind { get; set; }

        public string CarOwner { get; set; }

        public string CarPlateNo { get; set; }

        public string CarOwnerIdNumber { get; set; }

        public List<ZjModel> ZJ { get; set; }

        public int InsuranceCompanyId { get; set; }

        public string InsuranceCompanyName { get; set; }

        public string InsureImgUrl { get; set; }

        public string CommercialPrice { get; set; }

        public string TravelTaxPrice { get; set; }

        public string CompulsoryPrice { get; set; }

        public List<string> RecipientAddressList { get; set; }

        public string Recipient { get; set; }
        public string RecipientAddress { get; set; }
        public string RecipientPhoneNumber { get; set; }

    }
}