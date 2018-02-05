using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAgent.Models.SalesmanUser
{
    public class AddViewModel : BaseViewModel
    {
        private SysAgentUser _sysAgentUser = new SysAgentUser();

        private SysSalesmanUser _sysSalesmanUser = new SysSalesmanUser();

        public SysSalesmanUser SysSalesmanUser
        {
            get
            {
                return _sysSalesmanUser;
            }
            set
            {
                _sysSalesmanUser = value;
            }
        }

        public SysAgentUser SysAgentUser
        {
            get
            {
                return _sysAgentUser;
            }
            set
            {
                _sysAgentUser = value;
            }
        }

        public void LoadData(int agentId)
        {

            var sysAgentUser = CurrentDb.SysAgentUser.Where(m => m.Id == agentId).FirstOrDefault();
            if (sysAgentUser != null)
            {
                _sysAgentUser = sysAgentUser;
            }
        }

    }
}