using System.Resources;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Ui;

namespace AutoJTTXCoreUtilities.RobotMatrix
{
    internal class AJTAutoApproachAngelLocationValidator : ITxValidator
    {
        public AJTAutoApproachAngelLocationValidator(bool simulationMode)
        {
            this.m_simulationMode = simulationMode;
        }

        //public ITxObject GetObject(string str)
        //{
        //    ITxObject result = null;
        //    TxObjectList txObjectList = null;
        //    if (!string.IsNullOrEmpty(str))
        //    {
        //        txObjectList = TxApplication.ActiveDocument.GetObjectsByName(str);
        //    }
        //    if (txObjectList != null && txObjectList.Count > 0)
        //    {
        //        string text = null;
        //        foreach (ITxObject txObject in txObjectList)
        //        {
        //            if (this.IsValidObject(txObject, out text))
        //            {
        //                result = txObject;
        //                break;
        //            }
        //        }
        //    }
        //    return result;
        //}

        public string GetText(ITxObject obj)
        {
            string result = string.Empty;
            if (obj != null)
            {
                result = obj.Name;
            }
            return result;
        }

        //public bool IsValidObject(ITxObject obj, out string errorMessage)
        //{
        //    bool result = false;
        //    errorMessage = string.Empty;
        //    if (obj != null)
        //    {
        //        if (this.m_simulationMode)
        //        {
        //            if (obj is ITxRoboticLocationOperation)
        //            {
        //                if (AJTApGenUtils.IsValidLocationInSimulation(obj as ITxRoboticLocationOperation))
        //                {
        //                    result = true;
        //                    errorMessage = null;
        //                }
        //                else
        //                {
        //                    errorMessage = this.GetResourceManger().GetString("TT_INVALID_OBJECT");
        //                }
        //            }
        //            else if (obj is ITxWeldOperation || obj is TxWeldPoint || obj is TxGenericRoboticOperation || obj is ITxContinuousOperation || obj is TxRoboticSeamOperation || obj is TxSeamMfgFeature)
        //            {
        //                errorMessage = string.Empty;
        //            }
        //            else
        //            {
        //                errorMessage = this.GetResourceManger().GetString("TT_INVALID_OBJECT");
        //            }
        //        }
        //        else if (obj is TxWeldLocationOperation)
        //        {
        //            if ((obj as TxWeldLocationOperation).IsProjected)
        //            {
        //                result = true;
        //            }
        //            else
        //            {
        //                errorMessage = this.GetResourceManger().GetString("TT_INVALID_OBJECT");
        //            }
        //        }
        //        else if (false)//(obj is TxRoboticViaLocationOperation || obj is TxGenericRoboticLocationOperation || obj is TxRoboticSeamLocationOperation)
        //        {
        //            result = true;
        //        }
        //        else if (obj is ITxWeldOperation || obj is TxWeldPoint || obj is TxGenericRoboticOperation || obj is ITxContinuousOperation || obj is TxRoboticSeamOperation || obj is TxSeamMfgFeature)
        //        {
        //            result = true;
        //            //errorMessage = string.Empty;
        //        }
        //        else if (obj is TxCompoundOperation)
        //        {
        //            result = true;
        //            //errorMessage = string.Empty;
        //        }
        //        else
        //        {
        //            errorMessage = this.GetResourceManger().GetString("TT_INVALID_OBJECT");
        //        }
        //    }
        //    else
        //    {
        //        errorMessage = this.GetResourceManger().GetString("TT_INVALID_OBJECT");
        //    }
        //    return result;
        //}

        //public bool IsValidText(string text, out string errorMessage)
        //{
        //    ITxObject @object = this.GetObject(text);
        //    return this.IsValidObject(@object, out errorMessage);
        //}

        //private ResourceManager GetResourceManger()
        //{
        //    return new ResourceManager("AutoJTTecnomatix._14.Automatic_Robot_Movement.Resources.AutoApproachAngleStringTable", base.GetType().Assembly);
        //}

        private bool m_simulationMode;
    }

}
