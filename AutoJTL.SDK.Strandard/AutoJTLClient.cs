using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace AutoJTL.SDK.Strandard
{
    public class AutoJTLClient : IAutoJTLClient
    {
        private volatile static HttpClient httpClient;
        private readonly static object httpClientLock = new object();
        private static HttpClient SingletonHttpClient()
        {
            if (httpClient == null)
            {
                lock (httpClientLock)
                {
                    if (httpClient == null)
                    {
                        httpClient = new HttpClient();
                        httpClient.Timeout = TimeSpan.FromSeconds(10);
                    }
                }
            }
            return httpClient;
        }

        private readonly AutoJTLOptions _options;
        public AutoJTLClient(AutoJTLOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// 执行请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request">请求对象</param>
        /// <returns></returns>
        public Task<TResponse> ExecuteAsync<TResponse>(IRequest<TResponse> request) where TResponse : CommonResponse, new()
        {
            TResponse result = default(TResponse);

            var url = $"{_options.Endpoint}{request.GetUrl()}";


            HttpWebRequest httpWebRequest = null;

            var sendParams = request.GetParams();
            if (request.GetMethod().Equals("Get", StringComparison.OrdinalIgnoreCase))
            {
                if (!url.Contains("?"))
                {
                    url += "?";
                }
                if (sendParams != null)
                    url += string.Join("&", request.GetParams().Select(x => $"{x.Key}={x.Value?.ToString()}"));

                httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                httpWebRequest.Method = "GET";
            }
            else
            {
                string param = string.Join("&", request.GetParams().Select(x => $"{x.Key}={x.Value}"));
                byte[] postBytes = Encoding.UTF8.GetBytes(param);

                httpWebRequest.ContentType = "application/x-www-form-urlencoded;charset=utf8";
                httpWebRequest.ContentLength = postBytes.Length;
                using (Stream reqStream = httpWebRequest.GetRequestStream())
                {
                    reqStream.Write(postBytes, 0, postBytes.Length);
                }
            }

            using (var response = httpWebRequest.GetResponse())
            {
                using (var streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    var getString = streamReader.ReadToEnd();
                    if (!string.IsNullOrEmpty(getString))
                    {
                        result = JsonConvert.DeserializeObject<TResponse>(getString);
                    }
                    //result = JsonSerializer.Deserialize<TResponse>(getString, JsonSerializerOptions.Default);
                }
            }
            return Task.FromResult(result);
        }
    }
}
