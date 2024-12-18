using System.Collections.Generic;
using System.Text;

namespace AutoJTTXUtilities.Controls
{
    public class OpenFolderTagMsgHelper : AJTMsgHelperBase
    {
        string _tag;//其余信息

        public string m_Tag { get => _tag; set => _tag = value; }

        public OpenFolderTagMsgHelper(string msgLevel, string message, string tag)
        {
            this.m_MsgLevel = msgLevel;
            this.m_Message = message;
            this._tag = tag;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", this.m_MsgLevel, this.m_Message, this._tag);
        }
        public static string ToStringList(List<OpenFolderTagMsgHelper> helpers)
        {
            if (helpers == null) { return null; }

            string result = string.Empty;

            StringBuilder stringBuilder = new StringBuilder();

            try
            {
                foreach (OpenFolderTagMsgHelper item in helpers)
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
