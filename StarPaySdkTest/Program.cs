using log4net;
using Lumos.BLL;
using Lumos.Entity.AppApi;
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
            UnifiedOrderParams pms = new UnifiedOrderParams();
            pms.UserId = 1215;
            pms.MerchantId = 241;
            pms.PosMachineId = 148;
            pms.OrderSn = "18052209560000002603";
            pms.PayWay = Lumos.Entity.Enumeration.OrderPayWay.Wechat;
            var result = SdkFactory.StarPay.UnifiedOrder(pms.UserId, pms);

            

            //var starPayOrderInfo = new StarPayOrderInfo();
            //starPayOrderInfo.Amount = "2";
            //starPayOrderInfo.TransSn = "18052210080000000007";
            //starPayOrderInfo.OrderSn = pms.OrderSn;
            //starPayOrderInfo.PayWay = "WXPAY";
            //starPayOrderInfo.TransTime = DateTime.Parse("2018-05-22 10:08:33.783");

            ////StarPayUtil.CodeDownload(starPayOrderInfo);


            //string s = "";
            //StarPayUtil.PayQuery(starPayOrderInfo, out s);
        }
    }
}
