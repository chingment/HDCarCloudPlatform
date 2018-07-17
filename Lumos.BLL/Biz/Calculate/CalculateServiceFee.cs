using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{


    public class CalculateServiceFee
    {
        private decimal _deposit = 300;
        private decimal _mobileTrafficFee = 200;

        private string _version = "2018.04.01";

        private string _remark = "";


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
