using System.Runtime.Serialization;
using Caliburn.PresentationFramework;

namespace Rhino.Licensing.AdminTool.Model
{
    [DataContract(Name = "UserData", Namespace = "http://schemas.hibernatingrhinos.com/")]
    public class UserData : PropertyChangedBase
    {
        private string _key;
        private string _value;

        [DataMember]
        public string Key
        {
            get { return _key; }
            set
            {
                _key = value;
                NotifyOfPropertyChange(() => Key);
            }
        }

        [DataMember]
        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                NotifyOfPropertyChange(() => Value);
            }
        }
    }
}