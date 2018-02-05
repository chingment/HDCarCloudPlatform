using Lumos.BLL;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAgent.Models.ApplyLossAssess
{
    public class DetailsViewModel : BaseViewModel
    {
        private Lumos.Entity.Merchant _merchant = new Lumos.Entity.Merchant();

        private Lumos.Entity.OrderToApplyLossAssess _orderToApplyLossAssess = new Lumos.Entity.OrderToApplyLossAssess();

        private List<BizProcessesAuditDetails> _bizProcessesAuditDetails = new List<BizProcessesAuditDetails>();

        public DetailsViewModel()
        {

        }

        public DetailsViewModel(int id)
        {
            var orderToApplyLossAssess = CurrentDb.OrderToApplyLossAssess.Where(m => m.Id == id).FirstOrDefault();
            if (orderToApplyLossAssess != null)
            {
                _orderToApplyLossAssess = orderToApplyLossAssess;

                var merchant = CurrentDb.Merchant.Where(m => m.Id == orderToApplyLossAssess.MerchantId).FirstOrDefault();
                if (merchant != null)
                {
                    _merchant = merchant;
                }


                _bizProcessesAuditDetails = BizFactory.BizProcessesAudit.GetDetails(Enumeration.BizProcessesAuditType.ApplyLossAssess, id);

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

        public Lumos.Entity.OrderToApplyLossAssess OrderToApplyLossAssess
        {
            get
            {
                return _orderToApplyLossAssess;
            }
            set
            {
                _orderToApplyLossAssess = value;
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
    }
}