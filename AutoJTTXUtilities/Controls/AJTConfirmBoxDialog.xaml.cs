using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tecnomatix.Engineering.Ui.WPF;

namespace AutoJTTXUtilities.Controls
{
    /// <summary>
    /// AJTConfirmBoxDialog.xaml 的交互逻辑
    /// </summary>
    public partial class AJTConfirmBoxDialog : TxWindow
    {
        public Action<bool> IsAccept { get; set; }
        bool m_isaccept;
        public AJTConfirmBoxDialog(string tag, string msg, string checkTag)
        {
            InitializeComponent();

            this.Title = tag;
            this.msgTextBlock.Text = msg;
            this.userAcceptCheckBox.Content = checkTag;

            int ilength = msg.Length;
            try
            {
                //用换行符分割
                List<string> tChoose = msg.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();
                //降序
                string ichose = tChoose.OrderByDescending(t => t.Length).FirstOrDefault();
                //找到最长的这一行
                ilength = this.GetStrLength(ichose);
            }
            catch
            {
                ilength = msg.Length;
            }

            if (ilength < checkTag.Length)
            {
                ilength = checkTag.Length;
            }

            if (ilength <= 35)
            {
                this.Width = 365;
                this.MinWidth = 365;
                this.MaxWidth = 365;
            }
            else
            {
                int i_width = 365 + (ilength - 30) * 7;

                if (i_width < 365)
                {
                    i_width = 365;
                }

                this.Width = i_width;
                this.MinWidth = i_width;
                this.MaxWidth = i_width;
            }
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

        private void Window_Closed(object sender, EventArgs e)
        {
            this.IsAccept?.Invoke(m_isaccept);
        }

        private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            m_isaccept = false;
        }

        private void accept_btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.m_isaccept = true;
            this.Close();
        }

        private void cancel_btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.m_isaccept = false;
            this.Close();
        }
    }
}
