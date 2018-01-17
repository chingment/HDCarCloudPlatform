using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class FamilyVO
    {
        public string familyId { set; get; }
        public string familyName { set; get; }
        public FamilyVO()
        {
            familyId = "";
            familyName = "";
        }
    }
}
