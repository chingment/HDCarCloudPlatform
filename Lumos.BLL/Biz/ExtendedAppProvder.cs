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
    public class ExtendedAppProvder : BaseProvider
    {
        public CustomJsonResult Add(int operater, ExtendedApp extendedApp)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var isExsits = CurrentDb.ExtendedApp.Where(m => m.Name == extendedApp.Name || m.LinkUrl == extendedApp.LinkUrl).Count();
                if (isExsits > 0)
                {
                    return new CustomJsonResult(ResultType.Failure, "该已存在相同应用的名称或链接");
                }

                var isExsitsAppKey = CurrentDb.ExtendedApp.Where(m => m.AppKey == extendedApp.AppKey).Count();
                if (isExsitsAppKey > 0)
                {
                    return new CustomJsonResult(ResultType.Failure, "该已存在相同应用AppKey");
                }

                extendedApp.CreateTime = this.DateTime;
                extendedApp.Creator = operater;
                extendedApp.Status = Enumeration.ExtendedAppStatus.Normal;
                extendedApp.Type = Enumeration.ExtendedAppType.ThirdPartyApp;
                extendedApp.AppSecret = Guid.NewGuid().ToString().Replace("-", "");
                CurrentDb.ExtendedApp.Add(extendedApp);
                CurrentDb.SaveChanges();

                SysFactory.SysItemCacheUpdateTime.Update(Enumeration.SysItemCacheType.ExtendedApp);

                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, "添加成功");
            }

            return result;
        }


    }
}
