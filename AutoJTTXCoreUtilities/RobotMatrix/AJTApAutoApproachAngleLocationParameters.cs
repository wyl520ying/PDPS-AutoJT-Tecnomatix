using System.Collections.Generic;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Olp;

namespace AutoJTTXCoreUtilities.RobotMatrix
{
    internal class AJTApAutoApproachAngleLocationParameters
    {
        public AJTApAutoApproachAngleLocationParameters(ITxLocationOperation location)
        {
            this.InitializeMembers(location);
        }

        private void InitializeMembers(ITxLocationOperation location)
        {
            this.m_location = location;
            this.GetRobotFromLocationInformation();
            this.GetRoboticControllerServices();
            this.GetToolFromLocationInformation();
            this.GetToolFrameFromLocationInformation();
            this.GetToolPoseFromLocationInformation();
        }

        private void SetTool(ITxTool newTool)
        {
            if (this.m_tool == newTool)
            {
                return;
            }
            this.RestoreToolPose();
            this.m_tool = newTool;
        }

        public void RestoreLocationAndSetNewLocation(ITxLocationOperation location)
        {
            if (this.m_location != location)
            {
                this.RestoreLocationInformation();
                this.InitializeMembers(location);
            }
        }

        public void RestoreLocationInformation()
        {
            this.RestoreToolPose();
            this.RestoreToolFrame();
        }

        private void GetToolPoseFromLocationInformation()
        {
            this.m_toolPoseValues = new List<double>();
            ITxDevice txDevice = this.m_tool as ITxDevice;
            TxObjectList txObjectList = (txDevice != null) ? txDevice.DrivingJoints : null;
            if (txObjectList != null)
            {
                for (int i = 0; i < txObjectList.Count; i++)
                {
                    TxJoint txJoint = txObjectList[i] as TxJoint;
                    this.m_toolPoseValues.Add(txJoint.CurrentValue);
                }
            }
        }

        private void SetToolPose(TxPose newPose)
        {
            ITxDevice txDevice = this.m_tool as ITxDevice;
            TxObjectList txObjectList = (txDevice != null) ? txDevice.DrivingJoints : null;
            if (newPose == null || txObjectList == null)
            {
                return;
            }
            int num = 0;
            while (num < txObjectList.Count && num < newPose.PoseData.JointValues.Count)
            {
                TxJoint txJoint = txObjectList[num] as TxJoint;
                txJoint.CurrentValue = (double)newPose.PoseData.JointValues[num];
                num++;
            }
        }

        private void RestoreToolPose()
        {
            ITxDevice txDevice = this.m_tool as ITxDevice;
            TxObjectList txObjectList = (txDevice != null) ? txDevice.DrivingJoints : null;
            if (txObjectList != null)
            {
                for (int i = 0; i < this.m_toolPoseValues.Count; i++)
                {
                    TxJoint txJoint = txObjectList[i] as TxJoint;
                    if (txJoint != null)
                    {
                        txJoint.CurrentValue = this.m_toolPoseValues[i];
                    }
                }
            }
        }

        private void GetToolFrameFromLocationInformation()
        {
            this.m_transformation = this.m_robot.TCPF.AbsoluteLocation;
        }

        private void RestoreToolFrame()
        {
            this.m_robot.TCPF.AbsoluteLocation = this.m_transformation;
        }

        private void SetToolFrame(TxFrame newFrame)
        {
            this.m_robot.TCPF.AbsoluteLocation = newFrame.AbsoluteLocation;
        }

        private void GetRobotFromLocationInformation()
        {
            this.m_robot = null;
            ITxRoboticLocationOperation txRoboticLocationOperation = this.m_location as ITxRoboticLocationOperation;
            if (txRoboticLocationOperation != null && txRoboticLocationOperation.ParentRoboticOperation != null)
            {
                this.m_robot = (txRoboticLocationOperation.ParentRoboticOperation.Robot as TxRobot);
            }
        }

        private void GetToolFromLocationInformation()
        {
            bool flag = true;
            if (this.m_robot != null)
            {
                if (this.m_rcs == null)
                {
                    flag = false;
                }
                if (flag)
                {
                    this.m_tool = (this.m_rcs.GetActiveGun(this.m_location as ITxRoboticLocationOperation) as ITxTool);
                }
                if (flag && this.m_tool == null)
                {
                    this.m_tool = (this.m_rcs.GetActiveGripper(this.m_location as ITxRoboticLocationOperation) as ITxTool);
                }
            }
            else
            {
                flag = false;
            }
            if (!flag)
            {
                this.m_tool = null;
            }
        }

        private void GetRoboticControllerServices()
        {
            this.m_rcs = null;
            if (this.m_robot != null && this.m_robot.Controller != null)
            {
                TxOlpControllerUtilities txOlpControllerUtilities = new TxOlpControllerUtilities();
                this.m_rcs = (ITxRoboticControllerServices)txOlpControllerUtilities.GetInterfaceImplementationFromController(this.m_robot.Controller.Name, typeof(ITxRoboticControllerServices), typeof(TxControllerAttribute), "ControllerName");
                if (this.m_rcs == null)
                {
                    this.m_rcs = (ITxRoboticControllerServices)txOlpControllerUtilities.GetInterfaceImplementationFromController("default", typeof(ITxRoboticControllerServices), typeof(TxControllerAttribute), "ControllerName");
                }
                if (this.m_rcs != null)
                {
                    this.m_rcs.Init(this.m_robot);
                }
            }
        }

        public void SetState(TxPose pose, TxFrame frame, ITxTool tool)
        {
            this.SetTool(tool);
            this.SetToolPose(pose);
            this.SetToolFrame(frame);
        }

        private ITxLocationOperation m_location;

        private ITxTool m_tool;

        private TxTransformation m_transformation;

        private List<double> m_toolPoseValues;

        private TxRobot m_robot;

        private ITxRoboticControllerServices m_rcs;
    }

}
