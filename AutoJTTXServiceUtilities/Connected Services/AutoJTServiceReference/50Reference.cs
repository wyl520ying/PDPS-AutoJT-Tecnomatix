





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "BulkInsertMCMData_MCMResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class BulkInsertMCMData_MCMResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public bool BulkInsertMCMData_MCMResult;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string infos;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public int irows;

    public BulkInsertMCMData_MCMResponse()
    {
    }

    public BulkInsertMCMData_MCMResponse(bool BulkInsertMCMData_MCMResult, string infos, int irows)
    {
      this.BulkInsertMCMData_MCMResult = BulkInsertMCMData_MCMResult;
      this.infos = infos;
      this.irows = irows;
    }
  }
}
