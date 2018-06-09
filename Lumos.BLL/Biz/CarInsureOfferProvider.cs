using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YdtSdk;

namespace Lumos.BLL
{
    [Serializable]
    public class DrivingLicenceInfo
    {
        public string owner { get; set; }

        public string plateNum { get; set; }

        public string address { get; set; }

        public string userCharacter { get; set; }

        public string model { get; set; }

        public string vin { get; set; }

        public string engineNo { get; set; }

        public string registerDate { get; set; }

        public string issueDate { get; set; }

        public string vehicleType { get; set; }

        public string fileKey { get; set; }

    }

    [Serializable]
    public class IdentityCardInfo
    {
        public string address { get; set; }

        public string birthday { get; set; }

        public string idNumber { get; set; }

        public string name { get; set; }

        public string people { get; set; }

        public string sex { get; set; }

        public string type { get; set; }

        public string issueAuthority { get; set; }

        public string validity { get; set; }

        public string fileKey { get; set; }
    }

    public class CarInsureOfferProvider : BaseProvider
    {
        public DrivingLicenceInfo GetDrivingLicenceInfoFromImgUrl(string url)
        {
            DrivingLicenceInfo licenceInfo = null;


            var ydtLicenceInfo = YdtUtils.GetLicenseInfoByUrl(url);
            if (ydtLicenceInfo != null)
            {
                licenceInfo = new DrivingLicenceInfo();
                licenceInfo.owner = ydtLicenceInfo.owner;
                licenceInfo.plateNum = ydtLicenceInfo.plateNum;
                licenceInfo.address = null;
                licenceInfo.userCharacter = null;
                licenceInfo.model = ydtLicenceInfo.model;
                licenceInfo.vin = ydtLicenceInfo.vin;
                licenceInfo.engineNo = ydtLicenceInfo.engineNum;
                licenceInfo.registerDate = ydtLicenceInfo.registerDate;
                licenceInfo.issueDate = ydtLicenceInfo.issueDate;
                licenceInfo.vehicleType = ydtLicenceInfo.vehicleType;
                licenceInfo.fileKey = ydtLicenceInfo.fileKey;
            }

            return licenceInfo;
        }

        public IdentityCardInfo GetIdentityCardInfoFromImgUrl(string url)
        {
            IdentityCardInfo identityCardInfo = null;


            var ydtIdentityCardInfo = YdtUtils.GetIdentityInfoByUrl(url);
            if (ydtIdentityCardInfo != null)
            {
                identityCardInfo = new IdentityCardInfo();
                identityCardInfo.address = ydtIdentityCardInfo.address;
                identityCardInfo.birthday = ydtIdentityCardInfo.birthday;
                identityCardInfo.idNumber = ydtIdentityCardInfo.num;
                identityCardInfo.name = ydtIdentityCardInfo.name;
                identityCardInfo.people = ydtIdentityCardInfo.nationality;
                identityCardInfo.sex = ydtIdentityCardInfo.sex;
                identityCardInfo.type = null;
                identityCardInfo.issueAuthority = null;
                identityCardInfo.validity = null;
                identityCardInfo.fileKey = ydtIdentityCardInfo.fileKey;
            }

            return identityCardInfo;
        }
    }
}
