using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FangWeiSdk
{


    public class FangWeiUtils
    {
        public static CustomJsonResult GetVehiclesInfo(string plateNumber, string vin)
        {
            FangWeiApi c = new FangWeiApi();
            FangWeiVehiclesInfo ydtToken = new FangWeiVehiclesInfo(plateNumber, vin);
            var ydtTokenResult = c.DoGet(ydtToken);
            return ydtTokenResult;

        }
    }
}
