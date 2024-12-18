using EMPAPPLICATIONLib;
using EMPMODELLib;
using EMPTYPELIBRARYLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Tecnomatix.Engineering;

namespace AutoJTTXCoreUtilities
{
    public class AJTTxTuneData
    {
        public static AJTTxTuneData.AJTTxProcessModelPrototype ReadTuneDataXml(string tuneDataFile, string projectExternalID)
        {
            AJTTxTuneData.AJTTxProcessModelPrototype result;
            if (!string.IsNullOrEmpty(projectExternalID) && !string.IsNullOrEmpty(tuneDataFile))
            {
                if (Path.GetExtension(tuneDataFile).Equals(".cojt") || Path.GetExtension(tuneDataFile).Equals(".co"))
                {
                    tuneDataFile = Path.Combine(tuneDataFile, "TuneData.xml");
                }
                try
                {
                    if (!File.Exists(tuneDataFile))
                    {
                        return null;
                    }
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(tuneDataFile);
                    XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
                    xmlNamespaceManager.AddNamespace("emp", "http://www.tecnomatix.com/empower/schema");
                    XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/emp:TuneDocument/Header/ProcessModelsPrototypes/ProcessModelPrototype", xmlNamespaceManager);
                    if (xmlNodeList == null)
                    {
                        return null;
                    }
                    foreach (object obj in xmlNodeList)
                    {
                        XmlNode xmlNode = (XmlNode)obj;
                        string xmlNodeAttribute = AJTTxTuneData.GetXmlNodeAttribute(xmlNode, "ProjectId");
                        if (!string.IsNullOrEmpty(xmlNodeAttribute) && xmlNodeAttribute.Equals(projectExternalID))
                        {
                            AJTTxTuneData.AJTTxProcessModelPrototype ctxProcessModelPrototype = new AJTTxTuneData.AJTTxProcessModelPrototype();
                            ctxProcessModelPrototype.ProjectID = xmlNodeAttribute;
                            XmlNode xmlNode2 = xmlNode.SelectSingleNode("ProcessModelID/ExternalID");
                            ctxProcessModelPrototype.ExternalID = xmlNode2.InnerText;
                            xmlNode2 = xmlNode.SelectSingleNode("ProcessModelType/Type");
                            ctxProcessModelPrototype.Type = xmlNode2.InnerText;
                            xmlNode2 = xmlNode.SelectSingleNode("ProcessModelType/Family");
                            ctxProcessModelPrototype.Family = xmlNode2.InnerText;
                            return ctxProcessModelPrototype;
                        }
                    }
                }
                catch (Exception exception)
                {
                    AJTTxMessageHandling.WriteException(exception);
                    return null;
                }
                result = null;
            }
            else
            {
                result = null;
            }
            return result;
        }

        public static List<AJTTxTuneData.AJTTxProcessModelPrototype> ReadTuneDataXml(string tuneDataFile)
        {
            List<AJTTxTuneData.AJTTxProcessModelPrototype> list = new List<AJTTxTuneData.AJTTxProcessModelPrototype>();
            if (Path.GetExtension(tuneDataFile).Equals(".cojt") || Path.GetExtension(tuneDataFile).Equals(".co"))
            {
                tuneDataFile = Path.Combine(tuneDataFile, "TuneData.xml");
            }
            try
            {
                if (!File.Exists(tuneDataFile))
                {
                    return list;
                }
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(tuneDataFile);
                XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
                xmlNamespaceManager.AddNamespace("emp", "http://www.tecnomatix.com/empower/schema");
                XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/emp:TuneDocument/Header/ProcessModelsPrototypes/ProcessModelPrototype", xmlNamespaceManager);
                if (xmlNodeList == null)
                {
                    return list;
                }
                foreach (object obj in xmlNodeList)
                {
                    XmlNode xmlNode = (XmlNode)obj;
                    AJTTxTuneData.AJTTxProcessModelPrototype ctxProcessModelPrototype = new AJTTxTuneData.AJTTxProcessModelPrototype();
                    ctxProcessModelPrototype.ProjectID = AJTTxTuneData.GetXmlNodeAttribute(xmlNode, "ProjectId");
                    XmlNode xmlNode2 = xmlNode.SelectSingleNode("ProcessModelID/ExternalID");
                    ctxProcessModelPrototype.ExternalID = xmlNode2.InnerText;
                    xmlNode2 = xmlNode.SelectSingleNode("ProcessModelType/Type");
                    ctxProcessModelPrototype.Type = xmlNode2.InnerText;
                    xmlNode2 = xmlNode.SelectSingleNode("ProcessModelType/Family");
                    ctxProcessModelPrototype.Family = xmlNode2.InnerText;
                    list.Add(ctxProcessModelPrototype);
                }
            }
            catch (Exception exception)
            {
                AJTTxMessageHandling.WriteException(exception);
                return list;
            }
            return list;
        }

        public static bool CreateNewTuneDataEntry(string tuneDataPath, string prototypeClass, string prototypeFamily, string externalId, out string error)
        {
            error = "";
            bool result;
            if (TxApplication.PlatformType == TxPlatformType.Offline)
            {
                result = false;
            }
            else
            {
                string projectID = AJTTxTuneData.GetProjectID();
                if (!string.IsNullOrEmpty(projectID))
                {
                    AJTTxTuneData.AJTTxProcessModelPrototype ctxProcessModelPrototype = new AJTTxTuneData.AJTTxProcessModelPrototype();
                    ctxProcessModelPrototype.ProjectID = projectID;
                    ctxProcessModelPrototype.ExternalID = externalId;
                    ctxProcessModelPrototype.Type = prototypeClass;
                    ctxProcessModelPrototype.Family = prototypeFamily;
                    tuneDataPath = Path.Combine(tuneDataPath, "TuneData.xml");
                    result = AJTTxTuneData.AddToTuneDataXml(tuneDataPath, ctxProcessModelPrototype, out error);
                }
                else
                {
                    error = "Cannot get project external id.";
                    result = false;
                }
            }
            return result;
        }

        public static bool AddToTuneDataXml(string tuneDataFile, AJTTxTuneData.AJTTxProcessModelPrototype processModelPrototype, out string error)
        {
            error = "Unknown error. (TecnomatixStudyLib.TuneData.AddToTuneDataXml)";
            List<AJTTxTuneData.AJTTxProcessModelPrototype> list = AJTTxTuneData.ReadTuneDataXml(tuneDataFile);
            if (list == null)
            {
                list = new List<AJTTxTuneData.AJTTxProcessModelPrototype>();
            }
            list.Add(processModelPrototype);
            AJTTxTuneData.WriteTuneDataXml(tuneDataFile, list, out error);
            return true;
        }

        public static bool WriteTuneDataXml(string path, string projectId, string externalId, string className, string family, out string error)
        {
            error = "Unknown error. (TecnomatixStudyLib.TuneData.WriteTuneDataXml)";
            List<AJTTxTuneData.AJTTxProcessModelPrototype> list = new List<AJTTxTuneData.AJTTxProcessModelPrototype>();
            string text = null;
            try
            {
                text = Path.Combine(path, "TuneData.xml");
                list.Add(new AJTTxTuneData.AJTTxProcessModelPrototype
                {
                    ProjectID = projectId,
                    ExternalID = externalId,
                    Type = className,
                    Family = family
                });
            }
            catch (Exception ex)
            {
                AJTTxMessageHandling.WriteException(ex);
                error = "Cannot create tune data file: " + text + ". " + ex.Message;
                return false;
            }
            return AJTTxTuneData.WriteTuneDataXml(text, list, out error);
        }

        private static bool WriteTuneDataXml(string tuneDataFile, List<AJTTxTuneData.AJTTxProcessModelPrototype> processModelPrototypes, out string error)
        {
            error = "Unknown error. (TecnomatixStudyLib.TuneData.WriteTuneDataXml)";
            try
            {
                TextWriter textWriter = new StreamWriter(tuneDataFile);
                textWriter.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                textWriter.Write("<emp:TuneDocument xmlns:emp=\"http://www.tecnomatix.com/empower/schema\">");
                textWriter.Write("<Header>");
                textWriter.Write("<ProcessModelsPrototypes>");
                foreach (AJTTxTuneData.AJTTxProcessModelPrototype ctxProcessModelPrototype in processModelPrototypes)
                {
                    textWriter.Write("<ProcessModelPrototype ProjectId=\"" + ctxProcessModelPrototype.ProjectID + "\">");
                    textWriter.Write("<ProcessModelID>");
                    textWriter.Write("<ExternalID>" + ctxProcessModelPrototype.ExternalID + "</ExternalID>");
                    textWriter.Write("</ProcessModelID>");
                    textWriter.Write("<ProcessModelType>");
                    textWriter.Write("<Type>" + ctxProcessModelPrototype.Type + "</Type>");
                    textWriter.Write("<Family>" + ctxProcessModelPrototype.Family + "</Family>");
                    textWriter.Write("</ProcessModelType>");
                    textWriter.Write("<ProcessModelData>");
                    textWriter.Write("<Weight>0.000</Weight>");
                    textWriter.Write("</ProcessModelData>");
                    textWriter.Write("</ProcessModelPrototype>");
                }
                textWriter.Write("</ProcessModelsPrototypes>");
                textWriter.Write("</Header>");
                textWriter.Write("</emp:TuneDocument>");
                textWriter.Close();
            }
            catch (Exception ex)
            {
                AJTTxMessageHandling.WriteException(ex);
                error = "Cannot create tune data file: " + tuneDataFile + ". " + ex.Message;
                return false;
            }
            return true;
        }

        public static string GetProjectID()
        {
            string result = "";
            if (TxApplication.PlatformType == TxPlatformType.EmServer)
            {
                EmpApplication empApplication = new EmpApplicationClass();
                EmpObjectKey currentProject = empApplication.CurrentProject;
                EmpContext context = empApplication.Context;
                EmpProjectClass empProjectClass = new EmpProjectClass();
                result = empProjectClass.get_ExternalID(ref context, ref currentProject);
            }
            else if (TxApplication.PlatformType == TxPlatformType.Offline)
            {
                result = AJTTxDisconnected.GetSessionManagerData().ProjectId;
            }
            return result;
        }

        public static string GetXmlNodeAttribute(XmlNode node, string attribute)
        {
            string result;
            if (node != null && !string.IsNullOrEmpty(attribute))
            {
                for (int i = 0; i < node.Attributes.Count; i++)
                {
                    if (node.Attributes[i].Name.Equals(attribute))
                    {
                        return node.Attributes[i].Value;
                    }
                }
                result = "";
            }
            else
            {
                result = "";
            }
            return result;
        }

        public const string TUNE_DATA_XML_FILE = "TuneData.xml";

        public class AJTTxProcessModelPrototype
        {
            public string ProjectID { get; set; }

            public string ExternalID { get; set; }

            public string Type { get; set; }

            public string Family { get; set; }
        }
    }
}
