using AutoJTTXUtilities.Controls.InvitationWindow.ViewModels;
using System.Threading.Tasks;
using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Tecnomatix.Engineering.Ui.WPF;
using System.Threading;

namespace AutoJTTXUtilities.Controls.InvitationWindow
{
    /// <summary>
    /// Interaction logic for InvitationDialog.xaml
    /// </summary>
    public partial class InvitationDialog : TxWindow
    {
        InvitationDialogViewModel m_app;

        public InvitationDialog(string inviteCode,string downloadLink)
        {
            InitializeComponent();

            this.DataContext = this.m_app = new InvitationDialogViewModel(inviteCode, downloadLink);

            DisplayText();
        }

        private void DisplayText()
        {
            // 创建一个 FlowDocument
            FlowDocument document = new FlowDocument();

            // 创建一个段落 (Paragraph)
            Paragraph paragraph = new Paragraph();

            // 添加文本内容到段落
            paragraph.Inlines.Add(new Run("每位新用户输入推荐码并成功登录后，推荐人即可增加一个月AutoJT_TX专业版使用时长(重新登录后查看)。")
            {
                Foreground = Brushes.Black, // 设置文本颜色
                FontSize = 14 // 设置字体大小
            });

            // 在段落中插入一段有样式的文本
            paragraph.Inlines.Add(new LineBreak()); // 换行
            paragraph.Inlines.Add(new Run("AutoJT包含2D尺寸标注功能，也可以分享给设计同事。")
            {
                Foreground = Brushes.Black, // 设置文本颜色
                FontSize = 14,
                //FontWeight = FontWeights.Bold // 设置加粗
            });

            // 在段落中插入一段有样式的文本
            paragraph.Inlines.Add(new LineBreak()); // 换行
            paragraph.Inlines.Add(new Run("仿真插件: 导插枪、焊点清单、焊点参照分配、焊点截图等十几项功能，试用期一个月。软件右上角有分享功能")
            {
                Foreground = Brushes.Black, // 设置文本颜色
                FontSize = 14,
                //FontWeight = FontWeights.Bold // 设置加粗
            });

            // 在段落中插入一段有样式的文本
            paragraph.Inlines.Add(new LineBreak()); // 换行
            paragraph.Inlines.Add(new Run("设计插件: 组孔公差尺寸批量标注，最低版本要求AutoCAD2018，安装后输入JRH命令，按提示操作。此功能目前免费。")
            {
                Foreground = Brushes.Black, // 设置文本颜色
                FontSize = 14,
                //FontWeight = FontWeights.Bold // 设置加粗
            });

            // 将段落添加到文档中
            document.Blocks.Add(paragraph);

            // 将文档设置为 RichTextBox 的内容
            richTextBox.Document = document;
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private async void accept_btn_Click(object sender, RoutedEventArgs e)
        {
            await this.CopyLogical(this.m_app.ToString());
        }

        // 处理按键事件
        private void TxWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)  // 检查是否按下ESC键
            {
                this.Close();  // 关闭窗口
            }
        }


        #region 复制逻辑

        /// <summary>
        ///复制逻辑
        /// </summary>
        /// <returns></returns>
        async Task<bool> CopyLogical(string strTemp)
        {
            bool bl1 = false;

            try
            {
                //判断是否为空
                if (strTemp.Equals(""))
                    return false;

                await this.SetClipboard2(strTemp);//将文字添加到剪切板中，还添加Object类型数据

                //设置复制数据的标题
                //UNSetClipboardTitle(this.DataTable_Clipboard);

                bl1 = true;
            }
            catch
            {
                bl1 = false;
            }

            return bl1;
        }

        /// <summary>
        /// 复制文本到剪切板
        /// </summary>
        Task SetClipboard2(string text)
        {
            Task task = null;
            task = Task.Run(() =>
            {
                try
                {
                    Thread th = new Thread(new ThreadStart(delegate ()
                    {
                        try
                        {
                            Clipboard.Clear();//清除原有剪切板中内容
                            System.Windows.Clipboard.SetText(text);

                            Thread.Sleep(800);
                        }
                        catch
                        {
                        }
                    }));
                    th.TrySetApartmentState(ApartmentState.STA);
                    th.Start();
                    th.Join();
                }
                catch
                {
                }
            });

            return task;
        }

        #endregion
    }
}
