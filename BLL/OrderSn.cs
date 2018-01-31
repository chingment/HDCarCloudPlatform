using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public enum SnType
    {
        CarInsure = 1,
        Withdraw = 2,
        Transactions = 3,
        CarClaim = 4,
        ServiceFee = 5,
        TalentDemand = 6,
        ApplyLossAssess = 7
    }

    public class SnModel
    {
        public string Sn { get; set; }
        public string TradeSnByWechat { get; set; }
        public string TradeSnByAlipay { get; set; }
    }

    public class Sn
    {

        public static SnModel Build(SnType type, int id)
        {
            SnModel model = new SnModel();
            string prefix = "";
            switch (type)
            {
                case SnType.CarInsure:
                    prefix = "I";
                    break;
                case SnType.CarClaim:
                    prefix = "C";
                    break;
                case SnType.Withdraw:
                    prefix = "W";
                    break;
                case SnType.Transactions:
                    prefix = "T";
                    break;
                case SnType.ServiceFee:
                    prefix = "D";
                    break;
                case SnType.TalentDemand:
                    prefix = "M";
                    break;
                case SnType.ApplyLossAssess:
                    prefix = "L";
                    break;
            }

            string dateTime = DateTime.Now.ToString("yyMMddHHmm");

            string sId = id.ToString().PadLeft(8, '0');

            string sn = prefix + dateTime + sId;

            model.Sn = sn;
            model.TradeSnByWechat = string.Format("{0}W", sn);
            model.TradeSnByAlipay = string.Format("{0}A", sn);
            return model;
        }
    }
}
