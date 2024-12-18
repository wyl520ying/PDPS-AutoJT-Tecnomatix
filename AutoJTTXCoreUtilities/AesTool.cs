





using System;
using System.Security.Cryptography;
using System.Text;


namespace AutoJTTXCoreUtilities
{
  public class AesTool
  {
    private static int AESKeyLength = 32;
    private static char AESFillChar = 'Y';

    public static string Encrypt(string str, string key)
    {
      key = FmtPassword(key);
      byte[] bytes1 = Encoding.UTF8.GetBytes(key);
      byte[] bytes2 = Encoding.UTF8.GetBytes(str);
      RijndaelManaged rijndaelManaged = new RijndaelManaged();
      rijndaelManaged.Key = bytes1;
      rijndaelManaged.Mode = CipherMode.ECB;
      rijndaelManaged.Padding = PaddingMode.PKCS7;
      byte[] inArray = rijndaelManaged.CreateEncryptor().TransformFinalBlock(bytes2, 0, bytes2.Length);
      return Convert.ToBase64String(inArray, 0, inArray.Length);
    }

    public static byte[] Encrypt(byte[] array, string key)
    {
      key = FmtPassword(key);
      byte[] bytes = Encoding.UTF8.GetBytes(key);
      RijndaelManaged rijndaelManaged = new RijndaelManaged();
      rijndaelManaged.Key = bytes;
      rijndaelManaged.Mode = CipherMode.ECB;
      rijndaelManaged.Padding = PaddingMode.PKCS7;
      return rijndaelManaged.CreateEncryptor().TransformFinalBlock(array, 0, array.Length);
    }

    public static string Decrypt(string str, string key)
    {
      key = FmtPassword(key);
      byte[] bytes = Encoding.UTF8.GetBytes(key);
      byte[] inputBuffer = Convert.FromBase64String(str);
      RijndaelManaged rijndaelManaged = new RijndaelManaged();
      rijndaelManaged.Key = bytes;
      rijndaelManaged.Mode = CipherMode.ECB;
      rijndaelManaged.Padding = PaddingMode.PKCS7;
      return Encoding.UTF8.GetString(rijndaelManaged.CreateDecryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length));
    }

    public static byte[] Decrypt(byte[] array, string key)
    {
      key = FmtPassword(key);
      byte[] bytes = Encoding.UTF8.GetBytes(key);
      RijndaelManaged rijndaelManaged = new RijndaelManaged();
      rijndaelManaged.Key = bytes;
      rijndaelManaged.Mode = CipherMode.ECB;
      rijndaelManaged.Padding = PaddingMode.PKCS7;
      return rijndaelManaged.CreateDecryptor().TransformFinalBlock(array, 0, array.Length);
    }

    public static string FmtPassword(string s)
    {
      string str = s ?? "";
      if (str.Length < AESKeyLength)
        str += new string(AESFillChar, AESKeyLength - str.Length);
      else if (str.Length > AESKeyLength)
        str = str.Substring(0, AESKeyLength);
      return str;
    }
  }
}
