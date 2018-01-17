using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class CarQuotePriceResponseMain
    {


        public PackageVOCarQuotePriceResponse PackageJQVO { set; get; }
        public PackageVOCarQuotePriceResponse PackageSYVO { set; get; }
        public CarQuotePriceResponseMain()
        {
            PackageJQVO = new PackageVOCarQuotePriceResponse();
            PackageSYVO = new PackageVOCarQuotePriceResponse();
        }
    }
}
