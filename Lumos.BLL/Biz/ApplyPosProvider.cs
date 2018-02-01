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
        public CustomJsonResult Apply(int operater, int salesmanId, int[] merchantPosMachineIds)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                if (merchantPosMachineIds != null)
                {

                    foreach (var id in merchantPosMachineIds)
                    {
                        var merchantPosMachine = CurrentDb.MerchantPosMachine.Where(m => m.Id == id).FirstOrDefault();
                        var merchant = CurrentDb.Merchant.Where(m => m.Id == merchantPosMachine.MerchantId).FirstOrDefault();
                        if (merchant.SalesmanId != null)
                        {
                            var salesman = CurrentDb.SysSalesmanUser.Where(m => m.Id == merchant.SalesmanId.Value).FirstOrDefault();
                            var posMachine = CurrentDb.PosMachine.Where(m => m.Id == merchantPosMachine.PosMachineId).FirstOrDefault();
                            return new CustomJsonResult(ResultType.Failure, string.Format("POS机({0})已被业务员({1})领用，请移除后再次确定提交", posMachine.DeviceId, salesman.FullName));
                        }

                        merchantPosMachine.Mender = operater;
                        merchantPosMachine.LastUpdateTime = this.DateTime;


                        merchant.SalesmanId = salesmanId;
                        merchant.Mender = operater;
                        merchant.LastUpdateTime = this.DateTime;

                        CurrentDb.SaveChanges();

                        SalesmanApplyPosRecord salesmanApplyPosRecord = new SalesmanApplyPosRecord();
                        salesmanApplyPosRecord.MerchantId = merchantPosMachine.MerchantId;
                        salesmanApplyPosRecord.UserId = merchantPosMachine.UserId;
                        salesmanApplyPosRecord.PosMachineId = merchantPosMachine.PosMachineId;
                        salesmanApplyPosRecord.SalesmanId = salesmanId;
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
