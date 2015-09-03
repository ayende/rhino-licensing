using Caliburn.PresentationFramework.Screens;
using Rhino.Licensing.AdminTool.Model;

namespace Rhino.Licensing.AdminTool.ViewModels
{
    public class LicenseInfoViewModel : Screen, ILicenseInfoViewModel
    {
        private License _currentLicense;

        public virtual License CurrentLicense
        {
            get { return _currentLicense; }
            set
            {
                _currentLicense = value;
                NotifyOfPropertyChange(() => CurrentLicense);
            }
        }
    }
}