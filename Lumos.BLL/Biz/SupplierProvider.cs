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
    public class SupplierProvider : BaseProvider
    {
        public CustomJsonResult Add(int operater, Company model)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {

                model.CreateTime = this.DateTime;
                model.Creator = operater;
                model.Status = Enumeration.CompanyStatus.Valid;
                model.Type = Enumeration.CompanyType.Supplier;
                var md = CurrentDb.Company.Where(m => m.Name == model.Name).FirstOrDefault();
                if (md != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在，请修改名称。");
                }
                md = CurrentDb.Company.Where(m => m.Code == model.Code).FirstOrDefault();
                if (md != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "代码已存在，请修改代码。");
                }

                CurrentDb.Company.Add(model);
                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }
            return result;
        }
        public CustomJsonResult Edit(int operater, Company model)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                //var md = CurrentDb.Supplier.Where(m => m.Name == model.Name && m.Id != model.Id).FirstOrDefault();
                //if (md != null)
                //{
                //    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在，请修改名称。");
                //}
                //md = CurrentDb.Supplier.Where(m => m.Code == model.Code && m.Id != model.Id).FirstOrDefault();
                //if (md != null)
                //{
                //    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "代码已存在，请修改代码 。");
                //}

                Lumos.Entity.Company model_o = CurrentDb.Company.Where(m => m.Id == model.Id).FirstOrDefault();
                //model_o.Name = model.Name;
                //model_o.Code = model.Code;
                model_o.Address = model.Address;
                model_o.PhoneNumber = model.PhoneNumber;
                model_o.Status = model.Status;
                model_o.Description = model.Description;
                model_o.LastUpdateTime = this.DateTime;
                model_o.Mender = operater;

                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }
            return result;
        }

    }
}
