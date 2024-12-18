





using System.CodeDom.Compiler;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "CreateAndBulkInsertUserMCMData_MCM", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class CreateAndBulkInsertUserMCMData_MCMRequest
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public DataTable dataTable1;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string tbName;

    public CreateAndBulkInsertUserMCMData_MCMRequest()
    {
    }

    public CreateAndBulkInsertUserMCMData_MCMRequest(DataTable dataTable1, string tbName)
    {
      this.dataTable1 = dataTable1;
      this.tbName = tbName;
    }
  }
}
