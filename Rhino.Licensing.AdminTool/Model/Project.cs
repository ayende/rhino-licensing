using System;
using System.Runtime.Serialization;
using Caliburn.PresentationFramework;

namespace Rhino.Licensing.AdminTool.Model
{
    [DataContract(Name = "Project", Namespace = "http://schemas.hibernatingrhinos.com/")]
    public class Project : PropertyChangedBase
    {
        private Product _product;

        public Project()
        {
            Product = new Product();
        }

        [DataMember]
        public virtual Product Product
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