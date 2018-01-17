using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{
    public class YdtUploadFile
    {
        public string key { get; set; }

        public int size { get; set; }
    }

    public class YdtUpdateResultData
    {
        public string type { get; set; }

        public YdtUploadFile file { get; set; }
    }
}
