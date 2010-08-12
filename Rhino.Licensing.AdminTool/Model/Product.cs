using System;
using Caliburn.PresentationFramework;

namespace Rhino.Licensing.AdminTool.Model
{
    public class Product : PropertyChangedBase
    {
        private string _publicKey;
        private string _name;
        private string _privateKey;
        private Guid _id;

        public virtual Guid Id
        {
            get { return _id; }
            set
            {
                _id = value;
                NotifyOfPropertyChange(() => Id);
            }
        }

        public virtual string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public virtual string PrivateKey
        {
            get { return _privateKey; }
            set
            {
                _privateKey = value;
                NotifyOfPropertyChange(() => PrivateKey);
            }
        }

        public virtual string PublicKey
        {
            get { return _publicKey; }
            set
            {
                _publicKey = value;
                NotifyOfPropertyChange(() => PublicKey);
            }
        }
    }
}