using Microsoft.Win32;
using System;
using System.Collections.Generic;

namespace AutoJTTXCoreUtilities
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
    }
}
