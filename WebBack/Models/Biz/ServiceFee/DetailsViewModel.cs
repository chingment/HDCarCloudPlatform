using Lumos.BLL;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.ServiceFee
{
    public class DetailsViewModel : BaseViewModel
    {
        private Lumos.Entity.SysClientUser _sysClientUser = new Lumos.Entity.SysClientUser();

        private Lumos.Entity.Merchant _merchant = new Lumos.Entity.Merchant();

        private Lumos.Entity.OrderToServiceFee _orderToServiceFee = new Lumos.Entity.OrderToServiceFee();

        private List<BizProcessesAuditDetails> _bizProcessesAuditDetails = new List<BizProcessesAuditDetails>();

        public DetailsViewModel()
        {

        }

        public DetailsViewModel(int id)
        {
            var orderToServiceFee = CurrentDb.OrderToServiceFee.Where(m => m.Id == id).FirstOrDefault();
            if (_orderToServiceFee != null)
            {
                _orderToServiceFee = orderToServiceFee;

                var merchant = CurrentDb.Merchant.Where(m => m.Id == orderToServiceFee.MerchantId).FirstOrDefault();
                if (merchant != null)
                {
                    _merchant = merchant;
                }

                var sysClientUser = CurrentDb.SysClientUser.Where(m => m.Id == orderToServiceFee.UserId).FirstOrDefault();
                if (sysClientUser != null)
                {
                    _sysClientUser = sysClientUser;
                }

                //_bizProcessesAuditDetails = BizFactory.BizProcessesAudit.GetDetails(Enumeration.BizProcessesAuditType.SE, id);

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

        public Lumos.Entity.OrderToServiceFee OrderToServiceFee
        {
            get
            {
                return _orderToServiceFee;
            }
            set
            {
                _orderToServiceFee = value;
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

        public Lumos.Entity.SysClientUser SysClientUser
        {
            get
            {
                return _sysClientUser;
            }
            set
            {
                _sysClientUser = value;
            }
        }
    }
}