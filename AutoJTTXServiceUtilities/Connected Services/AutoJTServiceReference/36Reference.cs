





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "GetAutoJTTecnomatixVersionCodeExternal3Response", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class GetAutoJTTecnomatixVersionCodeExternal3Response
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string GetAutoJTTecnomatixVersionCodeExternal3Result;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string versionAndContents;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public string FORCEDUPDATE;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 3)]
    public string downloadLink;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 4)]
    public string downloadLink2;

    public GetAutoJTTecnomatixVersionCodeExternal3Response()
    {
    }

    public GetAutoJTTecnomatixVersionCodeExternal3Response(
      string GetAutoJTTecnomatixVersionCodeExternal3Result,
      string versionAndContents,
      string FORCEDUPDATE,
      string downloadLink,
      string downloadLink2)
    {
      this.GetAutoJTTecnomatixVersionCodeExternal3Result = GetAutoJTTecnomatixVersionCodeExternal3Result;
      this.versionAndContents = versionAndContents;
      this.FORCEDUPDATE = FORCEDUPDATE;
      this.downloadLink = downloadLink;
      this.downloadLink2 = downloadLink2;
    }
  }
}
