using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.PosMachine
{
    public class ChangeViewModel : BaseViewModel
    {
        private Lumos.Entity.PosMachine _oldPosMachine = new Lumos.Entity.PosMachine();

        private Lumos.Entity.MerchantPosMachineChangeHistory _changeHistory = new Lumos.Entity.MerchantPosMachineChangeHistory();

        private Lumos.Entity.Merchant _merchant = new Lumos.Entity.Merchant();

        public Lumos.Entity.PosMachine OldPosMachine
        {
            get
            {
                return _oldPosMachine;
            }
            set
            {
                _oldPosMachine = value;
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

        public Lumos.Entity.MerchantPosMachineChangeHistory ChangeHistory
        {
            get
            {
                return _changeHistory;
            }
            set
            {
                _changeHistory = value;
            }
        }

        public ChangeViewModel()
        {

        }

        public ChangeViewModel(int id)
        {
            _changeHistory.MerchantPosMachineId = id;

            var merchantPosMachine = CurrentDb.MerchantPosMachine.Where(m => m.Id == id).FirstOrDefault();
            if (merchantPosMachine != null)
            {
                var merchant = CurrentDb.Merchant.Where(m => m.Id == merchantPosMachine.MerchantId).FirstOrDefault();
                if (merchant != null)
                {
                    _merchant = merchant;
                }

                var oldPosMachine = CurrentDb.PosMachine.Where(m => m.Id == merchantPosMachine.PosMachineId).FirstOrDefault();
                if (oldPosMachine != null)
                {
                    _oldPosMachine = oldPosMachine;

                    _changeHistory.OldPosMachineId = oldPosMachine.Id;
                }
            }
        }
    }
}