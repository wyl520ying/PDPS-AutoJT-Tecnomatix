





using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;


namespace AutoJTTXServiceUtilities.AutoJTServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(WrapperName = "GetAutoJTVersionCode2Response", WrapperNamespace = "http://tempuri.org/", IsWrapped = true)]
  public class GetAutoJTVersionCode2Response
  {
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 0)]
    public string GetAutoJTVersionCode2Result;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 1)]
    public string versionAndContents;
    [MessageBodyMember(Namespace = "http://tempuri.org/", Order = 2)]
    public string FORCEDUPDATE;

    public GetAutoJTVersionCode2Response()
    {
    }

    public GetAutoJTVersionCode2Response(
      string GetAutoJTVersionCode2Result,
      string versionAndContents,
      string FORCEDUPDATE)
    {
      this.GetAutoJTVersionCode2Result = GetAutoJTVersionCode2Result;
      this.versionAndContents = versionAndContents;
      this.FORCEDUPDATE = FORCEDUPDATE;
    }
  }
}
