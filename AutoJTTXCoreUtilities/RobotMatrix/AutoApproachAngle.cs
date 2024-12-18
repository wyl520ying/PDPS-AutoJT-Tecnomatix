using EngineeringInternalExtension.DataTypes;
using EngineeringInternalExtension.Options;
using System;
using System.Collections;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Olp;
using TxEuOlpUtil;
using static Tecnomatix.Engineering.TxRobotInverseData;

namespace AutoJTTXCoreUtilities.RobotMatrix
{
    public class AutoApproachAngle
    {
        //操作得机器人
        public TxRobot m_Robot { get; set; }
        //操作得via
        //private TxRoboticViaLocationOperation m_testLoc { get; set; }

        //是否检查碰撞
        public bool Iscollision { get; set; }

        //是否flip计算
        public bool IsFlip { get; set; }

        //是否旋转测试
        public bool IsRotateTest { get; set; }

        #region 用于创建para

        ITxTool m_txTool = null;
        ArrayList m_arrayList = null;
        TxInputReachabilityAndAccessibilityDataGunScenarioType m_txInputReachabilityAndAccessibilityDataGunScenarioType => TxInputReachabilityAndAccessibilityDataGunScenarioType.ByLocationAssignment;
        TxAutomaticApproachAngleAlgorithmTypeEx m_algorithmTypeEx => TxOptionsEx.AutomaticApproachAngleAlgorithm;
        //rcs
        ITxRoboticControllerServices m_txRoboticControllerServices = null;

        #endregion

        public AutoApproachAngle()
        {
            //初始化干涉查询
            TxCollisionRoot collisionRoot = TxApplication.ActiveDocument.CollisionRoot;
            this.m_context = collisionRoot.CreateQueryContext();
            this.m_queryGlobalContext = collisionRoot.GlobalQueryContext;
        }

        //用于创建para
        public void SetParamMethod()
        {
            //get tool
            //all mounted tools
            var tools = m_Robot.MountedTools;
            if (tools != null && tools.Count > 0)
            {
                foreach (var item in tools)
                {
                    if (item is ITxDisplayableObject displayableObject)
                    {
                        if (displayableObject.Visibility != TxDisplayableObjectVisibility.CannotBeDisplayed &&
                            displayableObject.Visibility != TxDisplayableObjectVisibility.None && item is ITxTool txTool)
                        {
                            m_txTool = txTool;
                            break;
                        }
                    }
                }
            }


            //get open post
            TxPose txPose = GetSelectedPose(m_txTool);
            m_arrayList = new ArrayList
            {
                txPose
            };

            //rcs
            m_txRoboticControllerServices = GetRoboticControllerServices(m_Robot);
        }

        #region 干涉查询

        //干涉查询
        TxCollisionQueryContext m_context;
        private TxCollisionQueryContext m_queryGlobalContext;

        //删除context
        public void DeleteContext()
        {
            if (m_context != null)
            {
                TxCollisionRoot collisionRoot = TxApplication.ActiveDocument.CollisionRoot;
                collisionRoot.DeleteQueryContext(this.m_context);
            }

            if (this.m_queryGlobalContext != null)
            {
                this.m_queryGlobalContext = null;
            }
        }

        #endregion

        #region 判断 TxTransformation 可达

        //旋转得次数
        int Ccount;
        //Checks whether the robot's tool frame can reach a location.  
        public (bool result, TxTransformation trans) DoesInverseExist(TxTransformation txTransformation)
        {
            //第一次判断可达
            var (bl230, txtransfor) = CheckInverseMethod(txTransformation);

            //如果不可达开始递归
            if (!bl230)
            {
                Ccount = 2;

                //旋转a°判断可达
                (bl230, txtransfor) = RotateCheckReached(txTransformation);
                if (!bl230)
                {
                    //还是不可达,flip
                    //x旋转180°
                    TxTransformation txTransformation2 = COlpUtilMath.RotateX(txTransformation, 3.14159265358979);
                    //flip后第一次判断可达
                    (bl230, txtransfor) = CheckInverseMethod(txTransformation2);
                    //如果不可达开始递归
                    if (!bl230)
                    {
                        Ccount = 2;

                        //旋转a°判断可达
                        return RotateCheckReached(txTransformation);
                    }
                    else
                    {
                        return (true, txtransfor);
                    }
                }
                else
                {
                    return (true, txtransfor);
                }
            }

            return (bl230, txtransfor);
        }

        //旋转判断可达
        (bool result, TxTransformation trans) RotateCheckReached(TxTransformation txTransformation)
        {
            (bool result, TxTransformation trans) res;

            Ccount = 45;
            res = RotateCheckInverseMethod(txTransformation, 45);
            if (!res.result)
            {
                Ccount = 20;
                res = RotateCheckInverseMethod(txTransformation, 20);
                if (!res.result)
                {
                    Ccount = 15;
                    res = RotateCheckInverseMethod(txTransformation, 15);
                    if (!res.result)
                    {
                        Ccount = 10;
                        res = RotateCheckInverseMethod(txTransformation, 10);
                        if (!res.result)
                        {
                            Ccount = 5;
                            res = RotateCheckInverseMethod(txTransformation, 5);
                            if (!res.result)
                            {
                                Ccount = 2;
                                res = RotateCheckInverseMethod(txTransformation, 2);
                            }
                        }
                    }
                }
            }

            return res;
        }

        //旋转a°判断可达
        (bool result, TxTransformation trans) RotateCheckInverseMethod(TxTransformation txTransformation, int a)
        {
            //旋转a°
            TxTransformation txTransformation2 = COlpUtilMath.RotateZ(txTransformation, 0.0174532925199432957692 * a);//0.0174532925199432957692
            Ccount += a;

            var (blsdf, transefef) = CheckInverseMethod(txTransformation);
            //判断可达
            if (!blsdf)
            {
                if (Ccount >= 360)
                {
                    return (false, null);
                }
                //继续递归
                return RotateCheckInverseMethod(txTransformation2, a);
            }
            else
            {
                return (true, transefef);
            }
        }

        //判断可达
        (bool result, TxTransformation trans) CheckInverseMethod(TxTransformation txTransformation)
        {
            bool result = m_Robot.DoesInverseExist(new TxRobotInverseData(txTransformation, TxRobotInverseData.TxInverseType.InverseFullReach));

            //可达
            if (result && this.Iscollision)
            {
                //测试碰撞
                result = CalcCollisionMethod(txTransformation);
            }

            return (result, txTransformation);
        }












        //检查干涉
        bool CalcCollisionMethod(TxTransformation txTransformation)
        {
            //locate
            //if (m_testLoc != null)
            //{
            //    m_testLoc.Locate(txTransformation);
            //}

            SetTCPFPosition(txTransformation, m_Robot);

            return HasCollision();
            /*
            //para
            AdjustLocationParams adjustLocationParams = new AdjustLocationParams(m_Robot, m_txTool, m_arrayList, m_testLoc, this.m_context, m_txInputReachabilityAndAccessibilityDataGunScenarioType, m_txRoboticControllerServices, m_algorithmTypeEx);

            //开始查询
            return AJTApGenUtils.CalcLocationReachabilityStatus(adjustLocationParams, out AJTApRmxUtils.EApRmxReachabilityStatus eapRmxReachabilityStatus, true);*/
        }
        /*
        //检查干涉2
        bool CalcCollisionMethod2(TxTransformation txTransformation)
        {
            //locate
            if (m_testLoc != null)
            {
                m_testLoc.Locate(txTransformation);
            }

            m_txRoboticControllerServices.JumpToLocation(m_testLoc,new TxJumpToLocationData {
                ForceConfiguration = false,
                UseWorkToolAndSystemFrames = false,
                UseTaughtLocations = false,
                UseTaughtPose = false,
                GenerateMessage = false
            });

            TxCollisionRoot collisionRoot = TxApplication.ActiveDocument.CollisionRoot;
            collisionRoot.HasCollidingObjectsFromLists(,,this.m_context);

            return false;
        }*/
        //设置bot的pose
        static bool JumpToPoseMethod(string poseName, ITxObject collection)
        {
            bool result = false;

            try
            {
                TxRobot robot = (TxRobot)collection;

                //获取指定的pose
                TxPose pose = null;
                foreach (var item in robot.PoseList)
                {
                    if (item.Name.ToUpper().Equals(poseName.ToUpper()))
                    {
                        pose = (TxPose)item;
                        break;
                    }
                }

                //jumptopose
                robot.CurrentPose = pose.PoseData;

                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public static bool SetTCPFPosition(TxTransformation newTCPFLocation, TxRobot txRobot)
        {
            TxRobotInverseData val = new TxRobotInverseData(newTCPFLocation, TxInverseType.InverseFullReach);
            bool bl1 = SetPose(txRobot.CalcInverseSolutions(val), txRobot);
            return bl1;
        }
        private static bool SetPose(ArrayList inverseSolutions, TxRobot txRobot)
        {
            if (inverseSolutions == null || inverseSolutions.Count == 0)
            {
                return false;
            }

            var val = txRobot;
            object obj = inverseSolutions[0];
            val.CurrentPose = (TxPoseData)((obj is TxPoseData) ? obj : null);
            return true;
        }



















        //计算可达, 返回三种状态
        public (int result, TxTransformation trans) DoesInverseExist2(TxTransformation txTransformation)//, TxRobot bot
        {
            (int, TxTransformation) result = default;

            try
            {
                //初始状态
                isReach = 0;
                //第一次判断可达
                var c1 = CheckInverseMethod2(txTransformation);
                result.Item1 = c1.result;
                result.Item2 = c1.trans;

                //如果不可达开始递归
                if (result.Item1 != 2 && this.IsRotateTest)
                {
                    Ccount = 2;

                    //旋转2°判断可达
                    var c2 = RotateCheckInverseMethod22(txTransformation);
                    result.Item1 = c2.result;
                    result.Item2 = c2.trans;

                    if (result.Item1 != 2 && this.IsFlip)
                    {
                        //还是不可达,flip
                        //x旋转180°
                        TxTransformation txTransformation2 = COlpUtilMath.RotateX(txTransformation, 3.14159265358979);
                        //flip后第一次判断可达
                        var c3 = CheckInverseMethod2(txTransformation2);
                        result.Item1 = c3.result;
                        result.Item2 = c3.trans;

                        //如果不可达开始递归
                        if (result.Item1 != 2)
                        {
                            Ccount = 2;

                            //判断可达
                            return RotateCheckInverseMethod22(txTransformation);
                        }
                        else
                        {
                            return result;
                        }
                    }
                    else
                    {
                        return result;
                    }
                }
            }
            finally
            {
                //JumpToPoseMethod("home",bot);
            }

            return result;
        }
        public (int result, TxTransformation trans) DoesInverseExist55(TxTransformation txTransformation)//, TxRobot bot
        {
            (int, TxTransformation) result = default;

            try
            {
                //初始状态
                isReach = 0;
                //第一次判断可达
                var c1 = CheckInverseMethod2(txTransformation);
                result.Item1 = c1.result;
                result.Item2 = c1.trans;

                //如果不可达开始递归
                if (result.Item1 != 2 && this.IsRotateTest)
                {
                    Ccount = 5;

                    //旋转2°判断可达
                    var c2 = RotateCheckInverseMethod55(txTransformation);
                    result.Item1 = c2.result;
                    result.Item2 = c2.trans;

                    if (result.Item1 != 2 && this.IsFlip)
                    {
                        //还是不可达,flip
                        //x旋转180°
                        TxTransformation txTransformation2 = COlpUtilMath.RotateX(txTransformation, 3.14159265358979);
                        //flip后第一次判断可达
                        var c3 = CheckInverseMethod2(txTransformation2);
                        result.Item1 = c3.result;
                        result.Item2 = c3.trans;

                        //如果不可达开始递归
                        if (result.Item1 != 2)
                        {
                            Ccount = 5;

                            //判断可达
                            return RotateCheckInverseMethod55(txTransformation);
                        }
                        else
                        {
                            return result;
                        }
                    }
                    else
                    {
                        return result;
                    }
                }
            }
            finally
            {
                //JumpToPoseMethod("home",bot);
            }

            return result;
        }
        public (int result, TxTransformation trans) DoesInverseExist1010(TxTransformation txTransformation)//, TxRobot bot
        {
            (int, TxTransformation) result = default;

            try
            {
                //初始状态
                isReach = 0;
                //第一次判断可达
                var c1 = CheckInverseMethod2(txTransformation);
                result.Item1 = c1.result;
                result.Item2 = c1.trans;

                //如果不可达开始递归
                if (result.Item1 != 2 && this.IsRotateTest)
                {
                    Ccount = 10;

                    //旋转2°判断可达
                    var c2 = RotateCheckInverseMethod1010(txTransformation);
                    result.Item1 = c2.result;
                    result.Item2 = c2.trans;

                    if (result.Item1 != 2 && this.IsFlip)
                    {
                        //还是不可达,flip
                        //x旋转180°
                        TxTransformation txTransformation2 = COlpUtilMath.RotateX(txTransformation, 3.14159265358979);
                        //flip后第一次判断可达
                        var c3 = CheckInverseMethod2(txTransformation2);
                        result.Item1 = c3.result;
                        result.Item2 = c3.trans;

                        //如果不可达开始递归
                        if (result.Item1 != 2)
                        {
                            Ccount = 10;

                            //判断可达
                            return RotateCheckInverseMethod1010(txTransformation);
                        }
                        else
                        {
                            return result;
                        }
                    }
                    else
                    {
                        return result;
                    }
                }
            }
            finally
            {
                //JumpToPoseMethod("home",bot);
            }

            return result;
        }
        public (int result, TxTransformation trans) DoesInverseExist1515(TxTransformation txTransformation)//, TxRobot bot
        {
            (int, TxTransformation) result = default;

            try
            {
                //初始状态
                isReach = 0;
                //第一次判断可达
                var c1 = CheckInverseMethod2(txTransformation);
                result.Item1 = c1.result;
                result.Item2 = c1.trans;

                //如果不可达开始递归
                if (result.Item1 != 2 && this.IsRotateTest)
                {
                    Ccount = 15;

                    //旋转15°判断可达
                    var c2 = RotateCheckInverseMethod1515(txTransformation);
                    result.Item1 = c2.result;
                    result.Item2 = c2.trans;

                    if (result.Item1 != 2 && this.IsFlip)
                    {
                        //还是不可达,flip
                        //x旋转180°
                        TxTransformation txTransformation2 = COlpUtilMath.RotateX(txTransformation, 3.14159265358979);
                        //flip后第一次判断可达
                        var c3 = CheckInverseMethod2(txTransformation2);
                        result.Item1 = c3.result;
                        result.Item2 = c3.trans;

                        //如果不可达开始递归
                        if (result.Item1 != 2)
                        {
                            Ccount = 15;

                            //判断可达
                            return RotateCheckInverseMethod1515(txTransformation);
                        }
                        else
                        {
                            return result;
                        }
                    }
                    else
                    {
                        return result;
                    }
                }
            }
            finally
            {
                //JumpToPoseMethod("home",bot);
            }

            return result;
        }





        //旋转2°判断可达
        (int result, TxTransformation trans) RotateCheckInverseMethod22(TxTransformation txTransformation)
        {
            //旋转2°
            TxTransformation txTransformation2 = COlpUtilMath.RotateZ(txTransformation, 0.0349065850398864);//0.017453292519943295
            Ccount += 2;

            var blsdf = CheckInverseMethod2(txTransformation);
            //判断可达
            if (blsdf.result != 2)
            {
                if (Ccount >= 360)
                {
                    return blsdf;
                }
                //继续递归
                return RotateCheckInverseMethod22(txTransformation2);
            }
            else
            {
                return blsdf;
            }
        }

        //旋转5°判断可达
        (int result, TxTransformation trans) RotateCheckInverseMethod55(TxTransformation txTransformation)
        {
            //旋转5°
            TxTransformation txTransformation2 = COlpUtilMath.RotateZ(txTransformation, 0.0872664625997164);//0.017453292519943295
            Ccount += 5;

            var blsdf = CheckInverseMethod2(txTransformation);
            //判断可达
            if (blsdf.result != 2)
            {
                if (Ccount >= 360)
                {
                    return blsdf;
                }
                //继续递归
                return RotateCheckInverseMethod55(txTransformation2);
            }
            else
            {
                return blsdf;
            }
        }
        //旋转10°判断可达
        (int result, TxTransformation trans) RotateCheckInverseMethod1010(TxTransformation txTransformation)
        {
            //旋转10°
            TxTransformation txTransformation2 = COlpUtilMath.RotateZ(txTransformation, 0.17453292519943);//0.017453292519943295
            Ccount += 10;

            var blsdf = CheckInverseMethod2(txTransformation);
            //判断可达
            if (blsdf.result != 2)
            {
                if (Ccount >= 360)
                {
                    return blsdf;
                }
                //继续递归
                return RotateCheckInverseMethod1010(txTransformation2);
            }
            else
            {
                return blsdf;
            }
        }
        //旋转15°判断可达
        (int result, TxTransformation trans) RotateCheckInverseMethod1515(TxTransformation txTransformation)
        {
            //旋转10°
            TxTransformation txTransformation2 = COlpUtilMath.RotateZ(txTransformation, 0.2617993877991);//0.017453292519943295
            Ccount += 15;

            var blsdf = CheckInverseMethod2(txTransformation);
            //判断可达
            if (blsdf.result != 2)
            {
                if (Ccount >= 360)
                {
                    return blsdf;
                }
                //继续递归
                return RotateCheckInverseMethod1515(txTransformation2);
            }
            else
            {
                return blsdf;
            }
        }


        int isReach = 0;
        //判断可达 0 不可达, 1 可达干涉, 2 可达不干涉
        (int result, TxTransformation trans) CheckInverseMethod2(TxTransformation txTransformation)
        {
            //逆解
            bool exit = m_Robot.DoesInverseExist(new TxRobotInverseData(txTransformation, TxRobotInverseData.TxInverseType.InverseFullReach));

            //可达
            if (exit)
            {
                //1 可达干涉
                isReach = 1;

                if (this.Iscollision)
                {
                    //测试碰撞
                    bool iscollision = CalcCollisionMethod(txTransformation);
                    if (iscollision)
                    {
                        //2 可达不干涉
                        isReach = 2;
                    }
                }
                else
                {
                    isReach = 2;
                }
            }

            return (isReach, txTransformation);
        }












        #region 检查干涉

        private bool HasCollision()
        {
            TxCollisionQueryResults txCollisionQueryResults;
            txCollisionQueryResults = GetAllCollisionsQueryResults();
            return !(txCollisionQueryResults.States.Count != 0 && this.HasCollidingObjects(txCollisionQueryResults));
        }
        private bool HasCollidingObjects(TxCollisionQueryResults listCollision)
        {
            foreach (object obj in listCollision.States)
            {
                TxCollisionState txCollisionState = (TxCollisionState)obj;
                if (txCollisionState.Type == TxCollisionState.TxCollisionStateType.Collision)
                {
                    return true;
                }
            }
            return false;
        }

        //计算干涉
        TxCollisionQueryResults GetAllCollisionsQueryResults()
        {
            TxCollisionQueryResults txCollisionQueryResults = null;

            txCollisionQueryResults = this.GetCollisionAndNearMissResults(true);

            return txCollisionQueryResults;
        }
        private TxCollisionQueryResults GetCollisionAndNearMissResults(bool all)
        {
            TxCollisionQueryResults txCollisionQueryResults = null;
            if (this.m_queryGlobalContext != null)
            {
                try
                {
                    TxCollisionRoot collisionRoot = TxApplication.ActiveDocument.CollisionRoot;
                    TxCollisionQueryParams txCollisionQueryParams = new TxCollisionQueryParams();
                    txCollisionQueryParams.Mode = collisionRoot.CollisionQueryMode;
                    txCollisionQueryParams.ComputeDistanceIfInNearMiss = true;
                    txCollisionQueryParams.NearMissDistance = collisionRoot.NearMissDefaultValue;
                    txCollisionQueryParams.UseNearMiss = collisionRoot.CheckNearMiss;
                    txCollisionQueryParams.AllowedPenetration = (double)TxApplication.Options.Collision.ContactTolerance;
                    txCollisionQueryParams.UseAllowedPenetration = TxApplication.Options.Collision.CheckContact;
                    txCollisionQueryParams.FindPenetrationRegions = TxApplication.Options.Collision.FindPenetrationRegions;
                    txCollisionQueryParams.ReportLevel = TxApplication.Options.Collision.ReportLevel;
                    if (all)
                    {
                        this.ResetQueryContext();
                    }
                    return this.GetGlobalCollidingObjectsChanges(txCollisionQueryParams);
                }
                catch (Exception)
                {
                }
                return txCollisionQueryResults;
            }
            return txCollisionQueryResults;
        }

        public void ResetQueryContext()
        {
            this.ResetQueryGlobalContext();
        }

        private void ResetQueryGlobalContext()
        {
            try
            {
                if (this.m_queryGlobalContext != null)
                {
                    TxCollisionRoot collisionRoot = TxApplication.ActiveDocument.CollisionRoot;
                    collisionRoot.ResetGlobalQueryContext(this.m_queryGlobalContext);
                }
            }
            catch (Exception)
            {
            }
        }

        private TxCollisionQueryResults GetGlobalCollidingObjectsChanges(TxCollisionQueryParams queryParams)
        {
            TxCollisionRoot collisionRoot = TxApplication.ActiveDocument.CollisionRoot;
            return collisionRoot.GetGlobalCollidingObjectsChanges(queryParams, this.m_queryGlobalContext);
        }

        #endregion



        #endregion

        #region static  method

        //获取open pose
        static TxPose GetSelectedPose(ITxTool gun)
        {
            TxPose txPose = null;

            if (gun != null && gun is ITxDevice txDevice)
            {
                TxObjectList poseList = txDevice.PoseList;
                foreach (ITxObject txObject in poseList)
                {
                    if (txObject.Name.ToLower().Contains("open"))
                    {
                        txPose = txObject as TxPose;
                        return txPose;
                    }
                }

                //如果没有open 返回home
                if (txPose is null)
                {
                    return poseList?[0] as TxPose;
                }
            }

            return txPose;
        }

        //获取rcs
        static ITxRoboticControllerServices GetRoboticControllerServices(TxRobot robot)
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

        #endregion

    }
}