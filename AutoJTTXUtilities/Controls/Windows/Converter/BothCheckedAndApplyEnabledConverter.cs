using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AutoJTTXUtilities.Controls.Windows.Converter
{
    public class BothCheckedAndApplyEnabledConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2//3
                //&& values[0] is bool collisionMode
                && values[0] is bool distributionCheckBox
                && values[1] is bool applyIsEnabled)
            {
                return distributionCheckBox && applyIsEnabled;//collisionMode && distributionCheckBox && applyIsEnabled;
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
