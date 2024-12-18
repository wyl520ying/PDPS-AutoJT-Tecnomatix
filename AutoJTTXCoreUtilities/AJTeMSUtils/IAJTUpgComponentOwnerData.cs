using EMPTYPELIBRARYLib;

namespace AutoJTTXCoreUtilities
{
    internal interface IAJTUpgComponentOwnerData
    {
        int Id { get; }

        string Caption { get; }

        string ExternalId { get; }

        EmpEnumCOState CiCoState { get; }
    }
}
