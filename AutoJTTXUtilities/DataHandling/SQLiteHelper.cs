//using AutoJTTXCoreUtilities;
using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace AutoJTTXUtilities.DataHandling
{
    public class SQLiteHelper
    {
        #region 静态方法

        //初始化System.Data.SQLite //SQLite.Interop.dll
        //public static bool InitializeSQLiteInterop(out string error)
        //{
        //    bool result = false;
        //    error = String.Empty;

        //    try
        //    {
        //        //dll所在路径
        //        string installDir = AJTTxApplicationUtilities.GetInstallationDirectory();

        //        //创建结果
        //        bool bl1 = false;
        //        bool bl2 = false;
        //        bool bl3 = false;

        //        //检查文件是否存在
        //        bool bl1_isfileExists = !File.Exists(Path.Combine(installDir, "System.Data.SQLite.dll"));
        //        bool bl2_isfileExists = !File.Exists(Path.Combine(installDir, "x86\\SQLite.Interop.dll"));
        //        bool bl3_isfileExists = !File.Exists(Path.Combine(installDir, "x64\\SQLite.Interop.dll"));

        //        if (bl1_isfileExists)
        //        {
        //            bl1 = !string.IsNullOrEmpty(AJTFile.CreateFileFromEmbeddedResource(installDir, Assembly.GetExecutingAssembly(),
        //                                                                                    "AutoJTTecnomatix.EmbeddedResources.System.Data.SQLite.dll", "System.Data.SQLite.dll"));
        //        }
        //        if (bl2_isfileExists)
        //        {
        //            bl2 = !string.IsNullOrEmpty(AJTFile.CreateFileFromEmbeddedResource(Path.Combine(installDir, "x86"), Assembly.GetExecutingAssembly(),
        //                                                                                    "AutoJTTecnomatix.EmbeddedResources.x86.SQLite.Interop.dll", "SQLite.Interop.dll"));
        //        }
        //        if (bl3_isfileExists)
        //        {
        //            bl3 = !string.IsNullOrEmpty(AJTFile.CreateFileFromEmbeddedResource(Path.Combine(installDir, "x64"), Assembly.GetExecutingAssembly(),
        //                                                                                    "AutoJTTecnomatix.EmbeddedResources.x64.SQLite.Interop.dll", "SQLite.Interop.dll"));
        //        }

        //        //再次检查文件是否存在
        //        bl1_isfileExists = !File.Exists(Path.Combine(installDir, "System.Data.SQLite.dll"));
        //        bl2_isfileExists = !File.Exists(Path.Combine(installDir, "x86\\SQLite.Interop.dll"));
        //        bl3_isfileExists = !File.Exists(Path.Combine(installDir, "x64\\SQLite.Interop.dll"));

        //        //文件已经存在
        //        if (!bl1_isfileExists && !bl2_isfileExists && !bl3_isfileExists)
        //        {
        //            return true;
        //        }

        //        //初始化失败
        //        if (!bl1 || !bl2 || !bl3)
        //        {
        //            error = string.Format("无法提取资源 {0}", "System.Data.SQLite");
        //            return false;
        //        }
        //        else
        //        {
        //            result = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        error = ex.Message;
        //        return false;
        //    }

        //    return result;
        //}

        #endregion

        ////db所在路径
        private string m_DB_Directory;
        ////db文件full name
        private string m_DB_FileFullName;

        //创建表的sql "CREATE TABLE IF NOT EXISTS Data (Id INTEGER PRIMARY KEY AUTOINCREMENT, Column1 TEXT, Column2 TEXT)"
        private string m_createTbl_Sql;

        //数据库连接
        //private SQLiteConnection m_db_Connection;
        //数据库Command
        //private SQLiteCommand m_command;
        //数据库Reader
        //private SQLiteDataReader m_reader;


        //当前数据库操作的表名
        public string m_tableName { get; set; }

        //@"URI=file:" + this.m_DB_FileFullName
        string ConnectionString = string.Empty;

        /// <summary>
        /// SQLiteHelper 辅助类
        /// </summary>
        /// <param name="sysRootPath">system root</param>
        /// <param name="db_dirPath">db文件所在的文件夹</param>
        public SQLiteHelper(string sysRootPath, string db_dirPath = "Data")
        {
            //db所在路径
            this.m_DB_Directory = Path.Combine(sysRootPath, db_dirPath);
            //db文件full name
            this.m_DB_FileFullName = (this.m_DB_Directory.EndsWith("\\") ? this.m_DB_Directory : this.m_DB_Directory + "\\") + "WELDPOINTPLATEDATA.db";

            this.m_tableName = "WeldPointPlateInfos";

            this.ConnectionString = @"URI=file:" + this.m_DB_FileFullName;

            //检查db所在的文件夹和.db文件是否存在, 不存在则创建
            this.CheckDBFile_Initialize();
        }

        /// <summary>
        /// SQLiteHelper 辅助类2
        /// </summary>
        /// <param name="dbdir">db所在文件夹</param>
        /// <param name="dbFileName">db文件名</param>
        /// <param name="tbname">数据库表名</param>
        private SQLiteHelper(string dbdir, string dbFileName, string tbname,string createTblSQLString = null)
        {
            //db所在路径
            this.m_DB_Directory = dbdir;
            //db文件full name
            this.m_DB_FileFullName = (this.m_DB_Directory.EndsWith("\\") ? this.m_DB_Directory : this.m_DB_Directory + "\\") + dbFileName;
            //表名
            this.m_tableName = tbname;

            //链接字符串
            this.ConnectionString = @"URI=file:" + this.m_DB_FileFullName;

            //创建表的sql
            this.m_createTbl_Sql = createTblSQLString;

            //检查db所在的文件夹和.db文件是否存在, 不存在则创建
            this.CheckDBFile_Initialize();
        }

        #region 单例模式

        //实例 AutoJTTecnomatix_MCM.db
        private static SQLiteHelper mInstance = null;

        public static SQLiteHelper GetInstance()
        {
            if (mInstance == null)
            {
                mInstance = new SQLiteHelper(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AutoJTTecnomatix"), "AutoJTTecnomatix_MCM.db", "tx_project_mcm");
            }

            return mInstance;
        }

        #endregion
        
        #region 用于机器人清单管理的sqlite单例模式

        private static SQLiteHelper m_SQLiteHelper = null;
        public static SQLiteHelper GetInstanceRoboMgr(string sysPath)
        {
            if (m_SQLiteHelper == null)
            {                
                m_SQLiteHelper = new SQLiteHelper(sysPath, "ROBOLISTMANAGER.db", "RoboMgr", "CREATE TABLE IF NOT EXISTS Data (Id INTEGER PRIMARY KEY AUTOINCREMENT, Column1 TEXT, Column2 TEXT)");
            }

            return m_SQLiteHelper;
        }

        #endregion

        #region db Initialize

        //检查db所在的文件夹和.db文件是否存在, 不存在则创建
        private void CheckDBFile_Initialize()
        {
            try
            {
                //检查db所在的文件夹
                if (!Directory.Exists(this.m_DB_Directory))
                {
                    Directory.CreateDirectory(this.m_DB_Directory);
                }

                //检查.db文件是否存在
                if (File.Exists(this.m_DB_FileFullName))
                {
                    //创建一个连接到指定数据库，打开数据库
                    this.connectToDatabase();
                }
                //不存在数据文件 //创建该文件
                else
                {
                    //创建一个空的数据库,empty
                    this.createNewDatabase();
                    //创建一个连接到指定数据库，打开数据库  
                    this.connectToDatabase();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        #endregion

        #region 创建空表 连接打开 关闭 删除  表是否存在

        /// <summary>
        /// 创建一个空的数据库
        /// </summary>
        public void createNewDatabase()
        {
            SQLiteConnection.CreateFile(this.m_DB_FileFullName);
        }

        /// <summary>
        /// 创建一个连接到指定数据库
        /// </summary>
        public void connectToDatabase()
        {
            //初始化连接指定的连接字符串
            //this.m_db_Connection = new SQLiteConnection(this.ConnectionString);//("Data Source=" + this.m_DB_FileFullName);// + "; Password=autojt");
            //this.m_db_Connection.Open();
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void CloseConnection()
        {
            /*
            //销毁Command
            if (this.m_command != null)
            {
                try
                {
                    this.m_command.Cancel();
                }
                catch { }
            }
            this.m_command = null;

            //销毁Reader
            if (this.m_reader != null)
            {
                this.m_reader.Close();
            }
            this.m_reader = null;

            //销毁Connection
            if (this.m_db_Connection != null)
            {
                this.m_db_Connection.Close();
            }
            this.m_db_Connection = null;
            */
        }

        /// <summary>
        /// 删除所有数据
        /// </summary>
        public int DeleteFromTable()
        {
            int result = 0;


            //DELETE FROM m_tableName;
            string sql = "DELETE FROM " + this.m_tableName;
            /*m_command = new SQLiteCommand(sql, m_db_Connection);
            m_reader = m_command.ExecuteReader();

            //接收受影响的行数
            result = m_reader.RecordsAffected;
            */

            using (SQLiteConnection c = new SQLiteConnection(this.ConnectionString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
                {
                    using (SQLiteDataReader rdr = cmd.ExecuteReader())
                    {
                        //接收受影响的行数
                        result = rdr.RecordsAffected;
                    }
                }
            }

            return result;
        }

        //删除一条数据
        public int DeleteOneItem(string uuid)
        {
            int result = 0;


            //"DELETE FROM "+this.m_tableName+" WHERE uuid = '"+uuid+"'";
            string sql = "DELETE FROM " + this.m_tableName + " WHERE uuid = '" + uuid + "'";
            /*m_command = new SQLiteCommand(sql, m_db_Connection);
            m_reader = m_command.ExecuteReader();

            //接收受影响的行数
            result = m_reader.RecordsAffected;
            */

            using (SQLiteConnection c = new SQLiteConnection(this.ConnectionString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
                {
                    using (SQLiteDataReader rdr = cmd.ExecuteReader())
                    {
                        //接收受影响的行数
                        result = rdr.RecordsAffected;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 判断表是否存在
        /// </summary>
        /// <returns></returns>
        public bool tableExists()
        {
            try
            {
                //SELECT count(*) FROM sqlite_master WHERE type='table' AND name='tableName';
                string sql = "SELECT count(*) FROM " + this.m_tableName;
                //m_command = new SQLiteCommand(sql, m_db_Connection);

                //m_reader = m_command.ExecuteReader();



                using (SQLiteConnection c = new SQLiteConnection(ConnectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
                    {
                        using (SQLiteDataReader rdr = cmd.ExecuteReader())
                        {

                        }
                    }
                }

            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion

        #region 创建表 添加数据

        /// <summary>
        /// //在指定数据库中创建一个table 名 "WeldPointPlateInfos"
        /// </summary>
        public void createTable()
        {
            //创建一个表
            string[] colNames = new string[] { "WELDPOINTNAME", "WELDPOINTTYPE" };
            string[] colTypes = new string[] { "TEXT", "INTEGER" };
            string queryString = "CREATE TABLE IF NOT EXISTS " + this.m_tableName + "( " + colNames[0] + " " + colTypes[0];

            for (int i = 1; i < colNames.Length; i++)
            {
                queryString += ", " + colNames[i] + " " + colTypes[i];
            }
            queryString += ", CONSTRAINT WELDPOINTNAME_pk PRIMARY KEY (WELDPOINTNAME) ";
            queryString += "  ) ";

            /*
            SQLiteCommand dbCommand = this.m_db_Connection.CreateCommand();
            dbCommand.CommandText = queryString;

            dbCommand.ExecuteNonQuery();
            */

            using (SQLiteConnection c = new SQLiteConnection(ConnectionString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(queryString, c))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void createTable_MCM()
        {
            //创建一个表
            string[] colNames = new string[] { "uuid", "project", "model", "region", "creator", "datatime", "type" };
            string[] colTypes = new string[] { "char(100)", "char(100)", "char(100)", "char(100)", "char(100)", "char(100)", "char(10)" };

            StringBuilder queryString = new StringBuilder();

            queryString.Append("CREATE TABLE IF NOT EXISTS ");
            queryString.Append(this.m_tableName);
            queryString.Append("( ");
            queryString.Append(colNames[0]);
            queryString.Append(" ");
            queryString.Append(colTypes[0]);

            for (int i = 1; i < colNames.Length; i++)
            {
                queryString.Append(", ");
                queryString.Append(colNames[i]);
                queryString.Append(" ");
                queryString.Append(colTypes[i]);
            }
            queryString.Append(", CONSTRAINT project_pk PRIMARY KEY (uuid) ");
            queryString.Append("  ) ");

            /*
            SQLiteCommand dbCommand = this.m_db_Connection.CreateCommand();
            dbCommand.CommandText = queryString;

            dbCommand.ExecuteNonQuery();
            */

            using (SQLiteConnection c = new SQLiteConnection(ConnectionString))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(queryString.ToString(), c))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 一次查询数据批量导入
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dt1"></param>
        /// <param name="currentTime"></param>
        /// <param name="iCount"></param>
        /// <param name="muf"></param>
        public void InserPatch(DataTable dt1, out int iCount)
        {
            iCount = 0;

            if (dt1 == null || dt1.Rows.Count == 0)
            {
                return;
            }

            using (SQLiteConnection c = new SQLiteConnection(ConnectionString))
            {
                c.Open();
                using (SQLiteCommand insertRngCmd = c.CreateCommand())
                {
                    insertRngCmd.CommandText = @"INSERT OR REPLACE INTO " + this.m_tableName + " VALUES (@WELDPOINTNAME, @WELDPOINTTYPE)";// ON DUPLICATE KEY UPDATE Name1=@Name1, Name2=@Name2, Face=@Face, type=@type";
                    var transaction = c.BeginTransaction();

                    foreach (DataRow itemRow in dt1.Rows)
                    {
                        insertRngCmd.Parameters.AddWithValue("@WELDPOINTNAME", itemRow[0].ToString());
                        insertRngCmd.Parameters.AddWithValue("@WELDPOINTTYPE", itemRow[1].ToString());

                        iCount = iCount + insertRngCmd.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
        }

        /// <summary>
        /// 一次查询数据批量导入
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dt1"></param>
        /// <param name="currentTime"></param>
        /// <param name="iCount"></param>
        /// <param name="muf"></param>
        public void InserPatch_mcm(DataTable dt1, out int iCount, int itype)
        {
            iCount = 0;

            if (dt1 == null || dt1.Rows.Count == 0)
            {
                return;
            }

            using (SQLiteConnection c = new SQLiteConnection(ConnectionString))
            {
                c.Open();
                using (SQLiteCommand insertRngCmd = c.CreateCommand())
                {
                    insertRngCmd.CommandText = @"INSERT OR REPLACE INTO " + this.m_tableName + " VALUES (@uuid, @project, @model, @region, @creator, @datatime, @type)";// ON DUPLICATE KEY UPDATE Name1=@Name1, Name2=@Name2, Face=@Face, type=@type";
                    var transaction = c.BeginTransaction();

                    string strTime = System.DateTime.Now.ToString();
                    foreach (DataRow itemRow in dt1.Rows)
                    {
                        try
                        {
                            insertRngCmd.Parameters.AddWithValue("@uuid", itemRow[1]);
                            insertRngCmd.Parameters.AddWithValue("@project", itemRow[2]);
                            insertRngCmd.Parameters.AddWithValue("@model", itemRow[3]);
                            insertRngCmd.Parameters.AddWithValue("@region", itemRow[4]);
                            insertRngCmd.Parameters.AddWithValue("@creator", itemRow[5]);
                            insertRngCmd.Parameters.AddWithValue("@datatime", strTime);
                            insertRngCmd.Parameters.AddWithValue("@type", itype);

                            iCount = iCount + insertRngCmd.ExecuteNonQuery();
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    transaction.Commit();
                }
            }
        }
        /// <summary>
        /// 插入一行数据
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="iCount"></param>
        public void InserLineData(string uuid, string project, string model, string region, string creator, out int iCount, int itype)
        {
            iCount = 0;

            if (string.IsNullOrEmpty(uuid) || string.IsNullOrEmpty(project) || string.IsNullOrEmpty(model) || string.IsNullOrEmpty(region) || string.IsNullOrEmpty(creator))
            {
                return;
            }

            using (SQLiteConnection c = new SQLiteConnection(ConnectionString))
            {
                c.Open();
                using (SQLiteCommand insertRngCmd = c.CreateCommand())
                {
                    insertRngCmd.CommandText = @"INSERT OR REPLACE INTO " + this.m_tableName + " VALUES (@uuid, @project, @model, @region, @creator, @datatime, @type)";
                    var transaction = c.BeginTransaction();

                    insertRngCmd.Parameters.AddWithValue("@uuid", uuid);
                    insertRngCmd.Parameters.AddWithValue("@project", project);
                    insertRngCmd.Parameters.AddWithValue("@model", model);
                    insertRngCmd.Parameters.AddWithValue("@region", region);
                    insertRngCmd.Parameters.AddWithValue("@creator", creator);
                    insertRngCmd.Parameters.AddWithValue("@datatime", System.DateTime.Now.ToString());
                    insertRngCmd.Parameters.AddWithValue("@type", itype);

                    iCount = iCount + insertRngCmd.ExecuteNonQuery();

                    transaction.Commit();
                }
            }
        }

        #endregion

        #region 查询

        /// <summary>
        /// 查询指定表中的行数
        /// </summary>
        /// <returns></returns>
        public DataTable QueryRowsInTable()
        {
            DataTable dt1 = new DataTable(this.m_tableName);
            try
            {
                string sql = "SELECT *, PROJECT||' | '||MODEL||' | '||REGION||' | '||UUID FROM " + this.m_tableName;
                /*SQLiteDataAdapter SQLiteDataAdapter1 = new SQLiteDataAdapter(sql, this.m_db_Connection);
                SQLiteDataAdapter1.Fill(dt1);

                SQLiteDataAdapter1.Dispose();//释放资源  

                */
                using (SQLiteConnection c = new SQLiteConnection(this.ConnectionString))
                {
                    c.Open();
                    using (SQLiteDataAdapter SQLiteDataAdapter1 = new SQLiteDataAdapter(sql, c))
                    {
                        SQLiteDataAdapter1.Fill(dt1);

                        SQLiteDataAdapter1.Dispose();//释放资源
                    }
                }
            }
            catch
            {
                return dt1;
            }

            return dt1;
        }

        /// <summary>
        /// 查询指定表中的行数
        /// </summary>
        /// <returns></returns>
        public DataTable QueryRowsInTable2()
        {
            DataTable dt1 = new DataTable(this.m_tableName);
            try
            {
                string sql = "SELECT * FROM " + this.m_tableName;
                /*SQLiteDataAdapter SQLiteDataAdapter1 = new SQLiteDataAdapter(sql, this.m_db_Connection);
                SQLiteDataAdapter1.Fill(dt1);

                SQLiteDataAdapter1.Dispose();//释放资源  

                */
                using (SQLiteConnection c = new SQLiteConnection(this.ConnectionString))
                {
                    c.Open();
                    using (SQLiteDataAdapter SQLiteDataAdapter1 = new SQLiteDataAdapter(sql, c))
                    {
                        SQLiteDataAdapter1.Fill(dt1);

                        SQLiteDataAdapter1.Dispose();//释放资源
                    }
                }
            }
            catch
            {
                return dt1;
            }

            return dt1;
        }

        /// <summary>
        /// 使用sql查询语句，并显示结果
        /// </summary>
        /// <returns></returns>
        public int QueryTbBystring(string _index)
        {
            //string sql = "select * from users order by score desc";
            //select * from users WHERE id = '1905229312D2X5YW';
            //string sql = "select * from users WHERE id = '" + GlobalClass.user.strUsrId + "'";

            ////desc 是降序//asc 升序
            string sql = "select * from " + this.m_tableName + " order by " + _index + " asc";

            int result = -1;
            /*
            m_command = new SQLiteCommand(sql, m_db_Connection);
            m_reader = m_command.ExecuteReader();

            while (m_reader.Read())
            {
                result = m_reader.GetInt32(1);
            }
            */
            return result;
        }

        /// <summary>
        /// 使用sql查询,返回datatable
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="desc1"></param>
        /// <returns></returns>
        /*public DataTable QueryData4TableName(string desc1)
        {
            connectToDatabase();//连接数据库
            DataTable dt1 = new DataTable(this.m_tableName);
            try
            {
                //string sql = "select * from users order by score desc";
                //select * from users WHERE id = '1905229312D2X5YW';
                //string sql = "select * from users WHERE id = '" + GlobalClass.user.strUsrId + "'";

                //desc降序，asc升序    
                string sql = "select * from " + this.m_tableName + " order by " + desc1 + " asc";
                SQLiteDataAdapter SQLiteDataAdapter1 = new SQLiteDataAdapter(sql, this.m_db_Connection);
                SQLiteDataAdapter1.Fill(dt1);

                SQLiteDataAdapter1.Dispose();//释放资源                
            }
            catch
            {
                return null;
            }

            return dt1;
        }*/

        #endregion

        #region 公共的查询方法

        //创建表
        public void CreateTable(string sql = null)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                if (string.IsNullOrEmpty(this.m_createTbl_Sql))
                {
                    throw new ArgumentNullException(nameof(this.m_createTbl_Sql));
                }
                sql = this.m_createTbl_Sql;
            }
            using (var connection = new SQLiteConnection(this.ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        #endregion
    }
}