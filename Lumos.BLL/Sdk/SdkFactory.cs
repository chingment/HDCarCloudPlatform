using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class SdkFactory
    {
        public static FangWeiProvider FangWei
        {
            get
            {
                return new FangWeiProvider();
            }
        }

        public static MinShunPayProvider MinShunPay
        {
            get
            {
                return new MinShunPayProvider();
            }
        }

        public static HeLianProvider HeLian
        {
            get
            {
                return new HeLianProvider();
            }
        }
    }
}
