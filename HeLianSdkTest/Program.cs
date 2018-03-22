using HeLianSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeLianSdkTest
{
    class Program
    {
        static void Main(string[] args)
        {
            CarQueryDataList_Params pams = new CarQueryDataList_Params();

            pams.carNo = "YA123456";
            pams.carType = "02";
            pams.enginNo = "654322hotdatacc'";
            pams.rackNo = "654321";
            pams.isCompany = "5";
            HeLianApi.CarQueryDataList(pams);
        }
    }
}
