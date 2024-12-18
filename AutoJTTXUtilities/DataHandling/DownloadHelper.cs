using Downloader;
using System.Net;
using System.Reflection;

namespace AutoJTTXUtilities.DataHandling
{
    internal class DownloadHelper
    {
        //创建一个下载服务
        public static DownloadService GetInstance()
        {
            //创建一个下载服务
            DownloadService downloader = null;

            try
            {
                //创建一个下载配置
                var downloadOpt = new DownloadConfiguration()
                {
                    BufferBlockSize = 10240, // 通常，主机最大支持8000字节，默认值为8000。
                    ChunkCount = 8, // 要下载的文件分片数量，默认值为1
                    //MaximumBytesPerSecond = 1024 * 1024, // 下载速度限制为1MB/s，默认值为零或无限制
                    MaxTryAgainOnFailover = int.MaxValue, // 失败的最大次数
                    OnTheFlyDownload = false, // 是否在内存中进行缓存？默认值是true
                    ParallelDownload = true, // 下载文件是否为并行的。默认值为false
                    //TempDirectory = "C:\\temp", // 设置用于缓冲大块文件的临时路径，默认路径为Path.GetTempPath()。
                    Timeout = 1000, // 每个 stream reader  的超时（毫秒），默认值是1000
                    RequestConfiguration = // 定制请求头文件
                    {
                        Accept = "*/*",
                        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                        CookieContainer =  new CookieContainer(), // Add your cookies
                        Headers = new WebHeaderCollection(), // Add your custom headers
                        KeepAlive = false,
                        ProtocolVersion = HttpVersion.Version11, // Default value is HTTP 1.1
                        UseDefaultCredentials = false,
                        UserAgent = $"DownloaderSample/{Assembly.GetExecutingAssembly().GetName().Version.ToString(3)}"
                    }
                };

                //创建一个下载服务
                downloader = new DownloadService(downloadOpt);
            }
            catch
            {
            }

            return downloader;
        }
    }

}

