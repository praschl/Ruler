using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MiP.Ruler.Converters
{
    public class OrientationToMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            const int Overlength = 20;

            // TODO: turn the constant Overlength into a property on the window.
            if (value.Equals(Orientation.Horizontal))
                return new Thickness(0, Overlength, 0, Overlength);

            return new Thickness(Overlength, 0, Overlength, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}