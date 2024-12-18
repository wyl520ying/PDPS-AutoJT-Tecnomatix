





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "GetCurrentMachineLoginCode", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class GetCurrentMachineLoginCodeRequest
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string clientInfos;

    public GetCurrentMachineLoginCodeRequest()
    {
    }

    public GetCurrentMachineLoginCodeRequest(string clientInfos) => this.clientInfos = clientInfos;
  }
}
