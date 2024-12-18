using AutoJTL.SDK.Strandard;
using AutoJTTXServiceUtilities.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoJTTXServiceUtilities.Request
{
    public class PopupDiaRequest : IRequest<InviteResponse>
    {
        public string Json { get; set; }

        public string GetMethod()
        {
            return "Post";
        }

        public IDictionary<string, object> GetParams()
        {
            return new Dictionary<string, object>()
            {
                { "Json", Json },
            };
        }

        public string GetUrl()
        {
            return "/user/PopupDia";
        }
    }
}
