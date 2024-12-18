using System;
using System.Threading;
using System.Windows;

namespace AutoJTTXUtilities.Controls
{
    public class AJTProgressWindowBase : System.Windows.Window
    {
        protected CancellationTokenSource currentCancellationSource;

        public AJTProgressWindowBase(double x, double y)
        {
            #region 窗口位置

            if (x == 0 && y == 0)
            {
                this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            else
            {
                //启用‘Manual’属性后，可以手动设置窗体的显示位置
                this.WindowStartupLocation = WindowStartupLocation.Manual;
                this.Top = x - this.Height / 2;
                this.Left = y - this.Width / 2;
            }

            #endregion

            this.Closed += Window_Closed;
        }

        //窗口关闭事件
        private void Window_Closed(object sender, EventArgs e)
        {
            // Cancel the cancellation token
            if (this.currentCancellationSource != null)
            {
                this.currentCancellationSource.Cancel();
                this.currentCancellationSource.Dispose();
                this.currentCancellationSource = null;
            }
        }
    }
}