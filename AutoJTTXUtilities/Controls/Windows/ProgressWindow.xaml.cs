using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AutoJTTXUtilities.Controls.Windows
{
    /// <summary>
    /// ProgressWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ProgressWindow : Window
    {
        private CancellationTokenSource _cancellationTokenSource;

        public ProgressWindow(CancellationTokenSource cancellationTokenSource, int maxValue)
        {
            InitializeComponent();
            _cancellationTokenSource = cancellationTokenSource;
            ProgressBar.Maximum = maxValue;  // 设置自定义最大值
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }

        public void UpdateProgress(int value)
        {
            ProgressBar.Value = value;

            if (value >= ProgressBar.Maximum)
            {
                this.Close();
            }
        }
    }
}