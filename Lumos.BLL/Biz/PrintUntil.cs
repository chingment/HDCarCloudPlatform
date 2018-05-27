using Lumos.Entity.AppApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class PrintUntil
    {
        public static PrintDataModel GetPrintData(string typeName, string tradeType, string transSn, string payMethod, DateTime tradeDateTime, string orderSn, decimal price)
        {
            PrintDataModel printData = new PrintDataModel();
            printData.MerchantName = BizFactory.AppSettings.MerchantName;
            printData.OrderType = typeName;
            printData.ProductName = typeName;
            printData.TradeType = tradeType;
            printData.OrderSn = orderSn;
            printData.TradeNo = transSn;
            printData.TradePayMethod = payMethod;
            printData.TradeAmount = price.ToF2Price();
            printData.TradeDateTime = tradeDateTime.ToUnifiedFormatDateTime();
            printData.ServiceHotline = BizFactory.AppSettings.ServiceHotline;

            return printData;
        }
    }
}
