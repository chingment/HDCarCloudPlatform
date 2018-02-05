using Lumos.BLL;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAgent.Models.CarClaim
{
    public class DetailsViewModel : BaseViewModel
    {
        private Lumos.Entity.Merchant _merchant = new Lumos.Entity.Merchant();

        private Lumos.Entity.Merchant _estimateMerchant = new Lumos.Entity.Merchant();

        private Lumos.Entity.OrderToCarClaim _orderToCarClaim = new Lumos.Entity.OrderToCarClaim();

        private List<BizProcessesAuditDetails> _bizProcessesAuditDetails = new List<BizProcessesAuditDetails>();


        public DetailsViewModel()
        {

        }

        public DetailsViewModel(int id)
        {


            var orderToCarClaim = CurrentDb.OrderToCarClaim.Where(m => m.Id == id).FirstOrDefault();
            if (orderToCarClaim != null)
            {
                _orderToCarClaim = orderToCarClaim;

                var merchant = CurrentDb.Merchant.Where(m => m.Id == orderToCarClaim.MerchantId).FirstOrDefault();
                if (merchant != null)
                {
                    _merchant = merchant;
                }

                var estimateMerchant = CurrentDb.Merchant.Where(m => m.Id == orderToCarClaim.HandMerchantId).FirstOrDefault();
                if (estimateMerchant != null)
                {
                    _estimateMerchant = estimateMerchant;
                }

                _bizProcessesAuditDetails = BizFactory.BizProcessesAudit.GetDetails(Enumeration.BizProcessesAuditType.CarClaim, id);

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

        public Lumos.Entity.Merchant EstimateMerchant
        {
            get
            {
                return _estimateMerchant;
            }
            set
            {
                _estimateMerchant = value;
            }
        }

        public Lumos.Entity.OrderToCarClaim OrderToCarClaim
        {
            get
            {
                return _orderToCarClaim;
            }
            set
            {
                _orderToCarClaim = value;
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