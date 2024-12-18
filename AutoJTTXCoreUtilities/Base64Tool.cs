





using System;
using System.Text;


namespace AutoJTTXCoreUtilities
{
  public class Base64Tool
  {
    public static string EncodeBase64(string code_type, string code)
    {
      byte[] bytes = Encoding.GetEncoding(code_type).GetBytes(code);
      string str;
      try
      {
        str = Convert.ToBase64String(bytes);
      }
      catch
      {
        str = code;
      }
      return str;
    }

    public static string DecodeBase64(string code_type, string code)
    {
      byte[] bytes = Convert.FromBase64String(code);
      string str;
      try
      {
        str = Encoding.GetEncoding(code_type).GetString(bytes);
      }
      catch
      {
        str = code;
      }
      return str;
    }
  }
}
