using AutoJTMathUtilities;

namespace AutoJTTXUtilities.CATIAHandling
{
    /// <summary>
    /// 所有part Prototype
    /// </summary>
    public class IPrototype
    {
        string m_name;
        string m_nameWithoutExtension;
        AJTMatrix m_matrix;

        /// <summary>
        /// 文件名包含后缀
        /// </summary>
        public string m_Name { get => m_name; set => m_name = value; }

        /// <summary>
        /// 文件名不包含后缀
        /// </summary>
        public string m_NameWithoutExtension { get => m_nameWithoutExtension; set => m_nameWithoutExtension = value; }

        /// <summary>
        /// 位姿
        /// </summary>
        public AJTMatrix m_Matrix { get => m_matrix; set => m_matrix = value; }

    }

}
