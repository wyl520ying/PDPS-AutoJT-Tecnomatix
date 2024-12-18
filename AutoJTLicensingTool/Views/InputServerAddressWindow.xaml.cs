using AutoJTLicensingTool.Common;
using AutoJTTXServiceUtilities;
#if PSV
using AutoJTTXUtilities.ConfigurationHandling;
#endif
using System;
using System.Collections.Generic;
using System.Windows;

namespace AutoJTLicensingTool.Views
{
    /// <summary>
    /// InputServerAddressWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InputServerAddressWindow : Window
    {
        public Action<bool> IsSuccess;
        public InputServerAddressWindow()
        {
            AJTDatabaseOperation.PSVAddress = null;

            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            this.Loaded += InputServerAddressWindow_Loaded;
            this.Closed += InputServerAddressWindow_Closed;
        }

        private void InputServerAddressWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadConfigurationFile();
        }

        private void InputServerAddressWindow_Closed(object sender, EventArgs e)
        {
            WriteConfigurationFile();
        }

        private async void btn1_Click(object sender, RoutedEventArgs e)
        {
            if (this.box1.Text != "")
            {
                #region 检查私服IP

                AJTDatabaseOperation.PSVAddress = this.box1.Text;
                //检查输入的IP是否通
                bool bl1 = await Global.TestNetworkConnectionAsync();

                if (!bl1)
                {
                    AJTDatabaseOperation.PSVAddress = null;
                    //执行委托
                    this.IsSuccess.Invoke(false);
                    MessageBox.Show(this, "地址错误");
                    return;
                }

                #endregion

                try
                {
                    //检查身份,同时检查服务器的连通性
                    if (!AutoJTTXServiceUtilities.AJTDatabaseOperation.CheckPrivateServersAuth(this.box2_code.Text))
                    {
                        /*
                        //执行委托
                        this.IsSuccess.Invoke(false);
                        MessageBox.Show(this, "验证码错误");
                        return;*/
                    }
                }
                catch (Exception ex)
                {
                    //执行委托
                    this.IsSuccess.Invoke(false);
                    MessageBox.Show(ex.Message);
                    return;
                }

                //执行委托
                this.IsSuccess.Invoke(true);
                this.Close();
            }
            else
            {
                MessageBox.Show(this, "请输入地址和验证码");
            }
        }


#if PSV        

        #region TxConfig

        //写出配置文件
        void WriteConfigurationFile()
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(delegate
                {
                    List<TxConfig> txConfigs = new List<TxConfig>();

                    TxConfig txConfig1 = new TxConfig("PSV_ServerAddress", this.box1.Text);
                    txConfigs.Add(txConfig1);

                    ConfigurationFileOperation configurationFileOperation = new ConfigurationFileOperation(AutoJTTXCoreUtilities.AJTTxApplicationUtilities.GetInstallationDirectory_dotNetCmd());
                    configurationFileOperation.WriteConfiguration(txConfigs);
                }));
            }
            catch
            {
            }
        }

        //加载配置文件
        void LoadConfigurationFile()
        {
            try
            {
                ConfigurationFileOperation _configurationFileOperation = new ConfigurationFileOperation(AutoJTTXCoreUtilities.AJTTxApplicationUtilities.GetInstallationDirectory_dotNetCmd());

                Dispatcher.BeginInvoke(new Action(delegate
                {
                    string txConfig1 = _configurationFileOperation.ReadConfiguration("PSV_ServerAddress");
                    if (string.IsNullOrEmpty(txConfig1))
                    {
                        this.box1.Text = "";
                    }
                    else
                    {
                        this.box1.Text = txConfig1;
                    }

                }));
            }
            catch
            {
            }
        }

        #endregion

#endif
    }
}
