using Lumos.BLL;
using Lumos.BLL.Biz.Model;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL
{
    //[{"id":"2000","name":"颜色","value":[{"id":"20000001","name":"红色"}]},{"id":"1000","name":"重量","value":[{"id":"10000001","name":"100g"}]},{"id":"3000","name":"溶质","value":[{"id":"30000001","name":"1ml"}]}]

    public class ProductSkuPrice
    {
        public decimal Price { get; set; }

        public decimal DiscountPrice { get; set; }

        public ProductSkuPrice()
        {

        }

        public ProductSkuPrice(int skuid, decimal price, decimal showPrice)
        {
            this.Price = price;
            this.DiscountPrice = showPrice;
        }

    }


    public class SpecData
    {
        public int id { get; set; }
        public string name { get; set; }

        public SpecDataValue value { get; set; }
    }

    public class SpecDataValue
    {
        public int id { get; set; }
        public string name { get; set; }
    }


    public class ProductSkuProvider : BaseProvider
    {
        public string GetDetailsUrl(int id)
        {
            string linkUrl = "";

            linkUrl = string.Format("{0}/Resource/PrdDetails/{1}", BizFactory.AppSettings.WebAppServerUrl, id);

            return linkUrl;
        }

        public string GetMainImg(string imgSetJson)
        {
            if (string.IsNullOrEmpty(imgSetJson))
                return "";

            var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ImgSet>>(imgSetJson);

            var main = list.Where(m => m.IsMain == true).FirstOrDefault();
            if (main != null)
                return main.ImgUrl;

            return "";
        }

        public List<ImgSet> GetDispalyImgs(string imgSetJson)
        {
            if (string.IsNullOrEmpty(imgSetJson))
                return new List<ImgSet>();

            List<ImgSet> imgs = new List<ImgSet>();
            var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ImgSet>>(imgSetJson);

            foreach (var m in list)
            {
                if (!string.IsNullOrEmpty(m.ImgUrl))
                {
                    imgs.Add(m);
                }
            }

            return imgs;
        }


        private string GetSpec2(string spec)
        {
            if (string.IsNullOrEmpty(spec))
                return null;

            var specDataSet = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SpecData>>(spec);

            if (specDataSet.Count == 0)
                return null;

            var spec2 = "";
            for (int i = 0; i < specDataSet.Count; i++)
            {
                spec2 += specDataSet[i].id + ":" + specDataSet[i].value.id + ",";
            }

            spec2 = spec2.Substring(0, spec2.Length - 1);

            return spec2;
        }

        public string GetSpec3(string spec)
        {
            if (string.IsNullOrEmpty(spec))
                return null;

            var specDataSet = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SpecData>>(spec);

            if (specDataSet.Count == 0)
                return null;

            var spec3 = "";
            for (int i = 0; i < specDataSet.Count; i++)
            {
                spec3 += specDataSet[i].name + ":" + specDataSet[i].value.name + ",";
            }

            spec3 = spec3.Substring(0, spec3.Length - 1);

            return spec3;
        }


        public string BuildProductKindIds(string productKindIds)
        {
            if (string.IsNullOrEmpty(productKindIds))
                return null;

            if (productKindIds.Length == 1)
            {
                if (productKindIds == ",")
                {
                    return null;
                }
            }
            else if (productKindIds.Length > 1)
            {
                string start = productKindIds.Substring(0, 1);
                if (start != ",")
                {
                    productKindIds = "," + productKindIds;
                }

                string end = productKindIds.Substring(productKindIds.Length - 1, 1);
                if (end != ",")
                {
                    productKindIds = productKindIds + ",";
                }
            }



            return productKindIds;
        }

        public string BuildProductKindIdForSearch(string productKindId)
        {
            string kindIds = productKindId == null ? null : string.Format(",{0}", productKindId);

            return kindIds;
        }

        public CustomJsonResult AddByGoods(int operater, Product product, List<ProductSku> productSkus)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                // product.ProductCategory = product.ProductCategory.Trim();
                //  product.Supplier = product.Supplier.Trim();
                product.Type = Enumeration.ProductType.Goods;
                product.ProductKindIds = BuildProductKindIds(product.ProductKindIds);
                product.MainImg = BizFactory.ProductSku.GetMainImg(product.DispalyImgs);
                product.Status = Enumeration.ProductStatus.OnLine;
                product.Creator = operater;
                product.CreateTime = this.DateTime;

                CurrentDb.Product.Add(product);
                CurrentDb.SaveChanges();


                foreach (var prdSku in productSkus)
                {
                    prdSku.ProductId = product.Id;

                    prdSku.Status = product.Status;
                    prdSku.Name = product.Name;
                    prdSku.Creator = operater;
                    prdSku.CreateTime = this.DateTime;


                    CurrentDb.ProductSku.Add(prdSku);
                    CurrentDb.SaveChanges();


                    Inventory inventory = new Inventory();
                    inventory.ProductId = prdSku.ProductId;
                    inventory.ProductSkuId = prdSku.Id;
                    inventory.Quantity = prdSku.Quantity;
                    inventory.LockQuantity = 0;
                    inventory.SellQuantity = prdSku.Quantity;
                    inventory.IsOffSell = false;
                    inventory.CreateTime = this.DateTime;
                    inventory.Creator = operater;
                    CurrentDb.Inventory.Add(inventory);
                    CurrentDb.SaveChanges();


                    var inventoryOperateHis = new InventoryOperateHis();

                    inventoryOperateHis.ProductId = inventory.ProductId;
                    inventoryOperateHis.ProductSkuId = inventory.ProductSkuId;
                    inventoryOperateHis.Quantity = inventory.Quantity;
                    inventoryOperateHis.SellQuantity = inventory.SellQuantity;
                    inventoryOperateHis.LockQuantity = inventory.LockQuantity;
                    inventoryOperateHis.OperateType = Enumeration.InventoryOperateType.Increase;
                    inventoryOperateHis.ChangQuantity = 0;
                    inventoryOperateHis.Description = string.Format("新建商品，初始:{0}", prdSku.Quantity);
                    inventoryOperateHis.Creator = operater;
                    inventoryOperateHis.CreateTime = this.DateTime;

                    CurrentDb.InventoryOperateHis.Add(inventoryOperateHis);
                    CurrentDb.SaveChanges();
                }

                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }

            return result;
        }

        public CustomJsonResult EditByGoods(int operater, Product product, ProductSku productSku)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var l_product = CurrentDb.Product.Where(m => m.Id == product.Id).FirstOrDefault();
                if (l_product != null)
                {
                    l_product.Name = product.Name;
                    l_product.BriefIntro = product.BriefIntro;
                    //l_product.ProductCategoryId = product.ProductCategoryId;
                    //l_product.ProductCategory = product.ProductCategory.Trim();
                    //l_product.SupplierId = product.SupplierId;
                    //l_product.Supplier = product.Supplier.Trim();
                    l_product.ProductKindIds = BuildProductKindIds(product.ProductKindIds);
                    l_product.ProductKindNames = product.ProductKindNames;
                    l_product.MainImg = BizFactory.ProductSku.GetMainImg(product.DispalyImgs);
                    l_product.DispalyImgs = product.DispalyImgs;
                    l_product.ServiceDesc = product.ServiceDesc;
                    l_product.Details = product.Details;
                    l_product.Mender = operater;
                    l_product.LastUpdateTime = this.DateTime;

                    CurrentDb.SaveChanges();



                    var l_productSku = CurrentDb.ProductSku.Where(m => m.Id == productSku.Id).FirstOrDefault();

                    l_productSku.Name = product.Name;
                    l_productSku.Price = productSku.Price;
                    l_productSku.Mender = operater;
                    l_productSku.LastUpdateTime = this.DateTime;
                    CurrentDb.SaveChanges();
                }


                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }



            return result;
        }


        public CustomJsonResult AddByInsurance(int operater, Product product, List<ProductSku> productSkus)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                //todo 暂时写死
                product.ProductCategory = "团体意外险";
                product.ProductCategoryId = 1002001;
                product.Type = Enumeration.ProductType.InsureForYiWaiXian;
                product.ProductKindIds = BuildProductKindIds("100010001");
                product.ProductKindNames = "意外险";

                product.Supplier = product.Supplier.NullToEmpty();
                product.MainImg = BizFactory.ProductSku.GetMainImg(product.DispalyImgs);
                product.Creator = operater;
                product.CreateTime = this.DateTime;
                product.Status = Enumeration.ProductStatus.OnLine;
                CurrentDb.Product.Add(product);
                CurrentDb.SaveChanges();


                foreach (var prdSku in productSkus)
                {
                    prdSku.ProductId = product.Id;
                    prdSku.Name = product.Name;
                    prdSku.Creator = operater;
                    prdSku.CreateTime = this.DateTime;


                    CurrentDb.ProductSku.Add(prdSku);
                    CurrentDb.SaveChanges();


                    Inventory inventory = new Inventory();
                    inventory.ProductId = prdSku.ProductId;
                    inventory.ProductSkuId = prdSku.Id;
                    inventory.Quantity = prdSku.Quantity;
                    inventory.LockQuantity = 0;
                    inventory.SellQuantity = prdSku.Quantity;
                    inventory.IsOffSell = false;
                    inventory.CreateTime = this.DateTime;
                    inventory.Creator = operater;
                    CurrentDb.Inventory.Add(inventory);
                    CurrentDb.SaveChanges();


                    var inventoryOperateHis = new InventoryOperateHis();

                    inventoryOperateHis.ProductId = inventory.ProductId;
                    inventoryOperateHis.ProductSkuId = inventory.ProductSkuId;
                    inventoryOperateHis.Quantity = inventory.Quantity;
                    inventoryOperateHis.SellQuantity = inventory.SellQuantity;
                    inventoryOperateHis.LockQuantity = inventory.LockQuantity;
                    inventoryOperateHis.OperateType = Enumeration.InventoryOperateType.Increase;
                    inventoryOperateHis.ChangQuantity = 0;
                    inventoryOperateHis.Description = string.Format("新建商品，初始:{0}", prdSku.Quantity);
                    inventoryOperateHis.Creator = operater;
                    inventoryOperateHis.CreateTime = this.DateTime;

                    CurrentDb.InventoryOperateHis.Add(inventoryOperateHis);
                    CurrentDb.SaveChanges();
                }

                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }

            return result;
        }

        private ProductSkuPrice GetPriceModel(int skuid, decimal price, decimal showPrice)
        {
            ProductSkuPrice model = new ProductSkuPrice();
            model.Price = price;
            model.DiscountPrice = showPrice;
            return model;
        }

        public CustomJsonResult GetProductsByKindId(int kindId)
        {
            string str_id = kindId.ToString();

            string search_id = BizFactory.ProductSku.BuildProductKindIdForSearch(str_id);

            var products = CurrentDb.Product.Where(m => SqlFunctions.CharIndex(search_id, m.ProductKindIds) > 0).ToList();
            List<ProductSku> productSkus = new List<ProductSku>();
            foreach (var product in products)
            {

                var productSku = CurrentDb.ProductSku.Where(m => m.ProductId == product.Id).FirstOrDefault();

                productSkus.Add(productSku);
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", productSkus);
        }

    }
}
