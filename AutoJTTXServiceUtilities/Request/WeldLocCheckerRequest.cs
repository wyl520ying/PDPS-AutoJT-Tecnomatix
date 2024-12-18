using AutoJTL.SDK.Strandard;
using AutoJTTXServiceUtilities.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoJTTXServiceUtilities.Request
{
    //请求类, 包括请求的形参和地址
    public class WeldLocCheckerRequest : IRequest<WeldLocCheckerResponse>
    {
        public string Pairs { get; set; }

        public string GetMethod()
        {
            return "Post";
        }

        public IDictionary<string, object> GetParams()
        {
            return new Dictionary<string, object>()
            {
                { "Pairs", Pairs },
            };
        }

        public string GetUrl()
        {
            return "/fun/WeldLocChecker4";
        }
    }
}