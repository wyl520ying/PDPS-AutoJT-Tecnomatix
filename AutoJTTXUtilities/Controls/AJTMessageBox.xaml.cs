using System;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace AutoJTTXUtilities.Controls
{
    /// <summary>
    /// AJTMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class AJTMessageBox : Window
    {
        public Action<bool> IsAccept { get; set; }
        bool m_isaccept;
        public AJTMessageBox(double x, double y, string tag, string msg, string checkTag, bool isShowCheckBox = false, bool isShowCancelBtn = true)
        {
            InitializeComponent();

            //tigle
            this.Title = tag;
            this.msgTextBlock.Text = msg;
            this.userAcceptCheckBox.Content = checkTag;

            #region checkBox 显示状态

            //checkBox
            if (!isShowCheckBox)
            {
                this.userAcceptCheckBox.Visibility = Visibility.Collapsed;
                this.userAcceptCheckBox.IsChecked = true;
            }
            else
            {
                this.userAcceptCheckBox.Visibility = Visibility.Visible;
            }

            #endregion

            #region cancel按钮状态

            if (!isShowCancelBtn)
            {
                this.cancel_btn.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.cancel_btn.Visibility = Visibility.Visible;
            }

            #endregion

            #region 窗口尺寸

            int ilength;
            try
            {
                int istrLng = 0;
                string lenmsg = msg;
                string[] choice = msg.Split('\n');
                foreach (string item in choice)
                {
                    if (istrLng < item.Length)
                    {
                        istrLng = item.Length;
                        lenmsg = item;
                    }
                }

                ilength = this.GetStrLength(lenmsg);
            }
            catch
            {
                ilength = msg.Length;
            }

            if (ilength <= 32)
            {
                this.Width = 350;
                this.MinWidth = 350;
                this.MaxWidth = 350;
            }
            else
            {
                int i_width = 350 + (ilength - 32) * 7;

                if (i_width < 350)
                {
                    i_width = 350;
                }

                this.Width = i_width;
                this.MinWidth = i_width;
                this.MaxWidth = i_width;
            }

            #endregion

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

            this.accept_btn.Focus();
            this.TabIndex = 0;
        }

        //获取长度方法
        private int GetStrLength(string str)
        {
            if (string.IsNullOrEmpty(str)) return 0;
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            byte[] s = ascii.GetBytes(str);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }
            }
            return tempLen;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            m_isaccept = false;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (this.IsAccept != null)
            {
                this.IsAccept?.Invoke(m_isaccept);
            }

            this.accept_btn.KeyDown -= this.ModifyPrice_KeyDown;
        }

        private void accept_btn_Click(object sender, RoutedEventArgs e)
        {
            this.m_isaccept = true;
            this.Close();
        }

        private void cancel_btn_Click(object sender, RoutedEventArgs e)
        {
            this.m_isaccept = false;
            this.Close();
        }

        private void ModifyPrice_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Space)
                {
                    this.accept_btn_Click(null, null);
                }
            }
            catch
            {
            }
        }
    }
}
