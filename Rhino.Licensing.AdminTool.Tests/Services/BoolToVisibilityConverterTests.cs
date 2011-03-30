using System.Globalization;
using System.Windows;
using Rhino.Licensing.AdminTool.ValueConverters;
using Xunit;
using Xunit.Extensions;

namespace Rhino.Licensing.AdminTool.Tests.Services
{
    public class BoolToVisibilityConverterTests
    {
        private readonly BoolToVisibilityCollapsedConverter _converter;

        public BoolToVisibilityConverterTests()
        {
            _converter = new BoolToVisibilityCollapsedConverter();
        }

        [Theory]
        [InlineData(true, Visibility.Visible)]
        [InlineData(false, Visibility.Collapsed)]
        public void Converts_Boolean_To_Visibility(bool value, Visibility converted)
        {
            var visibility = _converter.Convert(value, null, null, CultureInfo.InvariantCulture);

            Assert.Equal(converted, visibility);
        }

        [Theory]
        [InlineData(true, Visibility.Visible)]
        [InlineData(null, Visibility.Collapsed)]
        public void Converts_Nullable_Boolean_To_Visibility(bool? value, Visibility converted)
        {
            var visibility = _converter.Convert(value, null, null, CultureInfo.InvariantCulture);

            Assert.Equal(converted, visibility);
        }

        [Theory]
        [InlineData(false, Visibility.Visible)]
        [InlineData(null, Visibility.Collapsed)]
        public void Setting_Invert_Converts_Nullable_Boolean_To_Visibility(bool? value, Visibility converted)
        {
            _converter.InvertBoolean = true;

            var visibility = _converter.Convert(value, null, null, CultureInfo.InvariantCulture);

            Assert.Equal(converted, visibility);
        }

        [Theory]
        [InlineData(Visibility.Visible, true)]
        [InlineData(Visibility.Collapsed, false)]
        public void Converts_Back_Visible_To_Boolean(Visibility visibility, bool converted)
        {
            var value = _converter.ConvertBack(visibility, null, null, CultureInfo.InvariantCulture);

            Assert.Equal(converted, value);
        }
    }
}