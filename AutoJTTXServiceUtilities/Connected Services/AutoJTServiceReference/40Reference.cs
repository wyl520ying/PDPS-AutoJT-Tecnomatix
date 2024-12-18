





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "CreateTx_Project_MCM_newResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class CreateTx_Project_MCM_newResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public bool CreateTx_Project_MCM_newResult;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string result;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public string error;

    public CreateTx_Project_MCM_newResponse()
    {
    }

    public CreateTx_Project_MCM_newResponse(
      bool CreateTx_Project_MCM_newResult,
      string result,
      string error)
    {
      this.CreateTx_Project_MCM_newResult = CreateTx_Project_MCM_newResult;
      this.result = result;
      this.error = error;
    }
  }
}
