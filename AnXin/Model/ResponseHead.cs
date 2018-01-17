using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class ResponseHead
    {
        public string responseCode{set;get;}//0找到数据，2没能找到数据
		public string requestType{set;get;}
		public string errorCode{set;get;}
		public string errorMessage{set;get;}
		public string esbCode{set;get;}
		public string esbMessage{set;get;}
		public string signData{set;get;}


       public  ResponseHead()
        {
            responseCode = "";
            requestType = "";
            errorCode = "";
            errorMessage = "";
            esbCode = "";
            esbMessage = "";
            signData = "";
        }
    }
}
