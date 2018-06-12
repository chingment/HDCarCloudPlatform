using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{
    public class YdtInscarPayQuery : IYdtApiGetRequest<YdtInscarPayQueryResultData>
    {
        private string token { get; set; }

        private string session { get; set; }

        private string inquirySeq { get; set; }

        private string orderSeq { get; set; }

        private string insureSeq { get; set; }

        private string paySeq { get; set; }

        public string ApiName
        {
            get
            {
                return "ins_car/query_pay";
            }
        }

        public YdtInscarPayQuery(string token, string session, string orderSeq, string inquirySeq, string insureSeq, string paySeq)
        {
            this.token = token;
            this.session = session;
            this.inquirySeq = inquirySeq;
            this.orderSeq = orderSeq;
            this.insureSeq = insureSeq;
            this.paySeq = paySeq;
        }

        public IDictionary<string, string> GetUrlParameters()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("token", this.token);
            parameters.Add("session", this.session);
            parameters.Add("orderSeq", this.orderSeq);
            parameters.Add("inquirySeq", this.inquirySeq);
            parameters.Add("insureSeq", this.insureSeq);
            parameters.Add("paySeq", this.paySeq);
            return parameters;
        }
    }
}
