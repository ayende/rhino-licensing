using System;
using System.Runtime.Serialization;

namespace Rhino.Licensing
{
    public class RhinoLicensingException : Exception
    {
        protected RhinoLicensingException()
        {
        }

        protected RhinoLicensingException(string message)
            : base(message)
        {
        }

        protected RhinoLicensingException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected RhinoLicensingException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}