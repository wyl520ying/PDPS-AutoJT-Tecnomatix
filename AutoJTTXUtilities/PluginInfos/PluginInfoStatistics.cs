using System;
using System.Data;

namespace AutoJTTXUtilities.PluginInfos
{
    internal class PluginInfoStatistics
    {
        //模块名
        //internal enum ModuleName
        //{
        //    None,
        //    自动标注,

        //1    导插枪,
        //2    导焊点清单,
        //3    快速选择,
        //4    新JT导入,
        //5    焊点查重查漏,
        //6    焊点参照分配,
        //7    焊点截图

        //8    重命名
        //9    cojt重命名
        //10    机构
        //11    焊点一键可达
        //12    创建进出枪点
        //13    Resource一键坐标回原点
        //14    修改olp command
        //}


        //模块名
        string m_ModuleName;
        //用户名
        string m_userName;
        //msg
        string m_message;
        //时间("yyyy-MM-dd hh:mm:ss")
        string m_dateTime;

        public PluginInfoStatistics(string moduleName, string msg)
        {
            this.m_ModuleName = moduleName;
            this.m_userName = GlobalClass.user.strUsrName;

            string new_msg = string.Empty;
            try
            {
                new_msg = msg.Replace("'", "_").Replace(",", "_").Replace(";", "_").Replace("\"", "_").Replace("/", "_").Replace("\\", "_").Replace(".", "_");
            }
            catch
            {
                new_msg = msg;
            }

            this.m_message = new_msg;
            this.m_dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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

            dataRow[0] = this.m_ModuleName;
            dataRow[1] = this.m_userName;
            dataRow[2] = this.m_message;
            dataRow[3] = this.m_dateTime;
            dataRow[4] = GlobalClass.SoftWareHostVersion;

            dt.Rows.Add(dataRow);

            return dt;
        }
    }
}