using EMPMODELLib;
using EMPTYPELIBRARYLib;
using EngineeringInternalExtension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Tecnomatix.Engineering;
using Tecnomatix.Planning;

namespace AutoJTTXCoreUtilities.AJTeMSUtils
{
    public class AJTeMServerUtils
    {
        #region 静态方法

        public static bool IsCheckOutByMe(EmpContext context, int internalId)
        {
            bool result = true;

            try
            {
                //selected object internalID
                EmpObjectKey objPrototypeKey = default(EmpObjectKey);
                objPrototypeKey.objectId = internalId;
                EmpObjectKey empObjectKey = objPrototypeKey;

                if (new EmpNodeClass().get_CheckInOutState(ref context, ref empObjectKey) != EmpEnumCOState.CO_Me_State)
                {
                    result = false;
                }

            }
            catch
            {


            }

            return result;
        }

        public static void GetFullPath(ITxPlanningObject planningObject)
        {
            TxObjectList result = new TxObjectList();
            if (TxApplication.PlatformType != TxPlatformType.EmServer)
            {
                throw new NotSupportedException("Can't get childrens from databse. Invalid platform type " + TxApplication.PlatformType.ToString() + ".");
            }

            int internalId = AJTTxPlanningObjectUtilities.GetInternalId(planningObject);

            if (internalId != 0)
            {
                EmpContext empContext = default(EmpContext);
                empContext.sessionId = TxApplication.ActiveDocument.eMSSessionId;
                EmpContext empContext2 = empContext;
                EmpObjectKey empObjectKey = default(EmpObjectKey);
                empObjectKey.objectId = internalId;
                EmpObjectKey empObjectKey2 = empObjectKey;
                EmpObjectKey[] children = AJTeMServer.GetChildren(ref empContext2, ref empObjectKey2);
                TxEmsGlobalServicesProvider txEmsGlobalServicesProvider = TxApplication.ActiveDocument.PlatformGlobalServicesProvider as TxEmsGlobalServicesProvider;
                if (txEmsGlobalServicesProvider != null)
                {
                    //ArrayList arrayList = new ArrayList();
                    foreach (EmpObjectKey empObjectKey3 in children)
                    {
                        //arrayList.Add(empObjectKey3.objectId);
                    }
                    //result = txEmsGlobalServicesProvider.GetObjectsByInternalIds(arrayList);
                }
                //return result;
            }
        }

        #endregion

        #region 公有变量

        //所有的实例
        public EmpObjectKey[] m_Instances { get; set; }

        #endregion

        #region 私有变量

        private EmpExternalDocumentClass m_empExternalDocument { get; set; }

        ////当前需要操作的资源
        private EmpObjectKey m_threeDRepKey { get; set; }

        private EmpNode m_empNode { get; set; }

        private EmpObjectKey m_ObjectKey;
        private EmpObjectKey m_ObjPrototypeKey;

        private AJTeMServerResourceHelper m_aJTeMServerResourceHelper { get; set; }

        #endregion

        public AJTeMServerUtils()
        {
            this.m_empNode = new EmpNodeClass();
            this.m_empExternalDocument = new EmpExternalDocumentClass();
            this.m_aJTeMServerResourceHelper = new AJTeMServerResourceHelper();
        }

        #region 实例方法

        public string GetFileName(int internalId, EmpContext context, out string infos, bool isPartPrototype = false)
        {
            string result = string.Empty;
            infos = string.Empty;

            //selected object internalID
            EmpObjectKey objPrototypeKey = default(EmpObjectKey);
            objPrototypeKey.objectId = internalId;
            this.m_ObjectKey = objPrototypeKey;




            result = this.m_aJTeMServerResourceHelper.GetFileName(internalId, out EmpObjectKey emp3ObjectKey, true);
            this.m_threeDRepKey = emp3ObjectKey;
            this.m_Instances = this.m_aJTeMServerResourceHelper.m_instances;
            this.m_ObjPrototypeKey = this.m_aJTeMServerResourceHelper.m_objPrototypeKey;

            if (string.IsNullOrEmpty(result))
            {
                //换种方式查找
                result = this.GetFileName2(isPartPrototype, context, objPrototypeKey);
            }

            return result;
        }

        //不区分planning类型的更改
        string GetFileName2(bool isPartPrototype, EmpContext context, EmpObjectKey objPrototypeKey)
        {
            string result = string.Empty;
            try
            {
                if (isPartPrototype)
                {
                    //Emp PartPrototype
                    EmpPartPrototype _empPartPrototype = new EmpPartPrototype();
                    IEmpPartPrototype empPartPrototype = _empPartPrototype;

                    //context
                    EmpContext empContext = context;


                    EmpObjectKey empObjectKey = objPrototypeKey;
                    //threeDRepKey
                    this.m_threeDRepKey = empPartPrototype.GetThreeDRep(ref empContext, ref empObjectKey);

                    if (this.m_threeDRepKey.objectId != 0)
                    {
                        //get fileName
                        empContext = context;
                        empObjectKey = this.m_threeDRepKey;
                        IEmpExternalDocument empExternalDocument2 = this.m_empExternalDocument;

                        string name1 = empExternalDocument2.get_Filename(ref empContext, ref empObjectKey);
                        //string name2 = empExternalDocument2.get_Name(ref empContext, ref empObjectKey);

                        //result = Path.Combine(Path.GetDirectoryName(name1), $"{name2}.{Path.GetExtension(name1)}");
                        result = name1;

                        empContext = context;
                        empObjectKey = objPrototypeKey;
                        IEmpPartPrototype empPartPrototype2 = empPartPrototype;
                        this.m_Instances = empPartPrototype2.GetInstances(ref empContext, ref empObjectKey);
                    }
                }
                else
                {


                    //Emp ToolPrototype
                    EmpToolPrototype _empToolPrototype = new EmpToolPrototype();
                    IEmpToolPrototype empToolPrototype = _empToolPrototype;

                    //context
                    EmpContext empContext = context;


                    EmpObjectKey empObjectKey = objPrototypeKey;
                    //threeDRepKey
                    this.m_threeDRepKey = empToolPrototype.GetThreeDRep(ref empContext, ref empObjectKey);

                    if (this.m_threeDRepKey.objectId != 0)
                    {
                        //get fileName
                        empContext = context;
                        empObjectKey = this.m_threeDRepKey;
                        IEmpExternalDocument empExternalDocument2 = this.m_empExternalDocument;

                        string name1 = empExternalDocument2.get_Filename(ref empContext, ref empObjectKey);
                        //string name2 = empExternalDocument2.get_Name(ref empContext, ref empObjectKey);

                        //result = Path.Combine(Path.GetDirectoryName(name1), $"{name2}.{Path.GetExtension(name1)}");
                        result = name1;

                        empContext = context;
                        empObjectKey = objPrototypeKey;
                        IEmpToolPrototype empToolPrototype2 = empToolPrototype;
                        this.m_Instances = empToolPrototype2.GetInstances(ref empContext, ref empObjectKey);

                        //this.GetInstancesForPrototype(name1);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        //修改planning对象的picturename和picpathname
        public bool? ChangePictureFileNameAndPath(string objName, string sysroot, int internalId, EmpContext context, bool isPartPrototype = false)
        {
            //输入完整的image路径
            //修改image文件名
            //修改对象的image的路径

            bool? result = false;

            if (string.IsNullOrEmpty(objName) || string.IsNullOrEmpty(sysroot))
            {
                return result;
            }

            try
            {
                //selected object internalID
                EmpObjectKey objPrototypeKey = default(EmpObjectKey);
                objPrototypeKey.objectId = internalId;


                if (isPartPrototype)
                {
                    //Emp PartPrototype
                    EmpPartPrototype _empPartPrototype = new EmpPartPrototype();
                    IEmpPartPrototype empPartPrototype = _empPartPrototype;

                    //context
                    EmpContext empContext = context;

                    EmpObjectKey empObjectKey = objPrototypeKey;
                    var imageKey = empPartPrototype.GetImage(ref empContext, ref empObjectKey);

                    if (imageKey.objectId != 0)
                    {
                        IEmpExternalDocument empExternalDocument2 = this.m_empExternalDocument;
                        string imageFullPath = empExternalDocument2.get_Filename(ref empContext, ref imageKey);


                        if (string.IsNullOrEmpty(imageFullPath))
                        {
                            return false;
                        }

                        //拼接全路径
                        string fullPath = Path.Combine(sysroot, imageFullPath.Replace("#\\", ""));

                        //新文件名
                        string sfasf = Path.Combine(Path.GetDirectoryName(fullPath), $"{objName}{Path.GetExtension(fullPath)}");


                        if (fullPath == sfasf)
                        {
                            return null;
                        }

                        //移动
                        File.Move(fullPath, sfasf);

                        //拼接新文件路径
                        string newFileName = sfasf.Replace(sysroot, "#");

                        //赋值
                        empExternalDocument2.set_Filename(empContext, imageKey, newFileName);
                        result = true;
                    }
                }
                else
                {
                    //Emp ToolPrototype
                    EmpToolPrototype _empToolPrototype = new EmpToolPrototype();
                    IEmpToolPrototype empToolPrototype = _empToolPrototype;

                    //context
                    EmpContext empContext = context;

                    EmpObjectKey empObjectKey = objPrototypeKey;
                    var imagekey = empToolPrototype.GetImage(ref empContext, empObjectKey);

                    if (imagekey.objectId != 0)
                    {
                        IEmpExternalDocument empExternalDocument2 = this.m_empExternalDocument;
                        string imageFullPath = empExternalDocument2.get_Filename(ref empContext, ref imagekey);

                        if (string.IsNullOrEmpty(imageFullPath))
                        {
                            return false;
                        }

                        //拼接全路径
                        string fullPath = Path.Combine(sysroot, imageFullPath.Replace("#\\", ""));

                        //新文件名
                        string sfasf = Path.Combine(Path.GetDirectoryName(fullPath), $"{objName}{Path.GetExtension(fullPath)}");
                        string sfasf2 = Path.Combine(Path.GetDirectoryName(fullPath), $"{objName}.tmp");


                        if (fullPath == sfasf)
                        {
                            return null;
                        }

                        //移动
                        File.Move(fullPath, sfasf2);
                        Thread.Sleep(125);
                        File.Move(sfasf2, sfasf);

                        //拼接新文件路径
                        string newFileName = sfasf.Replace(sysroot,"#");

                        //赋值
                        empExternalDocument2.set_Filename(empContext, imagekey, newFileName);
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public void UpdatePlanningInstances(EmpContext context, string newResourceNAME, string newName,
            out string _ex_FileName,
            out string _ex_ResourceName,
            out string _ex_InstancesName,
            out int instancesCount)
        {
            //文件路径
            _ex_FileName = string.Empty;
            //资源名
            _ex_ResourceName = string.Empty;
            //实例名
            _ex_InstancesName = string.Empty;
            //修改实例名字的数量
            instancesCount = 0;

            try
            {
                if (this.m_Instances != null)//&& this.m_Instances.Length > 0
                {
                    EmpContext empContext = context;

                    //当前操作的resource
                    EmpObjectKey empObjectKey_1 = this.m_threeDRepKey;

                    //更新filename
                    IEmpExternalDocument empExternalDocument = this.m_empExternalDocument;

                    try
                    {
                        empExternalDocument.set_Filename(ref empContext, ref empObjectKey_1, newResourceNAME);
                    }
                    catch (Exception exFileName)
                    {
                        _ex_FileName = exFileName.Message;
                    }

                    try
                    {
                        this.m_empNode.set_Name(ref context, ref this.m_ObjectKey, newName);
                        this.m_empNode.set_Name(ref context, ref this.m_ObjPrototypeKey, newName);
                    }
                    catch (Exception exInstancesName)
                    {
                        _ex_ResourceName = exInstancesName.Message;
                    }


                    foreach (EmpObjectKey item in this.m_Instances)
                    {
                        try
                        {
                            EmpObjectKey empObjectKey5 = item;
                            this.m_empNode.set_Name(ref context, ref empObjectKey5, newName);
                            instancesCount++;
                        }
                        catch (Exception exInstancesName)
                        {
                            _ex_InstancesName = exInstancesName.Message;
                            continue;
                        }
                    }
                }
            }
            catch
            {

            }
        }

        #endregion

        public List<TxComponent> GetInstancesForPrototype(string searchPath)
        {
            List<TxComponent> list = new List<TxComponent>();
            TxTypeFilter txTypeFilter = new TxTypeFilter();
            txTypeFilter.AddIncludedType(typeof(TxComponent));
            TxObjectList allDescendants = TxApplication.ActiveDocument.PhysicalRoot.GetAllDescendants(txTypeFilter);
            foreach (ITxObject txObject in allDescendants)
            {
                TxComponent txComponent = (TxComponent)txObject;
                string componentPath = txComponent.GetComponentPath();
                if (!string.IsNullOrEmpty(componentPath) && componentPath.Equals(searchPath))
                {
                    list.Add(txComponent);
                }
            }
            return list;
        }
    }
}
