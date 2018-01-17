using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.App.Order
{
    public class InsOfferPayViewModel : BaseViewModel
    {

        private OrderToCarInsureOfferCompany _orderToCarInsureOfferCompany = new OrderToCarInsureOfferCompany();


        public OrderToCarInsureOfferCompany OrderToCarInsureOfferCompany
        {
            get
            {
                return _orderToCarInsureOfferCompany;
            }
            set
            {
                _orderToCarInsureOfferCompany = value;
            }
        }

        public InsOfferPayViewModel()
        {

        }

        public InsOfferPayViewModel(int offerid)
        {
            var orderToCarInsureOfferCompany = CurrentDb.OrderToCarInsureOfferCompany.Where(m => m.Id == offerid).FirstOrDefault();
            if (orderToCarInsureOfferCompany != null)
            {
                _orderToCarInsureOfferCompany = orderToCarInsureOfferCompany;
            }
        }
    }
}