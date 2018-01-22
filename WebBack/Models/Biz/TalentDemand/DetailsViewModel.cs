using Lumos.BLL;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.TalentDemand
{
    public class DetailsViewModel : BaseViewModel
    {
        private Lumos.Entity.Merchant _merchant = new Lumos.Entity.Merchant();

        private Lumos.Entity.OrderToTalentDemand _orderToTalentDemand = new Lumos.Entity.OrderToTalentDemand();

        private List<BizProcessesAuditDetails> _bizProcessesAuditDetails = new List<BizProcessesAuditDetails>();

        public DetailsViewModel()
        {

        }

        public DetailsViewModel(int id)
        {
            var orderToTalentDemand = CurrentDb.OrderToTalentDemand.Where(m => m.Id == id).FirstOrDefault();
            if (orderToTalentDemand != null)
            {
                _orderToTalentDemand = orderToTalentDemand;

                var merchant = CurrentDb.Merchant.Where(m => m.Id == orderToTalentDemand.MerchantId).FirstOrDefault();
                if (merchant != null)
                {
                    _merchant = merchant;
                }


                _bizProcessesAuditDetails = BizFactory.BizProcessesAudit.GetDetails(Enumeration.BizProcessesAuditType.TalentDemand, id);

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

        public Lumos.Entity.OrderToTalentDemand OrderToTalentDemand
        {
            get
            {
                return _orderToTalentDemand;
            }
            set
            {
                _orderToTalentDemand = value;
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