using System;
using System.IO;
using System.Threading;
using Xunit;

namespace Rhino.Licensing.Tests
{
	public class Duplicate_license :BaseLicenseTest
	{
		[Fact]
		public void Will_detect_duplicate_license()
		{
			var guid = Guid.NewGuid();
			var generator = new LicenseGenerator(public_and_private);
			var expiration = DateTime.Now.AddDays(30);
			var key = generator.Generate("Oren Eini", guid, expiration, LicenseType.Trial);

			var path = Path.GetTempFileName();
			File.WriteAllText(path, key);

			var wait = new ManualResetEvent(false);

			var validator = new LicenseValidator(public_only, path)
			{
				LeaseTimeout = TimeSpan.FromSeconds(1)
			};
			InvalidationType? invalidation = null;
			validator.LicenseInvalidated += type => invalidation = type;
			validator.MultipleLicensesWereDiscovered += (sender, args) => wait.Set();
			validator.AssertValidLicense();

			var validator2 = new LicenseValidator(public_only, path);
			validator2.AssertValidLicense();

			Assert.True(wait.WaitOne(TimeSpan.FromSeconds(100)));
			Assert.Equal(invalidation.Value, InvalidationType.TimeExpired);
		}
	}
}