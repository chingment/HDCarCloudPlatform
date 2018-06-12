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
}