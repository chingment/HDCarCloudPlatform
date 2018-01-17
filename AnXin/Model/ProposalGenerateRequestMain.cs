using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class ProposalGenerateRequestMain
    {
        public Channel Channel { set; get; }
        public ApplicantVO ApplicantVO { set; get; }
        public InsuredVO InsuredVO { set; get; }
        public VhlownerVO VhlownerVO { set; get; }
        public PackageVOPG PackageJQVO { set; get; }
        public PackageVOPG PackageSYVO { set; get; }
        public DeliveryVO DeliveryVO { set; get; }

        public ProposalGenerateRequestMain()
        {
            Channel = new Channel();
            ApplicantVO = new ApplicantVO();
            InsuredVO = new InsuredVO();
            VhlownerVO = new VhlownerVO();
            PackageJQVO = new PackageVOPG();
            PackageSYVO = new PackageVOPG();
            DeliveryVO = new DeliveryVO();
        }

        
    }
}
