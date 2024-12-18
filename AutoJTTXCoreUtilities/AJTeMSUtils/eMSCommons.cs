using EMPAPPLICATIONLib;
using EMPMODELLib;
using EMPTYPELIBRARYLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Tecnomatix.Engineering;
using Tecnomatix.Planning;

namespace AutoJTTXCoreUtilities
{
    public class eMSCommons
    {
        #region ctor

        //mfgroot
        //Tecnomatix.Engineering.TxMfgRoot TxMfgRoot;
        //appliction
        public EmpApplicationClass app;
        //context
        public EmpContext context;


        public DataTable DataTable_EngineeringWeldPoint;
        //所有需要计算的TxWeldPoint
        List<TxWeldPoint> txWeldPoints;


        //Node
        EmpNodeClass m_empNodeClass;
        //MfgFeature
        EmpMfgFeatureClass m_empMfgFeatureClass;

        public eMSCommons()
        {
            this.InitialContextMethod();
        }

        public eMSCommons(List<TxWeldPoint> txWeldPoints, bool isOffline = false)
        {
            try
            {
                try
                {
                    //appliction
                    this.app = new EmpApplicationClass();
                    //context
                    this.context = this.app.Context;

                    EmpContextData currentContextData = this.app.get_CurrentContextData();
                }
                catch (Exception ex)
                {
                    if (!isOffline)
                    {
                        throw ex;
                    }
                }

                //txWeldPoints
                this.txWeldPoints = txWeldPoints;
                this.DataTable_EngineeringWeldPoint = getAllWeldpointsInLibrary(isOffline);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public eMSCommons(bool isMfgFeature)
        {
            if (isMfgFeature)
            {
                this.InitialContextMethod();

                this.m_empNodeClass = new EmpNodeClass();
                this.m_empMfgFeatureClass = new EmpMfgFeatureClass();
            }
            else
            {
                this.InitialContextMethod();
            }
        }
        void InitialContextMethod()
        {
            try
            {
                //appliction
                this.app = new EmpApplicationClass();
                //context
                this.context = this.app.Context;

                EmpContextData currentContextData = this.app.get_CurrentContextData();
            }
            catch (Exception ex)
            {
                throw new Exception("EmpApplicationClass ERROR" + "\n" + ex.Message);
            }
        }

        #endregion

        /// <summary>
        /// 通过External ID获取node name 
        /// </summary>
        /// <param name="strExternalID"></param>
        /// <returns></returns>
        public string GetNameOfItemOnEMS(string strExternalID)
        {
            string result;
            try
            {
                //Node
                EmpNodeClass empNodeClass = new EmpNodeClass();

                EmpObjectKey nodeByExternalID = empNodeClass.GetNodeByExternalID(ref this.context, strExternalID);
                EmpObjectKey empObjectKey = nodeByExternalID;

                result = empNodeClass.get_Name(ref this.context, ref empObjectKey);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                result = string.Empty;
            }
            return result;
        }

        /// <summary>
        /// Proplanner API 返回对象InternalID，可以从  Planning representation 中检索
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int getPlanningInternalID(ITxPlanningObject obj)
        {
            int internalID = 0;
            if (obj != null)
            {
                TxEmsServicesProvider service = obj.PlatformServicesProvider as TxEmsServicesProvider;
                internalID = service.InternalId;
            }
            return internalID;
        }
        /// <summary>
        /// Proplanner API 返回对象ITxPlanningObject，可以从 internalID 中检索
        /// </summary>
        /// <param name="internalID"></param>
        /// <returns></returns>
        public static ITxPlanningObject getPlanningObjectByInternalID(int internalID)
        {
            TxEmsGlobalServicesProvider service = new TxEmsGlobalServicesProvider();
            return service.GetObjectByInternalId(internalID);
        }
        /// <summary>
        /// Proplanner API 返回对象ITxPlanningObject，可以从 ITxObject 中检索
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ITxPlanningObject GetObjectPlanningRepresentation(ITxObject obj)
        {
            ITxProcessModelObject proccesObject = obj as ITxProcessModelObject;
            ITxPlanningObject planningObject = obj as ITxPlanningObject;
            if (planningObject == null && proccesObject != null)
            {
                planningObject = proccesObject.PlanningRepresentation;
            }
            return planningObject;
        }
        /// <summary>
        ///TxObjectList planningObjects = TxApplication.ActiveSelection.GetPlanningItems();
        ///ImportFields(planningObjects);
        /// </summary>
        /// <param name="planningObjects"></param>
        public static void ImportFields(TxObjectList planningObjects)
        {
            ITxPlatformGlobalServicesProvider provider = TxApplication.ActiveDocument.PlatformGlobalServicesProvider as ITxPlatformGlobalServicesProvider;
            if (provider.IsPlanningTypeMetaDataInitialized())
            {
                ArrayList classFieldsToCollectList = new ArrayList();

                foreach (ITxPlanningObject plObject in planningObjects)
                {
                    string planningType = plObject.PlanningType;
                    TxPlanningTypeMetaData typeMetaData = provider.GetTypeMetaData(planningType);
                    ArrayList allFields = typeMetaData.AllFields;

                    if (allFields.Count > 0)
                    {
                        ArrayList fields = new ArrayList();
                        foreach (TxPlanningTypeFieldData field in allFields) { fields.Add(field.Name); }
                        TxClassFieldsCollectionData classFieldsToCollect = new TxClassFieldsCollectionData();
                        classFieldsToCollect.ClassName = planningType;
                        classFieldsToCollect.FieldNames = fields;
                        classFieldsToCollectList.Add(classFieldsToCollect);
                    }
                }

                if (classFieldsToCollectList.Count > 0)
                {
                    TxImportFieldsByClassData importData = new TxImportFieldsByClassData();
                    importData.addFieldsToCollect(classFieldsToCollectList);
                    importData.Objects = planningObjects;
                    TxApplication.ActiveDocument.PlatformGlobalServicesProvider.ImportFields(importData);
                }
            }
        }

        //遍历选中的MfgLibaray的 id,name ,libName
        public int GetMfgLibraryPointName_PD(string _uuid,
            DataTable tx_pro_mod_lib_mcm,
            TxObjectList txPlanningObjects,
            string _openid, out string infos)
        {
            int result = 0;
            infos = string.Empty;

            try
            {
                //遍历 Planning Object
                foreach (ITxPlanningObject txPlanningObject in txPlanningObjects)
                {
                    try
                    {
                        //mfgLibrary_Name
                        string mfgLibrary_Name = txPlanningObject.Name;

                        //Node
                        EmpNodeClass empNodeClass = new EmpNodeClass();
                        //MfgFeature
                        EmpMfgFeatureClass empMfgFeatureClass = new EmpMfgFeatureClass();
                        //MfgLibrary
                        EmpMfgLibraryClass empMfgLibraryClass = new EmpMfgLibraryClass();


                        //node
                        EmpObjectKey nodeByExternalID = empNodeClass.GetNodeByExternalID(ref this.context, txPlanningObject.ProcessModelId.ExternalId);
                        //MfgFeature children
                        EmpObjectKey[] children = empMfgLibraryClass.GetChildren(ref this.context, ref nodeByExternalID);

                        //遍历所有的 MfgFeature
                        for (int i = 0; i < children.Length; i++)
                        {
                            if (children[i].objectId > 1)
                            {
                                EmpEnumCOState empEnumCOState = empNodeClass.get_CheckInOutState(ref this.context, ref children[i]);
                                //The node is checked out by me
                                if (empEnumCOState == EmpEnumCOState.CO_Me_State)//The selected node not currently checked out by you.
                                {
                                    //weldPoint_Name
                                    string currentMfgFeatureName = empMfgFeatureClass.get_Name(ref this.context, ref children[i]);
                                    //externalID
                                    string externalID = empMfgFeatureClass.ExternalID[ref this.context, ref children[i]];

                                    this.DatatableSetDate(ref tx_pro_mod_lib_mcm, _uuid, externalID, currentMfgFeatureName, mfgLibrary_Name, _openid);
                                    result++;
                                }
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                infos = "successfully";
            }
            catch (Exception ex)
            {
                infos = "Failed" +
                    "\n" +
                    ex.Message;
                //MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

            return result;
        }
        public int GetMfgLibraryPointName_PS(string _uuid,
            DataTable tx_pro_mod_lib_mcm,
            TxObjectList txPlanningObjects,
            string _openid, out string infos)
        {
            int result = 0;
            infos = string.Empty;

            try
            {
                //遍历 Planning Object
                foreach (ITxPlanningObject txPlanningObject in txPlanningObjects)
                {
                    try
                    {
                        //mfgLibrary_Name
                        string mfgLibrary_Name = txPlanningObject.GetField("name").ToString();

                        //Node
                        EmpNodeClass empNodeClass = new EmpNodeClass();
                        //MfgFeature
                        EmpMfgFeatureClass empMfgFeatureClass = new EmpMfgFeatureClass();
                        //MfgLibrary
                        EmpMfgLibraryClass empMfgLibraryClass = new EmpMfgLibraryClass();


                        //node
                        EmpObjectKey nodeByExternalID = empNodeClass.GetNodeByExternalID(ref this.context, txPlanningObject.ProcessModelId.ExternalId);
                        //MfgFeature children
                        EmpObjectKey[] children = empMfgLibraryClass.GetChildren(ref this.context, ref nodeByExternalID);

                        //遍历所有的 MfgFeature
                        for (int i = 0; i < children.Length; i++)
                        {
                            if (children[i].objectId > 1)
                            {
                                EmpEnumCOState empEnumCOState = empNodeClass.get_CheckInOutState(ref this.context, ref children[i]);
                                //The node is checked out by me
                                if (empEnumCOState == EmpEnumCOState.CO_Me_State)//The selected node not currently checked out by you.
                                {
                                    //weldPoint_Name
                                    string currentMfgFeatureName = empMfgFeatureClass.get_Name(ref this.context, ref children[i]);
                                    //externalID
                                    string externalID = empMfgFeatureClass.ExternalID[ref this.context, ref children[i]];

                                    this.DatatableSetDate(ref tx_pro_mod_lib_mcm, _uuid, externalID, currentMfgFeatureName, mfgLibrary_Name, _openid);
                                    result++;
                                }
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                infos = "successfully";
            }
            catch (Exception ex)
            {
                infos = "Failed" +
                    "\n" +
                    ex.Message;
                //MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

            return result;
        }

        private void DatatableSetDate(ref DataTable dataTable12,
            string uuid,
            string externalID,
            string weldPoint_Name, string mfgLibrary_Name, string openID)
        {
            DataRow dataRow = dataTable12.NewRow();

            dataRow[0] = uuid;
            dataRow[1] = externalID;
            dataRow[2] = weldPoint_Name;
            dataRow[3] = mfgLibrary_Name;
            dataRow[4] = openID;

            dataTable12.Rows.Add(dataRow);
        }

        public void GetMfgLibraryPointName(ref DataTable dataTable1)
        {
            try
            {
                //Node
                EmpNodeClass empNodeClass = new EmpNodeClass();
                //MfgFeature
                EmpMfgFeatureClass empMfgFeatureClass = new EmpMfgFeatureClass();

                //Planning Object
                ITxPlanningObject txPlanningObject = TxApplication.ActiveSelection.GetAllItems()[0] as ITxPlanningObject;
                //node
                EmpObjectKey nodeByExternalID = empNodeClass.GetNodeByExternalID(ref this.context, txPlanningObject.ProcessModelId.ExternalId);

                //MfgLibrary
                EmpMfgLibraryClass empMfgLibraryClass = new EmpMfgLibraryClass();
                //Mfg children
                EmpObjectKey[] children = empMfgLibraryClass.GetChildren(ref this.context, ref nodeByExternalID);


                for (int i = 0; i < children.Length; i++)
                {
                    if (children[i].objectId > 1)
                    {
                        EmpEnumCOState empEnumCOState = empNodeClass.get_CheckInOutState(ref this.context, ref children[i]);
                        //The node is checked out by me
                        if (empEnumCOState == EmpEnumCOState.CO_Me_State)
                        {
                            string currentMfgFeatureName = empMfgFeatureClass.get_Name(ref this.context, ref children[i]);

                            #region reference

                            //empMfgFeatureClass.set_Name(ref context, ref children[i], "test_"+ str);

                            //EmpTreeControl empTreeControl;
                            //EmpTreeParams empTreeParams = new EmpTreeParams();
                            //empTreeParams.Init(,);
                            //EmpAppItemList treeItemList;
                            //empTreeControl.LoadTreeItemsFromKey(ref children[i],);
                            //TxApplication.ViewersManager.get

                            //EmpObjectKey empObjectKey22 = empMfgFeatureClass.GetThreeDRep(ref context, ref children[i]);                                                  

                            //GetAllDescendants(new TxTypeFilter(typeof(ITxComponent)));



                            //TxEmsClipboardData clipboardData = new TxEmsClipboardData();
                            //clipboardData. = new TxObjectList() { getPlanningObjectByInternalID(children[i].objectId) };
                            //TxEmsDragAndDropClipboardServices services = new TxEmsDragAndDropClipboardServices();
                            //System.Windows.Forms.IDataObject dataObject;
                            //services.CopyToDataObject(clipboardData, out dataObject);
                            //DoDragDrop(dataObject, DragDropEffects.Copy | DragDropEffects.Link | DragDropEffects.Move);



                            //empMfgFeatureClass.AddDoingOperation()


                            //TxApplication.CommandsManager.ExecuteCommand("Scope.Add_Root");

                            /*
                            TxPhysicalRoot txPhysicalRoot = TxApplication.ActiveDocument.PhysicalRoot;
                            ITxPlanningObject txPlanningObject1 = getPlanningObjectByInternalID(children[i].objectId);
                            Tecnomatix.Planning.TxPlanningMfgFeature txPlanningMfgFeature = txPlanningObject1 as TxPlanningMfgFeature;
                            EmpObjectKey empObjectKey111 = empMfgFeatureClass.CreateThreeDRep(ref context, ref children[i], @"C:\Program Files\Tecnomatix_15.1\eMPower\NewWorld\Data\defaultMfg.co");
                            

                            //MfgRoot
                            Tecnomatix.Engineering.TxMfgRoot txMfgRoot = TxApplication.ActiveDocument.MfgRoot;
                             
                             */

                            #endregion


                            //EmpLocation
                            EmpLocation empLocation = empMfgFeatureClass.get_Location(ref this.context, ref children[i]);
                            TxColor txColorRed = TxColor.TxColorRed;

                            CreateWeldLocOperation(currentMfgFeatureName, null, txColorRed, null);


                            #region reference

                            /*
                            Tecnomatix.Engineering.Utilities.TxEmsCacheManager cacheManager = new Tecnomatix.Engineering.Utilities.TxEmsCacheManager();
                            Tecnomatix.Engineering.Utilities.TxEmsTraversalManager traversalManger = new Tecnomatix.Engineering.Utilities.TxEmsTraversalManager();

                            TxObjectList mfgs = TxApplication.ActiveSelection.GetFilteredItems(new TxTypeFilter(typeof(TxWeldPoint)));
                            TxObjectList planningList = mfgs.PlanningRepresentations;




                            TxObjectList parts = TxApplication.ActiveDocument.PhysicalRoot.GetAllDescendants(new TxTypeFilter(typeof(TxComponent)));
                            TxObjectList pla = parts.PlanningRepresentations;
                            TxTypeFilter partsFilter = new TxTypeFilter(typeof(TxPlanningPart));
                            TxObjectList plan = partsFilter.Filter(pla);
                            TxObjectList pParts = plan.EngineeringRepresentations;
                           


                            if (planningList.Count > 0)
                            {
                                ITxPlanningObject planning = planningList[0] as ITxPlanningObject;
                                cacheManager.Clear();
                                cacheManager["test"].SetRootObject(planning);
                                cacheManager["test"].SetPath("threeDRep", "file");
                                cacheManager["test"].SetAttributes("fileName");

                                traversalManger["test"].Clear();
                                traversalManger["test"].SetRootObject(planning);
                                traversalManger["test"].SetPath("threeDRep", "file");
                                traversalManger["test"].SetAttributes("fileName");

                                string path3D = string.Empty;
                                try
                                {
                                    cacheManager.CacheData();
                                    traversalManger.Traverse();
                                    Tecnomatix.Engineering.Utilities.TxEmsTraversalObjectResultData data = traversalManger["test"].GetTraversalObjectData(planning);
                                    ArrayList values = data.GetAttributeValues("fileName");
                                    if (values != null && values.Count > 0)
                                    {
                                        path3D = values[0] as string;
                                    }
                                }
                                catch{}

                                MessageBox.Show(path3D, "Test");
                            }

                            //TxApplication.ActiveDocument.PhysicalRoot.AddObject((ITxObject)weldLocOpData);
                            //txPhysicalRoot.Collection.AddObject(txPlanningObject1);// InsertComponent(new TxInsertComponentCreationData(str, @"C:\Program Files\Tecnomatix_15.1\eMPower\NewWorld\Data\defaultMfg.co"));
                            txPlanningObject1.Delete();

                            */
                            #endregion


                        }
                        else
                        {
                            MessageBox.Show("The selected node not currently checked out by you.", "Error");
                        }
                    }
                }

                //刷新wiew
                //TxApplication.ViewersManager.GetActiveViewer().Refresh();
                TxApplication.RefreshDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        //创建标签
        void CreateWeldLocOperation(string currentMfgFeatureName, TxVector location, TxColor txColor, TxComponent txComponent)
        {
            try
            {
                #region plan a 创建operation

                /*
                //ITxObjectCollection
                ITxObjectCollection collection_root = TxApplication.ActiveDocument.OperationRoot;

                //ITxWeldPointCreationData
                TxTransformation txTransformation = MathUtilities.CreateRpyTransformation(empLocation.X, empLocation.Y, empLocation.Z, empLocation.RX, empLocation.RY, empLocation.RZ);
                Tecnomatix.Engineering.DataTypes.ITxWeldPointCreationData weldPointData = Tecnomatix.Engineering.DataTypes.TxMfgCreationDataFactory.CreateWeldPointCreationData(currentMfgFeatureName, txTransformation);



                //TxWeldLocationOperationCreationData
                TxWeldLocationOperationCreationData weldLocData = new TxWeldLocationOperationCreationData(currentMfgFeatureName);
                weldLocData.WeldPointCreationData = weldPointData;

                //ITxWeldLocationOperationCreation
                ITxWeldLocationOperationCreation weldLocCreation = collection_root as ITxWeldLocationOperationCreation;
                TxWeldLocationOperation weldLoc = weldLocCreation.CreateWeldLocationOperation(weldLocData);
                weldLoc.Color = txColor;
                weldLoc.WeldPoint.Color = txColor;

                //创建note
                TxLocationNoteCreationData creationData = new TxLocationNoteCreationData(weldLoc.Name, weldLoc);
                TxNote txLocationNote = TxApplication.ActiveDocument.PhysicalRoot.CreateNote(creationData);
                txLocationNote.Display();
                */

                #endregion

                #region plan b 创建 solid

                //创建球
                TxSolid txSolid1 = txComponent.CreateSolidSphere(new TxSphereCreationData(currentMfgFeatureName, location, 6));
                if (txSolid1 != null)
                {
                    txSolid1.Display();
                    txSolid1.Color = txColor;

                    //创建note
                    TxLocationNoteCreationData creationData = new TxLocationNoteCreationData(currentMfgFeatureName, txSolid1);
                    TxNote txLocationNote = TxApplication.ActiveDocument.PhysicalRoot.CreateNote(creationData);

                    txLocationNote.OutlineColor = txColor;
                    txLocationNote.BackgroundColor = txColor;
                    txLocationNote.TextColor = new TxColor(TxColor.TxColorWhite);
                    txLocationNote.Display();
                }

                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //显示选中的MfgFeature的位置
        public bool ShowSelectedMfgFeatureLoc(string externalID, TxColor txColor, TxComponent txComponent, out string errorInfo)
        {
            bool result = false;
            errorInfo = string.Empty;

            try
            {
                //MfgFeature
                EmpObjectKey nodeByExternalID = this.m_empNodeClass.GetNodeByExternalID(ref this.context, externalID);

                if (nodeByExternalID.objectId == 0)
                {
                    errorInfo = string.Format("ExternalID -- {0} 在焊点库中匹配不到", externalID);
                    return false;
                }

                //EmpLocation
                EmpLocation empLocation = this.m_empMfgFeatureClass.get_Location(ref this.context, ref nodeByExternalID);
                string currentMfgFeatureName = this.m_empMfgFeatureClass.get_Name(ref this.context, ref nodeByExternalID);

                //主形状
                //EmpObjectKey threedrdp = empMfgFeatureClass.GetThreeDRep(ref this.context, ref nodeByExternalID);
                //IEmpExternalDocument empExternalDocumentClass = new EmpExternalDocumentClass();
                //string cojtpath = empExternalDocumentClass.get_Filename(ref this.context, ref threedrdp);

                CreateWeldLocOperation(currentMfgFeatureName, new TxVector(empLocation.X, empLocation.Y, empLocation.Z), txColor, txComponent);
                result = true;
            }
            catch (Exception ex)
            {
                errorInfo = string.Format("ExternalID -- {0} {1}", externalID, ex.Message);
                return false;
            }
            return result;
        }
        //获取选中的MfgFeature的位置
        public int GetSelectedMfgFeatureLoc(DataTable dataTable_issue)
        {
            int result = 0;
            try
            {
                //Node
                EmpNodeClass empNodeClass = new EmpNodeClass();
                //MfgFeature
                EmpMfgFeatureClass empMfgFeatureClass = new EmpMfgFeatureClass();

                foreach (DataRow item in dataTable_issue.Rows)
                {
                    try
                    {
                        //MfgFeature
                        EmpObjectKey nodeByExternalID = empNodeClass.GetNodeByExternalID(ref this.context, item["externalID"].ToString());

                        if (nodeByExternalID.objectId == 0)
                        {
                            continue;
                            //throw new Exception(item + "\n" + "未找到");
                        }

                        //EmpLocation
                        EmpLocation empLocation = empMfgFeatureClass.get_Location(ref this.context, ref nodeByExternalID);
                        item["X"] = empLocation.X;
                        item["Y"] = empLocation.Y;
                        item["Z"] = empLocation.Z;

                        result++;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }


        //显示选中的MfgFeature的位置
        public List<TxMfgFeature> ShowSelectedMfgFeatureLoc_PS(List<string> WPexternalID)
        {
            //要显示的list
            List<TxMfgFeature> txMfgs = new List<TxMfgFeature>();

            try
            {
                //MfgRoot
                TxMfgRoot txMfgRoot = TxApplication.ActiveDocument.MfgRoot;

                //隐藏所有
                foreach (TxMfgFeature item in txMfgRoot)
                {
                    item.Blank();
                }

                //查找id
                foreach (string item in WPexternalID)
                {
                    try
                    {
                        foreach (TxMfgFeature item2 in txMfgRoot)
                        {
                            if (item == item2.ProcessModelId.ExternalId)
                            {
                                txMfgs.Add(item2);
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
            catch (Exception)
            {
                throw;
            }
            return txMfgs;
        }
        //获取选中的MfgFeature的位置
        public int GetSelectedMfgFeatureLoc_PS(DataTable dataTable_issue)
        {
            //要显示的list
            int result = 0;

            try
            {
                //MfgRoot
                TxMfgRoot txMfgRoot = TxApplication.ActiveDocument.MfgRoot;

                //查找id
                foreach (DataRow item in dataTable_issue.Rows)
                {
                    try
                    {
                        string externalID1 = item["externalID"].ToString();

                        foreach (TxMfgFeature item2 in txMfgRoot)
                        {
                            if (externalID1 == item2.ProcessModelId.ExternalId)
                            {
                                TxVector transformation = item2.AbsoluteLocation.Translation;

                                item["X"] = transformation.X;
                                item["Y"] = transformation.Y;
                                item["Z"] = transformation.Z;

                                result++;

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
            catch (Exception)
            {
                throw;
            }

            return result;
        }


        //显示选中的MfgFeature的位置
        public int ShowSelectedMfgFeatureLoc_PS(List<string> WPexternalID, TxColor txColor)
        {
            int result = 0;
            try
            {
                //MfgRoot
                TxMfgRoot txMfgRoot = TxApplication.ActiveDocument.MfgRoot;

                //隐藏所有
                foreach (TxMfgFeature item in txMfgRoot)
                {
                    item.Blank();
                }

                //查找id
                foreach (string item in WPexternalID)
                {
                    try
                    {
                        foreach (TxMfgFeature item2 in txMfgRoot)
                        {
                            if (item == item2.ProcessModelId.ExternalId)
                            {
                                //设置颜色
                                item2.Color = txColor;

                                //创建note
                                TxLocationNoteCreationData creationData = new TxLocationNoteCreationData(item2.Name, item2);
                                TxNote txLocationNote = TxApplication.ActiveDocument.PhysicalRoot.CreateNote(creationData);

                                item2.Display();
                                txLocationNote.Display();
                                result++;
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
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        //获取MFGWeldPoint的Location, 通过 InternalId
        public double[] GetMFGWeldPointLocation(string strExternalId)
        {
            double[] result = new double[3];

            try
            {
                //empWeldPointClass
                EmpWeldPointClass empWeldPointClass = new EmpWeldPointClass();
                //MfgFeature
                //EmpMfgFeatureClass empMfgFeatureClass = new EmpMfgFeatureClass();
                //Node
                EmpNodeClass empNodeClass = new EmpNodeClass();
                //EmpObjectKey
                EmpObjectKey nodeByExternalID = empNodeClass.GetNodeByExternalID(ref this.context, strExternalId);


                EmpLocation empLocation = empWeldPointClass.get_Location(ref this.context, nodeByExternalID);

                result[0] = empLocation.X;
                result[1] = empLocation.Y;
                result[2] = empLocation.Z;
            }
            catch
            {
                return null;
            }

            return result;
        }
        //合并提取
        public void GetMFGWeldPointLocation(string strExternalId_1, string strExternalId_2, out double[] loc1, out double[] loc2)
        {
            loc1 = null;
            loc2 = null;
            try
            {
                //empWeldPointClass
                EmpWeldPointClass empWeldPointClass = new EmpWeldPointClass();
                //MfgFeature
                //EmpMfgFeatureClass empMfgFeatureClass = new EmpMfgFeatureClass();
                //Node
                EmpNodeClass empNodeClass = new EmpNodeClass();
                //EmpObjectKey
                EmpObjectKey nodeByExternalID_1 = empNodeClass.GetNodeByExternalID(ref this.context, strExternalId_1);
                EmpLocation empLocation_1 = empWeldPointClass.get_Location(ref this.context, nodeByExternalID_1);

                //EmpObjectKey
                EmpObjectKey nodeByExternalID_2 = empNodeClass.GetNodeByExternalID(ref this.context, strExternalId_2);
                EmpLocation empLocation_2 = empWeldPointClass.get_Location(ref this.context, nodeByExternalID_2);

                loc1 = new double[3];
                loc1[0] = empLocation_1.X;
                loc1[1] = empLocation_1.Y;
                loc1[2] = empLocation_1.Z;

                loc2 = new double[3];
                loc2[0] = empLocation_2.X;
                loc2[1] = empLocation_2.Y;
                loc2[2] = empLocation_2.Z;
            }
            catch
            {

            }
        }

        //获取MFGWeldPoint的Location, 通过 InternalId
        public void GetMFGWeldPointLocation2(string strExternalId1, string strExternalId2, out double[] result1, out double[] result2)
        {
            result1 = null;
            result2 = null;

            try
            {
                //自定义筛选器
                TxTypeFilter filter_mfg = null;
                filter_mfg = new TxTypeFilter(new Type[] { typeof(TxWeldPoint) });//筛选焊点

                if (this.txWeldPoints != null && this.txWeldPoints.Count != 0)
                {
                    //TxObjectList txObjects = this.TxMfgRoot.GetAllDescendants(filter_mfg);

                    foreach (TxWeldPoint item in this.txWeldPoints)
                    {
                        if (result1 != null && result2 != null)
                        {
                            break;
                        }
                        if (item.ProcessModelId.ExternalId == strExternalId1)
                        {
                            TxVector txVector = item.AbsoluteLocation.Translation;
                            result1 = new double[3];
                            result1[0] = txVector.X;
                            result1[1] = txVector.Y;
                            result1[2] = txVector.Z;
                        }
                        if (item.ProcessModelId.ExternalId == strExternalId2)
                        {
                            TxVector txVector = item.AbsoluteLocation.Translation;
                            result2 = new double[3];
                            result2[0] = txVector.X;
                            result2[1] = txVector.Y;
                            result2[2] = txVector.Z;
                        }
                    }
                }
            }
            catch
            {
                result1 = null;
                result2 = null;
            }

        }

        //获取所有Engineering中的焊点的emp坐标
        DataTable getAllWeldpointsInLibrary(bool isOffline = false)
        {
            try
            {
                this.DataTable_EngineeringWeldPoint = new DataTable();
                //自定义筛选器
                //TxTypeFilter filter_mfg = null;
                //filter_mfg = new TxTypeFilter(new Type[] { typeof(TxWeldPoint) });//筛选焊点

                if (this.txWeldPoints != null && this.txWeldPoints.Count != 0)
                {
                    //TxObjectList txObjects = this.TxMfgRoot.GetAllDescendants(filter_mfg);

                    DataColumn dataColumn0 = new DataColumn("WeldName", typeof(string));
                    DataTable_EngineeringWeldPoint.Columns.Add(dataColumn0);

                    DataColumn dataColumn1 = new DataColumn("X", typeof(double));
                    DataTable_EngineeringWeldPoint.Columns.Add(dataColumn1);
                    DataColumn dataColumn2 = new DataColumn("Y", typeof(double));
                    DataTable_EngineeringWeldPoint.Columns.Add(dataColumn2);
                    DataColumn dataColumn3 = new DataColumn("Z", typeof(double));
                    DataTable_EngineeringWeldPoint.Columns.Add(dataColumn3);
                    DataColumn dataColumn4 = new DataColumn("ExtemalID", typeof(string));
                    DataTable_EngineeringWeldPoint.Columns.Add(dataColumn4);


                    //empWeldPointClass
                    EmpWeldPointClass empWeldPointClass = new EmpWeldPointClass();
                    //MfgFeature
                    //EmpMfgFeatureClass empMfgFeatureClass = new EmpMfgFeatureClass();
                    //Node
                    EmpNodeClass empNodeClass = new EmpNodeClass();


                    foreach (TxWeldPoint item in this.txWeldPoints)
                    {
                        try
                        {
                            string externaId = item.ProcessModelId.ExternalId;

                            //加入一行数据
                            DataRow dataRow = DataTable_EngineeringWeldPoint.NewRow();

                            //判断是否是offline
                            if (isOffline)
                            {
                                dataRow[0] = item.Name;
                                dataRow[1] = item.AbsoluteLocation.Translation.X;
                                dataRow[2] = item.AbsoluteLocation.Translation.Y;
                                dataRow[3] = item.AbsoluteLocation.Translation.Z;
                                dataRow[4] = externaId;
                            }
                            else
                            {
                                //EmpObjectKey
                                EmpObjectKey nodeByExternalID_1 = empNodeClass.GetNodeByExternalID(ref this.context, externaId);
                                EmpLocation empLocation_1 = empWeldPointClass.get_Location(ref this.context, nodeByExternalID_1);


                                dataRow[0] = item.Name;
                                dataRow[1] = empLocation_1.X;
                                dataRow[2] = empLocation_1.Y;
                                dataRow[3] = empLocation_1.Z;
                                dataRow[4] = externaId;
                            }                           

                            DataTable_EngineeringWeldPoint.Rows.Add(dataRow);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }

                return DataTable_EngineeringWeldPoint;
            }
            catch
            {
                return null;
            }
        }

        //从datatable中查询
        public void GetMFGWeldPointLocation4DT(string strExternalId_1, string strExternalId_2, out double[] loc1, out double[] loc2)
        {
            loc1 = null;
            loc2 = null;
            try
            {
                DataRow[] loc_tmp1 = this.DataTable_EngineeringWeldPoint.Select("ExtemalID = '" +
                    strExternalId_1 +
                    "'");
                DataRow[] loc_tmp2 = this.DataTable_EngineeringWeldPoint.Select("ExtemalID = '" +
                  strExternalId_2 +
                  "'");



                loc1 = new double[3];
                loc1[0] = Convert.ToDouble(loc_tmp1[0]["X"]);
                loc1[1] = Convert.ToDouble(loc_tmp1[0]["Y"]);
                loc1[2] = Convert.ToDouble(loc_tmp1[0]["Z"]);

                loc2 = new double[3];
                loc2[0] = Convert.ToDouble(loc_tmp2[0]["X"]);
                loc2[1] = Convert.ToDouble(loc_tmp2[0]["Y"]);
                loc2[2] = Convert.ToDouble(loc_tmp2[0]["Z"]);
            }
            catch
            {

            }
        }
        public void GetMFGWeldPointLocation4DT(string strExternalId_1, out double[] loc1)
        {
            loc1 = null;
            try
            {
                DataRow[] loc_tmp1 = this.DataTable_EngineeringWeldPoint.Select("ExtemalID = '" +
                    strExternalId_1 +
                    "'");

                loc1 = new double[3];
                loc1[0] = Convert.ToDouble(loc_tmp1[0]["X"]);
                loc1[1] = Convert.ToDouble(loc_tmp1[0]["Y"]);
                loc1[2] = Convert.ToDouble(loc_tmp1[0]["Z"]);

            }
            catch
            {

            }
        }

        public bool CheckIsOpenProject()
        {
            return this.app.IsProjectOpen();
        }

        //获取用户选中的lib的
        public static Dictionary<string, double> GetMfgLibraryPoint(EmpContext context, ITxPlanningObject txPlanningObject)
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            GetPointMethod(context, new EmpNodeClass().GetNodeByExternalID(ref context, txPlanningObject.ProcessModelId.ExternalId), ref result);

            return result;
        }

        private static void GetPointMethod(EmpContext context, EmpObjectKey txPlanningObject, ref Dictionary<string, double> result)
        {
            //Node
            EmpNodeClass empNodeClass = new EmpNodeClass();
            //MfgFeature
            EmpMfgFeatureClass empMfgFeatureClass = new EmpMfgFeatureClass();
            //MfgLibrary
            EmpMfgLibraryClass empMfgLibraryClass = new EmpMfgLibraryClass();

            //node
            EmpObjectKey nodeByExternalID = txPlanningObject;//empNodeClass.GetNodeByExternalID(ref context, txPlanningObject.ProcessModelId.ExternalId);
            //MfgFeature children
            EmpObjectKey[] children = empMfgLibraryClass.GetChildren(ref context, ref nodeByExternalID);

            //遍历所有的 MfgFeature
            for (int i = 0; i < children.Length; i++)
            {
                try
                {
                    if (children[i].objectId > 1)
                    {
                        //weldPoint_Name
                        string currentMfgFeatureName = empMfgFeatureClass.get_Name(ref context, ref children[i]);
                        //externalID
                        string externalID = empMfgFeatureClass.ExternalID[ref context, ref children[i]];

                        //get type
                        string type = AJTeMServer.GetEmpObjectKeyType(children[i]);

                        //还是焊点库
                        if (type.Equals("MfgLibrary"))
                        {
                            //递归
                            GetPointMethod(context, children[i], ref result);
                        }

                        //localtion
                        EmpLocation emploc = empMfgFeatureClass.get_Location(ref context, ref children[i]);

                        result.Add(currentMfgFeatureName, emploc.Y);
                    }
                }
                catch
                {
                    continue;
                }
            }
        }
    }
}