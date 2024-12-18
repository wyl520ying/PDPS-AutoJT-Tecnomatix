





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "GetAutoJTVersionCodeResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class GetAutoJTVersionCodeResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string GetAutoJTVersionCodeResult;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string versionAndContents;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public string FORCEDUPDATE;

    public GetAutoJTVersionCodeResponse()
    {
    }

    public GetAutoJTVersionCodeResponse(
      string GetAutoJTVersionCodeResult,
      string versionAndContents,
      string FORCEDUPDATE)
    {
      this.GetAutoJTVersionCodeResult = GetAutoJTVersionCodeResult;
      this.versionAndContents = versionAndContents;
      this.FORCEDUPDATE = FORCEDUPDATE;
    }
  }
}
