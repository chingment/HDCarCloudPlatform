using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FangWeiSdk
{

    public interface IFangWeiApiGetRequest<T> 
    {
        string ApiName { get; }

        IDictionary<string, string> GetUrlParameters();
    }
}
