using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL
{
    public class ApplyPosProvider : BaseProvider
    {
        public CustomJsonResult Apply(int operater,int agentId, int salesmanId, int[] posMachineIds)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                if (posMachineIds != null)
                {
                    var salesman = CurrentDb.SysSalesmanUser.Where(m => m.Id == salesmanId).FirstOrDefault();
                    var agent = CurrentDb.SysAgentUser.Where(m => m.Id == agentId).FirstOrDefault();

                    foreach (var id in posMachineIds)
                    {
                        var posMachine = CurrentDb.PosMachine.Where(m => m.Id == id).FirstOrDefault();
                        posMachine.AgentId = agent.Id;
                        posMachine.AgentName = agent.FullName;
                        posMachine.SalesmanId = salesman.Id;
                        posMachine.SalesmanName = salesman.FullName;
                        posMachine.Mender = operater;
                        posMachine.LastUpdateTime = this.DateTime;

                        CurrentDb.SaveChanges();

                        SalesmanApplyPosRecord salesmanApplyPosRecord = new SalesmanApplyPosRecord();
                        salesmanApplyPosRecord.PosMachineId = posMachine.Id;
                        salesmanApplyPosRecord.PosMachineDeviceId = posMachine.DeviceId;
                        salesmanApplyPosRecord.AgentId = agent.Id;
                        salesmanApplyPosRecord.AgentName = agent.FullName;
                        salesmanApplyPosRecord.SalesmanId = salesman.Id;
                        salesmanApplyPosRecord.SalesmanName = salesman.FullName;
                        salesmanApplyPosRecord.CreateTime = this.DateTime;
                        salesmanApplyPosRecord.Creator = operater;
                        CurrentDb.SalesmanApplyPosRecord.Add(salesmanApplyPosRecord);
                        CurrentDb.SaveChanges();

                    }
                }

                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, "登记成功");
            }

            return result;
        }
    }
}
