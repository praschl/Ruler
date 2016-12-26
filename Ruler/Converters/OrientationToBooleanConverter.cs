using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace MiP.Ruler.Converters
{
    public class OrientationToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !value.Equals(Orientation.Horizontal);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var convertBack = value.Equals(true) ? Orientation.Vertical : Orientation.Horizontal;

            Console.WriteLine("Convert back to " + convertBack);

            return convertBack;
        }
    }
}