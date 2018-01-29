using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinShunPaySdk
{
    public class CodeDownload_Request : IMinShunPayApiPostRequest<CodeDownload_Result>
    {
        private Dictionary<string, string> _urlParameters=  new Dictionary<string, string>();


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

        public CodeDownload_Request(CodeDownload_Params param)
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
