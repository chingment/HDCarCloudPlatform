using AnXinSdk;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnXinTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
     
            string idCard = System.Configuration.ConfigurationManager.AppSettings["IdCard"];

            log.Info("测试,img:" + idCard);

            if (idCard != null)
            {
                IdentityCardVO identityCardVO = AnXin.GetIdentityCardByImageBase64(idCard);
            }
        }
    }
}
