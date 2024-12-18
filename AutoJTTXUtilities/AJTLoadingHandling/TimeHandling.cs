namespace AutoJTTXUtilities.AJTLoadingHandling
{
    public class TimeHandling
    {
        //时间格式化传入的毫秒
        public static string FormatTime(int ms)
        {
            string result = string.Empty;

            int hour = 0;
            int minute = 0;

            //计算秒
            int second = ms / 1000;

            if (second > 60)
            {
                //分
                minute = second / 60;
                //秒
                second = second % 60;
            }
            if (minute > 60)
            {
                hour = minute / 60;
                minute = minute % 60;
            }

            if (hour != 0 && minute != 0 && second != 0)
            {
                result = string.Format("{0}小时{1}分钟{2}秒", hour.ToString(), minute.ToString(), second.ToString());
            }
            else if (hour == 0 && minute != 0 && second != 0)
            {
                result = string.Format("{0}分钟{1}秒", minute.ToString(), second.ToString());
            }
            else if (hour == 0 && minute == 0 && second != 0)
            {
                result = string.Format("{0}秒", second.ToString());
            }
            else if (hour == 0 && minute == 0 && second == 0)
            {
                result = string.Format("{0}秒", second.ToString());
            }


            return result;
        }

    }
}
