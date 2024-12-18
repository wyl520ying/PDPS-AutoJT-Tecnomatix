





using System;
using System.IO;
using System.Reflection;


namespace AutoJTTXUtilities.DocumentationHandling
{
  public class InitAutoJTTXUpdateHandlerEXE
  {
    public static bool InitEXE(
      out string error,
      string installDir,
      string manifestResource,
      Assembly assembly = null,
      string EXENameContains_extension = "AutoJTTXUpdateHandler.exe",
      bool isReRelease = true)
    {
      error = string.Empty;
      bool flag1;
      try
      {
        if (assembly == (Assembly) null)
          assembly = Assembly.GetExecutingAssembly();
        bool flag2 = false;
        bool flag3 = !File.Exists(Path.Combine(installDir, EXENameContains_extension));
        if (!flag3)
        {
          try
          {
            if (isReRelease)
              File.Delete(Path.Combine(installDir, EXENameContains_extension));
          }
          catch
          {
          }
          if (!File.Exists(Path.Combine(installDir, EXENameContains_extension)))
            flag2 = !string.IsNullOrEmpty(AJTFile.CreateFileFromEmbeddedResource(installDir, assembly, manifestResource, EXENameContains_extension));
        }
        if (flag3)
          flag2 = !string.IsNullOrEmpty(AJTFile.CreateFileFromEmbeddedResource(installDir, assembly, manifestResource, EXENameContains_extension));
        if (File.Exists(Path.Combine(installDir, EXENameContains_extension)))
          return true;
        if (!flag2)
        {
          error = string.Format("无法提取资源 {0}", (object) EXENameContains_extension);
          return false;
        }
        flag1 = true;
      }
      catch (Exception ex)
      {
        error = string.Format("无法提取资源 {0} {1}", (object) EXENameContains_extension, (object) ex.Message);
        return false;
      }
      return flag1;
    }
  }
}
