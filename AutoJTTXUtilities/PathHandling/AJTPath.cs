using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;

namespace AutoJTTXUtilities.PathHandling
{
    public static class AJTPath
    {
        /// <summary>
        /// 文件名格式化为"_"
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetAllowedStringForFilename(string input, bool isReplaceUnderline = false)
        {
            string result;
            if (input != null)
            {
                input = input.Replace(" ", "_");
                input = input.Replace(".", "_");
                input = input.Replace(":", "");
                input = input.Replace(";", "");
                input = input.Replace("/", "_");
                input = input.Replace("\\", "_");
                input = input.Replace(",", "");
                input = input.Replace("<", "");
                input = input.Replace(">", "");
                input = input.Replace('"', '_');
                input = input.Replace("|", "_");
                input = input.Replace("*", "_");
                input = input.Replace("?", "_");
                //是否替换-为_
                if (isReplaceUnderline)
                {
                    input = input.Replace("-", "_");
                    input = input.Replace("!", "_");
                    input = input.Replace("#", "_");
                    input = input.Replace("$", "_");
                    input = input.Replace("%", "_");
                    input = input.Replace("&", "_");
                    input = input.Replace("'", "_");
                    input = input.Replace("(", "_");
                    input = input.Replace(")", "_");
                    input = input.Replace("*", "_");
                    input = input.Replace("+", "_");
                }
                result = input;
            }
            else
            {
                result = "";
            }
            return result;
        }

        public static string GetFileNameNoNamingRules(string fileName, bool isReplaceUnderline = false)
        {
            try
            {
                //检查中文            
                if (Regex.IsMatch(fileName, @"[\u4e00-\u9fa5]"))
                {
                    try
                    {
                        fileName = AJTNPinyin.GetPinyin(fileName);
                    }
                    catch { }

                    //解决乱码问题
                    try
                    {
                        string pattern = "[A-Za-z0-9\u0020-\u002D\u005F]+";
                        string MatchStr = "";
                        MatchCollection results = Regex.Matches(fileName, pattern);
                        foreach (var s in results)
                        {
                            MatchStr += s.ToString();
                        }

                        //正确的匹配
                        if (!string.IsNullOrEmpty(MatchStr))
                        {
                            fileName = MatchStr;
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch { }


            /*
            //检查空格
            if (fileName.Contains(" "))
            {
                fileName = Regex.Replace(fileName.Trim(), " {1,}", "");
            }
            
            fileName = Regex.Replace(fileName, "[^A-Za-z0-9_]", "_");
            */
            fileName = GetAllowedStringForFilename(fileName, isReplaceUnderline);

            return fileName;
        }

        public static string ChangePathSyntax(string path, bool withPraefix = true, bool uriSyntax = false, bool withFilePraefix = false)
        {
            string result;
            if (!string.IsNullOrEmpty(path))
            {
                path = path.Replace("\\", "/");
                if (!path.StartsWith("/"))
                {
                    if (uriSyntax)
                    {
                        path = new UriBuilder
                        {
                            Path = path
                        }.Path;
                    }
                    bool flag = false;
                    if (Path.IsPathRooted(path))
                    {
                        flag = true;
                    }
                    if (withPraefix)
                    {
                        if (flag && !path.StartsWith("/"))
                        {
                            path = "/" + path;
                        }
                        else if (!flag)
                        {
                            path = "./" + path;
                        }
                    }
                    if (withFilePraefix && flag)
                    {
                        path = "file:///" + path.TrimStart(new char[]
                        {
                            '/'
                        });
                    }
                    result = path;
                }
                else
                {
                    result = path;
                }
            }
            else
            {
                result = "";
            }
            return result;
        }

        public static string ChangeURIPathToLocalPath(string uriPath)
        {
            string result;
            if (string.IsNullOrEmpty(uriPath))
            {
                result = "";
            }
            else
            {
                try
                {
                    uriPath = new UriBuilder
                    {
                        Path = uriPath
                    }.Uri.LocalPath;
                }
                catch
                {
                    return uriPath;
                }
                result = uriPath;
            }
            return result;
        }

        public static bool CheckPathes(string[] pathesToCheck, out string error)
        {
            error = "";
            bool result;
            if (pathesToCheck != null && pathesToCheck.Length != 0)
            {
                foreach (string text in pathesToCheck)
                {
                    if (!string.IsNullOrEmpty(text) && (text.Length >= 260 || Path.GetDirectoryName(text).Length >= 248))
                    {
                        error = text + ":\n" + new PathTooLongException().Message;
                        return false;
                    }
                }
                result = true;
            }
            else
            {
                result = true;
            }
            return result;
        }

        public static bool CheckPathLength(string pathToCheck, out string error)
        {
            return AJTPath.CheckPathes(new string[]
            {
                pathToCheck
            }, out error);
        }

        public static bool CheckPathLengths(string[] pathsToCheck, out string error)
        {
            return AJTPath.CheckPathes(pathsToCheck, out error);
        }

        public static string GetShortPathWithEllipsis(string path, ref int length)
        {
            string result;
            if (string.IsNullOrEmpty(path))
            {
                result = null;
            }
            else
            {
                try
                {
                    path = path.Replace('/', Path.DirectorySeparatorChar);
                    string directoryName = Path.GetDirectoryName(path);
                    string fileName = Path.GetFileName(path);
                    string pathRoot = AJTPath.GetPathRoot(path);
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append(pathRoot);
                    Stack<string> stack = new Stack<string>();
                    stack.Push(fileName);

                    if (fileName.Length > length)
                    {
                        length = fileName.Length;
                    }

                    int num = pathRoot.Length + fileName.Length;
                    string directoryName2 = Path.GetDirectoryName(directoryName);

                    fileName = Path.GetFileName(directoryName);

                    while (directoryName2 != null)
                    {
                        num += fileName.Length;
                        if (num > length)
                        {
                            stack.Push("..." + Path.DirectorySeparatorChar.ToString());
                            //IL_104:
                            while (stack.Count > 0)
                            {
                                stringBuilder.Append(stack.Pop());
                            }
                            return stringBuilder.ToString();
                        }
                        stack.Push(Path.DirectorySeparatorChar.ToString());
                        stack.Push(fileName);
                        fileName = Path.GetFileName(directoryName2);
                        directoryName2 = Path.GetDirectoryName(directoryName2);
                    }
                    //goto IL_104;
                }
                catch
                {
                }
                result = null;
            }
            return result;
        }

        private static string GetPathRoot(string path)
        {
            string text = Path.GetPathRoot(path);
            if (!text.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                text += Path.DirectorySeparatorChar.ToString();
            }
            return text;
        }

        public static bool ExistsFileOrDirectory(string path)
        {
            bool result;
            if (string.IsNullOrEmpty(path))
            {
                result = false;
            }
            else
            {
                try
                {
                    FileInfo fileInfo = new FileInfo(path);
                    if (fileInfo.Exists)
                    {
                        return true;
                    }
                }
                catch
                {
                }
                try
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(path);
                    if (directoryInfo.Exists)
                    {
                        return true;
                    }
                }
                catch
                {
                }
                result = false;
            }
            return result;
        }


        public static bool DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, out string infos)
        {
            bool result = false;
            infos = string.Empty;

            try
            {
                //检查输入
                if (string.IsNullOrEmpty(sourceDirName) || string.IsNullOrEmpty(destDirName))
                {
                    result = false;
                    infos = "empty data";
                    return result;
                }

                //source info
                DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirName);
                //子目录
                DirectoryInfo[] directories = directoryInfo.GetDirectories();

                //检查source 目录是否存在
                if (!directoryInfo.Exists)
                {
                    result = false;
                    infos = "Source directory does not exist or could not be found: " +
                        "\n" + sourceDirName;
                    return result;
                }

                //检查dest 目录是否存在
                if (!Directory.Exists(destDirName))
                {
                    //创建dest 路径
                    Directory.CreateDirectory(destDirName);
                }
                else
                {
                    result = false;
                    infos = destDirName + "\n" +
                        "Directory already exists.";
                    return result;
                }

                try
                {
                    //所有一级文件集合
                    FileInfo[] files = directoryInfo.GetFiles();

                    //在一级文件集合中遍历
                    foreach (FileInfo fileInfo in files)
                    {
                        //复制所有的一级文件
                        string destFileName = Path.Combine(destDirName, fileInfo.Name);
                        fileInfo.CopyTo(destFileName, true);//true 允许复制
                    }

                }
                catch (Exception ex)
                {
                    result = false;
                    infos = ex.Message;
                    return result;
                }

                try
                {
                    //复制source中所有的子目录
                    if (copySubDirs)
                    {
                        foreach (DirectoryInfo directoryInfo2 in directories)
                        {
                            string destDirName2 = Path.Combine(destDirName, directoryInfo2.Name);
                            DirectoryCopy(directoryInfo2.FullName, destDirName2, copySubDirs, out infos);
                        }
                    }
                }
                catch (Exception ex)
                {

                    result = false;
                    infos = ex.Message;
                    return result;
                }


                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                infos = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 检查当前用户是否拥有此文件夹的操作权限
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool HasOperationPermission(string path)
        {
            bool result = false;
            try
            {
                var NtAccountName = Path.Combine(Environment.UserDomainName, Environment.UserName);

                DirectoryInfo di = new DirectoryInfo(path);
                DirectorySecurity acl = di.GetAccessControl(AccessControlSections.All);
                AuthorizationRuleCollection rules = acl.GetAccessRules(true, true, typeof(NTAccount));

                //Go through the rules returned from the DirectorySecurity //遍历DirectorySecurity返回的规则
                foreach (AuthorizationRule rule in rules)
                {
                    //If we find one that matches the identity we are looking for//如果我们找到一个与我们正在寻找的身份相匹配的人
                    if (rule.IdentityReference.Value.Equals(NtAccountName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        var filesystemAccessRule = (FileSystemAccessRule)rule;

                        //Cast to a FileSystemAccessRule to check for access rights//转换为FileSystemAccessRule以检查访问权限
                        if ((filesystemAccessRule.FileSystemRights & FileSystemRights.WriteData) > 0 && filesystemAccessRule.AccessControlType != AccessControlType.Deny)
                        {
                            result = true;

                            break;
                        }                        
                    }
                }              
            }
            catch 
            {
                return false;
            }         

            return result;
        }

        //获取当前UNIX时间戳（秒）
        public static string GetUNIXTimestampAS()
        {
            // 获取当前时间
            DateTime now = DateTime.UtcNow;

            // 计算UNIX时间戳（以秒为单位）
            long unixTimestamp = new DateTimeOffset(now).ToUnixTimeSeconds();

            //Console.WriteLine("当前UNIX时间戳（秒）: " + unixTimestamp);

            return unixTimestamp.ToString();
        }

        //获取当前UNIX时间戳（毫秒）
        public static string GetUNIXTimestampMS()
        {
            // 获取当前时间
            DateTime now = DateTime.UtcNow;

            // 计算UNIX时间戳（以毫秒为单位）
            long unixTimestampMilliseconds = new DateTimeOffset(now).ToUnixTimeMilliseconds();

            //Console.WriteLine("当前UNIX时间戳（毫秒）: " + unixTimestampMilliseconds);

            return unixTimestampMilliseconds.ToString();
        }
    }
}
