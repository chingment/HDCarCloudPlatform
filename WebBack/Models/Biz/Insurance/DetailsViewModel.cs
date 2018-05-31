using Lumos.BLL;
using Lumos.Entity;
using Lumos.Entity.AppApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Insurance
{
    public class DetailsViewModel : BaseViewModel
    {
        private Lumos.Entity.Merchant _merchant = new Lumos.Entity.Merchant();

        private Lumos.Entity.OrderToInsurance _orderToInsurance = new Lumos.Entity.OrderToInsurance();

        private List<BizProcessesAuditDetails> _bizProcessesAuditDetails = new List<BizProcessesAuditDetails>();
        private List<ItemField> _insPlanDetailsItems = new List<ItemField>();
        public DetailsViewModel()
        {

        }

        public DetailsViewModel(int id)
        {
            var orderToInsurance = CurrentDb.OrderToInsurance.Where(m => m.Id == id).FirstOrDefault();
            if (orderToInsurance != null)
            {
                _orderToInsurance = orderToInsurance;

                var merchant = CurrentDb.Merchant.Where(m => m.Id == orderToInsurance.MerchantId).FirstOrDefault();
                if (merchant != null)
                {
                    _merchant = merchant;
                }

                if (!string.IsNullOrEmpty(_orderToInsurance.InsPlanDetailsItems))
                {
                    _insPlanDetailsItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ItemField>>(_orderToInsurance.InsPlanDetailsItems);
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

        public Lumos.Entity.OrderToInsurance OrderToInsurance
        {
            get
            {
                return _orderToInsurance;
            }
            set
            {
                _orderToInsurance = value;
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

        public List<ItemField> InsPlanDetailsItems
        {
            get
            {
                return _insPlanDetailsItems;
            }
            set
            {
                _insPlanDetailsItems = value;
            }
        }
    }
}