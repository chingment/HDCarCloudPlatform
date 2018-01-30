using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity.AppApi
{
    public class QrCodeDownloadParams
    {
        public int UserId { get; set; }

        //public int MerchantId { get; set; }

        //public int PosMachineId { get; set; }

        public string OrderSn { get; set; }

        public Enumeration.OrderPayWay PayWay { get; set; }

        public string TermId { get; set; }

        public string SpbillIp { get; set; }
    }
}
