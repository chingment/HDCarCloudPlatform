using FangWeiSdk;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class FangWeiProvider : BaseProvider
    {
        public CustomJsonResult GetVehiclesInfo(int operater, string clientCode, string plateNumber, string vin)
        {

            CustomJsonResult result = new CustomJsonResult();

            var expireDate = CurrentDb.CarInsuranceExpireDate.Where(m => m.CarPlateNo == plateNumber && m.CarVinLast6Num == vin).FirstOrDefault();

            bool isGoGet = false;

            if (expireDate == null)
            {
                isGoGet = true;
            }

            else {
                if (expireDate.InsuranceEndDate < this.DateTime)
                {
                    isGoGet = true;
                }

                if (expireDate.ValidityEndDate < this.DateTime)
                {
                    isGoGet = true;
                }
            }

            if (isGoGet)
            {
                result = FangWeiUtils.GetVehiclesInfo(plateNumber, vin);
                if (result.Result == ResultType.Success)
                {
                    var endDateModel = (EndDateModel)result.Data;
                    expireDate = new CarInsuranceExpireDate();
                    expireDate.CarPlateNo = plateNumber;
                    expireDate.CarVinLast6Num = vin;
                    expireDate.InsuranceEndDate = DateTime.Parse(endDateModel.insuranceEndDate);
                    expireDate.ValidityEndDate = DateTime.Parse(endDateModel.validityEnds);
                    expireDate.CreateTime = this.DateTime;
                    expireDate.Creator = operater;
                    CurrentDb.CarInsuranceExpireDate.Add(expireDate);
                    CurrentDb.SaveChanges();

                    result.Data = expireDate;
                }
            }
            else
            {
                result.Result = ResultType.Success;
                result.Code = ResultCode.Success;
                result.Data = expireDate;
            }

            var merchant = CurrentDb.Merchant.Where(m => m.ClientCode == clientCode).FirstOrDefault();
            if (merchant != null)
            {
                var carInsuranceExpireDateSearchHis = CurrentDb.CarInsuranceExpireDateSearchHis.Where(m => m.UserId == merchant.UserId && m.CarPlateNo == plateNumber && m.CarVinLast6Num == vin).FirstOrDefault();
                if (carInsuranceExpireDateSearchHis == null)
                {
                    carInsuranceExpireDateSearchHis = new CarInsuranceExpireDateSearchHis();
                    carInsuranceExpireDateSearchHis.UserId = merchant.UserId;
                    carInsuranceExpireDateSearchHis.MerchantId = merchant.Id;
                    carInsuranceExpireDateSearchHis.CarPlateNo = plateNumber;
                    carInsuranceExpireDateSearchHis.CarVinLast6Num = vin;
                    carInsuranceExpireDateSearchHis.InsuranceEndDate = expireDate.InsuranceEndDate;
                    carInsuranceExpireDateSearchHis.ValidityEndDate = expireDate.ValidityEndDate;
                    carInsuranceExpireDateSearchHis.CreateTime = this.DateTime;
                    carInsuranceExpireDateSearchHis.Creator = operater;
                    CurrentDb.CarInsuranceExpireDateSearchHis.Add(carInsuranceExpireDateSearchHis);
                    CurrentDb.SaveChanges();
                }
            }



            return result;

        }


        public CustomJsonResult GetCarInsuranceExpireDateSearchHis(int operater, string clientCode)
        {

            CustomJsonResult result = new CustomJsonResult();

      
            var merchant = CurrentDb.Merchant.Where(m => m.ClientCode == clientCode).FirstOrDefault();
            if (merchant != null)
            {
                var carInsuranceExpireDateSearchHis = CurrentDb.CarInsuranceExpireDateSearchHis.Where(m => m.UserId == merchant.UserId).ToList();

                result.Result = ResultType.Success;
                result.Code = ResultCode.Success;
                result.Data = carInsuranceExpireDateSearchHis;
            }

            return result;

        }

    }
}
