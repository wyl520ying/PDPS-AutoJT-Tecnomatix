using EngineeringInternalExtension;
using System.Collections;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Olp;

namespace AutoJTTXCoreUtilities.RobotMatrix
{
    public class AJTApRmxLocationRobotServices
    {
        internal AJTApRmxLocationRobotServices(ITxRoboticControllerServices robotControllerServices, ITxLocationOperation locationOp)
        {
            this.Callback = new TxRoboticControllerServicesDelegator(new TxRoboticControllerServicesDelegator.TxRoboticControllerServices_CalculateInverseSolutions(this.CalculateInverseSolutions), new TxRoboticControllerServicesDelegator.TxRoboticControllerServices_CheckInverse(this.CheckInverse));
            this.RobotControllerServices = robotControllerServices;
            this.LocationOperation = locationOp;
            this.JumpLocationData = new TxJumpToLocationData();
            this.JumpLocationData.UseExternalAxes = false;
            this.JumpLocationData.UseTaughtLocations = false;
            this.JumpLocationData.UseTaughtPose = false;
        }

        private ArrayList CalculateInverseSolutions(int inverseType)
        {
            ArrayList result = null;
            if (this.RobotControllerServices != null)
            {
                result = this.RobotControllerServices.CalculateInverseSolutions(this.LocationOperation, this.JumpLocationData, (TxRobotInverseData.TxInverseType)inverseType);
            }
            return result;
        }

        private int CheckInverse(int inverseType)
        {
            int result = -1;
            if (this.RobotControllerServices != null)
            {
                TxPoseData txPoseData;
                result = (int)this.RobotControllerServices.CheckInverse(this.LocationOperation, this.JumpLocationData, (TxRobotInverseData.TxInverseType)inverseType, out txPoseData);
            }
            return result;
        }

        public readonly TxRoboticControllerServicesDelegator Callback;

        public ITxRoboticControllerServices RobotControllerServices;

        public ITxLocationOperation LocationOperation;

        public readonly TxJumpToLocationData JumpLocationData;
    }
}
