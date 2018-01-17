using Lumos.BLL;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.CarClaim
{
    public class VerifyAmountViewModel:BaseViewModel
    {
        private Lumos.Entity.Merchant _merchant = new Lumos.Entity.Merchant();

        private Lumos.Entity.Merchant _estimateMerchant = new Lumos.Entity.Merchant();

        private Lumos.Entity.OrderToCarClaim _orderToCarClaim = new Lumos.Entity.OrderToCarClaim();

        private Lumos.Entity.BizProcessesAudit _bizProcessesAudit = new Lumos.Entity.BizProcessesAudit();


        public VerifyAmountViewModel()
        {

        }

        public VerifyAmountViewModel(int id)
        {
            var bizProcessesAudit = BizFactory.BizProcessesAudit.ChangeCarClaimDealtStatus(this.Operater, id, Enumeration.CarClaimDealtStatus.InVerifyAmount);
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

                    var estimateMerchant = CurrentDb.Merchant.Where(m => m.Id == orderToCarClaim.HandMerchantId).FirstOrDefault();
                    if (estimateMerchant != null)
                    {
                        _estimateMerchant = estimateMerchant;
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