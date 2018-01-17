using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{
    public class YdtToken : IYdtApiGetRequest<YdtTokenResultData>
    {
        private string name { get; set; }

        private string password { get; set; }

        public string ApiName
        {
            get
            {
                return "token";
            }
        }

        public YdtToken(string name, string password)
        {
            this.name = name;
            this.password = password;
        }

        public IDictionary<string, string> GetUrlParameters()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("name", this.name);
            parameters.Add("password", this.password);
            return parameters;
        }
    }
}
