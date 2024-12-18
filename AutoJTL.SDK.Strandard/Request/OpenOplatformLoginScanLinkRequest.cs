﻿using AutoJTL.SDK.Strandard.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoJTL.SDK.Strandard
{
    public class OpenOplatformLoginScanLinkRequest : IRequest<OpenOplatformLoginScanLinkReponse>
    {
        public string LoginId { get; set; }

        public string DeviceId { get; set; }

        public string ClientVersion { get; set; }

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
                { "ClientVersion",ClientVersion}
            };
        }

        public string GetUrl()
        {
            return "/open_platform/login/scanlink";
        }
    }
}