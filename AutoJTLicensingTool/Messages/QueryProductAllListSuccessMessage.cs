using System.Collections.Generic;
using System.Data;

namespace AutoJTLicensingTool.Messages
{
    public class QueryProductAllListSuccessMessage
    {
        public List<string> ProductNameList { get; set; }
        public DataTable GridDataSource { get; set; }
    }
}
