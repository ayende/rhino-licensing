namespace Rhino.Licensing
{
	using System;
	using System.Runtime.Serialization;

	public class RhinoLicensingException : Exception
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		protected RhinoLicensingException()
		{
		}

		protected RhinoLicensingException(string message) : base(message)
		{
		}

		protected RhinoLicensingException(string message, Exception inner) : base(message, inner)
		{
		}

		protected RhinoLicensingException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}
}