using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{
    public class InsCarInfoModel
    {
        public string licensePlateNo { get; set; }
        public string vin { get; set; }
        public string engineNo { get; set; }
        public string firstRegisterDate { get; set; }
        public string modelCode { get; set; }
        public string modelName { get; set; }
        public int displacement { get; set; }
        public string marketYear { get; set; }
        public int ratedPassengerCapacity { get; set; }
        public decimal replacementValue { get; set; }
        public string chgownerType { get; set; }
        public int chgownerDate { get; set; }
        public int tonnage { get; set; }
        public string wholeWeight { get; set; }
    }
}
