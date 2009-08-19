namespace Rhino.Licensing
{
	using System;
	using System.Runtime.Serialization;

	public class FloatingLicenseNotAvialableException : RhinoLicensingException
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		public FloatingLicenseNotAvialableException()
		{
		}

		public FloatingLicenseNotAvialableException(string message) : base(message)
		{
		}

		public FloatingLicenseNotAvialableException(string message, Exception inner) : base(message, inner)
		{
		}

		protected FloatingLicenseNotAvialableException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}
}