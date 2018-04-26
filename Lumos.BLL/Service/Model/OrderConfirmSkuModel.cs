using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Model
{
    public class OrderConfirmSkuModel
    {
        public int CartId  {  get;set;  }

        public int SkuId { get; set; }

        public int Quantity { get; set; }

        public string SkuName { get; set; }

        public string SkuMainImg { get; set; }

        public string Price { get; set; }

        public int ChannelId { get; set; }
        public Enumeration.ChannelType ChannelType { get; set; }


    }
}
