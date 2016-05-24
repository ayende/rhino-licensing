using System;
using Caliburn.PresentationFramework.Filters;
using Caliburn.PresentationFramework.Screens;
using Rhino.Licensing.AdminTool.Model;
using Rhino.Licensing.AdminTool.Properties;

namespace Rhino.Licensing.AdminTool.ViewModels
{
    public class IssueLicenseViewModel : Conductor<ILicenseHolder>.Collection.OneActive
    {
        private License _currentLicense;

        public IssueLicenseViewModel(IUserDataViewModel userDataViewModel, ILicenseInfoViewModel licenseInfoViewModel)
        {
            UserData = userDataViewModel;
            LicenseInfo = licenseInfoViewModel;

            Items.Add(userDataViewModel);
            Items.Add(licenseInfoViewModel);

            CurrentLicense = new License
            {
                LicenseType = LicenseType.Trial,
                ExpirationDate = DateTime.Now.AddDays(Settings.Default.DefaultTrialDays).Date
            };
        }

        public virtual License CurrentLicense
        {
            get { return _currentLicense; }
            private set
            {
                _currentLicense = value;
                NotifyLicenseChanged();
                NotifyOfPropertyChange(() => CurrentLicense);
            }
        }

        private void NotifyLicenseChanged()
        {
            foreach (var item in Items)
            {
                item.CurrentLicense = CurrentLicense;
            }
        }

        public virtual IUserDataViewModel UserData
        {
            get; private set; 
        }

        public virtual ILicenseInfoViewModel LicenseInfo
        {
            get; private set;
        }

        public virtual bool CanAccept
        {
            get
            {
                return CurrentLicense != null &&
                       !string.IsNullOrWhiteSpace(CurrentLicense.OwnerName);
            }
        }

        [AutoCheckAvailability]
        public virtual void Accept()
        {
            TryClose(true);
        }
    }
}