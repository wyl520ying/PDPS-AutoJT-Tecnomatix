using AutoJTLicensingTool.Views;
using AutoJTMessageUtilities;
using AutoJTTXCoreUtilities;
using AutoJTTXUtilities.Controls;
using CommunityToolkit.Mvvm.Messaging;
using Contrib.Hub;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace AutoJTLicensingTool.Common
{
    public class Global
    {
        //用于通信
        public static HubConnection Hub { get; set; }

        //开放平台通知身份验证成功
        public static OpenOplatformNotifyAuth2Success Auth2Success { get; set; }

        //初始化用户电脑信息
        public static bool InitUserPCInfo()
        {
            try
            {
                if (GetEnvironment.GetEnv(out string osVersionName, out string machineName, out string userName, out string domainName,
                                                   out string processorCounter, out string SystemType, out string iWorkSet))
                {
                    GlobalClass.UserPCInfos = $"{osVersionName}:{machineName}:{userName}:" +
                                  $"{domainName}:{processorCounter}:{SystemType}:{iWorkSet}";
#if DEBUG
                    GlobalClass.UserPCInfos = $"test:{GlobalClass.UserPCInfos}";
#endif
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        //初始化机器码
        public static bool InitDeviceId()
        {
            try
            {
                GlobalClass.DeviceId = GetEnvironment.CheckJsonCode();
                GlobalClass.SoftWareHostVersion = TecnomatixInfos.GetUsersDirectorySoftVersion();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //初始化hub
        public static Task<bool> InitHubConnectionAsync()
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (!string.IsNullOrEmpty(GlobalClass.LoginId))
                        return true;

                    var builder = new HubConnectionBuilder()
                        .WithUrl(AppSetting.HubEndpoint)
                        .WithAutomaticReconnect()
                        .AddJsonProtocol();

                    Hub = builder.Build();

                    Hub.On<OpenOplatformNotifyAuth2Success>(OpenOplatformNotifyAuth2Success.MethodName, (value) =>
                    {
                        Auth2Success = value;
                        GlobalClass.NickName = value?.NickName;
                        WeakReferenceMessenger.Default.Send<OpenOplatformNotifyAuth2Success>(value);
                    });

                    Hub.On<OfficialSubscribeSuccess>(OfficialSubscribeSuccess.MethodName, (value) =>
                    {
                        if (Auth2Success != null)
                        {
                            Auth2Success.Subscribe = true;
                        }
                        GlobalClass.NickName = value?.NickName;
                        WeakReferenceMessenger.Default.Send<OfficialSubscribeSuccess>(value);
                    });

                    Hub.On<AliPaySuccess>(AliPaySuccess.MethodName, (value) =>
                    {
                        WeakReferenceMessenger.Default.Send<AliPaySuccess>(value);
                    });

                    await Hub.StartAsync();

                    GlobalClass.LoginId = Hub.ConnectionId;
                }
                catch (Exception ex)
                {
                    return false;
                }
                return true;
            });
        }

        //网络联通检测
        public static Task<bool> TestNetworkConnectionAsync()
        {
            return Task.Run(async () =>
            {
                return PingHost(AppSetting.TestNetworkIP);
                bool isSuccess = false;
                try
                {
                    var reply = (await new Ping().SendPingAsync(AppSetting.TestNetworkIP, 120));
                    isSuccess = reply.Status == IPStatus.Success;
                }
                catch (Exception ex)
                {
                    return false; //不通
                }
                return isSuccess;
            });
        }

        //检查网络
        public static bool PingHost(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(nameOrAddress, 250);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
                pingable = false;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            return pingable;
        }

        //释放资源
        public static async Task DisposeAsync()
        {
            try
            {
                if (Hub != null)
                {
                    await Hub.StopAsync();
                    await Hub.DisposeAsync();
                }
            }
            catch
            {
            }
        }

        //双工
        public static ServiceClient m_serviceClient { get; set; }

        //支付管理页面
        public static bool PaymentMethod(string fun_id = "")
        {
            //内部本版禁止打开订阅窗口
            if (GlobalClass.IsInternal == true)
            {
                throw new PlatformNotSupportedException("未经授权的登录");
            }

            bool _isPaySucceed = false;

            PaymentWindow paymentWindow = new PaymentWindow(fun_id);
            paymentWindow.IsPaySucceed = (isSucced) =>
            {
                _isPaySucceed = isSucced;
            };
            WindowHelper.SetOwner(paymentWindow, AutoJTTXCoreUtilities.TecnomatixInfos.GetAppHandle());
            try
            {
                paymentWindow.ShowDialog();
            }
            catch
            {
            }

            return _isPaySucceed;
        }

        //dt转json
        public static string DataTableToJsonWithJsonNet(DataTable table)
        {
            string JsonString = string.Empty;
            JsonString = JsonConvert.SerializeObject(table);
            return JsonString;
        }
        //json2dt
        public static DataTable JsonToDataTableWithJsonNet(string json)
        {
            return JsonConvert.DeserializeObject<DataTable>(json);
        }

        //解析弹窗json
        public static Dictionary<string, string> PearsPop(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            Dictionary<string, string> mydic = new Dictionary<string, string>();

            DataTable st1 = JsonToDataTableWithJsonNet(json);

            if (st1 != null && st1.Rows.Count > 0)
            {
                foreach (DataRow  item in st1.Rows)
                {
                    try
                    {
                        string title = item["title"].ToString();
                        string msg = item["msg"].ToString();

                        if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(msg))
                        {
                            mydic.Add(title, msg);
                        }
                    }
                    catch 
                    {
                        continue;
                    }
                }
            }

            return mydic;
        }
    }
}
