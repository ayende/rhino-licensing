using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Rhino.Licensing.AdminTool.ValueConverters
{
    public class BoolToVisibilityCollapsedConverter : IValueConverter
    {
        public bool InvertBoolean
        {
            get; set;
        }

        public object Convert(object o, Type targetType, object parameter, CultureInfo culture)
        {
            var collapsed = Visibility.Collapsed;

            if (o is bool?)
            {
                var nullable = (bool?)o;

                if (nullable.Value ^ InvertBoolean)
                    collapsed = Visibility.Visible;

                return collapsed;
            }

            if (o is bool && ((bool)o) ^ InvertBoolean)
            {
                collapsed = Visibility.Visible;
            }

            return collapsed;
        }

        public object ConvertBack(object o, Type targetType, object parameter, CultureInfo culture)
        {
            var visibility = (Visibility)o;
            return ((visibility == Visibility.Visible) ^ InvertBoolean);
        }
    }
}