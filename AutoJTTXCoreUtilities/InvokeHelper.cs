using System.Windows.Forms;

namespace AutoJTTXCoreUtilities
{
    public static class InvokeHelper
    {
        public static void Invoke(this Control control, MethodInvoker action)
        {
            control.Invoke(action);
        }
    }
}
