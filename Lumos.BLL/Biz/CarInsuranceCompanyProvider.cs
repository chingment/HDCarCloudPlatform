using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL
{
    public class CarInsuranceCompanyProvider : BaseProvider
    {
        public CustomJsonResult Add(int operater, int insuranceCompanyId, string insuranceCompanyName, string insuranceCompanyImgUrl, decimal commercialRate, decimal compulsoryRate)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {

                var isExsits = CurrentDb.CarInsuranceCompany.Where(m => m.InsuranceCompanyId == insuranceCompanyId).Count();
                if (isExsits > 0)
                {
                    ts.Dispose();
                    return new CustomJsonResult(ResultType.Failure, "已存在相同保险公司的名称");
                }
                CarInsuranceCompany carInsuranceCompany = new CarInsuranceCompany();
                carInsuranceCompany.InsuranceCompanyId = insuranceCompanyId;
                carInsuranceCompany.InsuranceCompanyImgUrl = insuranceCompanyImgUrl;
                carInsuranceCompany.Creator = operater;
                carInsuranceCompany.CreateTime = DateTime.Now;
                carInsuranceCompany.Status = Enumeration.CarInsuranceCompanyStatus.Audit;
                CurrentDb.CarInsuranceCompany.Add(carInsuranceCompany);
                CurrentDb.SaveChanges();

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, "提交成功");
            }

            return result;
        }


        public CustomJsonResult Disable(int operater, int id)
        {
            CustomJsonResult result = new CustomJsonResult();


            using (TransactionScope ts = new TransactionScope())
            {

                var carInsuranceCompany = CurrentDb.CarInsuranceCompany.Where(m => m.Id == id).FirstOrDefault();
                if (carInsuranceCompany == null)
                {
                    return new CustomJsonResult(ResultType.Failure, "找不到该数据");
                }

                if (carInsuranceCompany.Status == Enumeration.CarInsuranceCompanyStatus.Audit)
                {
                    return new CustomJsonResult(ResultType.Failure, "正在审核中");
                }

                if (carInsuranceCompany.Status == Enumeration.CarInsuranceCompanyStatus.Normal)
                {
                    carInsuranceCompany.Status = Enumeration.CarInsuranceCompanyStatus.Disable;

                }
                else if (carInsuranceCompany.Status == Enumeration.CarInsuranceCompanyStatus.Disable)
                {
                    carInsuranceCompany.Status = Enumeration.CarInsuranceCompanyStatus.Normal;
                }

                carInsuranceCompany.LastUpdateTime = this.DateTime;
                carInsuranceCompany.Mender = operater;
                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, "停用成功");
            }

            return result;
        }


    }
}
