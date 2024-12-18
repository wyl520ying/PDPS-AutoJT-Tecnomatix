using AutoJTL.SDK.Strandard.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoJTL.SDK.Strandard.Request
{
    public class OfficialSubscribeLinkReqeust : IRequest<OfficialSubscribeLinkResponse>
    {
        public string LoginId { get; set; }

        public string GetMethod()
        {
            return "Get";
        }

        public IDictionary<string, object> GetParams()
        {
            return new Dictionary<string, object>()
            {
                { "LoginId",LoginId},
            };
        }

        public string GetUrl()
        {
            return "/official/subscribe/link";
        }
    }
}
