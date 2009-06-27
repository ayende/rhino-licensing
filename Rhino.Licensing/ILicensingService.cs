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
		string LeaseLicense(
			string machine,
			string user,
			Guid id);
	}
}