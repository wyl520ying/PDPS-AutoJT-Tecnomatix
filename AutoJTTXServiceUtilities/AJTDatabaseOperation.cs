using AutoJTTXServiceUtilities.AutoJTServiceReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

#if INTERNAL

using AutoJTTXServiceUtilities.Request;
using AutoJTL.SDK.Strandard;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AutoJTTXServiceUtilities.Structure;

#endif

#if EXTERNAL

using AutoJTTXServiceUtilities.Request;
using AutoJTL.SDK.Strandard;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AutoJTTXServiceUtilities.Structure;
using System.Net;

#endif

namespace AutoJTTXServiceUtilities
{
    public class AJTDatabaseOperation
    {
        #region API Client

#if INTERNAL

        //终结点地址
        public static string Endpoint => "http://www.autojt.com";
        //client
        public static IAutoJTLClient AutoJTLClient = new AutoJTLClient(new AutoJTLOptions()
        {
            Endpoint = Endpoint,
        });

#endif

#if EXTERNAL


        //终结点地址
        public static string Endpoint => "http://www.autojt.com";
        //client
        public static IAutoJTLClient AutoJTLClient = new AutoJTLClient(new AutoJTLOptions()
        {
            Endpoint = Endpoint,
        });

#endif

        #endregion


        //私服版本的服务器ip
#if PSV

        public static string PSVAddress { get; set; }

#endif


        #region ping ip

        static string TestNetworkIP
        {
            get
            {
#if DEBUG
                return "localhost";
#elif PSV
                return PSVAddress;
#else
                return "www.autojt.com";
#endif

            }
        }

        public static async Task<bool> TestNetworkConnectionAsync()
        {
            bool isSuccess = false;
            try
            {
                var reply = (await new Ping().SendPingAsync(AJTDatabaseOperation.TestNetworkIP, 120));
                isSuccess = reply.Status == IPStatus.Success;
            }
            catch (Exception ex)
            {
                return false; //不通
            }
            return isSuccess;
        }

        static bool NetworkConnection(string targetIP)
        {
            Ping pingSender = new Ping();
            PingReply reply;
            try
            {
                reply = pingSender.Send(targetIP, 120);//第一个参数为ip地址，第二个参数为ping的时间
            }
            catch
            {
                return false; //不通
            }

            if (reply.Status == IPStatus.Success)
            {
                return true; //通
            }
            else
            {
                return false; //不通
            }
        }

        #endregion

        #region 检查网络连接

        public static bool CheckNetworkConnection(int fire = 5)//检查网络连接
        {
#if DEBUG
            //ip
            string ServerAddress = "localhost";
#elif PSV
            //ip
            string ServerAddress = PSVAddress;
#else
            //ip
            string ServerAddress = "www.autojt.com";
#endif

            //网络连接状态
            bool isAccess = false;

            try
            {
                //检查网络连接5次
                for (int i = 0; i < fire; i++)
                {
                    isAccess = NetworkConnection(ServerAddress);
                    if (isAccess)
                    {
                        break;
                    }
                }
            }
            catch
            {
                isAccess = false;
            }
            return isAccess;
        }

        #endregion

        #region 连接云服务器 获取实例

        //连接云服务器
        public static AutoJTServiceClient ServiceClientMethod()
        {
            bool isping = CheckNetworkConnection(5);//检查网络连接
            if (!isping)
            {
                return null;
            }

            AutoJTServiceClient autoJTServiceClient = GetNewServiceClientMethod();

            return autoJTServiceClient;
        }
        public static AutoJTServiceClient ServiceClientMethod(bool isShowMessage = true)
        {
            bool isping = CheckNetworkConnection();//检查网络连接
            if (!isping)
            {
                if (isShowMessage)
                {
                    throw new Exception("网络服务器连接故障！");
                }
                else
                {
                    return null;
                }
            }

            AutoJTServiceClient autoJTServiceClient = GetNewServiceClientMethod();

            return autoJTServiceClient;
        }
        static AutoJTServiceClient GetNewServiceClientMethod()
        {
            AutoJTServiceReference.AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                /*
                <system.serviceModel>
                   <bindings>
                     <basicHttpBinding>
                       <binding name="BasicHttpBinding_IAutoJTService" maxBufferPoolSize="2147483647" maxReceivedMessageSize="2147483647">
                         <readerQuotas maxDepth="32" maxStringContentLength="2147483647" maxArrayLength="2147483647" maxBytesPerRead="2147483647" maxNameTableCharCount="2147483647" />
                       </binding>
                     </basicHttpBinding>
                   </bindings>
                   <client>
                     <endpoint address="http://www.autojt.com:8002/" binding="basicHttpBinding" bindingConfiguration="BasicHttpBinding_IAutoJTService" contract="AutoJTServiceReference.IAutoJTService" name="BasicHttpBinding_IAutoJTService" />
                   </client>
                 </system.serviceModel>        

                */

                BasicHttpBinding binding = new BasicHttpBinding();

                binding.Name = "autojtBinding";//bindingConfiguration="autojtBinding"
                binding.MaxBufferPoolSize = 2147483647;
                binding.MaxBufferSize = 2147483647;
                binding.MaxReceivedMessageSize = 2147483647;

                binding.ReaderQuotas.MaxDepth = 2000000;
                binding.ReaderQuotas.MaxStringContentLength = 2147483647;
                binding.ReaderQuotas.MaxArrayLength = 2147483647;
                binding.ReaderQuotas.MaxBytesPerRead = 2147483647;
                binding.ReaderQuotas.MaxNameTableCharCount = 2147483647;

                //EndpointAddress endPointAddress = new EndpointAddress("http://www.autojt.com:8002/");
#if PSV
                EndpointAddress endPointAddress = new EndpointAddress($"http://{AJTDatabaseOperation.PSVAddress}:8002/");
#else
                EndpointAddress endPointAddress = new EndpointAddress("http://www.autojt.com:8002/");
#endif

                autoJTServiceClient = new AutoJTServiceReference.AutoJTServiceClient(binding, endPointAddress);

            }
            catch
            {
                autoJTServiceClient = null;
            }

            return autoJTServiceClient;
        }

        #endregion

        #region 检查新版本

        //检查版本操作内部
        public static string CheckLastReleaseVer(out string VERSIONCONTENTS, out string FORCEDUPDATE, out string downloadLink, out string downloadLink2)
        {
            string lastReleaseVer = string.Empty;

            VERSIONCONTENTS = string.Empty; ;
            FORCEDUPDATE = string.Empty;
            downloadLink = string.Empty;
            downloadLink2 = string.Empty;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
#if INTERNAL
                //INTERNAL
                //查询版本号
                lastReleaseVer = autoJTServiceClient.GetAutoJTTecnomatixVersionCode3(out VERSIONCONTENTS, out FORCEDUPDATE, out downloadLink, out downloadLink2);
#elif EXTERNAL
                //External
                //查询版本号
                lastReleaseVer = autoJTServiceClient.GetAutoJTTecnomatixVersionCodeExternal3(out VERSIONCONTENTS, out FORCEDUPDATE, out downloadLink, out downloadLink2);
#endif
            }
            catch
            {
                throw;
            }
            finally
            {
                autoJTServiceClient?.Close();
            }

            return lastReleaseVer;
        }

        #endregion

        #region 模块用量统计

        public static void ModelInfosSum(DataTable dataTable1)
        {
            return;
            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                //Task.Run(Action action)将任务放在线程池队列，返回并启动一个Task
                Task.Run(async () =>
                {
                    autoJTServiceClient = ServiceClientMethod();
                    await autoJTServiceClient.Module_usage_statistics_21Async(dataTable1);
                });
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close();
            }
        }

        #endregion

        #region 焊点协同

        //检查数据表 ,并且创建 (tx_project_mcm)
        public static bool Check_AND_CreateTx_tb_MCM_1(string tbName, out string result, out string error)
        {
            bool bl = false;
            result = string.Empty;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                //检查数据表 ,并且创建
                bl = autoJTServiceClient.CreateTx_Project_MCM_new(tbName, out result, out error);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return bl;
        }

        //添加数据 (tx_project_mcm)
        public static bool AddinProject_Model2DB_MCM_1(string PROJECT, string MODEL, string REGION, string uuid, string OPERATOR, string tbname, out string infos)
        {
            bool bl = false;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                //添加数据
                bl = autoJTServiceClient.AddinProject_Model2DB_MCM_new(PROJECT, MODEL, REGION, uuid, OPERATOR, tbname, out infos);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return bl;
        }

        //检查lib表是否存在, 新建或清空数据
        public static bool _Create_MCM_LibraryTable(string tbname, out string infos98)
        {
            bool bl = false;
            infos98 = string.Empty;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                //检查数据表 ,并且创建
                bl = autoJTServiceClient.Create_MCM_LibraryTable(tbname, out infos98);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return bl;
        }

        //批量插入lib数据
        public static bool Bulk_InsertMCMData_MCM(DataTable dataTable1, string tbName, out string infos, out int irows)
        {
            bool bl = false;
            infos = string.Empty;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                //检查数据表 ,并且创建
                bl = autoJTServiceClient.BulkInsertMCMData_MCM(dataTable1, tbName, out infos, out irows);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return bl;
        }

        //创建并批量添加User数据 MCM
        public static bool CreateAndBulkInsertUserMCMData_MCM_1(DataTable dataTable1, string tbName, out string infos, out int irows, out int icount)
        {
            bool bl = false;
            infos = string.Empty;
            irows = 0;
            icount = 0;

            if (dataTable1.Rows.Count == 0)
            {
                return false;
            }

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                //添加数据
                bl = autoJTServiceClient.CreateAndBulkInsertUserMCMData_MCM(dataTable1, tbName, out infos, out irows, out icount);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return bl;
        }

        //清空表
        public static void Empty_table(string taName, out string result)
        {
            result = string.Empty;

            if (string.IsNullOrEmpty(taName))
            {
                return;
            }

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                //清空数据
                result = autoJTServiceClient.EmptyTable_new(taName);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }
        }

        //删除项目
        public static void Delete_project(string uuid,
            out string infos_1, out string infos_2, out string infos_3,
            out int icount_1, out int icount_2, out int icount_3)
        {
            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                infos_1 = autoJTServiceClient.DeleteProject_1(uuid, out infos_2, out infos_3, out icount_1, out icount_2, out icount_3);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }
        }


        //判断当前项目和车型是否存在数据库

        //查询project和model是否存在
        public static DataTable Query_ProjectModleExists_MCM(string uuid, string openid, out int status, out string infos)
        {
            DataTable bl = null;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                bl = autoJTServiceClient.Query_ProExists_uuid_MCM(uuid, openid, out status, out infos);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return bl;
        }

        //根据openID查询username
        public static string Query_UserNameByOpenID_MCM(string openid, out string infos)
        {
            string bl = null;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                bl = autoJTServiceClient.Query_UserNameByOpenID(openid, out infos);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return bl;
        }

        //根据openid查询所有项目
        public static DataTable Query_AllProByOpenID_MCM(string openid, out string infos)
        {
            DataTable bl = null;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                bl = autoJTServiceClient.Query_AllPro4OpenID(openid, out infos);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return bl;
        }

        //查询用户参与的项目
        public static DataTable Query_UsersTakeProjectsOpenID_MCM(string openid, out string infos)
        {
            DataTable bl = null;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                bl = autoJTServiceClient.Query_UsersTakeProjectsOpenID(openid, out infos);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return bl;
        }



        /// <summary>
        /// 删除重复项,去重复项
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filedNames">common_name:所要筛选的字段</param>
        /// <returns></returns>
        public static DataTable Distinct(DataTable dt, string[] filedNames)
        {
            DataView dv = dt.DefaultView;
            DataTable DistTable = dv.ToTable(dt.TableName, true, filedNames);
            return DistTable;
        }


        /// <summary>
        /// DataTable分组
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<DataTable> GetDataTable(DataTable dt, string columnNames)
        {
            List<DataTable> dict = new List<DataTable>();

            //计算指定类型的数量
            DataView dv = new DataView(dt);
            DataTable dtTJ = dv.ToTable(true, columnNames);

            //只有一种op 直接赋值
            if (dtTJ.Rows.Count == 1)
            {
                return new List<DataTable>() { dt };
            }

            for (int i = 0; i < dtTJ.Rows.Count; i++)
            {
                var res = dt.Select(columnNames + " = '" + dtTJ.Rows[i][columnNames].ToString() + "'");//按条件查询出符合条件的行
                DataTable resDt = dt.Clone();//克隆一个壳子
                foreach (var j in res)
                {
                    resDt.ImportRow(j);//将符合条件的行放进壳子里
                }

                dict.Add(resDt);//将键值对存起来
            }

            return dict;
        }


        //0 从数据库get lib 根据project model 组合
        //1 查询所有的用户名
        //2 根据用户名和project model 组合的用户表逐个表查询所有的数据
        //3 向客户端返回所有查询到的用户数据(UNION ALL) List<Datable>
        public static void Tx_Refresh_MCM_1(string uuid, out DataTable dataTable_LIB, out DataTable[] dataTable_users_MCMS, out DataTable nikeNameDic, out string infos)
        {
            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                dataTable_LIB = autoJTServiceClient.Tx_Refresh_MCM_new1(uuid, out dataTable_users_MCMS, out nikeNameDic, out infos);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

        }

        #endregion

        #region 登录相关

        #region 退出登录

        //服务端退出自动登录
        public static int QuitLogin(string uuid)
        {
            if (uuid is null)
            {
                return -1;
            }
            int result = default;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                //服务端退出自动登录
                result = autoJTServiceClient.ExitLoginMethod(uuid);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return result;
        }

        #endregion

        #region 微信登录

        //微信登录记录用户信息
        public static void CloudDBOperation_wechat(string category, string userlnfos, string clientVersion, string txtUsername, string _SoftWareHostVersion)
        {
            AutoJTServiceClient autoJTServiceClient = null;

            try
            {
                autoJTServiceClient = ServiceClientMethod();
                autoJTServiceClient.GetUserFromDatabase_wechat2(category, userlnfos, clientVersion, txtUsername, _SoftWareHostVersion);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

        }

        #endregion

        #region 注册

        //查询用户名是否存在
        public static bool QueryUsernameFidle_1(string user)
        {
            bool result = false;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                //查询用户名是否存在
                result = autoJTServiceClient.QueryUsernameFidle(user);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return result;
        }

        //创建新用户
        public static string CreateAccount_1(string username, string password, string email)
        {
            string result = string.Empty;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                result = autoJTServiceClient.CreateAccount(username, password, email);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return result;
        }

        //更新wechat nicename
        public static string UpdateWchatNiceName_1(string username, string email, out bool m_dataSet, out string remarkname, out bool isjt)
        {
            string result = string.Empty;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                result = autoJTServiceClient.UpdateWchatNiceName_2Net6(username, email, out m_dataSet, out remarkname, out isjt);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return result;
        }



        #endregion


        #region 私服身份验证

#if PSV

        public static bool CheckPrivateServersAuth(string code)
        {
            bool result = default;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                //result = await autoJTServiceClient.ValidateAuthAsync(code);
                result = autoJTServiceClient.ValidateAuth(code);
            }
            catch(Exception ex)
            {
                if (ex is EndpointNotFoundException)
                {
                    throw new Exception("服务器出现故障, 请稍后再试");
                }
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return result;
        }

#endif

        #endregion


        #region 新版微信登录

        //判断用户是否可以自动登录 GetUnionIDNickName4ClientInfos
        public static bool GetUnionIDNickName4ClientInfos(
            bool isInternal,
            string category,
            string userlnfos,
            string clientInfos,
            string clientVersion,
            string softHostVersion,
            string ip,
            out string internal_tag,
            out string expiryDate,
            out string unionId,
            out string nickName, out string regid,
            out string version_desc,
            out string[] moduleIDList)
        {
            bool result = default;


            internal_tag = null;
            expiryDate = null;
            unionId = null;
            nickName = null;
            version_desc = null;
            moduleIDList = null;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                result = autoJTServiceClient.GetUnionIDNickName4ClientInfos(
                    isInternal,
                    category,
                    userlnfos,
                    clientInfos,
                    clientVersion,
                    softHostVersion,
                    ip,
                    out internal_tag,
                    out expiryDate,
                    out unionId,
                    out nickName, out regid,
                    out version_desc,
                    out moduleIDList);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return result;
        }

        //用户身份验证 UserAuthentication
        public static bool UserAuthentication(
            string unionid,
            string nickName,

            bool isInternal,
            string category,
            string userlnfos,
            string clientVersion,
            string softHostVersion,
            string ip,
            out string internal_tag,
            out string expiryDate,
            out string version_desc,
            out string[] moduleIDList, out string regid)
        {
            bool result = default;


            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                result = autoJTServiceClient.UserAuthentication(
                    unionid,
                    nickName,

                    isInternal,
                    category,
                    userlnfos,
                    clientVersion,
                    softHostVersion,
                    ip,
                    out internal_tag,
                    out expiryDate,
                    out version_desc,
                    out moduleIDList, out regid, out string nickname);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return result;
        }

        public static bool ChangeNickName(string uuid, string newName, bool isinter)
        {
            bool result = default;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                result = autoJTServiceClient.UpgradeNickname(uuid, newName, isinter);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return result;
        }

#if DEBUG
        public static string QueryAllComboMealInfo(string uuid)
        {
            string result = default;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                result = autoJTServiceClient.QueryAllComboMealInfo(uuid, out string dt, out string userinfo);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return result;
        }
#endif

        #endregion

        //查询用户所有可用的版本
        public static string QueryUserAllLic(string uuid)
        {
            string result = default;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                result = autoJTServiceClient.GetUserAllEnableVersion(uuid);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return result;
        }

        //如果用户已经购买了高版本, 再次购买低版本得时候提示不允许降级版本(基础版 => 高级版 => 专业版)
        public static bool CheckUserOwnerVersion(string uuid, string combo_cod, out int? ownerVer)
        {
            //初始化可以购买
            bool result = true;
            //已经购买得版本
            ownerVer = null;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                result = autoJTServiceClient.CheckUserOwnerVersion(uuid, combo_cod, out ownerVer);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return result;
        }

        #region EXTERNAL版本的登录弹窗

#if EXTERNAL

        public static async Task<string> GetPopularMsg()
        {
            string result = default;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                await Task.Run(async () =>
                {
                    autoJTServiceClient = ServiceClientMethod();
                    result = await autoJTServiceClient.GetLoginPopularAsync();
                });
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return result;
        }
#endif

        #endregion

        #region EXTERNAL版本的查询模块对应的版本描述

#if EXTERNAL

        public static async Task<string> GetModelIDVersionDesc(string fun_id)
        {
            string result = default;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                //取消异步,改为同步线程
                /*await Task.Run(async () =>
                {
                    autoJTServiceClient = ServiceClientMethod();
                    result = await autoJTServiceClient.QueryVersionDesc4ModuleIDAsync(fun_id);
                });
                */

                //同步线程
                autoJTServiceClient = ServiceClientMethod();
                result = await autoJTServiceClient.QueryVersionDesc4ModuleIDAsync(fun_id);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return result;
        }

#endif

        #endregion

        #endregion

        #region CPP运行时接口

        //GetUrlCPPRuntime 获取cpp下载地址接口
        public static async Task<string> HolenSiesichdieURLderCPPLaufzeit()
        {
            string result = default;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                result = await autoJTServiceClient.GetCPPRuntimeUrlAsync();
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return result;
        }

        #endregion

        #region 11.Automatic Weld Distribution


        public static double[] CalcWD_Rotation4(double[] txTrans_new_world_num, double[] txTrans_new_apperance_word_Inverse_num, double[] txTransformation_old_apperance_symmetry_num,
                                                    bool istxPartAppearance_newNOTNull,
                                                    bool isMirrors,
                                                    out string exInfos)
        {
            double[] txTranslate1 = null;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                txTranslate1 = autoJTServiceClient.CalcWD_Rotation_4(txTrans_new_world_num, txTrans_new_apperance_word_Inverse_num, txTransformation_old_apperance_symmetry_num,
                                                                        istxPartAppearance_newNOTNull,
                                                                        isMirrors,
                                                                        out exInfos);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return txTranslate1;
        }
        public static double[] CalcWD_Rotation5(double[] txTrans_new_world_num, double[] txTrans_new_apperance_word_Inverse_num, double[] txTransformation_old_apperance_symmetry_num,
                                                   bool istxPartAppearance_newNOTNull,
                                                   bool isMirrors,
                                                   out string exInfos)
        {
            double[] txTranslate1 = null;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                txTranslate1 = autoJTServiceClient.CalcWD_Rotation_5(txTrans_new_world_num, txTrans_new_apperance_word_Inverse_num, txTransformation_old_apperance_symmetry_num,
                                                                        istxPartAppearance_newNOTNull,
                                                                        isMirrors,
                                                                        out exInfos);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return txTranslate1;
        }
        public static double[] CalcVIA_Rotation4(double[] txTrans_old_VIA_OLD_apper_num, double[] txTrans_old_OLD_apper_world_num, out string exInfos)
        {
            double[] txTranslate1 = null;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                txTranslate1 = autoJTServiceClient.CalcVIA_Rotation_4(txTrans_old_VIA_OLD_apper_num, txTrans_old_OLD_apper_world_num, out exInfos);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return txTranslate1;
        }


        #endregion

        #region 1.ExportGun

        public static DataTable Calc_RPY2Matrix_Transform2(DataTable dataTable1, out string exInfos)
        {
            DataTable txTranslate1 = null;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                txTranslate1 = autoJTServiceClient.Calc_RPY2Matrix_Transform2(dataTable1, out exInfos);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return txTranslate1;
        }

        #endregion

        #region 3.CaptureWeldPoint

        public static DataTable GetDirect1(DataTable dataTable1, out object[] min, out object[] max, out object[] CENTER, out string exInfos)
        {
            DataTable datatable1 = null;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                datatable1 = autoJTServiceClient.CalcNoteQuadrant(dataTable1, out min, out max, out CENTER, out exInfos);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return datatable1;
        }

        #endregion

        #region 4.QuickJoint

        public static bool CalcRealBoxLocation(double[] txFrameCenter,
                                                double[] leftLower,
                                                double[] rightUpper,
                                                double[] txVector_ora,
                                                bool isCalcWordLoc,
                                                out double[] bottomLeftLower,
                                                out double[] bottomLeftUpper,
                                                out double[] bottomRightLower,
                                                out double[] bottomRightUpper,
                                                out double[] topLeftLower,
                                                out double[] topLeftUpper,
                                                out double[] topRightLower,
                                                out double[] topRightUpper,
                                                out double[] pp0,
                                                out double[] p1_word,
                                                out double[] p2_word)
        {
            bool bl = false;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                bl = autoJTServiceClient.CalcRealBoxLocation(txFrameCenter,
                                                 leftLower,
                                                 rightUpper,
                                                 txVector_ora,
                                                isCalcWordLoc,
                                                out bottomLeftLower,
                                                out bottomLeftUpper,
                                                out bottomRightLower,
                                                out bottomRightUpper,
                                                out topLeftLower,
                                                out topLeftUpper,
                                                out topRightLower,
                                                out topRightUpper,
                                                out pp0,
                                                out p1_word,
                                                out p2_word);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return bl;
        }

        public static bool MultiCalcRealBoxLocation(
                                                    double[] txFrameCenter_1,
                                                    double[] leftLower_1,
                                                    double[] rightUpper_1,
                                                    double[] txVector_ora_1,
                                                    double[] txFrameCenter_2,
                                                    double[] leftLower_2,
                                                    double[] rightUpper_2,
                                                    double[] txVector_ora_2,
                                                    bool isCalcWordLoc_1,
                                                    bool isCalcWordLoc_2,
                                                    out double[] bottomLeftLower_1,
                                                    out double[] bottomLeftUpper_1,
                                                    out double[] bottomRightLower_1,
                                                    out double[] bottomRightUpper_1,
                                                    out double[] topLeftLower_1,
                                                    out double[] topLeftUpper_1,
                                                    out double[] topRightLower_1,
                                                    out double[] topRightUpper_1,
                                                    out double[] pp0_1,
                                                    out double[] p1_word_1,
                                                    out double[] p2_word_1,
                                                    out double[] bottomLeftLower_2,
                                                    out double[] bottomLeftUpper_2,
                                                    out double[] bottomRightLower_2,
                                                    out double[] bottomRightUpper_2,
                                                    out double[] topLeftLower_2,
                                                    out double[] topLeftUpper_2,
                                                    out double[] topRightLower_2,
                                                    out double[] topRightUpper_2,
                                                    out double[] pp0_2,
                                                    out double[] p1_word_2,
                                                    out double[] p2_word_2)
        {
            bool bl = false;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                bl = autoJTServiceClient.MultiCalcRealBoxLocation(
                                                     txFrameCenter_1,
                                                     leftLower_1,
                                                     rightUpper_1,
                                                     txVector_ora_1,
                                                     txFrameCenter_2,
                                                     leftLower_2,
                                                     rightUpper_2,
                                                     txVector_ora_2,
                                                     isCalcWordLoc_1,
                                                     isCalcWordLoc_2,
                                                    out bottomLeftLower_1,
                                                    out bottomLeftUpper_1,
                                                    out bottomRightLower_1,
                                                    out bottomRightUpper_1,
                                                    out topLeftLower_1,
                                                    out topLeftUpper_1,
                                                    out topRightLower_1,
                                                    out topRightUpper_1,
                                                    out pp0_1,
                                                    out p1_word_1,
                                                    out p2_word_1,
                                                    out bottomLeftLower_2,
                                                    out bottomLeftUpper_2,
                                                    out bottomRightLower_2,
                                                    out bottomRightUpper_2,
                                                    out topLeftLower_2,
                                                    out topLeftUpper_2,
                                                    out topRightLower_2,
                                                    out topRightUpper_2,
                                                    out pp0_2,
                                                    out p1_word_2,
                                                    out p2_word_2);


            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return bl;
        }

        public static double[] ReCalcRotaFrme(double[] p1_word, double[] p2_word, double[] p11_word, double[] p21_word)
        {
            double[] v = null;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                v = autoJTServiceClient.ReCalcRotaFrme(p1_word, p2_word, p11_word, p21_word);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return v;
        }

        #endregion

        #region 5.WedlPointChecker

#if INTERNAL

        public static async Task<List<double[]>> GetWedlPointChecker(List<DoubleArrayPair> pairs)
        {
            // Serialize double[] to JSON string
            string json1 = JsonConvert.SerializeObject(pairs);

            var result = await AutoJTLClient.ExecuteAsync(new WeldLocCheckerRequest()
            {
                Pairs = json1,
            });

            // Deserialize JSON string to double[]
            //double[] result2 = JsonConvert.DeserializeObject<double[]>(result.Data.Doubles);

            return result.Data.ListDoubles;
        }
        public static async Task<List<double[]>> GetWedlPointChecker2(List<DoubleArrayPair> pairs)
        {
            // Serialize double[] to JSON string
            string json1 = JsonConvert.SerializeObject(pairs);

            var result = await AutoJTLClient.ExecuteAsync(new WeldLocCheckerRequest2()
            {
                Pairs = json1,
            });

            // Deserialize JSON string to double[]
            //double[] result2 = JsonConvert.DeserializeObject<double[]>(result.Data.Doubles);

            return result.Data.ListDoubles;
        }

#endif

        #endregion

        #region 外部用户的邀请逻辑

#if EXTERNAL

        public static async Task<bool> PopupDiaMethod(NewUserInviCode2 user)
        {
            // Serialize double[] to JSON string
            string json1 = JsonConvert.SerializeObject(user);

            var result = await AutoJTLClient.ExecuteAsync2(new PopupDiaRequest()
            {
                Json = json1,
            });

            // Deserialize JSON string to double[]
            //double[] result2 = JsonConvert.DeserializeObject<double[]>(result.Data.Doubles);

            return result.Data.Success;
        }
        public static async Task<bool> InviExistMethod(NewUserInviCode user)
        {
            // Serialize double[] to JSON string
            string json1 = JsonConvert.SerializeObject(user);

            var result = await AutoJTLClient.ExecuteAsync2(new InviExistRequest()
            {
                Json = json1,
            });

            // Deserialize JSON string to double[]
            //double[] result2 = JsonConvert.DeserializeObject<double[]>(result.Data.Doubles);

            return result.Data.Success;
        }

        public static async Task<string> GetInviCode(NewUserInviCode3 user)
        {
            // Serialize double[] to JSON string
            string json1 = JsonConvert.SerializeObject(user);

            var result = await AutoJTLClient.ExecuteAsync2(new InviCodeRequest()
            {
                Json = json1,
            });

            // Deserialize JSON string to double[]
            //double[] result2 = JsonConvert.DeserializeObject<double[]>(result.Data.Doubles);

            return result.Data.Code;
        }

#endif

        #endregion
    }
}
