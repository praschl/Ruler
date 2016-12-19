using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MiP.Ruler
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isEqual = (string) parameter == "true";

            if (value.Equals(isEqual))
                return Visibility.Visible;

            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}