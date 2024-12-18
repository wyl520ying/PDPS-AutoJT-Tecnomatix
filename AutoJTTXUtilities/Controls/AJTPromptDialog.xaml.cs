using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Tecnomatix.Engineering.Ui.WPF;

namespace AutoJTTXUtilities.Controls
{
    /// <summary>
    /// AJTPromptDialog.xaml 的交互逻辑
    /// </summary>
    public partial class AJTPromptDialog : TxWindow
    {
        List<OpenFolderTagMsgHelper> m_gunCloudMsgHelpers;
        List<AJTMsgHelperBase> m_AJTMsgHelpers;

        void InitDialog()
        {
            InitializeComponent();
        }
        public AJTPromptDialog(string secondTitle, string title, List<OpenFolderTagMsgHelper> msgValue)
        {
            this.InitDialog();

            this.Title = title;
            if (string.IsNullOrEmpty(secondTitle))
            {
                this.txtQuestion_box.Visibility = Visibility.Collapsed;
            }
            this.txtQuestion_box.Text = secondTitle;
            this.m_gunCloudMsgHelpers = msgValue;

            // Create a FlowDocument  
            FlowDocument mcFlowDoc = new FlowDocument();
            foreach (OpenFolderTagMsgHelper item in this.m_gunCloudMsgHelpers)
            {
                try
                {
                    this.AddRichParagraph(out Paragraph para, item.m_MsgLevel, item.m_Message, item.m_Tag);
                    // Add the paragraph to blocks of paragraph  
                    mcFlowDoc.Blocks.Add(para);
                }
                catch
                {
                    continue;
                }
            }
            mcFlowDoc.LineHeight = 3;
            this.message_textbox.Document = mcFlowDoc;

        }
        public AJTPromptDialog(string secondTitle, string title, List<AJTMsgHelperBase> msgValue)
        {
            this.InitDialog();

            this.Title = title;
            if (string.IsNullOrEmpty(secondTitle))
            {
                this.txtQuestion_box.Visibility = Visibility.Collapsed;
            }
            this.txtQuestion_box.Text = secondTitle;
            this.m_AJTMsgHelpers = msgValue;

            // Create a FlowDocument  
            FlowDocument mcFlowDoc = new FlowDocument();
            foreach (AJTMsgHelperBase item in this.m_AJTMsgHelpers)
            {
                try
                {
                    this.AddRichParagraph(out Paragraph para, item.m_MsgLevel, item.m_Message);
                    // Add the paragraph to blocks of paragraph  
                    mcFlowDoc.Blocks.Add(para);
                }
                catch
                {
                    continue;
                }
            }
            mcFlowDoc.LineHeight = 3;
            this.message_textbox.Document = mcFlowDoc;

        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = new bool?(true);
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        void AddRichParagraph(out Paragraph para, string headerText, string bodyText, string lastText = "")
        {


            // Paragraph 类似于 html 的 P 标签
            para = new Paragraph();
            // Run 是一个 Inline 的标签

            if (!string.IsNullOrEmpty(headerText))
            {
                headerText = string.Format("{0}: ", headerText.Trim());

                //Bold bold = new Bold(new Run(headerText));
                //bold.Foreground= new SolidColorBrush(Colors.Red);

                Run runHEADER = new Run(headerText);
                if (headerText.IndexOf("Error") != -1)
                {
                    runHEADER.Foreground = new SolidColorBrush(Colors.Red);
                }
                else if (headerText.IndexOf("Info") != -1)
                {
                    runHEADER.Foreground = new SolidColorBrush(Colors.Blue);
                }

                para.Inlines.Add(runHEADER);
            }
            if (!string.IsNullOrEmpty(bodyText))
            {
                bodyText = string.Format(" {0} ", bodyText.Trim());
                para.Inlines.Add(new Run(bodyText));
            }

            if (!string.IsNullOrEmpty(lastText))
            {
                lastText = lastText.Trim();

                Hyperlink link = new Hyperlink
                {
                    IsEnabled = true,
                    Foreground = new SolidColorBrush(Colors.Blue),
                    NavigateUri = new Uri(lastText),
                    Cursor = Cursors.Hand
                };
                link.Inlines.Add("点击打开COJT文件夹");

                link.RequestNavigate += (sender, args) => Process.Start(args.Uri.ToString());

                //定义鼠标经过事件
                link.MouseEnter += (sender, e) =>
                {
                    link.Cursor = Cursors.Hand;
                };

                //定义click事件
                link.MouseDown += (sender, e) =>
                {
                    //do someting...
                    try
                    {
                        System.Diagnostics.Process.Start(lastText);
                    }
                    catch
                    {
                    }
                };

                para.Inlines.Add(link);
            }
        }

        private void TxWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                this.Close();
            }
        }
    }
}