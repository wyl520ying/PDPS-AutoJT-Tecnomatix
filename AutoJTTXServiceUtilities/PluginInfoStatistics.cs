using System.Data;

namespace AutoJTTXServiceUtilities
{
    public class PluginInfoStatistics
    {
        //模块名
        string m_ModuleName;
        //用户名
        string m_userName;
        //msg
        string m_message;
        //时间("yyyy-MM-dd hh:mm:ss")
        string m_dateTime;
        //当前TX软件版本
        string m_Currversion;
        public PluginInfoStatistics(string moduleName, string msg, string _userName, string currentSoftVer, string versionInfos)
        {
            //模块
            this.m_ModuleName = moduleName;

            //用户名
            try
            {
                this.m_userName = (string.IsNullOrEmpty(_userName) || _userName.Equals("|")) ? System.Environment.UserName : _userName;
            }
            catch
            {
                this.m_userName = "用户名获取失败";
            }

            string new_msg = string.Empty;
            try
            {
                new_msg = msg.Replace("'", "_").Replace(",", "_").Replace(";", "_").Replace("\"", "_").Replace("/", "_").Replace("\\", "_").Replace(".", "_");
            }
            catch
            {
                new_msg = msg;
            }

            //message
            this.m_message = new_msg;

            //当前TX版本
            this.m_Currversion = currentSoftVer;

            //时间
            this.m_dateTime = versionInfos;//(2023年4月25日改为版本+到期日)
        }

        //新建一个datatable
        public DataTable CreateDatatable()
        {
            //实例化
            DataTable dt = new DataTable("modelInfos_1");

            //造一个列的数组用来 生成列的名字
            //  new DataColumn("Id"),  "Id"是给列起的Name属性，以后在前台绑定的时候就是绑定的这个名字
            DataColumn[] dc = new DataColumn[] {
                new DataColumn("MODULENAME",typeof(string)),
                new DataColumn("USERNAME",typeof(string)),
                new DataColumn("MESSAGE",typeof(string)),
                new DataColumn("DATETIME" ,typeof(string)),
                new DataColumn("ENGINEERINGSOFTINFOS" ,typeof(string))
            };

            dt.Columns.AddRange(dc);

            DataRow dataRow = dt.NewRow();

            try
            {
                dataRow[0] = this.m_ModuleName;
                dataRow[1] = this.m_userName;
                dataRow[2] = this.m_message;
                dataRow[3] = this.m_dateTime;
                dataRow[4] = string.IsNullOrEmpty(this.m_Currversion) ? "当前TX软件版本获取失败" : this.m_Currversion;
            }
            catch
            {
            }

            dt.Rows.Add(dataRow);

            return dt;
        }
    }
}
