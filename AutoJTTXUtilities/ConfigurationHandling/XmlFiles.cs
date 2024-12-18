using System;
using System.Threading.Tasks;
using System.Xml;

namespace AutoJTTXUtilities.ConfigurationHandling
{
    /// <summary>
    /// XmlFiles 的摘要说明。
    /// </summary>
    public class XmlFiles : XmlDocument
    {
        #region 字段与属性
        private string _xmlFileName;
        public string XmlFileName
        {
            set { _xmlFileName = value; }
            get { return _xmlFileName; }
        }
        #endregion

        public XmlFiles(string xmlFile)
        {
            XmlFileName = xmlFile;

            this.Load(xmlFile);
        }
        /// <summary>
        /// 给定一个节点的xPath表达式并返回一个节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public XmlNode FindNode(string xPath)
        {
            XmlNode xmlNode = this.SelectSingleNode(xPath);
            return xmlNode;
        }
        /// <summary>
        /// 给定一个节点的xPath表达式返回其值
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public string GetNodeValue(string xPath)
        {
            XmlNode xmlNode = this.SelectSingleNode(xPath);
            return xmlNode.InnerText;
        }
        /// <summary>
        /// 给定一个节点的表达式返回此节点下的孩子节点列表
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public XmlNodeList GetNodeList(string xPath)
        {
            XmlNodeList nodeList = this.SelectSingleNode(xPath).ChildNodes;
            return nodeList;
        }

        #region 静态方法

        //操作 Tune.exe.config
        /*
            
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <runtime>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <assemblyIdentity name="Microsoft.Bcl.AsyncInterfaces" publicKeyToken="cc7b13ffcd2ddd51" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-8.0.0.0" newVersion="8.0.0.0" />
      </dependentAssembly>
    </assemblyBinding>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <assemblyIdentity name="System.Buffers" publicKeyToken="cc7b13ffcd2ddd51" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-4.0.3.0" newVersion="4.0.3.0" />
      </dependentAssembly>
    </assemblyBinding>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <assemblyIdentity name="System.ComponentModel.Annotations" publicKeyToken="b03f5f7f11d50a3a" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-4.2.1.0" newVersion="4.2.1.0" />
      </dependentAssembly>
    </assemblyBinding>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <assemblyIdentity name="System.Runtime.CompilerServices.Unsafe" publicKeyToken="b03f5f7f11d50a3a" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-6.0.0.0" newVersion="6.0.0.0" />
      </dependentAssembly>
    </assemblyBinding>
  </runtime>
</configuration>

         */

        //添加node
        public static Task<bool> TunExeCfgEditer(string cfgPath)
        {
            Task<bool> task = null;

            task = Task.Run(() =>
            {
                //添加节点成功
                bool result1 = false;
                //保存xml成功
                bool result2 = false;

                if (string.IsNullOrEmpty(cfgPath))
                {
                    return false;
                }

                //查询runtime是否存在
                //查询 /configuration/runtime/assemblyBinding/dependentAssembly/assemblyIdentity/@name="Microsoft.Bcl.AsyncInterfaces"
                //查询 /configuration/runtime/assemblyBinding/dependentAssembly/assemblyIdentity/@name="System.Buffers"
                //查询 /configuration/runtime/assemblyBinding/dependentAssembly/assemblyIdentity/@name="System.ComponentModel.Annotations"
                //查询 /configuration/runtime/assemblyBinding/dependentAssembly/assemblyIdentity/@name="System.Runtime.CompilerServices.Unsafe"

                //xml doc
                XmlFiles xmlFiles = new XmlFiles(cfgPath);
                //根节点
                XmlElement documentElement = xmlFiles.DocumentElement;

                try
                {
                    //获取runtime节点
                    XmlNode xmlNode_runtime = documentElement.SelectSingleNode("runtime");

                    if (xmlNode_runtime == null)
                    {
                        //创建runtime节点
                        documentElement.AppendChild(CreateNodes(xmlFiles));
                        result1 = true;
                    }
                    else
                    {
                        //定义命名空间
                        XmlNamespaceManager m = new XmlNamespaceManager(xmlFiles.NameTable);
                        m.AddNamespace("nhb", "urn:schemas-microsoft-com:asm.v1");

                        //检查节点并创建
                        XmlNode xmlNode1 = xmlNode_runtime.SelectSingleNode("//nhb:assemblyIdentity[@name='Microsoft.Bcl.AsyncInterfaces']", m);
                        if (xmlNode1 is null)
                        {
                            //创建节点并添加
                            xmlNode_runtime.AppendChild(CreateAssemblyBindingMethod(xmlFiles, "Microsoft.Bcl.AsyncInterfaces", "cc7b13ffcd2ddd51", "0.0.0.0-8.0.0.0", "8.0.0.0"));
                        }

                        XmlNode xmlNode2 = xmlNode_runtime.SelectSingleNode("//nhb:assemblyIdentity[@name='System.Buffers']", m);
                        if (xmlNode2 is null)
                        {
                            //创建节点并添加
                            xmlNode_runtime.AppendChild(CreateAssemblyBindingMethod(xmlFiles, "System.Buffers", "cc7b13ffcd2ddd51", "0.0.0.0-4.0.3.0", "4.0.3.0"));
                        }

                        XmlNode xmlNode3 = xmlNode_runtime.SelectSingleNode("//nhb:assemblyIdentity[@name='System.ComponentModel.Annotations']", m);
                        if (xmlNode3 is null)
                        {
                            //创建节点并添加
                            xmlNode_runtime.AppendChild(CreateAssemblyBindingMethod(xmlFiles, "System.ComponentModel.Annotations", "b03f5f7f11d50a3a", "0.0.0.0-4.2.1.0", "4.2.1.0"));
                        }

                        XmlNode xmlNode4 = xmlNode_runtime.SelectSingleNode("//nhb:assemblyIdentity[@name='System.Runtime.CompilerServices.Unsafe']", m);
                        if (xmlNode4 is null)
                        {
                            //创建节点并添加
                            xmlNode_runtime.AppendChild(CreateAssemblyBindingMethod(xmlFiles, "System.Runtime.CompilerServices.Unsafe", "b03f5f7f11d50a3a", "0.0.0.0-6.0.0.0", "6.0.0.0"));
                        }

                        result1 = true;
                    }
                }
                catch (Exception ex)
                {
                    documentElement = null;
                    xmlFiles = null;

                    //添加节点失败
                    result1 = false;

                    throw ex;
                }
                finally
                {
                    try
                    {
                        xmlFiles.Save(cfgPath);
                        //保存xml成功
                        result2 = true;
                    }
                    catch (System.Exception ex)
                    {
                        //保存xml失败
                        result2 = false;
                        //权限不足
                        if (ex.GetType() == typeof(System.UnauthorizedAccessException))
                        {
                            throw new UnauthorizedAccessException("请重新安装插件");
                        }
                    }
                    documentElement = null;
                    xmlFiles = null;
                }

                return result1 && result2;
            });

            return task;
        }

        //检查node是否完整 
        public static Task<bool> CheckTunExeCfgComplete(string cfgPath)
        {
            Task<bool> task = null;

            task = Task.Run(() =>
            {
                if (string.IsNullOrEmpty(cfgPath))
                {
                    return false;
                }

                bool result = false;

                //查询runtime是否存在
                //查询 /configuration/runtime/assemblyBinding/dependentAssembly/assemblyIdentity/@name="Microsoft.Bcl.AsyncInterfaces"
                //查询 /configuration/runtime/assemblyBinding/dependentAssembly/assemblyIdentity/@name="System.Buffers"
                //查询 /configuration/runtime/assemblyBinding/dependentAssembly/assemblyIdentity/@name="System.ComponentModel.Annotations"
                //查询 /configuration/runtime/assemblyBinding/dependentAssembly/assemblyIdentity/@name="System.Runtime.CompilerServices.Unsafe"

                //xml doc
                XmlFiles xmlFiles = new XmlFiles(cfgPath);
                //根节点
                XmlElement documentElement = xmlFiles.DocumentElement;

                try
                {
                    //获取runtime节点
                    XmlNode xmlNode_runtime = documentElement.SelectSingleNode("runtime");

                    if (xmlNode_runtime == null)
                    {
                        return false;
                    }
                    else
                    {
                        //定义命名空间
                        XmlNamespaceManager m = new XmlNamespaceManager(xmlFiles.NameTable);
                        m.AddNamespace("nhb", "urn:schemas-microsoft-com:asm.v1");

                        //检查节点并创建
                        XmlNode xmlNode1 = xmlNode_runtime.SelectSingleNode("//nhb:assemblyIdentity[@name='Microsoft.Bcl.AsyncInterfaces']", m);
                        if (xmlNode1 is null)
                        {
                            return false;
                        }

                        XmlNode xmlNode2 = xmlNode_runtime.SelectSingleNode("//nhb:assemblyIdentity[@name='System.Buffers']", m);
                        if (xmlNode2 is null)
                        {
                            return false;
                        }

                        XmlNode xmlNode3 = xmlNode_runtime.SelectSingleNode("//nhb:assemblyIdentity[@name='System.ComponentModel.Annotations']", m);
                        if (xmlNode3 is null)
                        {
                            return false;
                        }

                        XmlNode xmlNode4 = xmlNode_runtime.SelectSingleNode("//nhb:assemblyIdentity[@name='System.Runtime.CompilerServices.Unsafe']", m);
                        if (xmlNode4 is null)
                        {
                            return false;
                        }

                        result = true;
                    }
                }
                catch
                {
                    documentElement = null;
                    xmlFiles = null;

                    return false;
                }
                finally
                {
                    documentElement = null;
                    xmlFiles = null;
                }

                return result;
            });

            return task;
        }

        static XmlElement CreateNodes(XmlFiles xmlFiles)
        {
            XmlElement xmlElement_0 = xmlFiles.CreateElement("runtime");

            //assemblyBinding
            XmlElement xmlElement_1 = CreateAssemblyBindingMethod(xmlFiles, "Microsoft.Bcl.AsyncInterfaces", "cc7b13ffcd2ddd51", "0.0.0.0-8.0.0.0", "8.0.0.0");
            XmlElement xmlElement_2 = CreateAssemblyBindingMethod(xmlFiles, "System.Buffers", "cc7b13ffcd2ddd51", "0.0.0.0-4.0.3.0", "4.0.3.0");
            XmlElement xmlElement_3 = CreateAssemblyBindingMethod(xmlFiles, "System.ComponentModel.Annotations", "b03f5f7f11d50a3a", "0.0.0.0-4.2.1.0", "4.2.1.0");
            XmlElement xmlElement_4 = CreateAssemblyBindingMethod(xmlFiles, "System.Runtime.CompilerServices.Unsafe", "b03f5f7f11d50a3a", "0.0.0.0-6.0.0.0", "6.0.0.0");

            xmlElement_0.AppendChild(xmlElement_1);
            xmlElement_0.AppendChild(xmlElement_2);
            xmlElement_0.AppendChild(xmlElement_3);
            xmlElement_0.AppendChild(xmlElement_4);

            return xmlElement_0;
        }

        static XmlElement CreateAssemblyBindingMethod(XmlFiles xmlFiles, string name, string token, string oldVer, string newVer)
        {
            XmlElement xmlElement_1 = xmlFiles.CreateElement("assemblyBinding");
            XmlAttribute attrixmlns = xmlFiles.CreateAttribute("xmlns");
            attrixmlns.InnerText = "urn:schemas-microsoft-com:asm.v1";
            xmlElement_1.SetAttributeNode(attrixmlns);

            XmlElement xmlElement_2 = xmlFiles.CreateElement("dependentAssembly");

            XmlElement xmlElement_3 = xmlFiles.CreateElement("assemblyIdentity");
            XmlAttribute attriName = xmlFiles.CreateAttribute("name");
            XmlAttribute attripublicKeyToken = xmlFiles.CreateAttribute("publicKeyToken");
            XmlAttribute attriculture = xmlFiles.CreateAttribute("culture");

            attriName.InnerText = name;
            attripublicKeyToken.InnerText = token;
            attriculture.InnerText = "neutral";
            xmlElement_3.SetAttributeNode(attriName);
            xmlElement_3.SetAttributeNode(attripublicKeyToken);
            xmlElement_3.SetAttributeNode(attriculture);

            XmlElement xmlElement_4 = xmlFiles.CreateElement("bindingRedirect");
            XmlAttribute attrioldVersion = xmlFiles.CreateAttribute("oldVersion");
            XmlAttribute attrinewVersion = xmlFiles.CreateAttribute("newVersion");
            attrioldVersion.InnerText = oldVer;
            attrinewVersion.InnerText = newVer;
            xmlElement_4.SetAttributeNode(attrioldVersion);
            xmlElement_4.SetAttributeNode(attrinewVersion);

            xmlElement_2.AppendChild(xmlElement_3);
            xmlElement_2.AppendChild(xmlElement_4);
            xmlElement_1.AppendChild(xmlElement_2);
            return xmlElement_1;
        }

        #endregion
    }
}
