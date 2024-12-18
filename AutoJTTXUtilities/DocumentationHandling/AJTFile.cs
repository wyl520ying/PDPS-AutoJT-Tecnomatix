using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace AutoJTTXUtilities.DocumentationHandling
{
    public class AJTFile
    {
        internal static string CreateFileFromEmbeddedResource(string folderOut, Assembly assembly, string manifestResource, string fileNameOut)
        {
            if (!string.IsNullOrEmpty(folderOut) && !(assembly == null) && !string.IsNullOrEmpty(manifestResource) && !string.IsNullOrEmpty(fileNameOut))
            {
                if (!Directory.Exists(folderOut))
                {
                    try
                    {
                        Directory.CreateDirectory(folderOut);
                    }
                    catch
                    {
                        return null;
                    }
                }
                Stream stream = null;
                string result;
                try
                {
                    stream = assembly.GetManifestResourceStream(manifestResource);

                }
                catch
                {
                    result = null;
                    return result;
                }

                if (stream == null)
                {
                    return null;
                }
                string text = Path.Combine(folderOut, fileNameOut);
                if (File.Exists(text))
                {
                    File.Delete(text);
                }
                using (Stream stream2 = File.OpenWrite(text))
                {
                    AJTFile.CopyStream(stream, stream2);
                    stream.Close();
                    stream2.Close();
                }
                return text;
            }
            return null;
        }

        internal static void CopyStream(Stream input, Stream output)
        {
            byte[] array = new byte[8192];
            int count;
            while ((count = input.Read(array, 0, array.Length)) > 0)
            {
                output.Write(array, 0, count);
            }
        }


        /// <summary>
        /// 复制文本到剪切板
        /// </summary>
        public static Task<bool> SetClipboard2(string text)
        {
            return Task.Run(() =>
            {
                bool bl898896532 = false;

                try
                {
                    Thread th = new Thread(new ThreadStart(delegate ()
                    {
                        try
                        {
                            System.Windows.Clipboard.Clear();//清除原有剪切板中内容
                            System.Windows.Clipboard.SetText(text);

                            Thread.Sleep(800);

                            bl898896532 = true;
                        }
                        catch
                        {
                        }
                    }));
                    th.TrySetApartmentState(ApartmentState.STA);
                    th.Start();
                    th.Join();
                }
                catch
                {
                    bl898896532 = false;
                }

                return bl898896532;
            });
        }

        public static bool IsFileOpen(string path)
        {
            FileStream stream = null;
            try
            {
                stream = File.Open(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            }
            catch (IOException ex)
            {
                if (ex.Message.Contains("being used by another process"))
                    return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return false;
        }
    }

    public class InitAutoJTTXUpdateHandlerEXE
    {
        #region 静态方法

        //初始化 AutoJTTXUpdateHandler.exe
        public static bool InitEXE(out string error, string installDir, string manifestResource,
            Assembly assembly = null, string EXENameContains_extension = "AutoJTTXUpdateHandler.exe", bool isReRelease = true)
        {
            bool result = false;
            error = string.Empty;

            try
            {
                if (assembly == null)
                {
                    assembly = Assembly.GetExecutingAssembly();
                }

                //创建结果
                bool bl1 = false;

                //检查文件是否存在
                bool bl1_isfileExists = !File.Exists(Path.Combine(installDir, EXENameContains_extension));

                if (!bl1_isfileExists)
                {
                    try
                    {
                        if (isReRelease)
                            File.Delete(Path.Combine(installDir, EXENameContains_extension));
                    }
                    catch
                    {
                    }

                    //再次检查文件是否存在
                    bool bl99 = !File.Exists(Path.Combine(installDir, EXENameContains_extension));
                    if (bl99)
                    {
                        bl1 = !string.IsNullOrEmpty(AJTFile.CreateFileFromEmbeddedResource(installDir, assembly,
                                                                                    manifestResource, EXENameContains_extension));
                    }
                }

                if (bl1_isfileExists)
                {
                    bl1 = !string.IsNullOrEmpty(AJTFile.CreateFileFromEmbeddedResource(installDir, assembly,
                                                                                            manifestResource, EXENameContains_extension));
                }

                //再次检查文件是否存在
                bl1_isfileExists = !File.Exists(Path.Combine(installDir, EXENameContains_extension));


                //文件已经存在
                if (!bl1_isfileExists)
                {
                    return true;
                }

                //初始化失败
                if (!bl1)
                {
                    error = string.Format("无法提取资源 {0}", EXENameContains_extension);
                    return false;
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                error = string.Format("无法提取资源 {0} {1}", EXENameContains_extension, ex.Message);
                return false;
            }

            return result;
        }

        #endregion
    }
}