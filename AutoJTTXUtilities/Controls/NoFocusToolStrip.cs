using System.Windows.Forms;

namespace AutoJTTXUtilities.Controls
{
    public class NoFocusToolStrip : ToolStrip
    {
        protected override void WndProc(ref Message m)
        {
            if ((long)m.Msg == 514L && !NoFocusToolStrip.isLeftButtonDown)
            {
                m.Msg = 513;
                base.WndProc(ref m);
                m.Msg = 514;
            }
            if ((long)m.Msg == 513L)
            {
                NoFocusToolStrip.isLeftButtonDown = true;
            }
            if ((long)m.Msg == 514L)
            {
                NoFocusToolStrip.isLeftButtonDown = false;
            }
            base.WndProc(ref m);
        }

        private const uint WM_LBUTTONDOWN = 513U;

        private const uint WM_LBUTTONUP = 514U;

        private static bool isLeftButtonDown;
    }
}
