using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using Xunit;

namespace Rhino.Licensing.Tests
{
    public class Can_use_subscription_license : BaseLicenseTest
    {
        

        [Fact]
        public void Can_generate_subscription_license()
        {
            var generator = new LicenseGenerator(public_and_private);
            var license = generator.Generate("ayende", new Guid("FFD0C62C-B953-403e-8457-E90F1085170D"),
                                             new DateTime(2010, 10, 10),
                                             new Dictionary<string, string> {{"version", "2.0"}}, LicenseType.Subscription);

            var license_header = @"<license id=""ffd0c62c-b953-403e-8457-e90f1085170d"" expiration=""2010-10-10T00:00:00.0000000"" type=""Subscription"" version=""2.0"">";
            var owner_name = "<name>ayende</name>";

            Assert.Contains(license_header, license);
            Assert.Contains(owner_name, license);
        }

        [Fact]
        public void Can_validate_subscription_license_with_time_to_spare()
        {
            var generator = new LicenseGenerator(public_and_private);
            var license = generator.Generate("ayende", new Guid("FFD0C62C-B953-403e-8457-E90F1085170D"),
                                             DateTime.UtcNow.AddDays(15),
                                             new Dictionary<string, string> { { "version", "2.0" } }, LicenseType.Subscription);

            
            var path = Path.GetTempFileName();
            File.WriteAllText(path, license);


            Assert.DoesNotThrow(() => new LicenseValidator(public_only, path).AssertValidLicense());	
        }

        [Fact]
        public void Can_validate_subscription_license_with_less_than_two_days_when_url_is_not_available()
        {
            var generator = new LicenseGenerator(public_and_private);
            var license = generator.Generate("ayende", new Guid("FFD0C62C-B953-403e-8457-E90F1085170D"),
                                             DateTime.UtcNow.AddDays(1),
                                             new Dictionary<string, string> { { "version", "2.0" } }, LicenseType.Subscription);


            var path = Path.GetTempFileName();
            File.WriteAllText(path, license);


            Assert.DoesNotThrow(() => new LicenseValidator(public_only, path)
                                        {
                                            SubscriptionEndpoint = "http://localhost/"+Guid.NewGuid()
                                        }.AssertValidLicense());

        }

        [Fact]
        public void Cannot_validate_expired_subscription_license_when_url_is_not_available()
        {
            var generator = new LicenseGenerator(public_and_private);
            var license = generator.Generate("ayende", new Guid("FFD0C62C-B953-403e-8457-E90F1085170D"),
                                             DateTime.UtcNow.AddDays(-1),
                                             new Dictionary<string, string> { { "version", "2.0" } }, LicenseType.Subscription);


            var path = Path.GetTempFileName();
            File.WriteAllText(path, license);


            Assert.Throws<LicenseExpiredException>(() => new LicenseValidator(public_only, path)
            {
                SubscriptionEndpoint = "http://localhost/" + Guid.NewGuid()
            }.AssertValidLicense());
        }

        [Fact]
        public void Can_validate_expired_subscription_license_when_service_returns_new_one()
        {
            var generator = new LicenseGenerator(public_and_private);
            var license = generator.Generate("ayende", new Guid("FFD0C62C-B953-403e-8457-E90F1085170D"),
                                             DateTime.UtcNow.AddDays(-1),
                                             new Dictionary<string, string> { { "version", "2.0" } }, LicenseType.Subscription);


            var path = Path.GetTempFileName();
            File.WriteAllText(path, license);

            var host = new ServiceHost(typeof(DummySubscriptionLicensingService));
            const string address = "http://localhost:19292/license";
            host.AddServiceEndpoint(typeof(ISubscriptionLicensingService), new BasicHttpBinding(), address);

            host.Open();

            Assert.DoesNotThrow(() => new LicenseValidator(public_only, path)
            {
                SubscriptionEndpoint = address
            }.AssertValidLicense());


            host.Close();
        }

        [Fact]
        public void Can_validate_expired_subscription_license_when_service_returns_old_one()
        {
            var generator = new LicenseGenerator(public_and_private);
            var license = generator.Generate("ayende", new Guid("FFD0C62C-B953-403e-8457-E90F1085170D"),
                                             DateTime.UtcNow.AddDays(-1),
                                             new Dictionary<string, string> { { "version", "2.0" } }, LicenseType.Subscription);


            var path = Path.GetTempFileName();
            File.WriteAllText(path, license);

            var host = new ServiceHost(typeof(NoOpSubscriptionLicensingService));
            const string address = "http://localhost:19292/license";
            host.AddServiceEndpoint(typeof(ISubscriptionLicensingService), new BasicHttpBinding(), address);

            host.Open();

            Assert.Throws<LicenseExpiredException>(() => new LicenseValidator(public_only, path)
            {
                SubscriptionEndpoint = address
            }.AssertValidLicense());


            host.Close();
        }
    }

    public class NoOpSubscriptionLicensingService : ISubscriptionLicensingService
    {
        public string LeaseLicense(string previousLicense)
        {
            return previousLicense;
        }
    }

    public class DummySubscriptionLicensingService : ISubscriptionLicensingService
    {
        public string LeaseLicense(string previousLicense)
        {
            var tempFileName = Path.GetTempFileName();
            try
            {
                var stringLicenseValidator = new StringLicenseValidator(BaseLicenseTest.public_only, previousLicense);
                if (stringLicenseValidator.TryLoadingLicenseValuesFromValidatedXml() == false)
                    throw new InvalidOperationException("Invalid license provided");
                return new LicenseGenerator(BaseLicenseTest.public_and_private).Generate(stringLicenseValidator.Name,
                                                                                         stringLicenseValidator.UserId,
                                                                                         DateTime.UtcNow.AddDays(15),
                                                                                         stringLicenseValidator.LicenseAttributes,
                                                                                         LicenseType.Subscription);
            }
            finally
            {
                File.Delete(tempFileName);
            }
        }
    }
}