using Caliburn.PresentationFramework.Screens;
using Rhino.Licensing.AdminTool.Model;

namespace Rhino.Licensing.AdminTool.ViewModels
{
    public class ProjectViewModel : Screen
    {
        private Product _product;

        public ProjectViewModel(Product product)
        {
            _product = product;
        }

        public virtual Product CurrentProduct
        {
            get { return _product; }
            set
            {
                _product = value;
                NotifyOfPropertyChange(() => CurrentProduct);
            }
        }
    }
}