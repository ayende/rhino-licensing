using System;
using System.Runtime.Serialization;

namespace Rhino.Licensing
{
    [Serializable]
	public class LicenseNotFoundException : RhinoLicensingException
    {
        public LicenseNotFoundException()
        {
        }

        public LicenseNotFoundException(string message) : base(message)
        {
        }

        public LicenseNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected LicenseNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}