using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using System;
using System.Runtime.InteropServices;

namespace AutoJTTXUtilities.DocumentationHandling
{
    public class AJTPowerPoint
    {
        /// <summary>
        /// 新建ppt应用程序
        /// </summary>
        /// <param name="visible"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static Application OpenPowerPoint(bool visible, out string error)
        {
            error = "";
            Application result = null;
            try
            {
                Application obj = (Application)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("91493441-5A91-11CF-8700-00AA0060263B")));
                obj.Visible = (visible ? MsoTriState.msoTrue : MsoTriState.msoFalse);
                result = obj;
            }
            catch (Exception ex)
            {
                error = string.Format("无法打开PowerPoint应用程序错误 {0}", ex.Message);
                result = null;
            }
            return result;
        }

        /// <summary>
        /// 打开一个ppt文件
        /// </summary>
        /// <param name="PowerPointApp"></param>
        /// <param name="PowerPointFile"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static Presentation OpenPresentation(Application PowerPointApp, string PowerPointFile, out string error)
        {
            error = "";
            if (PowerPointFile != null && PowerPointFile.Length != 0 && PowerPointApp != null)
            {
                Presentation result = null;
                try
                {
                    result = PowerPointApp.Presentations.Open(PowerPointFile, MsoTriState.msoFalse, MsoTriState.msoFalse, MsoTriState.msoTrue);
                }
                catch (Exception ex)
                {
                    error = string.Format("无法打开PowerPoint文档错误 {0} {1}", PowerPointFile, ex.Message);
                    result = null;
                }
                return result;
            }
            return null;
        }

        /// <summary>
        /// 关闭所有的ppt文件
        /// </summary>
        /// <param name="PowerPointApp"></param>
        public static void ClosePowerPoint(Application PowerPointApp)
        {
            try
            {
                if (PowerPointApp.Presentations != null)
                {
                    foreach (Presentation obj in PowerPointApp.Presentations)
                    {
                        obj.Close();
                    }
                }
                if (PowerPointApp != null)
                {
                    PowerPointApp.Quit();
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 新建ppt并保存
        /// </summary>
        /// <param name="PowerPointApp"></param>
        /// <param name="PowerPointFile"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static Presentation NewPresentation(Application PowerPointApp, string PowerPointFile, out string error)
        {
            error = "";
            if (PowerPointFile != null && PowerPointFile.Length != 0 && PowerPointApp != null)
            {
                Presentation result;
                try
                {
                    Presentation obj = PowerPointApp.Presentations.Add(MsoTriState.msoTrue);
                    obj.SaveAs(PowerPointFile, PpSaveAsFileType.ppSaveAsDefault, MsoTriState.msoTriStateMixed);
                    result = obj;
                }
                catch (Exception ex)
                {
                    error = string.Format("无法打开新的POWER POINT演示文稿错误 {0} {1}", PowerPointFile, ex.Message);
                    result = null;
                }
                return result;
            }
            return null;
        }
    }
}
