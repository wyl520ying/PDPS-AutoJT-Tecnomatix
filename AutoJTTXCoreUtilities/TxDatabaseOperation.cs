using AutoJTTecnomatix.AutoJTServiceReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using System.Windows.Forms;

namespace AutoJTTXCoreUtilities
{
    public class TxDatabaseOperation
    {
        #region 云数据库操作

        //连接云服务器
        public static AutoJTServiceClient ServiceClientMethod(bool isShowMessage = true)
        {
            if (isShowMessage)
            {
                bool isping = CheckNetworkConnection();//检查网络连接
                if (!isping)
                {
                    throw new Exception("网络故障");
                }
            }
            else
            {
                bool isping = CheckNetworkConnection(5, false);//检查网络连接
                if (!isping)
                {
                    return null;
                }
            }
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

            EndpointAddress endPointAddress = new EndpointAddress("http://www.autojt.com:8002/");
            AutoJTServiceReference.AutoJTServiceClient autoJTServiceClient = new AutoJTServiceReference.AutoJTServiceClient(binding, endPointAddress);

            return autoJTServiceClient;
        }

        //登录操作
        public static DataSet CloudDBOperation_CheckIn(string pass, string txtUsername,
            out string resultstatus, out bool resultStatus2, out int resultstatus3)
        {
            DataSet ds = null;
            AutoJTServiceClient autoJTServiceClient = null;

            try
            {
                autoJTServiceClient = ServiceClientMethod();
                ds = autoJTServiceClient.GetUserFromDatabase("users", pass, txtUsername.Trim().ToLower(), out resultstatus, out resultStatus2, out resultstatus3);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return ds;
        }
        //登录操作
        public static DataSet CloudDBOperation_CheckIn(string category, string userlnfos, string clientVersion,
            string pass, string txtUsername,
            out string resultstatus, out bool resultStatus2, out int resultstatus3)
        {
            DataSet ds = null;
            AutoJTServiceClient autoJTServiceClient = null;

            try
            {
                autoJTServiceClient = ServiceClientMethod();
                ds = autoJTServiceClient.GetUserFromDatabase2(category, userlnfos, clientVersion, "users", pass, txtUsername.Trim().ToLower(), out resultstatus, out resultStatus2, out resultstatus3);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return ds;
        }
        //微信登录记录用户信息
        public static void CloudDBOperation_wechat(string category, string userlnfos, string clientVersion, string txtUsername)
        {
            AutoJTServiceClient autoJTServiceClient = null;

            try
            {
                autoJTServiceClient = ServiceClientMethod();
                autoJTServiceClient.GetUserFromDatabase_wechat2(category, userlnfos, clientVersion, txtUsername, GlobalClass.SoftWareHostVersion);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

        }

        //检查版本操作
        public static string CheckLastReleaseVer(string userinfos, out string VERSIONCONTENTS, out string FORCEDUPDATE)
        {
            string lastReleaseVer = string.Empty;
            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                //查询版本号
                lastReleaseVer = autoJTServiceClient.GetAutoJTTecnomatixVersionCode(userinfos, out VERSIONCONTENTS, out FORCEDUPDATE);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return lastReleaseVer;
        }
        //检查版本操作
        public static string CheckLastReleaseVer(out string VERSIONCONTENTS, out string FORCEDUPDATE, out string downloadLink)
        {
            string lastReleaseVer = string.Empty;
            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                //查询版本号
                lastReleaseVer = autoJTServiceClient.GetAutoJTTecnomatixVersionCode3(out VERSIONCONTENTS, out FORCEDUPDATE, out downloadLink);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return lastReleaseVer;
        }


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

        //查询数据 (Tx_Project_MCM)
        public static DataTable GetTableFromDatabase_1(string tableName)
        {
            DataTable dataTable1 = null;
            if (string.IsNullOrEmpty(tableName))
            {
                return null;
            }

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                //查询
                dataTable1 = autoJTServiceClient.GetTableFromDatabase(tableName);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return dataTable1;
        }




        //检查数据表 ,并且创建 (Tx_Project_Model_Lib_MCM)
        public static bool CreateTx_Project_Model_Lib_MCM_1(string tbName, out string result)
        {
            bool bl = false;
            result = string.Empty;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                //检查数据表 ,并且创建
                bl = autoJTServiceClient.CreateTx_Project_Model_Lib_MCM(tbName, out result);
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


        //添加数据 (Tx_Project_Model_Lib_MCM)
        public static bool AddinTx_Project_Model_Lib_MCM_1(DataTable dataTable1, string SqlTableName, out string err1, out string info)
        {
            bool bl = false;
            err1 = string.Empty;
            info = string.Empty;

            if (dataTable1.Rows.Count == 0)
            {
                return false;
            }

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                //添加数据
                bl = autoJTServiceClient.AddinTx_Project_Model_Lib_MCM(dataTable1, SqlTableName, out err1, out info);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return bl;
        }
        //查询总行数
        public static int Query_total_records(string tbName)
        {
            int iCount = -1;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                //count
                iCount = autoJTServiceClient.GetRowsCount_2(tbName);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return iCount;
        }




        //检查数据表 ,并且创建 (Tx_Project_Model_Users_MCM)        
        public static bool CreateTx_Project_Model_Users_MCM_1(string tbName, out string result)
        {
            bool bl = false;
            result = string.Empty;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                //检查数据表 ,并且创建
                bl = autoJTServiceClient.CreateTx_Project_Model_Users_MCM(tbName, out result);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return bl;
        }
        //添加数据 (Tx_Project_Model_Lib_MCM)
        public static bool AddinTx_Project_Model_Users_MCM_1(DataTable dataTable1, string SqlTableName, out string err1, out string info)
        {
            bool bl = false;
            err1 = string.Empty;
            info = string.Empty;

            if (dataTable1.Rows.Count == 0)
            {
                return false;
            }

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                //添加数据
                bl = autoJTServiceClient.AddinTx_Project_Model_Users_MCM(dataTable1, SqlTableName, out err1, out info);
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


        //获取所有的用户名
        //public static DataTable GetAllUserNameList()
        //{
        //    DataTable dataTable1 = null;

        //    AutoJTServiceClient autoJTServiceClient = null;
        //    try
        //    {
        //        autoJTServiceClient = ServiceClientMethod();
        //        dataTable1 = autoJTServiceClient.QueryField_username();
        //    }
        //    finally
        //    {
        //        if (autoJTServiceClient != null)
        //            autoJTServiceClient.Close();autoJTServiceClient = null;
        //    }

        //    return dataTable1;
        //}

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
        #endregion


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

        //发送验证码
        public static bool SendVerifyCode_1(string email)
        {
            bool result = false;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                //查询用户名是否存在
                result = autoJTServiceClient.SendVerifyCode(email);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return result;
        }

        public static bool VerificationCodeValid_1(string email, string Verification_code)
        {
            bool result = false;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                //查询验证码
                result = autoJTServiceClient.VerificationCodeValid(email, Verification_code);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }

            return result;
        }

        #endregion

        #endregion

        #region 检查网络连接
        public static bool CheckNetworkConnection(int fire = 5, bool isShwomessage = true)//检查网络连接
        {
            bool isPing = false;
            //网络连接状态
            bool NetWorkStatus = false;

            //检查网络连接
            NetworkUtil networkUtil = new NetworkUtil();
            for (int i = 0; i < fire; i++)
            {
                NetWorkStatus = networkUtil.NetworkConnection(GlobalClass.ServerAddress);
                if (NetWorkStatus == true)
                {
                    isPing = true;
                    break;
                }
            }

            if (NetWorkStatus == false && isShwomessage == true)
            {
                //ToastNotification.Show(this, NetWorkStatus.ToString() + "网络服务器连接故障！", null, 5000, (eToastGlowColor)(eToastGlowColor.Blue), (eToastPosition)(eToastPosition.BottomCenter));
                MessageBox.Show("网络服务器连接故障！" +
                    "\n" +
                    "\n" +
                    "如果网络能正常访问，请联系管理员将以下站点加入受信任的站点：" +
                    "\n" +
                    "http://www.autojt.com" +
                    //"\n" +
                    //"http://www.autojt.com:81" +
                    //"\n" +
                    //"http://www.autojt.com:8002" +// /AutoJTService
                    " ", "网络故障提醒", MessageBoxButtons.OK, MessageBoxIcon.Error);
                {
                    //System.Environment.Exit(System.Environment.ExitCode);

                    isPing = false;
                    //this.Dispose();
                    //this.Close();
                }
            }

            return isPing;
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

        public static DataTable Calc_RPY2Matrix_Transform1(DataTable dataTable1, out string exInfos)
        {
            DataTable txTranslate1 = null;

            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                txTranslate1 = autoJTServiceClient.Calc_RPY2Matrix_Transform(dataTable1, out exInfos);
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

        #region 模块用量统计

        public static void ModelInfosSum(DataTable dataTable1, out string exInfos)
        {
            AutoJTServiceClient autoJTServiceClient = null;
            try
            {
                autoJTServiceClient = ServiceClientMethod();
                exInfos = autoJTServiceClient.Module_usage_statistics_21(dataTable1);
            }
            finally
            {
                if (autoJTServiceClient != null)
                    autoJTServiceClient.Close(); autoJTServiceClient = null;
            }
        }

        #endregion
    }

}