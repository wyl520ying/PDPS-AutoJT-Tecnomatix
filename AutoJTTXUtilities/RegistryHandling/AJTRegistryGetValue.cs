using Microsoft.Win32;
using System.Collections.Generic;

namespace AutoJTTXUtilities.RegistryHandling
{
    public class AJTRegistryGetValue
    {
        public string ClassVersion
        {
            get
            {
                return this.m_ClassVersion;
            }
        }

        public string ErrorString
        {
            get
            {
                return this.m_ErrorString;
            }
            private set
            {
                this.m_ErrorString = value;
            }
        }

        public string GetValue(RegistryKey RootKey, List<string> KeyNames, string ValueName)
        {
            string result;
            if (RootKey == null)
            {
                result = null;
            }
            else if (KeyNames != null && KeyNames.Count != 0)
            {
                if (ValueName != null && ValueName.Length != 0)
                {
                    string text = RootKey.Name;
                    for (int i = 0; i < KeyNames.Count; i++)
                    {
                        text = text + "\\" + KeyNames[i];
                    }
                    object value = Registry.GetValue(text, ValueName, null);
                    if (value != null)
                    {
                        result = value.ToString();
                    }
                    else
                    {
                        result = null;
                    }
                }
                else
                {
                    result = null;
                }
            }
            else
            {
                result = null;
            }
            return result;
        }

        private string m_ClassVersion = "1.0.0";

        private string m_ErrorString = null;
    }
}
