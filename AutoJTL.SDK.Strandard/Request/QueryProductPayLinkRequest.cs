using AutoJTL.SDK.Strandard.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoJTL.SDK.Strandard.Request
{
    public class QueryProductPayLinkRequest : IRequest<QueryProductPayLinkResponse>
    {
        public string LoginId { get; set; }

        public string ProductId { get; set; }

        public string ProductPermissionId { get; set; }
        public string Money { get; set; }

        public string GetMethod()
        {
            return "Get";
        }

        public IDictionary<string, object> GetParams()
        {
            return new Dictionary<string, object>()
            {
                { "LoginId",LoginId},
                { "ProductId",ProductId},
                { "ProductPermissionId",ProductPermissionId},
                { "Money",Money}
            };
        }

        public string GetUrl()
        {
            return "/product/paylink";
        }
    }
}
