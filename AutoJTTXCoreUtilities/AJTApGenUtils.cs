using AutoJTTXCoreUtilities.RobotMatrix;
using EngineeringInternalExtension;
using EngineeringInternalExtension.DataTypes;
using EngineeringInternalExtension.Options;
using EngineeringInternalExtension.Robotics;
using System;
using System.Collections;
using System.Collections.Generic;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Olp;
using Tecnomatix.Engineering.PrivateImplementationDetails;
using Tecnomatix.Engineering.Ui;

namespace AutoJTTXCoreUtilities
{
    public class AJTApGenUtils
    {
        public static bool AdjustLocation(TxRobot robot, ITxTool gun, ArrayList poseList, ITxLocationOperation locOp, TxCollisionQueryContext context, TxInputReachabilityAndAccessibilityDataGunScenarioType gunScenario, out AJTApRmxUtils.EApRmxReachabilityStatus reachabilityStatus)
        {
            ITxRoboticControllerServices rcs = null;
            return AJTApGenUtils.AdjustLocation(robot, gun, poseList, locOp, context, gunScenario, rcs, out reachabilityStatus);
        }

        public static bool AdjustLocation(TxRobot robot, ITxTool gun, ArrayList poseList, ITxLocationOperation locOp, TxCollisionQueryContext context, TxInputReachabilityAndAccessibilityDataGunScenarioType gunScenario, ITxRoboticControllerServices rcs, out AJTApRmxUtils.EApRmxReachabilityStatus reachabilityStatus)
        {
            TxAutomaticApproachAngleAlgorithmTypeEx txAutomaticApproachAngleAlgorithmTypeEx = TxOptionsEx.AutomaticApproachAngleAlgorithm;
            if (txAutomaticApproachAngleAlgorithmTypeEx == (TxAutomaticApproachAngleAlgorithmTypeEx)2)
            {
                txAutomaticApproachAngleAlgorithmTypeEx = 0;
            }
            AdjustLocationParams alParams = new AdjustLocationParams(robot, gun, poseList, locOp, context, gunScenario, rcs, txAutomaticApproachAngleAlgorithmTypeEx);
            return AJTApGenUtils.AdjustLocation(alParams, out reachabilityStatus);
        }

        public static bool AdjustLocation(AdjustLocationParams alParams, out AJTApRmxUtils.EApRmxReachabilityStatus reachabilityStatus)
        {
            List<TxLocationAvailabilitySector> sectors = AJTApGenUtils.CalculateLocationPieChart(alParams, 0.087266462599716474);
            return AJTApGenUtils.AdjustLocation(alParams, sectors, out reachabilityStatus);
        }

        public static bool AdjustLocationWithPieChart(AdjustLocationParams alParams, List<TxLocationAvailabilitySector> pieChart, out AJTApRmxUtils.EApRmxReachabilityStatus reachabilityStatus)
        {
            return AJTApGenUtils.AdjustLocation(alParams, pieChart, out reachabilityStatus);
        }

        public static bool AdjustLocation(AdjustLocationParams alParams, List<TxLocationAvailabilitySector> sectors, out AJTApRmxUtils.EApRmxReachabilityStatus reachabilityStatus)
        {
            //可达状态
            reachabilityStatus = AJTApRmxUtils.EApRmxReachabilityStatus.UnChecked;
            //当前焊点
            TxTransformation absoluteLocation = (alParams.m_locOp as ITxLocatableObject).AbsoluteLocation;

            if (sectors == null)
            {
                reachabilityStatus = AJTApRmxUtils.EApRmxReachabilityStatus.NoReach;
                return false;
            }
            //转换可达状态数据
            ArrayList arrayList = AJTApGenUtils.AAA_ConvertUtilitySectorsToSegments(sectors);
            if (arrayList.Count == 1 && (alParams.m_algorithm != (TxAutomaticApproachAngleAlgorithmTypeEx)2 || (alParams.m_algorithm == (TxAutomaticApproachAngleAlgorithmTypeEx)2 && ((Segment)arrayList[0]).endAngle - ((Segment)arrayList[0]).startAngle == 6.2831853071795862)))
            {
                Segment segment = arrayList[0] as Segment;
                reachabilityStatus = segment.statuts;
            }
            else if (arrayList.Count > 1 || (alParams.m_algorithm == (TxAutomaticApproachAngleAlgorithmTypeEx)2 && arrayList.Count > 0))
            {
                if (!AJTApGenUtils.AAA_DoesExistSuccessfulStatusInSegments(arrayList))
                {
                    reachabilityStatus = AJTApRmxUtils.EApRmxReachabilityStatus.NoReach;
                }
                else
                {
                    double num;
                    switch ((int)alParams.m_algorithm)
                    {
                        case 0:
                        case 2:
                            num = AJTApGenUtils.GetMiddleAngleOfLargestSegment(arrayList, out reachabilityStatus);
                            break;
                        case 1:
                            {
                                bool flag = false;
                                AJTApRmxUtils.EApRmxReachabilityStatus eapRmxReachabilityStatus = AJTApRmxUtils.EApRmxReachabilityStatus.UnChecked;
                                num = AJTApGenUtils.GetAngleNearEdgeOfClosestSegment(arrayList, out eapRmxReachabilityStatus, out flag);
                                reachabilityStatus = eapRmxReachabilityStatus;
                                break;
                            }
                        default:
                            num = 0.0;
                            break;
                    }
                    if (num != 0.0)
                    {
                        bool seamLocation = TxApplication.PlatformType == (TxPlatformType)3 || AJTApGenUtils.IsLocationUnderContinuousOperationHierarchy(alParams.m_locOp);
                        TxTransformation absoluteLocation2 = AJTApGenUtils.RotateLocationAroundPerpendicular(absoluteLocation, num, seamLocation);
                        (alParams.m_locOp as ITxLocatableObject).AbsoluteLocation = absoluteLocation2;
                    }
                }
            }
            return AJTApGenUtils.AAA_IsSuccessfulStatus(reachabilityStatus);
        }

        public static bool IsReachableForAllPoses(AdjustLocationParams alParams, TxTransformation loc, out AJTApRmxUtils.EApRmxReachabilityStatus currentReachStatus)
        {
            bool flag = false;
            currentReachStatus = AJTApRmxUtils.EApRmxReachabilityStatus.NoReach;
            for (int i = 0; i < alParams.m_poseList.Count; i++)
            {
                TxPose txPose = alParams.m_poseList[i] as TxPose;
                ArrayList arrayList = new ArrayList(1);
                if (txPose != null)
                {
                    arrayList.Add(txPose);
                }
                TxInputReachabilityAndAccessibilityData reachAccessData = new TxInputReachabilityAndAccessibilityData(alParams.m_locOp as ITxRoboticLocationOperation, alParams.m_robot, alParams.m_gun, arrayList, alParams.m_context, false, true, alParams.m_gunScenario);
                if (alParams.m_robot != null)
                {
                    TxJointLimitStatusTypeEx txJointLimitStatusTypeEx = (TxJointLimitStatusTypeEx)4;
                    flag = AJTApGenUtils.CheckLocationReachabilityCollisisionFree(alParams.m_robot, loc, alParams.m_gun, alParams.m_locOp, reachAccessData, alParams.m_rcs, out txJointLimitStatusTypeEx);
                    if (flag)
                    {
                        if (txJointLimitStatusTypeEx >= (TxJointLimitStatusTypeEx)3)
                        {
                            currentReachStatus = AJTApRmxUtils.EApRmxReachabilityStatus.Full;
                        }
                        else if (txJointLimitStatusTypeEx == (TxJointLimitStatusTypeEx)2)
                        {
                            currentReachStatus = AJTApRmxUtils.EApRmxReachabilityStatus.FullOutsideWorkingLimits;
                        }
                        else
                        {
                            currentReachStatus = AJTApRmxUtils.EApRmxReachabilityStatus.FullOutsidePhisicalLimits;
                        }
                    }
                    else
                    {
                        currentReachStatus = AJTApRmxUtils.EApRmxReachabilityStatus.NoReach;
                    }
                }
                else
                {
                    ITxLocatableObject txLocatableObject = alParams.m_locOp as ITxLocatableObject;
                    TxTransformation absoluteLocation = txLocatableObject.AbsoluteLocation;
                    txLocatableObject.AbsoluteLocation = loc;
                    TxPoseData robotPose = null;
                    flag = !AJTApGenUtils.CheckCollisionStatus(reachAccessData, robotPose);
                    if (flag)
                    {
                        currentReachStatus = AJTApRmxUtils.EApRmxReachabilityStatus.Full;
                    }
                    else
                    {
                        currentReachStatus = AJTApRmxUtils.EApRmxReachabilityStatus.NoReach;
                    }
                    txLocatableObject.AbsoluteLocation = absoluteLocation;
                }
                if (!flag)
                {
                    break;
                }
            }
            return flag;
        }

        internal static bool IsLocationUnderContinuousOperationHierarchy(ITxLocationOperation locOp)
        {
            bool result = false;
            if (locOp is TxRoboticSeamLocationOperation)
            {
                return true;
            }
            if (locOp is TxRoboticViaLocationOperation)
            {
                return locOp.Collection is ITxContinuousOperation;
            }
            return result;
        }

        public static bool CalcLocationReachabilityStatus(AdjustLocationParams alParams, out AJTApRmxUtils.EApRmxReachabilityStatus reachabilityStatus, bool iscalcCollision = false)
        {
            reachabilityStatus = AJTApGenUtils.AAA_CalculateCurrentLocationStatus(alParams,iscalcCollision);
            return AJTApGenUtils.AAA_IsSuccessfulStatus(reachabilityStatus);
        }

        internal static TxTransformation RotateLocationAroundPerpendicular(TxTransformation loc, double angleRad, bool seamLocation)
        {
            TxWeldOptions.TxPerpendicularAxis txPerpendicularAxis;
            if (seamLocation)
            {
                switch ((int)TxApplication.Options.Continuous.Normal)
                {
                    case 0:
                    case 3:
                        txPerpendicularAxis = (TxWeldOptions.TxPerpendicularAxis)0;
                        goto IL_53;
                    case 1:
                    case 4:
                        txPerpendicularAxis = (TxWeldOptions.TxPerpendicularAxis)1;
                        goto IL_53;
                }
                txPerpendicularAxis = (TxWeldOptions.TxPerpendicularAxis)2;
            }
            else
            {
                txPerpendicularAxis = TxApplication.Options.Weld.Perpendicular;
            }
        IL_53:
            TxTransformation txTransformation;
            if (txPerpendicularAxis == null)
            {
                txTransformation = new TxTransformation(new TxVector(angleRad, 0.0, 0.0), (TxTransformation.TxRotationType)1);
            }
            else if (txPerpendicularAxis == (TxWeldOptions.TxPerpendicularAxis)1)
            {
                txTransformation = new TxTransformation(new TxVector(0.0, angleRad, 0.0), (TxTransformation.TxRotationType)1);
            }
            else if (txPerpendicularAxis == (TxWeldOptions.TxPerpendicularAxis)2)
            {
                txTransformation = new TxTransformation(new TxVector(0.0, 0.0, angleRad), (TxTransformation.TxRotationType)1);
            }
            else
            {
                txTransformation = new TxTransformation();
            }
            return loc * txTransformation;
        }

        internal static TxTransformation FlippedLocationAroundApproach(TxTransformation loc)
        {
            TxTransformation txTransformation = null;
            if (loc != null)
            {
                int num = -1;
                switch ((int)TxApplication.Options.Weld.Approach)
                {
                    case 0:
                    case 3:
                        num = 0;
                        break;
                    case 1:
                    case 4:
                        num = 1;
                        break;
                    case 2:
                    case 5:
                        num = 2;
                        break;
                }
                if (num > -1)
                {
                    txTransformation = new TxTransformation(loc);
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (j != num)
                            {
                                TxTransformation txTransformation2 = txTransformation;
                                int num2 = i;
                                int num3 = j;
                                txTransformation2[num2, num3] *= -1.0;
                            }
                        }
                    }
                }
            }
            return txTransformation;
        }

        internal static void JumpGunToLocation(ITxGun gun, TxTransformation location)
        {
            if (gun != null)
            {
                ITxTool txTool = gun as ITxTool;
                TxFrame tcpf = txTool.TCPF;
                TxTransformation absoluteLocation = tcpf.AbsoluteLocation;
                TxTransformation inverse = absoluteLocation.Inverse;
                TxTransformation txTransformation = location * inverse;
                ITxLocatableObject txLocatableObject = gun as ITxLocatableObject;
                TxTransformation absoluteLocation2 = txLocatableObject.AbsoluteLocation;
                TxTransformation absoluteLocation3 = txTransformation * absoluteLocation2;
                txLocatableObject.AbsoluteLocation = absoluteLocation3;
            }
        }

        internal static bool JumpWorkpieceToGun(ITxLocationOperation locationOp, ITxGun gun)
        {
            bool result = false;
            if (locationOp != null && gun != null)
            {
                ITxLocatableObject attachmentParent = (locationOp as ITxLocatableObject).AttachmentParent;
                if (attachmentParent != null && attachmentParent is ITxComponent)
                {
                    TxTransformation absoluteLocation = (gun as ITxTool).TCPF.AbsoluteLocation;
                    TxTransformation absoluteLocation2 = attachmentParent.AbsoluteLocation;
                    TxTransformation absoluteLocation3 = (locationOp as ITxLocatableObject).AbsoluteLocation;
                    TxTransformation inverse = absoluteLocation3.Inverse;
                    TxTransformation absoluteLocation4 = absoluteLocation * inverse * absoluteLocation2;
                    attachmentParent.AbsoluteLocation = absoluteLocation4;
                    result = true;
                }
            }
            return result;
        }

        internal static bool PreliminaryCollisionCheck()
        {
            return TxWeldAbility.IsPerliminaryCollisionDetected();
        }

        public static bool IsValidLocationInSimulation(ITxRoboticLocationOperation loc)
        {
            bool flag = false;
            if (loc != null)
            {
                if (loc is TxWeldLocationOperation)
                {
                    if ((loc as TxWeldLocationOperation).IsProjected)
                    {
                        flag = true;
                    }
                }
                else if (loc is TxRoboticViaLocationOperation || loc is TxGenericRoboticLocationOperation || loc is TxRoboticSeamLocationOperation)
                {
                    flag = true;
                }
                if (flag)
                {
                    flag = AJTApGenUtils.IsValidOperationInSimulation(loc);
                }
            }
            return flag;
        }

        internal static bool IsValidOperationInSimulation(ITxRoboticLocationOperation loc)
        {
            bool result = false;
            ITxRoboticOrderedCompoundOperation parentRoboticOperation = loc.ParentRoboticOperation;
            if (parentRoboticOperation != null && parentRoboticOperation.Robot != null)
            {
                result = true;
            }
            return result;
        }

        private static bool CheckLocationReachabilityCollisisionFree(TxRobot robot, TxTransformation loc, ITxTool gun, ITxLocationOperation locOp, TxInputReachabilityAndAccessibilityData reachAccessData, ITxRoboticControllerServices rcs, out TxJointLimitStatusTypeEx statuts)
        {
            bool result = false;
            statuts = 0;
            TxRobotInverseData txRobotInverseData = new TxRobotInverseData(loc, 0);
            TxJumpToLocationData txJumpToLocationData = new TxJumpToLocationData();
            txJumpToLocationData.UseExternalAxes = false;
            txJumpToLocationData.UseTaughtLocations = false;
            txJumpToLocationData.UseTaughtPose = false;
            TxPoseData txPoseData = null;
            bool flag = false;
            if (TxApplication.Options.Motion.IndicateJointWorkingLimitsCheck)
            {
                if (rcs != null)
                {
                    ITxLocatableObject txLocatableObject = locOp as ITxLocatableObject;
                    TxTransformation absoluteLocation = txLocatableObject.AbsoluteLocation;
                    txLocatableObject.AbsoluteLocation = loc;
                    ArrayList arrayList = rcs.CalculateInverseSolutions(locOp, txJumpToLocationData, 0);
                    txLocatableObject.AbsoluteLocation = absoluteLocation;
                    if (arrayList != null && arrayList.Count > 0)
                    {
                        flag = TxRobotEx.GetRobotFirstOptimalSolution(robot, arrayList, out txPoseData, out statuts);
                    }
                }
                else
                {
                    flag = TxRobotEx.CalcInverseFirstOptimalSolution(robot, txRobotInverseData, out txPoseData, out statuts);
                }
            }
            else
            {
                if (rcs != null)
                {
                    ITxLocatableObject txLocatableObject2 = locOp as ITxLocatableObject;
                    TxTransformation absoluteLocation2 = txLocatableObject2.AbsoluteLocation;
                    txLocatableObject2.AbsoluteLocation = loc;
                    rcs.CheckInverse(locOp, txJumpToLocationData, 0, out txPoseData);
                    txLocatableObject2.AbsoluteLocation = absoluteLocation2;
                }
                else
                {
                    txPoseData = TxRobotEx.CalcInverseFirstSolution(robot, txRobotInverseData);
                }
                if (txPoseData != null)
                {
                    if (TxApplication.Options.Motion.LimitsCheck)
                    {
                        statuts = (TxJointLimitStatusTypeEx)4;
                    }
                    else
                    {
                        statuts = TxJointLimitUtils.GetDeviceLimitStatus(robot, txPoseData);
                        if (statuts > (TxJointLimitStatusTypeEx)1)
                        {
                            statuts = (TxJointLimitStatusTypeEx)4;
                        }
                        else
                        {
                            statuts = 0;
                        }
                    }
                    flag = true;
                }
            }
            if (flag && txPoseData != null)
            {
                bool flag2 = AJTApGenUtils.CheckCollisionStatus(reachAccessData, txPoseData);
                result = !flag2;
            }
            return result;
        }

        private static ArrayList MergeFirstAndLastSegmentsIfPossible(ArrayList inSegments)
        {
            if (inSegments.Count > 1)
            {
                Segment segment = inSegments[0] as Segment;
                Segment segment2 = inSegments[inSegments.Count - 1] as Segment;
                if (segment != null && segment2 != null && segment.startAngle == 0.0 && TxDoubleExtension.AlmostEquals(segment2.endAngle, 6.2831853071795862, 1E-05) && segment.statuts == segment2.statuts)
                {
                    Segment value = new Segment(segment2.startAngle, segment.endAngle + 6.2831853071795862, segment2.statuts);
                    inSegments.RemoveAt(inSegments.Count - 1);
                    inSegments.RemoveAt(0);
                    inSegments.Add(value);
                }
            }
            return inSegments;
        }

        private static double GetMiddleAngleOfLargestSegment(ArrayList segments, out AJTApRmxUtils.EApRmxReachabilityStatus reachabilityStatus)
        {
            segments = AJTApGenUtils.MergeFirstAndLastSegmentsIfPossible(segments);
            double num = 0.0;
            double num2 = 0.0;
            AJTApRmxUtils.EApRmxReachabilityStatus eapRmxReachabilityStatus = AJTApRmxUtils.EApRmxReachabilityStatus.NoReach;
            foreach (object obj in segments)
            {
                Segment segment = (Segment)obj;
                if (segment != null && (segment.statuts > eapRmxReachabilityStatus || (segment.statuts == eapRmxReachabilityStatus && segment.endAngle - segment.startAngle >= num2 - num)))
                {
                    num = segment.startAngle;
                    num2 = segment.endAngle;
                    eapRmxReachabilityStatus = segment.statuts;
                }
            }
            reachabilityStatus = eapRmxReachabilityStatus;
            double result;
            if (Math.Abs(num2 - num - 0.087266462599716474) <= 0.0001)
            {
                result = num;
            }
            else
            {
                result = (num2 + num) / 2.0;
            }
            return result;
        }

        private static double GetAngleNearEdgeOfClosestSegment(ArrayList segments, out AJTApRmxUtils.EApRmxReachabilityStatus reachabilityStatus, out bool isStart)
        {
            segments = AJTApGenUtils.MergeFirstAndLastSegmentsIfPossible(segments);
            double num = 0.0;
            double num2 = 0.0;
            AJTApRmxUtils.EApRmxReachabilityStatus eapRmxReachabilityStatus = AJTApRmxUtils.EApRmxReachabilityStatus.NoReach;
            double num3 = double.MaxValue;
            foreach (object obj in segments)
            {
                Segment segment = (Segment)obj;
                if (segment != null)
                {
                    double num4 = Math.Min(Math.Min(segment.startAngle, 6.2831853071795862 - segment.startAngle), Math.Min(segment.endAngle, Math.Abs(6.2831853071795862 - segment.endAngle)));
                    if (segment.statuts > eapRmxReachabilityStatus || (segment.statuts == eapRmxReachabilityStatus && num4 < num3))
                    {
                        num3 = num4;
                        num = segment.startAngle;
                        num2 = segment.endAngle;
                        eapRmxReachabilityStatus = segment.statuts;
                    }
                }
            }
            reachabilityStatus = eapRmxReachabilityStatus;
            double result = 0.0;
            isStart = true;
            if (num2 < 6.2831853071795862)
            {
                if (Math.Abs(num2 - num - 0.087266462599716474) <= 0.0001)
                {
                    result = num;
                }
                else if (num < 6.2831853071795862 - num2)
                {
                    result = num + 0.043633231299858237;
                }
                else
                {
                    result = num2 - 0.043633231299858237;
                    isStart = false;
                }
            }
            return result;
        }

        private static bool CheckCollisionStatus(TxInputReachabilityAndAccessibilityData reachAccessData, TxPoseData robotPose)
        {
            bool result;
            if (robotPose != null)
            {
                result = TxWeldAbility.IsCollisionDetected(reachAccessData, robotPose);
            }
            else
            {
                result = TxWeldAbility.IsCollisionDetected(reachAccessData);
            }
            return result;
        }

        internal static void SetLocationsForMountedWorkpiece(ITxRobot robot, ITxTool tool, ITxLocationOperation robLocation, ref TxTransformation currLoc, ref TxTransformation robotLocChange)
        {
            if (robot != null && tool != null)
            {
                TxTransformation absoluteLocation = tool.TCPF.AbsoluteLocation;
                TxTransformation absoluteLocation2 = robot.TCPF.AbsoluteLocation;
                robotLocChange = TxTransformation.Multiply(currLoc.Inverse, absoluteLocation2);
                robot.TCPF.AbsoluteLocation = currLoc;
                currLoc = absoluteLocation;
            }
        }

        internal static void ResetLocationsForMountedWorkpiece(ITxRobot robot, ITxTool tool, ITxLocationOperation robLocation, ref TxTransformation currLoc, TxTransformation robotLocChange)
        {
            if (robot != null && tool != null)
            {
                TxTransformation absoluteLocation = robot.TCPF.AbsoluteLocation;
                currLoc = absoluteLocation;
                TxTransformation absoluteLocation2 = TxTransformation.Multiply(absoluteLocation, robotLocChange);
                robot.TCPF.AbsoluteLocation = absoluteLocation2;
            }
        }

        internal static bool IsMountedWorkpiece(ITxLocationOperation locOp)
        {
            bool result = false;
            ITxRoboticLocationOperation txRoboticLocationOperation = locOp as ITxRoboticLocationOperation;
            if (txRoboticLocationOperation != null)
            {
                ITxRoboticOperation parentRoboticOperation = txRoboticLocationOperation.ParentRoboticOperation;
                if (parentRoboticOperation != null)
                {
                    result = parentRoboticOperation.IsMountedWorkpieceOperation;
                }
            }
            return result;
        }

        public static ITxTool GetAssignedGun(ITxLocationOperation locOp)
        {
            ITxTool result = null;
            ITxRoboticLocationOperation txRoboticLocationOperation = locOp as ITxRoboticLocationOperation;
            if (txRoboticLocationOperation != null)
            {
                ITxRoboticOrderedCompoundOperation parentRoboticOperation = txRoboticLocationOperation.ParentRoboticOperation;
                if (parentRoboticOperation != null)
                {
                    if (parentRoboticOperation is TxBaseWeldOperation)
                    {
                        result = ((parentRoboticOperation as TxBaseWeldOperation).Gun as ITxTool);
                    }
                    else if (parentRoboticOperation is TxBaseContinuousOperation)
                    {
                        result = ((parentRoboticOperation as TxBaseContinuousOperation).Tool as ITxTool);
                    }
                    else if (parentRoboticOperation is TxGenericRoboticOperation)
                    {
                        result = ((parentRoboticOperation as TxGenericRoboticOperation).Tool as ITxTool);
                    }
                }
            }
            return result;
        }

        internal static bool IsAttachedToWorkpiece(ITxLocationOperation locOp)
        {
            bool result = false;
            ITxComponent txComponent = (locOp as ITxLocatableObject).AttachmentParent as ITxComponent;
            if (txComponent != null)
            {
                result = true;
            }
            return result;
        }

        internal static void SetRobotTCPF(AdjustLocationParams alParams)
        {
            AJTApGenUtils.m_originalRobotTCPF = null;
            if (!AJTApGenUtils.IsMountedWorkpiece(alParams.m_locOp) && alParams.m_robot != null && alParams.m_gun != null && alParams.m_robot.TCPF != null)
            {
                ITxTool gun = alParams.m_gun;
                if (gun.TCPF != null)
                {
                    AJTApGenUtils.m_originalRobotTCPF = alParams.m_robot.TCPF.AbsoluteLocation;
                    alParams.m_robot.TCPF.AbsoluteLocation = gun.TCPF.AbsoluteLocation;
                }
            }
        }

        internal static void ResetRobotTCPF(AdjustLocationParams alParams)
        {
            if (!AJTApGenUtils.IsMountedWorkpiece(alParams.m_locOp) && alParams.m_robot != null && alParams.m_robot.TCPF != null && AJTApGenUtils.m_originalRobotTCPF != null)
            {
                alParams.m_robot.TCPF.AbsoluteLocation = AJTApGenUtils.m_originalRobotTCPF;
            }
        }

        public static List<TxLocationAvailabilitySector> CalculateLocationPieChart(AdjustLocationParams alParams, double sampleSizeRad)
        {
            TxLocationAvailability txLocationAvailability = null;

            txLocationAvailability = AJTApGenUtils.CreateUtility3(alParams);


            List<TxLocationAvailabilitySector> result = txLocationAvailability.CalculatePieChart(sampleSizeRad);
            AJTApGenUtils.ReleaseUtility();
            return result;
        }

        public static TxLocationAvailabilityState CalculateCurrentLocationState(AdjustLocationParams alParams, bool iscalcCollision = false)
        {
            return AJTApGenUtils.CalculateLocationStateRotatedAt(alParams, 0.0, iscalcCollision);
        }

        public static TxLocationAvailabilityState CalculateLocationStateRotatedAt(AdjustLocationParams alParams, double angleRad, bool iscalcCollision = false)
        {
            TxLocationAvailability txLocationAvailability = null;

            txLocationAvailability = AJTApGenUtils.CreateUtility3(alParams, iscalcCollision);

            TxLocationAvailabilityState result;
            if (angleRad == 0.0)
            {
                result = txLocationAvailability.CalculateCurrentState();
            }
            else
            {
                result = txLocationAvailability.CalculateStateAt(angleRad);
            }
            AJTApGenUtils.ReleaseUtility();
            return result;
        }

        //public static TxLocationAvailability CreateUtility(AdjustLocationParams alParams)
        //{
        //    ArrayList arrayList;
        //    if (alParams.m_poseList == null || alParams.m_poseList.Count < 1)
        //    {
        //        arrayList = new ArrayList(1);
        //    }
        //    else
        //    {
        //        arrayList = new ArrayList(alParams.m_poseList.Count);
        //        for (int i = 0; i < alParams.m_poseList.Count; i++)
        //        {
        //            object obj = alParams.m_poseList[i];
        //            if (obj != null)
        //            {
        //                arrayList.Add(obj);
        //            }
        //        }
        //    }
        //    TxInputReachabilityAndAccessibilityData txInputReachabilityAndAccessibilityData = new TxInputReachabilityAndAccessibilityData(alParams.m_locOp as ITxRoboticLocationOperation, alParams.m_robot, alParams.m_gun, arrayList, alParams.m_context, false, true, alParams.m_gunScenario);

        //    try
        //    {
        //        if (alParams.m_toolFrame != null)
        //        {
        //            txInputReachabilityAndAccessibilityData.ToolFrame = alParams.m_toolFrame;
        //        }
        //    }
        //    catch
        //    {


        //    }


        //    TxRoboticControllerServicesDelegator txRoboticControllerServicesDelegator = null;
        //    AJTApGenUtils.m_utilityRobotServices = null;
        //    if (alParams.m_rcs != null)
        //    {
        //        AJTApGenUtils.m_utilityRobotServices = new AJTApRmxLocationRobotServices(alParams.m_rcs, alParams.m_locOp);
        //        txRoboticControllerServicesDelegator = AJTApGenUtils.m_utilityRobotServices.Callback;
        //    }
        //    AJTApGenUtils.m_utilityData = new TxLocationAvailabilityData(txInputReachabilityAndAccessibilityData, txRoboticControllerServicesDelegator);
        //    AJTApGenUtils.m_utility = new TxLocationAvailability(AJTApGenUtils.m_utilityData);
        //    return AJTApGenUtils.m_utility;
        //}

        /// <summary>
        /// 不计算碰撞
        /// </summary>
        /// <param name="alParams"></param>
        /// <returns></returns>
        //public static TxLocationAvailability CreateUtility2(AdjustLocationParams alParams)
        //{
        //    ArrayList arrayList;
        //    if (alParams.m_poseList == null || alParams.m_poseList.Count < 1)
        //    {
        //        arrayList = new ArrayList(1);
        //    }
        //    else
        //    {
        //        arrayList = new ArrayList(alParams.m_poseList.Count);
        //        for (int i = 0; i < alParams.m_poseList.Count; i++)
        //        {
        //            object obj = alParams.m_poseList[i];
        //            if (obj != null)
        //            {
        //                arrayList.Add(obj);
        //            }
        //        }
        //    }
        //    TxInputReachabilityAndAccessibilityData txInputReachabilityAndAccessibilityData = new TxInputReachabilityAndAccessibilityData(alParams.m_locOp as ITxRoboticLocationOperation, alParams.m_robot, alParams.m_gun, arrayList, alParams.m_context, false, true, alParams.m_gunScenario);
        //    txInputReachabilityAndAccessibilityData.CalcCollision = false;

        //    if (alParams.m_toolFrame != null)
        //    {
        //        txInputReachabilityAndAccessibilityData.ToolFrame = alParams.m_toolFrame;
        //    }
        //    TxRoboticControllerServicesDelegator txRoboticControllerServicesDelegator = null;
        //    AJTApGenUtils.m_utilityRobotServices = null;
        //    if (alParams.m_rcs != null)
        //    {
        //        AJTApGenUtils.m_utilityRobotServices = new AJTApRmxLocationRobotServices(alParams.m_rcs, alParams.m_locOp);
        //        txRoboticControllerServicesDelegator = AJTApGenUtils.m_utilityRobotServices.Callback;
        //    }
        //    AJTApGenUtils.m_utilityData = new TxLocationAvailabilityData(txInputReachabilityAndAccessibilityData, txRoboticControllerServicesDelegator);
        //    AJTApGenUtils.m_utility = new TxLocationAvailability(AJTApGenUtils.m_utilityData);
        //    return AJTApGenUtils.m_utility;
        //}

        /// <summary>
        /// 14.1
        /// </summary>
        /// <param name="alParams"></param>
        /// <returns></returns>
        public static TxLocationAvailability CreateUtility3(AdjustLocationParams alParams, bool iscalcCollision = false)
        {
            ArrayList arrayList;
            if (alParams.m_poseList == null || alParams.m_poseList.Count < 1)
            {
                arrayList = new ArrayList(1);
            }
            else
            {
                arrayList = new ArrayList(alParams.m_poseList.Count);
                for (int i = 0; i < alParams.m_poseList.Count; i++)
                {
                    object obj = alParams.m_poseList[i];
                    if (obj != null)
                    {
                        arrayList.Add(obj);
                    }
                }
            }
            TxInputReachabilityAndAccessibilityData txInputReachabilityAndAccessibilityData = new TxInputReachabilityAndAccessibilityData(alParams.m_locOp as ITxRoboticLocationOperation, alParams.m_robot, alParams.m_gun, arrayList, alParams.m_context, false, true, alParams.m_gunScenario);
            txInputReachabilityAndAccessibilityData.CalcCollision = iscalcCollision;

            TxRoboticControllerServicesDelegator txRoboticControllerServicesDelegator = null;
            AJTApGenUtils.m_utilityRobotServices = null;
            if (alParams.m_rcs != null)
            {
                AJTApGenUtils.m_utilityRobotServices = new AJTApRmxLocationRobotServices(alParams.m_rcs, alParams.m_locOp);
                txRoboticControllerServicesDelegator = AJTApGenUtils.m_utilityRobotServices.Callback;
            }
            AJTApGenUtils.m_utilityData = new TxLocationAvailabilityData(txInputReachabilityAndAccessibilityData, txRoboticControllerServicesDelegator);
            AJTApGenUtils.m_utility = new TxLocationAvailability(AJTApGenUtils.m_utilityData);
            return AJTApGenUtils.m_utility;
        }

        public static void ReleaseUtility()
        {
            AJTApGenUtils.m_utility = null;
            AJTApGenUtils.m_utilityData = null;
            AJTApGenUtils.m_utilityRobotServices = null;
        }

        public static AJTApRmxUtils.EApRmxReachabilityStatus AAA_CalculateCurrentLocationStatus(AdjustLocationParams alParams, bool iscalcCollision = false)
        {
            TxLocationAvailabilityState txLocationAvailabilityState = AJTApGenUtils.CalculateCurrentLocationState(alParams, iscalcCollision);
            if (txLocationAvailabilityState != null)
            {
                if (iscalcCollision)
                {
                    return AJTApGenUtils.AAA_ConvertUtilityStateToReachabilityStatus_3(txLocationAvailabilityState);
                }
                return AJTApGenUtils.AAA_ConvertUtilityStateToReachabilityStatus2(txLocationAvailabilityState);
            }
            return AJTApRmxUtils.EApRmxReachabilityStatus.NoReach;
        }

        public static AJTApRmxUtils.EApRmxReachabilityStatus AAA_CalculateLocationStatusAt(AdjustLocationParams alParams, double angleRad)
        {
            TxLocationAvailabilityState txLocationAvailabilityState = AJTApGenUtils.CalculateLocationStateRotatedAt(alParams, angleRad);
            if (txLocationAvailabilityState != null)
            {
                return AJTApGenUtils.AAA_ConvertUtilityStateToReachabilityStatus(txLocationAvailabilityState);
            }
            return AJTApRmxUtils.EApRmxReachabilityStatus.NoReach;
        }

        public static bool AAA_IsSuccessfulStatus(AJTApRmxUtils.EApRmxReachabilityStatus status)
        {
            bool result = false;
            if (status == AJTApRmxUtils.EApRmxReachabilityStatus.FullOutsidePhisicalLimits ||
                status == AJTApRmxUtils.EApRmxReachabilityStatus.FullOutsideWorkingLimits ||
                status == AJTApRmxUtils.EApRmxReachabilityStatus.Full)
            {
                result = true;
            }
            return result;
        }

        private static bool AAA_DoesExistSuccessfulStatusInSegments(ArrayList segments)
        {
            foreach (object obj in segments)
            {
                Segment segment = (Segment)obj;
                if (AJTApGenUtils.AAA_IsSuccessfulStatus(segment.statuts))
                {
                    return true;
                }
            }
            return false;
        }

        private static ArrayList AAA_ConvertUtilitySectorsToSegments(List<TxLocationAvailabilitySector> sectors)
        {
            ArrayList arrayList = new ArrayList(sectors.Count);
            foreach (TxLocationAvailabilitySector sector in sectors)
            {
                Segment value = AJTApGenUtils.AAA_ConvertUtilitySectorToSegment(sector);
                arrayList.Add(value);
            }
            return arrayList;
        }

        private static Segment AAA_ConvertUtilitySectorToSegment(TxLocationAvailabilitySector sector)
        {
            double startAngle = sector.StartAngle;
            double endAngle = sector.EndAngle;
            AJTApRmxUtils.EApRmxReachabilityStatus reachabilityStatuts = AJTApGenUtils.AAA_ConvertUtilityStateToReachabilityStatus(sector.State);
            return new Segment(startAngle, endAngle, reachabilityStatuts);
        }

        private static AJTApRmxUtils.EApRmxReachabilityStatus AAA_ConvertUtilityStateToReachabilityStatus(TxLocationAvailabilityState state)
        {
            AJTApRmxUtils.EApRmxReachabilityStatus result = AJTApRmxUtils.EApRmxReachabilityStatus.UnChecked;
            switch ((int)state.ReachabilityState)
            {
                case 0:
                    switch ((int)state.CollisionState)
                    {
                        case 1:
                            result = AJTApRmxUtils.EApRmxReachabilityStatus.Full;
                            break;
                        case 2:
                            result = AJTApRmxUtils.EApRmxReachabilityStatus.NoReach;
                            break;
                    }
                    break;
                case 1:
                    result = AJTApRmxUtils.EApRmxReachabilityStatus.NoReach;
                    break;
                case 2:
                    switch ((int)state.CollisionState)
                    {
                        case 0:
                            result = AJTApRmxUtils.EApRmxReachabilityStatus.Full;
                            break;
                        case 1:
                            result = AJTApRmxUtils.EApRmxReachabilityStatus.Full;
                            break;
                        case 2:
                            result = AJTApRmxUtils.EApRmxReachabilityStatus.NoReach;
                            break;
                    }
                    break;
                case 3:
                    result = AJTApRmxUtils.EApRmxReachabilityStatus.NoReach;
                    break;
                case 4:
                    switch ((int)state.CollisionState)
                    {
                        case 1:
                            result = AJTApRmxUtils.EApRmxReachabilityStatus.FullOutsideWorkingLimits;
                            break;
                        case 2:
                            result = AJTApRmxUtils.EApRmxReachabilityStatus.NoReach;
                            break;
                    }
                    break;
                case 5:
                    switch ((int)state.CollisionState)
                    {
                        case 1:
                            result = AJTApRmxUtils.EApRmxReachabilityStatus.FullOutsidePhisicalLimits;
                            break;
                        case 2:
                            result = AJTApRmxUtils.EApRmxReachabilityStatus.NoReach;
                            break;
                    }
                    break;
            }
            return result;
        }

        /// <summary>
        /// 不计算碰撞
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private static AJTApRmxUtils.EApRmxReachabilityStatus AAA_ConvertUtilityStateToReachabilityStatus2(TxLocationAvailabilityState state)
        {
            AJTApRmxUtils.EApRmxReachabilityStatus result = AJTApRmxUtils.EApRmxReachabilityStatus.UnChecked;
            switch ((int)state.ReachabilityState)
            {
                case 0:
                    switch ((int)state.CollisionState)
                    {
                        case 1:
                            result = AJTApRmxUtils.EApRmxReachabilityStatus.Full;
                            break;
                        case 2:
                            result = AJTApRmxUtils.EApRmxReachabilityStatus.NoReach;
                            break;
                    }
                    break;
                case 1:
                    result = AJTApRmxUtils.EApRmxReachabilityStatus.NoReach;
                    break;
                case 2:
                    switch ((int)state.CollisionState)
                    {
                        case 0:
                            result = AJTApRmxUtils.EApRmxReachabilityStatus.Full;
                            break;
                        case 1:
                            result = AJTApRmxUtils.EApRmxReachabilityStatus.Full;
                            break;
                        case 2:
                            result = AJTApRmxUtils.EApRmxReachabilityStatus.NoReach;
                            break;
                    }
                    break;
                case 3:
                    result = AJTApRmxUtils.EApRmxReachabilityStatus.NoReach;
                    break;
                case 4:
                    switch ((int)state.CollisionState)
                    {
                        case 1:
                            result = AJTApRmxUtils.EApRmxReachabilityStatus.FullOutsideWorkingLimits;
                            break;
                        case 2:
                            result = AJTApRmxUtils.EApRmxReachabilityStatus.NoReach;
                            break;
                    }
                    break;
                case 5:
                    switch ((int)state.CollisionState)
                    {
                        case 1:
                            result = AJTApRmxUtils.EApRmxReachabilityStatus.FullOutsidePhisicalLimits;
                            break;
                        case 2:
                            result = AJTApRmxUtils.EApRmxReachabilityStatus.NoReach;
                            break;
                    }
                    break;
            }
            return result;
        }

        private static AJTApRmxUtils.EApRmxReachabilityStatus AAA_ConvertUtilityStateToReachabilityStatus_3(TxLocationAvailabilityState state)
        {
            AJTApRmxUtils.EApRmxReachabilityStatus eapRmxReachabilityStatus = AJTApRmxUtils.EApRmxReachabilityStatus.UnChecked;
            switch ((int)state.ReachabilityState)
            {
                case 0:
                    switch ((int)state.CollisionState)
                    {
                        case 1:
                            eapRmxReachabilityStatus = AJTApRmxUtils.EApRmxReachabilityStatus.Full;
                            break;
                        case 2:
                            eapRmxReachabilityStatus = AJTApRmxUtils.EApRmxReachabilityStatus.NoReach;
                            break;
                    }
                    break;
                case 1:
                    eapRmxReachabilityStatus = AJTApRmxUtils.EApRmxReachabilityStatus.NoReach;
                    break;
                case 2:
                    switch ((int)state.CollisionState)
                    {
                        case 1:
                            eapRmxReachabilityStatus = AJTApRmxUtils.EApRmxReachabilityStatus.Full;
                            break;
                        case 2:
                            eapRmxReachabilityStatus = AJTApRmxUtils.EApRmxReachabilityStatus.NoReach;
                            break;
                    }
                    break;
                case 3:
                    eapRmxReachabilityStatus = AJTApRmxUtils.EApRmxReachabilityStatus.NoReach;
                    break;
                case 4:
                    switch ((int)state.CollisionState)
                    {
                        case 1:
                            eapRmxReachabilityStatus = AJTApRmxUtils.EApRmxReachabilityStatus.FullOutsideWorkingLimits;
                            break;
                        case 2:
                            eapRmxReachabilityStatus = AJTApRmxUtils.EApRmxReachabilityStatus.NoReach;
                            break;
                    }
                    break;
                case 5:
                    switch ((int)state.CollisionState)
                    {
                        case 1:
                            eapRmxReachabilityStatus = AJTApRmxUtils.EApRmxReachabilityStatus.FullOutsidePhisicalLimits;
                            break;
                        case 2:
                            eapRmxReachabilityStatus = AJTApRmxUtils.EApRmxReachabilityStatus.NoReach;
                            break;
                    }
                    break;
            }
            return eapRmxReachabilityStatus;
        }

        public static void AAA_Store_AutoFlipIfNeededOption(System.Windows.Forms.Form form, bool autoFlip)
        {
            string text = autoFlip ? "1" : "0";
            try
            {
                TxFormSettings.StoreFormValue(form, "AutomaticFlipIfNeeded", text);
            }
            catch
            {
            }
        }

        public static void AAA_Restore_AutoFlipIfNeededOption(System.Windows.Forms.Form form, out bool autoFlip)
        {
            autoFlip = false;
            try
            {
                object obj = null;
                TxFormSettings.RestoreFormValue(form, "AutomaticFlipIfNeeded", out obj);
                if (obj != null)
                {
                    string a = obj.ToString();
                    if (a == "1")
                    {
                        autoFlip = true;
                    }
                    else
                    {
                        autoFlip = false;
                    }
                }
            }
            catch
            {
            }
        }

        public static void AAA_Store_FollowModeIfNeededOption(System.Windows.Forms.Form form, bool followMode)
        {
            string text = followMode ? "1" : "0";
            try
            {
                TxFormSettings.StoreFormValue(form, "FollowModeIfNeeded", text);
            }
            catch
            {
            }
        }

        public static void AAA_Restore_FollowModeIfNeededOption(System.Windows.Forms.Form form, out bool followMode)
        {
            followMode = false;
            try
            {
                object obj = null;
                TxFormSettings.RestoreFormValue(form, "FollowModeIfNeeded", out obj);
                if (obj != null)
                {
                    string a = obj.ToString();
                    if (a == "1")
                    {
                        followMode = true;
                    }
                    else
                    {
                        followMode = false;
                    }
                }
            }
            catch
            {
            }
        }

        public const double SAMPLE_STEP_SIZE = 0.087266462599716474;

        private const double EPSILON = 0.0001;

        public const double SEGMENT_EDGE_MARGIN = 0.043633231299858237;

        private const string PROPERTY_AUTOMATIC_FLIP_IF_NEEDED = "AutomaticFlipIfNeeded";

        private const string PROPERTY_FOLLOW_MODE_IF_NEEDED = "FollowModeIfNeeded";

        private static TxTransformation m_originalRobotTCPF = null;

        private static List<TxLocationAvailabilitySector> m_currentCommonSectors = null;

        private static int m_lastCommonLocationIndex = 0;

        private static int m_currentLocationIndex = -1;

        private static bool m_commonSectorExist = false;

        private static TxLocationAvailability m_utility = null;

        private static TxLocationAvailabilityData m_utilityData = null;

        private static AJTApRmxLocationRobotServices m_utilityRobotServices = null;

        public static readonly TxPerformanceModeEx STATIC_APP_CALCULATION_MODE = new TxPerformanceModeEx(TxPerformanceModeType.StaticApplicationCalculation);
    }
}
