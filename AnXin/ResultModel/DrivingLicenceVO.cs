using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    [Serializable]
    public class DrivingLicenceVO
    {
        public string owner { get; set; }

        public string plateNum { get; set; }

        public string address { get; set; }

        public string userCharacter { get; set; }

        public string model { get; set; }

        public string vin { get; set; }

        public string engineNo { get; set; }

        public string registerDate { get; set; }

        public string issueDate { get; set; }

        public string vehicleType { get; set; }
    }
}
