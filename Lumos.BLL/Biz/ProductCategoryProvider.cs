using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumos.Entity;
using System.Transactions;

namespace Lumos.BLL
{
    public class ProductCategoryProvider : BaseProvider
    {
        public CustomJsonResult Add(int operater, ProductCategory model)
        {
            CustomJsonResult result = new CustomJsonResult();
            if (model.PId == 0)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "请选择上级分类");
            }
            using (TransactionScope ts = new TransactionScope())
            {
                model.CreateTime = this.DateTime;
                model.Creator = operater;
                model.Status = Enumeration.ProductCategoryStatus.Valid;
                var ctg = CurrentDb.ProductCategory.Where(m => m.Name == model.Name).FirstOrDefault();
                if (ctg != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在，请修改名称。");
                }
                ctg = CurrentDb.ProductCategory.Where(m => m.PId == model.PId).OrderByDescending(m => m.Id).FirstOrDefault();
                if (ctg == null)//同级没有分类
                {

                    model.Id = model.PId * 1000 + 001;
                }
                else//同级已存在分类则+1
                {
                    model.Id = ctg.Id + 1;
                }

                CurrentDb.ProductCategory.Add(model);
                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }
            return result;
        }
        public CustomJsonResult Edit(int operater, ProductCategory model)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var dist = CurrentDb.ProductCategory.Where(m => m.Name == model.Name && m.Id != model.Id).FirstOrDefault();
                if (dist != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在，请修改名称。");
                }

                Lumos.Entity.ProductCategory model_o = CurrentDb.ProductCategory.Where(m => m.Id == model.Id).FirstOrDefault();
                model_o.Name = model.Name;
                model_o.Description = model.Description;
                model_o.Status = model.Status;
                model_o.MainImg = model.MainImg;
                model_o.IconImg = model.IconImg;
                model_o.LastUpdateTime = this.DateTime;
                model_o.Mender = operater;

                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }
            return result;
        }


        public CustomJsonResult Delete(int operater, int[] ids)
        {

            if (ids != null)
            {

                if (ids.Contains(1001) || ids.Contains(1002))
                {
                    return new CustomJsonResult(ResultType.Success, ResultCode.Success, "该分类不能删除");
                }

                foreach (var id in ids)
                {
                    var productCategory = CurrentDb.ProductCategory.Where(m => m.Id == id).FirstOrDefault();
                    if (productCategory != null)
                    {
                        productCategory.IsDelete = true;

                        string str_id = id.ToString();


                        var products = CurrentDb.Product.Where(m => m.ProductCategoryId == productCategory.Id).ToList();

                        foreach (var product in products)
                        {
                            product.ProductCategoryId = 0;

                            var productSkus = CurrentDb.ProductSku.Where(m => m.ProductId == product.Id).ToList();

                            foreach (var productSku in productSkus)
                            {
                                productSku.ProductCategoryId = 0;
                            }

                            CurrentDb.SaveChanges();
                        }

                        CurrentDb.SaveChanges();
                    }

                }

            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "删除成功");
        }
    }
}
