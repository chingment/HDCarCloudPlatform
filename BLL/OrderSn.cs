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
        DepositRent = 5,
        TalentDemand = 6,
        ApplyLossAssess = 7
    }

    public class Sn
    {

        public static string Build(SnType type, int id)
        {
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
                case SnType.DepositRent:
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

            string sId = id.ToString().PadLeft(9, '0');

            string sn = prefix + dateTime + sId;

            return sn;
        }
    }
}
