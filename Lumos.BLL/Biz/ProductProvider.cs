using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumos.Entity;
using Lumos.Mvc;
using System.Transactions;

namespace Lumos.BLL
{
    public class ProductProvider : BaseProvider
    {
        public string GetLinkUrl(Enumeration.ProductType type,string clientCode, int id)
        {
            string index = ((int)type).ToString().Substring(0, 1);
            string linkUrl = "";
            switch (index)
            {
                case "1":
                    linkUrl = string.Format("{0}/App/Product/GoodsDetails?id={1}&clienCode={2}", BizFactory.AppSettings.WebAppServerUrl, id, clientCode);
                    break;
                default:
                    break;
            }

            return linkUrl;
        }

        public CustomJsonResult Add(int operater, Product product)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                product.CreateTime = this.DateTime;
                product.Creator = operater;
                product.Status = Enumeration.ProductStatus.OnLine;

                CurrentDb.Product.Add(product);
                CurrentDb.SaveChanges();

                result = new CustomJsonResult(ResultType.Success, "录入成功");

                ts.Complete();
            }

            return result;
        }

        public CustomJsonResult Edit(int operater, Product pProduct)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var product = CurrentDb.Product.Where(m => m.Id == pProduct.Id).FirstOrDefault();

                product.Name = pProduct.Name;
                product.Type = pProduct.Type;
                product.MainImgUrl = pProduct.MainImgUrl;
                product.ElseImgUrls = pProduct.ElseImgUrls;
                product.Details = pProduct.Details;
                product.LastUpdateTime = this.DateTime;
                product.Mender = operater;

                CurrentDb.SaveChanges();

                result = new CustomJsonResult(ResultType.Success, "保存成功");

                ts.Complete();
            }

            return result;
        }

        public CustomJsonResult OffLine(int operater, int id)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var product = CurrentDb.Product.Where(m => m.Id == id).FirstOrDefault();

                product.Status = Enumeration.ProductStatus.OffLine;
                product.LastUpdateTime = this.DateTime;
                product.Mender = operater;

                CurrentDb.SaveChanges();

                result = new CustomJsonResult(ResultType.Success, "下架成功");

                ts.Complete();
            }

            return result;
        }


        private Dictionary<int, string> GetProuctType()
        {
            Dictionary<int, string> prouctType = new Dictionary<int, string>();
            foreach (Enumeration.ProductType t in Enum.GetValues(typeof(Enumeration.ProductType)))
            {
                int strKey = Convert.ToInt32(t);
                Enum en = (Enum)Enum.Parse(t.GetType(), strKey.ToString());
                string strValue = en.GetCnName();
                prouctType.Add(strKey, strValue);
            }
            return prouctType;
        }

        public List<KeyValuePair<int, string>> GetGoodsType()
        {
            List<KeyValuePair<int, string>> types = GetProuctType().Where(m => ((int)m.Key).ToString().StartsWith(((int)Enumeration.ProductType.Goods).ToString()) && m.Key.ToString().Length > 1).ToList();

            return types;
        }
    }

}
