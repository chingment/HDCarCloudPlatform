using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
     [Serializable]
    public class IdentityCardVO
    {
        public string address { get; set; }

        public string birthday { get; set; }

        public string idNumber { get; set; }

        public string name { get; set; }

        public string people { get; set; }

        public string sex { get; set; }

        public string type { get; set; }

        public string issueAuthority { get; set; }

        public string validity { get; set; }
    }
}
