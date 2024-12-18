using System;
using System.Windows;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Ui;

namespace AutoJTTXCoreUtilities
{
    /// <summary>
    /// SetWorkingFrameWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SetWorkingFrameWindow : Window
    {
        TxFrameEditBoxCtrl_ValidFrameSetEventArgs _ValidFrameArgs;
        public Action<string> IsSetWorkingFrame;//之前的定义委托和定义事件由这一句话代替

        public SetWorkingFrameWindow(double x, double y)
        {
            InitializeComponent();
            this.Title = "Set Working Frame";

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
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txFrameEditBoxCtrl1.Focus();
            cUiContinuousButton_ok.IsEnabled = false;
            _ValidFrameArgs = null;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                this.txFrameEditBoxCtrl1.ValidFrameSet -= txFrameEditBoxCtrl1_ValidFrameSet;
                this.txFrameEditBoxCtrl1.UnInitialize();
                this.txFrameEditBoxCtrl1 = null;
            }
            catch
            {
            }
        }

        private void cUiContinuousButton_cancel_Click(object sender, RoutedEventArgs e)
        {
            IsSetWorkingFrame?.Invoke(null);//执行委托实例  
            this.Close();
        }
        private void txFrameEditBoxCtrl1_ValidFrameSet(object sender, TxFrameEditBoxCtrl_ValidFrameSetEventArgs args)
        {
            cUiContinuousButton_ok.IsEnabled = true;
            _ValidFrameArgs = args;
        }

        private void cUiContinuousButton_ok_Click(object sender, RoutedEventArgs e)
        {
            if (_ValidFrameArgs == null)
            {
                return;
            }


            //set working frame
            try
            {
                SetWorkFrameMethod(out string locationString);
                IsSetWorkingFrame?.Invoke(locationString);//执行委托实例  
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                IsSetWorkingFrame?.Invoke(null);//执行委托实例  
            }
        }

        void SetWorkFrameMethod(out string locationString)
        {
            locationString = null;
            TxDocument txDocument = TxApplication.ActiveDocument;
            txDocument.WorkingFrame = _ValidFrameArgs.Location;

            TxVector txVector = _ValidFrameArgs.Location.Translation;
            locationString = txVector.X.ToString() + txVector.Y.ToString() + txVector.Z.ToString();
        }



    }
}
