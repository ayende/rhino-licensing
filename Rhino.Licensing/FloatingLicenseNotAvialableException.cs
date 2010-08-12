using System;
using System.Runtime.Serialization;

namespace Rhino.Licensing
{
    public class FloatingLicenseNotAvialableException : RhinoLicensingException
    {
        public FloatingLicenseNotAvialableException()
        {
        }

        public FloatingLicenseNotAvialableException(string message)
            : base(message)
        {
        }

        public FloatingLicenseNotAvialableException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected FloatingLicenseNotAvialableException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}