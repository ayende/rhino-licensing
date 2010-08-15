using System;
using System.Runtime.Serialization;
using Caliburn.PresentationFramework;

namespace Rhino.Licensing.AdminTool.Model
{
    [DataContract(Name = "Product", Namespace = "http://schemas.hibernatingrhinos.com/")]
    public class Product : PropertyChangedBase
    {
        private string _publicKey;
        private string _name;
        private string _privateKey;
        private Guid _id;

        public Product()
        {
            Id = Guid.NewGuid();
        }

        [DataMember]
        public virtual Guid Id
        {
            get { return _id; }
            set
            {
                _id = value;
                NotifyOfPropertyChange(() => Id);
            }
        }

        [DataMember]
        public virtual string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        [DataMember]
        public virtual string PrivateKey
        {
            get { return _privateKey; }
            set
            {
                _privateKey = value;
                NotifyOfPropertyChange(() => PrivateKey);
            }
        }

        [DataMember]
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