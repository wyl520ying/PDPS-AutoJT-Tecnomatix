using System.Windows;

namespace AutoJTMessageUtilities
{
    /// <summary>
    /// ServerBroadcastWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ServerBroadcastWindow : Window
    {
        public ServerBroadcastWindow(string message,string title = "")
        {
            InitializeComponent();
            this.richBox1.AppendText(message);

            if (!string.IsNullOrEmpty(title))
            {
                this.Title = title;
            }
        }
    }
}
