using StarPaySdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarPaySdkTest
{
    class Program
    {
        static void Main(string[] args)
        {

            //StarPayUtil.CodeDownload(null);

            var starPayOrderInfo = new StarPayOrderInfo();
            starPayOrderInfo.Amount = "1";
            starPayOrderInfo.OrderSn = "18050313200000001523";
            starPayOrderInfo.PayWay = "";
            starPayOrderInfo.TransTime = DateTime.Parse("2018-05-03 13:20:37.633");
            string s = "";
            StarPayUtil.PayQuery(starPayOrderInfo, out s);
        }
    }
}
