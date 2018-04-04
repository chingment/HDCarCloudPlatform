using Lumos.BLL;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.TalentDemand
{
    public class VerifyViewModel : BaseViewModel
    {
        private Lumos.Entity.Merchant _merchant = new Lumos.Entity.Merchant();

        private Lumos.Entity.OrderToTalentDemand _orderToTalentDemand = new Lumos.Entity.OrderToTalentDemand();

        private Lumos.Entity.BizProcessesAudit _bizProcessesAudit = new Lumos.Entity.BizProcessesAudit();


        public VerifyViewModel()
        {

        }

        public VerifyViewModel(int id)
        {

            var bizProcessesAudit = BizFactory.BizProcessesAudit.ChangeStatus(id, Enumeration.TalentDemandAuditStatus.InVerify, this.Operater, null, "已取单，正在核实");

            if (bizProcessesAudit != null)
            {
                _bizProcessesAudit = bizProcessesAudit;

                if (_bizProcessesAudit.Auditor.Value != this.Operater)
                {
                    this.IsHasOperater = true;
                    this.OperaterName = SysFactory.SysUser.GetFullName(_bizProcessesAudit.Auditor.Value);
                }


                var orderToTalentDemand = CurrentDb.OrderToTalentDemand.Where(m => m.Id == bizProcessesAudit.AduitReferenceId).FirstOrDefault();
                if (orderToTalentDemand != null)
                {
                    _orderToTalentDemand = orderToTalentDemand;

                    var merchant = CurrentDb.Merchant.Where(m => m.Id == orderToTalentDemand.MerchantId).FirstOrDefault();
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