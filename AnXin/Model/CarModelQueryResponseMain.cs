using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{

    //using engineDesc = String;
    public class CarModelQueryResponseMain
    {
        public string pageTotal { set; get; }
        public List<FamilyVO> FamilyList { set; get; }
        public List<BrandVO> BrandList { set; get; }
        public List<string> EngineDescList { set; get; }
        public List<string> GearboxTypeList { set; get; }
        public List<VehicleVO> VehicleList { set; get; }
         public CarModelQueryResponseMain()
        {
            pageTotal = "0";
            FamilyList = new List<FamilyVO>();
            BrandList = new List<BrandVO>();
            EngineDescList = new List<string>();
            GearboxTypeList = new List<string>();
            VehicleList = new List<VehicleVO>();
        }
    }
}
