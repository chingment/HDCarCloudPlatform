using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{
    public class YdtInsCarApiSearchResultData
    {
        public string Belong { get; set; }

        public string BiEndDate { get; set; }

        public string BiStartDate { get; set; }

        public string CiEndDate { get; set; }

        public string CiStartDate { get; set; }

        public YdtInscarInfoModel Car { get; set; }

        public List<YdtInscarCustomerModel> CustomerList { get; set; }

        public List<YdtInscarCoveragesModel> CoverageList { get; set; }
    }
}
