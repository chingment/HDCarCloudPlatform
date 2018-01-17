using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{
    public interface IYdtApiPostRequest<T> 
    {
        string ApiName { get; }

        IDictionary<string, string> GetUrlParameters();

        YdtPostDataType PostDataTpye { get; set; }

        object PostData { get; set; }
    }
}
