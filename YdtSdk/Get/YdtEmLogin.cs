using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{
    public class YdtEmLogin : IYdtApiGetRequest<YdtEmLoginResultData>
    {
        private string token { get; set; }

        private string mobile { get; set; }

        private string password { get; set; }


        public string ApiName
        {
            get
            {
                return "em/login";
            }
        }

        public YdtEmLogin(string token,string mobile, string password)
        {
            this.token = token;
            this.mobile = mobile;
            this.password = password;
        }

        public IDictionary<string, string> GetUrlParameters()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("token", this.token);
            parameters.Add("mobile", this.mobile);
            parameters.Add("password", this.password);
            return parameters;
        }
    }
}
