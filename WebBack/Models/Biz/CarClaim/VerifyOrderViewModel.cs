using Lumos.BLL;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.CarClaim
{
    public class VerifyOrderViewModel : BaseViewModel
    {
        private Lumos.Entity.Merchant _merchant = new Lumos.Entity.Merchant();

        private Lumos.Entity.OrderToCarClaim _orderToCarClaim = new Lumos.Entity.OrderToCarClaim();

        private Lumos.Entity.BizProcessesAudit _bizProcessesAudit = new Lumos.Entity.BizProcessesAudit();

        public string EstimateMerchantRemarks { get; set; }

        public int EstimateMerchantId { get; set; }

        public VerifyOrderViewModel()
        {

        }

        public VerifyOrderViewModel(int id)
        {
            var bizProcessesAudit = BizFactory.BizProcessesAudit.ChangeCarClaimDealtStatus(this.Operater, id, Enumeration.CarClaimDealtStatus.InVerifyOrder);
            if (bizProcessesAudit != null)
            {
                _bizProcessesAudit = bizProcessesAudit;

                if (_bizProcessesAudit.Auditor.Value != this.Operater)
                {
                    this.IsHasOperater = true;
                    this.OperaterName = SysFactory.SysUser.GetFullName(_bizProcessesAudit.Auditor.Value);
                }


                var orderToCarClaim = CurrentDb.OrderToCarClaim.Where(m => m.Id == bizProcessesAudit.AduitReferenceId).FirstOrDefault();
                if (orderToCarClaim != null)
                {
                    _orderToCarClaim = orderToCarClaim;

                    var merchant = CurrentDb.Merchant.Where(m => m.Id == orderToCarClaim.MerchantId).FirstOrDefault();
                    if (merchant != null)
                    {
                        _merchant = merchant;
                    }

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

        public Lumos.Entity.BizProcessesAudit BizProcessesAudit
        {
            get
            {
                return _bizProcessesAudit;
            }
            set
            {
                _bizProcessesAudit = value;
            }
        }

    }
}