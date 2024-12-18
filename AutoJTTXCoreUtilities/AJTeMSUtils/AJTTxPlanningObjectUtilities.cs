using EMPTYPELIBRARYLib;
using System;
using System.Collections;
using Tecnomatix.Engineering;
using Tecnomatix.Planning;

namespace AutoJTTXCoreUtilities
{
    public class AJTTxPlanningObjectUtilities
    {
        public static string GetPlanningType(ITxProcessModelObject obj)
        {
            string result;
            if (obj != null && obj.PlanningRepresentation != null)
            {
                result = obj.PlanningRepresentation.PlanningType;
            }
            else
            {
                result = null;
            }
            return result;
        }

        public static ITxPlanningObject GetPlanningObject(ITxProcessModelObject obj)
        {
            ITxPlanningObject result;
            if (obj != null && obj.PlanningRepresentation != null)
            {
                result = obj.PlanningRepresentation;
            }
            else
            {
                result = null;
            }
            return result;
        }

        public static object GetFieldValue(ITxPlanningObject planningObject, string fieldName)
        {
            object result;
            if (planningObject != null && !string.IsNullOrEmpty(fieldName))
            {
                if (TxApplication.PlatformType != TxPlatformType.Offline)
                {
                    if (TxApplication.PlatformType == TxPlatformType.EmServer)
                    {
                        if (!planningObject.FieldWasImported(fieldName))
                        {
                            AJTTxPlanningObjectUtilities.ImportFieldName(planningObject, fieldName);
                        }
                        if (planningObject.FieldWasImported(fieldName))
                        {
                            return planningObject.GetField(fieldName);
                        }
                    }
                }
                else
                {
                    TxOfflineGlobalServicesProvider txOfflineGlobalServicesProvider = TxApplication.ActiveDocument.PlatformGlobalServicesProvider as TxOfflineGlobalServicesProvider;
                    if (txOfflineGlobalServicesProvider.AreFieldsImported && planningObject.FieldWasImported(fieldName))
                    {
                        return planningObject.GetField(fieldName);
                    }
                }
                result = null;
            }
            else
            {
                result = null;
            }
            return result;
        }

        public static TxPlanningTypeMetaData GetTypeMetaData(string planningType)
        {
            TxPlanningTypeMetaData result = null;
            ITxPlatformGlobalServicesProvider platformGlobalServicesProvider = TxApplication.ActiveDocument.PlatformGlobalServicesProvider;
            if (platformGlobalServicesProvider != null && platformGlobalServicesProvider.IsDataLoaded())
            {
                result = platformGlobalServicesProvider.GetTypeMetaData(planningType);
            }
            return result;
        }

        public static TxPlanningTypeMetaData GetTypeMetaData(ITxPlanningObject planningObject)
        {
            TxPlanningTypeMetaData result;
            if (planningObject != null && planningObject.PlanningType != null)
            {
                result = AJTTxPlanningObjectUtilities.GetTypeMetaData(planningObject.PlanningType);
            }
            else
            {
                result = null;
            }
            return result;
        }

        public static string GetObjectName(ITxObject obj)
        {
            string result;
            if (obj == null)
            {
                result = string.Empty;
            }
            else
            {
                string text = string.Empty;
                if (obj is ITxPlanningObject)
                {
                    if (!((ITxPlanningObject)obj).IsObjectLoaded())
                    {
                        goto IL_43;
                    }
                    try
                    {
                        text = obj.Name;
                        goto IL_43;
                    }
                    catch
                    {
                        goto IL_43;
                    }
                }
                text = obj.Name;
            IL_43:
                ITxPlanningObject txPlanningObject = null;
                if (!(obj is ITxPlanningObject))
                {
                    if (obj is ITxProcessModelObject)
                    {
                        txPlanningObject = ((ITxProcessModelObject)obj).PlanningRepresentation;
                    }
                }
                else
                {
                    txPlanningObject = (obj as ITxPlanningObject);
                }
                if (txPlanningObject != null)
                {
                    object fieldValue = AJTTxPlanningObjectUtilities.GetFieldValue(txPlanningObject, "name");
                    if (fieldValue != null && fieldValue is string)
                    {
                        text = (fieldValue as string);
                    }
                }
                result = text;
            }
            return result;
        }

        private static bool IsPrimitiveField(TxPlanningFieldType valType)
        {
            return valType == TxPlanningFieldType.Bool || valType == TxPlanningFieldType.Int16 ||
                valType == TxPlanningFieldType.Int32 || valType == TxPlanningFieldType.Int64 ||
                valType == TxPlanningFieldType.Float || valType == TxPlanningFieldType.Double ||
                valType == TxPlanningFieldType.String || valType == TxPlanningFieldType.Date;
        }

        public static object GetFieldValue(ITxPlanningObject planningObject, TxPlanningTypeFieldData fieldData)
        {
            object result;
            try
            {
                planningObject.FieldWasImported(fieldData.Name);
                result = ((!planningObject.FieldWasImported(fieldData.Name)) ? null : planningObject.GetField(fieldData.Name));
            }
            catch (Exception exception)
            {
                AJTTxMessageHandling.WriteException(exception);
                result = null;
            }
            return result;
        }

        public static void ImportFieldName(ITxPlanningObject planningObject, string fieldName)
        {
            try
            {
                if (TxApplication.ActiveDocument.PlatformGlobalServicesProvider is TxEmsGlobalServicesProvider)
                {
                    ArrayList arrayList = new ArrayList();
                    ArrayList arrayList2 = new ArrayList();
                    TxClassFieldsCollectionData txClassFieldsCollectionData = new TxClassFieldsCollectionData();
                    TxEmsImportFieldsData txEmsImportFieldsData = new TxEmsImportFieldsData();
                    TxEmsImportFieldsData txEmsImportFieldsData2 = txEmsImportFieldsData;
                    TxObjectList txObjectList = new TxObjectList();
                    txObjectList.Add(planningObject);
                    txEmsImportFieldsData2.Roots = txObjectList;
                    txClassFieldsCollectionData.ClassName = "PmNode";
                    arrayList.Add(fieldName);
                    txClassFieldsCollectionData.FieldNames = arrayList;
                    arrayList2.Add(txClassFieldsCollectionData);
                    txEmsImportFieldsData.addFieldsToCollect(arrayList2);
                    TxEmsGlobalServicesProvider txEmsGlobalServicesProvider = TxApplication.ActiveDocument.PlatformGlobalServicesProvider as TxEmsGlobalServicesProvider;
                    if (txEmsGlobalServicesProvider.CanImportFields)
                    {
                        txEmsGlobalServicesProvider.ImportOverwriteFields(txEmsImportFieldsData);
                    }
                }
            }
            catch (Exception exception)
            {
                AJTTxMessageHandling.WriteException(exception);
            }
        }

        //public static ITxPlanningToolPrototype GetPlanningToolPrototype(ITxPlanningObject planningObject)
        //{
        //    if (planningObject is TxPlanningToolInstance)
        //    {
        //        TxPlanningToolInstance instance = planningObject as TxPlanningToolInstance;
        //        ITxObject txObject = AJTTxComponentUtilities.InstanceGetPrototype(instance);
        //        if (txObject is ITxPlanningToolPrototype)
        //        {
        //            return (ITxPlanningToolPrototype)txObject;
        //        }
        //    }
        //    else if (planningObject is ITxPlanningToolPrototype)
        //    {
        //        return (ITxPlanningToolPrototype)planningObject;
        //    }
        //    return null;
        //}

        public static TxObjectList GetChildrens(ITxPlanningObject planningObject)
        {
            TxObjectList txObjectList = new TxObjectList();
            if (TxApplication.PlatformType != TxPlatformType.EmServer)
            {
                throw new NotSupportedException("Can't get childrens from database. Invalid platform type " + TxApplication.PlatformType.ToString() + ".");
            }
            int internalId = AJTTxPlanningObjectUtilities.GetInternalId(planningObject);
            if (internalId == 0)
            {
                throw new Exception("Can not get the internal id from the planning object.");
            }
            EmpContext empContext = new EmpContext
            {
                sessionId = TxApplication.ActiveDocument.eMSSessionId
            };
            EmpObjectKey empObjectKey = new EmpObjectKey
            {
                objectId = internalId
            };
            EmpObjectKey[] children = AJTeMServer.GetChildren(ref empContext, ref empObjectKey);
            TxEmsGlobalServicesProvider txEmsGlobalServicesProvider = TxApplication.ActiveDocument.PlatformGlobalServicesProvider as TxEmsGlobalServicesProvider;
            if (txEmsGlobalServicesProvider != null && children != null && children.Length != 0)
            {
                ArrayList arrayList = new ArrayList();
                foreach (EmpObjectKey empObjectKey2 in children)
                {
                    arrayList.Add(empObjectKey2.objectId);
                }
                txObjectList = txEmsGlobalServicesProvider.GetObjectsByInternalIds(arrayList);
            }
            return txObjectList;
        }

        public static int GetInternalId(ITxPlanningObject planningObject)
        {
            int result = 0;
            if (planningObject != null)
            {
                TxEmsServicesProvider txEmsServicesProvider = planningObject.PlatformServicesProvider as TxEmsServicesProvider;
                if (txEmsServicesProvider != null)
                {
                    result = txEmsServicesProvider.InternalId;
                }
            }
            return result;
        }

        //public static TxObjectList<ITxPlanningInstance> GetPlanningInstances(ITxPlanningObject planningObject)
        //{
        //    TxObjectList<ITxPlanningInstance> txObjectList = new TxObjectList<ITxPlanningInstance>();
        //    TxObjectList<ITxPlanningInstance> result;
        //    if (!(planningObject is ITxPlanningPrototype))
        //    {
        //        result = txObjectList;
        //    }
        //    else
        //    {
        //        txObjectList = ((ITxPlanningPrototype)planningObject).Instances;
        //        result = txObjectList;
        //    }
        //    return result;
        //}

        public static ITxPlanningObject Paste(ITxPlanningObject planningObject)
        {
            ITxPlanningObject result;
            if (planningObject != null)
            {
                if (planningObject.IsObjectLoaded())
                {
                    ITxObject txObject = AJTTxDocumentUtilities.Paste(planningObject.EngineeringRepresentation);
                    if (txObject == null)
                    {
                        AJTTxMessageHandling.WriteError("Cannot paste engineering representation of planning instance. " + AJTTxPlanningObjectUtilities.GetCaption(planningObject));
                        result = null;
                    }
                    else
                    {
                        if (txObject is ITxLocatableObject)
                        {
                            ((ITxLocatableObject)txObject).AbsoluteLocation = new TxTransformation();
                        }
                        result = AJTTxPlanningObjectUtilities.GetPlanningObject(txObject as ITxProcessModelObject);
                    }
                }
                else
                {
                    AJTTxMessageHandling.WriteError("Cannot paste planning instance, object is not loaded.");
                    result = null;
                }
            }
            else
            {
                AJTTxMessageHandling.WriteError("Cannot paste planning instance, is null.");
                result = null;
            }
            return result;
        }

        public static string GetCaption(ITxPlanningObject planningObject)
        {
            if (planningObject != null)
            {
                try
                {
                    return planningObject.Caption;
                }
                catch
                {
                    try
                    {
                        return planningObject.Name;
                    }
                    catch
                    {
                        return string.Empty;
                    }
                }
            }
            return string.Empty;
        }

        public static ITxPlanningObject GetPlanningObjtByInternalID(int internalID)
        {
            try
            {
                TxEmsGlobalServicesProvider txEmsGlobalServicesProvider = TxApplication.ActiveDocument.PlatformGlobalServicesProvider as TxEmsGlobalServicesProvider;
                if (txEmsGlobalServicesProvider != null)
                {
                    return txEmsGlobalServicesProvider.GetObjectByInternalId(internalID);
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }           
        }


        public static int GetInternalIdFromExternalId(string externalId)
        {
            int num = 0;
            ITxPlanningObject planningObjectFromExternalId = GetPlanningObjectFromExternalId(externalId);
            if (planningObjectFromExternalId != null)
            {
                TxEmsServicesProvider txEmsServicesProvider = planningObjectFromExternalId.PlatformServicesProvider as TxEmsServicesProvider;
                num = txEmsServicesProvider is null ? -1 : txEmsServicesProvider.InternalId;
            }
            return num;
        }

        public static ITxPlanningObject GetPlanningObjectFromExternalId(string externalId)
        {
            ITxPlanningObject txPlanningObject = null;
            if (externalId != null)
            {
                TxEmsGlobalServicesProvider txEmsGlobalServicesProvider = new TxEmsGlobalServicesProvider();
                TxProcessModelId txProcessModelId = new TxProcessModelId(externalId);
                txPlanningObject = txEmsGlobalServicesProvider.GetObjectByProcessModelId(txProcessModelId);
            }
            return txPlanningObject;
        }

        //获取external id
        public static string GetExternalID(ITxObject obj)
        {
            string result = null;

            if (obj is ITxProcessModelObject modelObject)
            {
                result = modelObject.PlanningRepresentation?.ProcessModelId?.ExternalId;
            }

            return result;
        }
    }
}