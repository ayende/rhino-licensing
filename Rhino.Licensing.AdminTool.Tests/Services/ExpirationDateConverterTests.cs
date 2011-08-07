using System;
using System.Globalization;
using Rhino.Licensing.AdminTool.ValueConverters;
using Xunit;

namespace Rhino.Licensing.AdminTool.Tests.Services
{
    public class ExpirationDateConverterTests
    {
        [Fact]
        public void Returns_Never_If_No_Date_Specified()
        {
            var converter = new ExpirationDateConverter();

            var value = converter.Convert(null, GetType(), null, CultureInfo.InvariantCulture);

            Assert.Equal("Never", value);
        }

        [Fact]
        public void Convertes_Date_Using_Provided_Culture()
        {
            var converter = new ExpirationDateConverter();
            var culture = new CultureInfo("en-au");

            var value = converter.Convert(new DateTime(2000, 1, 1), GetType(), null, culture);

            Assert.Equal("1/01/2000 12:00 AM", value);
        }

        [Fact]
        public void Object_Of_Non_Date_Type_Passed_In_Will_Be_Returned()
        {
            var converter = new ExpirationDateConverter();
            var culture = new CultureInfo("en-au");

            var value = converter.Convert("String Value", GetType(), null, culture);

            Assert.Equal("String Value", value);
        }

        [Fact]
        public void Converts_Date_Using_Invariant_Culture_When_No_Culture_Provided()
        {
            var converter = new ExpirationDateConverter();
            CultureInfo culture = null;

            var value = converter.Convert(new DateTime(2000, 1, 1), GetType(), null, culture);

            Assert.Equal("01/01/2000 00:00", value);
        }

        [Fact]
        public void Does_One_Way_Conversion()
        {
            var converter = new ExpirationDateConverter();

            Assert.Throws<NotImplementedException>(() => converter.ConvertBack(this, typeof(object), this, CultureInfo.InvariantCulture));
        }
    }
}