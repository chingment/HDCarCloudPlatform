using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace WebBack.Models.Biz.InsuranceCompany
{
    public class AddViewModel : BaseViewModel
    {
        private Lumos.Entity.InsuranceCompany _insuranceCompany = new Lumos.Entity.InsuranceCompany();

        public Lumos.Entity.InsuranceCompany InsuranceCompany
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