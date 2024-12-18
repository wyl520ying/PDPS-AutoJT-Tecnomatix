using System;
using System.Collections.Generic;
using System.Text;

namespace AutoJTL.SDK.Strandard.Request
{
    public class UserLoginRequest : IRequest<UserCanAutoLoginResponse>
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
                { "LoginId",LoginId}
            };
        }

        public string GetUrl()
        {
            return "/user/login";
        }
    }
}
