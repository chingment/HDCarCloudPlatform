using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    public class CarInsInsureInfo
    {
        public CarInsInsureInfo()
        {

        }

        public int Auto { get; set; }

        public CarInsInsureResult InsureInfo { get; set; }

        public CarInsComanyModel OfferInfo { get; set; }

        public CarInsPayResult PayInfo { get; set; }
    }
}
