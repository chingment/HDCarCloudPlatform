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
    public class InsuranceCompanyProvider : BaseProvider
    {
        public CustomJsonResult Add(int operater, InsuranceCompany insuranceCompany)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var isExsits = CurrentDb.InsuranceCompany.Where(m => m.Name == insuranceCompany.Name).Count();
                if (isExsits > 0)
                {
                    return new CustomJsonResult(ResultType.Failure, "已存在相同保险公司的名称");
                }

                insuranceCompany.CreateTime = this.DateTime;
                insuranceCompany.Creator = operater;

                CurrentDb.InsuranceCompany.Add(insuranceCompany);
                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, "提交成功");
            }

            return result;
        }

        public CustomJsonResult Edit(int operater, InsuranceCompany insuranceCompany)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var isExsits = CurrentDb.InsuranceCompany.Where(m => m.Name == insuranceCompany.Name && m.Id != insuranceCompany.Id).Count();
                if (isExsits > 0)
                {
                    return new CustomJsonResult(ResultType.Failure, "已存在相同保险公司的名称");
                }

                var l_InsuranceCompany = CurrentDb.InsuranceCompany.Where(m => m.Id == insuranceCompany.Id).FirstOrDefault();
                if (l_InsuranceCompany == null)
                {
                    return new CustomJsonResult(ResultType.Failure, "不存在该保险公司");
                }
                l_InsuranceCompany.ImgUrl = insuranceCompany.ImgUrl;
                l_InsuranceCompany.Name = insuranceCompany.Name;
                l_InsuranceCompany.LastUpdateTime = this.DateTime;
                l_InsuranceCompany.Mender = operater;

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, "提交成功");
            }

            return result;
        }
    }
}
