using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Model
{
    public class CartProcudtSkuListByOperateModel
    {
        public int SkuId { get; set; }
        public int Quantity { get; set; }
        public bool Selected { get; set; }
        public int ChannelId { get; set; }
        public Entity.Enumeration.ChannelType ChannelType { get; set; }
    }
}
