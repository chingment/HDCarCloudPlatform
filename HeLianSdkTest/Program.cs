using HeLianSdk;
using Newtonsoft.Json;
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

            pams.carNo = "粤YGY662";
            pams.carType = "02";
            pams.enginNo = "713477";
            pams.rackNo = "004711";
            pams.isCompany = "false";
            var s = HeLianApi.CarQueryDataList(pams);

            List<DataLllegal> r = new List<DataLllegal>();
            if (s != null)
            {
                if (s.data != null)
                {
                    var d = s.data;
                    foreach (var m in d)
                    {
                        DataLllegal dl = new DataLllegal();
                        dl.bookNo = m.bookNo;
                        dl.bookType = m.bookType;
                        dl.cityCode = m.cityCode;
                        dl.lllegalCode = m.lllegalCode;
                        dl.lllegalTime = m.lllegalTime;
                        dl.point = m.point;
                        dl.fine = m.fine;
                        dl.lllegalAddress = m.address;
                        r.Add(dl);
                    }
                }
            }

            CarQueryGetLllegalPrice_Params pams1 = new CarQueryGetLllegalPrice_Params();

            pams1.carNo = "粤YGY662";
            pams1.carType = "02";
            pams1.enginNo = "713477";
            pams1.rackNo = "004711";
            pams1.isCompany = "false";
            pams1.dataLllegal = r;
            var s1 =HeLianApi.CarQueryGetLllegalPrice(pams1);

            Console.ReadLine();

        }
    }
}
