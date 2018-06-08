using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{
    public class YdtUpload : IYdtApiPostRequest<YdtUploadResultData>
    {
        private string token { get; set; }

        private string session { get; set; }

        private string type { get; set; }

        public YdtUpload(string token, string session,string type, string url)
        {
            this.token = token;
            this.session = session;
            this.type = type;

        
            System.Net.WebRequest webreq = System.Net.WebRequest.Create(url);
            System.Net.WebResponse webres = webreq.GetResponse();
            System.IO.Stream fileStream = webres.GetResponseStream();

            this.PostData = fileStream;
        }

        public YdtPostDataType PostDataTpye { get; set; }


        public object PostData { get; set; }


        public string ApiName
        {
            get
            {
                return "upload";
            }
        }

        public IDictionary<string, string> GetUrlParameters()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("token", this.token);
            parameters.Add("session", this.session);
            parameters.Add("type", this.type);
            return parameters;
        }
    }
}
