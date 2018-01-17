using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{
    public class YdtInscarInquiryOffer
    {
        public int UplinkInsCompanyId { get; set; }

        public string YdtInsCompanyId { get; set; }

        public YdtApiBaseResult<YdtInscarInquiryResultData> Inquiry { get; set; }


        public decimal CommercialPrice
        {
            get
            {
                decimal price = 0;
                if (this.Inquiry != null)
                {
                    if (this.Inquiry.data != null)
                    {
                        if (this.Inquiry.data.inquirys != null)
                        {
                            var d = this.Inquiry.data.inquirys.Where(m => m.risk == 1).Sum(m => m.standardPremium);
                            if (d != null)
                            {
                                price = d.Value;
                            }
                        }
                    }
                }

                return price;
            }
        }

        public decimal TravelTaxPrice
        {
            get
            {
                decimal price = 0;
                if (this.Inquiry != null)
                {
                    if (this.Inquiry.data != null)
                    {
                        if (this.Inquiry.data.inquirys != null)
                        {
                            var d = this.Inquiry.data.inquirys.Where(m => m.risk == 2).Sum(m => m.sumPayTax);
                            if (d != null)
                            {
                                price = d.Value;
                            }
                        }
                    }
                }

                return price;
            }
        }

        public decimal CompulsoryPrice
        {
            get
            {
                decimal price = 0;
                if (this.Inquiry != null)
                {
                    if (this.Inquiry.data != null)
                    {
                        if (this.Inquiry.data.inquirys != null)
                        {
                            var d = this.Inquiry.data.inquirys.Where(m => m.risk == 2).FirstOrDefault();
                            if (d != null)
                            {
                                price = d.standardPremium.Value - d.sumPayTax.Value;
                            }
                        }
                    }
                }

                return price;
            }
        }

        public decimal SumPrice
        {
            get
            {
                decimal price = 0;
                if (this.Inquiry != null)
                {
                    if (this.Inquiry.data != null)
                    {
                        if (this.Inquiry.data.inquirys != null)
                        {
                            var d = this.Inquiry.data.inquirys.Sum(m=>m.standardPremium);
                            if (d != null)
                            {
                                price = d.Value;
                            }
                        }
                    }
                }

                return price;
            }
        }

        public string OfferImgUrl { get; set; }

    }
}
