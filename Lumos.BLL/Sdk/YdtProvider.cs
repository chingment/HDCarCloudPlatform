using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class YdtProvider : BaseProvider
    {

        public void NotifyLog(string type, string orderSeq, string content)
        {

            try
            {
                var ydtNotifyLog = new YdtNotifyLog();

                ydtNotifyLog.Type = type;
                ydtNotifyLog.OrderSeq = orderSeq;
                ydtNotifyLog.NotifyContent = content;
                ydtNotifyLog.Creator = 0;
                ydtNotifyLog.CreateTime = this.DateTime;

                CurrentDb.YdtNotifyLog.Add(ydtNotifyLog);
                CurrentDb.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.Error(type + "通知日志" + content);
            }
        }
    }
}
