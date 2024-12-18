using AutoJTLicensingTool.Common;
using AutoJTLicensingTool.Messages;
using AutoJTLicensingTool.ViewModels;
using AutoJTMessageUtilities;
using AutoJTTXCoreUtilities;
using AutoJTTXServiceUtilities;
using AutoJTTXUtilities.ConfigurationHandling;
using AutoJTTXUtilities.Controls;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutoJTLicensingTool.Views
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : AJTBaseWindow,
        IRecipient<LoginSuccessModelMessage>,
        IRecipient<CanAutoLoginResultModelMessage>
    {
        #region Constructor

        private LoginViewModel Context = null;
        private Assembly m_ParentAssembly = null;

        public Login(string _currVer, Assembly parentAssembly) : base()
        {
            //用于静默更新
            this.m_ParentAssembly = parentAssembly;

            //检查插件版本是否正确
            this.CheckInputVersion(_currVer);
            //插件当前版本
            GlobalClass.CurrentVersion = _currVer;

            //重置hub
            this.ResetHubMethod();

            //赋值内部版本
#if INTERNAL

            GlobalClass.ParseModels(true);

#endif

            InitializeComponent();

#if !PSV
            this.Context = new LoginViewModel();
            base.DataContext = Context;
#endif

            base.Closed += Login_Closed;
            this.Loaded += Login_Loaded;

#if !PSV
            //检查版本号   
            System.Threading.Tasks.Task.Factory.StartNew(CheckCurrentVersion);
#endif
        }

        #region 登录

        //登录成功的回调函数
        public async void Receive(LoginSuccessModelMessage message)
        {
            await LoginSuccedMethod();

            // 使用 Task.Run 启动一个新的线程来调用 GetInviCode 方法，主线程不等待
            Task.Run(async () =>
            {
                // 调用异步方法并处理返回值（获取用户的邀请码） 
                //GlobalClass.InvitationCode = await AJTDatabaseOperation.GetInviCode(new AutoJTTXServiceUtilities.Structure.NewUserInviCode2() { NewUserUUID = GlobalClass.user.strUsrId });
            });
        }
        //自动登录的回调函数
        public async void Receive(CanAutoLoginResultModelMessage message)
        {
            await LoginSuccedMethod();

            // 使用 Task.Run 启动一个新的线程来调用 GetInviCode 方法，主线程不等待
            Task.Run(async () =>
            {
                // 调用异步方法并处理返回值（获取用户的邀请码） 
                //GlobalClass.InvitationCode =await AJTDatabaseOperation.GetInviCode(new AutoJTTXServiceUtilities.Structure.NewUserInviCode2() { NewUserUUID = GlobalClass.user.strUsrId });
            });

        }
        //登录成功的方法
        private Task LoginSuccedMethod()
        {
            return Task.Run(() =>
            {
                //Application.Current?.
                base.Dispatcher.Invoke(new Action(() =>
                {
                    try
                    {
                        //开启双工通信
                        Global.m_serviceClient = new ServiceClient(GlobalClass.user.strUsrId, GlobalClass.user.strUsrName, GlobalClass.RegId);

#if EXTERNAL

                        //外部版本试用版弹窗
                        this.IsTrialVersionPromptTrialExpiry();

#endif

                        //登录成功
                        this.DialogResult = true;
                        //MainWindow mainWindow = new MainWindow();
                        //mainWindow.Show();
                        base.Close();

                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            //登录成功
                            this.DialogResult = false;
                            //退出登录
                            ServiceClient.CheckIdentity = false;
                            //退出双工通信
                            ServiceClient.ExitCommunication(Global.m_serviceClient);
                            System.Windows.MessageBox.Show(this, ex.Message, "AutoJT", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Asterisk);

                            //清空static数据
                            GlobalClass.ClearCache();
                        }
                        catch
                        {
                        }
                    }
                }));
            });
        }

#if EXTERNAL

        //外部版,试用期首次登录提醒,判断如果是试用版,并且没有弹窗提醒过, 就弹窗提醒试用版和到期日, 提醒完成后记录配置文件, 下次登录检查配置文件不再提醒
        void IsTrialVersionPromptTrialExpiry()
        {
            //判断是否是试用版
            if (!GlobalClass.VersionDesc.Equals("试用版"))
            {
                return;
            }

            //检查配置文件
            string sfe = LoadConfigurationFile();
            //没有弹窗
            if (sfe != "Y")
            {
                //开始弹窗
                //"您的软件试用期将于 [到期日期] 到期，请注意及时更新许可证以确保持续使用。到期后，您仍可继续使用试用版功能。"
                MessageBox.Show(this, $"您的试用版将于 [{GlobalClass.ExpireDate.ToString("yyy-MM-dd")}] 到期，到期后，您仍可继续使用免费版。","AutoJT",MessageBoxButton.OK,MessageBoxImage.Information);

                this.WriteConfigurationFile("Y");
            }
        }

        #region TxConfig

        //写出配置文件
        void WriteConfigurationFile(string infow)
        {
            try
            {
                List<TxConfig> txConfigs = new List<TxConfig>();

                Dispatcher.Invoke(() =>
                {
                    TxConfig txConfig1 = new TxConfig("TrialVersionPromptTrialExpiry", infow);
                    txConfigs.Add(txConfig1);
                });

                ConfigurationFileOperation configurationFileOperation = new ConfigurationFileOperation(AutoJTTXCoreUtilities.AJTTxApplicationUtilities.GetInstallationDirectory_dotNetCmd());
                configurationFileOperation.WriteConfiguration(txConfigs);
            }
            catch
            {
            }
        }

        //加载配置文件
        string LoadConfigurationFile()
        {
            string retsfasf = string.Empty;
            try
            {
                ConfigurationFileOperation _configurationFileOperation = new ConfigurationFileOperation(AutoJTTXCoreUtilities.AJTTxApplicationUtilities.GetInstallationDirectory_dotNetCmd());

                Dispatcher.Invoke(() =>
                {
                    retsfasf = _configurationFileOperation.ReadConfiguration("TrialVersionPromptTrialExpiry");
                });
            }
            catch
            {
                retsfasf = null;
            }

            return retsfasf;
        }

        #endregion

#endif

        #endregion

        //窗体加载
        private async void Login_Loaded(object sender, RoutedEventArgs e)
        {
#if PSV
            try
            {
                InputServerAddressWindow inputServerAddressWindow = new InputServerAddressWindow();
                bool isscc = false;
                WindowHelper.SetOwner(inputServerAddressWindow, this);
                inputServerAddressWindow.IsSuccess = (siss) => isscc = siss;
                inputServerAddressWindow.ShowDialog();

                if (!isscc)
                {
                    throw new Exception("请输入服务地址或验证码");
                }

                if (string.IsNullOrEmpty(AJTDatabaseOperation.PSVAddress))
                {
                    throw new Exception("请输入服务地址");
                }

                DateTime dt1 = Convert.ToDateTime("2025-11-20");//结束时间
                                                                //第一种方式
                TimeSpan td = dt1.Subtract(DateTime.Now);
                if (td.Days < 0)
                {
                    throw new Exception("请输入服务地址");
                }


                //赋值内部私服版本
                GlobalClass.ParseModels(true);

                //赋值内部版本私服用户名
                GlobalClass.Internal_tag = $"内部私服用户{System.Environment.MachineName}";
                GlobalClass.user.strUsrId = GlobalClass.user.strUsrName = GlobalClass.NickName = System.Environment.MachineName;

                //开启双工
                await LoginSuccedMethod();

                if (this.DialogResult !=true)
                {
                    //登录成功
                    this.DialogResult = true;
                    //MainWindow mainWindow = new MainWindow();
                    //mainWindow.Show();
                    base.Close();
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message);
            }          

            return;
#endif

            //运行前检查Tune.exe.config的runtime节点
            string cfgPath = System.IO.Path.Combine(AJTTxApplicationUtilities.GetInstallationDirectory(), "Tune.exe.config");
            if (!await AutoJTTXUtilities.ConfigurationHandling.XmlFiles.CheckTunExeCfgComplete(cfgPath))
            {
                //尝试添加节点
                if (!await AutoJTTXUtilities.ConfigurationHandling.XmlFiles.TunExeCfgEditer(cfgPath))
                {
                    //节点添加失败
                    throw new Exception("缺少必要的文件, 请重新安装插件");
                }
                else
                {
                    //节点添加成功, 提示用户重启Tecnomatix
                    throw new Exception("缺少必要的文件但已修复, 请重启Tecnomatix");
                }
            }


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

#if EXTERNAL

            //外部版本的登录弹窗
            try
            {
                var jsonvar = Task.Run(async () => await AJTDatabaseOperation.GetPopularMsg());
                //解析json
                Dictionary<string, string> mydic = Global.PearsPop(jsonvar.Result);

                if (mydic != null && mydic.Count > 0)
                {
                    foreach (var item in mydic)
                    {
                        try
                        {
                            ServerBroadcastWindow serverBroadcastWindow = new ServerBroadcastWindow(item.Value, item.Key);
                            WindowHelper.SetOwner(serverBroadcastWindow, AutoJTTXCoreUtilities.TecnomatixInfos.GetAppHandle());
                            serverBroadcastWindow.ShowDialog();
                            #region 模块统计
                            try
                            {
                                AJTDatabaseOperation.ModelInfosSum(new PluginInfoStatistics("登录弹窗", $"弹窗成功 - {item.Key}", $"{GlobalClass.RegId}|{GlobalClass.NickName}", GlobalClass.SoftWareHostVersion, string.Format("{0} {1} {2}", GlobalClass.VersionDesc, GlobalClass.CurrentVersion, GlobalClass.ExpireDate)).CreateDatatable());
                            }
                            catch
                            {

                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            #region 模块统计
                            try
                            {
                                AJTDatabaseOperation.ModelInfosSum(new PluginInfoStatistics("登录弹窗", $"未知异常 {ex.Message} - {item.Key}", $"{GlobalClass.RegId}|{GlobalClass.NickName}", GlobalClass.SoftWareHostVersion, string.Format("{0} {1} {2}", GlobalClass.VersionDesc, GlobalClass.CurrentVersion, GlobalClass.ExpireDate)).CreateDatatable());
                            }
                            catch
                            {

                            }
                            #endregion
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                #region 模块统计
                try
                {
                    AJTDatabaseOperation.ModelInfosSum(new PluginInfoStatistics("登录弹窗", $"未知异常{ex.Message}", $"{GlobalClass.RegId}|{GlobalClass.NickName}", GlobalClass.SoftWareHostVersion, string.Format("{0} {1} {2}", GlobalClass.VersionDesc, GlobalClass.CurrentVersion, GlobalClass.ExpireDate)).CreateDatatable());
                }
                catch
                {

                }
                #endregion
            }
#endif
        }
        //注册命令
        private Task RegSendMethod()
        {
            return Task.Run(async () =>
            {
                WeakReferenceMessenger.Default.Register<LoginSuccessModelMessage>(this);
                WeakReferenceMessenger.Default.Register<CanAutoLoginResultModelMessage>(this);

                WeakReferenceMessenger.Default.Send("Login_Init");

                //如果上面的调用失败, 就手动调用
                if (!this.Context.m_firstInit)
                {
                    await this.Context.InitialMethod();
                }
            });
        }

        //窗体关闭事件
        private void Login_Closed(object sender, EventArgs e)
        {
            this.ResetHubMethod();
        }


        //重置reset
        private async void ResetHubMethod()
        {
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

        #region 检查版本和自动更新

        //检查插件版本是否正确
        private Task<bool> CheckInputVersion(string ver)
        {
            return Task.Run(() =>
            {
                bool result = false;

                if (Version.TryParse(ver, out Version verResult))
                {
                    result = true;
                }
                else
                {
                    result = false;
                    AJTDatabaseOperation.ModelInfosSum(new PluginInfoStatistics("非法访问", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, $"{GlobalClass.RegId}|{GlobalClass.NickName}", GlobalClass.SoftWareHostVersion, string.Format("{0} {1} {2}", GlobalClass.VersionDesc, GlobalClass.CurrentVersion, GlobalClass.ExpireDate)).CreateDatatable());
                    throw new Exception("未知异常, 禁止登录");
                }

                return result;
            });
        }

        //检查版本号
        private async void CheckCurrentVersion()
        {
            try
            {
                //获取服务器最新版本号
                var lastVer = await this.CheckUpdate();
                string lastReleaseVer = GlobalClass.LastVersion = lastVer.lastReleaseVer;
                string VERSIONCONTENTS = lastVer.VERSIONCONTENTS;
                string FORCEDUPDATE = lastVer.FORCEDUPDATE;
                string downloadLink1 = GlobalClass.LastVersionDownloadLink = lastVer.downloadLink;
                string downloadLink2 = lastVer.downloadLink2;

                //新旧版本对比
                if (this.CheckVersion(lastReleaseVer, GlobalClass.CurrentVersion, out bool? differences))
                {
                    //更新内容
                    string[] verContents = VERSIONCONTENTS.Split('|');
                    string verContent1 = string.Empty;
                    foreach (string item in verContents)
                    {
                        if (verContent1 == string.Empty)
                        {
                            verContent1 = item;
                        }
                        else
                        {
                            verContent1 = verContent1 + "\n" + item;
                        }
                    }

                    //是否强制更新
                    if (bool.Parse(FORCEDUPDATE))
                    {
                        Dispatcher.Invoke(
                            new Action(
                                delegate
                                {
                                    this.Close();
                                    //退出登录
                                    ServiceClient.CheckIdentity = false;
                                    //退出通信
                                    ServiceClient.ExitCommunication(Global.m_serviceClient);
                                }
                            )
                        );
                    }

                    //是否大版本                 
                    Dispatcher.Invoke(
                          new Action(
                              delegate
                              {
                                  if (differences == true)
                                  {
                                      this.Close();
                                  }
                              }
                          )
                      );


                    //是否大版本    
                    if (differences == true)
                    {
                        Dispatcher.Invoke(
                            new Action(
                                delegate
                                {
                                    try
                                    {
                                        this.Close();
                                    }
                                    catch
                                    {
                                    }

                                    List<AutoJTTXUtilities.Controls.AJTMsgHelperBase> msgValue = new List<AutoJTTXUtilities.Controls.AJTMsgHelperBase>();

                                    StringBuilder sb12 = new StringBuilder();
                                    sb12.AppendLine("插件最新版本已上线。新版修正了诸多问题，提升了用户体验。建议将插件升级至最新版本。");
                                    sb12.AppendLine();
                                    sb12.Append(string.Format("当前版本：{0}   ", GlobalClass.CurrentVersion));
                                    sb12.Append(string.Format("最新版本：{0}", lastReleaseVer));
                                    sb12.AppendLine();
                                    sb12.AppendLine("更新内容：");
                                    sb12.AppendLine(verContent1);
                                    sb12.AppendLine(string.Format("新版本下载链接：{0}", downloadLink1));

                                    msgValue.Add(new AutoJTTXUtilities.Controls.AJTMsgHelperBase("更新提示", sb12.ToString()));
                                    AutoJTTXUtilities.Controls.AJTPromptDialog aJTPromptDialog = new AutoJTTXUtilities.Controls.AJTPromptDialog("", "检查更新", msgValue);
                                    aJTPromptDialog.ShowInTaskbar = true;
                                    aJTPromptDialog.ShowDialog();
                                    //System.Windows.MessageBox.Show(this,sb12.ToString(), "AutoJT", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);

                                    //退出登录
                                    ServiceClient.CheckIdentity = false;
                                    //退出通信
                                    ServiceClient.ExitCommunication(Global.m_serviceClient);
                                }
                            )
                        );
                    }

                    //开始自动静默更新
                    bool bl098 = this.AutomaticReleaseAndRunHandler(GlobalClass.CurrentVersion, lastReleaseVer, "", downloadLink2, this.m_ParentAssembly);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    AJTDatabaseOperation.ModelInfosSum(new PluginInfoStatistics("版本检查错误", ex.Message, $"{GlobalClass.RegId}|{GlobalClass.NickName}", GlobalClass.SoftWareHostVersion, string.Format("{0} {1} {2}", GlobalClass.VersionDesc, GlobalClass.CurrentVersion, GlobalClass.ExpireDate)).CreateDatatable());
                }
                catch
                {
                }
            }
        }
        //查询最新版本
        private Task<(string lastReleaseVer, string VERSIONCONTENTS, string FORCEDUPDATE, string downloadLink, string downloadLink2)> CheckUpdate()
        {
            string lastReleaseVer = string.Empty;//最新版本
            string VERSIONCONTENTS = string.Empty;//更新内容
            string FORCEDUPDATE = string.Empty;//是否强制更新
            string downloadLink = string.Empty;
            string downloadLink2 = string.Empty;

            //查询new版本号
            try
            {
                lastReleaseVer = AJTDatabaseOperation.CheckLastReleaseVer(out VERSIONCONTENTS, out FORCEDUPDATE, out downloadLink, out downloadLink2);
            }
            catch
            {
                return Task.FromResult((lastReleaseVer,
                        VERSIONCONTENTS,
                        FORCEDUPDATE,
                        downloadLink,
                        downloadLink2));
            }

            return Task.FromResult((lastReleaseVer,
                        VERSIONCONTENTS,
                        FORCEDUPDATE,
                        downloadLink,
                        downloadLink2));
        }
        //新旧版本对比
        private bool CheckVersion(string newVersion, string curVersion, out bool? differences)
        {
            differences = null;

            if (string.IsNullOrEmpty(newVersion))
            {
                return false;
            }
            if (string.IsNullOrEmpty(curVersion))
            {
                throw new ArgumentNullException(nameof(curVersion));
            }

            try
            {
                //计算两个版本差距得月数
                var diff = CheckMonths(curVersion, newVersion);
                //大于半年
                if (Math.Abs(diff) > 6)
                {
                    differences = true;
                }
            }
            catch
            {
                differences = null;
            }

            if (newVersion != curVersion)
            {
                return true;
            }

            return false;
        }

        //自动静默更新
        bool AutomaticReleaseAndRunHandler(string currentVersion, string newversion, string updateContent, string downloadLink, Assembly asset)
        {
            bool result12 = false;

            Dispatcher.Invoke(
               new Action(
                   delegate
                   {
                       //检查辅助程序是否正在运行
                       bool isrunning = false;

                       //配置文件夹
                       string _ini_outputPath = string.Empty;
                       try
                       {
                           //配置文件夹
                           _ini_outputPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AutoJTTecnomatix");
                           if (!System.IO.Directory.Exists(_ini_outputPath))
                           {
                               System.IO.Directory.CreateDirectory(_ini_outputPath);
                           }
                       }
                       catch
                       {
                       }

                       try
                       {
                           List<Process> processes = Process.GetProcesses().Where(p => p.ProcessName == "AutoJTTXUpdateHandler").ToList();
                           if (processes.Count > 0)
                           {
                               isrunning = true;
                               result12 = true;
                           }
                       }
                       catch
                       {
                       }

                       if (!isrunning)
                       {
                           #region 主程序DLL释放前检查配置文件中的版本和文件路径是否存在

                           //是否需要释放辅助程序
                           bool isRelesae = false;
                           try
                           {

                               //首先检查文件是否存在
                               string healEXEFullname = System.IO.Path.Combine(_ini_outputPath, "AutoJTTXUpdateHandler.exe");
                               if (System.IO.File.Exists(healEXEFullname))
                               {
                                   //文件存在

                                   //读取配置文件
                                   AutoJTTXUtilities.ConfigurationHandling.ConfigurationFileOperationAny configurationFileOperationAny = new AutoJTTXUtilities.ConfigurationHandling.ConfigurationFileOperationAny(healEXEFullname);
                                   string strVer = configurationFileOperationAny.ReadConfiguration("CurrentDllVer");
                                   if (!string.IsNullOrEmpty(strVer))
                                   {
                                       //与最新版本比较
                                       if (strVer.Equals(newversion))
                                       {
                                           string strexeFile = configurationFileOperationAny.ReadConfiguration("FileFullname");
                                           if (!string.IsNullOrEmpty(strexeFile))
                                           {
                                               //检查文件是否存在
                                               if (System.IO.File.Exists(strexeFile))
                                               {
                                                   //不需要释放
                                                   isRelesae = false;
                                               }
                                               else
                                               {
                                                   //需要释放
                                                   isRelesae = true;
                                               }
                                           }
                                           else
                                           {
                                               //file不存在, 需要释放
                                               isRelesae = true;
                                           }
                                       }
                                       else
                                       {
                                           //ver不匹配, 需要释放
                                           isRelesae = true;
                                       }
                                   }
                                   else
                                   {
                                       //ver不存在, 需要释放
                                       isRelesae = true;
                                   }
                               }
                               else
                               {
                                   //exe不存在, 需要释放
                                   isRelesae = true;
                               }
                           }
                           catch
                           {
                               //异常, 需要释放
                               isRelesae = true;
                           }

                           //不需要释放
                           if (!isRelesae)
                           {
                               #region 模块统计
                               try
                               {
                                   AJTDatabaseOperation.ModelInfosSum(new PluginInfoStatistics("静默升级程序", "已经下载最新版本的安装包, 等待下次重启", $"{GlobalClass.RegId}|{GlobalClass.NickName}", GlobalClass.SoftWareHostVersion, string.Format("{0} {1} {2}", GlobalClass.VersionDesc, GlobalClass.CurrentVersion, GlobalClass.ExpireDate)).CreateDatatable());
                               }
                               catch
                               {

                               }
                               #endregion

                               return;
                           }

                           #endregion

                           try
                           {
                               //数据格式
                               //new string[] { "user1", "https://", "C:\\Program Files\\Tecnomatix_14.1\\eMPower" };

                               #region 释放辅助程序

                               try
                               {
                                   if (!System.IO.Directory.Exists(_ini_outputPath))
                                   {
                                       System.IO.Directory.CreateDirectory(_ini_outputPath);
                                   }
                               }
                               catch
                               {
                                   _ini_outputPath = System.IO.Path.GetTempPath();
                               }

                               //删除config文件
                               try
                               {
                                   string configFileStr = System.IO.Path.Combine(_ini_outputPath, "AutoJTTXUpdateHandler.exe.config");
                                   if (System.IO.File.Exists(configFileStr))
                                   {
                                       System.IO.File.Delete(configFileStr);
                                   }
                               }
                               catch
                               {
                               }

                               //释放exe
                               bool bl09 = AutoJTTXUtilities.DocumentationHandling.InitAutoJTTXUpdateHandlerEXE.InitEXE(out string error, _ini_outputPath, "AutoJTTecnomatix.EmbeddedResources.Update.AutoJTTXUpdateHandler.exe", asset, "AutoJTTXUpdateHandler.exe");
                               if (bl09)
                               {
                                   //infos
                                   StringBuilder sb = new StringBuilder();
                                   sb.AppendLine("AutoJTTXUpdateHandler.exe提取成功");

                                   try
                                   {
                                       //运行参数
                                       StringBuilder sbArg = new StringBuilder();
                                       sbArg.Append((char)34);//双引号
                                       sbArg.Append(newversion);
                                       sbArg.Append((char)34);

                                       sbArg.Append((char)32);//空格

                                       sbArg.Append((char)34);//双引号
                                       sbArg.Append(GlobalClass.user.strUsrName);
                                       sbArg.Append((char)34);

                                       sbArg.Append((char)32);//空格

                                       sbArg.Append((char)34);//双引号
                                       sbArg.Append(downloadLink);
                                       sbArg.Append((char)34);

                                       sbArg.Append((char)32);//空格

                                       sbArg.Append((char)34);//双引号
                                       sbArg.Append(AutoJTTXCoreUtilities.AJTTxApplicationUtilities.GetInstallationDirectory());
                                       sbArg.Append((char)34);

                                       string exeFileName = System.IO.Path.Combine(_ini_outputPath, "AutoJTTXUpdateHandler.exe");

                                       ProcessStartInfo processStartInfo = new ProcessStartInfo(exeFileName);
                                       processStartInfo.Arguments = sbArg.ToString();

                                       processStartInfo.CreateNoWindow = true;
                                       processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                       processStartInfo.WorkingDirectory = _ini_outputPath;

                                       processStartInfo.UseShellExecute = false;

                                       try
                                       {
                                           //等待更新程序执行完毕
                                           Process.Start(processStartInfo);
                                           sb.AppendLine("更新辅助程序启动成功");
                                           //运行成工
                                           result12 = true;
                                       }
                                       catch (Exception ex)
                                       {
                                           //运行失败
                                           result12 = false;

                                           sb.AppendLine(string.Format("更新辅助程序启动成功，但出现错误。{0}", processStartInfo.FileName));
                                           Win32Exception ex2 = ex as Win32Exception;
                                           if (ex2 != null)
                                           {
                                               int nativeErrorCode = ex2.NativeErrorCode;
                                               if (nativeErrorCode != 2)
                                               {
                                                   if (nativeErrorCode == 193)
                                                   {
                                                       sb.AppendLine(string.Format("文件{0}不是有效的应用程序。", processStartInfo.FileName));
                                                   }
                                               }
                                               else
                                               {
                                                   sb.AppendLine(string.Format("找不到可执行{0}。", processStartInfo.FileName));
                                                   sb.AppendLine(string.Format("检查的计算机上是否安装了DotNetFramework。{0}", processStartInfo.FileName));
                                               }
                                           }
                                       }

                                   }
                                   catch (Exception ex)
                                   {
                                       result12 = false;
                                       sb.AppendLine(string.Format("未知异常 {0}", ex.Message));
                                   }

                                   #region 模块统计
                                   try
                                   {
                                       AJTDatabaseOperation.ModelInfosSum(new PluginInfoStatistics("静默升级程序", string.Format("{0}", sb.ToString()), $"{GlobalClass.RegId}|{GlobalClass.NickName}", GlobalClass.SoftWareHostVersion, string.Format("{0} {1} {2}", GlobalClass.VersionDesc, GlobalClass.CurrentVersion, GlobalClass.ExpireDate)).CreateDatatable());
                                   }
                                   catch
                                   {

                                   }
                                   #endregion
                               }
                               else
                               {
                                   //释放exe失败
                                   #region 模块统计
                                   try
                                   {
                                       AJTDatabaseOperation.ModelInfosSum(new PluginInfoStatistics("静默升级程序", string.Format("未知异常, 释放自动更新辅助程序异常 {0}", error), $"{GlobalClass.RegId}|{GlobalClass.NickName}", GlobalClass.SoftWareHostVersion, string.Format("{0} {1} {2}", GlobalClass.VersionDesc, GlobalClass.CurrentVersion, GlobalClass.ExpireDate)).CreateDatatable());
                                   }
                                   catch
                                   {

                                   }
                                   #endregion

                                   result12 = false;
                               }
                               #endregion
                           }
                           catch (Exception ex)
                           {
                               //未知异常更新失败
                               #region 模块统计
                               try
                               {
                                   AJTDatabaseOperation.ModelInfosSum(new PluginInfoStatistics("静默升级程序", string.Format("未知异常, 释放自动更新辅助程序异常 {0}", ex.Message), $"{GlobalClass.RegId}|{GlobalClass.NickName}", GlobalClass.SoftWareHostVersion, string.Format("{0} {1} {2}", GlobalClass.VersionDesc, GlobalClass.CurrentVersion, GlobalClass.ExpireDate)).CreateDatatable());
                               }
                               catch
                               {

                               }
                               #endregion

                               result12 = false;
                           }
                       }
                   }
                 )
              );

            return result12;
        }

        //比较月份
        static int CheckMonths(string d1, string d2)
        {
            try
            {
                string date1String = d1;
                string date2String = d2;

                // 解析日期字符串
                DateTime date1 = ParseDateString(date1String);
                DateTime date2 = ParseDateString(date2String);

                // 计算月份差
                return MonthsApart(date1, date2);
            }
            catch
            {
                return 0;
            }
        }

        // 解析日期字符串
        static DateTime ParseDateString(string dateString)
        {
            // 将日期字符串拆分为年、月、日
            string[] parts = dateString.Split('.');

            // 转换为整数
            int year = int.Parse(parts[0]);
            int month = int.Parse(parts[1]);
            int day = int.Parse(parts[2]);

            // 以 2000 年为基准，构造日期对象
            return new DateTime(year + 2000, month, day);
        }


        // 计算月份差
        static int MonthsApart(DateTime date1, DateTime date2)
        {
            int monthsApart = (date2.Year - date1.Year) * 12 + date2.Month - date1.Month;
            return monthsApart;
        }

        #endregion

    }
}