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
    public class OrderCarInsureDetailsModel
    {
        public OrderCarInsureDetailsModel()
        {
            this.OfferCompany = new List<OrderCarInsureOfferCompanyModel>();
            this.OfferKind = new List<OrderToCarInsureOfferKindModel>();
            this.RecipientAddressList = new List<string>();
            this.ZJ = new List<ZjModel>();
        }
        public int Id { get; set; }

        public string Sn { get; set; }

        public List<OrderCarInsureOfferCompanyModel> OfferCompany { get; set; }

        public List<OrderToCarInsureOfferKindModel> OfferKind { get; set; }

        public Enumeration.OrderStatus Status { get; set; }

        public int FollowStatus { get; set; }

        public string StatusName { get; set; }

        public DateTime SubmitTime { get; set; }

        public DateTime? CompleteTime { get; set; }

        public DateTime? CancleTime { get; set; }

        public DateTime? PayTime { get; set; }

        public string CarOwner { get; set; }

        public string CarPlateNo { get; set; }

        public string CarOwnerIdNumber { get; set; }

        public List<ZjModel> ZJ { get; set; }


        public int InsuranceCompanyId { get; set; }

        public string InsuranceCompanyName { get; set; }

        public string InsureImgUrl { get; set; }

        public decimal CommercialPrice { get; set; }

        public decimal TravelTaxPrice { get; set; }

        public decimal CompulsoryPrice { get; set; }

        public decimal Price { get; set; }

        public List<string> RecipientAddressList { get; set; }

        public string Recipient { get; set; }
        public string RecipientAddress { get; set; }
        public string RecipientPhoneNumber { get; set; }

        public string Remarks { get; set; }

    }
}