using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{
    public class YdtInsCarApiSearch : IYdtApiGetRequest<YdtInsCarApiSearchResultData>
    {
        private string token { get; set; }

        private string session { get; set; }

        private string licensePlateNo { get; set; }

        public string ApiName
        {
            get
            {
                return "ins_car/api_search";
            }
        }

        public YdtInsCarApiSearch(string token, string session,string licensePlateNo)
        {
            this.token = token;
            this.session = session;
            this.licensePlateNo = licensePlateNo;
        }

        public IDictionary<string, string> GetUrlParameters()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("token", this.token);
            parameters.Add("session", this.session);
            parameters.Add("licensePlateNo", this.licensePlateNo);
            return parameters;
        }

    }
}
