using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinShunPaySdk
{
    public interface IMinShunPayApiPostRequest<T> where T : MinShunPayApiBaseResult
    {
        string ApiName { get; }

        Dictionary<string, string> UrlParameters { get; set; }

        object PostData { get; set; }

    }
}
