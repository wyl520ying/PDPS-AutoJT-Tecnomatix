namespace AutoJTMessageUtilities
{
    public class WindowPositionHelper
    {
#if INTERNAL
        //INTERNAL
        //注册表地址
        private static readonly string _regPaht = @"Software/AutoJTApplication/WindowBounds/";

#elif EXTERNAL
        //External
        //注册表地址
        private static readonly string _regPaht = @"Software/AutoJTApplication/WindowBounds0/";
#elif PSV
        //INTERNAL 内部私服
        //注册表地址
        private static readonly string _regPaht = @"Software/AutoJTApplication/WindowBounds/";
#endif

        #region 机器码

        public static string SaveNewIdCode(string code)
        {
            try
            {
                Microsoft.Win32.Registry.CurrentUser.CreateSubKey(_regPaht).SetValue("Summary", code);
            }
            catch
            {
            }

            return code;
        }
        public static string GetNewIdCode()
        {
            string result = string.Empty;

            try
            {


                //读取注册表键值
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser
                                                                          .OpenSubKey(_regPaht);

                if (!(key is null))
                {
                    result = key.GetValue("Summary").ToString();
                }
            }
            catch
            {
            }

            return result;
        }

        #endregion
    }

}
