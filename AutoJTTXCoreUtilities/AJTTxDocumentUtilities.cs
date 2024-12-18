using AutoJTTXUtilities.DataHandling;
using AutoJTTXUtilities.PathHandling;
using EngineeringInternalExtension;
using EngineeringInternalExtension.Robotics;
using System;
using System.IO;
using System.Linq;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Ui;
using Tecnomatix.Planning;

namespace AutoJTTXCoreUtilities
{
    public class AJTTxDocumentUtilities
    {
        public static TxObjectList GetAllRobots(bool onlySelected)
        {
            return AJTTxDocumentUtilities.GetAllRobots(onlySelected, false);
        }

        public static TxObjectList GetAllRobots(bool onlySelected, bool includeEquipment)
        {
            TxObjectList result;
            if (onlySelected)
            {
                result = AJTTxDocumentUtilities.GetSelectedRobots(includeEquipment);
            }
            else
            {
                result = AJTTxDocumentUtilities.GetAllRobots();
            }
            return result;
        }

        public static TxObjectList GetAllRobots()
        {
            TxObjectList txObjectList = new TxObjectList();
            TxObjectList txObjectList2 = null;
            try
            {
                TxTypeFilter txTypeFilter = new TxTypeFilter(typeof(TxRobot));
                txObjectList2 = TxApplication.ActiveDocument.PhysicalRoot.GetAllDescendants(txTypeFilter);
            }
            catch (Exception ex)
            {
                AJTTxMessageHandling.WriteError("Couldn't get robot from the study. Internal exception accours: " + ex.Message);
                AJTTxMessageHandling.WriteException(ex);
                return new TxObjectList();
            }
            TxObjectList result;
            if (AJTTxObjectListUtilities.IsNullOrEmpty(txObjectList2))
            {
                result = new TxObjectList();
            }
            else
            {
                foreach (ITxObject txObject in txObjectList2)
                {
                    TxRobot txRobot = (TxRobot)txObject;
                    try
                    {
                        if (!AJTTxDocumentUtilities.IsRobot(txRobot))
                        {
                            continue;
                        }
                    }
                    catch (Exception ex2)
                    {
                        AJTTxMessageHandling.WriteError("Couldn't get robot driving joint count. Internal exception accours: " + ex2.Message);
                        AJTTxMessageHandling.WriteException(ex2);
                        continue;
                    }
                    txObjectList.Add(txRobot);
                }
                txObjectList2 = null;
                result = txObjectList;
            }
            return result;
        }

        public static TxObjectList GetSelectedRobots(bool includeEquipment)
        {
            TxObjectList txObjectList = new TxObjectList();
            TxObjectList txObjectList2 = null;
            try
            {
                txObjectList2 = TxApplication.ActiveSelection.GetAllItems();
            }
            catch (Exception ex)
            {
                AJTTxMessageHandling.WriteError("Couldn't get selected objects from the study. Internal exception occurred: " + ex.Message);
                AJTTxMessageHandling.WriteException(ex);
                return new TxObjectList();
            }
            TxObjectList result;
            if (AJTTxObjectListUtilities.IsNullOrEmpty(txObjectList2))
            {
                result = new TxObjectList();
            }
            else
            {
                foreach (ITxObject txObject in txObjectList2)
                {
                    if (!AJTTxDocumentUtilities.IsRobot(txObject))
                    {
                        if (includeEquipment && txObject is ITxComponent)
                        {
                            ITxObject robotFromEquipmentInstance = AJTTxComponentUtilities.GetRobotFromEquipmentInstance(txObject as ITxComponent);
                            if (AJTTxDocumentUtilities.IsRobot(robotFromEquipmentInstance))
                            {
                                txObjectList.Add(robotFromEquipmentInstance);
                            }
                        }
                    }
                    else
                    {
                        txObjectList.Add(txObject);
                    }
                }
                result = txObjectList;
            }
            return result;
        }

        public static bool IsRobot(ITxObject obj)
        {
            bool result = false;
            try
            {
                if (AJTTxDocumentUtilities.DoesObjectExist(obj) && obj is TxRobot)
                {
                    TxRobot txRobot = obj as TxRobot;
                    result = (txRobot.DrivingJoints.Count == 6 || txRobot.DrivingJoints.Count == 4 || txRobot.DrivingJoints.Count == 7);
                }
            }
            catch (Exception exception)
            {
                AJTTxMessageHandling.WriteException(exception);
                result = false;
            }
            return result;
        }

        public static TxFrame GetFrameByName(string name)
        {
            TxFrame result;
            if (name != null)
            {
                TxObjectList objectsByName = TxApplication.ActiveDocument.GetObjectsByName(name);
                if (objectsByName.Count != 0)
                {
                    foreach (ITxObject txObject in objectsByName)
                    {
                        TxFrame txFrame = txObject as TxFrame;
                        if (txFrame != null)
                        {
                            return txFrame;
                        }
                    }
                    result = null;
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

        public static bool DoesObjectExist(ITxObject obj)
        {
            bool result;
            try
            {
                if (obj != null && (!(obj is ITxPlanningObject) || ((ITxPlanningObject)obj).IsObjectLoaded()) && TxApplication.ActiveDocument.DoesObjectExist(obj.Id))
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public static void DeleteObject(ITxObject obj)
        {
            if (AJTTxDocumentUtilities.DoesObjectExist(obj))
            {
                try
                {
                    obj.Delete();
                }
                catch (Exception exception)
                {
                    AJTTxMessageHandling.WriteException(exception);
                }
            }
        }

        public static void DeleteObjects(TxObjectList objs)
        {
            try
            {
                foreach (ITxObject obj in objs)
                {
                    AJTTxDocumentUtilities.DeleteObject(obj);
                }
            }
            catch (Exception exception)
            {
                AJTTxMessageHandling.WriteException(exception);
            }
        }

        public static bool IsObjAttached(ITxLocatableObject obj, ITxObjectCollection parent)
        {
            TxObjectList allDescendants = parent.GetAllDescendants(new TxTypeFilter(typeof(ITxLocatableObject)));
            allDescendants.Add(parent);
            for (ITxLocatableObject nextAttachmentParent = AJTTxDocumentUtilities.GetNextAttachmentParent(obj); nextAttachmentParent != null; nextAttachmentParent = AJTTxDocumentUtilities.GetNextAttachmentParent(nextAttachmentParent))
            {
                if (allDescendants.Contains(nextAttachmentParent))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsPathUnderSystemRoot(string path)
        {
            return path.ToLower().StartsWith(TxApplication.SystemRootDirectory.ToLower());
        }

        public static string GetRelativeFilePath(string path)
        {
            string result;
            if (!AJTTxDocumentUtilities.IsPathUnderSystemRoot(path))
            {
                result = path;
            }
            else
            {
                result = path.Substring(TxApplication.SystemRootDirectory.Length).TrimStart(new char[]
                {
                    '\\',
                    '/'
                });
            }
            return result;
        }

        private static ITxLocatableObject GetNextAttachmentParent(ITxLocatableObject obj)
        {
            ITxLocatableObject txLocatableObject = obj;
            ITxLocatableObject attachmentParent = txLocatableObject.AttachmentParent;
            for (; ; )
            {
                if (attachmentParent != null)
                {
                    if (attachmentParent != TxApplication.ActiveDocument.PhysicalRoot)
                    {
                        break;
                    }
                }
                txLocatableObject = (txLocatableObject.Collection as ITxLocatableObject);
                if (txLocatableObject == null || txLocatableObject == TxApplication.ActiveDocument.PhysicalRoot)
                {
                    break;
                }
                attachmentParent = txLocatableObject.AttachmentParent;
            }
            return attachmentParent;
        }

        internal static ITxObject Paste(ITxObject obj)
        {
            ITxObject result;
            if (!AJTTxDocumentUtilities.DoesObjectExist(obj))
            {
                result = null;
            }
            else
            {
                try
                {
                    TxObjectList txObjectList = new TxObjectList();
                    txObjectList.Add(obj);
                    TxObjectList txObjectList2 = txObjectList;
                    if (TxApplication.ActiveDocument.PhysicalRoot.CanPasteList(txObjectList2))
                    {
                        TxObjectList txObjectList3 = TxApplication.ActiveDocument.PhysicalRoot.Paste(txObjectList2);
                        return txObjectList3[0];
                    }
                    AJTTxMessageHandling.WriteError("Cannot paste '" + obj.Name + "'.");
                }
                catch (Exception exception)
                {
                    AJTTxMessageHandling.WriteError("Cannot paste '" + obj.Name + "'.");
                    AJTTxMessageHandling.WriteException(exception);
                }
                result = null;
            }
            return result;
        }

        public static void DisplayObject(ITxObject obj)
        {
            if (AJTTxDocumentUtilities.DoesObjectExist(obj))
            {
                try
                {
                    TxObjectList txObjectList = new TxObjectList();
                    txObjectList.Add(obj);
                    AJTTxDocumentUtilities.DisplayObjects(txObjectList);
                }
                catch
                {
                }
            }
        }

        public static void DisplayObjects(TxObjectList objects)
        {
            if (!AJTTxObjectListUtilities.IsNullOrEmpty(objects))
            {
                try
                {
                    TxGraphicsUtilitiesEx.DisplayObjects(objects, true);
                }
                catch
                {
                }
            }
        }

        public static void BlankObject(ITxObject obj)
        {
            if (AJTTxDocumentUtilities.DoesObjectExist(obj))
            {
                try
                {
                    TxObjectList txObjectList = new TxObjectList();
                    txObjectList.Add(obj);
                    AJTTxDocumentUtilities.BlankObjects(txObjectList);
                }
                catch
                {
                }
            }
        }

        public static void BlankObjects(TxObjectList objects)
        {
            if (!AJTTxObjectListUtilities.IsNullOrEmpty(objects))
            {
                try
                {
                    TxGraphicsUtilitiesEx.BlankObjects(objects, true);
                }
                catch
                {
                }
            }
        }

        public static ITxDisplayableObject GetDisplayableObject(ITxObject obj)
        {
            ITxProcessModelObject objectEngineeringRepresentation = AJTTxDocumentUtilities.GetObjectEngineeringRepresentation(obj);
            ITxDisplayableObject result;
            if (!(objectEngineeringRepresentation is ITxDisplayableObject))
            {
                result = null;
            }
            else
            {
                result = (ITxDisplayableObject)objectEngineeringRepresentation;
            }
            return result;
        }

        public static ITxProcessModelObject GetObjectEngineeringRepresentation(ITxObject obj)
        {
            ITxProcessModelObject txProcessModelObject = obj as ITxProcessModelObject;
            ITxPlanningObject txPlanningObject = obj as ITxPlanningObject;
            if (txProcessModelObject == null && txPlanningObject != null)
            {
                txProcessModelObject = (txPlanningObject.EngineeringRepresentation as ITxProcessModelObject);
            }
            return txProcessModelObject;
        }

        public static void ZoomToObjects(TxObjectList objects, bool display = true)
        {
            if (!AJTTxObjectListUtilities.IsNullOrEmpty(objects))
            {
                try
                {
                    if (display)
                    {
                        AJTTxDocumentUtilities.DisplayObjects(objects);
                    }
                    TxApplication.ActiveSelection.SetItems(objects);
                    AJTTxDocumentUtilities.ZoomToSelection();
                }
                catch (Exception exception)
                {
                    AJTTxMessageHandling.WriteException(exception);
                }
            }
        }

        public static string GetParentName(ITxObjectCollection obj)
        {
            string result;
            if (obj == null)
            {
                result = string.Empty;
            }
            else
            {
                try
                {
                    if (obj.Collection == null)
                    {
                        result = string.Empty;
                    }
                    else
                    {
                        result = obj.Collection.Name;
                    }
                }
                catch (Exception exception)
                {
                    AJTTxMessageHandling.WriteException(exception);
                    result = string.Empty;
                }
            }
            return result;
        }

        public static void ZoomToObject(ITxObject obj, bool display = true)
        {
            if (obj != null)
            {
                try
                {
                    if (display)
                    {
                        ITxDisplayableObject txDisplayableObject = obj as ITxDisplayableObject;
                        if (txDisplayableObject != null)
                        {
                            txDisplayableObject.Display();
                        }
                    }
                    if (TxApplication.ActiveSelection.Count != 1 && TxApplication.ActiveSelection.GetItems()[0] != obj)
                    {
                        TxObjectList txObjectList = new TxObjectList();
                        txObjectList.Add(obj);
                        TxApplication.ActiveSelection.SetItems(txObjectList);
                    }
                    AJTTxDocumentUtilities.ZoomToSelection();
                }
                catch (Exception exception)
                {
                    AJTTxMessageHandling.WriteException(exception);
                }
            }
        }

        public static void ZoomToSelection()
        {
            try
            {
                TxCommandsManager commandsManager = TxApplication.CommandsManager;
                commandsManager.ExecuteCommand("GraphicViewer.ZoomToSelection");
            }
            catch
            {
            }
        }

        public static void SelectObject(ITxObject obj, bool only = true)
        {
            if (obj != null && TxApplication.ActiveDocument.DoesObjectExist(obj.Id))
            {
                TxObjectList txObjectList = new TxObjectList(1);
                txObjectList.Add(obj);
                AJTTxDocumentUtilities.SelectObjects(txObjectList, only);
            }
        }

        public static void SelectObjects(TxObjectList objects, bool only = true)
        {
            if (!AJTTxObjectListUtilities.IsNullOrEmpty(objects))
            {
                try
                {
                    if (only)
                    {
                        TxApplication.ActiveSelection.SetItems(objects);
                    }
                    else
                    {
                        TxApplication.ActiveSelection.AddItems(objects);
                    }
                }
                catch (Exception exception)
                {
                    AJTTxMessageHandling.WriteException(exception);
                }
            }
        }

        public static void DeselectObject(ITxObject obj)
        {
            if (obj != null && TxApplication.ActiveDocument.DoesObjectExist(obj.Id))
            {
                TxObjectList txObjectList = new TxObjectList(1);
                txObjectList.Add(obj);
                AJTTxDocumentUtilities.DeselectObjects(txObjectList);
            }
        }

        public static void DeselectObjects(TxObjectList objects)
        {
            if (!AJTTxObjectListUtilities.IsNullOrEmpty(objects))
            {
                try
                {
                    TxApplication.ActiveSelection.RemoveItems(objects);
                }
                catch (Exception exception)
                {
                    AJTTxMessageHandling.WriteException(exception);
                }
            }
        }

        public static void BlankAll()
        {
            AJTTxDocumentUtilities.BlankAll(true);
        }

        public static void BlankAll(bool useTransaction)
        {
            try
            {
                if (useTransaction)
                {
                    TxApplication.ActiveUndoManager.StartTransaction();
                }
                TxObjectList txObjectList = new TxObjectList();
                txObjectList.Add(TxApplication.ActiveDocument.PhysicalRoot);
                TxCommandManager txCommandManager = new TxCommandManager();
                txCommandManager.ExecuteCommandByCommandProgId("DisplayCommands.CUiDspBlank.1", txObjectList, false);
                if (useTransaction)
                {
                    TxApplication.ActiveUndoManager.EndTransaction();
                }
            }
            catch
            {
            }
        }

        public static void DisplayOnly(TxObjectList objects)
        {
            if (!AJTTxObjectListUtilities.IsNullOrEmpty(objects))
            {
                try
                {
                    TxApplication.ActiveUndoManager.StartTransaction();
                    TxCommandManager txCommandManager = new TxCommandManager();
                    txCommandManager.ExecuteCommandByCommandProgId("DisplayCommands.CUiDspDisplayOnly.1", objects, false);
                    TxApplication.ActiveUndoManager.EndTransaction();
                }
                catch
                {
                }
            }
        }

        public static TxObjectList GetAllComponents()
        {
            TxObjectList result = new TxObjectList();
            try
            {
                result = TxApplication.ActiveDocument.PhysicalRoot.GetAllDescendants(new TxTypeFilter(new Type[]
                {
                    typeof(ITxComponent)
                }));
            }
            catch
            {
            }
            return result;
        }

        public static TxObjectList GetAllDevices()
        {
            TxObjectList result = new TxObjectList();
            try
            {
                result = TxApplication.ActiveDocument.PhysicalRoot.GetAllDescendants(new TxTypeFilter(new Type[]
                {
                    typeof(ITxDevice)
                }));
            }
            catch
            {
            }
            return result;
        }

        public static TxObjectList GetAllVisibleComponents()
        {
            TxObjectList txObjectList = new TxObjectList();
            TxObjectList txObjectList2 = new TxObjectList();
            try
            {
                txObjectList2 = TxApplication.ActiveDocument.PhysicalRoot.GetAllDescendants(new TxTypeFilter(new Type[]
                {
                    typeof(ITxComponent)
                }));
            }
            catch
            {
            }
            if (txObjectList2 != null)
            {
                foreach (ITxObject txObject in txObjectList2)
                {
                    if (txObject is ITxDisplayableObject && (((ITxDisplayableObject)txObject).Visibility == null || ((ITxDisplayableObject)txObject).Visibility == TxDisplayableObjectVisibility.Partial))
                    {
                        txObjectList.Add(txObject);
                    }
                }
            }
            return txObjectList;
        }

        public static string GetExternalIdFromObject(ITxObject obj)
        {
            string empty;
            if (obj != null)
            {
                try
                {
                    if (obj is ITxProcessModelObject && ((ITxProcessModelObject)obj).ProcessModelId != null)
                    {
                        return ((ITxProcessModelObject)obj).ProcessModelId.ExternalId;
                    }
                }
                catch (Exception exception)
                {
                    AJTTxMessageHandling.WriteError("Can't get external id from process model object '" + obj.Name + "'.");
                    AJTTxMessageHandling.WriteException(exception);
                }
                empty = string.Empty;
            }
            else
            {
                empty = string.Empty;
            }
            return empty;
        }

        public static ITxPlanningObject GetCurrentStudy()
        {
            ITxPlanningObject result;
            if (!AJTTxDocumentUtilities.IsDocumentLoaded())
            {
                result = null;
            }
            else
            {
                result = TxApplication.ActiveDocument.CurrentStudy;
            }
            return result;
        }

        public static object GetNormalizesStudyName()
        {
            string text = "Study";
            ITxPlanningObject currentStudy = AJTTxDocumentUtilities.GetCurrentStudy();
            object result;
            if (currentStudy == null)
            {
                result = text;
            }
            else
            {
                text = currentStudy.Name;
                text = AJTPath.GetAllowedStringForFilename(currentStudy.Name);
                result = text;
            }
            return result;
        }

        public static bool IsDocumentLoaded()
        {
            bool flag = false;
            bool result;
            try
            {
                if (TxApplication.ActiveDocument == null)
                {
                    result = flag;
                }
                else
                {
                    if (!AJTTxDocumentUtilities.IsAnyTcPlatform)
                    {
                        if (TxApplication.ActiveDocument.CurrentStudy != null)
                        {
                            flag = true;
                        }
                    }
                    else
                    {
                        flag = TxApplication.ActiveDocument.PlatformGlobalServicesProvider.IsDataLoaded();
                    }
                    result = flag;
                }
            }
            catch (Exception exception)
            {
                AJTTxMessageHandling.WriteException(exception);
                result = false;
            }
            return result;
        }

        public static bool IsAnyTcPlatform
        {
            get
            {
                return TxApplication.PlatformType == null || TxApplication.PlatformType == TxPlatformType.TeamcenterOffline;
            }
        }

        public static bool IsAnyEmServerPlatform
        {
            get
            {
                return TxApplication.PlatformType == TxPlatformType.EmServer || TxApplication.PlatformType == TxPlatformType.Offline;
            }
        }

        public static bool IsAnyRobotExpertPlatform
        {
            get
            {
                return TxApplication.PlatformType == TxPlatformType.RobotExpert;
            }
        }

        public static string GetStudyExternalId()
        {
            string result;
            if (AJTTxDocumentUtilities.IsDocumentLoaded())
            {
                result = TxApplication.ActiveDocument.CurrentStudy.ProcessModelId.ExternalId;
            }
            else
            {
                result = null;
            }
            return result;
        }

        public static TxSnapshot GetSnapshot(string name)
        {
            TxTypeFilter txTypeFilter = new TxTypeFilter(typeof(TxSnapshot));
            TxObjectList directDescendants = TxApplication.ActiveDocument.PhysicalRoot.GetDirectDescendants(txTypeFilter);
            TxSnapshot result = null;
            if (directDescendants != null)
            {
                result = (directDescendants.FirstOrDefault((ITxObject p) => p.Name.Equals(name)) as TxSnapshot);
            }
            return result;
        }

        public static string GetRelativeIdString(ITxObject obj, ITxObject referenceObj)
        {
            return TxEngineeringDataInternal.GetRelativeIdString(obj, referenceObj);
        }

        public static string GetIdStringFromObject(ITxObject obj)
        {
            return TxEngineeringDataInternal.GetIdStringFromObject(obj);
        }

        public static string GetGlobalUniqueId(ITxObject obj)
        {
            return TxEngineeringDataInternal.GetGlobalUniqueIdFromObj(obj);
        }

        public static ITxObject GetObjectFromIdString(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    return TxEngineeringDataInternal.GetObjectFromIdString(id);
                }
                catch (Exception exception)
                {
                    AJTTxMessageHandling.WriteException(exception);
                    return null;
                }
            }
            return null;
        }

        public static ITxObject GetObjectFromRelativeIdString(ITxObject refObj, string id)
        {
            ITxObject result;
            if (string.IsNullOrEmpty(id))
            {
                result = null;
            }
            else
            {
                try
                {
                    result = TxEngineeringDataInternal.GetObjFromRelativeIdString(refObj, id);
                }
                catch (Exception exception)
                {
                    AJTTxMessageHandling.WriteException(exception);
                    result = null;
                }
            }
            return result;
        }

        public static string GetEngDataFolder()
        {
            string result;
            if (AJTTxDocumentUtilities.IsDocumentLoaded())
            {
                result = new TxDocumentEx().NodeGetAttachmentFullPath(TxApplication.ActiveDocument.CurrentStudy.ProcessModelId, TxFileRoleEnum.EngineeringDataDir);
            }
            else
            {
                result = null;
            }
            return result;
        }

        public static string GetTuneCellFolder()
        {
            string engDataFolder = AJTTxDocumentUtilities.GetEngDataFolder();
            string result;
            if (engDataFolder != null)
            {
                result = Path.GetDirectoryName(engDataFolder);
            }
            else
            {
                result = null;
            }
            return result;
        }

        public static string GetTuneCellsPath()
        {
            return TxEngineeringDataInternal.GetTuneCellsPath();
        }

        public static bool StudyLoadedAsFileFormat()
        {
            return TxEngineeringDataInternal.GlobalGetStudyFormat() == TxStudyFormatEnum.File;
        }

        public static ITxPlanningObject GetEmServerPrototypeByFullPath(string fullPath)
        {
            ITxPlanningObject result;
            if (TxApplication.PlatformType != TxPlatformType.EmServer)
            {
                result = null;
            }
            else
            {
                try
                {
                    string prototypeExternalId = AJTTxTuneDataUtilitites.GetPrototypeExternalId(fullPath);
                    if (string.IsNullOrEmpty(prototypeExternalId))
                    {
                        return null;
                    }
                    if (TxApplication.ActiveDocument.PlatformGlobalServicesProvider is ITxEmsGlobalServicesProvider)
                    {
                        return ((ITxEmsGlobalServicesProvider)TxApplication.ActiveDocument.PlatformGlobalServicesProvider).GetObjectByProcessModelId(new TxProcessModelId(prototypeExternalId));
                    }
                }
                catch
                {
                }
                result = null;
            }
            return result;
        }

        public static ITxPlanningObject GetEmServerObjectByExternalId(string externalId)
        {
            try
            {
                if (TxApplication.ActiveDocument.PlatformGlobalServicesProvider is ITxEmsGlobalServicesProvider)
                {
                    return ((ITxEmsGlobalServicesProvider)TxApplication.ActiveDocument.PlatformGlobalServicesProvider).GetObjectByProcessModelId(new TxProcessModelId(externalId));
                }
            }
            catch (Exception exception)
            {
                AJTTxMessageHandling.WriteException(exception);
            }
            return null;
        }

        public static ITxObject GetObjectByExternalId(string externalId)
        {
            ITxObject result;
            if (!AJTTxDocumentUtilities.IsDocumentLoaded())
            {
                result = null;
            }
            else
            {
                try
                {
                    return TxApplication.ActiveDocument.GetObjectByProcessModelId(new TxProcessModelId(externalId));
                }
                catch (Exception exception)
                {
                    AJTTxMessageHandling.WriteException(exception);
                }
                result = null;
            }
            return result;
        }

        //public static ITxComponent InsertComponent(string name, ITxPlanningObject prototype, string relativeFileName)
        //{
        //    return AJTTxDocumentUtilities.InsertComponent(name, prototype, relativeFileName, false);
        //}

        //public static ITxComponent InsertComponent(string name, ITxPlanningObject prototype, string fileName, bool pathIsAbsolute)
        //{
        //    if (string.IsNullOrEmpty(name))
        //    {
        //        name = TxApplication.ActiveDocument.PhysicalRoot.GetUniqueDisplayName("Comp");
        //    }
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(fileName) && !pathIsAbsolute)
        //        {
        //            fileName = Path.Combine(TxApplication.SystemRootDirectory.TrimEnd(new char[]
        //            {
        //                '\\'
        //            }), fileName);
        //        }
        //        TxInsertComponentCreationData txInsertComponentCreationData = new TxInsertComponentCreationData(name, fileName);
        //        if (prototype != null)
        //        {
        //            txInsertComponentCreationData.Prototype = prototype;
        //            if (prototype.IsDerivedFromPlanningType("PmEquipmentPrototype"))
        //            {
        //                AJTTxMessageHandling.WriteDetailed("Get instances from PmEquipmentPrototype " + AJTTxPlanningObjectUtilities.GetCaption(prototype) + ".");
        //                TxObjectList<ITxPlanningInstance> planningInstances = AJTTxPlanningObjectUtilities.GetPlanningInstances(prototype);
        //                if (planningInstances.Count <= 0)
        //                {
        //                    AJTTxMessageHandling.WriteDetailed("PmEquipmentPrototype has no instance, is not loaded.");
        //                }
        //                else
        //                {
        //                    AJTTxMessageHandling.WriteDetailed("Copy instance from PmEquipmentPrototype " + AJTTxPlanningObjectUtilities.GetCaption(planningInstances[0]));
        //                    ITxPlanningObject txPlanningObject = AJTTxPlanningObjectUtilities.Paste(planningInstances[0]);
        //                    if (txPlanningObject != null && txPlanningObject.EngineeringRepresentation is ITxComponent)
        //                    {
        //                        AJTTxDocumentUtilities.DisplayObject(txPlanningObject.EngineeringRepresentation);
        //                        return (ITxComponent)txPlanningObject.EngineeringRepresentation;
        //                    }
        //                    AJTTxMessageHandling.WriteDetailed("Could not copy instance from PmEquipmentPrototype");
        //                }
        //            }
        //        }
        //        return TxApplication.ActiveDocument.PhysicalRoot.InsertComponent(txInsertComponentCreationData);
        //    }
        //    catch (Exception exception)
        //    {
        //        AJTTxMessageHandling.WriteError(string.Format("Cannot insert component. File:{0}, PathIsAbsolute: {1}, Name: {2}", fileName, pathIsAbsolute, name));
        //        AJTTxMessageHandling.WriteException(exception);
        //    }
        //    return null;
        //}

        public static ITxObject GetObjectByNameAndType(string name, Type type)
        {
            ITxObject result;
            if (!AJTTxDocumentUtilities.IsDocumentLoaded())
            {
                result = null;
            }
            else
            {
                try
                {
                    TxObjectList objectsByName = TxApplication.ActiveDocument.GetObjectsByName(name);
                    if (objectsByName == null)
                    {
                        return null;
                    }
                    foreach (ITxObject txObject in objectsByName)
                    {
                        if (txObject.GetType().Equals(type))
                        {
                            return txObject;
                        }
                    }
                }
                catch (Exception exception)
                {
                    AJTTxMessageHandling.WriteException(exception);
                }
                result = null;
            }
            return result;
        }

        public static bool IsPerliminaryCollisionDetected()
        {
            return TxWeldAbility.IsPerliminaryCollisionDetected();
        }

        public static ITxObject GetDirectDescendantByNameAndType(ITxObjectCollection collection, string name, Type type, bool ignoreCase = false)
        {
            ITxObject result;
            if (AJTTxDocumentUtilities.IsDocumentLoaded() && !string.IsNullOrEmpty(name) && !(type == null) && collection != null)
            {
                TxObjectList directDescendants = collection.GetDirectDescendants(new TxNoTypeFilter());
                foreach (ITxObject txObject in directDescendants)
                {
                    if (!ignoreCase)
                    {
                        if (txObject.Name.Equals(name))
                        {
                            if (txObject.GetType().Equals(type))
                            {
                                return txObject;
                            }
                        }
                    }
                    else if (txObject.Name.ToLower().Equals(name.ToLower()) && txObject.GetType().Equals(type))
                    {
                        return txObject;
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

        public static ITxObject GetAllDescendantByNameAndType(ITxObjectCollection collection, string name, Type type, bool ignoreCase = false)
        {
            ITxObject result;
            if (AJTTxDocumentUtilities.IsDocumentLoaded() && !string.IsNullOrEmpty(name) && !(type == null) && collection != null)
            {
                TxObjectList allDescendants = collection.GetAllDescendants(new TxTypeFilter(type));
                foreach (ITxObject txObject in allDescendants)
                {
                    if (!ignoreCase)
                    {
                        if (txObject.Name.Equals(name) && txObject.GetType().Equals(type))
                        {
                            return txObject;
                        }
                    }
                    else if (txObject.Name.ToLower().Equals(name.ToLower()) && txObject.GetType().Equals(type))
                    {
                        return txObject;
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

        public static void StopEmphasizeCollisionPair()
        {
            try
            {
                TxApplication.CommandsManager.ExecuteCommand("CollisionCommands.EmphasizeCollisionPair");
            }
            catch (Exception exception)
            {
                AJTTxMessageHandling.WriteException(exception);
            }
        }

        public static TxObjectList GetObjectsFromCollection(ITxObjectCollection collection, ITxValidator validator, TxTypeFilter typeFilter, TxObjectList existingObjects = null)
        {
            TxObjectList txObjectList = new TxObjectList();
            if (existingObjects != null)
            {
                txObjectList.AddRange(existingObjects);
            }
            TxObjectList result;
            if (collection == null)
            {
                result = txObjectList;
            }
            else
            {
                TxObjectList allDescendants = collection.GetAllDescendants(typeFilter);
                foreach (ITxObject txObject in allDescendants)
                {
                    string text;
                    if (!txObjectList.Contains(txObject) && validator.IsValidObject(txObject, out text))
                    {
                        txObjectList.Add(txObject);
                    }
                }
                result = txObjectList;
            }
            return result;
        }

        public static string GetFullPath(string path)
        {
            string result;
            if (path == null)
            {
                result = null;
            }
            else
            {
                string text = path;
                if (text.StartsWith("#"))
                {
                    text = text.Substring(1);
                    if (text.StartsWith("//") || text.StartsWith("\\"))
                    {
                        text = text.Substring(1);
                    }
                    text = Path.Combine(TxApplication.SystemRootDirectory, text);
                }
                result = text;
            }
            return result;
        }

        public static void SetObjectCompressedStringParam(ITxObject obj, string paramName, string paramValue)
        {
            if (obj != null && !string.IsNullOrEmpty(paramName))
            {
                AJTTxDocumentUtilities.DeleteObjectCompressedStringParam(obj, paramName);
                paramValue = AJTStringCompression.Compress(paramValue);
                string text = paramName + "_l";
                obj.SetAttribute(new TxIntAttribute(text, paramValue.Length));
                int num = 0;
                while (paramValue.Length > 0)
                {
                    if (paramValue.Length > 9000000)
                    {
                        obj.SetAttribute(new TxStringAttribute(paramName + "_" + num.ToString(), paramValue.Substring(0, 9000000)));
                        num++;
                        paramValue = paramValue.Substring(9000000);
                    }
                    else
                    {
                        obj.SetAttribute(new TxStringAttribute(paramName + "_" + num.ToString(), paramValue));
                        paramValue = string.Empty;
                    }
                }
            }
        }

        public static void DeleteObjectCompressedStringParam(ITxObject obj, string paramName)
        {
            if (AJTTxDocumentUtilities.DoesObjectExist(obj) && !string.IsNullOrEmpty(paramName))
            {
                try
                {
                    string text = paramName + "_l";
                    TxIntAttribute txIntAttribute = obj.GetAttribute(text) as TxIntAttribute;
                    if (txIntAttribute != null)
                    {
                        obj.RemoveAttribute(text);
                        int num = (int)Math.Ceiling((double)txIntAttribute.Value / 9000000.0);
                        for (int i = 0; i < num; i++)
                        {
                            obj.RemoveAttribute(paramName + "_" + i.ToString());
                        }
                    }
                }
                catch (Exception exception)
                {
                    AJTTxMessageHandling.WriteException(exception);
                }
            }
        }

        public static bool HasObjectCompressedStringParam(ITxObject obj, string paramName)
        {
            bool result = false;
            if (AJTTxDocumentUtilities.DoesObjectExist(obj))
            {
                string text = paramName + "_l";
                TxIntAttribute txIntAttribute = obj.GetAttribute(text) as TxIntAttribute;
                if (txIntAttribute != null)
                {
                    result = true;
                }
            }
            return result;
        }

        public static string GetObjectCompressedStringParam(ITxObject obj, string paramName)
        {
            string result;
            if (!AJTTxDocumentUtilities.DoesObjectExist(obj))
            {
                result = null;
            }
            else
            {
                try
                {
                    string text = paramName + "_l";
                    TxIntAttribute txIntAttribute = obj.GetAttribute(text) as TxIntAttribute;
                    if (txIntAttribute != null && txIntAttribute.Value > 0)
                    {
                        int i = 0;
                        int num = 0;
                        string text2 = string.Empty;
                        while (i < txIntAttribute.Value)
                        {
                            TxStringAttribute txStringAttribute = obj.GetAttribute(paramName + "_" + num.ToString()) as TxStringAttribute;
                            if (txStringAttribute == null)
                            {
                                break;
                            }
                            text2 += txStringAttribute.Value;
                            i = text2.Length;
                            num++;
                        }
                        text2 = AJTStringCompression.Uncompress(text2);
                        result = text2;
                    }
                    else
                    {
                        result = null;
                    }
                }
                catch (Exception exception)
                {
                    AJTTxMessageHandling.WriteException(exception);
                    result = null;
                }
            }
            return result;
        }

        internal const int MAX_STRING_ATTRIBUTE_LENGTH = 9000000;

        private const string PM_OBJ_ID_SUFFIX = "PmObjId#";
    }
}
