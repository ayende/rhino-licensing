using System;
using System.Globalization;
using System.Windows.Data;

namespace Rhino.Licensing.AdminTool.ValueConverters
{
    public class ExpirationDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "Never";

            if(value is DateTime)
            {
                return ((DateTime)value).ToString("g");
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}