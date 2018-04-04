using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public enum SnType
    {
        OrderToCarInsure = 1,
        FundTrans = 3,
        OrderToCarClaim = 4,
        OrderToServiceFee = 5,
        OrderToTalentDemand = 6,
        OrderToApplyLossAssess = 7,
        LllegalQueryScoreTrans = 8,
        OrderToLllegalQueryRecharge = 9,
        OrderToLllegalDealt = 10,
        OrderPayTrans=11,
        OrderToCredit=12,
        OrderToInsurance = 13

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
            //switch (type)
            //{
            //    case SnType.CarInsure:
            //        prefix = "A";
            //        break;
            //    case SnType.CarClaim:
            //        prefix = "B";
            //        break;
            //    case SnType.Withdraw:
            //        prefix = "C";
            //        break;
            //    case SnType.FundTrans:
            //        prefix = "D";
            //        break;
            //    case SnType.ServiceFee:
            //        prefix = "E";
            //        break;
            //    case SnType.TalentDemand:
            //        prefix = "F";
            //        break;
            //    case SnType.ApplyLossAssess:
            //        prefix = "G";
            //        break;
            //    case SnType.LllegalQueryScoreTrans:
            //        prefix = "H";
            //        break;
            //    case SnType.LllegalQueryRecharge:
            //        prefix = "I";
            //        break;
            //    case SnType.LllegalDealt:
            //        prefix = "J";
            //        break;
            //}

            string dateTime = DateTime.Now.ToString("yyMMddHHmm");

            string sId = id.ToString().PadLeft(10, '0');

            string sn = prefix + dateTime + sId;

            model.Sn = sn;
            //model.TradeSnByWechat = string.Format("{0}W", sn);
            //model.TradeSnByAlipay = string.Format("{0}A", sn);
            return model;
        }
    }
}
