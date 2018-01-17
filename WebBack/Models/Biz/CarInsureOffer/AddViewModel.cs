using Lumos.BLL;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace WebBack.Models.Biz.CarInsureOffer
{
    public class AddViewModel : BaseViewModel
    {
        private Lumos.Entity.Merchant _merchant = new Lumos.Entity.Merchant();
        private Lumos.Entity.OrderToCarInsure _orderToCarInsure = new Lumos.Entity.OrderToCarInsure();
        private List<Lumos.Entity.CarInsuranceCompany> _carInsuranceCompany = new List<Lumos.Entity.CarInsuranceCompany>();
        private List<Lumos.Entity.OrderToCarInsureOfferCompany> _orderToCarInsureOfferCompany = new List<Lumos.Entity.OrderToCarInsureOfferCompany>();
        private List<Lumos.Entity.OrderToCarInsureOfferKind> _orderToCarInsureOfferKind = new List<Lumos.Entity.OrderToCarInsureOfferKind>();
        private List<InsurePlanKindModel> _insurePlanKinds;


        public AddViewModel()
        {

        }

        public void GetModel()
        {
            var carInsurePlans = CurrentDb.CarKind.ToList();

            List<InsurePlanKindModel> insurePlanKindModels = new List<InsurePlanKindModel>();

            foreach (var m in carInsurePlans)
            {
                var carKind = CurrentDb.CarKind.Where(c => c.Id == m.Id).FirstOrDefault();
                InsurePlanKindModel insurePlanKindModel = new InsurePlanKindModel();
                insurePlanKindModel.Id = carKind.Id;
                insurePlanKindModel.Name = carKind.Name;
                insurePlanKindModel.AliasName = carKind.AliasName;
                insurePlanKindModel.Type = carKind.Type;
                insurePlanKindModel.CanWaiverDeductible = carKind.CanWaiverDeductible;
                insurePlanKindModel.InputType = carKind.InputType;
                insurePlanKindModel.InputUnit = carKind.InputUnit;
                if (!string.IsNullOrEmpty(carKind.InputValue))
                {
                    insurePlanKindModel.InputValue = Newtonsoft.Json.JsonConvert.DeserializeObject(carKind.InputValue);
                }

                insurePlanKindModel.IsHasDetails = carKind.IsHasDetails;

                insurePlanKindModels.Add(insurePlanKindModel);

                _insurePlanKinds = insurePlanKindModels;
            }

            var carInsuranceCompany = CurrentDb.CarInsuranceCompany.Where(m => m.Status == Enumeration.CarInsuranceCompanyStatus.Normal).ToList();

            if (carInsuranceCompany != null)
            {
                _carInsuranceCompany = carInsuranceCompany;
            }
        }

        public List<Lumos.Entity.CarInsuranceCompany> CarInsuranceCompany
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


        public List<InsurePlanKindModel> InsurePlanKinds
        {
            get
            {
                return _insurePlanKinds;
            }
            set
            {
                _insurePlanKinds = value;
            }
        }

        public Lumos.Entity.Merchant Merchant
        {
            get
            {
                return _merchant;
            }
            set
            {
                _merchant = value;
            }
        }

        public Lumos.Entity.OrderToCarInsure OrderToCarInsure
        {
            get
            {
                return _orderToCarInsure;
            }
            set
            {
                _orderToCarInsure = value;
            }
        }

        public List<Lumos.Entity.OrderToCarInsureOfferCompany> OrderToCarInsureOfferCompany
        {
            get
            {
                return _orderToCarInsureOfferCompany;
            }
            set
            {
                _orderToCarInsureOfferCompany = value;
            }
        }

        public List<Lumos.Entity.OrderToCarInsureOfferKind> OrderToCarInsureOfferKind
        {
            get
            {
                return _orderToCarInsureOfferKind;
            }
            set
            {
                _orderToCarInsureOfferKind = value;
            }
        }

    }
}