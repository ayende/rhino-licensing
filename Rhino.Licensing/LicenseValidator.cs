using System;
using System.IO;

namespace Rhino.Licensing
{
    public class LicenseValidator : AbstractLicenseValidator
    {
        private readonly string licensePath;
        private string inMemoryLicense;

        public LicenseValidator(string publicKey, string licensePath)
            : base(publicKey)
        {
            this.licensePath = licensePath;
        }

        public LicenseValidator(string publicKey, string licensePath, string licenseServerUrl, Guid clientId)
            : base(publicKey, licenseServerUrl, clientId)
        {
            this.licensePath = licensePath;
        }

        protected override string License
        {
            get
            {
                return inMemoryLicense ?? File.ReadAllText(licensePath);
            }
            set
            {
                try
                {
                    File.WriteAllText(licensePath, value);
                }
                catch (Exception e)
                {
                    inMemoryLicense = value;
                    Log.Warn("Could not write new license value, using in memory model instrea", e);
                }
            }
        }

        public override void AssertValidLicense()
        {
            if (File.Exists(licensePath) == false)
            {
                Log.WarnFormat("Could not find license file: {0}", licensePath);
                throw new LicenseFileNotFoundException();
            }

            base.AssertValidLicense();
        }

        public override void RemoveExistingLicense()
        {
            File.Delete(licensePath);
        }
    }
}
