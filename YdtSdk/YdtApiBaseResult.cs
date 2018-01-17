using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{
    public class YdtApiBaseResult<T>
    {
        public int code { get; set; }

        public string msg { get; set; }

        public string extmsg { get; set; }

        public T data { get; set; }

    }
}
