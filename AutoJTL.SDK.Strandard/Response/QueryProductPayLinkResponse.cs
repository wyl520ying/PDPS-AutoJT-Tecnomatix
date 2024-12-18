using System;
using System.Collections.Generic;
using System.Text;

namespace AutoJTL.SDK.Strandard.Response
{
    public class QueryProductPayLinkResponse : CommonResponse<QueryProductPayLinkResult>
    {

    }

    public class QueryProductPayLinkResult
    {
        public string QrCode { get; set; }
        public string Body { get; set; }
    }
}
