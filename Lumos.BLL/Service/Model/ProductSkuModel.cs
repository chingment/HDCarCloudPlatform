using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Model
{
    public class ProductSkuModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public string MainImg { get; set; }
        public List<Lumos.Entity.ImgSet> DispalyImgs { get; set; }
        public ProductSkuModel()
        {

        }
    }
}
