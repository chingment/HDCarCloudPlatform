﻿using Lumos.BLL;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.ApplyLossAssess
{
    public class VerifyViewModel : BaseViewModel
    {
        private Lumos.Entity.Merchant _merchant = new Lumos.Entity.Merchant();

        private Lumos.Entity.OrderToApplyLossAssess _orderToApplyLossAssess = new Lumos.Entity.OrderToApplyLossAssess();

        private Lumos.Entity.BizProcessesAudit _bizProcessesAudit = new Lumos.Entity.BizProcessesAudit();


        public VerifyViewModel()
        {

        }

        public VerifyViewModel(int id)
        {
            var bizProcessesAudit = BizFactory.BizProcessesAudit.ChangeStatusByAuditFlowV1(id, Enumeration.AuditFlowV1Status.InVerify, this.Operater, null, "已取单，正在核实");
            if (bizProcessesAudit != null)
            {
                _bizProcessesAudit = bizProcessesAudit;

                if (_bizProcessesAudit.Auditor.Value != this.Operater)
                {
                    this.IsHasOperater = true;
                    this.OperaterName = SysFactory.SysUser.GetFullName(_bizProcessesAudit.Auditor.Value);
                }


                var orderToApplyLossAssess = CurrentDb.OrderToApplyLossAssess.Where(m => m.Id == bizProcessesAudit.AduitReferenceId).FirstOrDefault();
                if (orderToApplyLossAssess != null)
                {
                    _orderToApplyLossAssess = orderToApplyLossAssess;

                    var merchant = CurrentDb.Merchant.Where(m => m.Id == orderToApplyLossAssess.MerchantId).FirstOrDefault();
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