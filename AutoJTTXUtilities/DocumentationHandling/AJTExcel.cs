





using Aspose.Cells;
using Microsoft.Office.Interop.Excel;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Workbook = Microsoft.Office.Interop.Excel.Workbook;
using Worksheet = Microsoft.Office.Interop.Excel.Worksheet;


namespace AutoJTTXUtilities.DocumentationHandling
{
  public class AJTExcel
  {
    public Application m_Excel;

    public Microsoft.Office.Interop.Excel.Workbook m_Workbook { get; set; }

    [DllImport("user32.DLL")]
    public static extern IntPtr GetWindowThreadProcessId(int hWnd, ref IntPtr lpdwProcessID);

    public static void KillExcelInstanceById(ref Application myExcelApp)
    {
      try
      {
        IntPtr lpdwProcessID = (IntPtr) 0;
        AJTExcel.GetWindowThreadProcessId(myExcelApp.Hwnd, ref lpdwProcessID);
        Process.GetProcessById(lpdwProcessID.ToInt32()).Kill();
      }
      catch
      {
      }
    }

    public static void KillExcelInstanceById(ref _Application myExcelApp)
    {
      try
      {
        IntPtr lpdwProcessID = (IntPtr) 0;
        AJTExcel.GetWindowThreadProcessId(myExcelApp.Hwnd, ref lpdwProcessID);
        Process.GetProcessById(lpdwProcessID.ToInt32()).Kill();
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
        o = (object) null;
      }
    }

    public static void CloseExcel(Application _excel2)
    {
      try
      {
        Thread.Sleep(500);
        if (_excel2.Workbooks != null)
        {
          // ISSUE: reference to a compiler-generated method
          _excel2.Workbooks.Close();
        }
        Thread.Sleep(500);
        if (_excel2 == null)
          return;
        // ISSUE: reference to a compiler-generated method
        _excel2.Quit();
        _excel2 = (Application) null;
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
          // ISSUE: reference to a compiler-generated method
          _excel2.Workbooks.Close();
        }
        Thread.Sleep(500);
        if (_excel2 == null)
          return;
        // ISSUE: reference to a compiler-generated method
        _excel2.Quit();
        _excel2 = (_Application) null;
      }
      catch
      {
      }
    }

    public static void SaveWorkbookAndCloseExcel(
      Application excelApp,
      Workbook workbook,
      string excelFile)
    {
      try
      {
        if (workbook != null)
        {
          // ISSUE: reference to a compiler-generated method
          workbook.Save();
          // ISSUE: reference to a compiler-generated method
          workbook.Close((object) true, (object) excelFile, Type.Missing);
          AJTExcel.NAR((object) workbook);
          workbook = (Workbook) null;
        }
        if (excelApp != null)
        {
          excelApp.DisplayAlerts = true;
          // ISSUE: reference to a compiler-generated method
          excelApp.Quit();
          AJTExcel.KillExcelInstanceById(ref excelApp);
          AJTExcel.NAR((object) excelApp);
          excelApp = (Application) null;
        }
        GC.Collect();
        GC.WaitForPendingFinalizers();
      }
      catch
      {
      }
    }

    public static void DonotSaveWorkbookAndCloseExcel(
      Application excelApp,
      Workbook workbook,
      bool isEixtXlsAPP = true)
    {
      try
      {
        if (workbook != null)
        {
          try
          {
            // ISSUE: reference to a compiler-generated method
            workbook.Close((object) true, Type.Missing, Type.Missing);
          }
          catch
          {
          }
          AJTExcel.NAR((object) workbook);
          workbook = (Workbook) null;
        }
        if (isEixtXlsAPP && excelApp != null)
        {
          try
          {
            excelApp.DisplayAlerts = true;
            // ISSUE: reference to a compiler-generated method
            excelApp.Quit();
          }
          catch
          {
          }
          AJTExcel.KillExcelInstanceById(ref excelApp);
          AJTExcel.NAR((object) excelApp);
          excelApp = (Application) null;
        }
        GC.Collect();
        GC.WaitForPendingFinalizers();
      }
      catch
      {
      }
    }

    public static Application OpenExcel(bool visible, out string error)
    {
      error = "";
      // ISSUE: variable of a compiler-generated type
      Application application1;
      try
      {
        application1 = (Application) Marshal.GetActiveObject("Excel.Application");
      }
      catch
      {
        try
        {
          application1 = (Application) Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")));
          application1.Visible = visible;
        }
        catch (Exception ex)
        {
          error = string.Format("无法打开Excel应用程序错误 {0}", (object) ex.Message);
          application1 = (Application) null;
        }
      }
      // ISSUE: variable of a compiler-generated type
      Application application2 = application1;
      return application2;
    }

    public static Workbook OpenWorkbook(Application excelApp, string excelFile, out string error)
    {
      error = "";
      if (excelFile != null && excelFile.Length != 0 && excelApp != null)
      {
        // ISSUE: variable of a compiler-generated type
        Workbook workbook1;
        try
        {
          // ISSUE: reference to a compiler-generated method
          workbook1 = excelApp.Workbooks.Open(excelFile, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        }
        catch (Exception ex)
        {
          error = string.Format("无法打开 Excel 文档错误 {0} {1}", (object) excelFile, (object) ex.Message);
          workbook1 = (Workbook) null;
        }
        // ISSUE: variable of a compiler-generated type
        Workbook workbook2 = workbook1;
        return workbook2;
      }
      // ISSUE: variable of a compiler-generated type
      Workbook workbook = (Workbook) null;
      return workbook;
    }

    public static Application OpenExcel(
      string xlsFiles,
      out Workbook _workbook,
      out Worksheet _worksheet_first)
    {
      _workbook = (Workbook) null;
      _worksheet_first = (Worksheet) null;
      // ISSUE: variable of a compiler-generated type
      Application instance = (Application) Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")));
      instance.Visible = false;
      instance.DisplayAlerts = false;
      // ISSUE: variable of a compiler-generated type
      Application application1 = instance;
      Path.GetDirectoryName(xlsFiles);
      FileInfo fileInfo = new FileInfo(xlsFiles);
      // ISSUE: reference to a compiler-generated method
      _workbook = application1.Workbooks.Open(fileInfo.FullName, (object) Missing.Value, (object) Missing.Value, (object) Missing.Value, (object) Missing.Value, (object) Missing.Value, (object) Missing.Value, (object) Missing.Value, (object) Missing.Value, (object) Missing.Value, (object) Missing.Value, (object) Missing.Value, (object) Missing.Value, (object) Missing.Value, (object) Missing.Value);
      if (_workbook == null)
      {
        // ISSUE: variable of a compiler-generated type
        Application application2 = (Application) null;
        return application2;
      }
      _worksheet_first = _workbook.Sheets[(object) 1] as Worksheet;
      if (_worksheet_first == null)
      {
        // ISSUE: variable of a compiler-generated type
        Application application3 = (Application) null;
        return application3;
      }
      // ISSUE: variable of a compiler-generated type
      Application application4 = application1;
      return application4;
    }

    public bool OpenExcelAndLoadTemplate(
      string outputDir,
      string outputFilename,
      string EXCEL_TEMPLATE_PATH,
      Assembly ass,
      out string error,
      bool visible = true)
    {
      this.m_Excel = (Application) null;
      this.m_Workbook = (Workbook) null;
      this.m_Excel = AJTExcel.OpenExcel(visible, out error);
      this.m_Excel.DisplayAlerts = false;
      if (this.m_Excel == null)
        return false;
      if (string.IsNullOrEmpty(outputDir))
        outputDir = Directory.GetCurrentDirectory();
      try
      {
        if (!InitAutoJTTXUpdateHandlerEXE.InitEXE(out string _, outputDir, EXCEL_TEMPLATE_PATH, ass, outputFilename, false))
        {
          error = string.Format("无法提取资源 {0}", (object) EXCEL_TEMPLATE_PATH);
          return false;
        }
      }
      catch (UnauthorizedAccessException ex)
      {
        error = string.Format("您没有足够的写入权限或文件是只读的 {0}", (object) Path.Combine(outputDir, outputFilename));
        return false;
      }
      catch (IOException ex)
      {
        error = string.Format("该文件可能正在使用中 {0}", (object) Path.Combine(outputDir, outputFilename));
        return false;
      }
      this.m_Workbook = this.m_Excel.Workbooks.Cast<Workbook>().FirstOrDefault<Workbook>((Func<Workbook, bool>) (x => x.Name == "焊点板层信息.xlsx"));
      if (this.m_Workbook != null)
      {
        this.m_Excel.WindowState = XlWindowState.xlMinimized;
        Thread.Sleep(500);
        this.m_Excel.WindowState = XlWindowState.xlNormal;
      }
      else
        this.m_Workbook = AJTExcel.OpenWorkbook(this.m_Excel, Path.Combine(outputDir, outputFilename), out error);
      return this.m_Workbook != null;
    }

    public class Cell
    {
      public readonly int Column;
      public readonly int Row;

      public Cell(int row, int column)
      {
        this.Column = column;
        this.Row = row;
      }
    }
  }
}
