using System;
using System.Collections.Generic;
using System.Text;

namespace AutoJTL.SDK.Strandard
{
    public class UserCanAutoLoginRequest : IRequest<UserCanAutoLoginResponse>
    {
        public string LoginId { get; set; }

        public string DeviceId { get; set; }

        public string Category { get; set; }

        public bool IsInternal { get; set; }

        public string Userlnfos { get; set; }

        public string ClientVersion { get; set; }

        public string SoftHostVersion { get; set; }

        public string GetMethod()
        {
            return "Get";
        }

        public IDictionary<string, object> GetParams()
        {
            return new Dictionary<string, object>()
            {
                { "LoginId",LoginId},
                { "DeviceId",DeviceId},
                { "Category",Category},
                { "IsInternal",IsInternal},
                { "Userlnfos",Userlnfos},
                { "ClientVersion",ClientVersion},
                { "SoftHostVersion",SoftHostVersion}
            };
        }

        public string GetUrl()
        {
            return "/user/canAutoLogin";
        }
    }
}
