using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class CalculateWithdrawFee
    {
        private string _feeRateRule = "16.11.15";
        private decimal _fee = 0;
        private decimal _amount = 0;
        private decimal _amountByAfterFee = 0;
        public string FeeRateRule
        {
            get
            {
                return _feeRateRule;
            }
        }

        public decimal Fee
        {
            get
            {
                return _fee;
            }
        }

        public decimal Amount
        {
            get
            {
                return _amount;
            }
        }

        public decimal AmountByAfterFee
        {
            get
            {
                return _amountByAfterFee;
            }
        }

        public CalculateWithdrawFee(decimal withdrawAmount)
        {
            _amount = withdrawAmount;


            _fee = 0;

            if (_fee > _amount)
            {
                _fee = _amount;
            }

            _amountByAfterFee = _amount - _fee;

        }
    }
}
