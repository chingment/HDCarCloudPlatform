using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{
    public class YdtInsCarQueryInsure : IYdtApiGetRequest<YdtInsCarQueryInsureResultData>
    {
        private string token { get; set; }

        private string session { get; set; }

        private string inquirySeq { get; set; }

        private string orderSeq { get; set; }

        private string insureSeq { get; set; }

        public string ApiName
        {
            get
            {
                return "ins_car/query_inquiry";
            }
        }

        public YdtInsCarQueryInsure(string token, string session, string orderSeq, string inquirySeq,string insureSeq)
        {
            this.token = token;
            this.session = session;
            this.inquirySeq = inquirySeq;
            this.orderSeq = orderSeq;
            this.insureSeq = insureSeq;
        }

        public IDictionary<string, string> GetUrlParameters()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("token", this.token);
            parameters.Add("session", this.session);
            parameters.Add("orderSeq", this.orderSeq);
            parameters.Add("inquirySeq", this.inquirySeq);
            parameters.Add("insureSeq", this.insureSeq);
            return parameters;
        }
    }
}
