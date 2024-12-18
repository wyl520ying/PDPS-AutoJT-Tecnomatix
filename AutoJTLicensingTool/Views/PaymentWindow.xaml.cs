using AutoJTLicensingTool.Common;
using AutoJTLicensingTool.Messages;
using AutoJTLicensingTool.Pages;
using AutoJTLicensingTool.ViewModels;
using AutoJTTXCoreUtilities;
using AutoJTTXUtilities.Controls;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AutoJTLicensingTool.Views
{
    /// <summary>
    /// PaymentWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PaymentWindow : Window, IRecipient<PaySuccessMessage>
    {
        //是否支付成功
        bool isPaySucceed = false;
        public Action<bool> IsPaySucceed;

        List<Page> Pages = new List<Page>();
        PaymentWindowModel _app;
        public PaymentWindow(string fun_id)
        {
            InitializeComponent();
            Pages.Add(new ProductPage(this,fun_id));

            //this.Title += $" - {GlobalClass.NickName}";
            this.DataContext = this._app = new PaymentWindowModel();

            base.Closed += Login_Closed;

            #region 加载产品页面

            MainFrame.Navigate(Pages[0]);

            #endregion

            //注册支付成功回调函数
            WeakReferenceMessenger.Default.Register<PaySuccessMessage>(this);
            //初始化main window
            WeakReferenceMessenger.Default.Send("MainWindow_Init");
        }

        #region 窗体关闭事件

        //窗体关闭事件
        private async void Login_Closed(object sender, EventArgs e)
        {
            this.IsPaySucceed?.Invoke(isPaySucceed);
            await this.RsetMethod();
        }

        //重置reset Task
        private Task RsetMethod()
        {
            return Task.Run(async () =>
            {
                try
                {
                    WeakReferenceMessenger.Default?.Reset();
                    //Global.Hub?.DisposeAsync();
                    await Global.DisposeAsync();
                    Global.Hub = null;
                    Global.Auth2Success = null;
                    GlobalClass.LoginId = null;
                }
                catch
                {
                }
            });
        }

        #endregion

        #region 支付成功的回调

        //支付成功的回调
        void IRecipient<PaySuccessMessage>.Receive(PaySuccessMessage message)
        {
            //支付成功
            isPaySucceed = true;
            Dispatcher.BeginInvoke(new Action(delegate
            {
                this.Close();

            }));
        }

        #endregion
    }
}