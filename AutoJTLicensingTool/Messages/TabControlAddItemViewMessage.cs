using AutoJTL.SDK.Strandard.Response;
using AutoJTLicensingTool.PageModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AutoJTLicensingTool.Messages
{
    public class TabControlAddItemViewMessage
    {       
        public List<string> ProductNameList { get; set; }
        public List<ProductItemShowModel> ProductShowModels { get; set; }
    }
}
