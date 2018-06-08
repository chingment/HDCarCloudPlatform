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

    public class YdtUploadResultData
    {
        public string type { get; set; }

        public YdtUploadFile file { get; set; }

    }


    public class YdtLicenseInfo
    {
        public string owner { get; set; }
        public string plateNum { get; set; }
        public string vehicleType { get; set; }
        public string model { get; set; }
        public string vin { get; set; }
        public string engineNum { get; set; }
        public string registerDate { get; set; }
        public string issueDate { get; set; }

        public string fileKey { get; set; }
    }

    public class YdtUpdateResultDataByLicense
    {
        public string type { get; set; }

        public YdtUploadFile file { get; set; }

        public YdtLicenseInfo license { get; set; }

    }


    public class YdtIdentityInfo
    {
        public string num { get; set; }
        public string name { get; set; }
        public string sex { get; set; }
        public string birthday { get; set; }
        public string nationality { get; set; }
        public string address { get; set; }

        public string fileKey { get; set; }
    }

    public class YdtUpdateResultDataByIdentity
    {
        public string type { get; set; }

        public YdtUploadFile file { get; set; }

        public YdtIdentityInfo identity { get; set; }

    }
}
