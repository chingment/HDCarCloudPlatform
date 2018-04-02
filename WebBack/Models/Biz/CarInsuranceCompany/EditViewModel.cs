using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.CarInsuranceCompany
{
    public class EditViewModel : BaseViewModel
    {
        private Lumos.Entity.CarInsuranceCompany _carInsuranceCompany = new Lumos.Entity.CarInsuranceCompany();

        public Lumos.Entity.CarInsuranceCompany CarInsuranceCompany
        {
            get
            {
                return _carInsuranceCompany;
            }
            set
            {
                _carInsuranceCompany = value;
            }
        }


        public EditViewModel()
        {

        }

        public void LoadData(int id)
        {
            var carInsuranceCompany = CurrentDb.CarInsuranceCompany.Where(m => m.Id == id).FirstOrDefault();

            if (carInsuranceCompany != null)
            {
                _carInsuranceCompany = carInsuranceCompany;
            }
        }
    }
}