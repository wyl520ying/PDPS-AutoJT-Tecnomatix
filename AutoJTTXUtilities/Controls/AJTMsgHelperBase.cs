using System.Collections.Generic;
using System.Text;

namespace AutoJTTXUtilities.Controls
{
    public class AJTMsgHelperBase
    {
        string _msgLevel;//Info , Error

        string _message;//msg

        public AJTMsgHelperBase(string msgLevel, string message)
        {
            _msgLevel = msgLevel;
            _message = message;
        }
        public AJTMsgHelperBase()
        {

        }
        //没有绑定焊枪
        //没有转cgr
        //没有绑定appearance

        public string m_Message { get => _message; set => _message = value; }
        public string m_MsgLevel { get => _msgLevel; set => _msgLevel = value; }

        public virtual new string ToString()
        {
            return string.Format("{0} {1}", this.m_MsgLevel, this.m_Message);
        }

        public static string ToStringList(List<AJTMsgHelperBase> helpers)
        {
            if (helpers == null) { return null; }

            string result = string.Empty;

            StringBuilder stringBuilder = new StringBuilder();

            try
            {
                foreach (AJTMsgHelperBase item in helpers)
                {
                    stringBuilder.AppendLine(item.ToString());
                }
            }
            catch
            {
            }

            result = stringBuilder.ToString();

            return result;
        }
    }

}
