using System;
using Caliburn.PresentationFramework.Screens;
using Rhino.Licensing.AdminTool.Model;
using Rhino.Licensing.AdminTool.Properties;

namespace Rhino.Licensing.AdminTool.ViewModels
{
    public class IssueLicenseViewModel : Screen
    {
        private License currentLicense;

        public IssueLicenseViewModel()
        {
            CurrentLicense = new License
            {
                LicenseType = LicenseType.Trial,
                ExpirationDate = DateTime.Now.AddDays(Settings.Default.DefaultTrialDays)
            };
        }

        public virtual License CurrentLicense
        {
            get { return currentLicense; }
            set
            {
                currentLicense = value;
                NotifyOfPropertyChange(() => CurrentLicense);
            }
        }

        public virtual void Close()
        {
            TryClose(true);
        }
    }
}