using System.Windows.Controls;
using System.Windows.Media;

namespace AutoJTTXUtilities.Controls
{
    public class AJTSharedLogicWPF
    {
        public static void SetActiveControlState(RadioButton c)
        {
            try
            {
                if (c.IsChecked == true)
                {
                    //c.BorderBrush= new SolidColorBrush(Color.FromRgb(137, 196, 255));//51, 153, 255
                    c.Background = new SolidColorBrush(Color.FromRgb(137, 196, 255));//137, 196, 255
                    return;
                }

                //c.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FFFFFF"));
                c.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FFFFFF"));
            }
            catch
            {
            }
        }
    }
}
