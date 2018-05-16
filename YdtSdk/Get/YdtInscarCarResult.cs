using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{
    public class YdtInscarCarResultData
    {
        public string id
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
        }

        public string modelCode { get; set; }
        public string modelName { get; set; }
        public string displacement { get; set; }
        public string marketYear { get; set; }
        public string ratedPassengerCapacity { get; set; }
        public string replacementValue { get; set; }
    }

}
