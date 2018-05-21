using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarPaySdk
{
    public interface IStarPayPostRequest<T> where T : StarPayBaseResult
    {
        string ApiName { get; }

        object PostData { get; set; }

    }
}
