using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lumos.Entity
{

    public class ItemParentField
    {
        public ItemParentField()
        {
            this.Child = new List<ItemChildField>();
        }

        public ItemParentField(string field, string value)
        {
            this.Field = field;
            this.Value = value;
            this.Child = new List<ItemChildField>();
        }

        public string Field { get; set; }

        public string Value { get; set; }

        public List<ItemChildField> Child { get; set; }
    }

    public class ItemChildField
    {
        public ItemChildField()
        {

        }

        public ItemChildField(string field, string value)
        {
            this.Field = field;
            this.Value = value;
        }

        public string Field { get; set; }

        public string Value { get; set; }
    }

    public class CarInsInquiryResult
    {
        public CarInsInquiryResult()
        {
            this.InsureItem = new List<ItemParentField>();
            this.Company = new Channel();
        }

        public Channel Company { get; set; }

        public List<ItemParentField> InsureItem { get; set; }

        public decimal SumPremium { get; set; }

        public string OrderSeq { get; set; }

        public string InquirySeq { get; set; }
    }
}