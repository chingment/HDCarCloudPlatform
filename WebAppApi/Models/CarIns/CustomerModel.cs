using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi.Models.CarIns
{
    public class CustomerModel
    {
        public string InsuredFlag { get; set; }
        public string Name { get; set; }
        public string CertNo { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string IdentityFacePicKey { get; set; }
        public string IdentityBackPicKey { get; set; }
        public string OrgPicKey { get; set; }
    }
}