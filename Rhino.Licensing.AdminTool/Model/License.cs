using System;
using System.Runtime.Serialization;
using Caliburn.PresentationFramework;

namespace Rhino.Licensing.AdminTool.Model
{
    [DataContract(Name = "License", Namespace = "http://schemas.hibernatingrhinos.com/")]
    public class License : PropertyChangedBase
    {
        private string _ownerName;
        private DateTime? _expirationDate;
        private LicenseType _licenseType;

        [DataMember]
        public virtual string OwnerName
        {
            get { return _ownerName; }
            set
            {
                _ownerName = value;
                NotifyOfPropertyChange(() => OwnerName);
            }
        }

        [DataMember]
        public virtual DateTime? ExpirationDate
        {
            get { return _expirationDate; }
            set
            {
                _expirationDate = value;
                NotifyOfPropertyChange(() => ExpirationDate);
            }
        }

        [DataMember]
        public virtual LicenseType LicenseType
        {
            get { return _licenseType; }
            set
            {
                _licenseType = value;
                NotifyOfPropertyChange(() => LicenseType);
            }
        }
    }
}