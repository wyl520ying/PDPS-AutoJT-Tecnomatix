using System.Drawing;
using System.Windows.Forms;

namespace AutoJTTXUtilities.Controls
{
    public class AJTSharedLogic
    {
        private static readonly Color ACTIVE_BUTTON_BORDER_COLOR = Color.FromArgb(51, 153, 255);

        private static readonly Color ACTIVE_BUTTON_BACKGROUND_COLOR = Color.FromArgb(137, 196, 255);
        public static void SetActiveControlState(CheckBox c)
        {
            try
            {
                if (c.Checked)
                {
                    c.FlatStyle = FlatStyle.Flat;
                    c.FlatAppearance.BorderSize = 1;
                    c.FlatAppearance.BorderColor = AJTSharedLogic.ACTIVE_BUTTON_BORDER_COLOR;
                    c.BackColor = AJTSharedLogic.ACTIVE_BUTTON_BACKGROUND_COLOR;
                    return;
                }
                c.FlatStyle = FlatStyle.Standard;
                c.BackColor = SystemColors.Control;
            }
            catch
            {


            }
        }
        public static void SetActiveControlState(RadioButton c)
        {
            try
            {
                if (c.Checked)
                {
                    //c.FlatStyle = FlatStyle.Flat;
                    c.FlatAppearance.BorderSize = 1;
                    c.FlatAppearance.BorderColor = AJTSharedLogic.ACTIVE_BUTTON_BORDER_COLOR;
                    c.BackColor = AJTSharedLogic.ACTIVE_BUTTON_BACKGROUND_COLOR;
                    return;
                }
                c.FlatStyle = FlatStyle.Standard;
                c.BackColor = SystemColors.Control;
            }
            catch
            {

                throw;
            }
        }
    }
}
