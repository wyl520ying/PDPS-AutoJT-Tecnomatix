using EngineeringInternalExtension;
using EngineeringInternalExtension.CommandParameters;
using EngineeringInternalExtension.DataTypes;
using System;
using System.IO;
using Tecnomatix.Engineering;

namespace AutoJTTXCoreUtilities
{
    public class AJTTxComponentUtilities
    {
        public static TxComponent CreateLocalComponent(string name, out string error, bool setAsDisplay = true)
        {
            error = string.Empty;
            TxComponent result = null;
            TxLocalComponentCreationDataEx txLocalComponentCreationDataEx = new TxLocalComponentCreationDataEx();
            if (setAsDisplay)
            {
                txLocalComponentCreationDataEx.SetAsDisplay();
            }
            txLocalComponentCreationDataEx.Name = name;
            TxPhysicalRootEx txPhysicalRootEx = new TxPhysicalRootEx();
            try
            {
                result = txPhysicalRootEx.CreateLocalComponent(txLocalComponentCreationDataEx);
            }
            catch (Exception exception)
            {
                AJTTxMessageHandling.WriteException(exception);
                error = AJTTxMessageHandling.GenerateExceptionLine(exception, true);
                return null;
            }
            return result;
        }

        public static TxComponent CreateNewResource(string type, string typeNiceName)
        {
            try
            {
                TxCommandManager txCommandManager = new TxCommandManager();
                string externalFromInternalCommandId = txCommandManager.GetExternalFromInternalCommandId("ComponentOperations.NewResource.1");
                TxNewPartResourceParametersEx txNewPartResourceParametersEx = new TxNewPartResourceParametersEx();
                txNewPartResourceParametersEx.Type = type;
                txNewPartResourceParametersEx.TypeNiceName = typeNiceName;
                TxApplication.CommandsManager.ExecuteCommand(externalFromInternalCommandId, txNewPartResourceParametersEx);
                return txNewPartResourceParametersEx.CreatedObject as TxComponent;
            }
            catch (Exception exception)
            {
                AJTTxMessageHandling.WriteException(exception);
            }
            return null;
        }

        public static TxComponent CreateNewPart(string type, string typeNiceName)
        {
            try
            {
                TxCommandManager txCommandManager = new TxCommandManager();
                string externalFromInternalCommandId = txCommandManager.GetExternalFromInternalCommandId("ComponentOperations.NewPart.1");
                TxNewPartResourceParametersEx txNewPartResourceParametersEx = new TxNewPartResourceParametersEx();
                txNewPartResourceParametersEx.Type = type;
                txNewPartResourceParametersEx.TypeNiceName = typeNiceName;
                TxApplication.CommandsManager.ExecuteCommand(externalFromInternalCommandId, txNewPartResourceParametersEx);
                return txNewPartResourceParametersEx.CreatedObject as TxComponent;
            }
            catch (Exception exception)
            {
                AJTTxMessageHandling.WriteException(exception);
            }
            return null;
        }

        public static void DeleteAllEntities(TxComponent component)
        {
            if (!(component == null))
            {
                try
                {
                    foreach (ITxObject txObject in component.GetDirectDescendants(new TxNoTypeFilter()))
                    {
                        if (txObject.CanBeDeleted)
                        {
                            txObject.Delete();
                        }
                    }
                }
                catch (Exception exception)
                {
                    AJTTxMessageHandling.WriteException(exception);
                }
            }
        }

        public static void SetObjectTransparentAndColor(ITxObject obj, TxColor color, double transparency)
        {
            try
            {
                if (obj is ITxDisplayableObject)
                {
                    ((ITxDisplayableObject)obj).Color = new TxColor(color.Red, color.Green, color.Blue, transparency);
                }
            }
            catch (Exception exception)
            {
                AJTTxMessageHandling.WriteException(exception);
            }
        }

        //public static ITxObject InstanceGetPrototype(ITxObject instance)
        //{
        //    ITxObject result;
        //    if (AJTTxDocumentUtilities.DoesObjectExist(instance) && instance.IsValid())
        //    {
        //        result = TxEngineeringDataInternal.InstanceGetPrototype(instance);
        //    }
        //    else
        //    {
        //        result = null;
        //    }
        //    return result;
        //}

        public static void SetObjectTransparentAndColor(ITxObject obj, TxColor color)
        {
            try
            {
                if (obj is ITxDisplayableObject)
                {
                    ((ITxDisplayableObject)obj).Color = color;
                }
            }
            catch (Exception exception)
            {
                AJTTxMessageHandling.WriteException(exception);
            }
        }

        public static TxSolid CreateCylinder(TxComponent component, string name, TxTransformation start, TxTransformation end, double radius, TxColor color, bool setAsDisplay = false)
        {
            TxSolid txSolid = null;
            try
            {
                TxTransformation inverse = start.Inverse;
                TxTransformation txTransformation = TxTransformation.Multiply(inverse, end);
                TxCylinderCreationData txCylinderCreationData = new TxCylinderCreationData(name, new TxVector(0.0, 0.0, 0.0), txTransformation.Translation, radius);
                if (setAsDisplay)
                {
                    txCylinderCreationData.SetAsDisplay();
                }
                txSolid = component.CreateSolidCylinder(txCylinderCreationData);
                txSolid.AbsoluteLocation *= start;
                AJTTxComponentUtilities.SetObjectTransparentAndColor(txSolid, color);
            }
            catch (Exception exception)
            {
                txSolid = null;
                AJTTxMessageHandling.WriteException(exception);
            }
            return txSolid;
        }

        public static bool IsEquipment(ITxObject obj)
        {
            ITxComponent txComponent = obj as ITxComponent;
            return txComponent != null && txComponent.IsEquipment;
        }

        public static TxSolid CreateCylinder(TxComponent component, string name, TxTransformation start, TxTransformation end, double radius, TxColor color, bool setAsDisplay, bool setAsPersistent)
        {
            TxSolid txSolid = null;
            try
            {
                TxTransformation inverse = start.Inverse;
                TxTransformation txTransformation = TxTransformation.Multiply(inverse, end);
                TxCylinderCreationData txCylinderCreationData = new TxCylinderCreationData(name, new TxVector(0.0, 0.0, 0.0), txTransformation.Translation, radius);
                if (setAsDisplay)
                {
                    txCylinderCreationData.SetAsDisplay();
                }
                if (setAsPersistent)
                {
                    txCylinderCreationData.SetAsPersistent();
                }
                txSolid = component.CreateSolidCylinder(txCylinderCreationData);
                txSolid.AbsoluteLocation *= start;
                AJTTxComponentUtilities.SetObjectTransparentAndColor(txSolid, color);
            }
            catch (Exception exception)
            {
                txSolid = null;
                AJTTxMessageHandling.WriteException(exception);
            }
            return txSolid;
        }

        public static TxObjectList GetEntityByName(ITxObjectCollection component, string name, ITxTypeFilter filter, bool directDescendants = false)
        {
            TxObjectList txObjectList = new TxObjectList();
            try
            {
                if (string.IsNullOrEmpty(name) || !AJTTxDocumentUtilities.DoesObjectExist(component))
                {
                    return txObjectList;
                }
                TxObjectList txObjectList2;
                if (!directDescendants)
                {
                    txObjectList2 = component.GetAllDescendants(filter);
                }
                else
                {
                    txObjectList2 = component.GetDirectDescendants(filter);
                }
                if (txObjectList2 == null)
                {
                    return txObjectList;
                }
                foreach (ITxObject txObject in txObjectList2)
                {
                    try
                    {
                        if (txObject.Name != null && txObject.Name.Equals(name))
                        {
                            txObjectList.Add(txObject);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception exception)
            {
                txObjectList = new TxObjectList();
                AJTTxMessageHandling.WriteException(exception);
            }
            return txObjectList;
        }

        public static string GetComponentFileName(ITxComponent component, bool includeSuffix = false)
        {
            string result;
            if (component == null)
            {
                result = string.Empty;
            }
            else
            {
                try
                {
                    string text = AJTTxComponentUtilities.GetComponentFilePath(component);
                    if (!string.IsNullOrEmpty(text) && !includeSuffix)
                    {
                        text = Path.GetFileNameWithoutExtension(text);
                    }
                    result = text;
                }
                catch (Exception exception)
                {
                    AJTTxMessageHandling.WriteException(exception);
                    result = string.Empty;
                }
            }
            return result;
        }

        public static string GetComponentFilePath(ITxComponent component)
        {
            if (component != null)
            {
                try
                {
                    return TxComponentEx.GetComponentPath(component);
                }
                catch (Exception exception)
                {
                    AJTTxMessageHandling.WriteException(exception);
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        public static string GetComponentRelativeFilePath(ITxComponent component)
        {
            return AJTTxComponentUtilities.GetComponentRelativeFilePath(AJTTxComponentUtilities.GetComponentFilePath(component));
        }

        public static string GetComponentRelativeFilePath(string path)
        {
            string result;
            if (!AJTTxDocumentUtilities.IsPathUnderSystemRoot(path))
            {
                result = path;
            }
            else
            {
                result = AJTTxDocumentUtilities.GetRelativeFilePath(path);
            }
            return result;
        }

        public static bool IsCoOrCojtComponentPath(string path)
        {
            return path.EndsWith(".cojt", StringComparison.OrdinalIgnoreCase) || path.EndsWith(".co", StringComparison.OrdinalIgnoreCase);
        }

        public static ITxRobot GetRobotFromEquipmentInstance(ITxComponent equipmentInstance)
        {
            TxObjectList equipmentsChildren = AJTTxComponentUtilities.GetEquipmentsChildren(equipmentInstance);
            if (equipmentsChildren != null)
            {
                foreach (ITxObject txObject in equipmentsChildren)
                {
                    if (txObject is ITxRobot)
                    {
                        return (ITxRobot)txObject;
                    }
                }
            }
            return null;
        }

        public static TxObjectList GetEquipmentsChildren(ITxComponent equipmentInstance)
        {
            TxObjectList result;
            if (AJTTxDocumentUtilities.DoesObjectExist(equipmentInstance) && equipmentInstance.IsEquipment)
            {
                TxObjectList txObjectList = new TxObjectList();
                ITxObjectCollection txObjectCollection = equipmentInstance as ITxObjectCollection;
                if (txObjectCollection != null)
                {
                    txObjectList = txObjectCollection.GetAllDescendants(new TxTypeFilter(typeof(ITxComponent)));
                }
                result = txObjectList;
            }
            else
            {
                result = null;
            }
            return result;
        }

        public static ITxComponent CopyComponent(ITxComponent component, ITxObjectCollection parent = null)
        {
            if (AJTTxDocumentUtilities.DoesObjectExist(component))
            {
                if (!AJTTxDocumentUtilities.DoesObjectExist(parent))
                {
                    parent = TxApplication.ActiveDocument.PhysicalRoot;
                }
                try
                {
                    if (TxObjectCollectionEx.CanPaste(parent, component))
                    {
                        TxObjectList txObjectList = new TxObjectList();
                        txObjectList.Add(component);
                        TxObjectList txObjectList2 = txObjectList;
                        return parent.Paste(txObjectList2)[0] as ITxComponent;
                    }
                    TxPhysicalRoot physicalRoot = TxApplication.ActiveDocument.PhysicalRoot;
                    TxObjectList txObjectList3 = new TxObjectList();
                    txObjectList3.Add(component);
                    return physicalRoot.Paste(txObjectList3)[0] as ITxComponent;
                }
                catch (Exception exception)
                {
                    AJTTxMessageHandling.WriteException(exception);
                    return null;
                }
            }
            return null;
        }

        public static bool IsSubComponent(ITxObject obj)
        {
            bool result;
            if (!(obj is ITxComponent))
            {
                result = false;
            }
            else
            {
                ITxComponent txComponent = ((ITxComponent)obj).Collection as ITxComponent;
                result = (txComponent != null);
            }
            return result;
        }

        public static ITxComponent GetParentEquipment(ITxObject obj)
        {
            ITxComponent result;
            if (!AJTTxDocumentUtilities.DoesObjectExist(obj))
            {
                result = null;
            }
            else if (obj is ITxComponent)
            {
                ITxObjectCollection collection;
                for (collection = obj.Collection; collection != TxApplication.ActiveDocument.PhysicalRoot; collection = collection.Collection)
                {
                    if (collection is ITxComponent && (collection as ITxComponent).IsEquipment)
                    {
                        return (ITxComponent)collection;
                    }
                }
                result = (collection as ITxComponent);
            }
            else
            {
                result = null;
            }
            return result;
        }

        public static ITxComponent GetRootEquipment(ITxObject obj)
        {
            ITxComponent result;
            if (AJTTxDocumentUtilities.DoesObjectExist(obj) && obj is ITxComponent)
            {
                if (!AJTTxComponentUtilities.IsSubComponent(obj))
                {
                    result = (ITxComponent)obj;
                }
                else
                {
                    ITxComponent txComponent = (ITxComponent)obj;
                    for (; ; )
                    {
                        if (!AJTTxComponentUtilities.IsSubComponent(txComponent))
                        {
                            if (txComponent != null)
                            {
                                break;
                            }
                        }
                        txComponent = AJTTxComponentUtilities.GetParentEquipment(txComponent);
                    }
                    result = txComponent;
                }
            }
            else
            {
                result = null;
            }
            return result;
        }

        public static ITxComponent GetAttachedComponent(ITxLocatableObject obj)
        {
            ITxComponent result;
            if (AJTTxDocumentUtilities.DoesObjectExist(obj) && obj.AttachmentParent != null)
            {
                ITxLocatableObject txLocatableObject = obj.AttachmentParent;
                while (!(txLocatableObject is ITxComponent))
                {
                    if (txLocatableObject == null)
                    {
                        return null;
                    }
                    txLocatableObject = (txLocatableObject.Collection as ITxLocatableObject);
                }
                result = (txLocatableObject as ITxComponent);
            }
            else
            {
                result = null;
            }
            return result;
        }

        public static ITxComponent GetEntityComponent(ITxLocatableObject entityObject)
        {
            ITxLocatableObject txLocatableObject = entityObject;
            ITxComponent result;
            if (txLocatableObject is ITxComponent)
            {
                result = (ITxComponent)txLocatableObject;
            }
            else
            {
                while (txLocatableObject != null && txLocatableObject != TxApplication.ActiveDocument.PhysicalRoot)
                {
                    txLocatableObject = (txLocatableObject.Collection as ITxLocatableObject);
                    if (txLocatableObject is ITxComponent)
                    {
                        return (ITxComponent)txLocatableObject;
                    }
                }
                result = null;
            }
            return result;
        }

        public static bool IsPartInstance(ITxObject collidingObject)
        {
            ITxObject entityComponent = AJTTxComponentUtilities.GetEntityComponent(collidingObject as ITxLocatableObject);
            if (entityComponent is ITxProcessModelObject)
            {
                ITxProcessModelObject txProcessModelObject = entityComponent as ITxProcessModelObject;
                if (txProcessModelObject.PlanningRepresentation.IsDerivedFromPlanningType("PmPartInstance"))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsComponent2Or3GeometryBlanked(ITxComponent component)
        {
            bool result;
            if (component is ITxObjectCollection)
            {
                TxTypeFilter txTypeFilter = new TxTypeFilter(typeof(ITx2Or3DimensionalGeometry));
                txTypeFilter.AddIncludedType(typeof(TxSurface));
                txTypeFilter.AddIncludedType(typeof(TxSolid));
                txTypeFilter.AddIncludedType(typeof(TxKinematicLink));
                txTypeFilter.AddIncludedType(typeof(TxGroup));
                TxObjectList allDescendants = ((ITxObjectCollection)component).GetAllDescendants(txTypeFilter);
                bool flag = true;
                foreach (ITxObject txObject in allDescendants)
                {
                    if (txObject is ITxDisplayableObject && ((txObject as ITxDisplayableObject).Visibility == null || (txObject as ITxDisplayableObject).Visibility == TxDisplayableObjectVisibility.Partial))
                    {
                        flag = false;
                        break;
                    }
                }
                result = flag;
            }
            else
            {
                result = false;
            }
            return result;
        }

        //public static TxGroup CreateGroup(ITxComponent component, string groupName)
        //{
        //    return ((ITxGeometryCreation)component).CreateGroup(new TxGroupCreationData(groupName));
        //}

        public static bool HasPMIInformation(ITxComponent component)
        {
            if (component is ITxStorable)
            {
                ITxStorable txStorable = component as ITxStorable;
                if (txStorable.StorageObject is TxLibraryStorage)
                {
                    return TxComponentEx.HasPMIInformation((TxLibraryStorage)txStorable.StorageObject);
                }
            }
            return false;
        }

        public static void LoadPMI(ITxComponent component)
        {
            if (AJTTxComponentUtilities.HasPMIInformation(component) && !component.IsOpenForModeling)
            {
                ITxStorable txStorable = component as ITxStorable;
                if (txStorable != null && txStorable.StorageObject is TxLibraryStorage && TxComponentEx.CanLoadPMI(component) && !TxComponentEx.IsPMILoaded(component))
                {
                    TxComponentEx.Reload((TxLibraryStorage)txStorable.StorageObject, true);
                }
            }
        }

        public static void UnloadPMI(ITxComponent component)
        {
            if (AJTTxComponentUtilities.HasPMIInformation(component) && !component.IsOpenForModeling)
            {
                ITxStorable txStorable = component as ITxStorable;
                if (txStorable != null && txStorable.StorageObject is TxLibraryStorage && TxComponentEx.IsPMILoaded(component))
                {
                    TxComponentEx.Reload((TxLibraryStorage)txStorable.StorageObject, false);
                }
            }
        }

        public static bool HasDetailedRepresentation(ITxComponent component)
        {
            if (component is ITxStorable)
            {
                ITxStorable txStorable = component as ITxStorable;
                if (txStorable.StorageObject is TxLibraryStorage)
                {
                    return ((TxLibraryStorage)txStorable.StorageObject).HasDetailedRepresentation;
                }
            }
            return false;
        }

        public static void LoadDetailed(ITxComponent component)
        {
            if (AJTTxComponentUtilities.HasDetailedRepresentation(component) && component.RepresentationType == TxRepresentationType.United && !(component is TxSweptVolume) && !(component is TxInterferenceVolume))
            {
                ITxStorable txStorable = component as ITxStorable;
                if (txStorable.StorageObject is TxLibraryStorage)
                {
                    ((TxLibraryStorage)txStorable.StorageObject).Reload(TxRepresentationLevel.Detailed);
                }
            }
        }

        public static void UnloadDetailed(ITxComponent component)
        {
            if (AJTTxComponentUtilities.HasDetailedRepresentation(component) && component.RepresentationType == TxRepresentationType.Detailed && !(component is TxSweptVolume) && !(component is TxInterferenceVolume))
            {
                ITxStorable txStorable = component as ITxStorable;
                if (txStorable.StorageObject is TxLibraryStorage)
                {
                    ((TxLibraryStorage)txStorable.StorageObject).Reload(TxRepresentationLevel.United);
                }
            }
        }

        public static string GetThreeDRepFullPath(string threeDRep)
        {
            return AJTTxDocumentUtilities.GetFullPath(threeDRep);
        }
    }
}
