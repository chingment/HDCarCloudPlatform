using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class BaseVO
    {
        public string cprovince { set; get; }// 保单归属地（省）440000
        public string cAreaFlag { set; get; }//保单归属地（市）441900
        public string cCountryFlag { set; get; }//保单归属地（县区）999999
        public string cAppointAreaCode { set; get; }//指定查询地区440000
        public string cImmeffMrk { set; get; }//及时生效（交强险）：0-否1-是
        
        
        public BaseVO()
        {
            cprovince = "";
            cAreaFlag = "";
            cCountryFlag = "";
            cAppointAreaCode = "";
            cImmeffMrk = "";
        }
    }
}
