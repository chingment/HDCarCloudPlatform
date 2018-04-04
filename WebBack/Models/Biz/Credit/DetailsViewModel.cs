using Lumos.BLL;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Credit
{
    public class DetailsViewModel : BaseViewModel
    {
        private Lumos.Entity.Merchant _merchant = new Lumos.Entity.Merchant();

        private Lumos.Entity.OrderToCredit _orderToCredit = new Lumos.Entity.OrderToCredit();

        private List<BizProcessesAuditDetails> _bizProcessesAuditDetails = new List<BizProcessesAuditDetails>();

        public DetailsViewModel()
        {

        }

        public DetailsViewModel(int id)
        {
            var orderToCredit = CurrentDb.OrderToCredit.Where(m => m.Id == id).FirstOrDefault();
            if (orderToCredit != null)
            {
                _orderToCredit = orderToCredit;

                var merchant = CurrentDb.Merchant.Where(m => m.Id == orderToCredit.MerchantId).FirstOrDefault();
                if (merchant != null)
                {
                    _merchant = merchant;
                }


                _bizProcessesAuditDetails = BizFactory.BizProcessesAudit.GetDetails(Enumeration.BizProcessesAuditType.OrderToCredit, id);

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

        public Lumos.Entity.OrderToCredit OrderToCredit
        {
            get
            {
                return _orderToCredit;
            }
            set
            {
                _orderToCredit = value;
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