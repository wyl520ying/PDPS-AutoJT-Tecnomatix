using EMPAPPLICATIONLib;
using EMPTYPELIBRARYLib;
using EMPTYPEMGMTLib;
using System;
using System.Resources;
using Tecnomatix.Engineering;
using Tecnomatix.Planning;

namespace AutoJTTXCoreUtilities.RobotMatrix
{
    public class Segment
    {
        public Segment(double start, double end, AJTApRmxUtils.EApRmxReachabilityStatus reachabilityStatuts)
        {
            this.startAngle = start;
            this.endAngle = end;
            this.statuts = reachabilityStatuts;
        }

        public double startAngle;

        public double endAngle;

        public AJTApRmxUtils.EApRmxReachabilityStatus statuts;
    }
    public class AJTApRmxUtils
    {
        internal AJTApRmxUtils()
        {
        }

        internal ResourceManager GetResourceManager()
        {
            return new ResourceManager("AutoJTTXCoreUtilities.RobotMatrix", base.GetType().Assembly);
        }

        internal static string GetAttributeUnitValue(object fieldObject, EmpEnumUnitsTypes types)
        {
            EmpAppOptionsClass empAppOptionsClass = new EmpAppOptionsClass();
            string result;
            if ((int)types != -1)
            {
                double unitConversionFactor = empAppOptionsClass.GetUnitConversionFactor(types);
                int unitPrecision = empAppOptionsClass.GetUnitPrecision(types);
                double num = Convert.ToDouble(fieldObject);
                num /= unitConversionFactor;
                result = Math.Round(num, unitPrecision).ToString();
            }
            else
            {
                result = fieldObject.ToString();
            }
            return result;
        }

        internal static EmpEnumUnitsTypes GetEnumUnitType(int classID, string attrName)
        {
            EmpApplicationClass empApplicationClass = new EmpApplicationClass();
            return empApplicationClass.get_ValueUnitType(attrName, classID);
        }

        internal static int GetClassIDByName(string objType)
        {
            EmpApplicationClass empApplicationClass = new EmpApplicationClass();
            EmpClassDefinition empClassDefinition = new EmpClassDefinitionClass();
            EmpContext context = empApplicationClass.Context;
            EmpTypeKey classByName = empClassDefinition.GetClassByName(ref context, objType);
            return classByName.typeId;
        }

        internal static TxPlanningObject GetPlanningObject(ITxObject obj)
        {
            TxPlanningObject txPlanningObject = obj as TxPlanningObject;
            if (txPlanningObject == null)
            {
                ITxProcessModelObject txProcessModelObject = obj as ITxProcessModelObject;
                if (txProcessModelObject != null)
                {
                    txPlanningObject = (txProcessModelObject.PlanningRepresentation as TxPlanningObject);
                }
            }
            return txPlanningObject;
        }

        internal static TxRobotInverseData.TxInverseType GetRobotInverseType()
        {
            TxRobotInverseData.TxInverseType result = 0;
            switch ((int)TxApplication.Options.Weld.Perpendicular)
            {
                case 0:
                    result = (TxRobotInverseData.TxInverseType)1;
                    break;
                case 1:
                    result = (TxRobotInverseData.TxInverseType)2;
                    break;
                case 2:
                    result = (TxRobotInverseData.TxInverseType)3;
                    break;
            }
            return result;
        }

        internal static ITxGun GetMountedGun(TxRobot robot)
        {
            if (!(robot != null))
            {
                return null;
            }
            if (robot.MountedTools.Count > 0)
            {
                return robot.MountedTools[0] as ITxGun;
            }
            return null;
        }

        internal static TxObjectList GetObjectsFromHierarchy(TxObjectList rootObjs, ITxTypeFilter filter, bool includeRoots, bool directChildrenOnly)
        {
            TxObjectList txObjectList = new TxObjectList();
            if (rootObjs != null && rootObjs.Count > 0 && filter != null)
            {
                foreach (ITxObject txObject in rootObjs)
                {
                    TxObjectList txObjectList2 = new TxObjectList();
                    if (includeRoots && filter.DoesPassFilter(txObject))
                    {
                        txObjectList2.Add(txObject);
                    }
                    else
                    {
                        ITxObjectCollection txObjectCollection = txObject as ITxObjectCollection;
                        if (txObjectCollection != null)
                        {
                            if (directChildrenOnly)
                            {
                                txObjectList2 = txObjectCollection.GetDirectDescendants(filter);
                            }
                            else
                            {
                                txObjectList2 = txObjectCollection.GetAllDescendants(filter);
                            }
                        }
                    }
                    if (txObjectList2 != null && txObjectList2.Count > 0)
                    {
                        foreach (ITxObject item in txObjectList2)
                        {
                            if (!txObjectList.Contains(item))
                            {
                                txObjectList.Add(item);
                            }
                        }
                    }
                }
            }
            return txObjectList;
        }

        internal const string RobotMatrixCommandBmp = "RobotMatrix.Res.wdc.bmp";

        internal const string RobotMatrixCommandBmp_LARGE = "RobotMatrix.Res.wdc_LARGE.png";

        //internal static string[] WeldabilityImages = new string[]
        //{
        //    "RobotMatrix.Res.no_reach.ico",
        //    "RobotMatrix.Res.ReachStatusPartialOutOfWorkingLimits.ico",
        //    "RobotMatrix.Res.fullReachOutOfWorkingLimits.ico",
        //    "RobotMatrix.Res.ReachStatusPartialOutOfWorkingLimits.ico",
        //    "RobotMatrix.Res.fullReachOutOfWorkingLimits.ico",
        //    "RobotMatrix.Res.ReachStatusPartial1.ico",
        //    "RobotMatrix.Res.fullReach.ico"
        //};

        internal const string FullReachImage = "RobotMatrix.Res.fullReach.ico";

        internal const string FullReachOutsideWorkingLimitsImage = "RobotMatrix.Res.fullReachOutOfWorkingLimits.ico";

        internal const string NoReachImage = "RobotMatrix.Res.no_reach.ico";

        internal const string PartialReachImage = "RobotMatrix.Res.ReachStatusPartial1.ico";

        internal const string PartialReachOutsideWorkingLimitsImage = "RobotMatrix.Res.ReachStatusPartialOutOfWorkingLimits.ico";

        internal const string PartialAccessibilityImage = "RobotMatrix.Res.ReachStatusPartial1.ico";

        internal const string PartialAccessibilityOutsideLimitsImage = "RobotMatrix.Res.ReachStatusPartialOutOfWorkingLimits.ico";

        internal const string ReachabilityStatusHeader = "RobotMatrix.Res.reach_status.ico";

        internal const string CollisionStatusHeader = "RobotMatrix.Res.coll_status.ico";

        internal const string AttributeStatusHeader = "RobotMatrix.Res.gunAttr.ico";

        internal const string CollidingImage = "RobotMatrix.Res.no_reach.ico";

        internal const string NotCollidingImage = "RobotMatrix.Res.fullReach.ico";

        internal const string NotCollidingOutsideLimitsImage = "RobotMatrix.Res.fullReachOutOfWorkingLimits.ico";

        internal const string MatchImage = "RobotMatrix.Res.fullReach.ico";

        internal const string NoMatchImage = "RobotMatrix.Res.no_reach.ico";

        internal const string WarningImage = "RobotMatrix.Res.warning.ico";

        internal const string GunImage = "RobotMatrix.Res.gun.ico";

        internal const string MountedTCPImage = "RobotMatrix.Res.MountedTCP.ico";

        internal const string ExternalTCPImage = "RobotMatrix.Res.ExternalTCP.ico";

        internal const string ImagesPath = "RobotMatrix.Res.";

        internal const string AllocatedTimeFieldName = "allocatedTime";

        internal const string WeldOperationClassNameToCollect = "Operation";

        internal const string WeldPointClassNameToCollect = "PmWeldPoint";

        internal const string WeldPointClassNameToCollectTC = "WeldPointRevision";

        internal const string WeldPointClassNameToDisplay = "WeldPoint";

        internal const string MfgFeatureClassName = "PmMfgFeature";

        internal const string MfgFeatureClassNameToDisplay = "MfgFeature";

        internal const string SubTypeAttribute = "subType";

        internal const string ExceedStatusTimeImage = "Exceed_Bar.bmp";

        internal const string StatusTimeImageSuffix = "_Bar.bmp";

        internal const int PaddingWidth = 5;

        internal const int AssignedColumnWidth = 25;

        internal const int ReachabilityColumnWidth = 25;

        internal const int AttributeColumnWidth = 25;

        internal const int OperationColumnWidth = 55;

        internal const int SimulatedGunColumnWidth = 75;

        internal const string locationFieldName = "location";

        internal const string rotationFieldName = "rotation";

        internal const string AttributesStorageKey = "WeldPointsAttribitesXml";

        internal const string WeldTimeFactorStorageKey = "WeldTimeFactor";

        internal const string CheckFlippedLocationsStorageKey = "CheckFlippedLocations";

        internal const string CheckAllWeldLocationsStorageKey = "CheckAllWeldLocations";

        internal const string ApplyAutomaticApproachAngleStorageKey = "ApplyAutomaticApproachAngle";

        internal const string ShowWeldLocationNameStorageKey = "ShowWeldLocationName";

        internal const string ShowWeldPointNameStorageKey = "ShowWeldPointName";

        internal const string GeoSubType = "GEO";

        internal const string RespotSubType = "Respot";

        //internal static string WELD_TIME_PARAMETER = "SW_TIME_ON_PT";

        //internal static string COOL_DOWN_TIME_PARAMETER = "SW_WAIT_TIME";

        //internal static string MOTION_TIME_PARAMETER = "RRS_MOTION_TIME";

        internal const string CheckWpVsGunAttributesKey = "CheckWpVsGunAttributes";

        internal enum EApRmxAccessibilityStatus
        {
            NoReachability,
            NotColliding,
            Colliding,
            UnChecked,
            Partial,
            NotCollidingOutsideWorkingLimits,
            NotCollidingOutsidePhysicalLimits,
            PartialOutsideWorkingLimits,
            PartialOutsidePhysicalLimits
        }

        public enum EApRmxReachabilityStatus
        {
            NoReach,
            UnChecked,
            FullOutsidePhisicalLimits,
            PartialOutsidePhisicalLimits,
            FullOutsideWorkingLimits,
            PartialOutsideWorkingLimits,
            Partial,
            Full,
            Error
        }

        internal enum EApRmxWeldabilityStatus
        {
            None,
            PartialOutsidePhisicalLimits,
            FullOutsidePhisicalLimits,
            PartialOutsideWorkingLimits,
            FullOutsideWorkingLimits,
            Partial,
            Full,
            UnChecked
        }

        internal enum EApRmxAttributeStatus
        {
            NotCalculated,
            Irrelevant,
            NoMatch,
            Match
        }

        internal enum EApRmxFilter
        {
            NoFilter,
            Geo,
            Respot,
            Assigned,
            Unassigned,
            FullByAssigned,
            FullByAny,
            PartialByAssigned,
            PartialByAny,
            NoneByAssigned,
            NoneByAny,
            NoCalc,
            All
        }

        internal enum EApAttributesMatchingFileStatus
        {
            AttributesMatchingFileExists,
            AttributesMatchingFileDoesNotExist,
            AttributesMatchingWrongFileFormat
        }
    }

}
