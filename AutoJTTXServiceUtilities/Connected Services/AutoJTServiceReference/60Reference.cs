





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "CreateAndBulkInsertUserMCMData_MCMResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class CreateAndBulkInsertUserMCMData_MCMResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public bool CreateAndBulkInsertUserMCMData_MCMResult;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string infos;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public int irows;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 3)]
    public int icount;

    public CreateAndBulkInsertUserMCMData_MCMResponse()
    {
    }

    public CreateAndBulkInsertUserMCMData_MCMResponse(
      bool CreateAndBulkInsertUserMCMData_MCMResult,
      string infos,
      int irows,
      int icount)
    {
      this.CreateAndBulkInsertUserMCMData_MCMResult = CreateAndBulkInsertUserMCMData_MCMResult;
      this.infos = infos;
      this.irows = irows;
      this.icount = icount;
    }
  }
}
