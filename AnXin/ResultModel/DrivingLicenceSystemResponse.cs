using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AnXinSdk
{
    [XmlRootAttribute("CertificateIdentificationSystemResponse")]
    public class DrivingLicenceSystemResponse
    {
        public DrivingLicenceSystemResponseMain CertificateIdentificationSystemResponseMain { get; set; }

    }
}
