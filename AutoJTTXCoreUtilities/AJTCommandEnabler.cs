using Tecnomatix.Engineering;

namespace AutoJTTXCoreUtilities
{
    public class AJTCommandEnabler : ITxCommandEnabler
    {
        private bool m_checkCommandOpened = false;

        public AJTCommandEnabler(string moduleStr)
        {
            try
            {
                if (GlobalClass.EditionAbilityModules != null && GlobalClass.EditionAbilityModules.Count > 0 && GlobalClass.EditionAbilityModules.Contains(moduleStr))
                {
                    this.m_checkCommandOpened = true;
                }
                else
                {
                    this.m_checkCommandOpened = false;
                }
            }
            catch
            {
                this.m_checkCommandOpened = false;
            }
        }
        public virtual bool Enable
        {
            get
            {
                return true;//this.m_checkCommandOpened;
            }
        }
    }
}
