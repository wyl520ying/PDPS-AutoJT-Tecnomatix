using System.Collections.Generic;

namespace AutoJTTXUtilities.CATIAHandling
{
    public class Compound_Part : IPrototype
    {
        List<IPrototype> m_list;

        /// <summary>
        /// 零件集合
        /// </summary>
        public List<IPrototype> m_List
        {
            get
            {
                if (m_list == null)
                {
                    m_list = new List<IPrototype>();
                }
                return m_list;
            }
            set
            {
                m_list = value;
            }
        }
    }

}
