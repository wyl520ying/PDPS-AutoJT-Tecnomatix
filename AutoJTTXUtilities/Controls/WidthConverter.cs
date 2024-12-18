using System;
using System.Globalization;
using System.Windows.Data;

namespace AutoJTTXUtilities.Controls
{
    public class WidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width)
            {
                // Subtracting 20 to account for the scrollbar and padding
                return width - 20;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
