namespace Rhino.Licensing
{
    public class StringLicenseValidator : AbstractLicenseValidator
    {
        public StringLicenseValidator(string publicKey, string license)
            : base(publicKey)
        {
            License = license;
        }

        protected override string License
        {
            get; set;
        }
    }
}