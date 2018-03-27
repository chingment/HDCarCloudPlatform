using Lumos.BLL;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Lllegal
{
    public class DetailsViewModel : BaseViewModel
    {
        private Lumos.Entity.Merchant _merchant = new Lumos.Entity.Merchant();

        private Lumos.Entity.OrderToLllegalDealt _orderToLllegalDealt = new Lumos.Entity.OrderToLllegalDealt();

        private List<Lumos.Entity.OrderToLllegalDealtDetails> _orderToLllegalDealtDetails = new List<Lumos.Entity.OrderToLllegalDealtDetails>();

        private List<BizProcessesAuditDetails> _bizProcessesAuditDetails = new List<BizProcessesAuditDetails>();

        public DetailsViewModel()
        {

        }

        public DetailsViewModel(int id)
        {
            var orderToLllegalDealt = CurrentDb.OrderToLllegalDealt.Where(m => m.Id == id).FirstOrDefault();
            if (orderToLllegalDealt != null)
            {
                _orderToLllegalDealt = orderToLllegalDealt;

                var merchant = CurrentDb.Merchant.Where(m => m.Id == orderToLllegalDealt.MerchantId).FirstOrDefault();
                if (merchant != null)
                {
                    _merchant = merchant;
                }


                _bizProcessesAuditDetails = BizFactory.BizProcessesAudit.GetDetails(Enumeration.BizProcessesAuditType.TalentDemand, id);


                var orderToLllegalDealtDetails = CurrentDb.OrderToLllegalDealtDetails.Where(m => m.OrderId == orderToLllegalDealt.Id).ToList();
                if (orderToLllegalDealtDetails != null)
                {
                    _orderToLllegalDealtDetails = orderToLllegalDealtDetails;
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

        public Lumos.Entity.OrderToLllegalDealt OrderToLllegalDealt
        {
            get
            {
                return _orderToLllegalDealt;
            }
            set
            {
                _orderToLllegalDealt = value;
            }
        }

        public List<BizProcessesAuditDetails> BizProcessesAuditDetails
        {
            get
            {
                return _bizProcessesAuditDetails;
            }
            set
            {
                _bizProcessesAuditDetails = value;
            }
        }

        public List<Lumos.Entity.OrderToLllegalDealtDetails> OrderToLllegalDealtDetails
        {
            get
            {
                return _orderToLllegalDealtDetails;
            }
            set
            {
                _orderToLllegalDealtDetails = value;
            }
        }
    }
}