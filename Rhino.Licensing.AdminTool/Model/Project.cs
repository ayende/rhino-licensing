using Caliburn.PresentationFramework;

namespace Rhino.Licensing.AdminTool.Model
{
    public class Project : PropertyChangedBase
    {
        private string _name;
        private Product _product;

        public Project()
        {
            Product = new Product();
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public Product Product
        {
            get { return _product; }
            set
            {
                _product = value;
                NotifyOfPropertyChange(() => Product);
            }
        }
    }
}