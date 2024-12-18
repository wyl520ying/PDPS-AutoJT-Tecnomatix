using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace AutoJTTXUtilities.DataHandling
{
    public class ZipArchiveHelper
    {
        public static async void CompressDirectoryZip(string folderPath)
        {
            DirectoryInfo sourceFolder = new DirectoryInfo(folderPath);
            DirectoryInfo zipFolder = sourceFolder.Parent;  
            string parentFolder = zipFolder.FullName;
            string fileName = $"{sourceFolder.Name}_{string.Format("{0:yyyyMMdd}",System.DateTime.Now)}.zip";
            await ZipArchiveHelper.CompressDirectoryZip(folderPath,System.IO.Path.Combine(parentFolder, fileName));
        }
        /// <summary>
        /// 将指定目录压缩为Zip文件
        /// </summary>
        /// <param name="folderPath">文件夹地址 D:/1/ </param>
        /// <param name="zipPath">zip地址 D:/1.zip </param>
        public static Task CompressDirectoryZip(string folderPath, string zipPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(zipPath);

            if (directoryInfo.Parent != null)
            {
                directoryInfo = directoryInfo.Parent;
            }

            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            ZipFile.CreateFromDirectory(folderPath, zipPath, CompressionLevel.Optimal, false);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 将指定文件压缩为Zip文件
        /// </summary>
        /// <param name="filePath">文件地址 D:/1.txt </param>
        /// <param name="zipPath">zip地址 D:/1.zip </param>
        public static void CompressFileZip(string filePath, string zipPath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            string dirPath = fileInfo.DirectoryName?.Replace("\\", "/") + "/";
            string tempPath = dirPath + Guid.NewGuid() + "_temp/";
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }
            fileInfo.CopyTo(tempPath + fileInfo.Name);
            CompressDirectoryZip(tempPath, zipPath);
            DirectoryInfo directory = new DirectoryInfo(tempPath);
            if (directory.Exists)
            {
                //将文件夹属性设置为普通,如：只读文件夹设置为普通
                directory.Attributes = FileAttributes.Normal;

                directory.Delete(true);
            }
        }
    }
}