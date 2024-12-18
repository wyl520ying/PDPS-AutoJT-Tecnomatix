





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "GetAutoJTVersionCode", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class GetAutoJTVersionCodeRequest
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string userInfos;

    public GetAutoJTVersionCodeRequest()
    {
    }

    public GetAutoJTVersionCodeRequest(string userInfos) => this.userInfos = userInfos;
  }
}
