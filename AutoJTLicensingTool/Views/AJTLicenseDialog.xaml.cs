using AutoJTLicensingTool.Common;
using AutoJTLicensingTool.Messages;
using AutoJTLicensingTool.ViewModels;
using AutoJTMessageUtilities;
using AutoJTTXCoreUtilities;
using AutoJTTXUtilities.Controls;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AutoJTLicensingTool.Views
{
    /// <summary>
    /// AJTLicenseDialog.xaml 的交互逻辑
    /// </summary>
    public partial class AJTLicenseDialog : AJTBaseWindow,
         IRecipient<LoginSuccessModelMessage>
    {
        #region Constructor

        //用于记录用户是否重新扫码登录过
        public Action<bool> IsReLoginUsers;

        private LoginViewModel Context = null;
        public AJTLicenseDialog() : base()
        {
            InitializeComponent();

            this.Context = new LoginViewModel(false);
            base.DataContext = Context;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {

            int itestNet = default;
            do
            {
                itestNet++;

                //检查网络连接
                if (await Global.TestNetworkConnectionAsync())
                {
                    //网络畅通
                    break;
                }
                else
                {
                    if (itestNet == 5)
                    {
                        MessageBox.Show(this, "网络链接失败", "AutoJT", MessageBoxButton.OK);
                        this.Close();
                        return;
                    }
                }

            } while (itestNet < 5);

            await RegSendMethod();
        }

        // 注册命令
        private Task RegSendMethod()
        {
            return Task.Run(async () =>
            {
                WeakReferenceMessenger.Default.Register<LoginSuccessModelMessage>(this);
                //WeakReferenceMessenger.Default.Send("AJTLicenseDialog_Init");

                //如果上面的调用失败, 就手动调用
                if (!this.Context.m_firstInit)
                {
                    await this.Context.InitialMethod(false);
                }
            });
        }

        #endregion

        #region 登录成功的回调函数

        public async void Receive(LoginSuccessModelMessage message)
        {
            await LoginSuccedMethod();
        }

        //登录成功的方法
        private Task LoginSuccedMethod()
        {
            return Task.Run(() =>
            {
                //Application.Current?.
                base.Dispatcher.Invoke(new Action(() =>
                {
                    //退出登录
                    try
                    {
                        //身份验证
                        ServiceClient.CheckIdentity = false;
                        //退出双工通信
                        ServiceClient.ExitCommunication(Global.m_serviceClient);                        
                    }
                    catch
                    {
                    }

                    try
                    {
                        //开启双工通信
                        Global.m_serviceClient = new ServiceClient(GlobalClass.user.strUsrId, GlobalClass.user.strUsrName, GlobalClass.RegId);

                        //身份验证(避免重复扫码)
                        ServiceClient.CheckIdentity = true;

                        //执行委托
                        this.IsReLoginUsers?.Invoke(true);

                        //登录成功
                        this.DialogResult = true;
                        
                        base.Close();
                    }
                    catch
                    {
                        
                    }
                }));
            });
        }

        #endregion

        
    }
}