using EngineeringInternalExtension;
using System.Collections.Generic;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Olp;

namespace AutoJTTXCoreUtilities
{
    internal class AJTGhostGunDisplayer
    {
        #region private field

        private ITxRobot _robot;

        private ITxObject _gun;

        private ITxRoboticLocationOperation _location;

        private TxGhostGunEx _ghostGunCreator = new TxGhostGunEx();

        private ITxRoboticControllerServices _controllerServices;

        #endregion

        public AJTGhostGunDisplayer(ITxRobot robot)
        {
            this._robot = robot;
            if (this._robot != null)
            {
                this._controllerServices = this.GetControllerServices();
            }
        }

        public bool IsDisplayed { get; private set; }

        private ITxObject Gun
        {
            get
            {
                return this._gun;
            }
            set
            {
                this.UnregisterFromGunEvents(this._gun);
                this.HideGun();
                this._gun = value;
                if (this._gun != null)
                {
                    this.RegisterForGunEvents(this._gun);
                    this.ShowGunOnLocation();
                }
            }
        }

        private ITxRoboticLocationOperation Location
        {
            get
            {
                return this._location;
            }
            set
            {
                this.UnregisterFromLocationEvents(this._location);
                ITxRobot roboticLocationOperationAssignedRobot = AJTTxRobotUtilities.GetRoboticLocationOperationAssignedRobot(value);
                if (this._robot != roboticLocationOperationAssignedRobot)
                {
                    this._location = null;
                    this.Gun = null;
                    return;
                }
                this._location = value;
                this.RegisterForLocationEvents(this._location);
                this.Gun = this.GetActiveGun();
            }
        }

        public void Show(ITxRoboticLocationOperation location)
        {
            this.Location = location;
        }

        public void Hide()
        {
            if (this.IsDisplayed)
            {
                this.Gun = null;
                this.Location = null;
            }
        }

        private void ShowGunOnLocation()
        {
            if (this.Location != null && !this.IsDisplayed)
            {
                Dictionary<TxJoint, double> dictionary = this.SetExternalValuesOnLocation();
                this._ghostGunCreator.CreateGhostGun(this.Gun, false);
                this.JumpGunToLocation();
                if (dictionary.Count > 0)
                {
                    this.RestoreChangedJointValues(dictionary);
                    TxApplication.ActiveUndoManager.ClearAllTransactions();
                }
                this.IsDisplayed = true;
            }
        }

        private void JumpGunToLocation()
        {
            TxTransformation activeMountedTcpRelativeLocation = this._controllerServices.GetActiveMountedTcpRelativeLocation(this.Location);
            if (activeMountedTcpRelativeLocation != null)
            {
                TxTransformation gunTCPFForJump = TxGhostGunEx.GetGunTCPFForJump(this.Gun);
                TxTransformation txTransformation = this._robot.Toolframe.AbsoluteLocation * activeMountedTcpRelativeLocation;
                TxTransformation txTransformation2 = txTransformation.Inverse * gunTCPFForJump;
                TxTransformation absoluteLocation = (this.Location as ITxLocatableObject).AbsoluteLocation;
                this._ghostGunCreator.JumpGhostGunToLocation(absoluteLocation * txTransformation2);
                return;
            }
            this._ghostGunCreator.JumpGhostGunToLocation(this.Location);
        }

        private void HideGun()
        {
            if (this.IsDisplayed)
            {
                this._ghostGunCreator.DestroyGhostGun();
                this.IsDisplayed = false;
            }
        }

        private void RegisterForGunEvents(ITxObject gun)
        {
            if (gun != null)
            {
                gun.Deleted += new TxObject_DeletedEventHandler(this.GunDeleted);
                (gun as ITxDisplayableObject).VisibilitySet += new TxDisplayableObject_VisibilitySetEventHandler(this.GunVisibilitySet);
            }
        }

        private void UnregisterFromGunEvents(ITxObject gun)
        {
            if (gun != null)
            {
                gun.Deleted -= new TxObject_DeletedEventHandler(this.GunDeleted);
                (gun as ITxDisplayableObject).VisibilitySet -= new TxDisplayableObject_VisibilitySetEventHandler(this.GunVisibilitySet);
            }
        }

        private void GunVisibilitySet(object sender, TxDisplayableObject_VisibilitySetEventArgs args)
        {
            ITxDisplayableObject txDisplayableObject = this.Gun as ITxDisplayableObject;
            if (txDisplayableObject.Visibility == null || txDisplayableObject.Visibility == (TxDisplayableObjectVisibility)1)
            {
                this._controllerServices.Init(this._robot as TxRobot);
                this.ShowGunOnLocation();
                return;
            }
            this.HideGun();
        }

        private void RegisterForLocationEvents(ITxRoboticLocationOperation location)
        {
            if (location != null)
            {
                (location as ITxLocatableObject).AbsoluteLocationChanged += new TxLocatableObject_AbsoluteLocationChangedEventHandler(this.LocationAbsoluteLocationChanged);
                location.Deleted += new TxObject_DeletedEventHandler(this.LocationDeleted);
            }
        }

        private void UnregisterFromLocationEvents(ITxRoboticLocationOperation location)
        {
            if (location != null)
            {
                (location as ITxLocatableObject).AbsoluteLocationChanged -= new TxLocatableObject_AbsoluteLocationChangedEventHandler(this.LocationAbsoluteLocationChanged);
                location.Deleted -= new TxObject_DeletedEventHandler(this.LocationDeleted);
            }
        }

        private void GunDeleted(object sender, TxObject_DeletedEventArgs args)
        {
            this.Hide();
        }

        private void LocationDeleted(object sender, TxObject_DeletedEventArgs args)
        {
            this.Hide();
        }

        private void LocationAbsoluteLocationChanged(object sender, TxLocatableObject_AbsoluteLocationChangedEventArgs args)
        {
            this._ghostGunCreator.JumpGhostGunToLocation(this.Location);
        }

        private ITxRoboticControllerServices GetControllerServices()
        {
            TxOlpControllerUtilities txOlpControllerUtilities = new TxOlpControllerUtilities();
            ITxRoboticControllerServices txRoboticControllerServices = txOlpControllerUtilities.GetInterfaceImplementationFromController(this._robot.Controller.Name, typeof(ITxRoboticControllerServices), typeof(TxControllerAttribute), "ControllerName") as ITxRoboticControllerServices;
            if (txRoboticControllerServices == null)
            {
                txRoboticControllerServices = (txOlpControllerUtilities.GetInterfaceImplementationFromController("default", typeof(ITxRoboticControllerServices), typeof(TxControllerAttribute), "ControllerName") as ITxRoboticControllerServices);
            }
            return txRoboticControllerServices;
        }

        private ITxObject GetActiveGun()
        {
            if (this._controllerServices == null)
            {
                return null;
            }
            ITxObject txObject = null;
            try
            {
                this._controllerServices.Init(this._robot as TxRobot);
                txObject = this._controllerServices.GetActiveGun(this.Location);
                if (txObject == null)
                {
                    txObject = this._controllerServices.GetActiveGripper(this.Location);
                }
                if (txObject != null && !txObject.IsValid())
                {
                    txObject = null;
                }
            }
            catch
            {
            }
            return txObject;
        }

        private Dictionary<TxJoint, double> SetExternalValuesOnLocation()
        {
            Dictionary<TxJoint, double> dictionary = new Dictionary<TxJoint, double>();
            ITxDevice txDevice = this.Gun as ITxDevice;
            if (txDevice == null || this.Location == null || this.Location.RobotExternalAxesData == null)
            {
                return dictionary;
            }
            TxObjectList drivingJoints = txDevice.DrivingJoints;
            foreach (TxRobotExternalAxisData txRobotExternalAxisData in this.Location.RobotExternalAxesData)
            {
                if (txRobotExternalAxisData.Device == txDevice)
                {
                    dictionary.Add(txRobotExternalAxisData.Joint, txRobotExternalAxisData.Joint.CurrentValue);
                    txRobotExternalAxisData.Joint.CurrentValue = txRobotExternalAxisData.JointValue;
                }
            }
            return dictionary;
        }

        private void RestoreChangedJointValues(Dictionary<TxJoint, double> savedValues)
        {
            if (savedValues == null)
            {
                return;
            }
            foreach (KeyValuePair<TxJoint, double> keyValuePair in savedValues)
            {
                keyValuePair.Key.CurrentValue = keyValuePair.Value;
            }
        }


    }
}