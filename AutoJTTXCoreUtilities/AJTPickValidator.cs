using System.Collections;
using System.Collections.Generic;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Ui;
using Tecnomatix.Planning;

namespace AutoJTTXCoreUtilities
{
    public class AJTPickValidator : ITxValidator
    {
        private ArrayList m_validTypes;

        private ArrayList m_invalidTypes;

        private TxTypeFilter m_TxFilter;

        private bool m_showErrMsgForPartApp = true;

        public AJTPickValidator(bool showErrPartApp = true)
        {
            this.m_showErrMsgForPartApp = showErrPartApp;
        }

        public bool IsValidObject(ITxObject obj, out string errorMessage)
        {
            errorMessage = null;
            bool flag = false;
            bool flag2 = false;
            ITxPlanningObject planningRepresantation = this.GetPlanningRepresantation(obj);
            if (planningRepresantation != null)
            {
                flag = this.DoesPassPlanningFilter(ref planningRepresantation);
                if (planningRepresantation.EngineeringRepresentation != null)
                {
                    flag2 = this.DoesPassTxFilter(obj);
                }
            }
            else
            {
                flag2 = this.DoesPassTxFilter(obj);
            }
            flag = (flag || flag2);
            if (!flag)
            {
                if (obj is TxPartAppearance && !this.m_showErrMsgForPartApp)
                {
                    errorMessage = "";
                }
                else
                {
                    //ResourceManager resourceManager = new ResourceManager("DnProcessDesignerCommands.Resource1", base.GetType().Assembly);
                    //if (resourceManager != null)
                    //{
                    if (obj is TxDevice)
                    {
                        errorMessage = "Creation of appearance is not supported for parts with kinematics.";
                    }
                    else
                    {
                        errorMessage = "Invalid object";
                    }
                    //}
                    //else
                    //{
                    //errorMessage = "Invalid object";
                    //}
                }
            }
            return flag;
        }

        public bool IsValidText(string text, out string errorMessage)
        {
            bool result = true;
            errorMessage = null;
            TxObjectList txObjectList = null;
            if (text != null)
            {
                txObjectList = TxApplication.ActiveDocument.GetObjectsByName(text);
            }
            if (txObjectList != null && txObjectList.Count > 0)
            {
                result = false;
                using (IEnumerator<ITxObject> enumerator = txObjectList.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        ITxObject obj = enumerator.Current;
                        if (this.IsValidObject(obj, out errorMessage))
                        {
                            result = true;
                            break;
                        }
                    }
                    return result;
                }
            }
            //ResourceManager resourceManager = new ResourceManager("DnProcessDesignerCommands.Resource1", base.GetType().Assembly);
            //if (resourceManager != null)
            //{
            errorMessage = "Invalid object";
            if (text == "")
            {
                errorMessage = "Entity name is empty";
            }
            else
            {
                errorMessage = "Invalid or not loaded object";
            }
            //}
            result = false;
            return result;
        }

        public ITxObject GetObject(string text)
        {
            ITxObject result = null;
            TxObjectList txObjectList = null;
            if (text != null)
            {
                txObjectList = TxApplication.ActiveDocument.GetObjectsByName(text);
            }
            if (txObjectList != null && txObjectList.Count > 0)
            {
                result = txObjectList[0];
                foreach (ITxObject txObject in txObjectList)
                {
                    string text2;
                    if (this.IsValidObject(txObject, out text2))
                    {
                        result = txObject;
                        break;
                    }
                }
            }
            return result;
        }

        public string GetText(ITxObject obj)
        {
            ITxPlanningObject planningRepresantation = this.GetPlanningRepresantation(obj);
            string name;
            if (planningRepresantation != null)
            {
                name = planningRepresantation.Name;
            }
            else
            {
                name = obj.Name;
            }
            return name;
        }

        public void SetTxFilter(TxTypeFilter invalidTypes)
        {
            this.m_TxFilter = invalidTypes;
        }

        internal void SetValidTypes(ArrayList validTypes)
        {
            this.m_validTypes = validTypes;
        }

        internal void SetInalidTypes(ArrayList invalidTypes)
        {
            this.m_invalidTypes = invalidTypes;
        }

        private ITxPlanningObject GetPlanningRepresantation(ITxObject obj)
        {
            ITxPlanningObject txPlanningObject = obj as ITxPlanningObject;
            if (txPlanningObject == null)
            {
                ITxProcessModelObject txProcessModelObject = obj as ITxProcessModelObject;
                if (txProcessModelObject != null)
                {
                    txPlanningObject = txProcessModelObject.PlanningRepresentation;
                }
            }
            return txPlanningObject;
        }

        private bool DoesPassPlanningFilter(ref ITxPlanningObject planObject)
        {
            ITxObject engineeringRepresentation = planObject.EngineeringRepresentation;
            bool flag = false;
            if (this.m_validTypes != null && this.m_validTypes.Count > 0)
            {
                foreach (object obj in this.m_validTypes)
                {
                    string otherType = (string)obj;
                    if (planObject.IsDerivedFromPlanningType(otherType))
                    {
                        flag = true;
                        break;
                    }
                }
            }
            bool flag2 = false;
            if (this.m_invalidTypes != null && this.m_invalidTypes.Count > 0)
            {
                foreach (object obj2 in this.m_invalidTypes)
                {
                    string otherType2 = (string)obj2;
                    if (planObject.IsDerivedFromPlanningType(otherType2))
                    {
                        flag2 = true;
                        break;
                    }
                }
            }
            return flag && !flag2;
        }

        private bool DoesPassTxFilter(ITxObject obj)
        {
            bool flag = false;
            if (this.m_TxFilter != null)
            {
                flag = this.m_TxFilter.DoesPassFilter(obj);
            }
            if (!flag)
            {
                ITxPlanningObject planningRepresantation = this.GetPlanningRepresantation(obj);
                if (planningRepresantation != null)
                {
                    flag = this.m_TxFilter.DoesPassFilter(planningRepresantation);
                }
            }
            return flag;
        }
    }
}
