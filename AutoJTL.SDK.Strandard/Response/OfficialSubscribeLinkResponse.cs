using System;
using System.Collections.Generic;
using System.Text;

namespace AutoJTL.SDK.Strandard.Response
{
    public class OfficialSubscribeLinkResponse : CommonResponse<OfficialSubscribeLinkResult>
    {
    }

    public class OfficialSubscribeLinkResult
    {
        public string Ticket { get; set; }

        public string Url { get; set; }

        public int? ExpireSeconds { get; set; }
    }

}
