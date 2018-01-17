using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Withdraw
{
    public class DetailsViewModel : BaseViewModel
    {
        private Lumos.Entity.Merchant _merchant = new Lumos.Entity.Merchant();
        private Lumos.Entity.Withdraw _withdraw = new Lumos.Entity.Withdraw();
        private Lumos.Entity.BankCard _withdrawBankCard = new Lumos.Entity.BankCard();

        public Lumos.Entity.Withdraw Withdraw
        {
            get
            {
                return _withdraw;
            }
            set
            {
                _withdraw = value;
            }
        }


        public Lumos.Entity.BankCard WithdrawBankCard
        {
            get
            {
                return _withdrawBankCard;
            }
            set
            {
                _withdrawBankCard = value;
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

        public DetailsViewModel()
        {

        }

        public DetailsViewModel(int id)
        {
            var withdraw = CurrentDb.Withdraw.Where(m => m.Id == id).FirstOrDefault();
            if(withdraw!=null)
            {
                _withdraw=withdraw;

                var withdrawbankCard=CurrentDb.BankCard.Where(m=>m.Id==withdraw.BankCardId).FirstOrDefault();
                if(withdrawbankCard!=null)
                {
                    _withdrawBankCard = withdrawbankCard;
                }

                var merchant = CurrentDb.Merchant.Where(m => m.Id == withdraw.MerchantId).FirstOrDefault();
                if (merchant != null)
                {
                    _merchant = merchant;
                }

            }
        }
    }
}