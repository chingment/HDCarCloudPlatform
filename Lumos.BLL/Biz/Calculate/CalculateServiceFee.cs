using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{


    public class CalculateServiceFee
    {
        private decimal _deposit = 800;
        private decimal _mobileTrafficFee = 200;

        private string _version = "2018.04.01";

        private string _remark = "公告：在优惠活动期间激活的亲，如果选择租期6个月可获得100元的减免优惠，即1100元；选择12个月的租期可获得200元的减免优惠，即2200元！";


        public string Version
        {
            get
            {
                return _version;
            }

        }

        public string Remark
        {
            get
            {
                return _remark;
            }

        }

        public decimal Deposit
        {
            get
            {
                return _deposit;
            }
        }

        public decimal MobileTrafficFee
        {
            get
            {
                return _mobileTrafficFee;
            }
        }


        public CalculateServiceFee()
        {
 
        }


    }
}
