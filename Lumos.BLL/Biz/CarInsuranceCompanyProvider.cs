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
        public CustomJsonResult Add(int operater, CarInsuranceCompany carInsuranceCompany)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {

                var isExsits = CurrentDb.CarInsuranceCompany.Where(m => m.InsuranceCompanyId == carInsuranceCompany.InsuranceCompanyId).Count();
                if (isExsits > 0)
                {
                    ts.Dispose();
                    return new CustomJsonResult(ResultType.Failure, "已存在相同保险公司的名称");
                }

                CarInsuranceCompany l_carInsuranceCompany = new CarInsuranceCompany();
                l_carInsuranceCompany.InsuranceCompanyId = carInsuranceCompany.InsuranceCompanyId;
                l_carInsuranceCompany.InsuranceCompanyName = carInsuranceCompany.InsuranceCompanyName;
                l_carInsuranceCompany.InsuranceCompanyImgUrl = carInsuranceCompany.InsuranceCompanyImgUrl;
                l_carInsuranceCompany.CanInsure = carInsuranceCompany.CanInsure;
                l_carInsuranceCompany.CanClaims = carInsuranceCompany.CanClaims;
                l_carInsuranceCompany.CanApplyLossAssess = carInsuranceCompany.CanApplyLossAssess;
                l_carInsuranceCompany.Creator = operater;
                l_carInsuranceCompany.CreateTime = DateTime.Now;
                l_carInsuranceCompany.Status = Enumeration.CarInsuranceCompanyStatus.Normal;
                CurrentDb.CarInsuranceCompany.Add(l_carInsuranceCompany);
                CurrentDb.SaveChanges();


                SysFactory.SysItemCacheUpdateTime.Update(Enumeration.SysItemCacheType.CarInsCompanys);

                ts.Complete();



                result = new CustomJsonResult(ResultType.Success, "添加成功");
            }

            return result;
        }

        public CustomJsonResult Edit(int operater, CarInsuranceCompany carInsuranceCompany)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {

                var l_carInsuranceCompany = CurrentDb.CarInsuranceCompany.Where(m => m.Id == carInsuranceCompany.Id).FirstOrDefault();
                l_carInsuranceCompany.InsuranceCompanyImgUrl = carInsuranceCompany.InsuranceCompanyImgUrl;
                l_carInsuranceCompany.CanInsure = carInsuranceCompany.CanInsure;
                l_carInsuranceCompany.CanClaims = carInsuranceCompany.CanClaims;
                l_carInsuranceCompany.CanApplyLossAssess = carInsuranceCompany.CanApplyLossAssess;
                l_carInsuranceCompany.Mender = operater;
                l_carInsuranceCompany.LastUpdateTime = DateTime.Now;
                l_carInsuranceCompany.Status = carInsuranceCompany.Status;
                CurrentDb.SaveChanges();

                SysFactory.SysItemCacheUpdateTime.Update(Enumeration.SysItemCacheType.CarInsCompanys);

                ts.Complete();



                result = new CustomJsonResult(ResultType.Success, "修改成功");
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


                SysFactory.SysItemCacheUpdateTime.Update(Enumeration.SysItemCacheType.CarInsCompanys);

                ts.Complete();


                result = new CustomJsonResult(ResultType.Success, "操作成功");
            }

            return result;
        }


    }
}
