using System.Drawing;
using Tecnomatix.Engineering;

namespace AutoJTTXCoreUtilities.AJTeMSUtils
{
    public interface IAJTComponentNodeComposite
    {
        string Name { get; }

        Image Icon { get; }

        TxPlanningTypeMetaData TypeMetaData { get; set; }

        string TypeName { get; }

        string FullPath { get; }

        bool IsAssigned { get; }
    }
}
