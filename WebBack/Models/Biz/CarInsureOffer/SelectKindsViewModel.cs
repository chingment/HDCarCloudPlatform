using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.CarInsureOffer
{
    public class SelectKindsViewModel : BaseViewModel
    {
        private List<InsurePlanKindModel> _insurePlanKinds;

        public List<InsurePlanKindModel> InsurePlanKinds
        {
            get
            {
                return _insurePlanKinds;
            }
            set
            {
                _insurePlanKinds = value;
            }
        }

        public SelectKindsViewModel()
        {

        }

        public SelectKindsViewModel(string selectedIds)
        {
            string[] arr_SelectedId = selectedIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            int[] intArrSelectedId = Array.ConvertAll<string, int>(arr_SelectedId, s => int.Parse(s));

            var carInsurePlans = CurrentDb.CarKind.Where(m => !intArrSelectedId.Contains(m.Id)).ToList();

            List<InsurePlanKindModel> insurePlanKindModels = new List<InsurePlanKindModel>();

            foreach (var m in carInsurePlans)
            {
                var carKind = CurrentDb.CarKind.Where(c => c.Id == m.Id).FirstOrDefault();
                InsurePlanKindModel insurePlanKindModel = new InsurePlanKindModel();
                insurePlanKindModel.Id = carKind.Id;
                insurePlanKindModel.Name = carKind.Name;
                insurePlanKindModel.AliasName = carKind.AliasName;
                insurePlanKindModel.Type = carKind.Type;
                insurePlanKindModel.CanWaiverDeductible = carKind.CanWaiverDeductible;
                insurePlanKindModel.InputType = carKind.InputType;
                insurePlanKindModel.InputUnit = carKind.InputUnit;
                if (!string.IsNullOrEmpty(carKind.InputValue))
                {
                    insurePlanKindModel.InputValue = Newtonsoft.Json.JsonConvert.DeserializeObject(carKind.InputValue);
                }

                insurePlanKindModel.IsHasDetails = carKind.IsHasDetails;

                insurePlanKindModels.Add(insurePlanKindModel);

                _insurePlanKinds = insurePlanKindModels;
            }
        }
    }
}