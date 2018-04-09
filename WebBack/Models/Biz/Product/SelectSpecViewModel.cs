using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.Product
{
    public class SpecValueModel
    {
        public int Id
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }
    }

    public class SpecModel
    {

        public SpecModel()
        {
            this.Value = new List<SpecValueModel>();
        }

        public int Id
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public List<SpecValueModel> Value { get; set; }
    }

    public class SelectSpecViewModel : BaseViewModel
    {
        private List<SpecModel> _specs = new List<SpecModel>();

        public List<SpecModel> Specs
        {
            get
            {

                return _specs;
            }
            set
            {
                _specs = value;
            }
        }

        public SelectSpecViewModel()
        {
            var specs = CurrentDb.ProductSpecDataSet.OrderBy(m => m.Priority).ToList();

            foreach (var spec in specs)
            {
                SpecModel specModel = new SpecModel();
                specModel.Id = spec.Id;
                specModel.Name = spec.Name;

                var specsValues = CurrentDb.ProductSpecDataSetValue.Where(m => m.ProductSpecDataSetId == spec.Id).ToList();

                foreach (var specsValue in specsValues)
                {
                    SpecValueModel specValueModel = new SpecValueModel();
                    specValueModel.Id = specsValue.Id;
                    specValueModel.Name = specsValue.Name;

                    specModel.Value.Add(specValueModel);
                }

                _specs.Add(specModel);
            }
        }


    }
}