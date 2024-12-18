





using System;
using System.Globalization;
using System.Windows.Data;


namespace AutoJTTXUtilities.Controls
{
  public class IntToStringConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      try
      {
        int result;
        if (!int.TryParse(value.ToString(), out result) || result < 1)
          return (object) "-";
        return result == 1 ? (object) "false" : (object) "true";
      }
      catch
      {
        return (object) "-";
      }
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
