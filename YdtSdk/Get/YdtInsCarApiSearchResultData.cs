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

        public InsCarInfoModel Car { get; set; }

        public List<InsCustomers> CustomerList { get; set; }

        public List<YdtInsCoverageModel> CoverageList { get; set; }
    }
}
