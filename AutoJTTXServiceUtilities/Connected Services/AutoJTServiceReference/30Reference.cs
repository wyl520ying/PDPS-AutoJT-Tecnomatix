





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "GetAutoJTTecnomatixVersionCode3Response", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class GetAutoJTTecnomatixVersionCode3Response
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string GetAutoJTTecnomatixVersionCode3Result;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string versionAndContents;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public string FORCEDUPDATE;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 3)]
    public string downloadLink;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 4)]
    public string downloadLink2;

    public GetAutoJTTecnomatixVersionCode3Response()
    {
    }

    public GetAutoJTTecnomatixVersionCode3Response(
      string GetAutoJTTecnomatixVersionCode3Result,
      string versionAndContents,
      string FORCEDUPDATE,
      string downloadLink,
      string downloadLink2)
    {
      this.GetAutoJTTecnomatixVersionCode3Result = GetAutoJTTecnomatixVersionCode3Result;
      this.versionAndContents = versionAndContents;
      this.FORCEDUPDATE = FORCEDUPDATE;
      this.downloadLink = downloadLink;
      this.downloadLink2 = downloadLink2;
    }
  }
}
