using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Merchant
{
    public class OpenAccountViewModel:BaseViewModel
    {
        private Lumos.Entity.Merchant _merchant = new Lumos.Entity.Merchant();
        private Lumos.Entity.MerchantPosMachine _merchantPosMachine = new Lumos.Entity.MerchantPosMachine();
        private Lumos.Entity.BankCard _bankCard = new Lumos.Entity.BankCard();
        public OpenAccountViewModel()
        {

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

        public Lumos.Entity.BankCard BankCard
        {
            get
            {
                return _bankCard;
            }
            set
            {
                _bankCard = value;
            }
        }

        public Lumos.Entity.MerchantPosMachine MerchantPosMachine
        {
            get
            {
                return _merchantPosMachine;
            }
            set
            {
                _merchantPosMachine = value;
            }
        }


    }
}