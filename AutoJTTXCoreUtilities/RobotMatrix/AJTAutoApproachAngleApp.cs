using EngineeringInternalExtension;
using EngineeringInternalExtension.DataTypes;
using EngineeringInternalExtension.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Olp;

namespace AutoJTTXCoreUtilities.RobotMatrix
{
    public class AJTAutoApproachAngleApp
    {
        //代表碰撞查询所使用的Context
        private TxCollisionQueryContext m_context;
        //位置信息
        private bool m_useLocationInformation;

        //pose dictionary
        private Dictionary<string, TxPose> m_poseDictionary;

        private const double RTSqrEpsilonD = 1E-10;

        public AJTAutoApproachAngleApp()
        {
            //碰撞对象的root
            TxCollisionRoot collisionRoot = TxApplication.ActiveDocument.CollisionRoot;
            //创建一个新的碰撞Query
            this.m_context = collisionRoot.CreateQueryContext();

            //Options 里的 motion > Robotics > "Use location information in static app..."			
            this.m_useLocationInformation = TxApplication.Options.Motion.UseLocationInformation;

        }

        /// <summary>
        /// pose dictionary
        /// </summary>
        public Dictionary<string, TxPose> PosesDictionary
        {
            get
            {
                return this.m_poseDictionary;
            }
            set
            {
                this.m_poseDictionary = value;
            }
        }

        /// <summary>
        /// 代表碰撞查询所使用的Context
        /// </summary>
        public TxCollisionQueryContext Context
        {
            get
            {
                return this.m_context;
            }
        }

        /// <summary>
        /// 位置信息
        /// </summary>
        public bool UseLocationInformation
        {
            get
            {
                return this.m_useLocationInformation;
            }
        }

        /// <summary>
        /// Get All Robots
        /// </summary>
        /// <returns></returns>
        public TxObjectList GetAllRobots()
        {
            TxTypeFilter txTypeFilter = new TxTypeFilter(typeof(TxRobot));
            return TxApplication.ActiveDocument.PhysicalRoot.GetAllDescendants(txTypeFilter);
        }

        /// <summary>
        /// Get All Guns
        /// </summary>
        /// <returns></returns>
        public TxObjectList GetAllGuns()
        {
            TxTypeFilter txTypeFilter = new TxTypeFilter(typeof(ITxTool));
            return TxApplication.ActiveDocument.PhysicalRoot.GetAllDescendants(txTypeFilter);
        }

        #region 获取用户选中的对象


        /// <summary>
        /// Get Unique Objects
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        public TxObjectList GetUniqueObjects(TxObjectList objs)
        {
            if (objs.Count <= 1)
            {
                return objs;
            }
            TxObjectList txObjectList = new TxObjectList(objs.Count);
            foreach (ITxObject item in objs)
            {
                if (!txObjectList.Contains(item))
                {
                    txObjectList.Add(item);
                }
            }
            return txObjectList;
        }

        /// <summary>
        /// Get Selected Loc Ops
        /// </summary>
        /// <returns></returns>
        public TxObjectList GetSelectedLocOps()
        {
            TxTypeFilter txTypeFilter = new TxTypeFilter(typeof(TxWeldLocationOperation));
            txTypeFilter.AddIncludedType(typeof(TxRoboticViaLocationOperation));
            txTypeFilter.AddIncludedType(typeof(TxGenericRoboticLocationOperation));
            txTypeFilter.AddIncludedType(typeof(TxRoboticSeamLocationOperation));
            TxObjectList filteredItems = TxApplication.ActiveSelection.GetFilteredItems(txTypeFilter);
            TxTypeFilter txTypeFilter2 = new TxTypeFilter(typeof(ITxWeldOperation));
            txTypeFilter2.AddIncludedType(typeof(ITxContinuousOperation));
            txTypeFilter2.AddIncludedType(typeof(TxGenericRoboticOperation));
            TxObjectList filteredItems2 = TxApplication.ActiveSelection.GetFilteredItems(txTypeFilter2);
            foreach (ITxObject txObject in filteredItems2)
            {
                ITxRoboticOrderedCompoundOperation txRoboticOrderedCompoundOperation = (ITxRoboticOrderedCompoundOperation)txObject;
                TxObjectList allDescendants = txRoboticOrderedCompoundOperation.GetAllDescendants(txTypeFilter);
                filteredItems.Append(allDescendants);
            }
            TxTypeFilter txTypeFilter3 = new TxTypeFilter(typeof(TxWeldPoint));
            TxObjectList filteredItems3 = TxApplication.ActiveSelection.GetFilteredItems(txTypeFilter3);
            foreach (ITxObject txObject2 in filteredItems3)
            {
                TxWeldPoint txWeldPoint = (TxWeldPoint)txObject2;
                if (txWeldPoint != null)
                {
                    TxObjectList weldLocationOperations = txWeldPoint.WeldLocationOperations;
                    filteredItems.Append(weldLocationOperations);
                }
            }
            TxTypeFilter txTypeFilter4 = new TxTypeFilter(typeof(TxRoboticSeamOperation));
            TxObjectList filteredItems4 = TxApplication.ActiveSelection.GetFilteredItems(txTypeFilter4);
            foreach (ITxObject txObject3 in filteredItems4)
            {
                ITxObjectCollection txObjectCollection = (ITxObjectCollection)txObject3;
                TxObjectList directDescendants = txObjectCollection.GetDirectDescendants(txTypeFilter);
                filteredItems.Append(directDescendants);
            }
            TxTypeFilter txTypeFilter5 = new TxTypeFilter(typeof(TxSeamMfgFeature));
            TxObjectList filteredItems5 = TxApplication.ActiveSelection.GetFilteredItems(txTypeFilter5);
            foreach (ITxObject txObject4 in filteredItems5)
            {
                TxSeamMfgFeature txSeamMfgFeature = (TxSeamMfgFeature)txObject4;
                TxObjectList roboticSeamOperations = txSeamMfgFeature.RoboticSeamOperations;
                foreach (ITxObject txObject5 in roboticSeamOperations)
                {
                    ITxObjectCollection txObjectCollection2 = (ITxObjectCollection)txObject5;
                    TxObjectList directDescendants2 = txObjectCollection2.GetDirectDescendants(txTypeFilter);
                    filteredItems.Append(directDescendants2);
                }
            }
            return this.GetUniqueObjects(filteredItems);
        }

        #endregion

        /// <summary>
        /// OpenPieChart
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="gun"></param>
        /// <param name="toolFrame"></param>
        /// <param name="pose"></param>
        public void OpenPieChart(TxRobot robot, ITxTool gun, TxFrame toolFrame, TxPose pose)
        {
            TxCommandManager txCommandManager = new TxCommandManager();
            TxObjectList txObjectList = new TxObjectList();
            if (robot != null)
            {
                txObjectList.Add(robot);
            }
            if (gun != null)
            {
                txObjectList.Add(gun);
            }
            if (toolFrame != null)
            {
                txObjectList.Add(toolFrame);
            }
            if (pose != null)
            {
                txObjectList.Add(pose);
            }
            txCommandManager.ExecuteCommandByCommandProgId("RobotPlacement.PieChartCmd.1", txObjectList, false);
        }

        /// <summary>
        /// Delete  Query Context
        /// </summary>
        public void DeleteContext()
        {
            TxCollisionRoot collisionRoot = TxApplication.ActiveDocument.CollisionRoot;
            collisionRoot.DeleteQueryContext(this.m_context);
        }


        private TxVector GetPlanePerpToNormalVec(TxVector normal)
        {
            TxVector txVector = new TxVector();
            double num = Math.Sqrt(normal.X * normal.X + normal.Y * normal.Y + normal.Z * normal.Z);
            txVector.X = normal.X / num;
            txVector.Y = normal.Y / num;
            txVector.Z = normal.Z / num;
            return txVector;
        }

        private TxVector GetVecFromCol(TxTransformation tFrame, int axis)
        {
            return new TxVector
            {
                X = tFrame[0, axis],
                Y = tFrame[1, axis],
                Z = tFrame[2, axis]
            };
        }

        private TxVector ProjectPointToPlain(TxVector point, TxVector plane)
        {
            TxVector txVector = new TxVector();
            double num = -this.GetScalarProduct(point, plane);
            txVector.X = point.X + num * plane.X;
            txVector.Y = point.Y + num * plane.Y;
            txVector.Z = point.Z + num * plane.Z;
            return txVector;
        }

        private double GetAlignmentAngle(TxTransformation loc, TxTransformation refLoc, TxWeldOptions.TxPerpendicularAxis perpAxis, TxWeldOptions.TxApproachAxis appAxis)
        {
            TxVector vecFromCol = this.GetVecFromCol(loc, (int)perpAxis);
            TxVector vecFromCol2 = this.GetVecFromCol(refLoc, (int)appAxis);
            TxVector vecFromCol3 = this.GetVecFromCol(loc, (int)appAxis);
            TxVector planePerpToNormalVec = this.GetPlanePerpToNormalVec(vecFromCol);
            TxVector v = this.ProjectPointToPlain(vecFromCol2, planePerpToNormalVec);
            return this.GetDirectedAngle(vecFromCol3, v, vecFromCol);
        }

        private double GetDirectedAngle(TxVector v1, TxVector v2, TxVector normal)
        {
            double num = Math.Sqrt(this.GetScalarProduct(v1, v1));
            double num2 = Math.Sqrt(this.GetScalarProduct(v2, v2));
            TxVector crossProduct = this.GetCrossProduct(v1, v2);
            double num3 = Math.Sqrt(this.GetScalarProduct(crossProduct, crossProduct));
            double num4 = num3 / (num * num2);
            double num5;
            if (num4 >= 1.0)
            {
                num5 = 1.5707963267948966;
            }
            else if (num4 <= -1.0)
            {
                num5 = -1.5707963267948966;
            }
            else
            {
                num5 = Math.Asin(num4);
            }
            if (this.GetScalarProduct(v1, v2) < 0.0)
            {
                num5 = 3.1415926535897931 - num5;
            }
            crossProduct = this.GetCrossProduct(v1, v2);
            if (this.GetScalarProduct(normal, crossProduct) < 0.0)
            {
                num5 = -num5;
            }
            return num5;
        }

        private double GetScalarProduct(TxVector vec1, TxVector vec2)
        {
            double num = 0.0;
            num += vec1.X * vec2.X;
            num += vec1.Y * vec2.Y;
            return num + vec1.Z * vec2.Z;
        }

        private TxVector GetCrossProduct(TxVector vec1, TxVector vec2)
        {
            return new TxVector
            {
                X = vec1.Y * vec2.Z - vec1.Z * vec2.Y,
                Y = vec1.Z * vec2.X - vec1.X * vec2.Z,
                Z = vec1.X * vec2.Y - vec1.Y * vec2.X
            };
        }

        private void AlignWeldLocationOperation(ITxLocatableObject originalWeldLocationOperation, ITxLocatableObject newWeldLocationOperation)
        {
            TxTransformation absoluteLocation = newWeldLocationOperation.AbsoluteLocation;
            TxTransformation absoluteLocation2 = originalWeldLocationOperation.AbsoluteLocation;
            TxWeldOptions.TxPerpendicularAxis perpendicular = TxApplication.Options.Weld.Perpendicular;
            TxWeldOptions.TxApproachAxis approach = TxApplication.Options.Weld.Approach;
            if (absoluteLocation != absoluteLocation2)
            {
                double alignmentAngle = this.GetAlignmentAngle(absoluteLocation, absoluteLocation2, perpendicular, approach);
                if (alignmentAngle > 1E-10 || alignmentAngle < -1E-10)
                {
                    TxTransformation absoluteLocation3 = this.RotateLocAboutPerp(absoluteLocation, perpendicular, alignmentAngle);
                    newWeldLocationOperation.AbsoluteLocation = absoluteLocation3;
                }
            }
        }

        private TxTransformation RotateLocAboutPerp(TxTransformation loc, TxWeldOptions.TxPerpendicularAxis perpAxis, double angle)
        {
            TxTransformation rotationMatrix = this.GetRotationMatrix(perpAxis, angle);
            return loc * rotationMatrix;
        }

        private TxTransformation GetRotationMatrix(TxWeldOptions.TxPerpendicularAxis axis, double theta)
        {
            TxTransformation result = null;
            switch (axis)
            {
                case TxWeldOptions.TxPerpendicularAxis.X:
                    result = new TxTransformation(new TxVector(theta, 0.0, 0.0), TxTransformation.TxRotationType.RPY_XYZ);
                    break;
                case TxWeldOptions.TxPerpendicularAxis.Y:
                    result = new TxTransformation(new TxVector(0.0, theta, 0.0), TxTransformation.TxRotationType.RPY_XYZ);
                    break;
                case TxWeldOptions.TxPerpendicularAxis.Z:
                    result = new TxTransformation(new TxVector(0.0, 0.0, theta), TxTransformation.TxRotationType.RPY_XYZ);
                    break;
            }
            return result;
        }

        //public Dictionary<int, List<TxLocationAvailabilitySector>> CalculatePieCharts(TxObjectList locations, TxRobot robot, ITxTool gun, TxPose pose, TxFrame toolFrame,
        //    bool automaticFlip, TxAutomaticApproachAngleAlgorithmTypeEx angleAlgorithm)
        //{
        //    Dictionary<int, List<TxLocationAvailabilitySector>> dictionary = new Dictionary<int, List<TxLocationAvailabilitySector>>();
        //    int num = 0;
        //    int num2 = 0;
        //    foreach (ITxObject txObject in locations)
        //    {
        //        //当前焊点
        //        ITxLocationOperation txLocationOperation = (ITxLocationOperation)txObject;
        //        AdjustLocationParams adjustLocationParams = this.InitLocationInformationParams(txLocationOperation, gun, robot, pose, toolFrame, angleAlgorithm);
        //        //当前焊点的绝对位姿
        //        TxTransformation absoluteLocation = (adjustLocationParams.m_locOp as ITxLocatableObject).AbsoluteLocation;

        //        if (angleAlgorithm == TxAutomaticApproachAngleAlgorithmTypeEx.MiddleAngleOfCommonSegment && dictionary.Count > 0)
        //        {
        //            bool flag = TxApplication.PlatformType == TxPlatformType.RobotExpert || AJTApGenUtils.IsLocationUnderContinuousOperationHierarchy(adjustLocationParams.m_locOp);
        //            ITxLocatableObject txLocatableObject = locations[num2] as ITxLocatableObject;
        //            TxTransformation absoluteLocation2 = (adjustLocationParams.m_locOp as ITxLocatableObject).AbsoluteLocation;
        //            TxVector rotationRPY_XYZ = txLocatableObject.AbsoluteLocation.RotationRPY_XYZ;
        //            this.AlignWeldLocationOperation(locations[num2] as ITxLocatableObject, adjustLocationParams.m_locOp as ITxLocatableObject);
        //        }

        //        List<TxLocationAvailabilitySector> list = AJTApGenUtils.CalculateLocationPieChart(adjustLocationParams, 0.087266462599716474);

        //        if (!this.DoesSolutionExist2(list) && automaticFlip)
        //        {
        //            TxObjectList txObjectList = new TxObjectList();
        //            txObjectList.Add(txLocationOperation);
        //            this.FlipLocation(txObjectList, false);
        //            list = AJTApGenUtils.CalculateLocationPieChart(adjustLocationParams, 0.087266462599716474);
        //            if (!this.DoesSolutionExist2(list))
        //            {
        //                this.FlipLocation(txObjectList, false);
        //            }
        //        }

        //        if (angleAlgorithm == TxAutomaticApproachAngleAlgorithmTypeEx.MiddleAngleOfCommonSegment && list[0].StartAngle == 0.0 && list[0].State.ReachabilityState == TxLocationAvailabilityState.ReachabilityStateType.FullReach &&
        //            list[0].State.CollisionState == TxLocationAvailabilityState.CollisionStateType.CollisionNotFound &&
        //            list[list.Count - 1].EndAngle == 6.2831853071795862 && list[list.Count - 1].State.ReachabilityState == TxLocationAvailabilityState.ReachabilityStateType.FullReach &&
        //            list[list.Count - 1].State.CollisionState == TxLocationAvailabilityState.CollisionStateType.CollisionNotFound)
        //        {
        //            list[list.Count - 1].EndAngle += list[0].EndAngle;
        //            list.RemoveAt(0);
        //        }

        //        if (angleAlgorithm == TxAutomaticApproachAngleAlgorithmTypeEx.MiddleAngleOfCommonSegment && dictionary.Count > 0)
        //        {
        //            List<TxLocationAvailabilitySector> list2 = this.FindCommonSegment(dictionary[num - 1], list);
        //            list2 = this.RemoveSmallCommonSegments(list2);
        //            if (list2 != null && list2.Count > 0)
        //            {
        //                for (int i = num2; i < num; i++)
        //                {
        //                    dictionary[i] = list2;
        //                    list = list2;
        //                }
        //            }
        //            else
        //            {
        //                num2 = num;
        //                bool flag2 = false;
        //                foreach (TxLocationAvailabilitySector txLocationAvailabilitySector in list)
        //                {
        //                    if (txLocationAvailabilitySector.State.ReachabilityState == TxLocationAvailabilityState.ReachabilityStateType.FullReach &&
        //                        txLocationAvailabilitySector.State.CollisionState == TxLocationAvailabilityState.CollisionStateType.CollisionNotFound)
        //                    {
        //                        flag2 = true;
        //                        break;
        //                    }
        //                }
        //                if (!flag2)
        //                {
        //                    (adjustLocationParams.m_locOp as ITxLocatableObject).AbsoluteLocation = absoluteLocation;
        //                }
        //            }
        //        }

        //        dictionary.Add(num, list);
        //        num++;
        //    }
        //    return dictionary;
        //}

        /// <summary>
        /// 只计算最优区域(Optimal Zone)
        /// </summary>
        /// <param name="location"></param>
        /// <param name="robot"></param>
        /// <param name="gun"></param>
        /// <param name="pose"></param>
        /// <param name="toolFrame"></param>
        /// <param name="automaticFlip"></param>
        /// <param name="angleAlgorithm"></param>
        /// <returns></returns>
        public void CalculatePieCharts2(ITxLocationOperation location, TxRobot robot, ITxTool gun, TxPose pose, TxFrame toolFrame,
        bool automaticFlip, out List<TxLocationAvailabilitySector> list, TxAutomaticApproachAngleAlgorithmTypeEx angleAlgorithm = TxAutomaticApproachAngleAlgorithmTypeEx.MiddleAngleOfLargestSegment)
        {
            //当前焊点
            ITxLocationOperation txLocationOperation = location;
            AdjustLocationParams adjustLocationParams = this.InitLocationInformationParams(txLocationOperation, gun, robot, pose, toolFrame, angleAlgorithm);

            //当前焊点的绝对位姿
            TxTransformation absoluteLocation = (adjustLocationParams.m_locOp as ITxLocatableObject).AbsoluteLocation;

            //首先不flip当前状态, 并Sample step size 5(deg)
            list = AJTApGenUtils.CalculateLocationPieChart(adjustLocationParams, 0.087266462599716474);

            //检查计算结果 , 不满足则进行flip
            if (!this.DoesSolutionExist2(list) && automaticFlip)
            {
                TxObjectList txObjectList = new TxObjectList();
                txObjectList.Add(txLocationOperation);
                this.FlipLocation(txObjectList, false);
                //再次计算flip后的状态, 并Sample step size 5(deg)
                list = AJTApGenUtils.CalculateLocationPieChart(adjustLocationParams, 0.087266462599716474);

                //再次检查二次计算的结果
                if (!this.DoesSolutionExist2(list))
                {
                    //如果不满足则恢复焊点的状态
                    this.FlipLocation(txObjectList, false);
                }
            }
        }

        public List<TxLocationAvailabilitySector> RemoveSmallCommonSegments(List<TxLocationAvailabilitySector> commonSectors)
        {
            List<TxLocationAvailabilitySector> list = new List<TxLocationAvailabilitySector>();
            foreach (TxLocationAvailabilitySector txLocationAvailabilitySector in commonSectors)
            {
                if (Math.Abs(txLocationAvailabilitySector.EndAngle - txLocationAvailabilitySector.StartAngle) >= 0.087266462599716474)
                {
                    list.Add(txLocationAvailabilitySector);
                }
            }
            return list;
        }

        public List<TxLocationAvailabilitySector> FindCommonSegment(List<TxLocationAvailabilitySector> commonPieChart, List<TxLocationAvailabilitySector> currPieChart)
        {
            List<TxLocationAvailabilitySector> list = new List<TxLocationAvailabilitySector>();
            foreach (TxLocationAvailabilitySector txLocationAvailabilitySector in currPieChart)
            {
                if (txLocationAvailabilitySector.State.ReachabilityState == TxLocationAvailabilityState.ReachabilityStateType.FullReach && txLocationAvailabilitySector.State.CollisionState == TxLocationAvailabilityState.CollisionStateType.CollisionNotFound)
                {
                    int containingSectorIndex = this.GetContainingSectorIndex(commonPieChart, txLocationAvailabilitySector.StartAngle);
                    if (containingSectorIndex > -1)
                    {
                        double startAngle = txLocationAvailabilitySector.StartAngle;
                        double num = commonPieChart[containingSectorIndex].EndAngle;
                        int containingSectorIndex2 = this.GetContainingSectorIndex(commonPieChart, txLocationAvailabilitySector.EndAngle);
                        if (containingSectorIndex2 > -1 && containingSectorIndex == containingSectorIndex2)
                        {
                            num = txLocationAvailabilitySector.EndAngle;
                        }
                        if (num - startAngle > 6.2831853071795862)
                        {
                            num -= 6.2831853071795862;
                        }
                        TxLocationAvailabilitySector txLocationAvailabilitySector2 = new TxLocationAvailabilitySector(startAngle, num, new TxLocationAvailabilityState(TxLocationAvailabilityState.ReachabilityStateType.FullReach, TxLocationAvailabilityState.CollisionStateType.CollisionNotFound, 0));
                        if (!this.DoesPieChartContainSector(list, txLocationAvailabilitySector2))
                        {
                            list.Add(txLocationAvailabilitySector2);
                        }
                        if (containingSectorIndex2 > -1 && containingSectorIndex != containingSectorIndex2)
                        {
                            num = txLocationAvailabilitySector.EndAngle;
                            startAngle = commonPieChart[containingSectorIndex2].StartAngle;
                            if (num - startAngle > 6.2831853071795862)
                            {
                                num -= 6.2831853071795862;
                            }
                            txLocationAvailabilitySector2 = new TxLocationAvailabilitySector(startAngle, num, new TxLocationAvailabilityState(TxLocationAvailabilityState.ReachabilityStateType.FullReach, TxLocationAvailabilityState.CollisionStateType.CollisionNotFound, 0));
                            if (!this.DoesPieChartContainSector(list, txLocationAvailabilitySector2))
                            {
                                list.Add(txLocationAvailabilitySector2);
                            }
                        }
                    }
                    else
                    {
                        containingSectorIndex = this.GetContainingSectorIndex(commonPieChart, txLocationAvailabilitySector.EndAngle);
                        if (containingSectorIndex > -1)
                        {
                            double startAngle = commonPieChart[containingSectorIndex].StartAngle;
                            int containingSectorIndex3 = this.GetContainingSectorIndex(commonPieChart, txLocationAvailabilitySector.EndAngle);
                            if (containingSectorIndex3 > -1 && containingSectorIndex == containingSectorIndex3)
                            {
                                double num = txLocationAvailabilitySector.EndAngle;
                                if (num - startAngle > 6.2831853071795862)
                                {
                                    num -= 6.2831853071795862;
                                }
                                TxLocationAvailabilitySector txLocationAvailabilitySector3 = new TxLocationAvailabilitySector(startAngle, num, new TxLocationAvailabilityState(TxLocationAvailabilityState.ReachabilityStateType.FullReach, TxLocationAvailabilityState.CollisionStateType.CollisionNotFound, 0));
                                if (!this.DoesPieChartContainSector(list, txLocationAvailabilitySector3))
                                {
                                    list.Add(txLocationAvailabilitySector3);
                                }
                            }
                        }
                        else
                        {
                            foreach (TxLocationAvailabilitySector txLocationAvailabilitySector4 in commonPieChart)
                            {
                                if (txLocationAvailabilitySector4.State.ReachabilityState == TxLocationAvailabilityState.ReachabilityStateType.FullReach &&
                                    txLocationAvailabilitySector4.State.CollisionState == TxLocationAvailabilityState.CollisionStateType.CollisionNotFound)
                                {
                                    containingSectorIndex = this.GetContainingSectorIndex(currPieChart, txLocationAvailabilitySector4.StartAngle);
                                    if (containingSectorIndex > -1)
                                    {
                                        double startAngle = txLocationAvailabilitySector4.StartAngle;
                                        double num = currPieChart[containingSectorIndex].EndAngle;
                                        int containingSectorIndex4 = this.GetContainingSectorIndex(currPieChart, txLocationAvailabilitySector4.EndAngle);
                                        if (containingSectorIndex4 > -1 && containingSectorIndex == containingSectorIndex4)
                                        {
                                            num = txLocationAvailabilitySector4.EndAngle;
                                        }
                                        if (num - startAngle > 6.2831853071795862)
                                        {
                                            num -= 6.2831853071795862;
                                        }
                                        TxLocationAvailabilitySector txLocationAvailabilitySector5 = new TxLocationAvailabilitySector(startAngle, num, new TxLocationAvailabilityState(TxLocationAvailabilityState.ReachabilityStateType.FullReach, TxLocationAvailabilityState.CollisionStateType.CollisionNotFound, 0));
                                        if (!this.DoesPieChartContainSector(list, txLocationAvailabilitySector5))
                                        {
                                            list.Add(txLocationAvailabilitySector5);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return list;
        }

        private bool DoesPieChartContainSector(List<TxLocationAvailabilitySector> pieChart, TxLocationAvailabilitySector sector)
        {
            bool result = false;
            foreach (TxLocationAvailabilitySector txLocationAvailabilitySector in pieChart)
            {
                if (txLocationAvailabilitySector.StartAngle == sector.StartAngle &&
                    txLocationAvailabilitySector.EndAngle == sector.EndAngle &&
                    txLocationAvailabilitySector.State.ReachabilityState == sector.State.ReachabilityState && txLocationAvailabilitySector.State.CollisionState == sector.State.CollisionState)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private bool DoesSolutionExist(List<TxLocationAvailabilitySector> sectors)
        {
            if (sectors == null)
            {
                return false;
            }
            bool result = false;
            foreach (TxLocationAvailabilitySector txLocationAvailabilitySector in sectors)
            {
                if (txLocationAvailabilitySector.State.ReachabilityState == TxLocationAvailabilityState.ReachabilityStateType.FullReach &&
                    txLocationAvailabilitySector.State.CollisionState == TxLocationAvailabilityState.CollisionStateType.CollisionNotFound)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 不检查碰撞
        /// </summary>
        /// <param name="sectors"></param>
        /// <returns></returns>
        private bool DoesSolutionExist2(List<TxLocationAvailabilitySector> sectors)
        {
            if (sectors == null)
            {
                return false;
            }
            bool result = false;
            foreach (TxLocationAvailabilitySector txLocationAvailabilitySector in sectors)
            {
                if (txLocationAvailabilitySector.State.ReachabilityState == TxLocationAvailabilityState.ReachabilityStateType.FullReach &&
                    txLocationAvailabilitySector.State.CollisionState == TxLocationAvailabilityState.CollisionStateType.CollisionNotFound ||
                    txLocationAvailabilitySector.State.ReachabilityState == TxLocationAvailabilityState.ReachabilityStateType.FullReach &&
                    txLocationAvailabilitySector.State.CollisionState == TxLocationAvailabilityState.CollisionStateType.Unchecked)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private int GetContainingSectorIndex(List<TxLocationAvailabilitySector> commonPieChart, double angle)
        {
            int result = -1;
            foreach (TxLocationAvailabilitySector txLocationAvailabilitySector in commonPieChart)
            {
                if (txLocationAvailabilitySector.State.ReachabilityState == TxLocationAvailabilityState.ReachabilityStateType.FullReach &&
                    txLocationAvailabilitySector.State.CollisionState == TxLocationAvailabilityState.CollisionStateType.CollisionNotFound &&
                    ((txLocationAvailabilitySector.StartAngle < angle && txLocationAvailabilitySector.EndAngle > angle) || (txLocationAvailabilitySector.StartAngle - 6.2831853071795862 < angle && txLocationAvailabilitySector.EndAngle - 6.2831853071795862 > angle)))
                {
                    result = commonPieChart.IndexOf(txLocationAvailabilitySector);
                    break;
                }
            }
            return result;
        }


        /// <summary> 
        /// Get Robot for ITxRoboticLocationOperation
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public TxRobot GetRobot(ITxRoboticLocationOperation location)
        {
            TxRobot result = null;
            if (location != null && location.ParentRoboticOperation != null)
            {
                result = (location.ParentRoboticOperation.Robot as TxRobot);
            }
            return result;
        }


        /// <summary>
        /// Get Robotic Controller Services
        /// </summary>
        /// <param name="robot"></param>
        /// <returns></returns>
        public ITxRoboticControllerServices GetRoboticControllerServices(TxRobot robot)
        {
            ITxRoboticControllerServices txRoboticControllerServices = null;
            if (robot != null && robot.Controller != null)
            {
                TxOlpControllerUtilities txOlpControllerUtilities = new TxOlpControllerUtilities();
                txRoboticControllerServices = (ITxRoboticControllerServices)txOlpControllerUtilities.GetInterfaceImplementationFromController(robot.Controller.Name, typeof(ITxRoboticControllerServices), typeof(TxControllerAttribute), "ControllerName");
                if (txRoboticControllerServices == null)
                {
                    txRoboticControllerServices = (ITxRoboticControllerServices)txOlpControllerUtilities.GetInterfaceImplementationFromController("default", typeof(ITxRoboticControllerServices), typeof(TxControllerAttribute), "ControllerName");
                }
                if (txRoboticControllerServices != null)
                {
                    txRoboticControllerServices.Init(robot);
                }
            }
            return txRoboticControllerServices;
        }

        public AdjustLocationParams InitLocationInformationParams(ITxLocationOperation locOp, ITxTool gun, TxRobot robot, TxPose pose, TxFrame toolFrame, TxAutomaticApproachAngleAlgorithmTypeEx angleAlgorithm)
        {
            ITxRoboticControllerServices txRoboticControllerServices = null;
            if (this.UseLocationInformation)
            {
                robot = this.GetRobot(locOp as ITxRoboticLocationOperation);
                if (robot != null)
                {
                    txRoboticControllerServices = this.GetRoboticControllerServices(robot);
                }
                if (txRoboticControllerServices != null)
                {
                    gun = (txRoboticControllerServices.GetActiveGun(locOp as ITxRoboticLocationOperation) as ITxTool);
                    if (gun == null)
                    {
                        gun = (txRoboticControllerServices.GetActiveGripper(locOp as ITxRoboticLocationOperation) as ITxTool);
                    }
                }
                else
                {
                    gun = null;
                }
                if (gun != null && pose != null)
                {
                    string key = gun.Id + "|" + pose;
                    this.PosesDictionary.TryGetValue(key, out pose);
                }
            }
            ArrayList arrayList = new ArrayList();
            arrayList.Add(pose);
            TxInputReachabilityAndAccessibilityDataGunScenarioType gunScenario = 0;
            AdjustLocationParams adjustLocationParams = new AdjustLocationParams(robot, gun, arrayList, locOp, this.Context, gunScenario, txRoboticControllerServices, angleAlgorithm);
            if (toolFrame != null)
            {
                adjustLocationParams.SetToolFrame(toolFrame.AbsoluteLocation);
            }
            return adjustLocationParams;
        }

        public void FlipLocation(TxObjectList selectedLocations, bool refreshDisplay)
        {
            TxCommandManager txCommandManager = new TxCommandManager();
            bool flag = txCommandManager.ExecuteCommandByCommandProgId("WeldLocationsManipulation.FlipLocsCmd.1", selectedLocations, false);
            if (flag && refreshDisplay)
            {
                TxApplication.RefreshDisplay();
            }
        }
    }
}