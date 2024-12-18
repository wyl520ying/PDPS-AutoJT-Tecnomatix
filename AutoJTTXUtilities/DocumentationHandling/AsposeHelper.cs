using Aspose.Cells;
using System;
using System.Data;

namespace AutoJTTXUtilities.DocumentationHandling
{
    public class AsposeHelper
    {
        #region 静态方法

        ////初始化 Aspose.Cells.dll
        //public static bool InitializeAsposeCellsDll(out string error)
        //{
        //    bool result = false;
        //    error = String.Empty;

        //    try
        //    {
        //        //dll所在路径
        //        string installDir = AJTTxApplicationUtilities.GetInstallationDirectory();

        //        //创建结果
        //        bool bl1 = false;

        //        //检查文件是否存在
        //        bool bl1_isfileExists = !File.Exists(Path.Combine(installDir, "Aspose.Cells.dll"));

        //        if (bl1_isfileExists)
        //        {
        //            bl1 = !string.IsNullOrEmpty(AJTFile.CreateFileFromEmbeddedResource(installDir, Assembly.GetExecutingAssembly(),
        //                                                                                    "AutoJTTecnomatix.EmbeddedResources.Aspose.Cells.dll", "Aspose.Cells.dll"));
        //        }

        //        //再次检查文件是否存在
        //        bl1_isfileExists = !File.Exists(Path.Combine(installDir, "Aspose.Cells.dll"));


        //        //文件已经存在
        //        if (!bl1_isfileExists)
        //        {
        //            return true;
        //        }

        //        //初始化失败
        //        if (!bl1)
        //        {
        //            error = string.Format("无法提取资源 {0}", "Aspose.Cells.dll");
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

        #region app

        //工作簿
        public Workbook m_workbook;
        //工作表
        public Worksheet m_worksheet;
        //单元格
        public Cells m_cells;


        #endregion

        public AsposeHelper()
        {
            try
            {
                Aspose.Hook.Manager.StartHook();
            }
            catch
            {

            }
        }

        //读取数据的ctor
        public AsposeHelper(string xlsFullname) : this()
        {
            //Tecnomatix.Engineering.TxMessageBox.ShowModal("1", "2", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.None);
            //创建工作簿对象,并使用其文件路径打开Excel文件
            this.m_workbook = new Workbook(xlsFullname);
            this.m_worksheet = this.m_workbook.Worksheets[0];
            this.m_cells = this.m_worksheet.Cells;
        }

        //写出数据的ctor
        public AsposeHelper(DataTable dt1, string filename) : this()
        {
            try
            {
                if (string.IsNullOrEmpty(filename) || dt1 == null || dt1.Rows.Count == 0)
                {
                    throw new ArgumentNullException("没有需要到出的数据");
                }

                using (this.m_workbook = new Workbook())
                {
                    using (this.m_worksheet = m_workbook.Worksheets[0])
                    {
                        using (this.m_cells = this.m_worksheet.Cells)
                        {
                            Style style;
                            //生成行列名行 
                            for (int i = 0; i < dt1.Columns.Count; i++)
                            {
                                this.m_cells[0, i].PutValue(dt1.Columns[i].ColumnName);
                                try
                                {
                                    style = this.m_cells[0, i].GetStyle();
                                    style.BackgroundColor = System.Drawing.Color.Blue;
                                    style.ForegroundColor = System.Drawing.Color.FromArgb(153, 204, 0);
                                    style.Pattern = BackgroundType.Solid;
                                    this.m_cells[0, i].SetStyle(style);
                                }
                                catch
                                {
                                    continue;
                                }
                            }

                            //生成数据行 
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                for (int k = 0; k < dt1.Columns.Count; k++)
                                {
                                    //添加数据
                                    this.m_cells[1 + i, k].PutValue(dt1.Rows[i][k].ToString());
                                }
                            }

                            this.m_worksheet.AutoFitColumns();
                            this.m_cells.SetRowHeight(0, 30);
                            this.m_worksheet.FreezePanes(1, 0, 1, 0);
                            this.m_workbook.Save(filename);
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        //获取Excel数据
        public void GetExcelData()
        {
            for (int i = 0; i < this.m_cells.MaxDataRow + 1; i++)
            {
                for (int j = 0; j < this.m_cells.MaxDataColumn + 1; j++)
                {
                    string s = this.m_cells[i, j].StringValue.Trim();
                    //一行行的读取数据，插入数据库的代码也可以在这里写

                }
            }
        }

        //返回DataTable数据
        public System.Data.DataTable GetDataTableData()
        {
            //System.Data.DataTable dataTable1 = cells.ExportDataTable(0, 0, cells.MaxDataRow + 1, cells.MaxColumn + 1);//没有标题
            //System.Data.DataTable dataTable2 = cells.ExportDataTable(0, 0, cells.MaxDataRow + 1, cells.MaxColumn + 1, true);//有标题

            System.Data.DataTable dataTable = null;

            dataTable = this.m_cells.ExportDataTableAsString(0, 0, this.m_cells.MaxDataRow + 1, this.m_cells.MaxColumn + 1, true);//有标题

            return dataTable;
        }

        //检查数据格式
        public bool CheckFormatOfSheet1()
        {
            bool bl1 = false;

            bl1 = this.m_cells[0, 0].StringValue.Trim() == "焊点号" && this.m_cells[0, 1].StringValue.Trim() == "板层数量";


            return bl1;
        }

        //检查数据格式
        public bool CheckFormatOfSheet2()
        {
            return true;
        }


        /// <summary>
        /// 销毁hook
        /// </summary>
        public void DestructionHook()
        {
            try
            {
                Aspose.Hook.Manager.StopHook();
            }
            catch
            {

            }

            try
            {
                this.m_cells.Dispose();
                this.m_cells = null;
            }
            catch
            {

            }

            try
            {
                this.m_worksheet.Dispose();
                this.m_worksheet = null;
            }
            catch
            {

            }

            try
            {
                this.m_workbook.Dispose();
                this.m_workbook = null;
            }
            catch
            {

            }
        }
    }
}