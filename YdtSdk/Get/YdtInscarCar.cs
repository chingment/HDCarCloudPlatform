using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YdtSdk
{
    public class YdtInscarCar : IYdtApiGetRequest<List<YdtInscarCarResultData>>
    {

        private string token { get; set; }

        private string session { get; set; }

        private string keyword { get; set; }

        private string vin { get; set; }

        private string firstRegisterDate { get; set; }


        public string ApiName
        {
            get
            {
                return "ins_car/car";
            }
        }

        public YdtInscarCar(string token, string session, string keyword, string vin, string firstRegisterDate)
        {
            this.token = token;
            this.session = session;
            this.keyword = keyword;
            this.vin = vin;
            this.firstRegisterDate = firstRegisterDate;
        }

        public IDictionary<string, string> GetUrlParameters()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("token", this.token);
            parameters.Add("session", this.session);
            parameters.Add("keyword", this.keyword);
            parameters.Add("vin", this.vin);
            parameters.Add("firstRegisterDate", this.firstRegisterDate);
            return parameters;
        }
    }
}
