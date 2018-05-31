using Lumos.BLL;
using Lumos.Entity;
using Lumos.Entity.AppApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Insurance
{
    public class VerifyViewModel : BaseViewModel
    {
        private Lumos.Entity.Merchant _merchant = new Lumos.Entity.Merchant();

        private Lumos.Entity.OrderToInsurance _orderToInsurance = new Lumos.Entity.OrderToInsurance();

        private Lumos.Entity.BizProcessesAudit _bizProcessesAudit = new Lumos.Entity.BizProcessesAudit();

        private List<ItemField> _insPlanDetailsItems = new List<ItemField>();


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


                var orderToInsurance = CurrentDb.OrderToInsurance.Where(m => m.Id == bizProcessesAudit.AduitReferenceId).FirstOrDefault();
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