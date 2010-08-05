using System;
using System.ServiceModel;
namespace Rhino.Licensing
{
    [ServiceContract(SessionMode = SessionMode.NotAllowed)]
    public interface ILicensingService
    {
        [OperationContract]
        string LeaseLicense(string machine, string user, Guid id);
    }
}