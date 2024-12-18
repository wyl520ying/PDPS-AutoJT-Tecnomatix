





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "GetAutoJTVersionCodeExternal3Response", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class GetAutoJTVersionCodeExternal3Response
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string GetAutoJTVersionCodeExternal3Result;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string versionAndContents;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public string FORCEDUPDATE;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 3)]
    public string downloadLink;

    public GetAutoJTVersionCodeExternal3Response()
    {
    }

    public GetAutoJTVersionCodeExternal3Response(
      string GetAutoJTVersionCodeExternal3Result,
      string versionAndContents,
      string FORCEDUPDATE,
      string downloadLink)
    {
      this.GetAutoJTVersionCodeExternal3Result = GetAutoJTVersionCodeExternal3Result;
      this.versionAndContents = versionAndContents;
      this.FORCEDUPDATE = FORCEDUPDATE;
      this.downloadLink = downloadLink;
    }
  }
}
