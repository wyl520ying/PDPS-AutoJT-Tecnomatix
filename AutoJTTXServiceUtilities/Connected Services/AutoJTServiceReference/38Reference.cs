





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "CreateTx_Project_MCMResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class CreateTx_Project_MCMResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public bool CreateTx_Project_MCMResult;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string result;

    public CreateTx_Project_MCMResponse()
    {
    }

    public CreateTx_Project_MCMResponse(bool CreateTx_Project_MCMResult, string result)
    {
      this.CreateTx_Project_MCMResult = CreateTx_Project_MCMResult;
      this.result = result;
    }
  }
}
