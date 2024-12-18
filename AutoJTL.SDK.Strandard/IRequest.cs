using System.Collections.Generic;

namespace AutoJTL.SDK.Strandard
{
    /// <summary>
    /// 请求对象接口
    /// </summary>
    public interface IRequest<T> where T : CommonResponse
    {
        /// <summary>
        /// 获取请求参数
        /// </summary>
        /// <returns></returns>
        IDictionary<string, object> GetParams();

        /// <summary>
        /// 获取请求方法
        /// </summary>
        /// <returns></returns>
        string GetMethod();

        /// <summary>
        /// 获取请求地址
        /// </summary>
        /// <returns></returns>
        string GetUrl();
    }
}
