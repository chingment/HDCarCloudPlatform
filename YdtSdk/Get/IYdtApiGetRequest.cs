using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{

    public interface IYdtApiGetRequest<T> 
    {
        string ApiName { get; }

        IDictionary<string, string> GetUrlParameters();
    }
}
