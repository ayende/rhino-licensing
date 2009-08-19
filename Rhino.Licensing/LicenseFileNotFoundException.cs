using System;
using System.Runtime.Serialization;

namespace Rhino.Licensing
{
    [Serializable]
	public class LicenseFileNotFoundException : RhinoLicensingException
    {
        public LicenseFileNotFoundException()
        {
        }

        public LicenseFileNotFoundException(string message) : base(message)
        {
        }

        public LicenseFileNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected LicenseFileNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}