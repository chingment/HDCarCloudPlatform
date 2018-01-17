using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class ProposalGenerateResponseMain
    {
        public PackageVOPGResponse PackageJQVO { set; get; }
        public PackageVOPGResponse PackageSYVO { set; get; }
        public ProposalGenerateResponseMain()
        {
            PackageJQVO = new PackageVOPGResponse();
            PackageSYVO = new PackageVOPGResponse();
        }
    }
}
