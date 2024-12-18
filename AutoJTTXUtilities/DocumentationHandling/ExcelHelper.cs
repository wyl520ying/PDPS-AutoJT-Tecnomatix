using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AutoJTTXUtilities.DocumentationHandling
{
    public class ExcelHelper
    {
        /// <summary>
        /// 写入CSV文件
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="fileName">文件全名</param>
        /// <returns>是否写入成功</returns>
        public static string SaveCSV(System.Data.DataTable dt, string fileName)
        {
            FileStream fs = null;
            StreamWriter sw = null;
            string data = string.Empty;
            try
            {
                //判断文件是否存在,存在就不再次写入列名
                if (!File.Exists(fileName))
                {
                    fs = new FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                    sw = new StreamWriter(fs, Encoding.UTF8);

                    //写出列名称
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        data += dt.Columns[i].ColumnName.ToString();
                        if (i < dt.Columns.Count - 1)
                        {
                            data += ",";
                        }
                    }
                    sw.WriteLine(data);
                }
                else
                {
                    fs = new FileStream(fileName, FileMode.Append, FileAccess.Write);
                    sw = new StreamWriter(fs, Encoding.UTF8);
                }

                //写出各行数据
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    data = "";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        data += dt.Rows[i][j].ToString();
                        if (j < dt.Columns.Count - 1)
                        {
                            data += ",";
                        }
                    }
                    sw.WriteLine(data);
                }

                return "success";
            }
            catch (Exception ex)
            {
                return "export failed" + "\n" + ex.Message;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Dispose();
                    sw.Close();
                }
                if (fs != null)
                {
                    fs.Dispose();
                    fs.Close();
                }
            }
        }

        //打开另存为对话框
        public static string ShowSaveFileDialog(int filterIndex = 1)
        {
            string resultFile = string.Empty;

            try
            {
                System.Windows.Forms.SaveFileDialog saveFile = new System.Windows.Forms.SaveFileDialog();

                //SaveFile.Filter = "All files(*.*)|*.*|txt files(*.txt)|*.txt";//文本筛选,txt
                saveFile.Filter = "Excel 工作簿(*.xlsx)|*.xlsx|CSV 文件(*.csv)|*.csv";//文本筛选

                saveFile.FilterIndex = filterIndex;//文本筛选器索引，选择第一项就是1
                saveFile.RestoreDirectory = true;//还原目录
                saveFile.OverwritePrompt = true;//是否覆盖当前文件

                if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    resultFile = saveFile.FileName;
                }
            }
            catch
            {
                resultFile = string.Empty;
            }

            return resultFile;
        }

        //打开文件对话框
        public static string[] OpenFile()
        {
            string[] strFileName = null;

            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "CSV文件(*.csv)| *.csv";//"Excel文件(*.xls;*.xlsx)|*.xls;*.xlsx|word文档(*.doc;*.docx)|*.doc;*.docx|所有文件|*.*";
                ofd.ValidateNames = true; // 验证用户输入是否是一个有效的Windows文件名
                ofd.CheckFileExists = true; //验证路径的有效性
                ofd.CheckPathExists = true;//验证路径的有效性
                ofd.Multiselect = true;//是否允许多选，false表示单选

                if (ofd.ShowDialog() == DialogResult.OK) //用户点击确认按钮，发送确认消息
                {
                    strFileName = ofd.FileNames;// 获取对话框中所有选定文件的文件名（String 类型数组），为绝对路径，类似"E:\\code\\123.xml"               
                }
            }
            catch
            {
                return null;
            }

            return strFileName;
        }

        /// <summary>
        /// 读取CSV文件
        /// </summary>
        /// <param name="fileName">文件路径</param>
        public static System.Data.DataTable ReadCSV(string fileName, out string infos)
        {
            infos = string.Empty;
            System.Data.DataTable dt = null;
            FileStream fs = null;
            StreamReader sr = null;
            try
            {
                dt = new System.Data.DataTable();
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                sr = new StreamReader(fs, Encoding.UTF8);

                //记录每次读取的一行记录
                string strLine = null;
                //记录每行记录中的各字段内容
                string[] arrayLine = null;
                //分隔符
                string[] separators = { "," };
                //判断，若是第一次，建立表头
                bool isFirst = true;
                //判断，第一行是否是数据行
                bool isDataLine = false;

                //检查 1数据大于两列 2标题包含 "External ID" "name" "mfgLibrary"
                bool isDataFalmu = false;

                //逐行读取CSV文件
                while ((strLine = sr.ReadLine()) != null)
                {
                    strLine = strLine.Trim();//去除头尾空格
                    arrayLine = strLine.Split(separators, StringSplitOptions.None);//分隔字符串，返回数组

                    if (arrayLine.Length < 3)
                    {
                        infos = "格式错误";
                        return null;
                    }

                    if (!isDataFalmu && !
                        (
                        arrayLine[0].ToUpper() == "External ID".ToUpper() && arrayLine[1].ToUpper() == "name".ToUpper() && arrayLine[2].ToUpper() == "mfgLibrary".ToUpper()
                        )
                        )
                    {
                        infos = "格式错误";
                        return null;
                    }
                    isDataFalmu = true;

                    int dtColumns = 3;//= arrayLine.Length;//列的个数

                    if (isFirst)  //建立表头
                    {
                        isFirst = false;
                        isDataLine = true;
                        for (int i = 0; i < dtColumns; i++)
                        {
                            dt.Columns.Add(arrayLine[i]);//每一列名称
                        }
                    }
                    else   //表内容
                    {
                        DataRow dataRow = dt.NewRow();//新建一行
                        for (int j = 0; j < dtColumns; j++)
                        {
                            dataRow[j] = arrayLine[j];
                        }
                        dt.Rows.Add(dataRow);//添加一行
                    }
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Dispose();
                    sr.Close();
                }
                if (fs != null)
                {
                    fs.Dispose();
                    fs.Close();
                }
            }

            return dt;
        }

        //合并多个datatataoble
        public static System.Data.DataTable NewItemArray2Sum(System.Data.DataTable dt1, System.Data.DataTable dt2)
        {
            if (dt1 == null || dt1.Rows.Count == 0)
            {
                if (dt2 == null || dt2.Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    return dt2;
                }
            }
            else
            {
                if (dt2 == null || dt2.Rows.Count == 0)
                {
                    return dt1;
                }
                else
                {
                    System.Data.DataTable newtable = dt1.Copy();
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        newtable.Rows.Add(dt2.Rows[i].ItemArray);
                    }
                    return newtable;
                }
            }
        }


        //打开一个已存在的excel文件
        public static Workbook OpenExcelFile(string filePath, out Microsoft.Office.Interop.Excel.Application excelApp)
        {
            excelApp = null;

            try
            {
                // 创建 Excel 应用程序的实例
                excelApp = new Microsoft.Office.Interop.Excel.Application();

                // 使 Excel 应用程序可见（可以根据需要设置为 false）
                excelApp.Visible = true;

                // 打开 Excel 文件
                Workbook workbook = excelApp.Workbooks.Open(filePath);
                return workbook;
            }
            catch (Exception ex)
            {
                // 确保在发生异常时释放已创建的 Excel 应用程序实例
                if (excelApp != null)
                {
                    excelApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                    excelApp = null;
                }

                // 处理异常，例如文件路径无效、Excel 未安装等
                //Console.WriteLine($"Error: {ex.Message}");
                throw ex;

                //return null;
            }            
        }
        //关闭打开的excel,并释放内存
        public static void CloseExcelFile(Workbook workbook, Microsoft.Office.Interop.Excel.Application excelApp, bool saveChanges = false)
        {
            if (workbook != null)
            {
                workbook.Close(saveChanges);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            }

            if (excelApp != null)
            {
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }

            // 强制垃圾回收
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        //打开一个 Excel 文件并且不打算对其进行进一步操
        public static void OpenExcelFile(string filePath)
        {
            Microsoft.Office.Interop.Excel.Application excelApp = null;

            try
            {
                // 创建 Excel 应用程序的实例
                excelApp = AJTExcel.OpenExcel(true, out string error);

                // 使 Excel 应用程序可见
                excelApp.Visible = true;

                // 打开 Excel 文件
                excelApp.Workbooks.Open(filePath);
            }
            catch (Exception ex)
            {
                // 处理异常，例如文件路径无效、Excel 未安装等
                //Console.WriteLine($"Error: {ex.Message}");

                // 使用默认的文件关联程序打开文件（通常是Excel）
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });

            }
            finally
            {
                // 如果不再需要操作 Excel 文件，可以选择不关闭或释放 Excel 应用程序
                // excelApp.Quit();
                // System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                // excelApp = null;
            }
        }
    }

    /// <summary>
    /// Microsoft.Office.Interop.Excel 操作基础类
    /// </summary>
    public class Office_Excel_Interop_Helper
    {
        #region excel 对象 Application，Workbook，Worksheet和Range                       

        //当前打开的excel app
        public _Application _ExcelApp = null;
        //当前活动的excel文档
        public Workbook _WorkBook = null;
        //得到当前active Sheet
        public Worksheet _ActiveXlsSheet = null;

        #endregion

        public Office_Excel_Interop_Helper()
        {
            #region 检查excel

            try
            {
                object o = System.Runtime.InteropServices.Marshal.GetActiveObject("EXCEL.Application");
                _ExcelApp = o as _Application;

                if (_ExcelApp == null)
                {
                    throw new Exception("请打开一个Excel");
                    //MessageBox.Show("请打开一个Excel");
                    //return;
                }

                //得到当前活动的excel文档
                _WorkBook = _ExcelApp.ActiveWorkbook;

                if (_WorkBook == null)
                {
                    //内存中的execl进程,没有workbook
                    AJTExcel.CloseExcel(_ExcelApp);
                    AJTExcel.KillExcelInstanceById(ref _ExcelApp);
                    AJTExcel.NAR(_ExcelApp);

                    throw new Exception("请打开一个Workbook");
                    //MessageBox.Show("请打开一个Workbook");
                    //return;
                }

                //得到当前active Sheet
                _ActiveXlsSheet = (Worksheet)_WorkBook.ActiveSheet;

                if (_ActiveXlsSheet == null)
                {
                    throw new Exception("请打开一个Sheet");
                    //MessageBox.Show("请打开一个Sheet");
                    //return;
                }
            }
            catch
            {
                throw new Exception("请打开一个Excel");
            }

            #endregion
        }

        /*
         *  #region excel 对象 Application，Workbook，Worksheet和Range            

            Office_Excel_Interop_Helper _office_Excel_Interop_Helper = new Office_Excel_Interop_Helper();

            //当前打开的excel app
            _Application excelApp = _office_Excel_Interop_Helper._ExcelApp;
            //当前活动的excel文档
            Workbook workBook = _office_Excel_Interop_Helper._WorkBook;
            //得到当前active Sheet
            Worksheet activeXlsSheet = _office_Excel_Interop_Helper._ActiveXlsSheet;

            #endregion
        */

        /// <summary>
        /// 检查Application准备就绪
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void CheckInEditMode()
        {
            //CommandBar commandBar1 = _ExcelApp.CommandBars["Fill Color"];
            //return commandBar1 != null && commandBar1.Enabled;

            object m = Type.Missing;
            const int MENU_ITEM_TYPE = 1;
            const int NEW_MENU = 18;

            CommandBarControl oNewMenu =
              this._ExcelApp.CommandBars["Worksheet Menu Bar"].FindControl(
                      MENU_ITEM_TYPE, //the type of item to look for
                      NEW_MENU, //the item to look for
                      m, //the tag property (in this case missing)
                      m, //the visible property (in this case missing)
                      true); //we want to look for it recursively
                             //so the last argument should be true.

            if (oNewMenu != null)
            {
                if (!oNewMenu.Enabled)
                {
                    throw new Exception("Excel is in Edit Mode");
                }
            }
        }
    }

    public class AJTExcel
    {
        public class Cell
        {
            public Cell(int row, int column)
            {
                this.Column = column;
                this.Row = row;
            }

            public readonly int Column;

            public readonly int Row;
        }


        //用于打开excel后继续操作excel
        public Microsoft.Office.Interop.Excel.Application m_Excel;
        public Microsoft.Office.Interop.Excel.Workbook m_Workbook { get; set; }



        #region 结束Excel进程

        [DllImport("user32.DLL")]
        public static extern IntPtr GetWindowThreadProcessId(int hWnd, ref IntPtr lpdwProcessID);

        public static void KillExcelInstanceById(ref Microsoft.Office.Interop.Excel.Application myExcelApp)
        {
            try
            {
                IntPtr intPtr = (IntPtr)0;
                AJTExcel.GetWindowThreadProcessId(myExcelApp.Hwnd, ref intPtr);
                System.Diagnostics.Process.GetProcessById(intPtr.ToInt32()).Kill();
            }
            catch
            {
            }
        }
        public static void KillExcelInstanceById(ref _Application myExcelApp)
        {
            try
            {
                IntPtr intPtr = (IntPtr)0;
                AJTExcel.GetWindowThreadProcessId(myExcelApp.Hwnd, ref intPtr);
                System.Diagnostics.Process.GetProcessById(intPtr.ToInt32()).Kill();
            }
            catch
            {
            }
        }

        public static void NAR(object o)
        {
            try
            {
                Marshal.ReleaseComObject(o);
            }
            catch
            {
            }
            finally
            {
                o = null;
            }
        }

        public static void CloseExcel(Microsoft.Office.Interop.Excel.Application _excel2)
        {
            try
            {
                Thread.Sleep(500);
                if (_excel2.Workbooks != null)
                {
                    _excel2.Workbooks.Close();
                }
                Thread.Sleep(500);
                if (_excel2 != null)
                {
                    _excel2.Quit();
                    //System.Runtime.InteropServices.Marshal.ReleaseComObject(_excel2);
                    _excel2 = null;
                }
            }
            catch
            {
            }
        }
        public static void CloseExcel(_Application _excel2)
        {
            try
            {
                Thread.Sleep(500);
                if (_excel2.Workbooks != null)
                {
                    _excel2.Workbooks.Close();
                }
                Thread.Sleep(500);
                if (_excel2 != null)
                {
                    _excel2.Quit();
                    //System.Runtime.InteropServices.Marshal.ReleaseComObject(_excel2);
                    _excel2 = null;
                }
            }
            catch
            {
            }
        }
        public static void SaveWorkbookAndCloseExcel(Microsoft.Office.Interop.Excel.Application excelApp, Microsoft.Office.Interop.Excel.Workbook workbook, string excelFile)
        {
            try
            {
                if (workbook != null)
                {
                    workbook.Save();
                    workbook.Close(true, excelFile, Type.Missing);
                    AJTExcel.NAR(workbook);
                    workbook = null;
                }
                if (excelApp != null)
                {
                    excelApp.DisplayAlerts = true;
                    excelApp.Quit();
                    AJTExcel.KillExcelInstanceById(ref excelApp);
                    AJTExcel.NAR(excelApp);
                    excelApp = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch
            {
            }
        }

        public static void DonotSaveWorkbookAndCloseExcel(Microsoft.Office.Interop.Excel.Application excelApp, Microsoft.Office.Interop.Excel.Workbook workbook,bool isEixtXlsAPP = true)
        {
            try
            {
                if (workbook != null)
                {
                    try
                    {
                        workbook.Close(true, Type.Missing);
                    }
                    catch
                    {
                    }

                    AJTExcel.NAR(workbook);
                    workbook = null;
                }
                if (isEixtXlsAPP && excelApp != null)
                {
                    try
                    {
                        excelApp.DisplayAlerts = true;
                        excelApp.Quit();
                    }
                    catch
                    {
                    }
                  
                    AJTExcel.KillExcelInstanceById(ref excelApp);
                    AJTExcel.NAR(excelApp);
                    excelApp = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch
            {
            }
        }

        #endregion

        #region 打开Excel

        //初始化一个exce, 如果没有打开Excel 实例，将会抛出错误。新建一个excel实例
        public static Microsoft.Office.Interop.Excel.Application OpenExcel(bool visible, out string error)
        {
            error = "";
            Microsoft.Office.Interop.Excel.Application result = null;

            try
            {
                result = (Microsoft.Office.Interop.Excel.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");
            }
            catch 
            {
                try
                {
                    result = (Microsoft.Office.Interop.Excel.Application)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")));
                    result.Visible = visible;
                }
                catch (Exception ex)
                {
                    error = string.Format("无法打开Excel应用程序错误 {0}", ex.Message);
                    result = null;
                }
            }          
            
            return result;
        }
        public static Workbook OpenWorkbook(Microsoft.Office.Interop.Excel.Application excelApp, string excelFile, out string error)
        {
            error = "";
            if (excelFile != null && excelFile.Length != 0 && excelApp != null)
            {
                Workbook result;
                try
                {
                    result = excelApp.Workbooks.Open(excelFile, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                }
                catch (Exception ex)
                {
                    error = string.Format("无法打开 Excel 文档错误 {0} {1}", excelFile, ex.Message);
                    result = null;
                }
                return result;
            }
            return null;
        }
        public static Microsoft.Office.Interop.Excel.Application OpenExcel(string xlsFiles, out Workbook _workbook, out Worksheet _worksheet_first)
        {
            //app
            Microsoft.Office.Interop.Excel.Application _excel2 = null;
            //workbook
            _workbook = null;
            //第一个sheet
            _worksheet_first = null;

            #region 打开excel进程

            Microsoft.Office.Interop.Excel.Application application = (Microsoft.Office.Interop.Excel.Application)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")));
            application.Visible = false;
            application.DisplayAlerts = false;

            _excel2 = application;
            string directoryName = Path.GetDirectoryName(xlsFiles);

            FileInfo fileInfo = new FileInfo(xlsFiles);

            _workbook = _excel2.Workbooks.Open(fileInfo.FullName, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

            if (_workbook == null)
            {
                return null;
            }

            #endregion

            #region 检查sheet

            //取第一个sheet
            _worksheet_first = _workbook.Sheets[1] as Worksheet;
            if (_worksheet_first == null)
            {
                return null;
            }

            #endregion

            return _excel2;
        }

        #endregion

        public bool OpenExcelAndLoadTemplate(string outputDir, string outputFilename, string EXCEL_TEMPLATE_PATH, Assembly ass, out string error, bool visible = true)
        {
            this.m_Excel = null;
            this.m_Workbook = null;

            this.m_Excel = OpenExcel(visible, out error);

            this.m_Excel.DisplayAlerts = false;

            if (this.m_Excel == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(outputDir))
            {
                outputDir = Directory.GetCurrentDirectory();
            }
            try
            {
                //todo,如果文件已经存在则不释放,直接打开, 防止文件已经打开的bug
                if (!InitAutoJTTXUpdateHandlerEXE.InitEXE(out string errorinfo, outputDir, EXCEL_TEMPLATE_PATH, ass, outputFilename,false))
                {
                    error = string.Format("无法提取资源 {0}", EXCEL_TEMPLATE_PATH);
                    return false;
                }
            }
            catch (UnauthorizedAccessException)
            {
                error = string.Format("您没有足够的写入权限或文件是只读的 {0}", Path.Combine(outputDir, outputFilename));
                return false;
            }
            catch (IOException)
            {
                error = string.Format("该文件可能正在使用中 {0}", Path.Combine(outputDir, outputFilename));
                return false;
            }


            //检查workbook是否已经打开, 如果没有打开则新打开一个, 如果已经打开则前台显示
            this.m_Workbook = this.m_Excel.Workbooks.Cast<Workbook>().FirstOrDefault(x => x.Name == "焊点板层信息.xlsx");
            bool wbOpened = this.m_Workbook != null;
            //已经打开了
            if (wbOpened) 
            {
                this.m_Excel.WindowState = XlWindowState.xlMinimized;
                Thread.Sleep(500);
                this.m_Excel.WindowState = XlWindowState.xlNormal;
            }
            else
            {
                this.m_Workbook = OpenWorkbook(this.m_Excel, Path.Combine(outputDir, outputFilename), out error);
            }

            return this.m_Workbook != null;
        }
    }
}