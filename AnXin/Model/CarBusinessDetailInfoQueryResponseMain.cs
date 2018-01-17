using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class CarBusinessDetailInfoQueryResponseMain
    {
        public BaseVOCarBusinessDetailInfoQueryResponse BaseVO { set; get; }
        public ApplicantVO ApplicantVO { set; get; }
        public InsuredVO InsuredVO { set; get; }
        public List<CvrgVO> CvrgList { set; get; }
        public VhlownerVO VhlownerVO { set; get; }
        public VhlVO VhlVO { set; get; }
        public VsTaxVO VsTaxVO { set; get; }
        public PayVO PayVO { set; get; }
        public CarBusinessDetailInfoQueryResponseMain()
        {
            BaseVO = new BaseVOCarBusinessDetailInfoQueryResponse();
            ApplicantVO = new ApplicantVO();
            InsuredVO = new InsuredVO();
            CvrgList = new List<CvrgVO>();
            VhlownerVO = new VhlownerVO();
            VhlVO = new VhlVO();
            VsTaxVO = new VsTaxVO();
            PayVO = new PayVO();
        }

    }
}
