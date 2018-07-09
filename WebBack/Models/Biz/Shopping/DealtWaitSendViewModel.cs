using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Shopping
{
    public class DealtWaitSendViewModel : BaseViewModel
    {
        private Lumos.Entity.Merchant _merchant = new Lumos.Entity.Merchant();
        private Lumos.Entity.OrderToShopping _orderToShopping = new Lumos.Entity.OrderToShopping();
        private List<Lumos.Entity.OrderToShoppingGoodsDetails> _orderToShoppingGoodsDetails = new List<Lumos.Entity.OrderToShoppingGoodsDetails>();

        public DealtWaitSendViewModel()
        {

        }

        public void LoadData(int id)
        {
            var orderToShopping = CurrentDb.OrderToShopping.Where(m => m.Id == id).FirstOrDefault();

            if(orderToShopping!=null)
            {
                _orderToShopping = orderToShopping;

                var merchant = CurrentDb.Merchant.Where(m => m.Id == orderToShopping.MerchantId).FirstOrDefault();
                if (merchant != null)
                {
                    _merchant = merchant;
                }

                var orderToShoppingGoodsDetails = CurrentDb.OrderToShoppingGoodsDetails.Where(m => m.OrderId == orderToShopping.Id).ToList();
                if (orderToShoppingGoodsDetails != null)
                {
                    _orderToShoppingGoodsDetails = orderToShoppingGoodsDetails;
                }
            }
        }

        public Lumos.Entity.Merchant Merchant
        {
            get
            {
                return _merchant;
            }
            set
            {
                _merchant = value;
            }
        }

        public Lumos.Entity.OrderToShopping OrderToShopping
        {
            get
            {
                return _orderToShopping;
            }
            set
            {
                _orderToShopping = value;
            }
        }

        public List<Lumos.Entity.OrderToShoppingGoodsDetails> OrderToShoppingGoodsDetails
        {
            get
            {
                return _orderToShoppingGoodsDetails;
            }
            set
            {
                _orderToShoppingGoodsDetails = value;
            }
        }
    }
}