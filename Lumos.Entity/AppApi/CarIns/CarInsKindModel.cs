using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lumos.Entity
{
    public class CarInsKindModel
    {
        public int Id { get; set; }
        public int PId { get; set; }
        public String Name { get; set; }
        public String AliasName { get; set; }
        public bool IsCheck { get; set; }// 默认投保
        public bool CanWaiverDeductible { get; set; }
        public bool IsWaiverDeductible { get; set; }
        public Enumeration.CarKindType Type { get; set; }
        public Enumeration.CarKindInputType InputType { get; set; }
        public String InputUnit { get; set; }
        public string InputValue { get; set; }
        public List<string> InputOption { get; set; }
        public bool IsHasDetails { get; set; }
    }
}