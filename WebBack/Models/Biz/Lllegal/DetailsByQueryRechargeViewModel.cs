using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Lllegal
{
    public class DetailsByQueryRechargeViewModel : BaseViewModel
    {
        private Lumos.Entity.Merchant _merchant = new Lumos.Entity.Merchant();

        private Lumos.Entity.OrderToLllegalQueryRecharge _orderToLllegalQueryRecharge = new Lumos.Entity.OrderToLllegalQueryRecharge();

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

        public Lumos.Entity.OrderToLllegalQueryRecharge OrderToLllegalQueryRecharge
        {
            get
            {
                return _orderToLllegalQueryRecharge;
            }
            set
            {
                _orderToLllegalQueryRecharge = value;
            }
        }



        public DetailsByQueryRechargeViewModel(int id)
        {



            var orderToLllegalQueryRecharge = CurrentDb.OrderToLllegalQueryRecharge.Where(m => m.Id == id).FirstOrDefault();
            if (orderToLllegalQueryRecharge != null)
            {
                _orderToLllegalQueryRecharge = orderToLllegalQueryRecharge;

                var merchant = CurrentDb.Merchant.Where(m => m.Id == orderToLllegalQueryRecharge.MerchantId).FirstOrDefault();
                if (merchant != null)
                {
                    _merchant = merchant;
                }

            }

        }

    }
}