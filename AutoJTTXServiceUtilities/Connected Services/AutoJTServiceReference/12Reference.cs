





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "GetCurrentMachineLoginCodeResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class GetCurrentMachineLoginCodeResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string GetCurrentMachineLoginCodeResult;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string openid;

    public GetCurrentMachineLoginCodeResponse()
    {
    }

    public GetCurrentMachineLoginCodeResponse(
      string GetCurrentMachineLoginCodeResult,
      string openid)
    {
      this.GetCurrentMachineLoginCodeResult = GetCurrentMachineLoginCodeResult;
      this.openid = openid;
    }
  }
}
