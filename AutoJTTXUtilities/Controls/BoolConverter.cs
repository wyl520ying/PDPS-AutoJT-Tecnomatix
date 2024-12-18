using System;
using System.Windows.Data;

namespace AutoJTTXUtilities.Controls
{
    //转换器，将 true/false 属性值转换为 System.Windows.Visibility 属性类型。
    //https://stackoverflow.com/questions/8847661/datagridtextcolumn-visibility-binding
    //https://blog.csdn.net/qq_20758141/article/details/80856217
    public class BoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                switch ((bool)value)
                {
                    case false:
                        return System.Windows.Visibility.Collapsed;
                    case true:
                        return System.Windows.Visibility.Visible;
                    default:
                        return System.Windows.Visibility.Collapsed;
                }
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolConverterInverse : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                switch ((bool)value)
                {
                    case true:
                        return System.Windows.Visibility.Collapsed;
                    case false:
                        return System.Windows.Visibility.Visible;
                    default:
                        return System.Windows.Visibility.Collapsed;
                }
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}