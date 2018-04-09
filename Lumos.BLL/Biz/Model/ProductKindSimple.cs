using System.Collections.Generic;

namespace Lumos.BLL.Biz.Model
{
    public class SuperProductKind:ProductKindSimple
    {
        public SuperProductKind()
        {
            SubProductKinds=new List<SubProductKind>();
        }
        public string MainImg { get; set; }

        public List<SubProductKind> SubProductKinds { get; set; }
    }

    public class ProductKindSimple {
        public string Id { get; set; }

        public string Name { get; set; }

        public string IconImg { get; set; }
    }

    public class SubProductKind: ProductKindSimple
    {
        public string SuperName { get; set; }
        public string PId { get; set; }
    }

}