using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lumos.Entity
{
    public class CarInsCustomerModel
    {
        public string InsuredFlag { get; set; }
        public string Name { get; set; }
        public string CertNo { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string IdentityFacePicKey { get; set; }

        public string IdentityFacePicUrl{ get; set; }
        public string IdentityBackPicKey { get; set; }

        public string IdentityBackPicUrl { get; set; }
        public string OrgPicKey { get; set; }

        public string OrgPicUrl { get; set; }
    }
}