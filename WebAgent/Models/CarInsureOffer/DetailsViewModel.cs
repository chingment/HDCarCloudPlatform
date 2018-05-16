using Lumos.BLL;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAgent.Models.CarInsureOffer
{
    public class DetailsViewModel : BaseViewModel
    {
        private Lumos.Entity.Merchant _merchant = new Lumos.Entity.Merchant();
        private Lumos.Entity.OrderToCarInsure _orderToCarInsure = new Lumos.Entity.OrderToCarInsure();
        private List<Lumos.Entity.OrderToCarInsureOfferCompany> _orderToCarInsureOfferCompany = new List<Lumos.Entity.OrderToCarInsureOfferCompany>();
        private List<Lumos.Entity.OrderToCarInsureOfferCompanyKind> _orderToCarInsureOfferCompanyKind = new List<Lumos.Entity.OrderToCarInsureOfferCompanyKind>();

        private List<Lumos.Entity.BizProcessesAuditDetails> _bizProcessesAuditDetails = new List<Lumos.Entity.BizProcessesAuditDetails>();


        public DetailsViewModel()
        {

        }

        public DetailsViewModel(int id)
        {

            var orderToCarInsure = CurrentDb.OrderToCarInsure.Where(m => m.Id == id).FirstOrDefault();
            if (orderToCarInsure != null)
            {
                _orderToCarInsure = orderToCarInsure;

                var merchant = CurrentDb.Merchant.Where(m => m.Id == orderToCarInsure.MerchantId).FirstOrDefault();
                if (merchant != null)
                {
                    _merchant = merchant;
                }

                var orderToCarInsureOfferCompany = CurrentDb.OrderToCarInsureOfferCompany.Where(m => m.OrderId == orderToCarInsure.Id).ToList();
                var insureOfferCompanys = CurrentDb.Company.ToList();
                if (orderToCarInsureOfferCompany != null)
                {
                    _orderToCarInsureOfferCompany = orderToCarInsureOfferCompany;

                    foreach (var m in _orderToCarInsureOfferCompany)
                    {
                        var insureOfferCompany = insureOfferCompanys.Where(q => q.Id == m.InsuranceCompanyId).FirstOrDefault();
                        if (insureOfferCompany != null)
                        {
                            m.InsuranceCompanyName = insureOfferCompany.Name;
                            m.InsuranceCompanyImgUrl = insureOfferCompany.ImgUrl;

                        }
                    }

                }

                var orderToCarInsureOfferCompanyKind = CurrentDb.OrderToCarInsureOfferCompanyKind.Where(m => m.OrderId == orderToCarInsure.Id).ToList();
                var carKinds = CurrentDb.CarKind.ToList();
                if (orderToCarInsureOfferCompanyKind != null)
                {
                    _orderToCarInsureOfferCompanyKind = orderToCarInsureOfferCompanyKind;

                    foreach (var m in orderToCarInsureOfferCompanyKind)
                    {
                        var carKind = carKinds.Where(q => q.Id == m.KindId).FirstOrDefault();
                        if (carKind != null)
                        {
                            m.KindName = carKind.Name;
                            m.KindUnit = carKind.InputUnit;
                        }
                    }
                }

                _bizProcessesAuditDetails = BizFactory.BizProcessesAudit.GetDetails(Enumeration.BizProcessesAuditType.OrderToCarInsure, id);
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

        public List<Lumos.Entity.OrderToCarInsureOfferCompanyKind> OrderToCarInsureOfferCompanyKind
        {
            get
            {
                return _orderToCarInsureOfferCompanyKind;
            }
            set
            {
                _orderToCarInsureOfferCompanyKind = value;
            }
        }

        public List<Lumos.Entity.BizProcessesAuditDetails> BizProcessesAuditDetails
        {
            get
            {
                return _bizProcessesAuditDetails;
            }
            set
            {
                _bizProcessesAuditDetails = value;
            }
        }

    }
}