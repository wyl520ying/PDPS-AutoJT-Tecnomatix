using AutoJTTXUtilities.Controls;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Xml;

namespace AutoJTTXUtilities.ConfigurationHandling
{
    public class XMLConfigurationFileOperation
    {
        #region 公共部分

        #region 属性

        //xml配置文件夹名
        const string xmlConfigFoderName = "AJT_Tools";




        //sys root
        string sysRoot;
        //xml文件名(所有的功能配置同一个文件名)
        string xmlConfigFileName;
        //xml full path
        string xml_fullPath;

        /// <summary>
        /// sys root
        /// </summary>
        public string SysRoot { get => sysRoot; set => sysRoot = value; }
        /// <summary>
        /// xml文件名
        /// </summary>
        public string XmlConfigFileName { get => xmlConfigFileName; set => xmlConfigFileName = value; }



        #endregion

        #region ctor

        public XMLConfigurationFileOperation(string sysRoot, string xmlConfigFileName = "AutoJTConfiguration.xml")
        {
            this.SysRoot = sysRoot;
            this.XmlConfigFileName = xmlConfigFileName;

            //检查xml配置文件夹是否存在并创建文件夹 
            string ajt_tools_path = CheckXMLConfigFolder();
            //xml fullPath
            this.xml_fullPath = Path.Combine(ajt_tools_path, xmlConfigFileName);
            //检查xml配置文件夹是否存在并创建文件夹
            this.CheckXMLANDCreate(xml_fullPath);
        }


        #endregion

        #region private

        /// <summary>
        /// 检查xml配置文件夹是否存在并创建文件夹
        /// </summary>
        /// <returns></returns>
        string CheckXMLConfigFolder()
        {
            string folder_path = "";

            if (this.sysRoot.EndsWith("\\"))
            {
                folder_path = this.sysRoot + xmlConfigFoderName;
            }
            else
            {
                folder_path = this.sysRoot + "\\" + xmlConfigFoderName;
            }

            if (!Directory.Exists(folder_path))
            {
                try
                {
                    Directory.CreateDirectory(folder_path);
                }
                catch (Exception)
                {
                    AJTTxMessageHandling aJTTxMessageHandling = new AJTTxMessageHandling();
                    aJTTxMessageHandling.AddWarning("AutoJTConfiguration Usage Error", "AJT_Tools folder on Sys root does not exist, Failed to create AJT_Tools folder.\nPlease contact Administrator.");
                    aJTTxMessageHandling.ShowMessages("AutoJT");
                }
            }

            return folder_path;
        }

        /// <summary>
        /// 检查公用xml并创建公用xml
        /// </summary>
        /// <param name="xml_fullPath"></param>
        void CheckXMLANDCreate(string xml_fullPath)
        {
            //判断文件是否存在
            if (File.Exists(xml_fullPath))
            {
                //文件存在
                //判断最顶层节点是否存在
                XmlFiles xmlFiles1 = null;
                System.Xml.XmlNode root_node = null;

                try
                {
                    xmlFiles1 = new XmlFiles(xml_fullPath);
                    root_node = xmlFiles1.FindNode("/AutoJTConfiguration");
                }
                catch { }


                //没有root node
                if (xmlFiles1 == null || root_node == null)
                {
                    //删除文件, 重新创建
                    System.IO.File.Delete(xml_fullPath);

                    //创建xml文件, 并写入根节点
                    CreateXMLFileWriteRootNode(xml_fullPath);
                }
            }
            else
            {
                //无此文件
                //创建xml文件, 并写入根节点
                CreateXMLFileWriteRootNode(xml_fullPath);
            }
        }

        /// <summary>
        /// 创建xml文件, 并写入根节点
        /// </summary>
        /// <param name="xml_fullPath"></param>
        void CreateXMLFileWriteRootNode(string xml_fullPath)
        {
            FileStream fs = null;
            StreamWriter sw = null;
            try
            {
                fs = new FileStream(xml_fullPath, FileMode.Create);
                sw = new StreamWriter(fs, System.Text.Encoding.Default);

                sw.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                sw.Write("\r\n<AutoJTConfiguration>\r\n");

                sw.Write("</AutoJTConfiguration>");

            }
            catch (Exception)
            {
                AJTTxMessageHandling aJTTxMessageHandling = new AJTTxMessageHandling();
                aJTTxMessageHandling.AddWarning("AutoJTConfiguration Usage Error", "AutoJTConfiguration.xml on Sys root does not exist, Failed to create AutoJTConfiguration.xml.\nPlease contact Administrator.");
                aJTTxMessageHandling.ShowMessages("AutoJT");
            }
            finally
            {
                sw.Dispose();
                sw.Close();

                fs.Dispose();
                fs.Close();
            }
        }


        #endregion

        #region 静态方法

        /// <summary>
        /// 检查xml配置文件夹是否存在并创建文件夹
        /// </summary>
        /// <returns></returns>
        public static string CheckXMLConfigFolder(string _m_sysRoot)
        {
            string folder_path = "";

            if (_m_sysRoot.EndsWith("\\"))
            {
                folder_path = _m_sysRoot + xmlConfigFoderName;
            }

            else
            {
                folder_path = _m_sysRoot + "\\" + xmlConfigFoderName;
            }

            if (!Directory.Exists(folder_path))
            {
                try
                {
                    Directory.CreateDirectory(folder_path);
                }
                catch (Exception)
                {
                    AJTTxMessageHandling aJTTxMessageHandling = new AJTTxMessageHandling();
                    aJTTxMessageHandling.AddWarning("AutoJTConfiguration Usage Error", "AJT_Tools folder on Sys root does not exist, Failed to create AJT_Tools folder.\nPlease contact Administrator.");
                    aJTTxMessageHandling.ShowMessages("AutoJT");
                }
            }

            return folder_path;
        }

        #endregion


        #endregion

        #region [13.AJTDataAdministration]
        //用于 13.AJTDataAdministration

        //加载
        public StringCollection LoadConfig_AJTDataAdministration(string sysRoot, string projectName, string config_1_Name,
            string config_2_Name, out bool isCreateDateFolder)
        {
            StringCollection strColl = new StringCollection();

            //xml
            XmlFiles xmlFiles = null;
            //config2
            isCreateDateFolder = false;

            try
            {
                //xml doc
                xmlFiles = new XmlFiles(this.xml_fullPath);
                //root                
                XmlElement root = xmlFiles.DocumentElement;

                //当前项目
                XmlNode currnetProject = root.SelectSingleNode(projectName);

                #region 配置1

                if (currnetProject == null)
                {
                    strColl = null;
                }
                else
                {
                    //当前配置1
                    XmlNode currnetConfig = currnetProject.SelectSingleNode(config_1_Name);
                    if (currnetConfig == null)
                    {
                        strColl = null;
                    }
                    else
                    {
                        //获取当前配置下所有的子节点
                        XmlNodeList xmlNodeList = currnetConfig.ChildNodes;
                        if (xmlNodeList == null)
                        {
                            strColl = null;
                        }
                        else
                        {
                            if (xmlNodeList != null)
                            {
                                foreach (XmlNode item in xmlNodeList)
                                {
                                    string fixturePath = item.Attributes[1].Value;
                                    if (fixturePath.StartsWith("//") || fixturePath.StartsWith("\\"))
                                    {
                                        fixturePath = fixturePath.Substring(1);
                                    }

                                    fixturePath = System.IO.Path.Combine(sysRoot, fixturePath);

                                    strColl.Add(item.Attributes[0].Value + "|" + fixturePath);
                                }
                            }
                        }
                    }
                }


                #endregion

                #region 配置2

                //当前项目
                if (currnetProject == null)
                {
                    isCreateDateFolder = false;
                }
                else
                {
                    //当前配置2
                    XmlNode currnetConfig_2 = currnetProject.SelectSingleNode(config_2_Name);
                    if (currnetConfig_2 == null)
                    {
                        isCreateDateFolder = false;
                    }
                    else
                    {
                        //获取当前配置2的属性
                        XmlAttribute xmlAttribute_2 = currnetConfig_2.Attributes[0];
                        bool bl1 = bool.TryParse(xmlAttribute_2.Value, out bool bl2);
                        if (bl1)
                        {
                            isCreateDateFolder = bl2;
                        }
                        else
                        {
                            isCreateDateFolder = false;
                        }
                    }
                }

                #endregion
            }
            catch (Exception)
            {
                AJTTxMessageHandling aJTTxMessageHandling = new AJTTxMessageHandling();
                aJTTxMessageHandling.AddWarning("AutoJTConfiguration Usage Error", "AutoJTConfiguration.xml on Sys project does not exist, Failed to load AutoJTConfiguration.xml.\nPlease contact Administrator.");
                aJTTxMessageHandling.ShowMessages("AutoJT");
            }

            return strColl;
        }

        //写入
        public void WriteConfig_AJTDataAdministration(string projectName, string config_1_Name, StringCollection strColl,
            string config_2_Name, bool isCreateDateFolder)
        {
            XmlFiles xmlFiles = null;
            try
            {
                //xml doc
                xmlFiles = new XmlFiles(this.xml_fullPath);
                //root                
                XmlElement root = xmlFiles.DocumentElement;


                //当前项目
                XmlElement xmlNode_project = (XmlElement)root.SelectSingleNode(projectName);
                //当前项目不存在
                if (xmlNode_project == null)
                {
                    //创建当前项目的节点                    
                    xmlNode_project = (XmlElement)root.AppendChild(xmlFiles.CreateElement(projectName));
                }

                #region 配置1

                //当前配置项
                XmlElement xmlNode_config = (XmlElement)xmlNode_project.SelectSingleNode(config_1_Name);
                //当前配置不存在
                if (xmlNode_config == null)
                {
                    //创建当前配置的节点                    
                    xmlNode_config = (XmlElement)xmlNode_project.AppendChild(xmlFiles.CreateElement(config_1_Name));
                }
                else
                {
                    //delete当前配置下所有的子节点
                    xmlNode_config.RemoveAll();
                }

                if (strColl == null || strColl.Count == 0)
                {
                    //清除当前配置项
                    //delete当前配置下所有的子节点
                    xmlNode_config.RemoveAll();
                }
                else
                {
                    //添加当前配置的新配置
                    for (int i = 0; i < strColl.Count; i++)
                    {
                        string[] config1 = strColl[i].Split('|');

                        XmlElement fixture_Node = (XmlElement)xmlNode_config.AppendChild(xmlFiles.CreateElement("Fixture"));

                        //节点的属性
                        XmlAttribute attriType = xmlFiles.CreateAttribute("Type");
                        attriType.InnerText = config1[0];
                        XmlAttribute attriFullPath = xmlFiles.CreateAttribute("FullPath");
                        attriFullPath.InnerText = config1[1];

                        //添加属性
                        fixture_Node.SetAttributeNode(attriType);
                        fixture_Node.SetAttributeNode(attriFullPath);
                    }
                }

                #endregion

                #region 配置2

                //当前配置项 2
                XmlElement xmlNode_config_2 = (XmlElement)xmlNode_project.SelectSingleNode(config_2_Name);
                //当前配置不存在
                if (xmlNode_config_2 == null)
                {
                    //创建当前配置的节点                    
                    xmlNode_config_2 = (XmlElement)xmlNode_project.AppendChild(xmlFiles.CreateElement(config_2_Name));
                }
                else
                {
                    //delete当前配置下所有的attributes
                    xmlNode_config_2.RemoveAllAttributes();
                }

                //节点的属性
                XmlAttribute attriisCrateFolder = xmlFiles.CreateAttribute("isCreateDateFolder");
                attriisCrateFolder.InnerText = isCreateDateFolder.ToString();

                //添加属性
                xmlNode_config_2.SetAttributeNode(attriisCrateFolder);

                #endregion
            }
            catch (Exception)
            {
                AJTTxMessageHandling aJTTxMessageHandling = new AJTTxMessageHandling();
                aJTTxMessageHandling.AddWarning("AutoJTConfiguration Usage Error", "AutoJTConfiguration.xml on Sys project does not exist, Failed to write AutoJTConfiguration.xml.\nPlease contact Administrator.");
                aJTTxMessageHandling.ShowMessages("AutoJT");
            }
            finally
            {
                xmlFiles.Save(this.xml_fullPath);
            }
        }

        #endregion [13.AJTDataAdministration]
    }
}
