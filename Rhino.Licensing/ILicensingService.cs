namespace Rhino.Licensing
{
	using System;
	using System.ServiceModel;

	[ServiceContract(
		SessionMode = SessionMode.NotAllowed
		)]
	public interface ILicensingService
	{
		[OperationContract]
		string LeaseLicense(Guid id);
	}
}