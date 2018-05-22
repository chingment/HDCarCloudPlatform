using log4net;
using StarPaySdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StarPaySdkTest
{
    class Program
    {
        public static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            var starPayOrderInfo = new StarPayOrderInfo();
            starPayOrderInfo.Amount = "2";
            starPayOrderInfo.TransSn = "180522090100000026012";
            starPayOrderInfo.OrderSn = "180522090100000026010";
            starPayOrderInfo.PayWay = "WXPAY";
            starPayOrderInfo.TransTime = DateTime.Parse("2018-05-22 09:01:07.637");

            StarPayUtil.CodeDownload(starPayOrderInfo);


            //string s = "";
            //StarPayUtil.PayQuery(starPayOrderInfo, out s);
        }
    }
}
