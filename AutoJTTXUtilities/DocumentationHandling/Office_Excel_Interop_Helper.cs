





using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


namespace AutoJTTXUtilities.DocumentationHandling
{
  public class Office_Excel_Interop_Helper
  {
    public _Application _ExcelApp = (_Application) null;
    public Workbook _WorkBook = (Workbook) null;
    public Worksheet _ActiveXlsSheet = (Worksheet) null;

    public Office_Excel_Interop_Helper()
    {
      try
      {
        this._ExcelApp = Marshal.GetActiveObject("EXCEL.Application") as _Application;
        this._WorkBook = this._ExcelApp != null ? this._ExcelApp.ActiveWorkbook : throw new Exception("请打开一个Excel");
        if (this._WorkBook == null)
        {
          AJTExcel.CloseExcel(this._ExcelApp);
          AJTExcel.KillExcelInstanceById(ref this._ExcelApp);
          AJTExcel.NAR((object) this._ExcelApp);
          throw new Exception("请打开一个Workbook");
        }
        //if (Office_Excel_Interop_Helper.\u003C\u003Eo__3.\u003C\u003Ep__0 == null)
        {
         // Office_Excel_Interop_Helper.\u003C\u003Eo__3.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, Worksheet>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (Worksheet), typeof (Office_Excel_Interop_Helper)));
        }
        //this._ActiveXlsSheet = Office_Excel_Interop_Helper.\u003C\u003Eo__3.\u003C\u003Ep__0.Target((CallSite) Office_Excel_Interop_Helper.\u003C\u003Eo__3.\u003C\u003Ep__0, this._WorkBook.ActiveSheet);
        if (this._ActiveXlsSheet == null)
          throw new Exception("请打开一个Sheet");
      }
      catch
      {
        throw new Exception("请打开一个Excel");
      }
    }

    public void CheckInEditMode()
    {
      object missing = Type.Missing;
      // ISSUE: reference to a compiler-generated method
      // ISSUE: variable of a compiler-generated type
      CommandBarControl control = this._ExcelApp.CommandBars[(object) "Worksheet Menu Bar"].FindControl((object) 1, (object) 18, missing, missing, (object) true);
      if (control != null && !control.Enabled)
        throw new Exception("Excel is in Edit Mode");
    }
  }
}
