using Lumos.BLL;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Lllegal
{
    public class DealtViewModel : BaseViewModel
    {
        private Lumos.Entity.Merchant _merchant = new Lumos.Entity.Merchant();

        private Lumos.Entity.OrderToLllegalDealt _orderToLllegalDealt = new Lumos.Entity.OrderToLllegalDealt();

        private List<Lumos.Entity.OrderToLllegalDealtDetails> _orderToLllegalDealtDetails = new List<Lumos.Entity.OrderToLllegalDealtDetails>();

        private Lumos.Entity.BizProcessesAudit _bizProcessesAudit = new Lumos.Entity.BizProcessesAudit();


        public DealtViewModel()
        {

        }

        public DealtViewModel(int id)
        {
            var bizProcessesAudit = BizFactory.BizProcessesAudit.ChangeTalentDemandDealtStatus(this.Operater, id, Enumeration.TalentDemandDealtStatus.InDealt, "后台人员正在处理中");
            if (bizProcessesAudit != null)
            {
                _bizProcessesAudit = bizProcessesAudit;

                if (_bizProcessesAudit.Auditor.Value != this.Operater)
                {
                    this.IsHasOperater = true;
                    this.OperaterName = SysFactory.SysUser.GetFullName(_bizProcessesAudit.Auditor.Value);
                }


                var orderToLllegalDealt = CurrentDb.OrderToLllegalDealt.Where(m => m.Id == bizProcessesAudit.AduitReferenceId).FirstOrDefault();
                if (OrderToLllegalDealt != null)
                {
                    _orderToLllegalDealt = orderToLllegalDealt;

                    var merchant = CurrentDb.Merchant.Where(m => m.Id == orderToLllegalDealt.MerchantId).FirstOrDefault();
                    if (merchant != null)
                    {
                        _merchant = merchant;
                    }


                    var orderToLllegalDealtDetails = CurrentDb.OrderToLllegalDealtDetails.Where(m => m.OrderId == orderToLllegalDealt.Id).ToList();
                    if (orderToLllegalDealtDetails != null)
                    {
                        _orderToLllegalDealtDetails = orderToLllegalDealtDetails;
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