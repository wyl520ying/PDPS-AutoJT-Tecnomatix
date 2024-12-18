using System;
using System.Collections.Generic;
using System.Text;

namespace AutoJTL.SDK.Strandard.Response
{
    public class ProductAllListResponse : CommonResponse<ProductAllListResult>
    {

    }

    public class ProductAllListResult
    {
        public List<Product> Products { get; set; }

        public List<ProductPermissions> Permissions { get; set; }

        public List<ProductUserInfo> UserInfos { get; set; }
    }
}
