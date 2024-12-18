using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Olp.OLP_Utilities;

namespace AutoJTTXCoreUtilities.RobotMatrix
{
    public class AJTApFollowModeController
    {
        public ITxLocationOperation Location
        {
            get
            {
                return this.m_location;
            }
            set
            {
                if (this.m_shouldFollow)
                {
                    this.m_location = value;
                }
            }
        }

        public bool ShouldFollow
        {
            get
            {
                return this.m_shouldFollow;
            }
            set
            {
                if (!value && this.m_shouldFollow)
                {
                    this.StopFollowMode();
                }
                this.m_shouldFollow = value;
                this.m_followMode.FollowMode = value;
            }
        }

        public bool IsGhostGunAllowed { get; private set; }

        public AJTApFollowModeController()
        {
            this.m_followMode = new TxOlpRobotFollowMode();
            this.m_followMode.AllowMountedWorkpiece = true;
            this.IsGhostGunAllowed = false;
        }

        public bool UpdateRobotFollowMode(TxPose pose, TxFrame frame, ITxTool tool)
        {
            bool result = false;
            if (this.ShouldFollow && this.Location != null)
            {
                this.UpdateLocationInformation(pose, frame, tool);
                result = this.SetGunAndRobotOnLocation();
            }
            else
            {
                this.StopFollowMode();
            }
            TxApplication.RefreshDisplay();
            return result;
        }

        private void UpdateLocationInformation(TxPose pose, TxFrame frame, ITxTool tool)
        {
            if (this.m_locParams == null)
            {
                this.m_locParams = new AJTApAutoApproachAngleLocationParameters(this.Location);
            }
            else
            {
                this.m_locParams.RestoreLocationAndSetNewLocation(this.Location);
            }
            this.m_locParams.SetState(pose, frame, tool);
        }

        private bool SetGunAndRobotOnLocation()
        {
            bool result = true;
            this.m_followMode.FollowMode = true;
            if (this.m_location == null || !(this.m_location is ITxRoboticLocationOperation))
            {
                this.StopFollowMode();
                return result;
            }
            if (this.IsGhostGunAllowed)
            {
                result = this.m_followMode.SetGunAndRobotOnLocation(this.m_location as ITxRoboticLocationOperation);
            }
            else
            {
                throw new System.Exception();//result = this.m_followMode.SetGunAndRobotOnLocationWithoutGhostGun(this.m_location as ITxRoboticLocationOperation);
            }
            return result;
        }

        public void StopFollowMode()
        {
            this.ResetGunAndRobotPose();
            this.DestroyGhostGun();
            TxApplication.RefreshDisplay();
        }

        private void ResetGunAndRobotPose()
        {
            this.m_followMode.ResetGunAndRobotPose();
            AJTApAutoApproachAngleLocationParameters locParams = this.m_locParams;
            if (locParams == null)
            {
                return;
            }
            locParams.RestoreLocationInformation();
        }

        private void DestroyGhostGun()
        {
            this.m_followMode.DestroyGhostGun();
        }

        private TxOlpRobotFollowMode m_followMode;

        private ITxLocationOperation m_location;

        private AJTApAutoApproachAngleLocationParameters m_locParams;

        private bool m_shouldFollow;
    }

}