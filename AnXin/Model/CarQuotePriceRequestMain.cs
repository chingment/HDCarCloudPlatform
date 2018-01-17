using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class CarQuotePriceRequestMain
    {
        public Channel Channel { set; get; }
        public BaseVO BaseVO { set; get; }
        public InsuredVO InsuredVO { set; get; }
        public VhlownerVO VhlownerVO { set; get; }
        public PackageVO PackageJQVO { set; get; }
        public PackageVO PackageSYVO { set; get; }


        public CarQuotePriceRequestMain()
        {
            Channel = new Channel();
            BaseVO = new BaseVO();
            InsuredVO = new InsuredVO();
            VhlownerVO = new VhlownerVO();
            PackageJQVO = new PackageVO();
            PackageSYVO = new PackageVO();
            
        }

    }
}
    
