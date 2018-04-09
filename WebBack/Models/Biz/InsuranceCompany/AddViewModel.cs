using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace WebBack.Models.Biz.InsuranceCompany
{
    public class AddViewModel : BaseViewModel
    {
        private Lumos.Entity.Company _insuranceCompany = new Lumos.Entity.Company();

        public Lumos.Entity.Company InsuranceCompany
        {
            get
            {
                return _insuranceCompany;
            }
            set
            {
                _insuranceCompany = value;
            }
        }

        public AddViewModel()
        {

        }

    }
}