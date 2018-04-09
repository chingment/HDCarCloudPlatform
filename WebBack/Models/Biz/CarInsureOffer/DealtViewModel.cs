using Lumos.BLL;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.CarInsureOffer
{
    public class DealtViewModel : BaseViewModel
    {
        private Lumos.Entity.Merchant _merchant = new Lumos.Entity.Merchant();
        private Lumos.Entity.OrderToCarInsure _orderToCarInsure = new Lumos.Entity.OrderToCarInsure();
        private List<Lumos.Entity.OrderToCarInsureOfferCompany> _orderToCarInsureOfferCompany = new List<Lumos.Entity.OrderToCarInsureOfferCompany>();
        private List<Lumos.Entity.OrderToCarInsureOfferKind> _orderToCarInsureOfferKind = new List<Lumos.Entity.OrderToCarInsureOfferKind>();
        private Lumos.Entity.BizProcessesAudit _bizProcessesAudit = new Lumos.Entity.BizProcessesAudit();


        private bool _isHasCommercialPrice = false;
        private bool _isHasTravelTaxPrice = false;
        private bool _isHasCompulsoryPrice = false;
        public DealtViewModel()
        {

        }

        public DealtViewModel(int id)
        {
            var bizProcessesAudit = BizFactory.BizProcessesAudit.ChangeCarInsureOfferDealtStatus(this.Operater, id, Enumeration.CarInsureOfferDealtStatus.InOffer, "报价中");
            if (bizProcessesAudit != null)
            {
                _bizProcessesAudit = bizProcessesAudit;

                if (_bizProcessesAudit.Auditor.Value != this.Operater)
                {
                    this.IsHasOperater = true;
                    this.OperaterName = SysFactory.SysUser.GetFullName(_bizProcessesAudit.Auditor.Value);
                }



                var orderToCarInsure = CurrentDb.OrderToCarInsure.Where(m => m.Id == bizProcessesAudit.AduitReferenceId).FirstOrDefault();
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

                    var orderToCarInsureOfferKind = CurrentDb.OrderToCarInsureOfferKind.Where(m => m.OrderId == orderToCarInsure.Id).ToList();
                    var carKinds = CurrentDb.CarKind.ToList();
                    if (orderToCarInsureOfferKind != null)
                    {
                        _orderToCarInsureOfferKind = orderToCarInsureOfferKind;

                        var isHasCompulsoryPrice = _orderToCarInsureOfferKind.Where(m => m.KindId == 1).FirstOrDefault();
                        if (isHasCompulsoryPrice != null)
                        {
                            _isHasCompulsoryPrice = true;
                        }

                        var isHasTravelTaxPrice = _orderToCarInsureOfferKind.Where(m => m.KindId == 2).FirstOrDefault();
                        if (isHasTravelTaxPrice != null)
                        {
                            _isHasTravelTaxPrice = true;
                        }

                        var isHasCommercialPrice = _orderToCarInsureOfferKind.Where(m => m.KindId >= 3).FirstOrDefault();
                        if (isHasCommercialPrice != null)
                        {
                            _isHasCommercialPrice = true;
                        }


                        foreach (var m in _orderToCarInsureOfferKind)
                        {
                            var carKind = carKinds.Where(q => q.Id == m.KindId).FirstOrDefault();
                            if (carKind != null)
                            {
                                m.KindName = carKind.Name;
                                m.KindUnit = carKind.InputUnit;
                            }
                        }
                    }
                }

            }

        }

        public bool IsHasCommercialPrice
        {
            get
            {
                return _isHasCommercialPrice;
            }
            set
            {
                _isHasCommercialPrice = value;
            }
        }

        public bool IsHasCravelTaxPrice
        {
            get
            {
                return _isHasTravelTaxPrice;
            }
            set
            {
                _isHasTravelTaxPrice = value;
            }
        }
        public bool IsHasCompulsoryPrice
        {
            get
            {
                return _isHasCompulsoryPrice;
            }
            set
            {
                _isHasCompulsoryPrice = value;
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


        public Lumos.Entity.BizProcessesAudit BizProcessesAudit
        {
            get
            {
                return _bizProcessesAudit;
            }
            set
            {
                _bizProcessesAudit = value;
            }
        }

    }

}