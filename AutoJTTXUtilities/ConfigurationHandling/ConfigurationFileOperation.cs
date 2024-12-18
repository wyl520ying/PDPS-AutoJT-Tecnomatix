using System.Collections.Generic;
using System.Configuration;

namespace AutoJTTXUtilities.ConfigurationHandling
{
    public struct TxConfig
    {
        public TxConfig(string _ikey, string _iValue)
        {
            this.iKey = _ikey;
            this.iValue = _iValue;
        }
        public string iKey;
        public string iValue;
    }
    public class ConfigurationFileOperation
    {
        protected string m_installPath;

        public ConfigurationFileOperation(string installPath)
        {
            if (string.IsNullOrEmpty(installPath))
            {
                this.m_installPath = null;
            }
            else
            {
                this.m_installPath = System.IO.Path.Combine(installPath, "AutoJTTX", "AutoJTTecnomatix.dll");
            }
        }

        //读取内置配置文件,Tune.exe.Config
        public string ReadConfiguration(string iKey)
        {
            //return System.Configuration.ConfigurationManager.AppSettings[iKey];//获取当前应用程序默认配置的AppSettingsSection数据。

            string result = null;

            try
            {
                Configuration txConfig = ConfigurationManager.OpenExeConfiguration(this.m_installPath);
                if (txConfig.HasFile)
                {
                    KeyValueConfigurationElement setting1 = txConfig.AppSettings.Settings[iKey];

                    if (setting1 != null)
                    {
                        result = setting1.Value;
                    }
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }

            return result;
        }

        //写出配置到Tune.exe.Config文件
        public bool WriteConfiguration(List<TxConfig> txConfigs)
        {
            bool flag1 = false;

            if (txConfigs == null || txConfigs.Count == 0)
            {
                return false;
            }

            try
            {
                //更新配置文件：
                Configuration txConfig = ConfigurationManager.OpenExeConfiguration(this.m_installPath);//ConfigurationUserLevel.None
                var txSettings = txConfig.AppSettings.Settings;

                foreach (TxConfig item in txConfigs)
                {
                    try
                    {
                        if (txSettings[item.iKey] == null)
                        {
                            txSettings.Add(item.iKey, item.iValue);
                        }
                        else
                        {
                            txSettings[item.iKey].Value = item.iValue;
                        }

                        flag1 = true;
                    }
                    catch
                    {
                        continue;
                    }
                }

                //保存配置
                txConfig.Save(ConfigurationSaveMode.Modified);
                //当前的配置文件更新成功。
                ConfigurationManager.RefreshSection("appSettings");// 刷新命名节，在下次检索它时将从磁盘重新读取它。记住应用程序要刷新节点

            }
            catch
            {
                flag1 = false;
            }

            return flag1;
        }
    }
    public class ConfigurationFileOperationAny : ConfigurationFileOperation
    {
        public ConfigurationFileOperationAny(string installPath) : base(installPath)
        {
            base.m_installPath = installPath;
        }
    }
}
