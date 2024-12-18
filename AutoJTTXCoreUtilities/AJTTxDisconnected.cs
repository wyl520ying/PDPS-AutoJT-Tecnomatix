using EngineeringInternalExtension;
using System;
using System.Drawing;
using System.IO;
using Tecnomatix.Engineering;
using Tecnomatix.Planning;


namespace AutoJTTXCoreUtilities
{
    public class AJTTxDisconnected
    {
        public static string GetValueFromObjektField(ITxPlanningObject planningObject, string fieldName)
        {
            string result;
            if (planningObject != null && fieldName != null && fieldName.Length > 0 && !(planningObject.ProcessModelId == null))
            {
                result = AJTTxDisconnected.GetValueFromObjektField(planningObject.ProcessModelId.ExternalId, fieldName);
            }
            else
            {
                result = null;
            }
            return result;
        }

        public static string GetValueFromObjektField(string externalId, string fieldName)
        {
            string result;
            if (externalId != null && fieldName != null && fieldName.Length > 0 && externalId.Length != 0)
            {
                string text;
                string objektAsXmlText = AJTTxDisconnected.GetObjektAsXmlText(externalId, out text);
                if (objektAsXmlText != null && objektAsXmlText.Length > 0)
                {
                    result = AJTTxDisconnected.GetValueFromXml(objektAsXmlText, fieldName);
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

        public static string GetObjektAsXmlText(string externalId, out string className)
        {
            if (externalId == null)
            {
                throw new ArgumentException("No external id value in routine GetObjektAsXmlText", "externalId");
            }
            string disconnectedStudyFolder = AJTTxDisconnected.GetDisconnectedStudyFolder();
            if (disconnectedStudyFolder == null)
            {
                throw new Exception("Cannot get disconnected study folder");
            }
            StreamReader streamReader = File.OpenText(Path.Combine(disconnectedStudyFolder, "StandaloneStudy_PsState.xml"));
            if (streamReader == null)
            {
                throw new Exception("Cannot read study file: StandaloneStudy_PsState.xml");
            }
            string text = "ExternalId=\"" + externalId + "\"";
            className = "";
            string text2 = "";
            bool flag = false;
            while (!streamReader.EndOfStream && !flag)
            {
                string text3 = streamReader.ReadLine().Trim();
                if (text3.Length != 0)
                {
                    if (className.Length == 0)
                    {
                        int num = text3.IndexOf(text);
                        if (num != -1)
                        {
                            for (int num2 = num; num2 >= 0; num2--)
                            {
                                if (text3[num2].CompareTo('<') == 0)
                                {
                                    className = text3.Substring(num2 + 1, num - num2 - 1);
                                    className = className.Trim();
                                    text2 = text3.Substring(num2, num - num2 + text.Length);
                                    text3 = text3.Substring(num + text.Length);
                                    break;
                                }
                            }
                        }
                    }
                    if (className.Length > 0)
                    {
                        text = "</" + className + ">";
                        int num3 = text3.IndexOf(text);
                        if (num3 == -1)
                        {
                            text2 += text3;
                        }
                        else
                        {
                            text2 += text3.Substring(0, num3 + text.Length);
                            flag = true;
                        }
                    }
                }
            }
            streamReader.Close();
            if (text2 == null || text2.Length == 0)
            {
                throw new Exception("Cannot find object in the study file: " + externalId);
            }
            return text2;
        }

        public static string GetValueFromXml(string xmlString, string fieldName)
        {
            int num = xmlString.IndexOf("<" + fieldName + ">") + fieldName.Length + 2;
            string result;
            if (num < fieldName.Length + 2)
            {
                result = null;
            }
            else
            {
                int num2 = xmlString.IndexOf("</" + fieldName + ">", num);
                if (num2 >= 0)
                {
                    string text = xmlString.Substring(num, num2 - num).Trim();
                    if (text.CompareTo("NULL") == 0)
                    {
                        text = "";
                    }
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            return result;
        }

        public static Image GetClassImage(string className)
        {
            string path = AJTTxDisconnected.GetDisconnectedStudyFolder();
            path = Path.Combine(path, "Icons");
            string text = Path.Combine(path, className + ".bmp");
            Image result;
            if (!File.Exists(text))
            {
                result = null;
            }
            else
            {
                result = Image.FromFile(text);
            }
            return result;
        }

        public static AJTTxDisconnected.SessionMgrData GetSessionManagerData()
        {
            AJTTxDisconnected.SessionMgrData result = default(AJTTxDisconnected.SessionMgrData);
            string sessionManagerDataAsXmlText = AJTTxDisconnected.GetSessionManagerDataAsXmlText();
            result.UserName = AJTTxDisconnected.GetValueFromXml(sessionManagerDataAsXmlText, "UserName");
            result.ProjectId = AJTTxDisconnected.GetValueFromXml(sessionManagerDataAsXmlText, "ProjectId");
            result.ProjectInternalId = AJTTxDisconnected.GetValueFromXml(sessionManagerDataAsXmlText, "ProjectInternalId");
            result.RootNodeId = AJTTxDisconnected.GetValueFromXml(sessionManagerDataAsXmlText, "RootNodeId");
            result.VariantFilter = AJTTxDisconnected.GetValueFromXml(sessionManagerDataAsXmlText, "VariantFilter");
            result.ProjectName = AJTTxDisconnected.GetValueFromXml(sessionManagerDataAsXmlText, "ProjectName");
            return result;
        }

        private static string GetDisconnectedStudyFolder()
        {
            string result;
            try
            {
                TxDocumentEx txDocumentEx = new TxDocumentEx();
                result = Path.GetDirectoryName(txDocumentEx.NodeGetAttachmentFullPath(TxApplication.ActiveDocument.CurrentStudy.ProcessModelId, TxFileRoleEnum.EngineeringDataDir));
            }
            catch (Exception exception)
            {
                AJTTxMessageHandling.WriteException(exception);
                result = null;
            }
            return result;
        }

        public static string GetSessionManagerDataAsXmlText()
        {
            string disconnectedStudyFolder = AJTTxDisconnected.GetDisconnectedStudyFolder();
            if (disconnectedStudyFolder == null)
            {
                throw new Exception("Cannot get disconnected study folder");
            }
            StreamReader streamReader = File.OpenText(Path.Combine(disconnectedStudyFolder, "StandaloneStudy_PsState.xml"));
            if (streamReader == null)
            {
                throw new Exception("Cannot read study file: StandaloneStudy_PsState.xml");
            }
            string text = "";
            bool flag = false;
            while (!streamReader.EndOfStream && !flag)
            {
                string text2 = streamReader.ReadLine().Trim();
                if (text2.Length != 0)
                {
                    if (text.Length == 0)
                    {
                        int num = text2.IndexOf("<SessionMgrData>");
                        if (num != -1)
                        {
                            for (int i = num; i >= 0; i--)
                            {
                                if (text2[i].CompareTo('<') == 0)
                                {
                                    text = text2.Substring(i, num - i + 16);
                                    text2 = text2.Substring(num + 16);
                                    break;
                                }
                            }
                        }
                    }
                    if (text.Length > 0)
                    {
                        int num2 = text2.IndexOf("</SessionMgrData>");
                        if (num2 != -1)
                        {
                            text += text2.Substring(0, num2 + 17);
                            flag = true;
                        }
                        else
                        {
                            text += text2;
                        }
                    }
                }
            }
            streamReader.Close();
            if (text == null || text.Length == 0)
            {
                throw new Exception("Cannot find Session Manager Data");
            }
            return text;
        }

        private const string StudPsFile = "StandaloneStudy_PsState.xml";

        private const string StudEmsFile = "StandaloneStudy_EmsState.xml";

        public struct SessionMgrData
        {
            public string UserName { get; set; }

            public string ProjectId { get; set; }

            public string ProjectInternalId { get; set; }

            public string RootNodeId { get; set; }

            public string VariantFilter { get; set; }

            public string ProjectName { get; set; }
        }
    }
}