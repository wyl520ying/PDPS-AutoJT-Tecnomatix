using AutoJTTXCoreUtilities.AJTeMSUtils;
using EMPAPPBASELib;
using EMPAPPCLIENTMANAGERLib;
using EMPCLIENTCOMMANDSLib;
using EMPCORELib;
using EMPINTEGRATIONLib;
using EMPMODELLib;
using EMPTYPELIBRARYLib;
using EngineeringInternalExtension;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Tecnomatix.Engineering;
using Tecnomatix.Planning;
using Tecnomatix.Planning.DotNetFoundation;

namespace AutoJTTXCoreUtilities
{
    public class AJTeMServer
    {
        [Serializable]
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private class MyUNC
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string UniversalName;

            [MarshalAs(UnmanagedType.LPStr)]
            public string ConnectionName;

            [MarshalAs(UnmanagedType.LPStr)]
            public string RemainingPath;
        }









        public static EmpContext ServerLogin(string appName, Version appVersion, string user, string password, out string error)
        {
            error = "";
            EmpContext empContext = default(EmpContext);
            empContext.sessionId = 0;
            EmpContext result;
            if (!string.IsNullOrEmpty(user))
            {
                if (password == null || password.Length == 0)
                {
                    password = "";
                }
                error = "Unkown error!";
                try
                {
                    EmpBase empBase = new EmpBaseClass();
                    empContext = empBase.EmpLogin(user, password, "", Environment.MachineName, appName, appVersion.ToString(4), Environment.UserName);
                    EmpAppClientManager empAppClientManager = new EmpAppClientManagerClass();
                    IEmpAppClientManager empAppClientManager2 = empAppClientManager;
                    EmpContext empContext2 = empContext;
                    empAppClientManager2.OpenSession(ref empContext2);
                }
                catch (COMException e)
                {
                    error = AJTeMServer.CreateErrorMessage(e);
                }
                catch (Exception ex)
                {
                    error = "Login failed, please check the username and password. " + ex.Message;
                    empContext.sessionId = 0;
                    return empContext;
                }
                if (empContext.sessionId <= 0)
                {
                    error = "Login failed, please check the username and password. Error: " + error;
                    empContext.sessionId = 0;
                    result = empContext;
                }
                else
                {
                    result = empContext;
                }
            }
            else
            {
                error = "No user given.";
                result = empContext;
            }
            return result;
        }

        public static bool OpenProject(EmpContext context, string projectID, bool useInternalID, out string error)
        {
            error = "Unknown error!";
            EmpProjectData[] projectsFromDatabase = AJTeMServer.GetProjectsFromDatabase(context, out error);
            bool result;
            if (projectsFromDatabase == null)
            {
                result = false;
            }
            else if (projectsFromDatabase.Length != 0)
            {
                for (int i = 0; i < projectsFromDatabase.Length; i++)
                {
                    try
                    {
                        EmpProject empProject = new EmpProjectClass();
                        EmpObjectKey empObjectKey = default(EmpObjectKey);
                        EmpProjectManager empProjectManager = new EmpProjectManagerClass();
                        if (useInternalID)
                        {
                            if (projectsFromDatabase[i].Project.objectId.ToString() == projectID)
                            {
                                empProjectManager.OpenProject(ref context, ref projectsFromDatabase[i].Project);
                                return true;
                            }
                        }
                        else
                        {
                            empProjectManager.OpenProject(ref context, ref projectsFromDatabase[i].Project);
                            empObjectKey = empProject.GetNodeByExternalID(ref context, projectID);
                            if (projectsFromDatabase[i].Project.objectId == empObjectKey.objectId)
                            {
                                return true;
                            }
                            empProjectManager.CloseProject(ref context);
                        }
                    }
                    catch
                    {
                    }
                }
                if (!useInternalID)
                {
                    error = "Can't find project with the ExternalID " + projectID + " in the database.";
                }
                else
                {
                    error = "Can't find project with the Internal ID " + projectID + " in the database.";
                }
                result = false;
            }
            else
            {
                error = "No projects in the database.";
                result = false;
            }
            return result;
        }

        public static bool OpenProject(EmpContext context, string projectID, bool useInternalID = false)
        {
            string text = "Unknown error!";
            EmpProjectData[] projectsFromDatabase = AJTeMServer.GetProjectsFromDatabase(context, out text);
            bool result;
            if (projectsFromDatabase != null)
            {
                if (projectsFromDatabase.Length != 0)
                {
                    for (int i = 0; i < projectsFromDatabase.Length; i++)
                    {
                        try
                        {
                            EmpProject empProject = new EmpProjectClass();
                            EmpObjectKey empObjectKey = default(EmpObjectKey);
                            EmpProjectManager empProjectManager = new EmpProjectManagerClass();
                            if (useInternalID)
                            {
                                if (projectsFromDatabase[i].Project.objectId.ToString() == projectID)
                                {
                                    empProjectManager.OpenProject(ref context, ref projectsFromDatabase[i].Project);
                                    return true;
                                }
                            }
                            else
                            {
                                empProjectManager.OpenProject(ref context, ref projectsFromDatabase[i].Project);
                                empObjectKey = empProject.GetNodeByExternalID(ref context, projectID);
                                if (projectsFromDatabase[i].Project.objectId == empObjectKey.objectId)
                                {
                                    return true;
                                }
                                empProjectManager.CloseProject(ref context);
                            }
                        }
                        catch
                        {
                        }
                    }
                    text = "Can't find project with the ExternalID " + projectID + " in the database.";
                    result = false;
                }
                else
                {
                    text = "No projects in the database.";
                    result = false;
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        public static bool CloseProject(EmpContext context, out string error)
        {
            error = "Unknown error!";
            bool result;
            if (context.sessionId != 0)
            {
                try
                {
                    EmpProjectManager empProjectManager = new EmpProjectManagerClass();
                    empProjectManager.CloseProject(ref context);
                    return true;
                }
                catch (COMException e)
                {
                    error = "Can't close project. " + AJTeMServer.CreateErrorMessage(e);
                }
                catch (Exception ex)
                {
                    error = "Can't close project. " + ex.Message;
                }
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }

        public static EmpProjectData[] GetProjectsFromDatabase(EmpContext context, out string error)
        {
            EmpProjectData[] array = null;
            EmpProjectManager empProjectManager = new EmpProjectManagerClass();
            error = "Unknown error!";
            EmpProjectData[] result;
            if (context.sessionId != 0)
            {
                try
                {
                    array = empProjectManager.GetProjects(ref context);
                }
                catch (COMException e)
                {
                    error = AJTeMServer.CreateErrorMessage(e);
                    return null;
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    return null;
                }
                result = array;
            }
            else
            {
                error = "No context given (Session ID)";
                result = null;
            }
            return result;
        }

        public static bool ServerLogin(string appName, string appVersion, string loginName, bool showDialog, out string error)
        {
            error = "";
            EmpLoginCmd empLoginCmd = new EmpLoginCmdClass();
            EmpLoginCmdParams empLoginCmdParams = new EmpLoginCmdParamsClass();
            if (!string.IsNullOrEmpty(appName))
            {
                empLoginCmdParams.AppName = appName;
            }
            if (!string.IsNullOrEmpty(appVersion))
            {
                empLoginCmdParams.AppVersion = appVersion;
            }
            if (!string.IsNullOrEmpty(loginName))
            {
                empLoginCmdParams.LoginName = loginName;
            }
            empLoginCmdParams.ShowDialog = showDialog;
            empLoginCmdParams.Password = "";
            try
            {
                empLoginCmd.Execute(empLoginCmdParams);
            }
            catch (COMException ex)
            {
                int num = -2147220731;
                if (ex.ErrorCode != -2147220722 && ex.ErrorCode != num)
                {
                    error = "Login failed! " + ex.Message;
                }
                return false;
            }
            return true;
        }

        public static bool ServerLogin(string appName, string appVersion, string loginName, string loginPassword, out string error)
        {
            error = "";
            EmpLoginCmd empLoginCmd = new EmpLoginCmdClass();
            EmpLoginCmdParams empLoginCmdParams = new EmpLoginCmdParamsClass();
            if (!string.IsNullOrEmpty(appName))
            {
                empLoginCmdParams.AppName = appName;
            }
            if (!string.IsNullOrEmpty(appVersion))
            {
                empLoginCmdParams.AppVersion = appVersion;
            }
            if (!string.IsNullOrEmpty(loginName))
            {
                empLoginCmdParams.LoginName = loginName;
            }
            empLoginCmdParams.ShowDialog = false;
            empLoginCmdParams.Password = loginPassword;
            try
            {
                empLoginCmd.Execute(empLoginCmdParams);
            }
            catch (COMException ex)
            {
                int num = -2147220731;
                if (ex.ErrorCode != -2147220722 && ex.ErrorCode != num)
                {
                    error = "Login failed! " + ex.Message;
                }
                return false;
            }
            return true;
        }

        public static bool ServerLogout(bool showDialog, out string error)
        {
            error = "";
            try
            {
                EmpLogoutCmd empLogoutCmd = new EmpLogoutCmdClass();
                empLogoutCmd.Execute(new EmpLogoutCmdParamsClass
                {
                    ShowDialog = showDialog
                });
            }
            catch (COMException e)
            {
                error = "Logout failed! " + AJTeMServer.CreateErrorMessage(e);
                return false;
            }
            catch (Exception ex)
            {
                error = "Logout failed! " + ex.Message;
                return false;
            }
            return true;
        }

        public static void ServerLogout(EmpContext context)
        {
            if (context.sessionId > 0)
            {
                try
                {
                    EmpAppClientManager empAppClientManager = new EmpAppClientManagerClass();
                    empAppClientManager.CloseSession();
                }
                catch (COMException e)
                {
                    AJTeMServer.CreateErrorMessage(e);
                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                }
            }
        }

        public static EmpOpenProjectCmd OpenProject(string appName, string appVersion, string loginName, out string error)
        {
            error = "";
            EmpOpenProjectCmd empOpenProjectCmd = new EmpOpenProjectCmdClass();
            EmpOpenProjectCmdParams empOpenProjectCmdParams = new EmpOpenProjectCmdParamsClass();
            empOpenProjectCmdParams.ShowDialog = true;
            try
            {
                empOpenProjectCmd.Execute(empOpenProjectCmdParams);
            }
            catch (COMException ex)
            {
                int num = -2147220731;
                if (ex.ErrorCode != -2147220722 && ex.ErrorCode != num)
                {
                    error = "Open project failed! " + ex.Message;
                }
                return null;
            }
            return empOpenProjectCmd;
        }

        public static EmpContext GetContext(out string error)
        {
            EmpContext result;
            try
            {
                EmpAppClientManager empAppClientManager = new EmpAppClientManagerClass();
                EmpContext context = empAppClientManager.Context;
                error = "";
                result = context;
            }
            catch (Exception ex)
            {
                error = "Error on getting Context: " + ex.Message;
                result = default(EmpContext);
            }
            return result;
        }

        public static EmpProjectData[] GetProjects(ref EmpContext context, out string error)
        {
            EmpProjectData[] result;
            try
            {
                error = "";
                EmpProjectManager empProjectManager = new EmpProjectManagerClass();
                EmpProjectData[] projects = empProjectManager.GetProjects(ref context);
                result = projects;
            }
            catch (Exception ex)
            {
                error = "Error on getting Projects: " + ex.Message;
                result = new EmpProjectData[0];
            }
            return result;
        }

        public static string CreateErrorMessage(EmpExecutionInfo executionInfo)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < executionInfo.InfoItems.Size; i++)
            {
                EmpObjectKey key = executionInfo.InfoItems.Item(i).get_Key();
                if (key.objectId != 0)
                {
                    stringBuilder.Append(key.objectId.ToString());
                    stringBuilder.Append(" : ");
                }
                EmpInfoItem empInfoItem = executionInfo.InfoItems.Item(i) as EmpInfoItem;
                stringBuilder.Append(empInfoItem.Description);
                stringBuilder.Append(Environment.NewLine);
            }
            return stringBuilder.ToString();
        }

        public static string CreateErrorMessage(COMException e)
        {
            EmpErrorService empErrorService = new EmpErrorServiceClass();
            EmpExecutionInfo xmlinfo = empErrorService.GetXMLInfo(e.Message);
            return AJTeMServer.CreateErrorMessage(xmlinfo);
        }

        public static int GetExceptionCode(COMException comEx)
        {
            int result;
            try
            {
                int num;
                if (!int.TryParse(((IEmpInfoItem)new EmpErrorServiceClass().GetXMLInfo(comEx.Message).InfoItems.Item(0)).InfoId, out num))
                {
                    result = 0;
                }
                else
                {
                    result = num;
                }
            }
            catch
            {
                result = 0;
            }
            return result;
        }

        public static string GetExceptionDescription_2(Exception ex)
        {
            string result;
            try
            {
                result = ((IEmpInfoItem)new EmpErrorServiceClass().GetXMLInfo(ex.Message).InfoItems.Item(0)).Description;
            }
            catch (Exception ex2)
            {
                result = string.Concat(new string[]
                {
                    "Internal Error '",
                    ex2.ToString(),
                    "' while retrieving error description for '",
                    ex.Message,
                    "'."
                });
            }
            return result;
        }

        public static string CreateNewExternalId(string suffix = "PP-")
        {
            return suffix + Guid.NewGuid().ToString();
        }

        public static EmpObjectKey[] GetChildren(ref EmpContext context, ref EmpObjectKey parent)
        {
            EmpObjectKey[] array = null;
            int[] array2 = new EmpNodeClass().GetField(ref context, ref parent, "children").Value as int[];
            int num = array2.Length;
            if (num > 0)
            {
                array = new EmpObjectKey[num];
                for (int i = 0; i < num; i++)
                {
                    array[i].objectId = array2[i];
                }
            }
            return array;
        }















        [DllImport("kernel32.dll")]
        private static extern uint QueryDosDevice(string lpDeviceName, StringBuilder lpTargetPath, int ucchMax);
        [DllImport("mpr.dll")]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern int WNetGetUniversalName(string lpLocalPath, [MarshalAs(UnmanagedType.U4)] int dwInfoLevel, IntPtr lpBuffer, [MarshalAs(UnmanagedType.U4)] ref int lpBufferSize);

        private static string WNetGetUniversalNameCSharp(string path)
        {
            int cb = 2000;
            IntPtr intPtr = Marshal.AllocHGlobal(cb);
            bool flag = WNetGetUniversalName(path, 2, intPtr, ref cb) != 0;
            path = "";
            if (!flag)
            {
                MyUNC myUNC = new MyUNC();
                Marshal.PtrToStructure(intPtr, myUNC);
                path = myUNC.UniversalName;
            }
            Marshal.FreeHGlobal(intPtr);
            return path;
        }

        protected static string GetPhysicalPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }
            string text = WNetGetUniversalNameCSharp(path);
            if (text.Length > 0)
            {
                return text;
            }
            string pathRoot = Path.GetPathRoot(path);
            if (string.IsNullOrEmpty(pathRoot))
            {
                return path;
            }
            string lpDeviceName = pathRoot.Replace("\\", "");
            StringBuilder stringBuilder = new StringBuilder(260);
            if (QueryDosDevice(lpDeviceName, stringBuilder, stringBuilder.Capacity) != 0U)
            {
                string result;
                if (stringBuilder.ToString().StartsWith("\\??\\"))
                {
                    string text2 = stringBuilder.ToString();
                    string path2;
                    if (text2.IndexOf("\\??\\UNC\\") == 0)
                    {
                        text2 = text2.Remove(0, "\\??\\UNC\\".Length);
                        path2 = "\\\\" + text2;
                    }
                    else
                    {
                        path2 = text2.Remove(0, "\\??\\".Length);
                    }
                    result = Path.Combine(path2, path.Replace(Path.GetPathRoot(path), ""));
                }
                else
                {
                    result = path;
                }
                return result;
            }
            return path;
        }

        /// <summary>
        /// 获取用户选中ITxPlanningObject的InternalId
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public static bool setTarget(ref int objectId)
        {
            bool result = false;
            TxObjectList planningItems = TxApplication.ActiveSelection.GetPlanningItems();
            if (planningItems != null && planningItems.Count > 0)
            {
                ITxObject txObject = planningItems[0];
                ITxPlanningObject txPlanningObject = txObject as ITxPlanningObject;
                if (txPlanningObject == null)
                {
                    ITxProcessModelObject txProcessModelObject = txObject as ITxProcessModelObject;
                    if (txProcessModelObject != null)
                    {
                        txPlanningObject = txProcessModelObject.PlanningRepresentation;
                    }
                }
                TxEmsServicesProvider txEmsServicesProvider = txPlanningObject.PlatformServicesProvider as TxEmsServicesProvider;
                if (txEmsServicesProvider != null)
                {
                    objectId = txEmsServicesProvider.InternalId;
                    result = true;
                }
            }
            return result;
        }

        public static void ImportCojtXml(string xmlFullPath)
        {
            //连接eMS
            eMSCommons eMSCommons_1 = new eMSCommons();

            ArrayList arrayList = null;
            try
            {
                if (TxApplication.PlatformType == TxPlatformType.EmServer)
                {
                    EmpImportCollection empImportCollection = new EmpImportCollectionClass();
                    int objectId = 0;
                    if (setTarget(ref objectId))
                    {
                        EmpObjectKey empObjectKey;
                        empObjectKey.objectId = objectId;
                        empImportCollection.set_Target(ref empObjectKey);
                    }
                    EmpContext context = eMSCommons_1.context;

                    XmlDocument xmlDocument = new XmlDocument();
                    try
                    {
                        XmlTextReader xmlTextReader = new XmlTextReader(GetPhysicalPath(xmlFullPath));
                        xmlDocument.Load(xmlTextReader);
                        xmlTextReader.Close();
                    }
                    catch (Exception)
                    {
                    }
                    XmlDocumentConvertor xmlDocumentConvertor = new XmlDocumentConvertor(xmlDocument);
                    empImportCollection.Format = 0;
                    empImportCollection.FullLog = true;
                    empImportCollection.ImportFromStream(ref context, xmlDocumentConvertor.ToStream(), Path.GetDirectoryName(xmlFullPath));

                }
                else if (TxApplication.PlatformType == TxPlatformType.Offline)//&& string.IsNullOrEmpty(xmlFilesSkipImport.Find((string x) => x == xmlFullPath)))
                {
                    string errorMessage = "";
                    TxSelection activeSelection = TxApplication.ActiveSelection;
                    ITxObject txObject = null;
                    TxObjectList allItems = activeSelection.GetAllItems();
                    if (allItems.Count == 1)
                    {
                        txObject = allItems[0];
                    }
                    try
                    {
                        TxEmsUtilities.AddImportedCadFilesObjectsXml(xmlFullPath, txObject);
                    }
                    catch
                    {
                        foreach (object obj in TxErrorStack.GetAllParameters("PMWXML_ATTEMPT_TO_CHANGE_OBJECT_TYPE"))
                        {
                            IList<string> list = (IList<string>)obj;
                            if (list != null)
                            {

                            }
                        }
                        throw;
                    }
                }
            }
            catch (COMException ex)
            {
                EmpExecutionInfo xmlinfo = new EmpErrorServiceClass().GetXMLInfo(ex.Message);
                if (ex.ErrorCode == -2147220723)
                {
                    arrayList = new ArrayList();
                    for (int i = 0; i < xmlinfo.InfoItems.Size; i++)
                    {
                        EmpInfoItem empInfoItem = xmlinfo.InfoItems.Item(i) as EmpInfoItem;
                        if (empInfoItem != null)
                        {
                            int objectId2 = empInfoItem.get_Key().objectId;
                            if (objectId2 != 0)
                            {
                                arrayList.Add(objectId2);
                            }
                        }
                    }
                }
                else
                {
                }
            }
            catch
            {
            }
        }








        //创建CreateThreeDRep
        public EmpObjectKey Create_CreateThreeDrep(EmpContext context_app, EmpObjectKey empObjectKey_new, string systemRootDirectory, string cojtFullName, out bool infos)
        {
            EmpObjectKey result;
            infos = false;

            //Drep的相对路径
            string text = cojtFullName.Replace(systemRootDirectory, "#\\").Replace("\\\\", "\\");

            try
            {
                EmpObjectKey empObjectKey2 = new EmpToolPrototypeClass().CreateThreeDRep(ref context_app, ref empObjectKey_new, text);

                int num2 = 0;
                while (!new EmpToolPrototypeClass().IsObjectExist(ref context_app, ref empObjectKey2))
                {
                    Thread.Sleep(100);
                    num2++;
                    if (num2 > 10)
                    {
                        infos = false;
                        empObjectKey_new.objectId = 0;
                        return empObjectKey_new;
                    }
                }

                infos = true;
            }
            catch
            {
                infos = false;
                empObjectKey_new.objectId = 0;
                return empObjectKey_new;
            }

            result = empObjectKey_new;

            return result;
        }

        //创建CreateThreeDRep Part
        public EmpObjectKey Create_CreateThreeDrep_Part(EmpContext context_app, EmpObjectKey empObjectKey_new, string systemRootDirectory, string cojtFullName, out bool infos)
        {
            EmpObjectKey result;
            infos = false;

            //Drep的相对路径
            string text = cojtFullName.Replace(systemRootDirectory, "#\\").Replace("\\\\", "\\");

            try
            {
                EmpObjectKey empObjectKey2 = new EmpPartPrototypeClass().CreateThreeDRep(ref context_app, ref empObjectKey_new, text);

                int num2 = 0;
                while (!new EmpPartPrototypeClass().IsObjectExist(ref context_app, ref empObjectKey2))
                {
                    Thread.Sleep(100);
                    num2++;
                    if (num2 > 10)
                    {
                        infos = false;
                        empObjectKey_new.objectId = 0;
                        return empObjectKey_new;
                    }
                }

                infos = true;
            }
            catch
            {
                infos = false;
                empObjectKey_new.objectId = 0;
                return empObjectKey_new;
            }

            result = empObjectKey_new;

            return result;
        }

        /// <summary>
        /// 将指定的TuneData.xml导入当前项目, 并创建CreateThreeDRep
        /// </summary>
        /// <param name="_PlanningResourceLibrary_selected"></param>
        /// <param name="_Component_Name"></param>
        /// <param name="_FullDirectoryPath"></param>
        /// <param name="_ObjectPrototypeClass"></param>
        /// <param name="_PrototypeExternalId"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ImportTuneDataCreateThreeDRep(ITxPlanningObject _PlanningResourceLibrary_selected, string _Component_Name, string _FullDirectoryPath, string _ObjectPrototypeClass, string _PrototypeExternalId, out string error)
        {
            bool result = false;
            error = string.Empty;

            //连接eMS
            eMSCommons eMSCommons_1 = new eMSCommons();

            //检查用户选中的节点的check状态
            //Ems Services Provider 
            ITxEmsServicesProvider txEmsServices_sel = _PlanningResourceLibrary_selected.PlatformServicesProvider as ITxEmsServicesProvider;
            EmpObjectKey resourceObjKey_sel = default(EmpObjectKey);
            resourceObjKey_sel.objectId = txEmsServices_sel.InternalId;

            if (new EmpNodeClass().get_CheckInOutState(ref eMSCommons_1.context, ref resourceObjKey_sel) != EmpEnumCOState.CO_Me_State)
            {
                error = "ResourceLibrary is not checked out by me.";
                return false;
            }

            string text;
            EmpObjectKey createResourceLibrary = GetCreateResourceLibrary(eMSCommons_1.context, resourceObjKey_sel, _ObjectPrototypeClass, out text);

            if (createResourceLibrary.objectId != 0)
            {
                EmpContext context = GetContext(out text);
                if (context.sessionId != 0)
                {
                    bool flag = false;

                    string name = _Component_Name;
                    string fullDirectoryPath = _FullDirectoryPath;
                    string path = Path.Combine(TxApplication.SystemRootDirectory, fullDirectoryPath);

                    EmpObjectKey empObjectKey = CreateToolPrototype(context, createResourceLibrary, _ObjectPrototypeClass, name, path, out text, _PrototypeExternalId);

                    if (empObjectKey.objectId == 0)
                    {
                        flag = false;
                        string text2 = string.Format("Create ToolPrototype failed {0} {1} {2}", name, fullDirectoryPath, text);
                        TxMessageBox.ShowModal(text2, "AutoJT", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }

                    TxApplication.StatusBarMessage = "";
                    result = flag;
                }
                else
                {
                    TxMessageBox.ShowModal(text, "AutoJT", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    TxApplication.StatusBarMessage = "";
                    result = false;
                }
            }
            else
            {
                TxMessageBox.ShowModal(text, "AutoJT", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                TxApplication.StatusBarMessage = "";
                result = false;
            }

            return result;
        }
        public static EmpObjectKey GetCreateResourceLibrary(EmpContext context, EmpObjectKey selected_resourceLib_OBJ_Key, string planType, out string error)
        {
            error = "Unknow error. (GetCreateResourceLibrary)";
            EmpObjectKey empObjectKey = default(EmpObjectKey);
            EmpObjectKey result;

            if (TxApplication.PlatformType == TxPlatformType.EmServer)
            {
                try
                {
                    EmpObjectKey[] children = new EmpCollectionClass().GetChildren(ref context, ref selected_resourceLib_OBJ_Key);
                    if (children != null && children.Length != 0)
                    {
                        EmpNode empNode = new EmpNodeClass();
                        for (int i = 0; i < children.Length; i++)
                        {
                            EmpEnumCOState empEnumCOState = empNode.get_CheckInOutState(ref context, ref children[i]);
                            DnfObject dnfObject = new DnfObject(children[i].objectId);
                            DnfClassDef classDef = DnfClassDef.GetClassDef(dnfObject.GetClassDef().GetTypeId());
                            if (classDef.IsA(DnfClassDef.GetClassDef("PmResourceLibrary")) && empEnumCOState == EmpEnumCOState.CO_Me_State)
                            {
                                empObjectKey = children[i];
                                break;
                            }
                        }
                    }
                    if (empObjectKey.objectId == 0)
                    {
                        empObjectKey = new EmpCollectionClass().AddNewChild(ref context, ref selected_resourceLib_OBJ_Key, planType);
                        Thread.Sleep(100);
                        int num = 0;
                        while (!new EmpLibraryClass().IsObjectExist(ref context, ref empObjectKey))
                        {
                            Thread.Sleep(100);
                            num++;
                            if (num > 10)
                            {
                                error = "Cannot create a new Resource Library in the working folder.";
                                return empObjectKey;
                            }
                        }
                        new EmpResourceLibraryClass().set_Name(ref context, ref empObjectKey, "AutoJT Test Library");
                    }
                }
                catch (Exception ex)
                {
                    AJTTxMessageHandling.WriteException(ex);
                    error = "Cannot create a new Resource Library in the working folder. " + ex.Message;
                    return empObjectKey;
                }
                if (empObjectKey.objectId == 0)
                {
                    error = "Cannot create a new Resource Library in the working folder. Create a new Resource Library manually in your working folder.";
                }
                result = empObjectKey;
            }
            else
            {
                result = empObjectKey;
            }
            return result;
        }

        /// <summary>
        /// 添加ResourceLibrary新成员, 设置名称, 获取external id
        /// </summary>
        /// <param name="context_app"></param>
        /// <param name="selected_resourceLib_OBJ_Key"></param>
        /// <param name="empNode"></param>
        /// <param name="name"></param>
        /// <param name="planType"></param>
        /// <param name="externalID"></param>
        /// <returns></returns>
        public EmpObjectKey CreateNewChild4ResourceLibrary(EmpContext context_app, EmpObjectKey selected_resourceLib_OBJ_Key, EmpNode empNode,
            string name, string planType,
            string cojt_RelativePath,
            out string externalID, out bool flag9)
        {
            //检查cojt的父级文件夹
            string[] pmLibFolder = cojt_RelativePath.Split('\\');
            List<string> pmLibFolderList = new List<string>();

            try
            {
                for (int i = 0; i < pmLibFolder.Length; i++)
                {
                    if (pmLibFolder[i] != "" && !(pmLibFolder[i].ToLower().EndsWith(".cojt")))
                    {
                        pmLibFolderList.Add(pmLibFolder[i]);
                    }
                }
            }
            catch
            {


            }

            //创建cojt的父级文件夹节点
            if (pmLibFolderList.Count > 0)
            {
                //创建cojt的父级文件夹节点 PmResourceLibrary
                selected_resourceLib_OBJ_Key = CreateCOJTParentPmResourceLibrary(pmLibFolderList, context_app, selected_resourceLib_OBJ_Key, out string inofs);
            }



            flag9 = false;
            EmpObjectKey result;

            EmpCollectionClass empCollectionClass = null;
            EmpObjectKey empObjectKey_new = default(EmpObjectKey);
            try
            {
                empCollectionClass = new EmpCollectionClass();

                //添加新成员
                empObjectKey_new = empCollectionClass.AddNewChild(ref context_app, ref selected_resourceLib_OBJ_Key, planType);
                //set name
                empNode.set_Name(ref context_app, empObjectKey_new, name);
                //get external id
                IEmpNode empNode1 = empNode;
                externalID = empNode1.get_ExternalID(context_app, empObjectKey_new);

                result = empObjectKey_new;
                flag9 = true;
            }
            catch (Exception)
            {
                flag9 = false;
                empCollectionClass.DeleteChild(ref context_app, ref selected_resourceLib_OBJ_Key, ref empObjectKey_new);
                throw;
            }

            return result;
        }

        //创建cojt的父级文件夹节点
        EmpObjectKey CreateCOJTParentPmResourceLibrary(List<string> foldernames,
            EmpContext context, EmpObjectKey resourceLib_OBJ_Key, out string error, string planType = "PmResourceLibrary")
        {
            error = "Unknow error. (CreateCOJTParentPmResourceLibrary)";

            EmpObjectKey objectKey_tmp = resourceLib_OBJ_Key;

            //找到的或者新建的节点
            EmpObjectKey empObjectKey_node1 = default(EmpObjectKey);

            EmpObjectKey result;

            for (int k = 0; k < foldernames.Count; k++)
            {
                if (k != 0)
                {
                    //第一层的文件没找到或创建失败
                    if (objectKey_tmp.objectId == 0)
                    {
                        return resourceLib_OBJ_Key;
                    }
                    else
                    {
                        empObjectKey_node1 = default(EmpObjectKey);
                    }
                }

                string item = foldernames[k];

                try
                {
                    //在选定的RobcadResourceLibrary中遍历
                    EmpObjectKey[] children = new EmpCollectionClass().GetChildren(ref context, ref objectKey_tmp);
                    if (children != null && children.Length != 0)
                    {
                        EmpNode empNode = new EmpNodeClass();
                        for (int i = 0; i < children.Length; i++)
                        {
                            EmpEnumCOState empEnumCOState = empNode.get_CheckInOutState(ref context, ref children[i]);
                            DnfObject dnfObject = new DnfObject(children[i].objectId);
                            DnfClassDef classDef = DnfClassDef.GetClassDef(dnfObject.GetClassDef().GetTypeId());

                            string _niceName = classDef.GetNiceName();
                            string _displayName = classDef.GetName();
                            if ((_niceName == planType || _niceName == "ResourceLibrary") && new EmpResourceLibraryClass().get_Name(ref context, children[i]).ToLower() == item.ToLower() && empEnumCOState == EmpEnumCOState.CO_Me_State)
                            {
                                empObjectKey_node1 = children[i];
                                break;
                            }
                        }
                    }

                    //新建节点
                    if (empObjectKey_node1.objectId == 0)
                    {
                        empObjectKey_node1 = new EmpCollectionClass().AddNewChild(ref context, ref objectKey_tmp, planType);
                        Thread.Sleep(100);
                        int num = 0;
                        while (!new EmpLibraryClass().IsObjectExist(ref context, ref empObjectKey_node1))
                        {
                            Thread.Sleep(100);
                            num++;
                            if (num > 10)
                            {
                                error = "Cannot create a new Resource Library in the CurrentProject.";
                                return empObjectKey_node1;
                            }
                        }
                        new EmpResourceLibraryClass().set_Name(ref context, ref empObjectKey_node1, item);
                    }
                }
                catch (Exception ex)
                {
                    AJTTxMessageHandling.WriteException(ex);
                    error = "Cannot create a new Resource Library in the CurrentProject. " + ex.Message;
                    return resourceLib_OBJ_Key;
                }
                if (empObjectKey_node1.objectId == 0)
                {
                    error = "Cannot create a new Resource Library in the CurrentProject. Create a new Resource Library manually in your CurrentProject.";
                    return resourceLib_OBJ_Key;
                }

                objectKey_tmp = empObjectKey_node1;
            }

            result = objectKey_tmp;

            return result;
        }


        //创建CompoundPart
        public EmpObjectKey CreateRootNode_CompoundPart(EmpContext context_app, EmpObjectKey _PartLibrary_OBJ_Key, EmpNode empNode, string nodeName, out bool flag9)
        {
            flag9 = false;
            EmpObjectKey result;

            IEmpCompoundPart empCollectionClass = null;
            EmpObjectKey empObjectKey_new = default(EmpObjectKey);
            try
            {
                empCollectionClass = new EmpCompoundPart();

                //添加新成员
                empObjectKey_new = empCollectionClass.AddNewChild(ref context_app, ref _PartLibrary_OBJ_Key, "CompoundPart");
                //set name
                empNode.set_Name(ref context_app, empObjectKey_new, nodeName);
                //get external id
                IEmpNode empNode1 = empNode;

                result = empObjectKey_new;
                flag9 = true;
            }
            catch (Exception)
            {
                flag9 = false;
                empCollectionClass.DeleteChild(ref context_app, ref _PartLibrary_OBJ_Key, ref empObjectKey_new);
                throw;
            }

            return result;
        }

        //创建PartPrototype
        public EmpObjectKey CreatePartPrototypeNode(EmpContext context_app, EmpObjectKey _PartLibrary_OBJ_Key, EmpNode empNode,
        string name, string planType,
        out string externalID, out bool flag9)
        {
            flag9 = false;
            EmpObjectKey result;

            EmpCollectionClass empCollectionClass = null;
            EmpObjectKey empObjectKey_new = default(EmpObjectKey);
            try
            {
                empCollectionClass = new EmpCollectionClass();

                //添加新成员
                empObjectKey_new = empCollectionClass.AddNewChild(ref context_app, ref _PartLibrary_OBJ_Key, "PartPrototype");
                //set name
                empNode.set_Name(ref context_app, empObjectKey_new, name);
                //get external id
                IEmpNode empNode1 = empNode;
                externalID = empNode1.get_ExternalID(context_app, empObjectKey_new);

                result = empObjectKey_new;
                flag9 = true;
            }
            catch (Exception)
            {
                flag9 = false;
                empCollectionClass.DeleteChild(ref context_app, ref _PartLibrary_OBJ_Key, ref empObjectKey_new);
                throw;
            }

            return result;
        }

        //创建PartPrototype结构树
        public EmpObjectKey CreatePartPrototypeNodeStruct(EmpContext context_app, EmpObjectKey _PartLibrary_OBJ_Key, EmpNode empNode,
        string name, EmpObjectKey empprototype,
        out string externalID, out bool flag9,
        TxVector txVector_loc, TxVector txVector_rot)
        {
            flag9 = false;
            EmpObjectKey result;

            IEmpCompoundPart empCollectionClass = null;
            EmpObjectKey empObjectKey_new = default(EmpObjectKey);
            try
            {
                empCollectionClass = new EmpCompoundPart();

                //添加新成员
                empObjectKey_new = empCollectionClass.AddNewChildByPrototype(ref context_app, ref _PartLibrary_OBJ_Key, empprototype);
                //set name
                empNode.set_Name(ref context_app, empObjectKey_new, name);
                //get external id
                IEmpNode empNode1 = empNode;
                externalID = empNode1.get_ExternalID(context_app, empObjectKey_new);

                //设置姿态
                try
                {
                    IEmpPart empPart = new EmpPart();
                    empPart.SetPosition(ref context_app, ref empObjectKey_new, txVector_loc.X, txVector_loc.Y, txVector_loc.Z);
                    empPart.SetRotation(ref context_app, ref empObjectKey_new, txVector_rot.X, txVector_rot.Y, txVector_rot.Z);

                }
                catch
                {
                }

                result = empObjectKey_new;
                flag9 = true;
            }
            catch (Exception)
            {
                flag9 = false;
                empCollectionClass.DeleteChild(ref context_app, ref _PartLibrary_OBJ_Key, ref empObjectKey_new);
                throw;
            }

            return result;
        }

        /// <summary>
        /// 创建TuneData.xml
        /// </summary>
        /// <param name="_sysRootPath"></param>
        /// <param name="name"></param>
        /// <param name="cojtDir"></param>
        /// <param name="_TxPlanningType"></param>
        /// <param name="_ExternalID"></param>
        /// <returns></returns>
        public bool CreateTunDataXML(string _sysRootPath, string name, string cojtDir, TxPlanningTypeMetaData _TxPlanningType, string _ExternalID)
        {
            try
            {
                AJTFileComponentNode aJTCom = new AJTFileComponentNode(name, cojtDir, _TxPlanningType);
                aJTCom.TypeMetaData = _TxPlanningType;

                AJTDefineComponentType aJTDefineComponentType = new AJTDefineComponentType(_sysRootPath);

                return aJTDefineComponentType.UpdateComponentTuneDataXml(aJTCom, _ExternalID);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 链接EMS
        /// </summary>
        /// <param name="_PlanningResourceLibrary_selected"></param>
        /// <param name="eMSCommons_1"></param>
        /// <param name="resourceObjKey_sel_curr"></param>
        /// <exception cref="Exception"></exception>
        public void ConnectEMS_CheckSelectedNode(ITxPlanningObject _PlanningResourceLibrary_selected, out eMSCommons eMSCommons_1, out EmpObjectKey resourceObjKey_sel_curr)
        {
            //连接eMS
            eMSCommons_1 = new eMSCommons();

            //用户没有输入选择的内容
            if (_PlanningResourceLibrary_selected == null)
            {
                //寻找或创建planning中的 RobcadResourceLibrary
                resourceObjKey_sel_curr = GetCreateRobcadResourceLibrary(eMSCommons_1, out string ERROR);

                if (!string.IsNullOrEmpty(ERROR))
                {
                    throw new Exception(ERROR);
                }
            }
            //用户有选择的内容
            else
            {
                //检查用户选中的节点的check状态
                //Ems Services Provider 
                ITxEmsServicesProvider txEmsServices_sel = _PlanningResourceLibrary_selected.PlatformServicesProvider as ITxEmsServicesProvider;

                resourceObjKey_sel_curr = default(EmpObjectKey);
                resourceObjKey_sel_curr.objectId = txEmsServices_sel.InternalId;

                if (new EmpNodeClass().get_CheckInOutState(ref eMSCommons_1.context, ref resourceObjKey_sel_curr) != EmpEnumCOState.CO_Me_State)
                {
                    throw new Exception("ResourceLibrary is not checked out by me.");
                }

            }
        }

        /// <summary>
        /// 创建planning中的 PartLibrary
        /// </summary>
        /// <param name="eMSCommons_1"></param>
        /// <param name="resourceObjKey_sel_curr"></param>
        public void ConnectEMS_CheckSelectedNode_part(out eMSCommons eMSCommons_1, out EmpObjectKey resourceObjKey_partLib, string nodeName)
        {
            //连接eMS
            eMSCommons_1 = new eMSCommons();

            //创建planning中的 PartLibrary
            resourceObjKey_partLib = CreateNewNode4ClassName(eMSCommons_1, out string ERROR, "PartLibrary", nodeName);
        }
        /// <summary>
        /// 创建planning中的 CompoundPart
        /// </summary>
        /// <param name="eMSCommons_1"></param>
        /// <param name="resourceObjKey_sel_curr"></param>
        public void ConnectEMS_CheckSelectedNode_CompoundPart(eMSCommons eMSCommons_1, out EmpObjectKey resourceObjKey_CompPart, string nodeName)
        {
            //连接eMS
            eMSCommons_1 = new eMSCommons();

            //创建planning中的 PartLibrary
            resourceObjKey_CompPart = CreateNewNode4ClassName(eMSCommons_1, out string ERROR, "CompoundPart", nodeName);
        }

        /// <summary>
        /// 寻找或创建planning中的 RobcadResourceLibrary
        /// </summary>
        /// <param name="eMSCommons_1"></param>
        /// <returns></returns>
        EmpObjectKey GetCreateRobcadResourceLibrary(eMSCommons eMSCommons_1, out string error, string libraryName = "ResourceLibrary")
        {
            /*
            EmpObjectKey resourceObjKey_sel_curr;
            EMPAPPLICATIONLib.EmpApplicationClass empApplicationClass = eMSCommons_1.app;
            EmpContext context = eMSCommons_1.context;
            EmpObjectKey project = empApplicationClass.get_CurrentContextData().Project;

            //EmpChildrenData[] array = new EmpChildrenData[] { default(EmpChildrenData) };


            EmpObjectKey empObjectKey = default(EmpObjectKey);
            EmpObjectKey[] children = GetChildren(ref context, ref project);//new EmpCollectionClass().GetChildren(ref context, ref project);

            if (children != null && children.Length != 0)
            {
                EmpNode empNode = new EmpNodeClass();
                for (int i = 0; i < children.Length; i++)
                {
                    EmpEnumCOState empEnumCOState = empNode.get_CheckInOutState(ref context, ref children[i]);
                    DnfObject dnfObject = new DnfObject(children[i].objectId);
                    DnfClassDef classDef = DnfClassDef.GetClassDef(dnfObject.GetClassDef().GetTypeId());
                    if (classDef.IsA(DnfClassDef.GetClassDef("PmResourceLibrary")) && empEnumCOState == EmpEnumCOState.CO_Me_State)
                    {
                        empObjectKey = children[i];
                        break;
                    }
                }
            }

            resourceObjKey_sel_curr = project;
            return resourceObjKey_sel_curr;
            */

            //error
            error = string.Empty;//"Unknow error. (GetCreateRobcadResourceLibrary)";
            EmpObjectKey empObjectKey = default(EmpObjectKey);
            EmpObjectKey result;

            //检查当前EmServer项目
            if (TxApplication.PlatformType == TxPlatformType.EmServer)
            {
                EmpContext context = eMSCommons_1.context;//GetContext(out error);
                //CurrentProject
                EmpObjectKey _currentProject = eMSCommons_1.app.CurrentProject;

                if (_currentProject.objectId != 0)
                {
                    EmpNode empNode = new EmpNodeClass();
                    //if (EmpEnumCOState.CO_Me_State != empNode.get_CheckInOutState(ref context, ref _currentProject))
                    //{
                    //    error = "CurrentProject is not checked out by me.";
                    //    result = empObjectKey;                        
                    //}
                    //else
                    {
                        try
                        {
                            //找到的ResourceLibrary数量
                            int asdfasfe = 0;
                            //递归找项目中存在的RobcadResourceLibrary
                            RecursivelyFindResourceLibrary(libraryName, ref empObjectKey, context, _currentProject, empNode,ref asdfasfe);

                            //项目中没找到就新建lib
                            if (empObjectKey.objectId == 0)
                            {
                                //新建lib的时候检查项目是否被checkout
                                if (EmpEnumCOState.CO_Me_State != empNode.get_CheckInOutState(ref context, ref _currentProject))
                                {
                                    error = $"Over {asdfasfe} Resource Library have been found, Not checked by me.\nCannot create a new Resource Library in the CurrentProject.\nCurrentProject is not checked out by me.";
                                    throw new Exception(error);
                                }

                                empObjectKey = new EmpCollectionClass().AddNewChild(ref context, ref _currentProject, libraryName);
                                Thread.Sleep(100);
                                int num = 0;
                                while (!new EmpLibraryClass().IsObjectExist(ref context, ref empObjectKey))
                                {
                                    Thread.Sleep(100);
                                    num++;
                                    if (num > 10)
                                    {
                                        error = "Cannot create a new Resource Library in the CurrentProject.";
                                        return empObjectKey;
                                    }
                                }
                                new EmpResourceLibraryClass().set_Name(ref context, ref empObjectKey, "EngineeringResourceLibrary");
                            }
                        }
                        catch (Exception ex)
                        {
                            AJTTxMessageHandling.WriteException(ex);
                            error = "Unknow error. \n" + PreseEmsException(ex.Message);
                            return empObjectKey;
                        }
                        if (empObjectKey.objectId == 0)
                        {
                            error = "Cannot create a new Resource Library in the CurrentProject. Create a new Resource Library manually in your CurrentProject.";
                        }
                        result = empObjectKey;
                    }
                }
                else
                {
                    error = "Cannot find CurrentProject, please define the CurrentProject";
                    result = empObjectKey;
                }
            }
            else
            {
                result = empObjectKey;
            }

            return result;
        }
        /// <summary>
        /// 创建planning中的 New Node
        /// </summary>
        /// <param name="eMSCommons_1"></param>
        /// <returns></returns>
        EmpObjectKey CreateNewNode4ClassName(eMSCommons eMSCommons_1, out string error, string libraryName = "PartLibrary", string nodeName = "")
        {
            /*
            EmpObjectKey resourceObjKey_sel_curr;
            EMPAPPLICATIONLib.EmpApplicationClass empApplicationClass = eMSCommons_1.app;
            EmpContext context = eMSCommons_1.context;
            EmpObjectKey project = empApplicationClass.get_CurrentContextData().Project;

            //EmpChildrenData[] array = new EmpChildrenData[] { default(EmpChildrenData) };


            EmpObjectKey empObjectKey = default(EmpObjectKey);
            EmpObjectKey[] children = GetChildren(ref context, ref project);//new EmpCollectionClass().GetChildren(ref context, ref project);

            if (children != null && children.Length != 0)
            {
                EmpNode empNode = new EmpNodeClass();
                for (int i = 0; i < children.Length; i++)
                {
                    EmpEnumCOState empEnumCOState = empNode.get_CheckInOutState(ref context, ref children[i]);
                    DnfObject dnfObject = new DnfObject(children[i].objectId);
                    DnfClassDef classDef = DnfClassDef.GetClassDef(dnfObject.GetClassDef().GetTypeId());
                    if (classDef.IsA(DnfClassDef.GetClassDef("PmResourceLibrary")) && empEnumCOState == EmpEnumCOState.CO_Me_State)
                    {
                        empObjectKey = children[i];
                        break;
                    }
                }
            }

            resourceObjKey_sel_curr = project;
            return resourceObjKey_sel_curr;
            */

            //error
            StringBuilder sb1 = new StringBuilder();
            sb1.Append("Unknow error. (GetCreate");
            sb1.Append(libraryName);
            sb1.Append(")");
            error = sb1.ToString();

            EmpObjectKey empObjectKey = default(EmpObjectKey);
            EmpObjectKey result;

            //检查当前EmServer项目
            if (TxApplication.PlatformType == TxPlatformType.EmServer)
            {
                EmpContext context = eMSCommons_1.context;//GetContext(out error);
                //CurrentProject
                EmpObjectKey _currentProject = eMSCommons_1.app.CurrentProject;

                if (_currentProject.objectId != 0)
                {
                    EmpNode empNode = new EmpNodeClass();
                    if (EmpEnumCOState.CO_Me_State != empNode.get_CheckInOutState(ref context, ref _currentProject))
                    {
                        error = "CurrentProject is not checked out by me.";
                        result = empObjectKey;
                    }
                    else
                    {
                        try
                        {
                            //新建child
                            empObjectKey = new EmpCollectionClass().AddNewChild(ref context, ref _currentProject, libraryName);
                            Thread.Sleep(100);
                            int num = 0;
                            while (!new EmpLibraryClass().IsObjectExist(ref context, ref empObjectKey))
                            {
                                Thread.Sleep(100);
                                num++;
                                if (num > 10)
                                {
                                    StringBuilder sb2 = new StringBuilder();
                                    sb2.Append("Cannot create a new ");
                                    sb2.Append(libraryName);
                                    sb2.Append(" in the CurrentProject.");
                                    error = sb2.ToString();

                                    return empObjectKey;
                                }
                            }
                            if (string.IsNullOrEmpty(nodeName))
                            {
                                new EmpResourceLibraryClass().set_Name(ref context, ref empObjectKey, libraryName);
                            }
                            else
                            {
                                new EmpResourceLibraryClass().set_Name(ref context, ref empObjectKey, nodeName);
                            }
                        }
                        catch (Exception ex)
                        {
                            AJTTxMessageHandling.WriteException(ex);

                            StringBuilder sb3 = new StringBuilder();
                            sb3.Append("Cannot create a new ");
                            sb3.Append(libraryName);
                            sb3.Append(" in the CurrentProject. ");
                            sb3.Append(ex.Message);
                            error = sb3.ToString();

                            return empObjectKey;
                        }
                        if (empObjectKey.objectId == 0)
                        {
                            StringBuilder sb4 = new StringBuilder();
                            sb4.Append("Cannot create a new ");
                            sb4.Append(libraryName);
                            sb4.Append(" in the CurrentProject. Create a new ");
                            sb4.Append(" manually in your CurrentProject.");
                            error = sb4.ToString();
                        }
                        result = empObjectKey;
                    }
                }
                else
                {
                    error = "Cannot find CurrentProject, please define the CurrentProject";
                    result = empObjectKey;
                }
            }
            else
            {
                result = empObjectKey;
            }

            return result;
        }

        /// <summary>
        /// 递归找RobcadResourceLibrary
        /// </summary>
        /// <param name="libraryName"></param>
        /// <param name="empObjectKey"></param>
        /// <param name="context"></param>
        /// <param name="_currentProject"></param>
        /// <param name="empNode"></param>
        void RecursivelyFindResourceLibrary(string libraryName, ref EmpObjectKey empObjectKey, EmpContext context, EmpObjectKey _currentProject, EmpNode empNode,ref int icount)
        {
            //在项目中寻找已存在的lib
            EmpObjectKey[] children = new EmpCollectionClass().GetChildren(ref context, ref _currentProject);
            if (children != null && children.Length != 0)
            {
                for (int i = 0; i < children.Length; i++)
                {
                    try
                    {
                        EmpEnumCOState empEnumCOState = empNode.get_CheckInOutState(ref context, ref children[i]);
                        DnfObject dnfObject = new DnfObject(children[i].objectId);
                        DnfClassDef classDef = DnfClassDef.GetClassDef(dnfObject.GetClassDef().GetTypeId());

                        //如果是文件继续递归
                        string _name = classDef.GetNiceName();
                        if (_name == "Collection")
                        {
                            RecursivelyFindResourceLibrary(libraryName, ref empObjectKey, context, children[i], empNode,ref icount);
                        }

                        if (_name.Contains(libraryName) )
                        {
                            icount++;
                            if (empEnumCOState == EmpEnumCOState.CO_Me_State)
                            {
                                //取第一个对象
                                empObjectKey = children[i];
                                break;
                            }                           
                        }

                    }
                    catch
                    {

                        continue;
                    }
                }
            }
        }

        public static EmpObjectKey CreateToolPrototype(EmpContext context, EmpObjectKey parent, string prototypeClass, string name, string path, out string error, string externalId = null)
        {
            error = "Unknown error.";
            EmpObjectKey empObjectKey = default(EmpObjectKey);
            empObjectKey.objectId = 0;
            EmpObjectKey result;
            if (context.sessionId == 0)
            {
                error = "No connection to the database (sessionId = 0).";
                result = empObjectKey;
            }
            else
            {
                try
                {
                    empObjectKey = new EmpResourceLibraryClass().AddNewChild(ref context, ref parent, prototypeClass);
                    Thread.Sleep(100);
                    int num = 0;
                    while (!new EmpLibraryClass().IsObjectExist(ref context, ref empObjectKey))
                    {
                        Thread.Sleep(100);
                        num++;
                        if (num > 10)
                        {
                            error = "Cannot create a new prototype in the resource library \"" + parent.objectId.ToString() + "\".";
                            empObjectKey.objectId = 0;
                            return empObjectKey;
                        }
                    }
                    new EmpNodeClass().set_Name(ref context, ref empObjectKey, name);
                }
                catch (Exception ex)
                {
                    AJTTxMessageHandling.WriteException(ex);
                    error = "Cannot create a new prototype in the resource library. " + GetExceptionDescription(ex);
                    empObjectKey.objectId = 0;
                    return empObjectKey;
                }
                if (!string.IsNullOrEmpty(externalId))
                {
                    try
                    {
                        new EmpNodeClass().set_ExternalID(ref context, ref empObjectKey, externalId);
                    }
                    catch (Exception exception)
                    {
                        AJTTxMessageHandling.WriteException(exception);
                        externalId = null;
                    }
                }
                string systemRootDirectory = TxApplication.SystemRootDirectory;
                string text = path.Replace(systemRootDirectory, "#\\").Replace("\\\\", "\\");
                try
                {
                    EmpObjectKey empObjectKey2 = new EmpToolPrototypeClass().CreateThreeDRep(ref context, ref empObjectKey, text);
                    int num2 = 0;
                    while (!new EmpToolPrototypeClass().IsObjectExist(ref context, ref empObjectKey2))
                    {
                        Thread.Sleep(100);
                        num2++;
                        if (num2 > 10)
                        {
                            error = "Cannot create the threeDrep object for the prototype \"" + empObjectKey.objectId.ToString() + "\".";
                            empObjectKey.objectId = 0;
                            return empObjectKey;
                        }
                    }
                }
                catch (Exception ex2)
                {
                    AJTTxMessageHandling.WriteException(ex2);
                    error = "Cannot add 3D path to new prototype." + GetExceptionDescription(ex2);
                    empObjectKey.objectId = 0;
                    return empObjectKey;
                }
                AJTTxTuneData.WriteTuneDataXml(path, AJTTxTuneData.GetProjectID(), new EmpNodeClass().get_ExternalID(ref context, ref empObjectKey), prototypeClass, "ToolPrototype", out error);
                result = empObjectKey;
            }
            return result;
        }

        public static string GetExceptionDescription(Exception ex)
        {
            string result;
            try
            {
                result = ((IEmpInfoItem)new EmpErrorServiceClass().GetXMLInfo(ex.Message).InfoItems.Item(0)).Description;
            }
            catch (Exception ex2)
            {
                AJTTxMessageHandling.WriteException(ex2);
                result = string.Concat(new string[]
                {
                    "Internal Error '",
                    ex2.ToString(),
                    "' while retrieving error description for '",
                    ex.Message,
                    "'."
                });
            }
            return result;
        }


        //所有的ResourceLibrary和RobcadResourceLibrary集合
        List<ResourceLibModel> m_allLibrary;
        public AJTeMServer()
        {
            this.m_allLibrary = new List<ResourceLibModel>();
        }

        //递归遍历当前项目, 找到所有的ResourceLibrary和RobcadResourceLibrary
        //public async Task<(List<Product>, List<ProductPermissions>, List<ProductUserInfo>)> QueryAllComboMealInfoAsync(string unionId)
        public List<ResourceLibModel> FindAllResourceLibrary(eMSCommons eMSCommons_1)
        {
            //检查当前EmServer项目
            if (TxApplication.PlatformType == TxPlatformType.EmServer)
            {
                //Context
                EmpContext context = eMSCommons_1.context;
                //CurrentProject
                EmpObjectKey _currentProject = eMSCommons_1.app.CurrentProject;
                if (_currentProject.objectId != 0)
                {
                    //root node
                    EmpNode root_empNode = new EmpNodeClass();
                    //检查项目
                    if (EmpEnumCOState.CO_Me_State != root_empNode.get_CheckInOutState(ref context, ref _currentProject))
                    {
                        throw new Exception("CurrentProject is not checked out by me.");
                    }
                    else
                    {

                        try
                        {
                            //递归遍历当前项目, 找到所有的ResourceLibrary和RobcadResourceLibrary
                            List<ResourceLibModel> childLib = null;
                            this.TraversalCurrentProject(ref childLib, context, _currentProject, root_empNode);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }

                        //try
                        //{
                        //    //递归遍历所有的resource, 找到没有创建实例的resource
                        //    //TraversalAllResourceLib(allLibrary, context, ref reuslt);
                        //}
                        //catch (Exception ex)
                        //{
                        //    throw ex;
                        //}
                    }
                }
                else
                {
                    throw new Exception("Cannot find CurrentProject, please define the CurrentProject");
                }
            }

            return this.m_allLibrary;
        }

        public static List<EmpObjectKeyPair> FindSelectedResource(eMSCommons eMSCommons_1, List<EmpObjectKey> selectedLib)
        {
            if (selectedLib == null || selectedLib.Count == 0)
            {
                return null;
            }

            List<EmpObjectKeyPair> reuslt = new List<EmpObjectKeyPair>();

            try
            {
                //递归遍历所有的resource, 找到没有创建实例的resource
                TraversalAllResourceLib(selectedLib, eMSCommons_1.context, ref reuslt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return reuslt;
        }

        //递归遍历当前项目, 找到所有的ResourceLibrary和RobcadResourceLibrary 
        void TraversalCurrentProject(ref List<ResourceLibModel> childLib, EmpContext context, EmpObjectKey _currentProject, EmpNode empNode)
        {
            //在项目中寻找已存在的lib
            EmpObjectKey[] children = new EmpCollectionClass().GetChildren(ref context, ref _currentProject);
            if (children != null && children.Length != 0)
            {
                for (int i = 0; i < children.Length; i++)
                {
                    try
                    {
                        EmpEnumCOState empEnumCOState = empNode.get_CheckInOutState(ref context, ref children[i]);

                        string _name = GetEmpObjectKeyType(children[i]);

                        //文件夹
                        if (_name.Equals("Collection"))
                        {
                            TraversalCurrentProject(ref childLib, context, children[i], empNode);
                        }
                        //找子项
                        else if (childLib != null && (_name == "RobcadResourceLibrary" || _name == "ResourceLibrary") && empEnumCOState == EmpEnumCOState.CO_Me_State)
                        {
                            //接着进去找child
                            List<ResourceLibModel> childKey = new List<ResourceLibModel>();
                            childLib.Add(new ResourceLibModel()
                            {
                                LibEmpObjKey = children[i],
                                Name = GetEmpObjName(children[i], context) ?? _name,
                                ChildLib = childKey,
                            });
                            TraversalCurrentProject(ref childKey, context, children[i], empNode);
                        }
                        //找父项
                        else if ((_name == "RobcadResourceLibrary" || _name == "ResourceLibrary") && empEnumCOState == EmpEnumCOState.CO_Me_State)
                        {
                            //接着进去找child
                            List<ResourceLibModel> childKey = new List<ResourceLibModel>();
                            ResourceLibModel resourceLib_curr = new ResourceLibModel()
                            {
                                LibEmpObjKey = children[i],
                                Name = GetEmpObjName(children[i], context) ?? _name,
                                ChildLib = childKey,
                            };

                            TraversalCurrentProject(ref childKey, context, children[i], empNode);

                            //加入集合
                            this.m_allLibrary.Add(resourceLib_curr);
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }


        //递归遍历用户选择的项目, 找到所有的ResourceLibrary和RobcadResourceLibrary
        public List<ResourceLibModel> FindAllResourceLibrary(eMSCommons eMSCommons_1, ITxPlanningObject txPlanningObject1)
        {
            //检查当前EmServer项目
            if (TxApplication.PlatformType == TxPlatformType.EmServer)
            {
                //Context
                EmpContext context = eMSCommons_1.context;
                //CurrentProject
                EmpObjectKey _currentProject = eMSCommons_1.app.CurrentProject;
                if (_currentProject.objectId != 0)
                {
                    //用户选择的key
                    int idet = eMSCommons.getPlanningInternalID(txPlanningObject1);
                    EmpObjectKey userSelectedObj = new EmpObjectKey
                    {
                        objectId = idet
                    };

                    //root node
                    EmpNode root_empNode = new EmpNodeClass();
                    //检查项目
                    if (EmpEnumCOState.CO_Me_State != root_empNode.get_CheckInOutState(ref context, ref userSelectedObj))
                    {
                        throw new Exception("This node is not checked out by me.");
                    }
                    else
                    {

                        try
                        {
                            

                            //判断用户选择的类型
                            string firet_name = GetEmpObjectKeyType(userSelectedObj);
                            if (firet_name.Contains("ResourceLibrary"))
                            {
                                //递归遍历当前项目, 找到所有的ResourceLibrary和RobcadResourceLibrary
                                List<ResourceLibModel> childLib = null;
                                //this.TraversalCurrentProject2(ref childLib, context, userSelectedObj, root_empNode);

                                //接着进去找child
                                List<ResourceLibModel> childKey = new List<ResourceLibModel>();
                                ResourceLibModel resourceLib_curr = new ResourceLibModel()
                                {
                                    LibEmpObjKey = userSelectedObj,
                                    Name = GetEmpObjName(userSelectedObj, context) ?? firet_name,
                                    ChildLib = childKey,
                                };

                                TraversalCurrentProject2(ref childKey, context, userSelectedObj, root_empNode);

                                //加入集合
                                this.m_allLibrary.Add(resourceLib_curr);
                            }
                            else
                            {
                                //递归遍历当前项目, 找到所有的ResourceLibrary和RobcadResourceLibrary
                                List<ResourceLibModel> childLib = null;
                                this.TraversalCurrentProject2(ref childLib, context, userSelectedObj, root_empNode);
                            }

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }

                        //try
                        //{
                        //    //递归遍历所有的resource, 找到没有创建实例的resource
                        //    //TraversalAllResourceLib(allLibrary, context, ref reuslt);
                        //}
                        //catch (Exception ex)
                        //{
                        //    throw ex;
                        //}
                    }
                }
                else
                {
                    throw new Exception("Cannot find CurrentProject, please define the CurrentProject");
                }
            }

            return this.m_allLibrary;
        }
        void TraversalCurrentProject2(ref List<ResourceLibModel> childLib, EmpContext context, EmpObjectKey _currentProject, EmpNode empNode)
        {
            //在项目中寻找已存在的lib
            EmpObjectKey[] children = new EmpCollectionClass().GetChildren(ref context, ref _currentProject);
            if (children != null && children.Length != 0)
            {
                for (int i = 0; i < children.Length; i++)
                {
                    try
                    {
                        EmpEnumCOState empEnumCOState = empNode.get_CheckInOutState(ref context, ref children[i]);

                        string _name = GetEmpObjectKeyType(children[i]);

                        //文件夹
                        if (_name.Equals("Collection"))
                        {
                            TraversalCurrentProject2(ref childLib, context, children[i], empNode);
                        }
                        //找子项
                        else if (childLib != null && (_name == "RobcadResourceLibrary" || _name == "ResourceLibrary") && empEnumCOState == EmpEnumCOState.CO_Me_State)
                        {
                            //接着进去找child
                            List<ResourceLibModel> childKey = new List<ResourceLibModel>();
                            childLib.Add(new ResourceLibModel()
                            {
                                LibEmpObjKey = children[i],
                                Name = GetEmpObjName(children[i], context) ?? _name,
                                ChildLib = childKey,
                            });
                            TraversalCurrentProject2(ref childKey, context, children[i], empNode);
                        }
                        //找父项
                        else if ((_name == "RobcadResourceLibrary" || _name == "ResourceLibrary") && empEnumCOState == EmpEnumCOState.CO_Me_State)
                        {
                            //接着进去找child
                            List<ResourceLibModel> childKey = new List<ResourceLibModel>();
                            ResourceLibModel resourceLib_curr = new ResourceLibModel()
                            {
                                LibEmpObjKey = children[i],
                                Name = GetEmpObjName(children[i], context) ?? _name,
                                ChildLib = childKey,
                            };

                            TraversalCurrentProject2(ref childKey, context, children[i], empNode);

                            //加入集合
                            this.m_allLibrary.Add(resourceLib_curr);
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }


        //递归遍历所有的resource, 找到没有创建实例的resource
        static void TraversalAllResourceLib(List<EmpObjectKey> all_library, EmpContext context_app, ref List<EmpObjectKeyPair> allResources)
        {
            if (all_library != null && all_library.Count > 0)
            {
                //用于获取所有的chlid
                EmpCollectionClass empCollectionClass = new EmpCollectionClass();
                //所有的library对象中遍历
                foreach (EmpObjectKey item in all_library)
                {
                    EmpObjectKey empObjectKey_new = item;
                    //获得当前reslib中所有的第一层对象
                    List<EmpObjectKey> empObjectKeys_child = empCollectionClass.GetChildren(ref context_app, ref empObjectKey_new).ToList();

                    //递归reslib中的对象,返回所有的资源对象
                    TraversalResLibObject(empObjectKeys_child, context_app, ref allResources, empObjectKey_new);
                }
            }
        }
        //递归reslib中的对象,返回所有的资源对象
        static void TraversalResLibObject(List<EmpObjectKey> empObjectKeys_child, EmpContext context, ref List<EmpObjectKeyPair> libAllResources, EmpObjectKey parentkey)
        {
            if (empObjectKeys_child != null && empObjectKeys_child.Count > 0)
            {
                foreach (EmpObjectKey item in empObjectKeys_child)
                {
                    string _name = GetEmpObjectKeyType(item);
                    if (_name == "RobcadResourceLibrary" || _name == "ResourceLibrary")
                    {
                        EmpObjectKey empObjectKey_new = item;
                        //获得当前reslib中所有的第一层对象
                        List<EmpObjectKey> empObjectKeys21 = new EmpCollectionClass().GetChildren(ref context, ref empObjectKey_new).ToList();
                        //继续递归
                        TraversalResLibObject(empObjectKeys21, context, ref libAllResources, item);
                    }
                    else
                    {
                        //判断是否已经创建实例
                        bool bl12 = GetResourceISCreateInstance(item, context, out string sName, out DateTime sdate, out string sfileName) == false;
                        if (bl12)
                        {
                            bool bl13 = libAllResources?.Where(a => a.ChildKey.objectId == item.objectId).Count() == 0;
                            if (bl13)
                            {
                                libAllResources.Add(new EmpObjectKeyPair()
                                {
                                    Name = sName,
                                    Type = _name,
                                    Date = string.Format("{0}", sdate),
                                    FilePath = sfileName,
                                    ParentKey = parentkey,
                                    ChildKey = item,
                                    //IsChecked = true,
                                });
                            }
                        }
                    }
                }
            }
        }

        //获取一个EmpObjectKey的type
        public static string GetEmpObjectKeyType(EmpObjectKey objectKey)
        {
            try
            {
                DnfObject dnfObject = new DnfObject(objectKey.objectId);
                DnfClassDef classDef = DnfClassDef.GetClassDef(dnfObject.GetClassDef().GetTypeId());

                return classDef.GetNiceName();
            }
            catch
            {
                return null;
            }
        }

        //获取resource是否已经创建实例
        static bool? GetResourceISCreateInstance(EmpObjectKey objectKey, EmpContext context,
            out string sName, out DateTime sdate, out string sfileName)
        {
            sName = string.Empty;
            sdate = default;
            sfileName = string.Empty;

            try
            {
                //没有创建实例
                EmpToolPrototype empToolPrototype = new EmpToolPrototype();
                if (empToolPrototype.GetNumInstances(ref context, ref objectKey) == 0)
                {
                    IEmpExternalDocument empExternalDocument = new EmpExternalDocumentClass();

                    try
                    {
                        sName = empExternalDocument.get_Name(ref context, ref objectKey);
                    }
                    catch
                    {
                    }

                    try
                    {
                        sdate = empExternalDocument.get_ModificationDate(ref context, ref objectKey);
                    }
                    catch
                    {
                    }

                    try
                    {
                        //threeDRepKey
                        EmpObjectKey threeDRepKey = empToolPrototype.GetThreeDRep(ref context, ref objectKey);
                        if (threeDRepKey.objectId != 0)
                        {
                            //get fileName                       
                            sfileName = empExternalDocument.get_Filename(ref context, ref threeDRepKey);
                        }
                    }
                    catch
                    {
                    }

                    return false;
                }
                //已经有创建实例
                else
                {
                    return true;
                }
            }
            //异常情况
            catch
            {
                return null;
            }
        }

        public static bool DeleteEmpKeys(EmpObjectKey parentKey, EmpObjectKey childKey, EmpContext context, out string erromsg)
        {
            bool result = false;
            erromsg = string.Empty;
            if (parentKey.objectId != 0 && childKey.objectId != 0)
            {
                try
                {
                    EmpCollectionClass empCollectionClass = new EmpCollectionClass();
                    empCollectionClass.DeleteChild(ref context, ref parentKey, ref childKey);

                    result = true;
                }
                catch (Exception ex)
                {
                    erromsg = ex.Message;
                    result = false;
                }
            }
            return result;
        }

        //获取objekey的name
        public static string GetEmpObjName(EmpObjectKey objectKey, EmpContext context)
        {
            try
            {
                IEmpExternalDocument empExternalDocument = new EmpExternalDocumentClass();
                return empExternalDocument.get_Name(ref context, ref objectKey);
            }
            catch
            {
                return null;
            }
        }
    
        //解析ems中的exception
        static string PreseEmsException(string exd)
        {
            string result = exd;

            try
            {
                //var pattern = "(<Description>).[^<>]+(</Description>)";//表达式文本
                var regex = new Regex("(<Description>).[^<>]+(</Description>)", RegexOptions.IgnoreCase);//表达式对象
                string serdf = regex.Match(exd).Groups[1].Value ;
                if (!string.IsNullOrEmpty(serdf))
                {
                    result = serdf;
                }
            }
            catch
            {
            }

            return result;
        }
    }

    public class EmpObjectKeyPair
    {
        bool isChecked;

        EmpObjectKey parentKey;
        EmpObjectKey childKey;

        string name;
        string type;
        string date;
        string filename;

        public EmpObjectKey ParentKey { get => parentKey; set => parentKey = value; }
        public EmpObjectKey ChildKey { get => childKey; set => childKey = value; }
        public string Name { get => name; set => name = value; }
        public string Type { get => type; set => type = value; }
        public string Date { get => date; set => date = value; }
        public string FilePath { get => filename; set => filename = value; }
        //public bool IsChecked { get => isChecked; set => isChecked = value; }


        public EmpObjectKey ParentLib { get; set; }
        public List<EmpObjectKey> ChildLib { get; set; }
    }

    //资源库类
    public class ResourceLibModel
    {
        public EmpObjectKey LibEmpObjKey { get; set; }

        string name;
        public string Name { get => name; set => name = value; }

        public List<ResourceLibModel> ChildLib { get; set; }
    }
}