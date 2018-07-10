using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{
   public class YdtInscarPayByArtificial : IYdtApiPostRequest<YdtInscarPayByArtificialResultData>
    {
        private string token { get; set; }

        private string session { get; set; }

        public YdtInscarPayByArtificial(string token, string session, YdtPostDataType postdatatpye, object postdata)
        {
            this.token = token;
            this.session = session;
            this.PostDataTpye = postdatatpye;
            this.PostData = postdata;
        }

        public YdtPostDataType PostDataTpye { get; set; }


        public object PostData { get; set; }


        public string ApiName
        {
            get
            {
                return "ins_artificial/pay_record";
            }
        }

        public IDictionary<string, string> GetUrlParameters()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("token", this.token);
            parameters.Add("session", this.session);
            return parameters;
        }
    }
}
