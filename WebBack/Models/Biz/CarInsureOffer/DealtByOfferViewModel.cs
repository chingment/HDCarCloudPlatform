using Lumos.BLL;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.CarInsureOffer
{
    public class DealtByOfferViewModel : BaseViewModel
    {
        private Lumos.Entity.Merchant _merchant = new Lumos.Entity.Merchant();
        private Lumos.Entity.OrderToCarInsure _orderToCarInsure = new Lumos.Entity.OrderToCarInsure();
        private List<Lumos.Entity.OrderToCarInsureOfferCompany> _orderToCarInsureOfferCompany = new List<Lumos.Entity.OrderToCarInsureOfferCompany>();
        private List<Lumos.Entity.OrderToCarInsureOfferCompanyKind> _orderToCarInsureOfferCompanyKind = new List<Lumos.Entity.OrderToCarInsureOfferCompanyKind>();
        private Lumos.Entity.BizProcessesAudit _bizProcessesAudit = new Lumos.Entity.BizProcessesAudit();


        private bool _isHasCommercialPrice = false;
        private bool _isHasTravelTaxPrice = false;
        private bool _isHasCompulsoryPrice = false;
        public DealtByOfferViewModel()
        {

        }

        public DealtByOfferViewModel(int id)
        {
            var bizProcessesAudit = BizFactory.BizProcessesAudit.ChangeCarInsureStatus(id, Enumeration.CarInsureAuditStatus.InOffer, this.Operater, null, "报价中");
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

                    var orderToCarInsureOfferCompanyKind = CurrentDb.OrderToCarInsureOfferCompanyKind.Where(m => m.OrderId == orderToCarInsure.Id).Select(m => new
                    {

                        m.KindId,
                        m.KindName,
                        m.KindValue,
                        m.IsWaiverDeductible,
                        m.KindDetails,
                        m.KindUnit

                    }).Distinct();





                    var carKinds = CurrentDb.CarKind.ToList();
                    if (orderToCarInsureOfferCompanyKind != null)
                    {

                        _orderToCarInsureOfferCompanyKind = new List<Lumos.Entity.OrderToCarInsureOfferCompanyKind>();

                        foreach (var item in orderToCarInsureOfferCompanyKind)
                        {

                            _orderToCarInsureOfferCompanyKind.Add(new Lumos.Entity.OrderToCarInsureOfferCompanyKind
                            {
                                KindId = item.KindId,
                                KindName = item.KindName,
                                KindValue = item.KindValue,
                                IsWaiverDeductible = item.IsWaiverDeductible,
                                KindDetails = item.KindDetails,
                                KindUnit = item.KindUnit
                            });

                        }

                        var isHasCompulsoryPrice = _orderToCarInsureOfferCompanyKind.Where(m => m.KindId == 1).FirstOrDefault();
                        if (isHasCompulsoryPrice != null)
                        {
                            _isHasCompulsoryPrice = true;
                        }

                        var isHasTravelTaxPrice = _orderToCarInsureOfferCompanyKind.Where(m => m.KindId == 2).FirstOrDefault();
                        if (isHasTravelTaxPrice != null)
                        {
                            _isHasTravelTaxPrice = true;
                        }

                        var isHasCommercialPrice = _orderToCarInsureOfferCompanyKind.Where(m => m.KindId >= 3).FirstOrDefault();
                        if (isHasCommercialPrice != null)
                        {
                            _isHasCommercialPrice = true;
                        }


                        foreach (var m in _orderToCarInsureOfferCompanyKind)
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