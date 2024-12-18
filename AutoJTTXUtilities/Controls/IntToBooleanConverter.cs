using System;
using System.Data;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace AutoJTTXUtilities.Controls
{
    public class IntToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (int.TryParse(value.ToString(), out int sdfe))
                {
                    if (sdfe >= 1)
                    {
                        if (sdfe == 1)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }            
                }
                else
                {
                    return false;
                }          
            } 
            catch 
            { 
                return false;
            }        
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 如果值是true，则返回整数1；如果值是false，则返回整数-1
            return (bool)value ? "2" : "1";
        }
    }

    public class IntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (int.TryParse(value.ToString(), out int sdfe))
                {
                    if (sdfe >= 1)
                    {
                        if (sdfe == 1)
                        {
                            return "false";
                        }
                        else
                        {
                            return "true";
                        }
                    }
                    else
                    {
                        return "-";
                    }
                }
                else
                {
                    return "-";
                }
            }
            catch
            {
                return "-";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}