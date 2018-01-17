using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FangWeiSdk
{
    public class FangWeiVehiclesInfo : IFangWeiApiGetRequest<EndDateModel>
    {
        private string plateType { get; set; }

        private string plateNumber { get; set; }

        private string VIN { get; set; }


        public string ApiName
        {
            get
            {
                return "QXT/vehicles/information/endDate";
            }
        }

        public FangWeiVehiclesInfo(string plateNumber, string vin)
        {
            this.plateType = "02";
            this.plateNumber = plateNumber;
            this.VIN = vin;
        }

        public IDictionary<string, string> GetUrlParameters()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("plateType", this.plateType);
            parameters.Add("plateNumber", this.plateNumber);
            parameters.Add("VIN", this.VIN);
            return parameters;
        }
    }
}
