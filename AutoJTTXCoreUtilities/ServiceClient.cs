using AutoJTTecnomatix.AutoJTServiceReference;
using System;
using System.ServiceModel;

namespace AutoJTTXCoreUtilities
{
    #region 双工通信

    public class ServiceClient : AutoJTMessageService.IMessageServiceCallback
    {
        AutoJTMessageService.MessageServiceClient mServiceClient = null;

        int icount;

        int message_interval = 90000;

        public System.Timers.Timer timer = new System.Timers.Timer();

        public string m_cilentInfos = string.Empty;

        string m_current_openID = string.Empty;

        void MessageServiceClientMethod()
        {
            bool isping = TxDatabaseOperation.CheckNetworkConnection();//检查网络连接
            if (!isping)
            {
                throw new Exception("网络故障");
            }

            System.ServiceModel.InstanceContext context = new System.ServiceModel.InstanceContext(this);

            //设置netTCP协议
            NetTcpBinding tcpBinding = new NetTcpBinding();
            tcpBinding.MaxBufferPoolSize = 2147483647;
            tcpBinding.MaxReceivedMessageSize = 2147483647;
            tcpBinding.MaxBufferSize = 2147483647;
            //提供安全传输
            tcpBinding.Security.Mode = SecurityMode.None;
            //需要提供证书
            tcpBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            EndpointAddress endPointAddress = new EndpointAddress("net.tcp://www.autojt.com:9900/");


            this.mServiceClient = new AutoJTMessageService.MessageServiceClient(context, tcpBinding, endPointAddress);
        }

        public ServiceClient(string current_openID)
        {
            this.m_current_openID = current_openID;


            if (this.mServiceClient == null)
            {
                this.MessageServiceClientMethod();
            }

            if (string.IsNullOrEmpty(this.m_cilentInfos))
            {
                this.m_cilentInfos = GetEnvironment.getMNum();
            }

            this.mServiceClient.Register(GlobalClass.user.strUsrName, this.m_cilentInfos, this.m_current_openID);


            this.timer.Interval = message_interval;
            this.timer.Enabled = true;
            this.timer.Elapsed += Timer_Elapsed1;
        }

        void Timer_Elapsed1(object sender, System.Timers.ElapsedEventArgs e)
        {

            if (this.mServiceClient.State != CommunicationState.Opened)
            {
                this.timer.Stop();

                //检查当前客户端是否可以断线重连
                if (this.CheckUserLoginStatus())
                {
                    try
                    {
                        //重连成功
                        icount++;
                        string msg = icount.ToString();
                        this.mServiceClient.ClientSendMessage(msg, GlobalClass.user.strUsrName);
                    }
                    catch
                    {
                    }

                    return;
                }

                #region 网络链不上

                //检查网络连接
                bool isping = TxDatabaseOperation.CheckNetworkConnection(5, false);
                //网络故障
                if (!isping)
                {
                    //退出登录
                    AutoJTTecnomatix._0.License.LoginScanWindow.CheckIdentity = false;
                    return;
                }

                #endregion

                //Tecnomatix.Engineering.TxMessageBox.ShowModal("您已退出登录",
                //"AutoJT", MessageBoxButtons.OK, MessageBoxIcon.Error);

                try
                {
                    System.Threading.Tasks.Task.Factory.StartNew(ExitProcess);//异步退出程序
                }
                catch
                {
                    //退出登录
                    AutoJTTecnomatix._0.License.LoginScanWindow.CheckIdentity = false;
                }
            }
            else
            {
                try
                {
                    icount++;
                    string msg = icount.ToString();
                    this.mServiceClient.ClientSendMessage(msg, GlobalClass.user.strUsrName);
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// 收到服务器消息
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(string message)
        {
            //退出登录
            if (message.IndexOf("EXIT") != -1)
            {
                this.timer.Stop();

                System.Windows.MessageBox.Show("您的账号'" + GlobalClass.user.strUsrName + "'" + "已在另一台设备上登录",
                    "No license", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);

                try
                {
                    System.Threading.Tasks.Task.Factory.StartNew(ExitProcess);//异步退出程序
                }
                catch
                {
                    //退出登录
                    AutoJTTecnomatix._0.License.LoginScanWindow.CheckIdentity = false;
                }
            }
            //其他广播信息
            else if (message.IndexOf("你好，我是服务器") == -1)
            {
                try
                {
                    _995.ServerBroadcast.ServerBroadcastWindow serverBroadcastWindow = new _995.ServerBroadcast.ServerBroadcastWindow(message);
                    serverBroadcastWindow.ShowDialog();
                }
                catch
                {
                }
            }
        }

        void ExitProcess()
        {
            //退出登录
            AutoJTTecnomatix._0.License.LoginScanWindow.CheckIdentity = false;
        }

        #region 退出通信

        public static void ExitCommunication(ServiceClient serviceClient)
        {
            try
            {
                if (serviceClient != null && serviceClient.mServiceClient != null)
                {
                    serviceClient.mServiceClient.Close();
                    serviceClient.mServiceClient = null;
                    serviceClient.timer.Stop();
                    serviceClient = null;
                }
            }
            catch
            {
            }
        }

        #endregion

        #region 断线重连


        /// <summary>
        /// 检查当前客户端是否断线重连成功
        /// </summary>
        bool CheckUserLoginStatus()
        {
            bool result = false;

            //断线重连
            bool bl101 = false;

            #region 检查是否可以断线重连

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                //如果因为网络故障断开连接, 尝试链接5分钟, 超时退出登录
                for (int i = 0; i < 300; i++)
                {
                    try
                    {
                        //新建实例
                        autoJTServiceClient = TxDatabaseOperation.ServiceClientMethod(false);

                        //判断是否可以断线重连
                        bl101 = autoJTServiceClient.GetCurrentClientLoginStatus(GlobalClass.user.strUsrName, this.m_cilentInfos, this.m_current_openID);

                        //可以断线重连
                        if (bl101)
                        {
                            this.mServiceClient = null;
                            this.MessageServiceClientMethod();

                            this.mServiceClient.Register(GlobalClass.user.strUsrName, this.m_cilentInfos, this.m_current_openID);

                            this.timer.Stop();

                            this.timer.Interval = message_interval;
                            this.timer.Enabled = true;
                            this.timer.Elapsed += Timer_Elapsed1;

                            //断线重连成功
                            result = true;

                            break;
                        }
                    }
                    catch
                    {
                        System.Threading.Thread.Sleep(1000);
                        continue;
                    }

                    System.Threading.Thread.Sleep(1000);
                }
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            #endregion       

            return result;
        }

        #endregion

        #region 客户端是自动登录

        public ServiceClient()
        {
            if (string.IsNullOrEmpty(this.m_cilentInfos))
            {
                this.m_cilentInfos = GetEnvironment.getMNum();
            }
        }

        //检测当前机器码, 在服务器上的openid 和 昵称
        public bool CheckCurrentMachine(out string USERNAE, out string _Openid)
        {
            bool result = false;

            USERNAE = string.Empty;
            _Openid = string.Empty;

            #region 检查是否自动登录 id和昵称

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                //新建实例
                autoJTServiceClient = TxDatabaseOperation.ServiceClientMethod();

                USERNAE = autoJTServiceClient.GetCurrentMachineLoginCode(this.m_cilentInfos, out _Openid);

                result = true;
            }
            catch
            {
                USERNAE = string.Empty;
                _Openid = string.Empty;

                result = false;
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            #endregion

            return result;
        }

        #endregion

    }

    #endregion
}