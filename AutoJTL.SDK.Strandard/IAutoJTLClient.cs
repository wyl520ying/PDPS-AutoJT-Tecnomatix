using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AutoJTL.SDK.Strandard
{
    public interface IAutoJTLClient
    {
        /// <summary>
        /// 执行请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request">请求对象</param>
        /// <returns></returns>
        Task<TResponse> ExecuteAsync<TResponse>(IRequest<TResponse> request) where TResponse : CommonResponse, new();
    }
}
