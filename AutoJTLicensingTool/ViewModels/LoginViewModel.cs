using AutoJTL.SDK.Strandard;
using AutoJTL.SDK.Strandard.Request;
using AutoJTLicensingTool.Common;
using AutoJTLicensingTool.Messages;
using AutoJTTXCoreUtilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Contrib.Hub;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace AutoJTLicensingTool.ViewModels
{
    public class LoginViewModel :
        ObservableObject,
        IRecipient<OpenOplatformNotifyAuth2Success>,
        IRecipient<OfficialSubscribeSuccess>, IContext
    {
        #region 用于将UI Dispatcher传递给ViewModel

        private readonly Dispatcher _dispatcher;
        public bool IsSynchronized
        {
            get
            {
                return this._dispatcher.Thread == Thread.CurrentThread;
            }
        }
        public void Invoke(Action action)
        {
            Debug.Assert(action != null);

            this._dispatcher.Invoke(action);
        }

        public void BeginInvoke(Action action)
        {
            Debug.Assert(action != null);

            this._dispatcher.BeginInvoke(action);
        }
        public LoginViewModel(Dispatcher dispatcher)
        {
            Debug.Assert(dispatcher != null);

            this._dispatcher = dispatcher;
        }

        #endregion


        /// <summary>
        /// 二维码路径
        /// </summary>
        public BitmapImage qrCodeImage;
        public BitmapImage QRCodeImage
        {
            get { return qrCodeImage; }
            set
            {
                SetProperty(ref qrCodeImage, value);
                QRCodeImageVisibility = QRCodeImage == null ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        /// <summary>
        /// 二维码显示进度
        /// </summary>
        public Visibility qrCodeImageVisibility = Visibility.Visible;
        public Visibility QRCodeImageVisibility
        {
            get { return qrCodeImageVisibility; }
            set { SetProperty(ref qrCodeImageVisibility, value); }
        }

        /// <summary>
        /// 显示进度
        /// </summary>
        public Visibility visibilityProgress = Visibility.Visible;
        public Visibility VisibilityProgress
        {
            get { return visibilityProgress; }
            set { SetProperty(ref visibilityProgress, value); }
        }

        public string progress = "初始化中 . . .";
        public string Progress
        {
            get { return progress; }
            set { SetProperty(ref progress, value); }
        }

        public string nickName = "";
        public string NickName
        {
            get { return nickName; }
            set { SetProperty(ref nickName, value); }
        }

        /// <summary>
        /// 刷新按钮是否可用
        /// </summary>
        private bool refreshBtnIsEnabled;
        public bool RefreshBtnIsEnabled
        {
            get => refreshBtnIsEnabled;
            set { SetProperty(ref refreshBtnIsEnabled, value); }
        }

        //用于记录是否需要重新初始化hub(默认需要)
        private bool IsInitialHub = true;

        public LoginViewModel() : this(Dispatcher.CurrentDispatcher)
        {
            this.RefreshSubscribeQRCodeCommand = new AsyncRelayCommand(RefreshSubscribeQRCodeAsync);
            WeakReferenceMessenger.Default.Register("Login_Init", (MessageHandler<object, string>)(async
                (sender, message) =>
            {
                //首次初始化已经成功
                this.m_firstInit = true;
                //初始化方法
                await InitialMethod();
            }));
            WeakReferenceMessenger.Default.Register<OpenOplatformNotifyAuth2Success>(this);
            WeakReferenceMessenger.Default.Register<OfficialSubscribeSuccess>(this);
        }
        public LoginViewModel(bool isCanAutoLogin) : this(Dispatcher.CurrentDispatcher)
        {
            //用于记录是否需要重新初始化hub(默认需要)
            this.IsInitialHub = isCanAutoLogin;

            this.RefreshSubscribeQRCodeCommand = new AsyncRelayCommand(RefreshSubscribeQRCodeAsync);
            WeakReferenceMessenger.Default.Register("AJTLicenseDialog_Init", (MessageHandler<object, string>)(async
                (sender, message) =>
            {
                //首次初始化已经成功
                this.m_firstInit = true;
                //初始化方法
                await InitialMethod(isCanAutoLogin);
            }));
            WeakReferenceMessenger.Default.Register<OpenOplatformNotifyAuth2Success>(this);
            WeakReferenceMessenger.Default.Register<OfficialSubscribeSuccess>(this);
        }

        //是否已经初始化过一次, 用于防止初始加载的重复初始化
        public bool m_firstInit;
        //错误重连的次数
        private int reConnectCount = 0;
        //初始化方法
        public Task InitialMethod(bool isCanAutoLogin = true)
        {
            return Task.Run(async () =>
            {
                try
                {
                    //MessageBox.Show("66");

                    //重置hub
                    await ResetHubMethod();

                    Progress = "初始化中 . ";
                    await InitAsync();
                    Progress = "初始化中 . . ";


                    if (Global.Hub?.State != Microsoft.AspNetCore.SignalR.Client.HubConnectionState.Connected)
                    {
                        throw new Exception("等待重连...");
                    }
                    else
                    {
                        progress = Global.Hub.State.ToString();
                    }

                    if (isCanAutoLogin)
                    {
                        //检查当前客户端是否可以自动登录
                        await CheckCanAutoLoginAsync();
                    }
                    else
                    {
                        //直接刷新二维码
                        await ShowSubscribeQRCodeAsync();
                        Progress = "请扫二维码并关注公众号";
                    }
                }
                catch (Exception ex)
                {
                    Progress = "未知错误," + ex.Message;

                    #region 如果错误重连,只能链接8次

                    if (reConnectCount > 8)
                    {
                        MessageBox.Show(progress);
                        return;
                    }
                    reConnectCount++;

                    #endregion

                    //重新初始化
                    await this.InitialMethod();
                    Progress = "重新初始化成功";
                }
            });
        }

        #region 刷新二维码的command

        public AsyncRelayCommand RefreshSubscribeQRCodeCommand { get; set; }

        public async Task RefreshSubscribeQRCodeAsync()
        {
            //重置hub
            await ResetHubMethod();

            try
            {
                await InitAsync();
                if (await ShowSubscribeQRCodeAsync())
                {
                    //MessageBox.Show("刷新关注二维码成功");
                    Progress = "刷新关注二维码成功";
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"未知异常{ex.Message}");
            }
        }


        //重置hub
        private Task ResetHubMethod()
        {
            //是否需要重新初始化hub(默认需要)
            if (!this.IsInitialHub)
            {
                return Task.CompletedTask;
            }

            return Task.Run(async () =>
             {
                 await Global.DisposeAsync();
                 Global.Hub = null;
                 Global.Auth2Success = null;
                 GlobalClass.LoginId = null;
                 _isInit = false;
             });
        }

        #endregion

        #region 初始化线程

        private bool _isInit = false;
        public Task InitAsync()
        {
            return Task.Run(async () =>
            {
                if (_isInit)
                    return;

                try
                {
                    this.Invoke(new Action(() =>
                    {
                        this.RefreshBtnIsEnabled = false;
                        Progress = "检查网络链接 . ";
                    }));

                    if (!await Global.TestNetworkConnectionAsync())
                    {
                        Progress = "网络链接失败";
                        return;
                    }

                    if (!Global.InitUserPCInfo())
                    {
                        Progress = "未获取到客户端信息";
                        return;
                    }
                    if (!Global.InitDeviceId())
                    {
                        Progress = "未获取到设备信息";
                        return;
                    }

                    if (!await Global.InitHubConnectionAsync())
                    {
                        Progress = "初始化失败";
                        return;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"未知异常{ex.Message}");
                }

                _isInit = true;

            });
        }

        #endregion

        //检查当前客户端是否可以自动登录
        private Task CheckCanAutoLoginAsync()
        {
            return Task.Run(async () =>
            {
                try
                {
                    Progress = "检查自动登录.";
                    bool isAutoLogin = false;
                    //检查当前客户端是否可以自动登录
                    var canAutoLoginResponse = await AppSetting.AutoJTLClient.ExecuteAsync(new UserCanAutoLoginRequest()
                    {
                        LoginId = GlobalClass.LoginId,
                        DeviceId = GlobalClass.DeviceId,
                        Category = GlobalClass.Category,
                        IsInternal = GlobalClass.IsInternal == null ? false : (bool)GlobalClass.IsInternal,
                        Userlnfos = GlobalClass.UserPCInfos,
                        ClientVersion = GlobalClass.CurrentVersion,
                        SoftHostVersion = GlobalClass.SoftWareHostVersion
                    });
                    Progress = "检查自动登录..";
                    if (canAutoLoginResponse != null
                        && !canAutoLoginResponse.Msg.Equals("成功"))
                    {
                        //重新初始化
                        await this.InitialMethod();
                        Progress = "重新初始化成功";
                        return;
                    }

                    if (canAutoLoginResponse != null
                        && canAutoLoginResponse.Code.Equals(0)
                        && canAutoLoginResponse.Data.Success)
                    {
                        isAutoLogin = true;
                        Progress = "可以自动登录.";
                    }

                    if (isAutoLogin)
                    {
                        GlobalClass.NickName = canAutoLoginResponse.Data?.nickName;
                        GlobalClass.RegId = canAutoLoginResponse.Data?.regId;
                        GlobalClass.user.strUsrId = canAutoLoginResponse.Data?.unionId;
                        GlobalClass.user.strUsrName = canAutoLoginResponse.Data?.regId;
                        GlobalClass.VersionDesc = canAutoLoginResponse.Data?.Version_desc;

                        if (DateTime.TryParse(canAutoLoginResponse.Data?.expiryDate, out DateTime dtResult))
                        {
                            GlobalClass.ExpireDate = dtResult;
                        }
                        else
                        {
                            GlobalClass.ExpireDate = DateTime.MinValue;
                        }

                        GlobalClass.Internal_tag = canAutoLoginResponse.Data?.internal_tag;
                        GlobalClass.EditionAbilityModules = canAutoLoginResponse.Data?.moduleIDList?.ToList();

                        this.Invoke(new Action(() =>
                        {
                            Progress = "可以自动登录..";
                            //回调
                            WeakReferenceMessenger.Default.Send(new CanAutoLoginResultModelMessage());
                            //{
                            //    CanAutoLogin = isAutoLogin
                            //});
                            Progress = "可以自动登录...";
                        }));
                    }
                    else
                    {
                        await ShowSubscribeQRCodeAsync();
                        Progress = "请扫二维码并关注公众号";
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"未知异常{ex.Message}");
                }
            });
        }

        //未关注公众号
        public async void Receive(OpenOplatformNotifyAuth2Success message)
        {
            try
            {
                Debug.WriteLine($"{message.NickName} {message.UnionId}");
                NickName = $"昵称: {message.NickName}";
                if (message.Subscribe)
                    await LoginAsync();
                else
                {
                    QRCodeImage = null;
                    Progress = "请关注公众号";
                    Debug.WriteLine($"Subscribe:{message.Subscribe}");
                    //await ShowSubscribeQRCodeAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"未知异常{ex.Message}");
            }
        }

        //已关注公众号
        public async void Receive(OfficialSubscribeSuccess message)
        {
            try
            {
                Debug.WriteLine($"OfficialSubscribeSuccess：{message.NickName}");
                Progress = "已关注公众号，跳转中...";
                await LoginAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"未知异常{ex.Message}");
            }
        }

        //获取二维码
        public Task<bool> ShowSubscribeQRCodeAsync()
        {
            return Task.Run(async () =>
            {
                try
                {
                    var loginLinkResponse = await AppSetting.AutoJTLClient.ExecuteAsync(new OfficialSubscribeLinkReqeust()
                    {
                        LoginId = GlobalClass.LoginId
                    });

                    if (loginLinkResponse == null || !loginLinkResponse.Code.Equals(0))
                    {
                        Progress = "获取关注二维码失败";
                        return false;
                    }
                    else
                    {
                        string url = loginLinkResponse.Data.Url;
                        if (!loginLinkResponse.Data.Url.Contains("?"))
                            url = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + HttpUtility.UrlEncode(loginLinkResponse.Data.Ticket);




                        //下载二维码
                        Progress = "加载二维码.";

                        this.Invoke(new Action(() =>
                        {
                            BitmapImage img = new BitmapImage();

                            try
                            {
                                this.Invoke(new Action(() =>
                                {
                                    Progress = "加载二维码..";

                                    //img.CacheOption = BitmapCacheOption.OnLoad;
                                    //img.BeginInit();
                                    //img.StreamSource = await new HttpClient().GetStreamAsync(url);
                                    //img.EndInit();

                                    byte[] imageBytes = new WebClient().DownloadData(url);
                                    MemoryStream ms = new MemoryStream(imageBytes);
                                    img.BeginInit();
                                    img.StreamSource = ms;
                                    img.EndInit();

                                    Progress = "加载二维码...";
                                }));
                            }
                            catch (HttpRequestException ex)
                            {
                                // the download failed, log error
                                throw new Exception($"未知异常{ex.Message}");
                            }


                            //BitmapImage img = LoadImage(url); //GetQRCodeImage(url);//new BitmapImage(new Uri(url));

                            if (img != null)
                            {
                                QRCodeImage = img;
                                Progress = "请扫二维码并关注公众号";
                            }
                        }));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"未知异常{ex.Message}");
                }
                finally
                {
                    this.RefreshBtnIsEnabled = true;
                }

                return true;
            });
        }

        //登录
        private Task LoginAsync()
        {
            return Task.Run(async () =>
            {
                try
                {
                    var loginResponse = await AppSetting.AutoJTLClient.ExecuteAsync(new UserLoginRequest()
                    {
                        LoginId = GlobalClass.LoginId,
                        //DeviceId = GlobalClass.DeviceId,
                        //Category = GlobalClass.Category,
                        //IsInternal = GlobalClass.IsInternal == null ? false : (bool)GlobalClass.IsInternal,
                        //Userlnfos = GlobalClass.UserPCInfos,
                        //ClientVersion = GlobalClass.CurrentVersion,
                        //SoftHostVersion = GlobalClass.SoftWareHostVersion
                    });

                    if (loginResponse == null || !loginResponse.Code.Equals(0))
                    {
                        Progress = $"登录失败，{loginResponse.Msg}";

                        await this.RefreshSubscribeQRCodeAsync();

                        return;
                    }

                    GlobalClass.NickName = loginResponse.Data?.nickName;
                    GlobalClass.RegId = loginResponse.Data?.regId;
                    GlobalClass.user.strUsrId = loginResponse.Data?.unionId;
                    GlobalClass.user.strUsrName = loginResponse.Data?.regId;
                    GlobalClass.VersionDesc = loginResponse.Data?.Version_desc;

                    if (DateTime.TryParse(loginResponse.Data?.expiryDate, out DateTime dtResult))
                    {
                        GlobalClass.ExpireDate = dtResult;
                    }
                    else
                    {
                        GlobalClass.ExpireDate = DateTime.MinValue;
                    }

                    GlobalClass.Internal_tag = loginResponse.Data?.internal_tag;
                    GlobalClass.EditionAbilityModules = loginResponse.Data?.moduleIDList?.ToList();

                    this.Invoke(new Action(() =>
                    {
                        WeakReferenceMessenger.Default.Send(new LoginSuccessModelMessage());
                    }));
                }
                catch (Exception ex)
                {
                    throw new Exception($"未知异常{ex.Message}");
                }
            });
        }
    }
}