using System.ServiceModel;

namespace Rhino.Licensing
{
	[ServiceContract]
	public interface ISubscriptionLicensingService
	{
		[OperationContract]
		string LeaseLicense(string previousLicense);
	}
}