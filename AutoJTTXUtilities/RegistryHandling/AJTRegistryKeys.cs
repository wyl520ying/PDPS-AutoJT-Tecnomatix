using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AutoJTTXUtilities.RegistryHandling
{
    public class AJTRegistryKeys
    {
        public static string[] GetSubKeys(RegistryKey registry, string startKey)
        {
            string[] result = null;
            using (RegistryKey registryKey = registry.OpenSubKey(startKey))
            {
                if (registryKey != null)
                {
                    result = registryKey.GetSubKeyNames();
                    registryKey.Close();
                }
            }
            return result;
        }

        public static Dictionary<string, object> GetSubValues(RegistryKey registry, string startKey)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            using (RegistryKey registryKey = registry.OpenSubKey(startKey))
            {
                if (registryKey != null)
                {
                    string[] valueNames = registryKey.GetValueNames();
                    foreach (string text in valueNames)
                    {
                        object value = registryKey.GetValue(text);
                        dictionary.Add(text, value);
                    }
                    registryKey.Close();
                }
            }
            return dictionary;
        }

        public static string GetJavaInstallationPath()
        {
            try
            {
                string environmentPath = Environment.GetEnvironmentVariable("JAVA_HOME");
                if (!string.IsNullOrEmpty(environmentPath))
                {
                    return environmentPath;
                }

                string javaKey = "SOFTWARE\\JavaSoft\\Java Runtime Environment\\";
                using (Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(javaKey))
                {
                    string currentVersion = rk.GetValue("CurrentVersion").ToString();
                    using (Microsoft.Win32.RegistryKey key = rk.OpenSubKey(currentVersion))
                    {
                        return key.GetValue("JavaHome").ToString();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //获取catia安装的版本和路径
        public static Task<Dictionary<string, string>> GetInstallPath(bool isReg64 = false)
        {
            Dictionary<string, string> VerPairs = new Dictionary<string, string>();

            try
            {
                //view类型
                RegistryView regView = RegistryView.Default;
                if (isReg64)
                {
                    regView = RegistryView.Registry64;
                }

                using (RegistryKey localMacchine_RootNode = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, regView))
                {
                    using (RegistryKey Software_Key = localMacchine_RootNode.OpenSubKey("SOFTWARE", false))
                    {
                        using (RegistryKey tecnomatixRoot_Key = Software_Key.OpenSubKey("Dassault Systemes", false))
                        {
                            if (tecnomatixRoot_Key == null)
                            {
                                //进入64位视图
                                GetInstallPath(true);
                            }
                            else
                            {
                                foreach (string item in tecnomatixRoot_Key.GetSubKeyNames())
                                {
                                    try
                                    {
                                        //筛选版本
                                        if (item.StartsWith("B"))
                                        {
                                            using (RegistryKey registrySUb = tecnomatixRoot_Key.OpenSubKey(item, false))
                                            {
                                                if (registrySUb != null)
                                                {
                                                    foreach (var item23 in registrySUb.GetSubKeyNames())
                                                    {
                                                        using (RegistryKey registryVerSub = registrySUb.OpenSubKey(item23, false))
                                                        {
                                                            if (registryVerSub is null)
                                                            {
                                                                continue;
                                                            }
                                                            string[] keys = registryVerSub.GetValueNames();


                                                            //安装路径
                                                            string install = string.Empty;
                                                            //版本
                                                            string vername = string.Empty;

                                                            //安装路径 // DEST_FOLDER_OSDS
                                                            if (keys.Contains("DEST_FOLDER_OSDS"))
                                                            {
                                                                string strIns = System.IO.Path.Combine(registryVerSub.GetValue("DEST_FOLDER_OSDS").ToString(), @"code\bin");

                                                                //检查cnext是否存在
                                                                if (!System.IO.File.Exists(Path.Combine(strIns, "CNEXT.exe")) && !System.IO.File.Exists(Path.Combine(strIns, "DELMIA.exe")))
                                                                {
                                                                    install = null;
                                                                }
                                                                else
                                                                {
                                                                    install = System.IO.Directory.Exists(strIns) ? strIns : null;
                                                                }
                                                            }

                                                            //版本 ypeInstall
                                                            if (keys.Contains("TypeInstall"))
                                                            {
                                                                vername = registryVerSub.GetValue("TypeInstall").ToString();
                                                            }
                                                            else
                                                            {
                                                                vername = item;
                                                            }

                                                            if (!string.IsNullOrEmpty(install) && !string.IsNullOrEmpty(vername))
                                                            {
                                                                string binExeFilePath_cat = Path.Combine(install, "CNEXT.exe");
                                                                string binExeFilePath_del = Path.Combine(install, "DELMIA.exe");

                                                                if (System.IO.File.Exists(binExeFilePath_cat))
                                                                {
                                                                    if (!VerPairs.Values.Contains(binExeFilePath_cat))
                                                                    {
                                                                        if (VerPairs.Keys.Contains(vername))
                                                                        {
                                                                            VerPairs.Add($"{vername}_1", binExeFilePath_cat);
                                                                        }
                                                                        else
                                                                        {
                                                                            VerPairs.Add(vername, binExeFilePath_cat);
                                                                        }
                                                                    }
                                                                }
                                                                if (System.IO.File.Exists(binExeFilePath_del))
                                                                {
                                                                    if (!VerPairs.Values.Contains(binExeFilePath_del))
                                                                    {
                                                                        if (VerPairs.Keys.Contains($"{vername}_Delmia"))
                                                                        {
                                                                            VerPairs.Add($"{vername}_Delmia_1", binExeFilePath_del);
                                                                        }
                                                                        else
                                                                        {
                                                                            VerPairs.Add($"{vername}_Delmia", binExeFilePath_del);
                                                                        }
                                                                    }
                                                                  
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return Task.FromResult(VerPairs);
        }
    }
}