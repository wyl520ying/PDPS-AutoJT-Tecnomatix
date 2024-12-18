using EngineeringInternalExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Olp;

namespace AutoJTTXCoreUtilities
{
    public class AJTRoboticCommand
    {
        public static bool CheckDuplicates(TxObjectList txRoboticCommands, string commValue, out TxRoboticCommand currnetComm)
        {
            currnetComm = null;

            //检查是否已经添加
            bool flag1 = false;
            try
            {
                foreach (ITxObject item in txRoboticCommands)
                {
                    try
                    {
                        TxRoboticCommand txRoboticCommand = item as TxRoboticCommand;

                        if (txRoboticCommand == null)
                        {
                            continue;
                        }

                        if (txRoboticCommand.Text == commValue)
                        {
                            currnetComm = txRoboticCommand;
                            flag1 = true;
                            break;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            catch
            {
                flag1 = false;
            }

            return flag1;
        }

        public static void ClearAllCmd(TxObjectList olpCommands)
        {
            //移除现有command
            try
            {
                foreach (TxRoboticCommand item in olpCommands)
                {
                    try
                    {
                        item.Delete();
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            catch { }
        }

        static string GetServoValue(ITxOperation txRoboticLocation)
        {
            string result = string.Empty;

            try
            {
                TxRobot txRobot = AJTTxRobotUtilities.GetRobot(txRoboticLocation);
                if (txRobot != null)
                {
                    //当前rcs
                    string name = txRobot.Controller.Name;
                    TxOlpControllerUtilities controllerUtilities = new TxOlpControllerUtilities();

                    ITxOlpRobotControllerParametersHandler paramHandler = (ITxOlpRobotControllerParametersHandler)controllerUtilities.GetInterfaceImplementationFromController(name, typeof(ITxOlpRobotControllerParametersHandler), typeof(TxRobotSimulationControllerAttribute), "ControllerName");

                    //获取value
                    result = paramHandler.GetComplexRepresentation("Servo Value", txRoboticLocation, 0);

                    //使用正则表达式替换所有非数字和非小数点字符
                    if (!string.IsNullOrEmpty(result))
                    {
                        result = RemoveNonNumericCharacters(result);
                    }
                }
            }
            catch
            {
                result = string.Empty;
            }

            return result;
        }

        static bool SetServoValue(ITxOperation txRoboticLocation, double val)
        {
            bool result = false;


            try
            {
                TxRobot txRobot = AJTTxRobotUtilities.GetRobot(txRoboticLocation);
                if (txRobot != null)
                {
                    //当前rcs
                    string name = txRobot.Controller.Name;
                    TxOlpControllerUtilities controllerUtilities = new TxOlpControllerUtilities();

                    ITxOlpRobotControllerParametersHandler paramHandler = (ITxOlpRobotControllerParametersHandler)controllerUtilities.GetInterfaceImplementationFromController(name, typeof(ITxOlpRobotControllerParametersHandler), typeof(TxRobotSimulationControllerAttribute), "ControllerName");

                    if (paramHandler.HasComplexRepresentation("Servo Value", txRoboticLocation, TxOlpCommandLayerRepresentation.UI))
                    {
                        paramHandler.OnComplexValueChanged("Servo Value", val.ToString(), txRoboticLocation);

                        return true;
                    }
                    return false;
                }
            }
            catch
            {
                result = false;
            }


            return result;
        }

        //通过外部轴设置私服值
        public static bool SetServoValue4Gun(ITxOperation txRoboticLocation, double val)
        {
            bool result = false;

            try
            {
                
                if (txRoboticLocation is ITxRoboticLocationOperation txRoboticLocationOperation)
                {
                    //获取当前location的所有机器人外部轴的值
                    var axesData = txRoboticLocationOperation.RobotExternalAxesData;
                    if (axesData?.Length > 0)
                    {
                        foreach (var item in axesData)
                        {
                            if (item.Device is ITxGun)
                            {
                                var joint = item.Joint;
                             

                                if (joint.Type is TxJoint.TxJointType.Revolute)
                                {
                                    //修改solt limit
                                    ChangeSoftLimit(joint, val * 0.017453292519943);

                                    item.JointValue = val;//Math.Round(val * 57.29577951308232, 2);
                                    
                                    return SetServoValue(txRoboticLocation, val);
                                }
                                else
                                {
                                    //修改solt limit
                                    ChangeSoftLimit(joint, val);

                                    item.JointValue = val;//Math.Round(val, 2);

                                    return SetServoValue(txRoboticLocation, val);
                                }
                            }
                        }
                    }
                }                                
            }
            catch
            {
                return false;
            }

            return result;
        }


        //通过外部轴类型找焊枪的私服值
        public static string GetServoValue4Gun(ITxOperation txRoboticLocation)
        {
            string result = string.Empty;

            try
            {
                if (txRoboticLocation is ITxRoboticLocationOperation txRoboticLocationOperation)
                {
                    //获取当前location的所有机器人外部轴的值
                    var axesData = txRoboticLocationOperation.RobotExternalAxesData;
                    if (axesData?.Length > 0)
                    {
                        foreach (var item in axesData)
                        {
                            if (item.Device is ITxGun)
                            {
                                var joint = item.Joint;

                                if (joint.Type is TxJoint.TxJointType.Revolute)
                                {
                                    return Math.Round(item.JointValue * 57.29577951308232, 2).ToString();//Math.Round(item.JointValue * 57.29577951308232, 2).ToString();
                                }
                                else
                                {
                                    return item.JointValue.ToString();//Math.Round(item.JointValue, 2).ToString();
                                }
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(result))
                {
                    return GetServoValue(txRoboticLocation);
                }
            }
            catch
            {
                return null;
            }

            return result;
        }


        // 使用正则表达式替换所有非数字和非小数点字符
        static string RemoveNonNumericCharacters(string input)
        {
            // 使用正则表达式替换所有非数字和非小数点字符
            return Regex.Replace(input, "[^0-9.]", "");
        }

        //修改solt limit
        static void ChangeSoftLimit(TxJoint jont, double currentVla)
        {
            if (jont != null)
            {
                if (currentVla < jont.LowerSoftLimit)
                {
                    jont.LowerSoftLimit = currentVla;
                }
                else if (currentVla > jont.UpperSoftLimit)
                {
                    jont.UpperSoftLimit = currentVla;
                }
            }
        }
    }
}