using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.TalentDemand
{
    public class VerifyOrderViewModel : BaseViewModel
    {

        private Lumos.Entity.OrderToTalentDemand _orderToTalentDemand = new Lumos.Entity.OrderToTalentDemand();

        private Lumos.Entity.BizProcessesAudit _bizProcessesAudit = new Lumos.Entity.BizProcessesAudit();


        public VerifyOrderViewModel(int id)
        {

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