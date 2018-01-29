using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinShunPaySdk
{
    public class PayQuery_Request : IMinShunPayApiPostRequest<PayQuery_Result>
    {
        private Dictionary<string, string> _urlParameters = new Dictionary<string, string>();
        private string _signkey = "";

        public Dictionary<string, string> UrlParameters
        {
            get
            {
                return _urlParameters;
            }
            set
            {
                _urlParameters = value;
            }
        }

        public string SignKey
        {
            get
            {
                return _signkey;
            }
        }

        public PayQuery_Request(PayQuery_Params param)
        {

            _urlParameters.Add("partnerId", param.partnerId);
            _urlParameters.Add("tranCod", param.tranCod);
            _urlParameters.Add("tranType", param.tranType);
            _urlParameters.Add("txnamt", param.txnamt);
            _urlParameters.Add("orderId", param.orderId);
            _urlParameters.Add("mercid", param.mercid);
            _urlParameters.Add("termid", param.termid);
            _urlParameters.Add("spbill_ip", param.spbill_ip);
            _urlParameters.Add("notify_url", param.notify_url);
            _urlParameters.Add("remark", param.remark);
            _urlParameters.Add("orderDate", param.orderDate);
            _urlParameters.Add("orderTime", param.orderTime);

            //_urlParameters.Add

            //this.PostData = postdata;
        }

        public object PostData { get; set; }


        public string ApiName
        {
            get
            {
                return "posm/mspay_scanPay.tran2";
            }
        }
    }
}
