using System;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace AutoJTTXCoreUtilities
{
    public class NetworkUtil
    {
        public bool NetworkConnection(string targetIP)
        {
            Ping pingSender = new Ping();
            PingReply reply;
            try
            {
                reply = pingSender.Send(targetIP, 120);//第一个参数为ip地址，第二个参数为ping的时间
            }
            catch
            {
                return false; //不通
            }

            if (reply.Status == IPStatus.Success)
            {
                return true; //通
            }
            else
            {
                return false; //不通
            }
        }
    }
    public class Base64Tool
    {
        //示例
        //var val1 = EncodeBase64("utf-8", "<tr><td>你好啊</td></tr>");
        //var val2 = DecodeBase64("utf-8", "PHRyPjx0ZD7kvaDlpb3llYo8L3RkPjwvdHI+");        

        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="code_type">编码类型</param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string EncodeBase64(string code_type, string code)
        {
            string encode = "";
            byte[] bytes = Encoding.GetEncoding(code_type).GetBytes(code);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = code;
            }
            return encode;
        }

        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="code_type">编码类型</param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string DecodeBase64(string code_type, string code)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(code);
            try
            {
                decode = Encoding.GetEncoding(code_type).GetString(bytes);
            }
            catch
            {
                decode = code;
            }
            return decode;
        }
    }

    public class AesTool
    {
        private static int AESKeyLength = 32;//AES加密的密码长度为32位
        private static char AESFillChar = 'Y';//AES密码填充字符,补足不够长的密码

        //public static string DefaultPassword = "AutoJT";//默认密码

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str">要加密的 string 字符串</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encrypt(string str, string key)
        {
            key = FmtPassword(key);
            byte[] keyArray = Encoding.UTF8.GetBytes(key);
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="array">要加密的 byte[] 数组</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] array, string key)
        {
            key = FmtPassword(key);
            byte[] keyArray = Encoding.UTF8.GetBytes(key);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(array, 0, array.Length);

            return resultArray;
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="str">要解密的 string 字符串</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Decrypt(string str, string key)
        {
            key = FmtPassword(key);
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] toEncryptArray = Convert.FromBase64String(str);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="array">要解密的 byte[] 数组</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] array, string key)
        {
            key = FmtPassword(key);
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(array, 0, array.Length);

            return resultArray;
        }
        /// <summary>
        /// 格式化密码
        /// </summary>
        /// <param name="s">要格式化的密码</param>
        /// <returns></returns>
        public static string FmtPassword(string s)
        {
            string password = s ?? "";

            //格式化密码
            if (password.Length < AESKeyLength)
            {
                //补足不够长的密码
                password = password + new string(AESFillChar, AESKeyLength - password.Length);
            }
            else if (password.Length > AESKeyLength)
            {
                //截取过长的密码
                password = password.Substring(0, AESKeyLength);
            }
            return password;
        }
    }

    public class EncryptUtil
    {
        public static void _EncryptUtil(string password, out string p1)//密码加密算法
        {
            string pass1 = Base64Tool.EncodeBase64("utf-8", password);//密码，base64编码
            string key1 = Base64Tool.EncodeBase64("utf-8", "AutoJT123");//KEY，base64编码

            string pass2 = TakeBackToAli(pass1);//密码取反
            string key2 = TakeBackToAli(key1);//key取反

            string pass3 = Base64Tool.EncodeBase64("utf-8", pass2);//密码，base64编码
            string key3 = Base64Tool.EncodeBase64("utf-8", key2);//KEY，base64编码

            p1 = AesTool.Encrypt(pass3, key3);//密码加密             
        }

        static string TakeBackToAli(string str)//取反操作
        {
            char[] rs = str.ToCharArray();
            string result = string.Empty;
            for (int i = rs.Length - 1; i >= 0; i--)
            {
                result += rs[i];
            }
            return result;
        }
    }

}
