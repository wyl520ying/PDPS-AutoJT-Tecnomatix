





using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace AutoJTTXUtilities.Controls
{
  public class BoolConverterInverse : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null)
        return (object) false;
      return (bool) value ? (object) Visibility.Collapsed : (object) Visibility.Visible;
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
