using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeLianSdk
{
    public class HeLianApiBaseResult<T>
    {
        public string resultCode { get; set; }

        public string resultMsg { get; set; }

        public T  data{ get; set; }

    }
}
