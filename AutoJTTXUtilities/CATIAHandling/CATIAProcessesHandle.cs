using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace AutoJTTXUtilities.CATIAHandling
{
    public class CATIAProcessesHandle
    {
        //获取所有打开的catia进程的path
        public static Task<Dictionary<string, string>> GetCATIAProcessMethod()
        {
            Dictionary<string, string> keyValuePairs = null;

            Process[] myProcesses = Process.GetProcesses();//获取当前进程数组

            List<Process> catiaProcess = myProcesses.Where(a => a.ProcessName.ToUpper() == "CNEXT" || a.ProcessName.ToUpper() == "DELMIA").ToList();

            keyValuePairs = GetFilePathMethod(catiaProcess);

            return Task.FromResult(keyValuePairs);
        }

        //获取任意进程的路径
        public static Dictionary<string, string> GetFilePathMethod(List<Process> catiaProcess)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            string wmiQueryString = "SELECT ProcessId, ExecutablePath, CommandLine FROM Win32_Process";
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmiQueryString))

            using (ManagementObjectCollection results = searcher.Get())
            {
                var query = from p in catiaProcess
                            join mo in results.Cast<ManagementObject>()
                            on p.Id equals (int)(uint)mo["ProcessId"]
                            select new
                            {
                                Process = p.ProcessName,
                                Path = (string)mo["ExecutablePath"],
                                //CommandLine = (string)mo["CommandLine"],
                            };

                foreach (var item in query)
                {
                    // Do what you want with the Process, Path, and CommandLine
                    string ver = $"{item.Process}-{item.Path.Split('\\').Where(a => a.StartsWith("B")).FirstOrDefault()}";
                    string sPath = item.Path;

                    //keys中已经存在的同名数量
                    int contains = keyValuePairs.Keys.Where(c => c.IndexOf('_')!=-1).Select(a => a.Substring(0, a.IndexOf('_')))
                                                     .Where(b => b == ver)
                                                     .Count();
                    if (contains > 0)
                    {
                        ver = $"{ver}_{contains + 1}";
                    }

                    if (keyValuePairs.Keys.Contains(ver))
                    {
                        keyValuePairs.Add($"{ver}_1", sPath);
                    }
                    else
                    {
                        keyValuePairs.Add(ver, sPath);
                    }
                }
            }

            return keyValuePairs;
        }
    }
}
