using EngineeringInternalExtension.DataTypes;
using EngineeringInternalExtension.Options;
using System.Collections;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Olp;

namespace AutoJTTXCoreUtilities.RobotMatrix
{
    public class AdjustLocationParams
    {
        /// <summary>
        /// location 的各项参数
        /// </summary>
        /// <param name="robot">绑定的机器人</param>
        /// <param name="gun">绑定的焊枪</param>
        /// <param name="poseList">pose list</param>
        /// <param name="locOp">weld via ...</param>
        /// <param name="context"></param>
        /// <param name="gunScenario"></param>
        /// <param name="rcs"></param>
        /// <param name="algorithm"></param>
        public AdjustLocationParams(TxRobot robot, ITxTool gun, ArrayList poseList,
            ITxLocationOperation locOp, TxCollisionQueryContext context, TxInputReachabilityAndAccessibilityDataGunScenarioType gunScenario,
            ITxRoboticControllerServices rcs, TxAutomaticApproachAngleAlgorithmTypeEx algorithm)
        {
            this.m_robot = robot;
            this.m_gun = gun;
            this.m_poseList = poseList;
            this.m_locOp = locOp;
            this.m_context = context;
            this.m_gunScenario = gunScenario;
            this.m_rcs = rcs;
            this.m_algorithm = algorithm;
            this.m_toolFrame = null;
        }

        public void SetToolFrame(TxTransformation newToolFrame)
        {
            this.m_toolFrame = newToolFrame;
        }

        /// <summary>
        /// 绑定的机器人
        /// </summary>
        public TxRobot m_robot;

        /// <summary>
        /// 绑定的焊枪
        /// </summary>
        public ITxTool m_gun;

        /// <summary>
        /// pose list
        /// </summary>
        public ArrayList m_poseList;

        /// <summary>
        /// weld via ...
        /// </summary>
        public ITxLocationOperation m_locOp;

        /// <summary>
        /// Collision Query Context
        /// </summary>
        public TxCollisionQueryContext m_context;

        /// <summary>
        /// 可达性类型
        /// </summary>
        public TxInputReachabilityAndAccessibilityDataGunScenarioType m_gunScenario;

        /// <summary>
        /// Robotic Controller Services
        /// </summary>
        public ITxRoboticControllerServices m_rcs;

        /// <summary>
        /// Approach Angle type
        /// </summary>
        public TxAutomaticApproachAngleAlgorithmTypeEx m_algorithm;

        /// <summary>
        /// toolFrame
        /// </summary>
        public TxTransformation m_toolFrame;

        //机器人跳转到指定位置
        public static bool ChangeRobotPose(ITxRoboticLocationOperation location, TxRobot Robot)
        {
            bool flag = false;
            if (location != null && Robot != null)
            {
                new TxOLPUtilities();
                try
                {
                    TxOlpControllerUtilities txOlpControllerUtilities = new TxOlpControllerUtilities();
                    ITxRoboticControllerServices txRoboticControllerServices = (ITxRoboticControllerServices)txOlpControllerUtilities.GetInterfaceImplementationFromController(Robot.Controller.Name, typeof(ITxRoboticControllerServices), typeof(TxControllerAttribute), "ControllerName");
                    if (txRoboticControllerServices == null)
                    {
                        txRoboticControllerServices = (ITxRoboticControllerServices)txOlpControllerUtilities.GetInterfaceImplementationFromController("default", typeof(ITxRoboticControllerServices), typeof(TxControllerAttribute), "ControllerName");
                    }
                    if (txRoboticControllerServices != null)
                    {
                        txRoboticControllerServices.Init(Robot);
                        //TxRobotConfigurationData txRobotConfigurationData = null;
                        //if (this.ForceConfiguration)
                        //{
                        //    txRobotConfigurationData = location.RobotConfigurationData;
                        //    location.RobotConfigurationData = Robot.GetPoseConfiguration(Robot.CurrentPose);
                        //}
                        TxJumpToLocationData txJumpToLocationData = new TxJumpToLocationData();
                        //txJumpToLocationData.ForceConfiguration = this.ForceConfiguration;
                        txJumpToLocationData.UseTaughtLocations = false;
                        txJumpToLocationData.GenerateMessage = false;
                        //txJumpToLocationData.UseTaughtPose = this.UseTaughtPose;
                        try
                        {
                            TxJumpToLocationStatus txJumpToLocationStatus = txRoboticControllerServices.JumpToLocation(location, txJumpToLocationData);
                            if (txJumpToLocationStatus == TxJumpToLocationStatus.Success || txJumpToLocationStatus == TxJumpToLocationStatus.SuccessWithDifferentConfig)
                            {
                                flag = true;
                            }
                        }
                        catch
                        {
                        }
                        //if (this.ForceConfiguration)
                        //{
                        //    location.RobotConfigurationData = txRobotConfigurationData;
                        //}
                    }
                }
                catch
                {
                }
            }
            return flag;
        }

    }
}