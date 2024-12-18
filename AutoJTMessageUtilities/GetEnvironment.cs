using System;
using System.Management;

namespace AutoJTMessageUtilities
{
    public class GetEnvironment
    {
        /// <summary>
        /// 获取计算机基本信息
        /// </summary>
        /// <param name="osVersionName">操作系统</param>
        /// <param name="machineName">机器名</param>
        /// <param name="userName">用户名</param>
        /// <param name="domainName">域名称</param>
        /// <param name="processorCounter">CPU数</param>
        /// <param name="SystemType">系统类型</param>
        /// <param name="iWorkSet">内存</param>
        public static bool GetEnv(out string osVersionName, out string machineName, out string userName, out string domainName,
                            out string processorCounter, out string SystemType, out string iWorkSet)
        {
            bool result = false;

            osVersionName = string.Empty;
            machineName = string.Empty;
            userName = string.Empty;
            domainName = string.Empty;
            processorCounter = string.Empty;
            SystemType = string.Empty;
            iWorkSet = string.Empty;

            try
            {
                osVersionName = GetOsVersion(Environment.OSVersion.Version);
                string servicePack = Environment.OSVersion.ServicePack;
                osVersionName = osVersionName + " " + servicePack;//操作系统

                machineName = Environment.MachineName;//机器名
                userName = Environment.UserName;//用户名
                domainName = Environment.UserDomainName;//域名称
                processorCounter = Environment.ProcessorCount.ToString();//CPU数

                bool is64Os = Environment.Is64BitOperatingSystem;
                SystemType = is64Os ? "64bit" : "32bit";//系统类型

                iWorkSet = GetPhisicalMemory().ToString() + "MB";//内存
                result = true;
            }
            catch (Exception ex)
            {
                iWorkSet = $"{ex.Message},获取设备码失败";
            }
            return result;
        }
        private static string GetOsVersion(Version ver)
        {
            string strClient = "";
            if (ver.Major == 5 && ver.Minor == 1)
            {
                strClient = "Win XP";
            }
            else if (ver.Major == 6 && ver.Minor == 0)
            {
                strClient = "Win Vista";
            }
            else if (ver.Major == 6 && ver.Minor == 1)
            {
                strClient = "Win 7";
            }
            else if (ver.Major == 6 && ver.Minor == 2)
            {
                strClient = "Win 10";
            }
            else if (ver.Major == 5 && ver.Minor == 0)
            {
                strClient = "Win 2000";
            }
            else if (ver.Major == 10 && ver.Minor == 0)
            {
                strClient = "Win 10";
            }
            else
            {
                strClient = "unknown";
            }
            return strClient;
        }
        private static int GetPhisicalMemory()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher();   //用于查询一些如系统信息的管理对象 
            searcher.Query = new SelectQuery("Win32_PhysicalMemory ", "", new string[] { "Capacity" });//设置查询条件 
            ManagementObjectCollection collection = searcher.Get();   //获取内存容量 
            ManagementObjectCollection.ManagementObjectEnumerator em = collection.GetEnumerator();

            long capacity = 0;
            while (em.MoveNext())
            {
                ManagementBaseObject baseObj = em.Current;
                if (baseObj.Properties["Capacity"].Value != null)
                {
                    try
                    {
                        capacity += long.Parse(baseObj.Properties["Capacity"].Value.ToString());
                    }
                    catch
                    {
                        return 0;
                    }
                }
            }
            return (int)(capacity / 1024 / 1024);
        }

        #region 机器码

        /// <summary>
        /// 取得设备硬盘的卷标号
        /// </summary>
        /// <returns></returns>
        public static string GetDiskVolumeSerialNumber()
        {
            //ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
            disk.Get();
            return disk.GetPropertyValue("VolumeSerialNumber").ToString();
        }

        /// <summary>
        /// 获得CPU的序列号
        /// </summary>
        /// <returns></returns>
        public static string getCpu()
        {
            string strCpu = null;
            ManagementClass myCpu = new ManagementClass("win32_Processor");
            ManagementObjectCollection myCpuConnection = myCpu.GetInstances();
            foreach (ManagementObject myObject in myCpuConnection)
            {
                strCpu = myObject.Properties["Processorid"].Value.ToString();
                break;
            }
            return strCpu;
        }


        /// <summary>
        /// 生成机器码
        /// </summary>
        /// <returns></returns>
        public static string getMNum()
        {
            //string strNum = getCpu() + GetDiskVolumeSerialNumber();//获得24位Cpu和硬盘序列号
            //string strMNum = strNum.Substring(0, 24);//从生成的字符串中取出前24个字符做为机器码
            //return strMNum;

            if (string.IsNullOrEmpty(m_newIdCodeSuffix))
            {
                m_newIdCodeSuffix = Guid.NewGuid().ToString("N");
            }
            return m_newIdCodeSuffix;
        }
        public int[] intCode = new int[127];//存储密钥
        public int[] intNumber = new int[25];//存机器码的Ascii值
        public char[] Charcode = new char[25];//存储机器码字
        public void setIntCode()//给数组赋值小于10的数
        {
            for (int i = 1; i < intCode.Length; i++)
            {
                intCode[i] = i % 9;
            }
        }




        //注册表中设置的机器码
        static string m_newIdCodeSuffix;
        public static string CheckJsonCode()
        {
            ////解析json
            //string exCode = DeserializeExtraCode();

            ////解析失败
            //if (string.IsNullOrEmpty(exCode))
            //{
            //    m_newIdCodeSuffix = SerializeExtraCode();
            //}
            //else
            //{
            //    m_newIdCodeSuffix = exCode;
            //}


            //改成注册表方式
            string exCode = WindowPositionHelper.GetNewIdCode();
            //没有设置
            if (string.IsNullOrEmpty(exCode))
            {
                m_newIdCodeSuffix = WindowPositionHelper.SaveNewIdCode(Guid.NewGuid().ToString("N"));
            }
            else
            {
                m_newIdCodeSuffix = exCode;
            }

            return m_newIdCodeSuffix;
        }

        #endregion

    }
}
