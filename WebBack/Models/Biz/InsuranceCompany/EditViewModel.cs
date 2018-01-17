using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.InsuranceCompany
{
    public class EditViewModel:BaseViewModel
    {
        private Lumos.Entity.InsuranceCompany _insuranceCompany = new Lumos.Entity.InsuranceCompany();
        private string _creator = "";
        private string _mender = "";
        public string Creator
        {
            get
            {
                return _creator;
            }
            set
            {
                _creator = value;
            }
        }
        public string Mender
        {
            get
            {
                return _mender;
            }
            set
            {
                _mender = value;
            }
        }


        public EditViewModel()
        {

        }

        public EditViewModel(int id)
        {
            //var insuranceCompany = CurrentDb.InsuranceCompany.Where(m => m.Id == id).FirstOrDefault();

            var query = (from i in CurrentDb.InsuranceCompany
                         join u1 in CurrentDb.SysStaffUser on i.Creator equals u1.Id into tmp1
                         from tt1 in tmp1.DefaultIfEmpty()
                         join u2 in CurrentDb.SysStaffUser on i.Mender equals u2.Id into tmp2
                         from tt2 in tmp2.DefaultIfEmpty()
                         where i.Id.Equals(id)


             select new { i.Id, i.ImgUrl, Creator = tt1 == null ? "" : tt1.FullName, i.Name, i.LastUpdateTime, Mender = tt2 == null ? "" : tt2.FullName, i.CreateTime }).FirstOrDefault();

            var insuranceCompany =   new Lumos.Entity.InsuranceCompany();
            insuranceCompany.Id = query.Id;
            insuranceCompany.Name = query.Name;
            insuranceCompany.ImgUrl = query.ImgUrl;
            insuranceCompany.CreateTime = query.CreateTime;
            insuranceCompany.LastUpdateTime = query.LastUpdateTime;
            _creator = query.Creator;
            _mender = query.Mender;


            if (insuranceCompany != null)
            {
                _insuranceCompany = insuranceCompany;
            }
        }

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
    }
}