





using System.CodeDom.Compiler;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "AddinTx_Project_Model_Users_MCM", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class AddinTx_Project_Model_Users_MCMRequest
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public DataTable readDataTable;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string SqlTableName;

    public AddinTx_Project_Model_Users_MCMRequest()
    {
    }

    public AddinTx_Project_Model_Users_MCMRequest(DataTable readDataTable, string SqlTableName)
    {
      this.readDataTable = readDataTable;
      this.SqlTableName = SqlTableName;
    }
  }
}
