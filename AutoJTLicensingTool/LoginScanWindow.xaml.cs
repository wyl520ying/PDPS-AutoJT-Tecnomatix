using AutoJTMessageUtilities;
using AutoJTTXCoreUtilities;
using AutoJTTXServiceUtilities;
using AutoJTTXUtilities.WechatHandling;
using mshtml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using Tecnomatix.Engineering.Ui.WPF;

namespace AutoJTLicensingTool
{
    /// <summary>
    /// LoginScanWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginScanWindow : TxWindow
    {
        #region Constructors

        private ArrayList addressList = new ArrayList();

        //当前版本号
        string m__currentVersion;
        //用户信息
        string m__userinfos;

        //双工
        public static ServiceClient m_serviceClient;

        Assembly m_ParentAssembly;
        public LoginScanWindow(string _currVer, Assembly parentAssembly)
        {
            this.m_ParentAssembly = parentAssembly;

            #region 检查网络连接

            bool isping = AJTDatabaseOperation.CheckNetworkConnection();
            if (!isping)
            {
                Dispatcher.Invoke(
                      new Action(
                          delegate
                          {
                              MessageBox.Show(this, "网络服务器连接故障！" +
                                             "\n" +
                                             "\n" +
                                             "如果网络能正常访问，请联系管理员将以下站点加入受信任的站点：" +
                                             "\n" +
                                             "http://www.autojt.com" +
                                             //"\n" +
                                             //"http://www.autojt.com:81" +
                                             //"\n" +
                                             //"http://www.autojt.com:8002" +// /AutoJTService
                                             " ", "网络故障提醒", MessageBoxButton.OK, MessageBoxImage.Error);
                              //退出程序
                              this.Close();
                          }
                      )
                  );

                return;

                //throw new Exception("网络故障提醒! 网络服务器连接故障！如果网络能正常访问，请联系管理员将以下站点加入受信任的站点：http://www.autojt.com");
            }

            #endregion

            #region 检查版本

            if (string.IsNullOrEmpty(_currVer))
            {
                #region 模块统计
                try
                {
                    AJTDatabaseOperation.ModelInfosSum(new PluginInfoStatistics("非法访问", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, GlobalClass.user.strUsrName, GlobalClass.SoftWareHostVersion, string.Format("{0} {1} {2}", GlobalClass.VersionDesc, GlobalClass.CurrentVersion, GlobalClass.ExpireDate)).CreateDatatable(), out string exinfos);
                }
                catch
                {

                }
                #endregion

                Dispatcher.BeginInvoke(new Action(delegate
                {
                    MessageBox.Show(this, "未知异常", "AutoJT", MessageBoxButton.OK, MessageBoxImage.Error);
                    //退出程序
                    this.Close();
                }));

                return;
            }
            //获取当前dll的版本
            this.m__currentVersion = _currVer;

            #endregion

            InitializeComponent();

            this.ResizeMode = ResizeMode.CanResize;
            this.SemiModal = false;
            this.ShowInTaskbar = true;


            this.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            //空白图标
            //this.Icon = new DrawingImage();

            this.KeyDown += LoginScanWindow_KeyDown;

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            this.Loaded += LoginScanWindow_Loaded;
            this.Closing += LoginScanWindow_Closing;

            //检查版本号   
            System.Threading.Tasks.Task.Factory.StartNew(CheckCurrentVersion);

        }

        #endregion

        #region Events

        private void LoginScanWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void LoginScanWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                GetEnvironment getEnvironment = new GetEnvironment();
                GetEnvironment.GetEnv(out string osVersionName, out string machineName, out string userName, out string domainName,
                                out string processorCounter, out string SystemType, out string iWorkSet);

                this.m__userinfos = string.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}",
                    osVersionName, machineName, userName, domainName, processorCounter, SystemType, iWorkSet);

                try
                {
                    //获取机器码
                    getEnvironment.CheckJsonCode();
                    GlobalClass.SoftWareHostVersion = TecnomatixInfos.GetUsersDirectorySoftVersion();
                }
                catch
                {
                }
            }
            catch (Exception ex)
            {
                this.m__userinfos = ex.Message;
            }

            WebNavigateMethod();

            //Navigating="web1_Navigating" LoadCompleted="web1_LoadCompleted"
            this.web1.Navigating += web1_Navigating;
            this.web1.LoadCompleted += web1_LoadCompleted;

            this.web1.Navigated += Web1_Navigated;
        }

        private void LoginScanWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //Esc键退出  
            if (e.Key == Key.Escape)
            {
                Dispatcher.Invoke(
                      new Action(
                          delegate
                          {
                              this.Close();
                          }
                      )
                  );
            }
        }

        #endregion

        #region 检查版本号

        //获取当前dll的版本
        void CheckCurrentVersion()
        {
            try
            {
                //获取服务器版本号
                string lastReleaseVer = CheckUpdate(out string VERSIONCONTENTS, out string FORCEDUPDATE, out string downloadLink1, out string downloadLink2);

                if (CheckVersion(lastReleaseVer, this.m__currentVersion, out int differences))
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
                                    ServiceClient.ExitCommunication(m_serviceClient);
                                }
                            )
                        );
                    }

                    //是否大版本                 
                    Dispatcher.Invoke(
                          new Action(
                              delegate
                              {
                                  if (differences < 0)
                                  {
                                      this.Close();
                                  }
                              }
                          )
                      );


                    //是否大版本    
                    if (differences < 0)
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
                                    sb12.Append(string.Format("当前版本：{0}   ", this.m__currentVersion));
                                    sb12.Append(string.Format("最新版本：{0}", lastReleaseVer));
                                    sb12.AppendLine();
                                    sb12.AppendLine("更新内容：");
                                    sb12.AppendLine(verContent1);
                                    sb12.AppendLine(string.Format("新版本下载链接：{0}", downloadLink1));

                                    msgValue.Add(new AutoJTTXUtilities.Controls.AJTMsgHelperBase("更新提示", sb12.ToString()));
                                    AutoJTTXUtilities.Controls.AJTPromptDialog aJTPromptDialog = new AutoJTTXUtilities.Controls.AJTPromptDialog("", "检查更新", msgValue);
                                    aJTPromptDialog.ShowInTaskbar = true;
                                    aJTPromptDialog.ShowDialog();
                                    //System.Windows.MessageBox.Show(sb12.ToString(), "AutoJT", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);

                                    //退出登录
                                    ServiceClient.CheckIdentity = false;
                                    //退出通信
                                    ServiceClient.ExitCommunication(m_serviceClient);
                                }
                            )
                        );
                    }

                    //开始自动更新
                    bool bl098 = this.AutomaticReleaseAndRunHandler(this.m__currentVersion, lastReleaseVer, "", downloadLink2, this.m_ParentAssembly);
                }
            }
            catch
            {
            }
        }

        //查询最新版本
        string CheckUpdate(out string VERSIONCONTENTS, out string FORCEDUPDATE, out string downloadLink, out string downloadLink2)
        {
            string lastReleaseVer = string.Empty;//最新版本
            VERSIONCONTENTS = string.Empty;//更新内容
            FORCEDUPDATE = string.Empty;//是否强制更新
            downloadLink = string.Empty;
            downloadLink2 = string.Empty;

            //查询new版本号
            try
            {
                lastReleaseVer = AJTDatabaseOperation.CheckLastReleaseVer(out VERSIONCONTENTS, out FORCEDUPDATE, out downloadLink, out downloadLink2);
            }
            catch
            {
                return lastReleaseVer;
            }

            return lastReleaseVer;
        }

        //版本对比
        bool CheckVersion(string newVersion, string curVersion, out int differences)
        {
            differences = 0;

            if (string.IsNullOrEmpty(newVersion))
            {
                return false;
            }

            var arr1 = newVersion.Split('.');
            var arr2 = curVersion.Split('.');

            try
            {
                int level_new_y = Convert.ToInt32(arr1[arr1.Length - 4]);
                int level_cur_y = Convert.ToInt32(arr2[arr2.Length - 4]);

                int level_new_m = Convert.ToInt32(arr1[arr1.Length - 3]);
                int level_cur_m = Convert.ToInt32(arr2[arr2.Length - 3]);

                int differences_tmp_m = 0;
                int differences_tmp_y = 0;
                differences_tmp_m = level_cur_m - level_new_m;
                differences_tmp_y = level_cur_y - level_new_y;

                if (differences_tmp_y > 0)
                {
                    differences = 1;
                }
                else if (differences_tmp_y < 0)
                {
                    differences = -1;
                }
                else if (differences_tmp_m < 0 && differences_tmp_y == 0)
                {
                    differences = -1;
                }
                else if (differences_tmp_m > 0 && differences_tmp_y == 0)
                {
                    differences = 1;
                }

            }
            catch
            {
                differences = 0;
            }

            try
            {
                for (int i = 0; i < arr1.Length; i++)
                {
                    if (Convert.ToInt32(arr1[i]) > Convert.ToInt32(arr2[i]))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        //自动更新
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
                                   AJTDatabaseOperation.ModelInfosSum(new PluginInfoStatistics("静默升级程序", "已经下载最新版本的安装包, 等待下次重启", GlobalClass.user.strUsrName, GlobalClass.SoftWareHostVersion, string.Format("{0} {1} {2}", GlobalClass.VersionDesc, GlobalClass.CurrentVersion, GlobalClass.ExpireDate)).CreateDatatable(), out string exinfos);
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
                                       AJTDatabaseOperation.ModelInfosSum(new PluginInfoStatistics("静默升级程序", string.Format("{0}", sb.ToString()), GlobalClass.user.strUsrName, GlobalClass.SoftWareHostVersion, string.Format("{0} {1} {2}", GlobalClass.VersionDesc, GlobalClass.CurrentVersion, GlobalClass.ExpireDate)).CreateDatatable(), out string exinfos);
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
                                       AJTDatabaseOperation.ModelInfosSum(new PluginInfoStatistics("静默升级程序", string.Format("未知异常, 释放自动更新辅助程序异常 {0}", error), GlobalClass.user.strUsrName, GlobalClass.SoftWareHostVersion, string.Format("{0} {1} {2}", GlobalClass.VersionDesc, GlobalClass.CurrentVersion, GlobalClass.ExpireDate)).CreateDatatable(), out string exinfos);
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
                                   AJTDatabaseOperation.ModelInfosSum(new PluginInfoStatistics("静默升级程序", string.Format("未知异常, 释放自动更新辅助程序异常 {0}", ex.Message), GlobalClass.user.strUsrName, GlobalClass.SoftWareHostVersion, string.Format("{0} {1} {2}", GlobalClass.VersionDesc, GlobalClass.CurrentVersion, GlobalClass.ExpireDate)).CreateDatatable(), out string exinfos);
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

        #endregion

        #region 扫码登录

        public void WebNavigateMethod()
        {
            bool bl2 = false;

            string u_niceName = string.Empty;
            string u_openid = string.Empty;

            //检测自动登录
            try
            {
                //检查当前客户端是否可以自动登录
                bl2 = new ServiceClient(GlobalClass.user.strUsrName).CheckCurrentMachine(out u_niceName, out u_openid);
            }
            catch
            {
            }

            //自动登录成功
            if (bl2 && !string.IsNullOrEmpty(u_niceName) && !string.IsNullOrEmpty(u_openid))
            {
                ScaceCodeSuccessMethod(true, u_niceName, u_openid);
                return;
            }

            this.web1.Navigate("https://open.weixin.qq.com/connect/qrconnect?appid=wx0e61e107650f560f&redirect_uri=http://www.autojt.com/index.php&response_type=code&scope=snsapi_login&state=2014#wechat_redirect");
        }

        //扫码成功
        void ScaceCodeSuccessMethod(bool isscan_1, string _niceName, string openID)
        {
            try
            {
                if (!await AJTDatabaseOperation.TestNetworkConnectionAsync())
                {
                    throw new Exception("网络链接失败");                   
                }

                //扫码成功, 直接登录
                if (isscan_1)
                {
                    //登录成功
                    this.DialogResult = true;

                    GlobalClass.user.strUsrName = _niceName;
                    GlobalClass.user.strUsrId = openID;
                    GlobalClass.user.authority = GlobalClass.Authority.administrators;
                    //GlobalClass.SoftWareHostVersion = TecnomatixInfos.GetUsersDirectorySoftVersion();

                    //服务器记录信息
                    AJTDatabaseOperation.CloudDBOperation_wechat("Tecnomatix", this.m__userinfos, this.m__currentVersion, string.Format("{0}\t{1}", GlobalClass.user.strUsrName, GlobalClass.user.strUsrId), GlobalClass.SoftWareHostVersion);

                    #region 异步检查客户端

                    //异步检查客户端
                    try
                    {
                        m_serviceClient = new ServiceClient(openID, GlobalClass.user.strUsrName);

#if INTERNAL
                        todo,有时候网络故障会显示没有许可.,网络时连时断的情况
                        GlobalClass.ParseModels(new string[] { },
                            "2023-04-25", "内部版", "内部用户测试", true,this.m__currentVersion);
#endif

                    }
                    catch
                    {
                        //退出登录
                        ServiceClient.CheckIdentity = false;
                        ServiceClient.ExitCommunication(m_serviceClient);
                    }

                    #endregion

                    Dispatcher.Invoke(
                          new Action(
                              delegate
                              {
                                  this.Close();
                              }
                          )
                      );
                }
            }
            catch (Exception ex)
            {
                //退出登录
                ServiceClient.CheckIdentity = false;
                ServiceClient.ExitCommunication(m_serviceClient);
                System.Windows.MessageBox.Show(ex.Message, "AutoJT", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Asterisk);
            }
        }

        private void web1_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            string url = e.Uri.ToString();
            this.addressList.Add(url);
        }

        private void web1_LoadCompleted(object sender, NavigationEventArgs e)
        {
            mshtml.HTMLDocument dom = (mshtml.HTMLDocument)web1.Document; //定义HTML
            dom.documentElement.style.overflow = "hidden";    //隐藏浏览器的滚动条
            dom.body.setAttribute("scroll", "no");            //禁用浏览器的滚动条


            if (this.addressList.Count > 0)
            {
                #region 检查链接

                string code = string.Empty;
                try
                {
                    string tempCode = this.addressList[0].ToString();

                    if (!tempCode.Contains("code"))
                    {
                        return;
                    }
                    int iStart = tempCode.IndexOf("=");
                    int iEnd = tempCode.IndexOf('&', iStart);
                    if (iEnd < 0)
                    {
                        iEnd = tempCode.Length - iStart;
                    }
                    else
                    {
                        iEnd -= iStart;
                    }
                    code = tempCode.Substring(iStart + 1, iEnd - 1);
                    if (string.IsNullOrEmpty(code))
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message, "AutoJT", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Asterisk);

                    return;
                }

                #endregion

                #region 解析账户

                OAuthUser OAuthUser_Model = null;
                try
                {
                    OAuth_Token token = new OAuth_Token();
                    OAuth_Token Model = token.Get_token(code);
                    OAuthUser_Model = token.Get_UserInfo(Model.access_token, Model.openid);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message, "AutoJT", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Asterisk);

                    return;
                }


                #endregion

                #region 注册登录

                try
                {
                    if (!string.IsNullOrEmpty(OAuthUser_Model.openid))
                    {
                        //昵称
                        string email = this.GetMatchStr(OAuthUser_Model.nickname.ToString().Trim());
                        //openID
                        string u_OpenID = OAuthUser_Model.openid.ToString().Trim();

                        //微信账户不存在
                        if (!AJTDatabaseOperation.QueryUsernameFidle_1(u_OpenID))
                        {
                            //创建微信账户
                            string result = AJTDatabaseOperation.CreateAccount_1(u_OpenID, "", email);
                            if (result.IndexOf("成功") != -1)
                            {
                                this.ScaceCodeSuccessMethod(true, email, u_OpenID);
                            }
                        }
                        else
                        {
                            //更新资料
                            string result = AJTDatabaseOperation.UpdateWchatNiceName_1(u_OpenID.Trim(), email, out bool m_dataSet, out string remarkName, out bool isjt);

                            if (result.IndexOf("成功") != -1)
                            {
                                //try
                                //{
                                //    //新用户权限
                                //    string _authority = m_dataSet.Tables[0].Rows[0]["authority"].ToString();
                                //    if (_authority == "users")
                                //    {
                                //        GlobalClass.user.authority = GlobalClass.Authority.users;
                                //    }
                                //    else
                                //    {
                                //        GlobalClass.user.authority = GlobalClass.Authority.administrators;
                                //    }
                                //}
                                //catch
                                //{

                                //}

                                //检测账户许可
                                try
                                {
#if INTERNAL

                                    if (!m_dataSet || !isjt)
                                    {
                                        //非法访问
                                        if (!isjt)
                                        {
                                            #region 模块统计
                                            try
                                            {
                                                StringBuilder sb = new StringBuilder();
                                                sb.Append("<");
                                                sb.Append(u_OpenID);
                                                sb.Append(" ");
                                                sb.Append(email);
                                                sb.Append(" ");
                                                sb.Append(remarkName);
                                                sb.Append("> ");
                                                sb.Append("非九田用户, 已经禁止登录");
                                                AJTDatabaseOperation.ModelInfosSum(new PluginInfoStatistics("非法访问", sb.ToString(), GlobalClass.user.strUsrName, GlobalClass.SoftWareHostVersion, string.Format("{0} {1} {2}", GlobalClass.VersionDesc, GlobalClass.CurrentVersion, GlobalClass.ExpireDate)).CreateDatatable(), out string exinfos);
                                            }
                                            catch
                                            {

                                            }
                                            #endregion
                                        }

                                        //许可过期
                                        System.Windows.MessageBox.Show("微信登录失败", "AutoJT", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Asterisk);
                                        return;
                                    }

#elif EXTERNAL

                                    if (!m_dataSet)
                                    {
                                        //许可过期
                                        System.Windows.MessageBox.Show("微信登录失败", "AutoJT", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Asterisk);
                                        return;
                                    }
#endif

                                }
                                catch
                                {

                                }
#if INTERNAL
                                if (string.IsNullOrEmpty(remarkName))
                                {
                                    this.ScaceCodeSuccessMethod(true, email, u_OpenID);
                                }
                                else
                                {
                                    this.ScaceCodeSuccessMethod(true, remarkName, u_OpenID);
                                }
#elif EXTERNAL
                                    this.ScaceCodeSuccessMethod(true, email, u_OpenID);
#endif

                            }
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("微信登录失败", "AutoJT", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Asterisk);
                    }
                }
                catch (Exception ex)
                {

                    System.Windows.MessageBox.Show(ex.Message, "AutoJT", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Asterisk);

                    return;
                }

                #endregion

            }
        }


        private void Web1_Navigated(object sender, NavigationEventArgs e)
        {
            HTMLDocument doc = this.web1.Document as HTMLDocument;

            IHTMLElementCollection bodys = doc.getElementsByTagName("body");
            IHTMLElement body = null;

            if (bodys.length > 0)
                body = (IHTMLElement)bodys.item(0);
            if (body != null)
            {
                var str = body.all;

                //foreach (var child in str)
                //{
                //    HTMLDivElement hTMLDivElementClass = child as HTMLDivElement;

                //    if (hTMLDivElementClass != null)
                //    {
                //        string className = hTMLDivElementClass.className;

                //        if (className == "web_qrcode_app_wrp")
                //        {
                //            hTMLDivElementClass.style.display = "inline-block";
                //            break;
                //        }
                //    }
                //}
            }
        }

        /// <summary>
        /// 昵称规范化(保留汉字 字母 数字)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        string GetMatchStr(string source)
        {
            string pattern = "[A-Za-z0-9\u4e00-\u9fa5-]+";
            string MatchStr = "";
            MatchCollection results = Regex.Matches(source, pattern);
            foreach (var s in results)
            {
                MatchStr += s.ToString();
            }

            //全是表情符号
            if (string.IsNullOrEmpty(MatchStr))
            {
                MatchStr = TecnomatixInfos.GETNEWGuid();
            }

            return MatchStr;
        }

        #endregion
    }
}
