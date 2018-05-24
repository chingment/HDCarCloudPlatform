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
    public class PosMachineProvider : BaseProvider
    {
        public CustomJsonResult Add(int operater, PosMachine posMachine)
        {
            CustomJsonResult result = new CustomJsonResult();

            var l_posMachine = CurrentDb.PosMachine.Where(m => m.DeviceId == posMachine.DeviceId).FirstOrDefault();
            if (l_posMachine != null)
                return new CustomJsonResult(ResultType.Failure, "该POS机设备ID已经登记");


            posMachine.CreateTime = this.DateTime;
            posMachine.Creator = operater;
            posMachine.IsUse = false;

            CurrentDb.PosMachine.Add(posMachine);
            CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, "登记成功");
        }

        public CustomJsonResult Edit(int operater, PosMachine posMachine)
        {
            CustomJsonResult result = new CustomJsonResult();

            var l_posMachine = CurrentDb.PosMachine.Where(m => m.Id == posMachine.Id).FirstOrDefault();
            if (l_posMachine == null)
                return new CustomJsonResult(ResultType.Failure, "不存在");

            l_posMachine.AgentId = posMachine.AgentId;
            l_posMachine.AgentName = posMachine.AgentName;
            l_posMachine.FuselageNumber = posMachine.FuselageNumber;
            l_posMachine.TerminalNumber = posMachine.TerminalNumber;
            l_posMachine.Version = posMachine.Version;
            l_posMachine.LastUpdateTime = this.DateTime;
            l_posMachine.Mender = operater;
            CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, "保存成功");
        }

        public CustomJsonResult Change(int operater, int merchantId, int oldPosMachineId, int newPosMachineId)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {

                var merchantPosMachine = CurrentDb.MerchantPosMachine.Where(m => m.MerchantId == merchantId && m.PosMachineId == oldPosMachineId).FirstOrDefault();


                var oldPosMachine = CurrentDb.PosMachine.Where(m => m.Id == oldPosMachineId).FirstOrDefault();

                var newPosMachine = CurrentDb.PosMachine.Where(m => m.Id == newPosMachineId).FirstOrDefault();


                oldPosMachine.IsUse = false;
                oldPosMachine.LastUpdateTime = this.DateTime;
                oldPosMachine.Mender = operater;



                newPosMachine.IsUse = true;
                newPosMachine.LastUpdateTime = this.DateTime;
                newPosMachine.Mender = operater;


                merchantPosMachine.PosMachineId = newPosMachine.Id;
                merchantPosMachine.LastUpdateTime = this.DateTime;
                merchantPosMachine.Mender = operater;


                MerchantPosMachineChangeHistory changeHistory = new MerchantPosMachineChangeHistory();

                changeHistory.UserId = merchantPosMachine.UserId;
                changeHistory.MerchantId = merchantPosMachine.MerchantId;
                changeHistory.MerchantPosMachineId = changeHistory.MerchantPosMachineId;
                changeHistory.OldPosMachineId = oldPosMachine.Id;
                changeHistory.NewPosMachineId = newPosMachine.Id;
                changeHistory.Reason = changeHistory.Reason;
                changeHistory.CreateTime = this.DateTime;
                changeHistory.Creator = operater;

                CurrentDb.MerchantPosMachineChangeHistory.Add(changeHistory);

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, "更换成功");
            }

            return result;
        }

    }

}
