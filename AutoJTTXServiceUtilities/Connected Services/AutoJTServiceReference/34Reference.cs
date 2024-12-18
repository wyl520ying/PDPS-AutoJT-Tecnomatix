





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "GetAutoJTTecnomatixVersionCodeExternalResponse", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class GetAutoJTTecnomatixVersionCodeExternalResponse
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string GetAutoJTTecnomatixVersionCodeExternalResult;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string versionAndContents;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public string FORCEDUPDATE;

    public GetAutoJTTecnomatixVersionCodeExternalResponse()
    {
    }

    public GetAutoJTTecnomatixVersionCodeExternalResponse(
      string GetAutoJTTecnomatixVersionCodeExternalResult,
      string versionAndContents,
      string FORCEDUPDATE)
    {
      this.GetAutoJTTecnomatixVersionCodeExternalResult = GetAutoJTTecnomatixVersionCodeExternalResult;
      this.versionAndContents = versionAndContents;
      this.FORCEDUPDATE = FORCEDUPDATE;
    }
  }
}
