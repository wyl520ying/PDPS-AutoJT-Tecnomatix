using System.Collections.Generic;

namespace AutoJTL.SDK.Strandard
{
    /// <summary>
    /// 公共的返回结果对象
    /// </summary>
    public class CommonResponse
    {
        /// <summary>
        /// 获取或设置状态码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 获取或设置状态描述
        /// </summary>
        public string Msg { get; set; }
    }

    /// <summary>
    /// 泛型的响应结果对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommonResponse<T> : CommonResponse
    {
        /// <summary>
        /// 获取或设置Data对象
        /// </summary>
        public T Data { get; set; }
    }
}
