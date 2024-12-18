using EngineeringInternalExtension;
using System;
using System.Windows.Forms;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Olp;

namespace AutoJTTXCoreUtilities
{
    public class AJTTxRobotUtilities
    {
        public static bool GetRobotStamp(TxRobot robot, out string rcsVersion, out string controllerVersion, out string manipulatorType, out string controller)
        {
            manipulatorType = null;
            controller = null;
            rcsVersion = null;
            controllerVersion = null;
            try
            {
                if (robot.Controller != null)
                {
                    TxRobotRRSController txRobotRRSController = robot.Controller as TxRobotRRSController;
                    if (txRobotRRSController != null)
                    {
                        manipulatorType = txRobotRRSController.ManipulatorType;
                        controller = txRobotRRSController.Name;
                        rcsVersion = txRobotRRSController.Version;
                    }
                    TxRoboticStringParam txRoboticStringParam = GetRoboticParameter(robot, "OLP_CONTROLLER_VERSION") as TxRoboticStringParam;
                    if (txRoboticStringParam != null)
                    {
                        controllerVersion = txRoboticStringParam.Value;
                    }
                }
            }
            catch (Exception exception)
            {
                AJTTxMessageHandling.WriteException(exception);
                return false;
            }
            return true;
        }

        private static TxRoboticParam GetRoboticParameter(TxRobot robot, string sParamName)
        {
            TxRoboticParam result;
            if (!(robot == null))
            {
                TxRoboticParam txRoboticParam = null;
                try
                {
                    txRoboticParam = robot.GetInstanceParameter(sParamName);
                }
                catch
                {
                    txRoboticParam = null;
                }
                if (txRoboticParam != null)
                {
                    result = txRoboticParam;
                }
                else
                {
                    try
                    {
                        txRoboticParam = robot.GetParameter(sParamName);
                    }
                    catch
                    {
                        txRoboticParam = null;
                    }
                    result = txRoboticParam;
                }
            }
            else
            {
                result = null;
            }
            return result;
        }


        public static TxObjectList GetAllSystemFrames(TxRobot robot)
        {
            TxObjectList result;
            if (!(robot == null))
            {
                try
                {
                    return robot.GetAllSystemFrames();
                }
                catch (Exception exception)
                {
                    AJTTxMessageHandling.WriteException(exception);
                }
                result = new TxObjectList();
            }
            else
            {
                result = new TxObjectList();
            }
            return result;
        }

        public static TxFrame GetRobotWorldFrameKUKA(TxRobot robot)
        {
            return GetRobotSystemFrame(robot, "bf0");
        }
        public static TxFrame GetRobotSystemFrame(TxRobot robot, string name)
        {
            TxFrame result;
            if (!(robot == null) && !string.IsNullOrEmpty(name))
            {
                TxObjectList allSystemFrames = GetAllSystemFrames(robot);
                if (allSystemFrames != null && allSystemFrames.Count > 0)
                {
                    try
                    {
                        foreach (ITxObject txObject in allSystemFrames)
                        {
                            try
                            {
                                TxFrame txFrame = txObject as TxFrame;
                                if (txFrame != null && txFrame.Name.Equals(name))
                                {
                                    return txFrame;
                                }
                            }
                            catch
                            {
                            }
                        }
                        return null;
                    }
                    catch (Exception exception)
                    {
                        AJTTxMessageHandling.WriteException(exception);
                    }
                    result = null;
                }
                else
                {
                    result = null;
                }
            }
            else
            {
                result = null;
            }
            return result;
        }




        #region GetRobot

        public static TxRobot GetRobot(ITxRoboticLocationOperation location)
        {
            TxRobot result = null;
            if (location != null && location.ParentRoboticOperation != null)
            {
                result = (location.ParentRoboticOperation.Robot as TxRobot);
            }
            return result;
        }
        public static TxRobot GetRobot(ITxOperation location)
        {
            TxRobot result = null;

            if (location != null && location is ITxRoboticLocationOperation locationOperation)
            {
                if (locationOperation.ParentRoboticOperation != null)
                {
                    result = (locationOperation.ParentRoboticOperation.Robot as TxRobot);
                }
            }
          
            return result;
        }

        public static TxRobot GetRobot(ITxRoboticOperation operation)
        {
            TxRobot result = null;
            if (operation != null)
            {
                if (operation is ITxRoboticOrderedCompoundOperation)
                {
                    result = ((operation as ITxRoboticOrderedCompoundOperation).Robot as TxRobot);
                }
                else if (operation.Collection != null && operation.Collection is ITxRoboticOrderedCompoundOperation)
                {
                    result = ((operation.Collection as ITxRoboticOrderedCompoundOperation).Robot as TxRobot);
                }
            }
            return result;
        }

        public static ITxRobot GetRoboticLocationOperationAssignedRobot(ITxObject selectedObject)
        {
            ITxRoboticLocationOperation txRoboticLocationOperation = selectedObject as ITxRoboticLocationOperation;
            if (txRoboticLocationOperation != null && txRoboticLocationOperation.ParentRoboticOperation != null)
            {
                return txRoboticLocationOperation.ParentRoboticOperation.Robot;
            }
            return null;
        }

        #endregion

        #region JumpAssignedRobot

        public static void JumpAssignedRobot(ITxRoboticLocationOperation txRoboticLocationOperation, out TxRobot robot)
        {
            bool flag = false;
            string text = string.Empty;
            robot = null;
            if (txRoboticLocationOperation != null)
            {
                //当前绑定的 robot
                robot = GetRobot(txRoboticLocationOperation);

                Cursor value = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;

                if (robot != null)
                {
                    //撤销事务
                    TxApplication.ActiveUndoManager.StartTransaction();

                    //检查controller
                    if (robot.Controller != null)
                    {
                        //提供 OLP 控制器实用程序。
                        TxOlpControllerUtilities txOlpControllerUtilities = new TxOlpControllerUtilities();

                        //如果对象已经存在，则返回该对象。 如果对象不存在，则根据属性名、属性、接口、属性值在OLP目录下创建对象。
                        ITxRoboticControllerServices txRoboticControllerServices = (ITxRoboticControllerServices)txOlpControllerUtilities.GetInterfaceImplementationFromController(robot.Controller.Name,
                            typeof(ITxRoboticControllerServices), typeof(TxControllerAttribute), "ControllerName");

                        //没有设置rcs 检查default rcs 
                        if (txRoboticControllerServices == null)
                        {
                            txRoboticControllerServices = (ITxRoboticControllerServices)txOlpControllerUtilities.GetInterfaceImplementationFromController("default", typeof(ITxRoboticControllerServices), typeof(TxControllerAttribute), "ControllerName");
                        }

                        //将机器人跳转到指定位置。
                        if (txRoboticControllerServices != null)
                        {
                            //初始化指定的类(指定的机器人)。
                            txRoboticControllerServices.Init(robot);
                            //jump
                            TxJumpToLocationStatus txJumpToLocationStatus = txRoboticControllerServices.JumpToLocation(txRoboticLocationOperation, new TxJumpToLocationData
                            {
                                //true 由控制器生成消息并显示在状态栏中；
                                GenerateMessage = true
                            });

                            if (txJumpToLocationStatus == (TxJumpToLocationStatus)2)
                            {
                                text = "Unable to jump robot {0} to this location.";
                                text = string.Format(text, robot.Name);
                            }
                            else
                            {
                                flag = true;
                            }
                        }
                    }

                    //刷线显示
                    TxApplication.RefreshDisplay();
                    Cursor.Current = value;

                    //撤销事务
                    TxApplication.ActiveUndoManager.EndTransaction();

                    if (!flag)
                    {
                        TxMessageBox.Show(text, "Jump Assigned Robot", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                }
            }
        }

        //Get First Location for selected
        private ITxRoboticLocationOperation GetFirstLocation(ITxRoboticOperation operation)
        {
            ITxRoboticLocationOperation result = null;
            TxObjectList txObjectList = null;
            if (operation is ITxRoboticOrderedCompoundOperation)
            {
                txObjectList = (operation as ITxRoboticOrderedCompoundOperation).GetAllDescendants(new TxTypeFilter(typeof(ITxRoboticLocationOperation)));
            }
            else if (operation is TxRoboticSeamOperation)
            {
                txObjectList = (operation as TxRoboticSeamOperation).GetAllDescendants(new TxTypeFilter(typeof(TxRoboticSeamLocationOperation)));
            }
            if (txObjectList != null && txObjectList.Count > 0)
            {
                result = (txObjectList[0] as ITxRoboticLocationOperation);
            }
            return result;
        }

        #endregion

        public static TxObjectList GetAppearancesFromOperation(ITxObject sourceObject)
        {
            TxObjectList result = new TxObjectList();
            ITxOperation txOperation = sourceObject as ITxOperation;
            if (txOperation != null)
            {
                TxOperationEx txOperationEx = new TxOperationEx(txOperation);
                result = txOperationEx.GetAllPartAppearances();
            }
            return result;
        }        
    }
}
