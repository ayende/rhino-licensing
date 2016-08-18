using System;
using System.IO;

namespace Rhino.Licensing
{
    /// <summary>
    /// License validator validates a license file
    /// that can be located on disk.
    /// </summary>
    public class LicenseValidator : AbstractLicenseValidator
    {
        private readonly string licensePath;
        private string inMemoryLicense;

        /// <summary>
        /// Creates a new instance of <seealso cref="LicenseValidator"/>.
        /// </summary>
        /// <param name="publicKey">public key</param>
        /// <param name="licensePath">path to license file</param>
        /// <param name="inMemoryLicense">Optionally set the inMemory license</param>
        public LicenseValidator(string publicKey, string licensePath, string inMemoryLicense = null)
            : base(publicKey)
        {
            this.licensePath = licensePath;
            this.inMemoryLicense = inMemoryLicense;
        }

        /// <summary>
        /// Creates a new instance of <seealso cref="LicenseValidator"/>.
        /// </summary>
        /// <param name="publicKey">public key</param>
        /// <param name="licensePath">path to license file</param>
        /// <param name="licenseServerUrl">license server endpoint address</param>
        /// <param name="clientId">Id of the license holder</param>
        public LicenseValidator(string publicKey, string licensePath, string licenseServerUrl, Guid clientId)
            : base(publicKey, licenseServerUrl, clientId)
        {
            this.licensePath = licensePath;
        }

        /// <summary>
        /// Gets or Sets the license content
        /// </summary>
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
                    if (string.IsNullOrEmpty(licensePath))
                        inMemoryLicense = value;
                    else
                        File.WriteAllText(licensePath, value);
                }
                catch (Exception e)
                {
                    inMemoryLicense = value;
                    Log.Warn("Could not write new license value, using in memory model instead", e);
                }
            }
        }

        /// <summary>
        /// Validates loaded license
        /// </summary>
        public override void AssertValidLicense()
        {
            if (!(string.IsNullOrEmpty(licensePath) && !string.IsNullOrEmpty(inMemoryLicense)) && File.Exists(licensePath) == false)
            {
                Log.WarnFormat("Could not find license file: {0}", licensePath);
                throw new LicenseFileNotFoundException();
            }

            base.AssertValidLicense();
        }

        /// <summary>
        /// Removes existing license from the machine.
        /// </summary>
        public override void RemoveExistingLicense()
        {
            if (!string.IsNullOrEmpty(licensePath))
                File.Delete(licensePath);
        }
    }
}
