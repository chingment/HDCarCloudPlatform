using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Product
{
    public class EditViewModel : BaseViewModel
    {
        private List<Lumos.Entity.Company> _supplier = new List<Lumos.Entity.Company>();
        private List<Lumos.Entity.ProductCategory> _category = new List<Lumos.Entity.ProductCategory>();
        private List<Lumos.Entity.ProductKind> _kind = new List<Lumos.Entity.ProductKind>();
        private Lumos.Entity.Product _product = new Lumos.Entity.Product();
        private Lumos.Entity.ProductSku _productSku = new Lumos.Entity.ProductSku();
        private List<Lumos.Entity.ImgSet> _dispalyImgs = new List<Lumos.Entity.ImgSet>();


        public List<Lumos.Entity.Company> Supplier
        {
            get
            {
                return _supplier;
            }
            set
            {
                _supplier = value;
            }
        }

        public List<Lumos.Entity.ProductCategory> Category
        {
            get
            {
                return _category;
            }
            set
            {
                _category = value;
            }
        }

        public List<Lumos.Entity.ProductKind> Kind
        {
            get
            {
                return _kind;
            }
            set
            {
                _kind = value;
            }
        }

        public Lumos.Entity.Product Product
        {
            get
            {
                return _product;
            }
            set
            {
                _product = value;
            }
        }

        public Lumos.Entity.ProductSku ProductSku
        {
            get
            {
                return _productSku;
            }
            set
            {
                _productSku = value;
            }
        }

        public List<Lumos.Entity.ImgSet> DispalyImgs
        {
            get
            {
                return _dispalyImgs;
            }
            set
            {
                _dispalyImgs = value;
            }
        }

        public void LoadData(int id)
        {
            var supplier = CurrentDb.Company.Where(m => m.Status == Enumeration.CompanyStatus.Valid && m.Type == Enumeration.CompanyType.Supplier).ToList();

            if (supplier != null)
            {
                _supplier = supplier;
            }

            var category = CurrentDb.ProductCategory.Where(m => m.Id != 1 && m.IsDelete == false).ToList();

            if (category != null)
            {
                _category = category;
            }

            var kind = CurrentDb.ProductKind.Where(m => m.Id != 1 && m.IsDelete == false).ToList();

            if (kind != null)
            {
                _kind = kind;
            }

            var productSku = CurrentDb.ProductSku.Where(m => m.Id == id).FirstOrDefault();
            if (productSku != null)
            {
                _productSku = productSku;



                var product = CurrentDb.Product.Where(m => m.Id == productSku.ProductId).FirstOrDefault();
                if (product != null)
                {
                    _product = product;

                    if (!string.IsNullOrEmpty(_product.DispalyImgs))
                    {
                        var dispalyImgs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ImgSet>>(_product.DispalyImgs);

                        if (dispalyImgs != null)
                        {
                            _dispalyImgs = dispalyImgs;
                        }

                    }
                }
            }
        }
    }
}