





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "GetAutoJTTecnomatixVersionCodeResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class GetAutoJTTecnomatixVersionCodeResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string GetAutoJTTecnomatixVersionCodeResult;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string versionAndContents;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public string FORCEDUPDATE;

    public GetAutoJTTecnomatixVersionCodeResponse()
    {
    }

    public GetAutoJTTecnomatixVersionCodeResponse(
      string GetAutoJTTecnomatixVersionCodeResult,
      string versionAndContents,
      string FORCEDUPDATE)
    {
      this.GetAutoJTTecnomatixVersionCodeResult = GetAutoJTTecnomatixVersionCodeResult;
      this.versionAndContents = versionAndContents;
      this.FORCEDUPDATE = FORCEDUPDATE;
    }
  }
}
