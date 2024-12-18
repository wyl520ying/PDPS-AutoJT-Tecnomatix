using System.IO;
using System.Xml;
using Tecnomatix.Engineering.DataTypes;
using Tecnomatix.Engineering.Utilities;

namespace AutoJTTXCoreUtilities
{
    public class AJTTxTuneDataUtilitites
    {
        public static string GetPrototypeExternalId(string fullPath)
        {
            string result;
            if (!AJTTxDocumentUtilities.IsDocumentLoaded())
            {
                result = null;
            }
            else
            {
                string text = null;
                fullPath = AJTTxTuneDataUtilitites.GetTuneDataFilePath(fullPath);
                if (fullPath != null && File.Exists(fullPath))
                {
                    fullPath = Path.GetDirectoryName(fullPath);
                    TxTunePmDocumentManager txTunePmDocumentManager = new TxTunePmDocumentManager();
                    TxTunePmDocumentPrototypeData txTunePmDocumentPrototypeData = txTunePmDocumentManager.LoadPrototypeDataFromDocument(fullPath);
                    if (txTunePmDocumentPrototypeData == null)
                    {
                        result = text;
                    }
                    else
                    {
                        text = txTunePmDocumentPrototypeData.ExternalId;
                        result = text;
                    }
                }
                else
                {
                    result = null;
                }
            }
            return result;
        }

        public static string ReadFirstExternalIDsFromTuneDataXml(string tuneDataFile)
        {
            string result;
            if (!string.IsNullOrEmpty(tuneDataFile) && File.Exists(tuneDataFile))
            {
                string text = string.Empty;
                try
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(tuneDataFile);
                    XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("ProcessModelPrototype");
                    if (elementsByTagName == null || elementsByTagName.Count < 1)
                    {
                        return null;
                    }
                    XmlNode xmlNode = elementsByTagName[0].SelectSingleNode("ProcessModelID/ExternalID");
                    text = xmlNode.InnerText;
                }
                catch
                {
                }
                result = text;
            }
            else
            {
                result = null;
            }
            return result;
        }

        public static string GetTuneDataFilePath(string fullPath)
        {
            string result;
            if (string.IsNullOrEmpty(fullPath))
            {
                result = null;
            }
            else
            {
                try
                {
                    string fileName = Path.GetFileName(fullPath);
                    if (fileName.ToLower().Equals("TuneData.xml".ToLower()))
                    {
                        return fullPath;
                    }
                    if (!fileName.ToLower().EndsWith(".jt"))
                    {
                        return Path.Combine(fullPath, "TuneData.xml");
                    }
                    return Path.Combine(Path.GetDirectoryName(fullPath), "TuneData.xml");
                }
                catch
                {
                }
                result = null;
            }
            return result;
        }

        public static bool ExistsTuneDataFile(string fullPath)
        {
            AJTTxTuneDataUtilitites.GetTuneDataFilePath(fullPath);
            return File.Exists(fullPath);
        }

        public const string TUNE_DATA_FILE_NAME = "TuneData.xml";
    }
}
