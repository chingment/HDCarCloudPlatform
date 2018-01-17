using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Transactions
{
    public class DetailsViewModel:BaseViewModel
    {
        private Lumos.Entity.Merchant _merchant = new Lumos.Entity.Merchant();

        private Lumos.Entity.Transactions _transactions = new Lumos.Entity.Transactions();

        public Lumos.Entity.Transactions Transactions
        {
            get
            {
                return _transactions;
            }
            set
            {
                _transactions = value;
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
            var transactions = CurrentDb.Transactions.Where(m => m.Id == id).FirstOrDefault();
            if (transactions != null)
            {
                _transactions = transactions;

                var merchant = CurrentDb.Merchant.Where(m => m.UserId == transactions.UserId).FirstOrDefault();
                if (merchant != null)
                {
                    _merchant = merchant;
                }
            }
        }
    }
}